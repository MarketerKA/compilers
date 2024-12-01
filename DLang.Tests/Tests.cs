using System.Globalization;
using System.IO;
using DLang.Analysis;
using DLang.Execution;
using DLang.Lexing;
using DLang.Parsing;
using NUnit.Framework;

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
                Assert.DoesNotThrow(
                    () =>
                    {
                        parser.Parse();
                        var tree = parser.GetProgramTree();
                        var analyzer = new SemanticAnalyzer(tree);
                        analyzer.Analyze();
                    },
                    "Expected successful parsing and analysis"
                );
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
                    Assert.Pass(
                        $"Caught expected lexer error: {e.Message} at line {e.Location.StartLine}, column {e.Location.StartColumn}"
                    );
                }
                catch (ParsingError e)
                {
                    Assert.Pass($"Caught expected parser error: {e.Message}");
                }
                catch (SemanticErrors e)
                {
                    var errorMessages = string.Join(
                        "\n",
                        e.Errors.Select(err =>
                            $"- {err.Message} at line {err.Location.StartLine}, column {err.Location.StartColumn}"
                        )
                    );
                    Assert.Pass($"Caught expected semantic errors:\n{errorMessages}");
                }
                catch (ExecutionError e)
                {
                    var location =
                        e.Location != null
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
            private Queue<string> _inputs = new();

            public void SetInputs(IEnumerable<string> inputs)
            {
                _inputs = new Queue<string>(inputs);
            }

            public string? Next() => _inputs.Count > 0 ? _inputs.Dequeue() : null;
        }

        private class MockOutput : Output
        {
            public List<string> OutputValues { get; } = new List<string>();

            public void Write(string value)
            {
                OutputValues.Add(value);
            }
        }

        private MockOutput RunAndGetOutput(string input, MockInput? mockInput = null)
        {
            var lexer = new Lexer(input);
            var scanner = new Scanner(lexer, false);
            var parser = new Parser(scanner);

            parser.Parse();
            var tree = parser.GetProgramTree();
            var analyzer = new SemanticAnalyzer(tree);
            analyzer.Analyze();

            var mockOutput = new MockOutput();
            var stack = new Stack(256);
            var program = new Program(tree, stack, mockInput ?? new MockInput(), mockOutput);
            program.Run();

            return mockOutput;
        }

    
        [Test]
        public void Test_Multiple_Inputs()
        {
            string filePath = "../../../Tests/input/multiple_inputs.d";
            string input = ReadFileContent(filePath);
            var mockInput = new MockInput();
            mockInput.SetInputs([
                "42",
                "3.14",
                "hello"
            ]);

            var mockOutput = RunAndGetOutput(input, mockInput);

            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("45.14"));
        }

        [Test]
        public void Test_Arithmetic_Operations_Result()
        {
            string filePath = "../../../Tests/arithmetic/arithmetic_operations.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.GreaterThan(0));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("22"));
        }

        [Test]
        public void Test_Array_Basic_Output()
        {
            string filePath = "../../../Tests/arrays/array_basic.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(3));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("1"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("5"));
            Assert.That(mockOutput.OutputValues[2], Is.EqualTo("42"));
        }

        [Test]
        public void Test_Array_Dynamic()
        {
            string filePath = "../../../Tests/arrays/array_dynamic.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(3));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("10"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("15"));
            Assert.That(mockOutput.OutputValues[2], Is.EqualTo("25"));
        }

        [Test]
        public void Test_Array_Concat()
        {
            string filePath = "../../../Tests/arrays/array_concat.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(3));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("1"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("4"));
            Assert.That(mockOutput.OutputValues[2], Is.EqualTo("6"));
        }

        [Test]
        public void Test_Array_Loop()
        {
            string filePath = "../../../Tests/arrays/array_loop.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(5));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("2"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("4"));
            Assert.That(mockOutput.OutputValues[2], Is.EqualTo("6"));
            Assert.That(mockOutput.OutputValues[3], Is.EqualTo("8"));
            Assert.That(mockOutput.OutputValues[4], Is.EqualTo("10"));
        }

        [Test]
        public void Test_Tuple_Access()
        {
            string filePath = "../../../Tests/tuples/tuple_decl_and_access.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(3));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("1"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("hello world"));
            Assert.That(mockOutput.OutputValues[2], Is.EqualTo("3.1415"));
        }

        [Test]
        public void Test_Implicit_Type_Conversion()
        {
            string filePath = "../../../Tests/types/implicit_type_conversion.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(1));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("16.2")); // 1 + 15.2
        }

        [Test]
        public void Test_Control_Flow_While_Loop()
        {
            string filePath = "../../../Tests/control_flow/while_loop.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(6));
            for (int i = 0; i < 6; i++)
            {
                Assert.That(mockOutput.OutputValues[i], Is.EqualTo(i.ToString()));
            }
        }

        [Test]
        public void Test_Array_Mixed_Types()
        {
            string filePath = "../../../Tests/arrays/array_mixed_types.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(5));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("1"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("2.5"));
            Assert.That(mockOutput.OutputValues[2], Is.EqualTo("three"));
            Assert.That(mockOutput.OutputValues[3], Is.EqualTo("true"));
            Assert.That(mockOutput.OutputValues[4], Is.EqualTo("11")); // function call result
        }
        [Test]
        public void Test_Dynamic_Type_Reassign()
        {
            string filePath = "../../../Tests/variables/dynamic_type_reassign.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(2));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("5"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("Hello"));
        }


        [Test]
        public void Test_Array_Sparse()
        {
            string filePath = "../../../Tests/arrays/array_sparse.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(3));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("10"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("20"));
            Assert.That(mockOutput.OutputValues[2], Is.EqualTo("30"));
        }

        [Test]
        public void Test_Array_Nested()
        {
            string filePath = "../../../Tests/arrays/array_nested.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(4));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("1"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("4"));
            Assert.That(mockOutput.OutputValues[2], Is.EqualTo("5"));
            Assert.That(mockOutput.OutputValues[3], Is.EqualTo("42"));
        }

        [Test]
        public void Test_Tuple_Including_Func()
        {
            string filePath = "../../../Tests/tuples/tuple_including_func.d";
            string input = ReadFileContent(filePath);

            var mockOutput = RunAndGetOutput(input);

            Assert.That(mockOutput.OutputValues, Has.Count.EqualTo(6));
            Assert.That(mockOutput.OutputValues[0], Is.EqualTo("1"));
            Assert.That(mockOutput.OutputValues[1], Is.EqualTo("Hello"));
            Assert.That(mockOutput.OutputValues[2], Is.EqualTo("World"));
            Assert.That(mockOutput.OutputValues[3], Is.EqualTo("4"));
            Assert.That(mockOutput.OutputValues[4], Is.EqualTo("6"));
            Assert.That(mockOutput.OutputValues[5], Is.EqualTo("8"));
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
