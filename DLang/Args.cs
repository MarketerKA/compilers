using CommandLine;
using CommandLine.Text;

namespace DLang
{

    internal class ArgsError : Exception
    {
        public ArgsError(string message) : base(message) { }

        public ArgsError(string message, Exception innerException) : base(message, innerException) { }
    }

    internal class Args
    {
        [Value(0, Hidden = true, Required = true,
            HelpText = "Input filepath")]
        public string File { get; private set; }

        [Option('s', "stack-size", Default = 256,
            HelpText = "Interpreter stack size")]
        public int StackSize { get; private set; }

        [Option('l', "display-lexer-output", Default = false,
            HelpText = "Display lexer output")]
        public bool DisplayLexerOutput { get; private set; }

        [Option('a', "display-ast", Default = false,
            HelpText = "Display AST")]
        public bool DisplayAST { get; private set; }

        [Option('d', "disable-optimizations", Default = false,
            HelpText = "Disable AST optimizations")]
        public bool DisableOptimizations { get; private set; }

        [Option('i', "display-optimized-ast", Default = false,
            HelpText = "Display optimized AST")]
        public bool DisplayOptimizedAST { get; private set; }

        public Args()
        {
            File = "";
        }

        public Args(string file)
        {
            File = file;
        }

        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return [new("Run file", new Args("<file>"))];
            }
        }
    }

}
