namespace DLang.Parsing.AST
{

    internal class Range
    {
        public readonly Expression Left;
        public readonly Expression Right;

        public Range(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }
    }

}
