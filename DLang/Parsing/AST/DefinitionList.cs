using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class DefinitionList : Locationed
    {
        public readonly List<Definition> Definitions;

        public DefinitionList(LexLocation location) : base(location) 
        { 
            Definitions = [];  
        }

        public DefinitionList(LexLocation location, Definition definition) : base(location) 
        {  
            Definitions = [definition]; 
        }

        public void Add(Definition definition) { Definitions.Add(definition); }
    }

}
