using QUT.Gppg;

namespace DLang.Parsing.AST
{
    internal class Assignment : Locationed
    {
        public readonly Reference Reference;
        public readonly Expression Expression;

        public Assignment(LexLocation location, Reference reference, Expression expression) : base(location)
        {
            Reference = reference;
            Expression = expression;
        }
    }
}
