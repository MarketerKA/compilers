using QUT.Gppg;

namespace DLang.Parsing.AST
{
    internal class TupleElements : Locationed
    {
        public readonly List<TupleElement> Elements;

        public TupleElements(LexLocation location) : base(location)
        { 
            Elements = []; 
        }

        public TupleElements(LexLocation location, TupleElement element) : base(location)
        {
            Elements = [element];
        }

        public void Add(TupleElement element) { Elements.Add(element); }
    }
}
