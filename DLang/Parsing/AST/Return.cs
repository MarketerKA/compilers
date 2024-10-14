namespace DLang.Parsing.AST
{
    internal class Return
    {
        public readonly Expression? Expression;

        public Return(Expression? expression)
        {
            Expression = expression;
        }
    }
}
