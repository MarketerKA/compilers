using DLang.Parsing.AST;
using QUT.Gppg;

namespace DLang.Execution
{

    internal class Program
    {
        private readonly ProgramTree _tree;
        private readonly Stack _stack;
        private readonly Input _input;
        private readonly Output _output;

        public Program(ProgramTree tree, Stack stack, Input input, Output output)
        {
            _tree = tree;
            _stack = stack;
            _input = input;
            _output = output;
        }

        public void Run()
        {
            ExecuteStatementList(_tree.Statements);
        }

        private Value? ExecuteStatementList(StatementList statementList, IEnumerable<KeyValuePair<string, Value>>? addValues = null)
        {
            _stack.Push();

            if (addValues != null)
            {
                foreach (var pair in addValues)
                {
                    _stack.PutValue(pair.Key, pair.Value);
                }
            }

            foreach (Statement statement in statementList.List)
            {
                Value? val = ExecuteStatement(statement);
                if (val != null)
                {
                    _stack.Pop();
                    return val;
                }
            }

            _stack.Pop();

            return null;
        }

        private Value? ExecuteStatement(Statement statement)
        {
            switch (statement.Type)
            {
                case StatementType.Declaration:
                    ExecuteDeclaration(statement.Declaration!);
                    break;
                case StatementType.Assignment:
                    ExecuteAssignment(statement.Assignment!);
                    break;
                case StatementType.Print:
                    ExecutePrint(statement.Print!);
                    break;
                case StatementType.If:
                    return ExecuteIf(statement.If!);
                case StatementType.Loop:
                    return ExecuteLoop(statement.Loop!);
                case StatementType.Return:
                    return ExecuteReturn(statement.Return!);
                case StatementType.Expression:
                    EvalExpression(statement.Expression!);
                    break;
            }

            return null;
        }

        private void ExecuteDeclaration(Declaration declaration)
        {
            foreach (var def in declaration.Definitions.Definitions)
            {
                _stack.PutValue(def.Identifier,
                    def.Expression == null ?
                    new Value() :
                    EvalExpression(def.Expression));
            }
        }

        private void ExecuteAssignment(Assignment assignment)
        {
            ResolveReference(assignment.Reference).Assign(EvalExpression(assignment.Expression));
        }

        private void ExecutePrint(Print print)
        {
            foreach (var expr in print.Expressions.Expressions)
            {
                _output.Write(EvalExpression(expr).ToString());
            }
        }

        private Value? ExecuteIf(If @if)
        {
            Value val = EvalExpression(@if.Expression);
            if (val.Type != ValueType.Bool)
            {
                throw new ExecutionError(@if.Expression.Location, "if condition evaluated to non-bool value");
            }

            if (val.Bool())
            {
                Value? ret = ExecuteStatementList(@if.Statements);
                if (ret != null)
                {
                    return ret;
                }
            }

            if (@if.Tail != null)
            {
                if (!val.Bool())
                {
                    Value? ret = ExecuteStatementList(@if.Tail);
                    if (ret != null)
                    {
                        return ret;
                    }
                }
            }

            return null;
        }

        private Value? ExecuteLoop(Loop loop)
        {
            switch (loop.Type)
            {
                case LoopType.While:
                    while (true)
                    {
                        Value val = EvalExpression(loop.Expression!);
                        if (val.Type != ValueType.Bool)
                        {
                            throw new ExecutionError(loop.Expression!.Location, "while-loop condition evaluated to non-bool value");
                        }

                        if (!val.Bool())
                        {
                            break;
                        }

                        Value? ret = ExecuteStatementList(loop.Statements);
                        if (ret != null)
                        {
                            return ret;
                        }
                    }
                    break;
                case LoopType.For:
                    Value begin = EvalExpression(loop.Range!.Left);
                    Value end = EvalExpression(loop.Range!.Right);

                    if (begin.Type != ValueType.Int)
                    {
                        throw new ExecutionError(loop.Range!.Left.Location, "left end in range evaluated to non-bool value");
                    }
                    if (end.Type != ValueType.Int)
                    {
                        throw new ExecutionError(loop.Range!.Right.Location, "right end in range evaluated to non-bool value");
                    }

                    Func<string?, Int128, List<KeyValuePair<string, Value>>?> addValues = (id, value) =>
                    {
                        if (id == null)
                        {
                            return null;
                        }

                        return [new(id, new Value(value))];
                    };

                    if (begin.Int() < end.Int())
                    {
                        for (Int128 i = begin.Int(); i < end.Int(); i++)
                        {
                            Value? ret = ExecuteStatementList(loop.Statements, addValues(loop.Identifier, i));
                            if (ret != null)
                            {
                                return ret;
                            }
                        }
                    }
                    else if (begin.Int() > end.Int())
                    {
                        for (Int128 i = begin.Int(); i > end.Int(); i--)
                        {
                            Value? ret = ExecuteStatementList(loop.Statements, addValues(loop.Identifier, i));
                            if (ret != null)
                            {
                                return ret;
                            }
                        }
                    }

                    break;
            }

            return null;
        }

        private Value ExecuteReturn(Return @return)
        {
            if (@return.Expression != null)
            {
                return EvalExpression(@return.Expression);
            }

            return new Value();
        }

        private Value ResolveReference(Reference reference)
        {
            switch (reference.Type)
            {
                case ReferenceType.Identifier:
                    return _stack.GetValue(reference.Identifier!);
                case ReferenceType.ArrayIndex:
                    {
                        Value val = ResolveReference(reference.Subreference!);
                        if (val.Type != ValueType.Array)
                        {
                            throw new ExecutionError(reference.Location, "attempted to index a non-array value");
                        }
                        Value index = EvalExpression(reference.Expression!);
                        if (index.Type != ValueType.Int)
                        {
                            throw new ExecutionError(reference.Expression!.Location, "attempted to index an array with non-int value");
                        }

                        try
                        {
                            return val.Array().Get(index.Int());
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            throw new ExecutionError(reference.Expression!.Location, e.Message);
                        }
                    }
                case ReferenceType.FunctionCall:
                    {
                        Value val = ResolveReference(reference.Subreference!);
                        if (val.Type != ValueType.Func)
                        {
                            throw new ExecutionError(reference.Location, "attempted to call a non-function value");
                        }
                        List<Value> args = [];
                        foreach (var expr in reference.Expressions!.Expressions)
                        {
                            args.Add(EvalExpression(expr));
                        }

                        try
                        {
                            return CallFunction(val.Function(), args);
                        }
                        catch (ArgumentException e)
                        {
                            throw new ExecutionError(reference.Subreference!.Location, e.Message);
                        }
                    }
                case ReferenceType.TupleIndex:
                    {
                        Value val = ResolveReference(reference.Subreference!);
                        if (val.Type != ValueType.Tuple)
                        {
                            throw new ExecutionError(reference.Location, "attempted to tuple-index a non-tuple value");
                        }

                        try
                        {
                            return val.Tuple().Get((Int128)reference.TupleIndex!);
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            throw new ExecutionError(reference.Location, e.Message);
                        }
                    }
                case ReferenceType.Member:
                    {
                        Value val = ResolveReference(reference.Subreference!);
                        if (val.Type != ValueType.Tuple)
                        {
                            throw new ExecutionError(reference.Location, "attempted to access a member of a non-tuple value");
                        }

                        try
                        {
                            return val.Tuple().Get(reference.Identifier!);
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            throw new ExecutionError(reference.Location, e.Message);
                        }
                    }
            }

            throw new InvalidOperationException();
        }

        private Value CallFunction(Function function, List<Value> args)
        {
            int exp = function.Identifiers.Count;
            int got = args.Count;
            if (function.Identifiers.Count != args.Count)
            {
                throw new ArgumentException($"function called with incorrect number of arguments (expected {exp}, got {got})");
            }

            List<KeyValuePair<string, Value>> parameters = [];
            for (int i = 0; i < args.Count; i++)
            {
                Value val = new();
                val.Assign(args[i]);
                parameters.Add(new(function.Identifiers[i], val));
            }

            switch (function.Type)
            {
                case FunctionType.Full:
                    Value? returned = ExecuteStatementList(function.Statements(), parameters);
                    if (returned == null)
                    {
                        return new Value();
                    }
                    else
                    {
                        return returned;
                    }
                case FunctionType.Simple:
                    _stack.Push();

                    foreach (var p in parameters)
                    {
                        _stack.PutValue(p.Key, p.Value);
                    }
                    Value result = EvalExpression(function.Expression());

                    _stack.Pop();

                    return result;
            }

            throw new InvalidOperationException();
        }

        private Value EvalExpression(Expression expression)
        {
            if (expression.ConstexprValue is not null)
            {
                return new Value(expression.ConstexprValue);
            }

            Value left = EvalRelation(expression.Left!);

            if (expression.Right != null)
            {
                try
                {
                    Value right = EvalRelation(expression.Right);
                    switch (expression.Operator!)
                    {
                        case ExpressionOperator.OR:
                            return left.Or(right);
                        case ExpressionOperator.AND:
                            return left.And(right);
                        case ExpressionOperator.XOR:
                            return left.Xor(right);
                    }
                }
                catch (ValueError e)
                {
                    throw new ExecutionError(expression.Location, e.Message);
                }
            }

            return left;
        }

        private Value EvalRelation(Relation relation)
        {
            Value left = EvalFactor(relation.Left);

            if (relation.Right != null)
            {
                try
                {
                    Value right = EvalFactor(relation.Right);
                    switch (relation.Operator!)
                    {
                        case RelationOperator.LESS:
                            return left.Less(right);
                        case RelationOperator.LESS_EQUAL:
                            return left.LessEqual(right);
                        case RelationOperator.GREATER:
                            return left.Greater(right);
                        case RelationOperator.GREATER_EQUAL:
                            return left.GreaterEqual(right);
                        case RelationOperator.EQUAL:
                            return left.Equal(right);
                        case RelationOperator.NOT_EQUAL:
                            return left.NotEqual(right);
                    }
                }
                catch (ValueError e)
                {
                    throw new ExecutionError(relation.Location, e.Message);
                }
            }

            return left;
        }

        private Value EvalFactor(Factor factor)
        {
            Value left = EvalTerm(factor.Left);

            if (factor.Right != null)
            {
                try
                {
                    Value right = EvalTerm(factor.Right);
                    switch (factor.Operator!)
                    {
                        case FactorOperator.PLUS:
                            return left.Plus(right);
                        case FactorOperator.MINUS:
                            return left.Minus(right);
                    }
                }
                catch (ValueError e)
                {
                    throw new ExecutionError(factor.Location, e.Message);
                }
            }

            return left;
        }

        private Value EvalTerm(Term term)
        {
            Value left = EvalUnary(term.Left);

            if (term.Right != null)
            {
                try
                {
                    Value right = EvalUnary(term.Right);
                    switch (term.Operator!)
                    {
                        case TermOperator.MULTIPLY:
                            return left.Multiply(right);
                        case TermOperator.DIVIDE:
                            return left.Divide(right);
                    }
                }
                catch (ValueError e)
                {
                    throw new ExecutionError(term.Location, e.Message);
                }
            }

            return left;
        }

        private Value EvalUnary(Unary unary)
        {
            switch (unary.Type)
            {
                case UnaryType.Reference:
                    {
                        return ResolveReference(unary.Reference!);
                    }
                case UnaryType.Is:
                    {
                        Value val = ResolveReference(unary.Reference!);
                        Value @true = new(true);
                        Value @false = new(false);
                        switch (val.Type)
                        {
                            case ValueType.Int:
                                if (unary.TypeIndicator == TypeIndicator.INT) return @true; else return @false;
                            case ValueType.Real:
                                if (unary.TypeIndicator == TypeIndicator.REAL) return @true; else return @false;
                            case ValueType.Bool:
                                if (unary.TypeIndicator == TypeIndicator.BOOL) return @true; else return @false;
                            case ValueType.String:
                                if (unary.TypeIndicator == TypeIndicator.STRING) return @true; else return @false;
                            case ValueType.Array:
                                if (unary.TypeIndicator == TypeIndicator.ARRAY) return @true; else return @false;
                            case ValueType.Tuple:
                                if (unary.TypeIndicator == TypeIndicator.TUPLE) return @true; else return @false;
                            case ValueType.Func:
                                if (unary.TypeIndicator == TypeIndicator.FUNC) return @true; else return @false;
                            case ValueType.Empty:
                                if (unary.TypeIndicator == TypeIndicator.EMPTY) return @true; else return @false;
                        }
                        throw new InvalidOperationException();
                    }
                case UnaryType.Primary:
                    {
                        Value val = EvalPrimary(unary.Primary!);

                        if (unary.PrimaryOperator != null)
                        {
                            try
                            {
                                switch (unary.PrimaryOperator!)
                                {
                                    case PrimaryOperator.PLUS:
                                        return val.Plus();
                                    case PrimaryOperator.MINUS:
                                        return val.Minus();
                                    case PrimaryOperator.NOT:
                                        return val.Not();
                                }
                            }
                            catch (ValueError e)
                            {
                                throw new ExecutionError(unary.Location, e.Message);
                            }
                        }


                        return val;
                    }
            }

            throw new InvalidOperationException();
        }

        private Value EvalPrimary(Primary primary)
        {
            return primary.Type switch
            {
                PrimaryType.Literal => EvalLiteral(primary.Literal!),
                PrimaryType.Read => EvalRead((ReadType)primary.Read!, primary.Location),
                PrimaryType.FunctionLiteral => EvalFunctionLiteral(primary.FunctionLiteral!),
                PrimaryType.Expression => EvalExpression(primary.Expression!),
                _ => throw new InvalidOperationException()
            };
        }

        private Value EvalLiteral(Literal literal)
        {
            return literal.Type switch
            {
                LiteralType.Int => new Value((Int128)literal.IntValue!),
                LiteralType.Double => new Value((double)literal.DoubleValue!),
                LiteralType.Bool => new Value((bool)literal.BoolValue!),
                LiteralType.String => new Value(literal.StringValue!),
                LiteralType.Tuple => new Value(CreateTuple(literal.TupleValue!)),
                LiteralType.Array => new Value(CreateArray(literal.ArrayValue!)),
                LiteralType.Empty => new Value(),
                _ => throw new InvalidOperationException(),
            };
        }

        private Tuple CreateTuple(Parsing.AST.Tuple tuple)
        {
            Tuple result = new();
            foreach (var elem in tuple.Elements.Elements)
            {
                Value val = EvalExpression(elem.Expression);
                if (elem.Identifier == null)
                {
                    result.Add(val);
                }
                else
                {
                    result.Add(elem.Identifier!, val);
                }
            }

            return result;
        }

        private Array CreateArray(Parsing.AST.Array array)
        {
            Array result = new();
            foreach (var elem in array.Elements.Elements)
            {
                result.Add(EvalExpression(elem));
            }

            return result;
        }

        private Value EvalRead(ReadType type, LexLocation location)
        {
            string? value = _input.Next();
            if (value == null)
            {
                return new Value();
            }

            string expectedType = "";
            string statementName = "";

            try
            {
                switch (type)
                {
                    case ReadType.INT:
                        expectedType = "int";
                        statementName = "readInt";
                        return new Value(Int128.Parse(value));
                    case ReadType.REAL:
                        expectedType = "real";
                        statementName = "readReal";
                        return new Value(double.Parse(value));
                    case ReadType.STRING:
                        return new Value(value);
                }
            }
            catch (FormatException)
            {
                throw new ExecutionError(location, $"{statementName} expected {expectedType}, got \"{value}\"");
            }
            catch (OverflowException)
            {
                throw new ExecutionError(location, $"overflow on {statementName} ({value})");
            }

            throw new InvalidOperationException();
        }

        private Value EvalFunctionLiteral(FunctionLiteral literal)
        {
            List<string> identifiers = [];
            if (literal.Identifiers != null)
            {
                identifiers.AddRange(literal.Identifiers.Identifiers);
            }

            switch (literal.Body.Type)
            {
                case FunctionBodyType.Full:
                    return new Value(new Function(identifiers, literal.Body.Statements!));
                case FunctionBodyType.Simple:
                    return new Value(new Function(identifiers, literal.Body.Expression!));
            }

            throw new InvalidOperationException();
        }
    }

}
