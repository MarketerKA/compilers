namespace DLang.Parsing.AST
{

    internal class Print
    {
        public readonly ExpressionList Expressions;

        public Print(ExpressionList expressions) { Expressions = expressions; }
    }

}
