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
                try
                {
                    parser.Parse();
                    var tree = parser.GetProgramTree();
                    var analyzer = new SemanticAnalyzer(tree);
                    analyzer.Analyze();

                    // Если дошли до сюда без ошибок, запускаем выполнение
                    var stack = new Stack(256);
                    var program = new Program(tree, stack, new MockInput(), new MockOutput());
                    program.Run();

                    Assert.Fail("Expected test to fail but it succeeded. No errors were thrown.");
                }
                catch (LexerError e)
                {
                    Assert.Pass($"Caught expected lexer error: {e.Message} at line {e.Location.StartLine}, column {e.Location.StartColumn}");
                }
                catch (ParsingError e)
                {
                    Assert.Pass($"Caught expected parser error: {e.Message}");
                }
                catch (SemanticErrors e)
                {
                    var errorMessages = string.Join("\n", e.Errors.Select(err => 
                        $"- {err.Message} at line {err.Location.StartLine}, column {err.Location.StartColumn}"));
                    Assert.Pass($"Caught expected semantic errors:\n{errorMessages}");
                }
                catch (ExecutionError e)
                {
                    var location = e.Location != null 
                        ? $" at line {e.Location.StartLine}, column {e.Location.StartColumn}"
                        : "";
                    Assert.Pass($"Caught expected runtime error: {e.Message}{location}");
                }
                catch (StackOverflowError e)
                {
                    Assert.Pass($"Caught expected stack overflow error: {e.Message}");
                }
                catch (Exception e)
                {
                    Assert.Fail($"Unexpected error type: {e.GetType().Name}\nMessage: {e.Message}");
                }
            }
        }

        private class MockInput : Input
        {
            public string? Next() => null;
        }

        private class MockOutput : Output
        {
            public void Write(string value) { }
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

        // High order - Positive Tests
        [TestCase("../../../Tests/higher_order/partial_application.d", true)]
        [TestCase("../../../Tests/higher_order/array_operations.d", true)]
        [TestCase("../../../Tests/higher_order/collection_operations.d", true)]
        [TestCase("../../../Tests/higher_order/combinators.d", true)]
        [TestCase("../../../Tests/higher_order/currying.d", true)]
        [TestCase("../../../Tests/higher_order/decorators.d", true)]
        [TestCase("../../../Tests/higher_order/function_as_argument.d", true)]
        [TestCase("../../../Tests/higher_order/function_as_value.d", true)]
        [TestCase("../../../Tests/higher_order/function_composition.d", true)]
        [TestCase("../../../Tests/higher_order/multiple_args.d", true)]
        
        // Runtime Error Tests - These should parse successfully but fail at runtime
        [TestCase("../../../Tests/errors/array_bounds_check.d", false)]
        [TestCase("../../../Tests/errors/array_out_of_bounds.d", false)]
        [TestCase("../../../Tests/errors/func_decl_and_invocation_error.d", false)]
        [TestCase("../../../Tests/arithmetic/arithmetic_operations_division_by_zero.d", false)]

        // Syntax Errors
        [TestCase("../../../Tests/errors/syntax/missing_semicolon.d", false)]
        [TestCase("../../../Tests/errors/syntax/invalid_function.d", false)]
        [TestCase("../../../Tests/errors/syntax/invalid_structure.d", false)]
        [TestCase("../../../Tests/errors/syntax/unmatched_brackets.d", false)]

        // Semantic Errors
        [TestCase("../../../Tests/errors/semantic/undefined_variable.d", false)]
        [TestCase("../../../Tests/errors/semantic/duplicate_variable.d", false)]
        [TestCase("../../../Tests/errors/semantic/type_mismatch.d", false)]
        [TestCase("../../../Tests/errors/semantic/invalid_function_call.d", false)]

        // Runtime Errors
        [TestCase("../../../Tests/errors/runtime/division_by_zero.d", false)]
        [TestCase("../../../Tests/errors/runtime/array_bounds.d", false)]
        [TestCase("../../../Tests/errors/runtime/invalid_tuple_access.d", false)]
        [TestCase("../../../Tests/errors/runtime/stack_overflow.d", false)]

        // Lexical Errors
        [TestCase("../../../Tests/errors/lexical/invalid_character.d", false)]
        [TestCase("../../../Tests/errors/lexical/unclosed_string.d", false)]
        [TestCase("../../../Tests/errors/lexical/invalid_number.d", false)]

        public void Test_Parser(string filePath, bool shouldSucceed)
        {
            string input = ReadFileContent(filePath);
            RunFullTest(input, shouldSucceed);
        }
    }
}