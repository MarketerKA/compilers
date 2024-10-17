using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Expression : Locationed
    {
        public readonly Relation Left;
        public readonly ExpressionOperator? Operator;
        public readonly Relation? Right;

        public Expression(LexLocation location, Relation left, ExpressionOperator? @operator, Relation? right) : base(location)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

}
