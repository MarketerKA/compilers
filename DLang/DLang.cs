using System.Text;
using DLang.Analysis;
using DLang.Execution;
using DLang.Lexing;
using DLang.Parsing;
using DLang.Parsing.AST;
using QUT.Gppg;
using CommandLine;
using System.Text.Json;
using System.Globalization;

namespace DLang
{

    public class DLang
    {
        private static string ConstructErrorLocation(string filename, LexLocation location)
        {
            return $"{filename}:{location.StartLine}:{location.StartColumn}: error: ";
        }

        private static string ConstructExecutionError(string filename, ExecutionError e)
        {
            StringBuilder sb = new();
            sb.Append($"{filename}:");
            if (e.Location != null)
            {
                sb.Append($"{e.Location.StartLine}:{e.Location.StartColumn}:");
            }
            sb.Append($" runtime error: {e.Message}");

            return sb.ToString();
        }

        private static string ConstructExecutionError(string filename, string text)
        {
            return $"{filename}: runtime error: {text}";
        }

        public static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            CommandLine.Parser.Default.ParseArguments<Args>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleParseError);
        }

        static void Run(Args args)
        {
            if (args.StackSize < 64 || args.StackSize > 512)
            {
                Console.Error.WriteLine($"error: stack size out of bounds [64; 512] ({args.StackSize})");
                System.Environment.Exit(1);
            }

            string input = "";

            try
            {
                input = File.ReadAllText(args.File);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"error: failed to read file: {e.Message}");
                System.Environment.Exit(1);
            }

            string filename = Path.GetFileName(args.File);

            Lexer lexer = new(input);
            Scanner scanner = new(lexer, args.DisplayLexerOutput);
            Parsing.Parser parser = new(scanner);

            try
            {
                parser.Parse();
            }
            catch (ParsingError e)
            {
                Console.Error.WriteLine(ConstructErrorLocation(filename, scanner.yylloc) + e.Message);
                System.Environment.Exit(1);
            }
            catch (LexerError e)
            {
                Console.Error.WriteLine(ConstructErrorLocation(filename, e.Location) + e.Message);
                System.Environment.Exit(1);
            }

            ProgramTree programTree = parser.GetProgramTree();

            try
            {
                SemanticAnalyzer semanticAnalyzer = new(programTree);
                semanticAnalyzer.Analyze();
            }
            catch (SemanticErrors e)
            {
                foreach (var error in e.Errors)
                {
                    Console.Error.WriteLine(ConstructErrorLocation(filename, error.Location) + error.Message);
                }

                System.Environment.Exit(1);
            }

            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            if (args.DisplayAST)
            {
                string astJson = JsonSerializer.Serialize(programTree, jsonOptions);
                Console.WriteLine(astJson);
            }

            if (!args.DisableOptimizations)
            {
                try
                {
                    Optimizer optimizer = new(programTree);
                    optimizer.Optimize();
                }
                catch (OptimizerError e)
                {
                    Console.Error.WriteLine(ConstructErrorLocation(filename, e.Location) + e.Message);
                    System.Environment.Exit(1);
                }

                if (args.DisplayOptimizedAST)
                {
                    string optimizedAstJson = JsonSerializer.Serialize(programTree, jsonOptions);
                    Console.WriteLine(optimizedAstJson);
                }
            }

            try
            {
                Stack stack = new(args.StackSize);
                Program program = new(programTree, stack, new ConsoleInput(), new ConsoleOutput());
                program.Run();
            }
            catch (ExecutionError e)
            {
                Console.Error.WriteLine(ConstructExecutionError(filename, e));
                System.Environment.Exit(1);
            }
            catch (StackOverflowError e)
            {
                Console.Error.WriteLine(ConstructExecutionError(filename, e.Message));
                System.Environment.Exit(1);
            }
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            System.Environment.Exit(1);
        }
    }

}