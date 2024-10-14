namespace DLang.Parsing.AST
{

    internal class FunctionBody
    {
        public readonly StatementList? Statements;
        public readonly Expression? Expression;

        public FunctionBody(StatementList statements)
        {
            Statements = statements;
        }

        public FunctionBody(Expression expression)
        {
            Expression = expression;
        }
    }

}
