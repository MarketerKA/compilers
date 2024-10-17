using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Factor : Locationed
    {
        public readonly Term Left;
        public readonly FactorOperator? Operator;
        public readonly Term Right;

        public Factor(LexLocation location, Term left, FactorOperator? @operator, Term right) : base(location)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

}
