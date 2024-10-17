using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal enum FunctionBodyType
    {
        Full,
        Simple
    }

    internal class FunctionBody : Locationed
    {
        public readonly FunctionBodyType Type;
        public readonly StatementList? Statements;
        public readonly Expression? Expression;

        public FunctionBody(LexLocation location, StatementList statements) : base(location)
        {
            Statements = statements;
            Type = FunctionBodyType.Full;
        }

        public FunctionBody(LexLocation location, Expression expression) : base(location)
        {
            Expression = expression;
            Type = FunctionBodyType.Simple;
        }
    }

}
