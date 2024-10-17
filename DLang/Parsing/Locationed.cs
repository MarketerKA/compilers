using QUT.Gppg;

namespace DLang.Parsing
{

    internal class Locationed
    {
        public LexLocation Location { get; set; }

        public Locationed(LexLocation location)
        {
            Location = location;
        }
    }

}
