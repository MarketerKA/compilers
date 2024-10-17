using DLang.Parsing.AST;

namespace DLang.Analysis
{

    internal class SemanticAnalyzer
    {
        private readonly ProgramTree _programTree;
        private readonly NamespaceStack _namespaceStack;
        private readonly List<SemanticError> _errors;

        public SemanticAnalyzer(ProgramTree programTree)
        {
            _programTree = programTree;
            _namespaceStack = new();
            _errors = [];
        }

        public void Analyze()
        {
            AnalyzeStatementList(_programTree.Statements);
            if (_errors.Count > 0)
            {
                throw new SemanticErrors(_errors);
            }
        }

        private void AnalyzeStatementList(StatementList statementList, IEnumerable<string>? addNames = null, bool isFunctionBody = false)
        {
            _namespaceStack.Push(isFunctionBody);

            if (addNames != null)
            {
                foreach (string name in addNames)
                {
                    _namespaceStack.Add(name);
                }
            }

            foreach (var statement in statementList.List)
            {
                AnalyzeStatement(statement);
            }
            _namespaceStack.Pop();
        }

        private void AnalyzeStatement(Statement statement)
        {
            switch (statement.Type)
            {
                case StatementType.Declaration:
                    AnalyzeDeclaration(statement.Declaration!);
                    break;
                case StatementType.Assignment:
                    AnalyzeAssignment(statement.Assignment!);
                    break;
                case StatementType.Print:
                    AnalyzePrint(statement.Print!);
                    break;
                case StatementType.If:
                    AnalyzeIf(statement.If!);
                    break;
                case StatementType.Loop:
                    AnalyzeLoop(statement.Loop!);
                    break;
                case StatementType.Return:
                    AnalyzeReturn(statement.Return!);
                    break;
            }
        }

        private void AnalyzeDeclaration(Declaration declaration)
        {
            foreach (var definition in declaration.Definitions.Definitions)
            {
                string name = definition.Identifier;
                Expression? expr = definition.Expression;

                if (_namespaceStack.NameExistsInCurrentNamespace(name))
                {
                    _errors.Add(new SemanticError(declaration.Location, $"redefinition of \"{name}\""));
                }

                _namespaceStack.Add(name);

                if (expr != null)
                {
                    AnalyzeExpression(expr);
                }
            }
        }

        private void AnalyzeAssignment(Assignment assignment)
        {
            AnalyzeReference(assignment.Reference);
            AnalyzeExpression(assignment.Expression);
        }

        private void AnalyzePrint(Print print)
        {
            foreach (var expression in print.Expressions.Expressions)
            {
                AnalyzeExpression(expression);
            }
        }

        private void AnalyzeIf(If @if)
        {
            AnalyzeExpression(@if.Expression);
            AnalyzeStatementList(@if.Statements);
            if (@if.Tail != null)
            {
                AnalyzeStatementList(@if.Tail);
            }
        }

        private void AnalyzeLoop(Loop loop)
        {
            switch (loop.Type)
            {
                case LoopType.While:
                    AnalyzeExpression(loop.Expression!);
                    break;
                case LoopType.For:
                    AnalyzeExpression(loop.Range!.Left);
                    AnalyzeExpression(loop.Range!.Right);
                    break;
            }

            AnalyzeStatementList(loop.Statements,
                loop.Identifier != null ? [loop.Identifier] : null);
        }

        private void AnalyzeReturn(Return @return)
        {
            if (!_namespaceStack.IsFunctionBody())
            {
                _errors.Add(new SemanticError(@return.Location, "\"return\" keyword used outside of function body"));
            }

            if (@return.Expression != null)
            {
                AnalyzeExpression(@return.Expression);
            }
        }

        private void AnalyzeExpression(Expression expression, IEnumerable<string>? addNames = null, bool isFunctionBody = false)
        {
            if (addNames != null)
            {
                _namespaceStack.Push(isFunctionBody);

                foreach (string name in addNames)
                {
                    _namespaceStack.Add(name);
                }
            }

            AnalyzeRelation(expression.Left);
            if (expression.Right != null)
            {
                AnalyzeRelation(expression.Right);
            }

            if (addNames != null)
            {
                _namespaceStack.Pop();
            }
        }

        private void AnalyzeRelation(Relation relation)
        {
            AnalyzeFactor(relation.Left);
            if (relation.Right != null)
            {
                AnalyzeFactor(relation.Right);
            }
        }

        private void AnalyzeFactor(Factor factor)
        {
            AnalyzeTerm(factor.Left);
            if (factor.Right != null)
            {
                AnalyzeTerm(factor.Right);
            }
        }

        private void AnalyzeTerm(Term term)
        {
            AnalyzeUnary(term.Left);
            if (term.Right != null)
            {
                AnalyzeUnary(term.Right);
            }
        }

        private void AnalyzeUnary(Unary unary)
        {
            switch (unary.Type)
            {
                case UnaryType.Reference:
                    AnalyzeReference(unary.Reference!);
                    break;
                case UnaryType.Is:
                    AnalyzeReference(unary.Reference!);
                    break;
                case UnaryType.Primary:
                    AnalyzePrimary(unary.Primary!);
                    break;
            }
        }

        private void AnalyzeReference(Reference reference)
        {
            switch (reference.Type)
            {
                case ReferenceType.Identifier:
                    if (!_namespaceStack.NameExists(reference.Identifier!))
                    {
                        _errors.Add(new SemanticError(reference.Location, $"reference to undeclared identifier \"{reference.Identifier}\""));
                    }
                    break;
                case ReferenceType.ArrayIndex:
                    AnalyzeReference(reference.Subreference!);
                    AnalyzeExpression(reference.Expression!);
                    break;
                case ReferenceType.FunctionCall:
                    AnalyzeReference(reference.Subreference!);
                    foreach (var expression in reference.Expressions!.Expressions)
                    {
                        AnalyzeExpression(expression);
                    }
                    break;
                case ReferenceType.TupleIndex:
                    AnalyzeReference(reference.Subreference!);
                    break;
                case ReferenceType.Member:
                    AnalyzeReference(reference.Subreference!);
                    break;
            }
        }

        private void AnalyzePrimary(Primary primary)
        {
            switch (primary.Type)
            {
                case PrimaryType.Literal:
                    AnalyzeLiteral(primary.Literal!);
                    break;
                case PrimaryType.Read:
                    break;
                case PrimaryType.FunctionLiteral:
                    AnalyzeFunctionLiteral(primary.FunctionLiteral!);
                    break;
                case PrimaryType.Expression:
                    AnalyzeExpression(primary.Expression!);
                    break;
            }
        }

        private void AnalyzeLiteral(Literal literal)
        {
            switch (literal.Type)
            {
                case LiteralType.Int:
                    break;
                case LiteralType.Double:
                    break;
                case LiteralType.Bool:
                    break;
                case LiteralType.String:
                    break;
                case LiteralType.Tuple:
                    AnalyzeTuple(literal.TupleValue!);
                    break;
                case LiteralType.Array:
                    AnalyzeArray(literal.ArrayValue!);
                    break;
                case LiteralType.Empty:
                    break;
            }
        }

        void AnalyzeTuple(Parsing.AST.Tuple tuple)
        {
            foreach (var element in tuple.Elements.Elements)
            {
                AnalyzeExpression(element.Expression);
            }
        }

        void AnalyzeArray(Parsing.AST.Array array)
        {
            foreach (var element in array.Elements.Elements)
            {
                AnalyzeExpression(element);
            }
        }

        private void AnalyzeFunctionLiteral(FunctionLiteral functionLiteral)
        {
            var body = functionLiteral.Body;
            IEnumerable<string>? names = functionLiteral.Identifiers?.Identifiers;
            switch (body.Type)
            {
                case FunctionBodyType.Full:
                    AnalyzeStatementList(body.Statements!, names, true);
                    break;
                case FunctionBodyType.Simple:
                    AnalyzeExpression(body.Expression!, names, true);
                    break;
            }
        }
    }

}