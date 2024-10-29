using DLang.Parsing.AST;
using System.Numerics;

namespace DLang.Analysis
{

    internal enum ConstexprValueType
    {
        Int,
        Real,
        Bool,
        String,
        NonConstexpr
    }

    internal class ConstexprValue
    {
        public ConstexprValueType Type { get; set; }
        public Int128? IntValue { get; set; }
        public double? RealValue { get; set; }
        public bool? BoolValue { get; set; }
        public string? StringValue { get; set; }

        public static ConstexprValue EvalExpression(Expression expression)
        {

            var left = EvalRelation(expression.Left);
            if (expression.Right != null)
            {
                var right = EvalRelation(expression.Right);
                switch (expression.Operator!)
                {
                    case ExpressionOperator.OR:
                        return left | right;
                    case ExpressionOperator.AND:
                        return left & right;
                    case ExpressionOperator.XOR:
                        return left ^ right;
                }
            }

            return left;

        }

        private static ConstexprValue EvalRelation(Relation relation)
        {
            var left = EvalFactor(relation.Left);
            if (relation.Right != null)
            {
                var right = EvalFactor(relation.Right);
                switch (relation.Operator!)
                {
                    case RelationOperator.LESS:
                        return left < right;
                    case RelationOperator.LESS_EQUAL:
                        return left <= right;
                    case RelationOperator.GREATER:
                        return left > right;
                    case RelationOperator.GREATER_EQUAL:
                        return left >= right;
                    case RelationOperator.EQUAL:
                        return left == right;
                    case RelationOperator.NOT_EQUAL:
                        return left != right;
                }
            }

            return left;
        }

        private static ConstexprValue EvalFactor(Factor factor)
        {
            var left = EvalTerm(factor.Left);
            if (factor.Right != null)
            {
                var right = EvalTerm(factor.Right);
                switch (factor.Operator!)
                {
                    case FactorOperator.PLUS:
                        return left + right;
                    case FactorOperator.MINUS:
                        return left - right;
                }
            }

            return left;
        }

        private static ConstexprValue EvalTerm(Term term)
        {
            var left = EvalUnary(term.Left);
            if (term.Right != null)
            {
                var right = EvalUnary(term.Right);
                switch (term.Operator!)
                {
                    case TermOperator.MULTIPLY:
                        return left * right;
                    case TermOperator.DIVIDE:
                        return left / right;
                }
            }
            
            return left;
        }

        private static ConstexprValue EvalUnary(Unary unary)
        {
            if (unary.Type == UnaryType.Primary)
            {
                var value = EvalPrimary(unary.Primary!);
                switch (unary.PrimaryOperator)
                {
                    case PrimaryOperator.PLUS:
                        return +value;
                    case PrimaryOperator.MINUS:
                        return -value;
                    case PrimaryOperator.NOT:
                        return !value;
                    case null:
                        return value;
                }
            }

            return new ConstexprValue();
        }

        private static ConstexprValue EvalPrimary(Primary primary)
        {
            switch (primary.Type)
            {
                case PrimaryType.Literal:
                    var lit = primary.Literal!;
                    return lit.Type switch
                    {
                        LiteralType.Int => new ConstexprValue((Int128)lit.IntValue!),
                        LiteralType.Double => new ConstexprValue((double)lit.DoubleValue!),
                        LiteralType.Bool => new ConstexprValue((bool)lit.BoolValue!),
                        LiteralType.String => new ConstexprValue(lit.StringValue!),
                        _ => new ConstexprValue(),
                    };
                case PrimaryType.Expression:
                    return EvalExpression(primary.Expression!);
                default:
                    return new ConstexprValue();
            }
        }

        public ConstexprValue(Int128 value)
        {
            IntValue = value;
            Type = ConstexprValueType.Int;
        }

        public ConstexprValue(double value)
        {
            RealValue = value;
            Type = ConstexprValueType.Real;
        }

        public ConstexprValue(bool value)
        {
            BoolValue = value;
            Type = ConstexprValueType.Bool;
        }

        public ConstexprValue(string value)
        {
            StringValue = value;
            Type = ConstexprValueType.String;
        }

        public ConstexprValue()
        {
            Type = ConstexprValueType.NonConstexpr;
        }

        public static ConstexprValue operator +(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! +
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! +
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation +"),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! +
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! +
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation +"),
                },
                ConstexprValueType.Bool => throw new ConstexprValueError("Invalid operation +"),
                ConstexprValueType.String => right.Type switch
                {
                    ConstexprValueType.String => new ConstexprValue(
                                                        left.StringValue! +
                                                        right.StringValue!),
                    _ => throw new ConstexprValueError("Invalid operation +"),
                },
                _ => throw new ConstexprValueError("Invalid operation +"),
            };
        }

        public static ConstexprValue operator -(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! -
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! -
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation -"),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! -
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! -
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation -"),
                },
                _ => throw new ConstexprValueError("Invalid operation -"),
            };
        }

        public static ConstexprValue operator *(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! *
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! *
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation *"),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! *
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! *
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation *"),
                },
                _ => throw new ConstexprValueError("Invalid operation *"),
            };
        }

        public static ConstexprValue operator /(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! /
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! /
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation /"),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! /
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! /
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation /"),
                },
                _ => throw new ConstexprValueError("Invalid operation /"),
            };
        }

        public static ConstexprValue operator +(ConstexprValue other)
        {
            if (other.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return other.Type switch
            {
                ConstexprValueType.Int => new ConstexprValue((Int128)other.IntValue!),
                ConstexprValueType.Real => new ConstexprValue((double)other.RealValue!),
                _ => throw new ConstexprValueError("Invalid operation unary +")
            };
        }

        public static ConstexprValue operator -(ConstexprValue other)
        {
            if (other.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return other.Type switch
            {
                ConstexprValueType.Int => new ConstexprValue(-(Int128)other.IntValue!),
                ConstexprValueType.Real => new ConstexprValue(-(double)other.RealValue!),
                _ => throw new ConstexprValueError("Invalid operation unary -")
            };
        }

        public static ConstexprValue operator !(ConstexprValue other)
        {
            if (other.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return other.Type switch
            {
                ConstexprValueType.Bool => new ConstexprValue(!(bool)other.BoolValue!),
                _ => throw new ConstexprValueError("Invalid operation not")
            };
        }

        public static ConstexprValue operator &(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            if (left.Type != ConstexprValueType.Bool || right.Type != ConstexprValueType.Bool)
            {
                throw new ConstexprValueError("Invalid operation and");
            }

            return new ConstexprValue((bool)left.BoolValue! && (bool)right.BoolValue!);
        }

        public static ConstexprValue operator |(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            if (left.Type != ConstexprValueType.Bool || right.Type != ConstexprValueType.Bool)
            {
                throw new ConstexprValueError("Invalid operation or");
            }

            return new ConstexprValue((bool)left.BoolValue! || (bool)right.BoolValue!);
        }

        public static ConstexprValue operator ^(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            if (left.Type != ConstexprValueType.Bool || right.Type != ConstexprValueType.Bool)
            {
                throw new ConstexprValueError("Invalid operation xor");
            }

            return new ConstexprValue((bool)left.BoolValue! ^ (bool)right.BoolValue!);
        }

        public static ConstexprValue operator <(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! <
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! <
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation <"),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! <
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! <
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation <"),
                },
                _ => throw new ConstexprValueError("Invalid operation <"),
            };
        }

        public static ConstexprValue operator >(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! >
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! >
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation >"),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! >
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! >
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation >"),
                },
                _ => throw new ConstexprValueError("Invalid operation >"),
            };
        }

        public static ConstexprValue operator <=(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! <=
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! <=
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation <="),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! <=
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! <=
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation <="),
                },
                _ => throw new ConstexprValueError("Invalid operation <="),
            };
        }

        public static ConstexprValue operator >=(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! >=
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! >=
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation >="),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! >=
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! >=
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation >="),
                },
                _ => throw new ConstexprValueError("Invalid operation >="),
            };
        }

        public static ConstexprValue operator ==(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! ==
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! ==
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation ="),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! ==
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! ==
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation ="),
                },
                _ => throw new ConstexprValueError("Invalid operation ="),
            };
        }

        public static ConstexprValue operator !=(ConstexprValue left, ConstexprValue right)
        {
            if (left.Type == ConstexprValueType.NonConstexpr || right.Type == ConstexprValueType.NonConstexpr)
            {
                return new ConstexprValue();
            }

            return left.Type switch
            {
                ConstexprValueType.Int => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (Int128)left.IntValue! !=
                                                    (Int128)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.IntValue! !=
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation /="),
                },
                ConstexprValueType.Real => right.Type switch
                {
                    ConstexprValueType.Int => new ConstexprValue(
                                                    (double)left.RealValue! !=
                                                    (double)right.IntValue!),
                    ConstexprValueType.Real => new ConstexprValue(
                                                    (double)left.RealValue! !=
                                                    (double)right.RealValue!),
                    _ => throw new ConstexprValueError("Invalid operation /="),
                },
                _ => throw new ConstexprValueError("Invalid operation /="),
            };
        }
    }

}
