using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class ArrayElements : Locationed
    {
        public readonly List<Expression> Elements;

        public ArrayElements(LexLocation location) : base(location) { Elements = []; }

        public ArrayElements(LexLocation location, Expression expression) : base(location) 
        { 
            Elements = [expression]; 
        }

        public void Add(Expression expression) { Elements.Add(expression); }
    }

}
