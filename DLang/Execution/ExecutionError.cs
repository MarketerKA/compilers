using QUT.Gppg;

namespace DLang.Execution
{

    internal class ExecutionError : Exception
    {
        public readonly LexLocation? Location;

        public ExecutionError(string message) : base(message) { }

        public ExecutionError(string message, Exception innerException) : base(message, innerException) { }

        public ExecutionError(LexLocation location, string message) : base(message) 
        {
            Location = location;
        }

        public ExecutionError(LexLocation location, string message, Exception innerException) : base(message, innerException) 
        {
            Location = location;
        }
    }

}
