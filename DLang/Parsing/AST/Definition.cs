using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Definition : Locationed
    {
        public readonly string Identifier;
        public readonly Expression? Expression;

        public Definition(LexLocation location, string identifier, Expression? expression) : base(location)
        {
            Identifier = identifier;
            Expression = expression;
        }
    }

}
