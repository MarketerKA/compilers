namespace DLang.Parsing.AST
{

    internal class If
    {
        public readonly Expression Expression;
        public readonly StatementList Statements;
        public readonly StatementList? Tail;

        public If(Expression expression, StatementList statements, StatementList? tail)
        {
            Expression = expression;
            Statements = statements;
            Tail = tail;
        }
    }

}
