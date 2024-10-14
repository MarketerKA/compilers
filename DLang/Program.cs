using System.Text.Json;
using DLang.Lexing;
using DLang.Parsing;
using DLang.Parsing.AST;

namespace DLang
{

    public class Program
    {
        public static void Main(string[] args)
        {
            Args arguments = new(args);

            string input = File.ReadAllText(arguments.InputFilePath);

            Lexer lexer = new(input);
            Scanner scanner = new(lexer, true);
            Parser parser = new(scanner);
            if (!parser.Parse())
            {
                Console.WriteLine("Parsing failure");
                System.Environment.Exit(1);
            }

            ProgramTree programTree = parser.GetProgramTree();

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