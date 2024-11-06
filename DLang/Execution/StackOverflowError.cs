namespace DLang.Execution
{

    internal class StackOverflowError : Exception
    {
        public StackOverflowError(string message) : base(message) { }

        public StackOverflowError(string message, Exception innerException) : base(message, innerException) { }
    }

}
