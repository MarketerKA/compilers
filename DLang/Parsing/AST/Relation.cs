namespace DLang.Parsing.AST
{

    internal class Relation
    {
        public readonly Factor Left;
        public readonly RelationOperator? Operator;
        public readonly Factor? Right;

        public Relation(Factor left, RelationOperator? @operator, Factor? right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

}
