namespace DLang.Parsing.AST
{

    internal class Tuple
    {
        public readonly TupleElements Elements;

        public Tuple(TupleElements elements)
        {
            Elements = elements;
        }
    }

}
