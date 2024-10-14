namespace DLang.Parsing.AST
{

    internal class Loop
    {
        public readonly Expression? Expression;
        public readonly Range? Range;
        public readonly string? Identifier;
        public readonly StatementList Statements;

        public Loop(Expression expression, StatementList statements)
        {
            Expression = expression;
            Statements = statements;
        }

        public Loop(string? identifier, Range range, StatementList statements)
        {
            Identifier = identifier;
            Range = range;
            Statements = statements;
        }
    }

}
