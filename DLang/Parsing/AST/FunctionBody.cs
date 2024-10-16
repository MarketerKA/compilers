namespace DLang.Parsing.AST
{

    internal enum FunctionBodyType
    {
        Full,
        Simple
    }

    internal class FunctionBody
    {
        public readonly FunctionBodyType Type;
        public readonly StatementList? Statements;
        public readonly Expression? Expression;

        public FunctionBody(StatementList statements)
        {
            Statements = statements;
            Type = FunctionBodyType.Full;
        }

        public FunctionBody(Expression expression)
        {
            Expression = expression;
            Type = FunctionBodyType.Simple;
        }
    }

}
