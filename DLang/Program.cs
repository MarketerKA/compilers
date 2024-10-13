namespace DLang
{

    public class Program
    {
        public static void Main(string[] args)
        {
            Args arguments = new(args);

            string input = File.ReadAllText(arguments.InputFilePath);

            Lexer.Lexer lexer = new(input);
            Lexer.Token token;
            string tokensOutput = "";

            do
            {
                token = lexer.GetNextToken();
                tokensOutput += token.ToString() + Environment.NewLine;
            } while (token.Type != Lexer.TokenType.EOF);

            Console.WriteLine(tokensOutput);
        }
    }

}