using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Term : Locationed
    {
        public readonly Unary Left;
        public readonly TermOperator? Operator;
        public readonly Unary? Right;

        public Term(LexLocation location, Unary left, TermOperator? @operator, Unary? right) : base(location)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

}
