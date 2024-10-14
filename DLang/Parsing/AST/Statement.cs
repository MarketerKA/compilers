namespace DLang.Parsing.AST
{

    internal class Statement
    {
        public readonly Declaration? Declaration;
        public readonly Assignment? Assignment;
        public readonly Print? Print;
        public readonly If? If;
        public readonly Loop? Loop;
        public readonly Return? Return;

        public Statement(Declaration declaration) { Declaration = declaration; }

        public Statement(Assignment assignment) { Assignment = assignment; }

        public Statement(Print print) { Print = print; }

        public Statement(If @if) { If = @if; }

        public Statement(Loop? loop) { Loop = loop; }

        public Statement(Return @return) { Return = @return; }
    }

}
