using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal enum PrimaryType
    {
        Literal,
        Read,
        FunctionLiteral,
        Expression
    }

    internal class Primary : Locationed
    {
        public PrimaryType Type;
        public readonly Literal? Literal;
        public readonly ReadType? Read;
        public readonly FunctionLiteral? FunctionLiteral;
        public readonly Expression? Expression;

        public Primary(LexLocation location, Literal literal) : base(location)
        {
            Literal = literal;
            Type = PrimaryType.Literal;
        }

        public Primary(LexLocation location, ReadType read) : base(location)
        {
            Read = read;
            Type = PrimaryType.Read;
        }

        public Primary(LexLocation location, FunctionLiteral functionLiteral) : base(location)
        {
            FunctionLiteral = functionLiteral;
            Type = PrimaryType.FunctionLiteral;
        }

        public Primary(LexLocation location, Expression expression) : base(location)
        {
            Expression = expression;
            Type = PrimaryType.Expression;
        }
    }

}
