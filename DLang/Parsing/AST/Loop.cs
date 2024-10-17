using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal enum LoopType
    {
        While,
        For
    }

    internal class Loop : Locationed
    {
        public readonly LoopType Type;
        public readonly Expression? Expression;
        public readonly Range? Range;
        public readonly string? Identifier;
        public readonly StatementList Statements;

        public Loop(LexLocation location, Expression expression, StatementList statements) : base(location)
        {
            Expression = expression;
            Statements = statements;
            Type = LoopType.While;
        }

        public Loop(LexLocation location, string? identifier, Range range, StatementList statements) : base(location)
        {
            Identifier = identifier;
            Range = range;
            Statements = statements;
            Type = LoopType.For;
        }
    }

}
