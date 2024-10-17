namespace DLang.Parsing.AST
{

    internal enum UnaryType
    {
        Reference,
        Is,
        Primary
    }

    internal class Unary
    {
        public readonly UnaryType Type;
        public readonly Reference? Reference;
        public readonly TypeIndicator? TypeIndicator;
        public readonly Primary? Primary;
        public readonly PrimaryOperator? PrimaryOperator;

        public Unary(Reference reference)
        {
            Reference = reference;
            Type = UnaryType.Reference;
        }

        public Unary(Reference reference, TypeIndicator typeIndicator)
        {
            Reference = reference;
            TypeIndicator = typeIndicator;
            Type = UnaryType.Is;
        }

        public Unary(Primary primary)
        {
            Primary = primary;
            Type = UnaryType.Primary;
        }

        public Unary(PrimaryOperator primaryOperator, Primary primary)
        {
            PrimaryOperator = primaryOperator;
            Primary = primary;
            Type = UnaryType.Primary;
        }
    }

}
