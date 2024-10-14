namespace DLang.Parsing.AST
{

    internal class Factor
    {
        public readonly Term Left;
        public readonly FactorOperator? Operator;
        public readonly Term Right;

        public Factor(Term left, FactorOperator? @operator, Term right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

}
