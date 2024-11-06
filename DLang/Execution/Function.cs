using DLang.Parsing.AST;
using System.Text;

namespace DLang.Execution
{

    internal class Function
    {
        public readonly FunctionType Type;
        private readonly StatementList? _statements;
        private readonly Expression? _expression;
        public readonly List<string> Identifiers;

        public Function(List<string> identifiers, StatementList statements)
        {
            Identifiers = identifiers;
            _statements = statements;
            Type = FunctionType.Full;
        }

        public Function(List<string> identifiers, Expression expression)
        {
            Identifiers = identifiers;
            _expression = expression;
            Type = FunctionType.Simple;
        }

        public StatementList Statements()
        {
            if (Type != FunctionType.Full)
            {
                throw new InvalidOperationException();
            }

            return _statements!;
        }

        public Expression Expression()
        {
            if (Type != FunctionType.Simple)
            {
                throw new InvalidOperationException();
            }

            return _expression!;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("function(");
            for (int i = 0; i < Identifiers.Count; i++)
            {
                sb.Append(Identifiers[i]);
                if (i < Identifiers.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(')');
            return sb.ToString();
        }
    }

}
