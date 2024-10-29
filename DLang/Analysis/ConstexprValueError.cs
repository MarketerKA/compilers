using QUT.Gppg;

namespace DLang.Analysis
{

    internal class ConstexprValueError : Exception
    {
        public ConstexprValueError(string message) : base(message)
        {
        }

        public ConstexprValueError(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
