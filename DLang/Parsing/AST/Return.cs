using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Return : Locationed
    {
        public readonly Expression? Expression;

        public Return(LexLocation location, Expression? expression) : base(location)
        {
            Expression = expression;
        }
    }

}
