using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class Relation : Locationed
    {
        public readonly Factor Left;
        public readonly RelationOperator? Operator;
        public readonly Factor? Right;

        public Relation(LexLocation location, Factor left, RelationOperator? @operator, Factor? right) : base(location)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

}
