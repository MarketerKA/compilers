namespace DLang.Parsing.AST
{

    internal class Term
    {
        public readonly Unary Left;
        public readonly TermOperator? Operator;
        public readonly Unary? Right;

        public Term(Unary left, TermOperator? @operator, Unary? right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

}
