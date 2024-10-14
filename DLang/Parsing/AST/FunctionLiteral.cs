namespace DLang.Parsing.AST
{

    internal class FunctionLiteral
    {
        public readonly IdentifierList? Identifiers;
        public readonly FunctionBody Body;

        public FunctionLiteral(IdentifierList? identifiers, FunctionBody body)
        {
            Identifiers = identifiers;
            Body = body;
        }
    }

}
