namespace DLang.Parsing.AST
{
    internal class Assignment
    {
        public readonly string Identifier;
        public readonly Expression Expression;

        public Assignment(string identifier, Expression expression)
        {
            Identifier = identifier;
            Expression = expression;
        }
    }
}
