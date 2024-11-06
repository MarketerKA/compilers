using QUT.Gppg;

namespace DLang.Lexing
{

    internal class LexerError : Exception
    {
        public readonly LexLocation Location;

        public LexerError(LexLocation location, string msg) : base(msg) 
        {
            Location = location;
        }

    }

}
