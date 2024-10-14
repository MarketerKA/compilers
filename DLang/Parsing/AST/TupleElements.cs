namespace DLang.Parsing.AST
{
    internal class TupleElements
    {
        public readonly List<TupleElement> Elements;

        public TupleElements() { Elements = []; }

        public TupleElements(TupleElement element)
        {
            Elements = [element];
        }

        public void Add(TupleElement element) { Elements.Add(element); }
    }
}
