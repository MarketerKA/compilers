namespace DLang.Parsing.AST
{

    internal class Declaration
    {
        public readonly DefinitionList Definitions;

        public Declaration(DefinitionList definitionList) { Definitions = definitionList; }
    }

}
