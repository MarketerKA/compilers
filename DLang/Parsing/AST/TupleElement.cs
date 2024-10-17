using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class TupleElement : Locationed
    {
        public readonly string? Identifier;
        public readonly Expression Expression;

        public TupleElement(LexLocation location, string? identifier, Expression expression) : base(location)
        {
            Identifier = identifier;
            Expression = expression;
        }
    }

}
