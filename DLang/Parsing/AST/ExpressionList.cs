namespace DLang.Parsing.AST
{

    internal class ExpressionList
    {
        public readonly List<Expression> Expressions;

        public ExpressionList() { Expressions = []; }

        public ExpressionList(Expression expression) { Expressions = [expression]; }

        public void Add(Expression expression) { Expressions.Add(expression); }
    }

}
