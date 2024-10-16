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

    internal class Reference
    {
        public readonly ReferenceType Type;
        public readonly string? Identifier;
        public readonly Int128? TupleIndex;
        public readonly Reference? Subreference;
        public readonly Expression? Expression;
        public readonly ExpressionList? Expressions;

        public Reference(string? identifier)
        {
            Identifier = identifier;
            Type = ReferenceType.Identifier;
        }

        public Reference(Reference reference, Expression expression)
        {
            Subreference = reference;
            Expression = expression;
            Type = ReferenceType.ArrayIndex;
        }

        public Reference(Reference reference, ExpressionList expressionList)
        {
            Subreference = reference;
            Expressions = expressionList;
            Type = ReferenceType.FunctionCall;
        }

        public Reference(Reference reference, string identifier)
        {
            Identifier = identifier;
            Subreference = reference;
            Type = ReferenceType.Member;
        }

        public Reference(Reference reference, Int128 tupleIndex)
        {
            TupleIndex = tupleIndex;
            Subreference = reference;
            Type = ReferenceType.TupleIndex;
        }
    }

}
