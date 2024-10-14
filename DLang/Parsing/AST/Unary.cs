namespace DLang.Parsing.AST
{

    internal class Unary
    {
        public readonly Reference? Reference;
        public readonly TypeIndicator? TypeIndicator;
        public readonly Primary? Primary;

        public Unary(Reference reference)
        {
            Reference = reference;
        }

        public Unary(Reference reference, TypeIndicator typeIndicator)
        {
            Reference = reference;
            TypeIndicator = typeIndicator;
        }

        public Unary(Primary primary)
        {
            Primary = primary;
        }
    }

}
