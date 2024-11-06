using System.Text;
using DLang.Analysis;
using DLang.Execution;
using DLang.Lexing;
using DLang.Parsing;
using DLang.Parsing.AST;
using QUT.Gppg;

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
            Args arguments = new(args);

            string input = "";

            try
            { 
                input = File.ReadAllText(arguments.InputFilePath); 
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to read file ${arguments.InputFilePath}: {e.Message}");
                System.Environment.Exit(1);
            }

            string filename = Path.GetFileName(arguments.InputFilePath);

            Lexer lexer = new(input);
            Scanner scanner = new(lexer);
            Parser parser = new(scanner);

            try
            {
                parser.Parse();
            }
            catch (ParsingError e) {
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

            try
            {
                Stack stack = new(256);
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
    }

}