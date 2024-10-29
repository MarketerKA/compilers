using System.IO;
using NUnit.Framework;
using DLang.Lexing;
using DLang.Parsing;

namespace DLang.Tests
{
    public class ParserTests
    {
        private string ReadFileContent(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to read file at {filePath}: {ex.Message}");
                return string.Empty;
            }
        }

        [Test]
        [TestCase("../../../Tests/var_decl_with_init.d", true)]
        [TestCase("../../../Tests/dynamic_type_reassign.d", true)]
        [TestCase("../../../Tests/arithmetic_operations.d", true)]
        [TestCase("../../../Tests/implicit_type_conversion.d", true)]
        [TestCase("../../../Tests/cond_stmt_without_else.d", true)]
        [TestCase("../../../Tests/cond_stmt_with_else.d", true)]
        [TestCase("../../../Tests/while_loop.d", true)]
        [TestCase("../../../Tests/ranged_for_loop.d", true)]
        [TestCase("../../../Tests/array_declaration_and_access.d", true)]
        [TestCase("../../../Tests/tuple_decl_and_access.d", true)]
        [TestCase("../../../Tests/scopes_and_shadowing.d", true)]
        [TestCase("../../../Tests/is_operation.d", true)]
        [TestCase("../../../Tests/func_as_first_class_citizen.d", true)]
        [TestCase("../../../Tests/tuple_of_tuples.d", true)]
        [TestCase("../../../Tests/func_with_tuple_arg.d", true)]
        [TestCase("../../../Tests/func_returns_func.d", true)]
        [TestCase("../../../Tests/func_as_argument.d", true)]
        [TestCase("../../../Tests/tuple_including_func.d", true)]

        public void Test_Parser(string filePath, bool shouldSucceed)
        {
            string input = ReadFileContent(filePath);
            Lexer lexer = new Lexer(input);
            Scanner scanner = new Scanner(lexer, false);
            Parser parser = new Parser(scanner);

            if (shouldSucceed)
            {
                bool result = parser.Parse();
                Assert.IsTrue(result, $"Parsing failed for {filePath} when it should have succeeded.");
            }
            else
            {
                Assert.Throws<ParsingError>(() => parser.Parse(), $"Expected parsing to fail for {filePath}, but it succeeded.");
            }
        }
    }
}
