namespace DLang.Parsing.AST
{

    internal class TupleElement
    {
        public readonly string? Identifier;
        public readonly Expression Expression;

        public TupleElement(string? identifier, Expression expression)
        {
            Identifier = identifier;
            Expression = expression;
        }
    }

}
