using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class If : Locationed
    {
        public readonly Expression Expression;
        public readonly StatementList Statements;
        public readonly StatementList? Tail;

        public If(LexLocation location, Expression expression, StatementList statements, StatementList? tail) : base(location)
        {
            Expression = expression;
            Statements = statements;
            Tail = tail;
        }
    }

}
