using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Range : Locationed
    {
        public readonly Expression Left;
        public readonly Expression Right;

        public Range(LexLocation location, Expression left, Expression right) : base(location)
        {
            Left = left;
            Right = right;
        }
    }

}
