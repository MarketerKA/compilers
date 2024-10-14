namespace DLang.Parsing.AST
{

    internal class IdentifierList
    {
        public readonly List<string> Identifiers;

        public IdentifierList() { Identifiers = []; }

        public IdentifierList(string identifier) { Identifiers = [identifier]; }

        public void Add(string identifier) { Identifiers.Add(identifier); }
    }

}
