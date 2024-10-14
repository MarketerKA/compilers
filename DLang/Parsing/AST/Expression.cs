namespace DLang.Parsing.AST
{

    internal class Expression
    {
        public readonly Relation Left;
        public readonly ExpressionOperator? Operator;
        public readonly Relation? Right;

        public Expression(Relation left, ExpressionOperator? @operator, Relation? right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

}
