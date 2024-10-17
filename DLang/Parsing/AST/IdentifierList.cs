using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class IdentifierList : Locationed
    {
        public readonly List<string> Identifiers;

        public IdentifierList(LexLocation location) : base(location) 
        { 
            Identifiers = []; 
        }

        public IdentifierList(LexLocation location, string identifier) : base(location) 
        { 
            Identifiers = [identifier]; 
        }

        public void Add(string identifier) { Identifiers.Add(identifier); }
    }

}
