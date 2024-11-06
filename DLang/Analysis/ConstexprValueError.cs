using QUT.Gppg;

namespace DLang.Analysis
{

    internal class ConstexprValueError : Exception
    {
        private static string ConstructMessage(string op, ConstexprValueType left, ConstexprValueType right)
        {
            return "unsupported operation " +
                $"{left.ToString().ToLower()} {op} {right.ToString().ToLower()}";
        }

        private static string ConstructMessage(string op, ConstexprValueType left)
        {
            return "unsupported operation " +
                $"{op} {left.ToString().ToLower()}";
        }

        public ConstexprValueError(string message) : base(message) { }

        public ConstexprValueError(string op, ConstexprValueType left, ConstexprValueType right) : base(ConstructMessage(op, left, right))
        {
        }

        public ConstexprValueError(string op, ConstexprValueType left) : base(ConstructMessage(op, left))
        {
        }
    }

}
