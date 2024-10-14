namespace DLang.Parsing.AST
{

    internal class DefinitionList
    {
        public readonly List<Definition> Definitions;

        public DefinitionList() { Definitions = [];  }

        public DefinitionList(Definition definition) {  Definitions = [definition]; }

        public void Add(Definition definition) { Definitions.Add(definition); }
    }

}
