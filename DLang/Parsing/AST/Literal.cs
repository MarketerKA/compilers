namespace DLang.Parsing.AST
{

    internal enum LiteralType
    {
        Int,
        Double,
        Bool,
        String,
        Tuple,
        Array,
        Empty
    }

    internal class Literal
    {
        public readonly LiteralType Type;
        public readonly Int128? IntValue;
        public readonly double? DoubleValue;
        public readonly bool? BoolValue;
        public readonly string? StringValue;
        public readonly Tuple? TupleValue;
        public readonly Array? ArrayValue;

        public Literal(Int128 value) { 
            IntValue = value; 
            Type = LiteralType.Int;
        }

        public Literal(double value) { 
            DoubleValue = value;
            Type = LiteralType.Double;
        }

        public Literal(bool value) { 
            BoolValue = value; 
            Type = LiteralType.Bool;
        }

        public Literal(string value) { 
            StringValue = value;
            Type = LiteralType.String;
        }

        public Literal(Tuple value) { 
            TupleValue = value; 
            Type = LiteralType.Tuple;
        }

        public Literal(Array value) { 
            ArrayValue = value; 
            Type = LiteralType.Array;
        }

        public Literal() {
            Type = LiteralType.Empty;
        }
    }

}
