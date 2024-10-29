using DLang.Analysis;
using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Expression : Locationed
    {
        public Relation? Left { get; set; }
        public readonly ExpressionOperator? Operator;
        public readonly Relation? Right;
        public ConstexprValue? ConstexprValue { get; set; }

        public Expression(LexLocation location, Relation left, ExpressionOperator? @operator, Relation? right) : base(location)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

}
