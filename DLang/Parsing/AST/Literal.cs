using QUT.Gppg;

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

    internal class Literal : Locationed
    {
        public readonly LiteralType Type;
        public readonly Int128? IntValue;
        public readonly double? DoubleValue;
        public readonly bool? BoolValue;
        public readonly string? StringValue;
        public readonly Tuple? TupleValue;
        public readonly Array? ArrayValue;

        public Literal(LexLocation location, Int128 value) : base(location) 
        { 
            IntValue = value; 
            Type = LiteralType.Int;
        }

        public Literal(LexLocation location, double value) : base(location)
        { 
            DoubleValue = value;
            Type = LiteralType.Double;
        }

        public Literal(LexLocation location, bool value) : base(location)
        { 
            BoolValue = value; 
            Type = LiteralType.Bool;
        }

        public Literal(LexLocation location, string value) : base(location)
        { 
            StringValue = value;
            Type = LiteralType.String;
        }

        public Literal(LexLocation location, Tuple value) : base(location)
        { 
            TupleValue = value; 
            Type = LiteralType.Tuple;
        }

        public Literal(LexLocation location, Array value) : base(location)
        { 
            ArrayValue = value; 
            Type = LiteralType.Array;
        }

        public Literal(LexLocation location) : base(location)
        {
            Type = LiteralType.Empty;
        }
    }

}
