using QUT.Gppg;

namespace DLang.Analysis
{

    internal class SemanticError : Exception
    {
        public readonly LexLocation Location;

        public SemanticError(LexLocation location, string message) : base(message) 
        {
            Location = location;
        }

        public SemanticError(LexLocation location, string message, Exception innerException) : base(message, innerException) 
        {
            Location = location;
        }
    }

}
