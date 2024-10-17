using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class ExpressionList : Locationed
    {
        public readonly List<Expression> Expressions;

        public ExpressionList(LexLocation location) : base(location) { Expressions = []; }

        public ExpressionList(LexLocation location, Expression expression) : base(location) { Expressions = [expression]; }

        public void Add(Expression expression) { Expressions.Add(expression); }
    }

}
