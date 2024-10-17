using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class FunctionLiteral : Locationed
    {
        public readonly IdentifierList? Identifiers;
        public readonly FunctionBody Body;

        public FunctionLiteral(LexLocation location, IdentifierList? identifiers, FunctionBody body) : base(location)
        {
            Identifiers = identifiers;
            Body = body;
        }
    }

}
