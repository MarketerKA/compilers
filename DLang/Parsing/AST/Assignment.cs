namespace DLang.Parsing.AST
{
    internal class Assignment
    {
        public readonly Reference Reference;
        public readonly Expression Expression;

        public Assignment(Reference reference, Expression expression)
        {
            Reference = reference;
            Expression = expression;
        }
    }
}
