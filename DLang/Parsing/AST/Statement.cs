using QUT.Gppg;

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

    internal class Statement : Locationed
    {
        internal StatementType Type;
        public readonly Declaration? Declaration;
        public readonly Assignment? Assignment;
        public readonly Print? Print;
        public readonly If? If;
        public readonly Loop? Loop;
        public readonly Return? Return;

        public Statement(LexLocation location, Declaration declaration) : base(location)
        {
            Declaration = declaration;
            Type = StatementType.Declaration;
        }

        public Statement(LexLocation location, Assignment assignment) : base(location)
        {
            Assignment = assignment;
            Type = StatementType.Assignment;
        }

        public Statement(LexLocation location, Print print) : base(location)
        {
            Print = print;
            Type = StatementType.Print;
        }

        public Statement(LexLocation location, If @if) : base(location)
        {
            If = @if;
            Type = StatementType.If;
        }

        public Statement(LexLocation location, Loop? loop) : base(location)
        {
            Loop = loop;
            Type = StatementType.Loop;
        }

        public Statement(LexLocation location, Return @return) : base(location)
        {
            Return = @return;
            Type = StatementType.Return;
        }
    }

}
