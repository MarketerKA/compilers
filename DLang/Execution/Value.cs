using DLang.Analysis;

namespace DLang.Execution
{

    internal class Value
    {
        public ValueType Type { get; private set; }
        private Int128? _intValue;
        private double? _realValue;
        private bool? _boolValue;
        private string? _stringValue;
        private Array? _arrayValue;
        private Tuple? _tupleValue;
        private Function? _functionValue;

        private string? _currOp;

        public Value()
        {
            Type = ValueType.Empty;
        }

        public Value(Int128 value)
        {
            Type = ValueType.Int;
            _intValue = value;
        }

        public Value(double value)
        {
            Type = ValueType.Real;
            _realValue = value;
        }

        public Value(bool value)
        {
            Type = ValueType.Bool;
            _boolValue = value;
        }

        public Value(string value)
        {
            Type = ValueType.String;
            _stringValue = value;
        }

        public Value(Array value)
        {
            Type = ValueType.Array;
            _arrayValue = value;
        }

        public Value(Tuple value)
        {
            Type = ValueType.Tuple;
            _tupleValue = value;
        }

        public Value(Function value)
        {
            Type = ValueType.Func;
            _functionValue = value;
        }

        public Value(ConstexprValue constexprValue)
        {
            switch (constexprValue.Type)
            {
                case ConstexprValueType.Int:
                    Type = ValueType.Int;
                    _intValue = constexprValue.IntValue;
                    break;
                case ConstexprValueType.Real:
                    Type = ValueType.Real;
                    _realValue = constexprValue.RealValue;
                    break;
                case ConstexprValueType.Bool:
                    Type = ValueType.Bool;
                    _boolValue = constexprValue.BoolValue;
                    break;
                case ConstexprValueType.String:
                    Type = ValueType.String;
                    _stringValue = constexprValue.StringValue;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void Assign(Value other)
        {
            switch (Type)
            {
                case ValueType.Int:
                    _intValue = null;
                    break;
                case ValueType.Real:
                    _realValue = null;
                    break;
                case ValueType.Bool:
                    _boolValue = null;
                    break;
                case ValueType.String:
                    _stringValue = null;
                    break;
                case ValueType.Array:
                    _arrayValue = null;
                    break;
                case ValueType.Tuple:
                    _tupleValue = null;
                    break;
                case ValueType.Func:
                    _functionValue = null;
                    break;
            }

            Type = other.Type;

            switch (other.Type)
            {
                case ValueType.Int:
                    _intValue = (Int128)other._intValue!;
                    break;
                case ValueType.Real:
                    _realValue = (double)other._realValue!;
                    break;
                case ValueType.Bool:
                    _boolValue = (bool)other._boolValue!;
                    break;
                case ValueType.String:
                    _stringValue = other._stringValue;
                    break;
                case ValueType.Array:
                    _arrayValue = other._arrayValue;
                    break;
                case ValueType.Tuple:
                    _tupleValue = other._tupleValue;
                    break;
                case ValueType.Func:
                    _functionValue = other._functionValue;
                    break;
            }
        }

        private void ThrowError(Value value)
        {
            throw new ValueError("unsupported operation " +
                $"{Type.ToString().ToLower()} {_currOp!} {value.Type.ToString().ToLower()}");
        }

        private void ThrowError()
        {
            throw new ValueError("unsupported operation " +
                $"{_currOp!} {Type.ToString().ToLower()}");
        }

        public Int128 Int()
        {
            if (Type != ValueType.Int) throw new InvalidOperationException();
            return (Int128)_intValue!;
        }

        public double Real()
        {
            if (Type != ValueType.Real) throw new InvalidOperationException();
            return (double)_realValue!;
        }

        public bool Bool()
        {
            if (Type != ValueType.Bool) throw new InvalidOperationException();
            return (bool)_boolValue!;
        }

        public string String()
        {
            if (Type != ValueType.String) throw new InvalidOperationException();
            return _stringValue!;
        }

        public Array Array()
        {
            if (Type != ValueType.Array) throw new InvalidOperationException();
            return _arrayValue!;
        }

        public Tuple Tuple()
        {
            if (Type != ValueType.Tuple) throw new InvalidOperationException();
            return _tupleValue!;
        }

        public Function Function()
        {
            if (Type != ValueType.Func) throw new InvalidOperationException();
            return _functionValue!;
        }

        public Value Plus(Value value)
        {
            _currOp = "+";

            switch (Type)
            {
                case ValueType.Int:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Int() + value.Int());
                        case ValueType.Real:
                            return new Value((double)Int() + value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Real:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Real() + (double)value.Int());
                        case ValueType.Real:
                            return new Value(Real() + value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.String:
                    switch (value.Type)
                    {
                        case ValueType.String:
                            return new Value(String() + value.String());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Array:
                    switch (value.Type)
                    {
                        case ValueType.Array:
                            return new Value(Array() + value.Array());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Tuple:
                    switch (value.Type)
                    {
                        case ValueType.Tuple:
                            return new Value(Tuple() + value.Tuple());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value Plus()
        {
            _currOp = "+";

            switch (Type)
            {
                case ValueType.Int:
                    return new Value(+Int());
                case ValueType.Real:
                    return new Value(+Real());
                default:
                    ThrowError();
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value Minus(Value value)
        {
            _currOp = "-";

            switch (Type)
            {
                case ValueType.Int:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Int() - value.Int());
                        case ValueType.Real:
                            return new Value((double)Int() - value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Real:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Real() - (double)value.Int());
                        case ValueType.Real:
                            return new Value(Real() - value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value Minus()
        {
            _currOp = "-";

            switch (Type)
            {
                case ValueType.Int:
                    return new Value(-Int());
                case ValueType.Real:
                    return new Value(-Real());
                default:
                    ThrowError();
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value Multiply(Value value)
        {
            _currOp = "*";

            switch (Type)
            {
                case ValueType.Int:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Int() * value.Int());
                        case ValueType.Real:
                            return new Value((double)Int() * value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Real:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Real() * (double)value.Int());
                        case ValueType.Real:
                            return new Value(Real() * value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();

        }

        public Value Divide(Value value)
        {
            _currOp = "/";

            try
            {
                switch (Type)
                {
                    case ValueType.Int:
                        switch (value.Type)
                        {
                            case ValueType.Int:
                                return new Value(Int() / value.Int());
                            case ValueType.Real:
                                return new Value((double)Int() / value.Real());
                            default:
                                ThrowError(value);
                                break;
                        }
                        break;
                    case ValueType.Real:
                        switch (value.Type)
                        {
                            case ValueType.Int:
                                return new Value(Real() / (double)value.Int());
                            case ValueType.Real:
                                return new Value(Real() / value.Real());
                            default:
                                ThrowError(value);
                                break;
                        }
                        break;
                    default:
                        ThrowError(value);
                        break;
                }
            }
            catch (DivideByZeroException)
            {
                throw new ValueError("division by zero");
            }

            throw new InvalidOperationException();
        }

        public Value Not()
        {
            _currOp = "not";

            switch (Type)
            {
                case ValueType.Bool:
                    return new Value(!Bool());
                default:
                    ThrowError();
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value And(Value value)
        {
            _currOp = "and";

            switch (Type)
            {
                case ValueType.Bool:
                    switch (value.Type)
                    {
                        case ValueType.Bool:
                            return new Value(Bool() && value.Bool());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value Or(Value value)
        {
            _currOp = "or";

            switch (Type)
            {
                case ValueType.Bool:
                    switch (value.Type)
                    {
                        case ValueType.Bool:
                            return new Value(Bool() || value.Bool());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value Xor(Value value)
        {
            _currOp = "xor";

            switch (Type)
            {
                case ValueType.Bool:
                    switch (value.Type)
                    {
                        case ValueType.Bool:
                            return new Value(Bool() ^ value.Bool());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value Less(Value value)
        {
            _currOp = "<";

            switch (Type)
            {
                case ValueType.Int:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Int() < value.Int());
                        case ValueType.Real:
                            return new Value((double)Int() < value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Real:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Real() < (double)value.Int());
                        case ValueType.Real:
                            return new Value(Real() < value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value LessEqual(Value value)
        {
            _currOp = "<=";

            switch (Type)
            {
                case ValueType.Int:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Int() <= value.Int());
                        case ValueType.Real:
                            return new Value((double)Int() <= value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Real:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Real() <= (double)value.Int());
                        case ValueType.Real:
                            return new Value(Real() <= value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value Greater(Value value)
        {
            _currOp = ">";

            switch (Type)
            {
                case ValueType.Int:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Int() > value.Int());
                        case ValueType.Real:
                            return new Value((double)Int() > value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Real:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Real() > (double)value.Int());
                        case ValueType.Real:
                            return new Value(Real() > value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value GreaterEqual(Value value)
        {
            _currOp = ">=";

            switch (Type)
            {
                case ValueType.Int:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Int() >= value.Int());
                        case ValueType.Real:
                            return new Value((double)Int() >= value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Real:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Real() >= (double)value.Int());
                        case ValueType.Real:
                            return new Value(Real() >= value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value Equal(Value value)
        {
            _currOp = "=";

            switch (Type)
            {
                case ValueType.Int:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Int() == value.Int());
                        case ValueType.Real:
                            return new Value((double)Int() == value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Real:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Real() == (double)value.Int());
                        case ValueType.Real:
                            return new Value(Real() == value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public Value NotEqual(Value value)
        {
            _currOp = "/=";

            switch (Type)
            {
                case ValueType.Int:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Int() != value.Int());
                        case ValueType.Real:
                            return new Value((double)Int() != value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                case ValueType.Real:
                    switch (value.Type)
                    {
                        case ValueType.Int:
                            return new Value(Real() != (double)value.Int());
                        case ValueType.Real:
                            return new Value(Real() != value.Real());
                        default:
                            ThrowError(value);
                            break;
                    }
                    break;
                default:
                    ThrowError(value);
                    break;
            }

            throw new InvalidOperationException();
        }

        public override string ToString()
        {
            switch (Type)
            {
                case ValueType.Int:
                    return Int().ToString();
                case ValueType.Real:
                    return Real().ToString();
                case ValueType.Bool:
                    return Bool() == true ? "true" : "false";
                case ValueType.String:
                    return String().ToString();
                case ValueType.Array:
                    return Array().ToString();
                case ValueType.Tuple:
                    return Tuple().ToString();
                case ValueType.Func:
                    return Function().ToString();
                case ValueType.Empty:
                    return "empty";
            }

            throw new InvalidOperationException();
        }
    }

}