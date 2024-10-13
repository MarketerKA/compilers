using System;
using System.IO;
using DLang.Lexer;

public class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = "test_code.d";

        string outputFilePath = "tokens.txt";

        string input = File.ReadAllText(inputFilePath);

        Lexer lexer = new Lexer(input);
        Token token;
        string tokensOutput = "";

        do
        {
            token = lexer.GetNextToken();
            tokensOutput += token.ToString() + Environment.NewLine;
        } while (token.Type != TokenType.EOF);

        File.WriteAllText(outputFilePath, tokensOutput);

        Console.WriteLine("Tokens have been written to " + outputFilePath);
    }
}