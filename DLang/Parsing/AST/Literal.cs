namespace DLang.Parsing.AST
{

    internal class Literal
    {
        public readonly Int128? IntValue;
        public readonly double? DoubleValue;
        public readonly bool? BoolValue;
        public readonly string? StringValue;
        public readonly Tuple? TupleValue;
        public readonly Array? ArrayValue;

        public Literal(Int128 value) { IntValue = value; }

        public Literal(double value) { DoubleValue = value; }

        public Literal(bool value) { BoolValue = value; }

        public Literal(string value) { StringValue = value; }

        public Literal(Tuple value) { TupleValue = value; }

        public Literal(Array value) { ArrayValue = value; }

        public Literal() { }
    }

}
