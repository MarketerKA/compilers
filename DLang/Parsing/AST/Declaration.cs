using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Declaration : Locationed
    {
        public readonly DefinitionList Definitions;

        public Declaration(LexLocation location, DefinitionList definitionList) : base(location) 
        { 
            Definitions = definitionList; 
        }
    }

}
