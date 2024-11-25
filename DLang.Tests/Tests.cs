using System.IO;
using NUnit.Framework;
using DLang.Lexing;
using DLang.Parsing;
using DLang.Analysis;
using DLang.Execution;

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

        private void RunFullTest(string input, bool shouldSucceed)
        {
            var lexer = new Lexer(input);
            var scanner = new Scanner(lexer, false);
            var parser = new Parser(scanner);

            if (shouldSucceed)
            {
                Assert.DoesNotThrow(() => {
                    parser.Parse();
                    var tree = parser.GetProgramTree();
                    var analyzer = new SemanticAnalyzer(tree);
                    analyzer.Analyze();
                }, "Expected successful parsing and analysis");
            }
            else
            {
                Assert.That(() => {
                    parser.Parse();
                    var tree = parser.GetProgramTree();
                    var analyzer = new SemanticAnalyzer(tree);
                    analyzer.Analyze();
                }, Throws.Exception);
            }
        }

        [Test]
        // Arrays - Positive Tests
        [TestCase("../../../Tests/arrays/array_basic.d", true)]
        [TestCase("../../../Tests/arrays/array_empty.d", true)]
        [TestCase("../../../Tests/arrays/array_concat.d", true)]
        [TestCase("../../../Tests/arrays/array_sparse.d", true)]
        [TestCase("../../../Tests/arrays/array_mixed_types.d", true)]
        [TestCase("../../../Tests/arrays/array_nested.d", true)]
        [TestCase("../../../Tests/arrays/array_loop.d", true)]
        [TestCase("../../../Tests/arrays/array_dynamic.d", true)]
        
        // Functions - Positive Tests
        [TestCase("../../../Tests/functions/func_as_argument.d", true)]
        [TestCase("../../../Tests/functions/func_as_first_class_citizen.d", true)]
        [TestCase("../../../Tests/functions/func_returns_func.d", true)]
        [TestCase("../../../Tests/functions/func_with_tuple_arg.d", true)]

        // Tuples - Positive Tests
        [TestCase("../../../Tests/tuples/tuple_decl_and_access.d", true)]
        [TestCase("../../../Tests/tuples/tuple_including_func.d", true)]
        [TestCase("../../../Tests/tuples/tuple_of_tuples.d", true)]

        // Variables - Positive Tests
        [TestCase("../../../Tests/variables/var_decl_with_init.d", true)]
        [TestCase("../../../Tests/variables/dynamic_type_reassign.d", true)]

        // Control Flow - Positive Tests
        [TestCase("../../../Tests/control_flow/cond_stmt_with_else.d", true)]
        [TestCase("../../../Tests/control_flow/cond_stmt_without_else.d", true)]
        [TestCase("../../../Tests/control_flow/while_loop.d", true)]
        [TestCase("../../../Tests/control_flow/ranged_for_loop.d", true)]

        // Types - Positive Tests
        [TestCase("../../../Tests/types/is_operation.d", true)]
        [TestCase("../../../Tests/types/implicit_type_conversion.d", true)]

        // Arithmetic - Positive Tests
        [TestCase("../../../Tests/arithmetic/arithmetic_operations.d", true)]

        // Scopes - Positive Tests
        [TestCase("../../../Tests/scopes/scopes_and_shadowing.d", true)]

        // Runtime Error Tests - These should parse successfully but fail at runtime
        [TestCase("../../../Tests/errors/array_bounds_check.d", false)]
        [TestCase("../../../Tests/errors/array_out_of_bounds.d", false)]
        [TestCase("../../../Tests/errors/func_decl_and_invocation_error.d", false)]
        [TestCase("../../../Tests/arithmetic/arithmetic_operations_division_by_zero.d", false)]

        // Syntax Error Tests - These should fail during parsing
        [TestCase("../../../Tests/errors/syntax_error.d", false)]
        [TestCase("../../../Tests/errors/missing_semicolon.d", false)]
        [TestCase("../../../Tests/errors/invalid_function_declaration.d", false)]
        public void Test_Parser(string filePath, bool shouldSucceed)
        {
            string input = ReadFileContent(filePath);
            RunFullTest(input, shouldSucceed);
        }
    }
}