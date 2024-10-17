using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Print : Locationed
    {
        public readonly ExpressionList Expressions;

        public Print(LexLocation location, ExpressionList expressions) : base(location) 
        { 
            Expressions = expressions; 
        }
    }

}
