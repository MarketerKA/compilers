namespace DLang.Parsing.AST
{

    internal class ArrayElements
    {
        public readonly List<Expression> Elements;

        public ArrayElements() { Elements = []; }

        public ArrayElements(Expression expression) { Elements = [expression]; }

        public void Add(Expression expression) { Elements.Add(expression); }
    }

}
