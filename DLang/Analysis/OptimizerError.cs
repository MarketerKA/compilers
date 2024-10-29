using QUT.Gppg;

namespace DLang.Analysis
{

    internal class OptimizerError : Exception
    {
        public readonly LexLocation Location;

        public OptimizerError(LexLocation location, string message) : base(message)
        {
            Location = location;
        }

        public OptimizerError(LexLocation location, string message, Exception innerException) : base(message, innerException)
        {
            Location = location;
        }
    }

}
