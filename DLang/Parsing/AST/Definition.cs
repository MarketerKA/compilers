namespace DLang.Parsing.AST
{

    internal class Definition
    {
        public readonly string Identifier;
        public readonly Expression? Expression;

        public Definition(string identifier, Expression? expression)
        {
            Identifier = identifier;
            Expression = expression;
        }
    }

}
