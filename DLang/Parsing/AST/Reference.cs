using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal enum ReferenceType
    {
        Identifier,
        ArrayIndex,
        FunctionCall,
        TupleIndex,
        Member
    }

    internal class Reference : Locationed
    {
        public readonly ReferenceType Type;
        public readonly string? Identifier;
        public readonly Int128? TupleIndex;
        public readonly Reference? Subreference;
        public readonly Expression? Expression;
        public readonly ExpressionList? Expressions;

        public Reference(LexLocation location, string? identifier) : base(location)
        {
            Identifier = identifier;
            Type = ReferenceType.Identifier;
        }

        public Reference(LexLocation location, Reference reference, Expression expression) : base(location)
        {
            Subreference = reference;
            Expression = expression;
            Type = ReferenceType.ArrayIndex;
        }

        public Reference(LexLocation location, Reference reference, ExpressionList expressionList) : base(location)
        {
            Subreference = reference;
            Expressions = expressionList;
            Type = ReferenceType.FunctionCall;
        }

        public Reference(LexLocation location, Reference reference, string identifier) : base(location)
        {
            Identifier = identifier;
            Subreference = reference;
            Type = ReferenceType.Member;
        }

        public Reference(LexLocation location, Reference reference, Int128 tupleIndex) : base(location)
        {
            TupleIndex = tupleIndex;
            Subreference = reference;
            Type = ReferenceType.TupleIndex;
        }
    }

}
