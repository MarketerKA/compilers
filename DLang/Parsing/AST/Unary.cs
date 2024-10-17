using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal enum UnaryType
    {
        Reference,
        Is,
        Primary
    }

    internal class Unary : Locationed
    {
        public readonly UnaryType Type;
        public readonly Reference? Reference;
        public readonly TypeIndicator? TypeIndicator;
        public readonly Primary? Primary;
        public readonly PrimaryOperator? PrimaryOperator;

        public Unary(LexLocation location, Reference reference) : base(location)
        {
            Reference = reference;
            Type = UnaryType.Reference;
        }

        public Unary(LexLocation location, Reference reference, TypeIndicator typeIndicator) : base(location)
        {
            Reference = reference;
            TypeIndicator = typeIndicator;
            Type = UnaryType.Is;
        }

        public Unary(LexLocation location, Primary primary) : base(location)
        {
            Primary = primary;
            Type = UnaryType.Primary;
        }

        public Unary(LexLocation location, PrimaryOperator primaryOperator, Primary primary) : base(location)
        {
            PrimaryOperator = primaryOperator;
            Primary = primary;
            Type = UnaryType.Primary;
        }
    }

}
