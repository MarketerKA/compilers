using System;
using System.Collections.Generic;
namespace DLang.Parsing.AST
{
    internal class Primary
    {
        public readonly Literal? Literal;
        public readonly ReadType? Read;
        public readonly FunctionLiteral? FunctionLiteral;
        public readonly Expression? Expression;
        public readonly PrimaryOperator? Operator;
        public readonly Primary? Subprimary;

        public Primary(Literal literal)
        {
            Literal = literal;
        }

        public Primary(ReadType read)
        {
            Read = read;
        }

        public Primary(FunctionLiteral functionLiteral)
        {
            FunctionLiteral = functionLiteral;
        }

        public Primary(Expression expression)
        {
            Expression = expression;
        }

        public Primary(PrimaryOperator @operator, Primary primary)
        {
            Operator = @operator;
            Subprimary = primary;
        }
    }
}
