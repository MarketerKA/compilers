using System.Text.Json;
using DLang.Analysis;
using DLang.Lexing;
using DLang.Parsing;
using DLang.Parsing.AST;
using QUT.Gppg;

namespace DLang
{

    public class Program
    {
        private static string ConstructErrorLocation(string filename, LexLocation location)
        {
            return $"{filename}:{location.StartLine}:{location.StartColumn}: error: ";
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
            Scanner scanner = new(lexer, false);
            Parser parser = new(scanner);

            try
            {
                parser.Parse();
            }
            catch (ParsingError e) {
                Console.Error.WriteLine(ConstructErrorLocation(filename, scanner.yylloc) + e.Message);
                System.Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Unknown error: {e.Message}");
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

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            string json = JsonSerializer.Serialize(programTree, options);
            Console.WriteLine(json);
        }
    }

}