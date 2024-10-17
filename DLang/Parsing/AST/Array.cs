using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Array : Locationed
    {
        public readonly ArrayElements Elements;

        public Array(LexLocation location, ArrayElements elements) : base(location)
        {
            Elements = elements;
        }
    }

}
