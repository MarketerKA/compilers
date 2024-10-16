namespace DLang.Parsing.AST
{

    internal enum StatementType
    {
        Declaration,
        Assignment,
        Print,
        If,
        Loop,
        Return
    }

    internal class Statement
    {
        internal StatementType Type;
        public readonly Declaration? Declaration;
        public readonly Assignment? Assignment;
        public readonly Print? Print;
        public readonly If? If;
        public readonly Loop? Loop;
        public readonly Return? Return;

        public Statement(Declaration declaration)
        {
            Declaration = declaration;
            Type = StatementType.Declaration;
        }

        public Statement(Assignment assignment)
        {
            Assignment = assignment;
            Type = StatementType.Assignment;
        }

        public Statement(Print print)
        {
            Print = print;
            Type = StatementType.Print;
        }

        public Statement(If @if)
        {
            If = @if;
            Type = StatementType.If;
        }

        public Statement(Loop? loop)
        {
            Loop = loop;
            Type = StatementType.Loop;
        }

        public Statement(Return @return)
        {
            Return = @return;
            Type = StatementType.Return;
        }
    }

}
