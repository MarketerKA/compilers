using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Tuple : Locationed
    {
        public readonly TupleElements Elements;

        public Tuple(LexLocation location, TupleElements elements) : base(location)
        {
            Elements = elements;
        }
    }

}
