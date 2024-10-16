namespace DLang.Parsing.AST
{

    internal enum PrimaryType
    {
        Literal,
        Read,
        FunctionLiteral,
        Expression
    }

    internal class Primary
    {
        public PrimaryType Type;
        public readonly Literal? Literal;
        public readonly ReadType? Read;
        public readonly FunctionLiteral? FunctionLiteral;
        public readonly Expression? Expression;

        public Primary(Literal literal)
        {
            Literal = literal;
            Type = PrimaryType.Literal;
        }

        public Primary(ReadType read)
        {
            Read = read;
            Type = PrimaryType.Read;
        }

        public Primary(FunctionLiteral functionLiteral)
        {
            FunctionLiteral = functionLiteral;
            Type = PrimaryType.FunctionLiteral;
        }

        public Primary(Expression expression)
        {
            Expression = expression;
            Type = PrimaryType.Expression;
        }
    }

}
