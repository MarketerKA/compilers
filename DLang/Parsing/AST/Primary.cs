namespace DLang.Parsing.AST
{
    internal class Primary
    {
        public readonly Literal? Literal;
        public readonly ReadType? Read;
        public readonly FunctionLiteral? FunctionLiteral;
        public readonly Expression? Expression;

        public Primary(Literal literal)
        {
            Literal = literal;
        }

        public Primary(ReadType read)
        {
            Read = read;
        }

        public Primary(FunctionLiteral functionLiteral)
        {
            FunctionLiteral = functionLiteral;
        }

        public Primary(Expression expression)
        {
            Expression = expression;
        }
    }

}
