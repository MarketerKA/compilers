using DLang.Lexer;
using DLang;

public class Program
{
    public static void Main(string[] args)
    {
        Args arguments = new(args);


        string input = File.ReadAllText(arguments.InputFilePath);

        Lexer lexer = new Lexer(input);
        Token token;
        string tokensOutput = "";

        do
        {
            token = lexer.GetNextToken();
            tokensOutput += token.ToString() + Environment.NewLine;
        } while (token.Type != TokenType.EOF);

        Console.WriteLine(tokensOutput);
    }
}