namespace DLang.Execution
{

    internal class ValueError : Exception
    {
        public ValueError(string message) : base(message) { }

        public ValueError(string message, Exception innerException) : base(message, innerException) { }
    }

}
