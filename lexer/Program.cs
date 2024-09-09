using System;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        // Путь к файлу с тестовым кодом
        string inputFilePath = "test_code.d";

        // Путь к файлу для записи токенов
        string outputFilePath = "tokens.txt";

        // Чтение содержимого файла с тестовым кодом
        string input = File.ReadAllText(inputFilePath);

        // Создание лексера и обработка входного кода
        Lexer lexer = new Lexer(input);
        Token token;
        string tokensOutput = "";

        do
        {
            token = lexer.GetNextToken();
            tokensOutput += token.ToString() + Environment.NewLine;
        } while (token.Type != TokenType.EOF);

        // Запись токенов в файл
        File.WriteAllText(outputFilePath, tokensOutput);

        Console.WriteLine("Tokens have been written to " + outputFilePath);
    }
}