using DLang.Lexing;
using DLang.Parsing; // Add if needed


namespace DLang.Tests;
public class Tests
{
    public class ParserTests
    {
        [Test]
        public void Test_Parser_Success()
        {
            // Simulate reading from a valid input file
            string input = "var tup := {a := 1, b := 2};";

            // Create the necessary components: Lexer, Scanner, Parser
            Lexer lexer = new Lexer(input);
            Scanner scanner = new Scanner(lexer, "file", true);
            Parser parser = new Parser(scanner);

            // Attempt to parse
            bool result = parser.Parse();

            // Check that parsing succeeds
            Assert.IsTrue(result, "Parsing failure occurred when it should have succeeded.");
        }

        [Test]
        public void Test_Parser_Failure()
        {
            string input = "var tup := {a := 1 b := 2};";

            Lexer lexer = new Lexer(input);
            Scanner scanner = new Scanner(lexer, "file", true);
            Parser parser = new Parser(scanner);

            bool result = parser.Parse();

            Assert.IsFalse(result, "Parsing succeeded when it should have failed.");
        }
    }
}