namespace DLang.Parsing.AST
{

    internal class Reference
    {
        public readonly string? Identifier;
        public readonly Reference? Subreference;
        public readonly Expression? Expression;
        public readonly ExpressionList? Expressions;

        public Reference(string? identifier)
        {
            Identifier = identifier;
        }

        public Reference(Reference reference, Expression expression)
        {
            Subreference = reference;
            Expression = expression;
        }

        public Reference(Reference reference, ExpressionList expressionList)
        {
            Subreference = reference;
            Expressions = expressionList;
        }

        public Reference(Reference reference, string? identifier)
        {
            Identifier = identifier;
            Subreference = reference;
        }
    }

}
