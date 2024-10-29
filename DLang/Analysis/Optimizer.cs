using DLang.Parsing.AST;

namespace DLang.Analysis
{

    internal class Optimizer
    {
        private ProgramTree _programTree;

        public Optimizer(ProgramTree programTree)
        {
            _programTree = programTree;
        }

        public void Optimize()
        {
            OptimizeStatementList(_programTree.Statements);
        }

        private static void OptimizeStatementList(StatementList statementList)
        {
            StatementListRemoveUnreachableCode(statementList);

            foreach (var statement in statementList.List)
            {
                OptimizeStatement(statement);
            }
        }

        private static void OptimizeStatement(Statement statement)
        {
            switch (statement.Type)
            {
                case StatementType.Declaration:
                    OptimizeDeclaration(statement.Declaration!);
                    break;
                case StatementType.Assignment:
                    OptimizeAssignment(statement.Assignment!);
                    break;
                case StatementType.Print:
                    OptimizePrint(statement.Print!);
                    break;
                case StatementType.If:
                    OptimizeIf(statement.If!);
                    break;
                case StatementType.Loop:
                    OptimizeLoop(statement.Loop!);
                    break;
                case StatementType.Return:
                    OptimizeReturn(statement.Return!);
                    break;
            }
        }

        private static void OptimizeDeclaration(Declaration declaration)
        {
            foreach (var definition in declaration.Definitions.Definitions)
            {
                Expression? expr = definition.Expression;

                if (expr != null)
                {
                    OptimizeExpression(expr);
                }
            }
        }

        private static void OptimizeAssignment(Assignment assignment)
        {
            OptimizeReference(assignment.Reference);
            OptimizeExpression(assignment.Expression);
        }

        private static void OptimizePrint(Print print)
        {
            foreach (var expression in print.Expressions.Expressions)
            {
                OptimizeExpression(expression);
            }
        }

        private static void OptimizeIf(If @if)
        {
            OptimizeExpression(@if.Expression);
            OptimizeStatementList(@if.Statements);
            if (@if.Tail != null)
            {
                OptimizeStatementList(@if.Tail);
            }
        }

        private static void OptimizeLoop(Loop loop)
        {
            switch (loop.Type)
            {
                case LoopType.While:
                    OptimizeExpression(loop.Expression!);
                    break;
                case LoopType.For:
                    OptimizeExpression(loop.Range!.Left);
                    OptimizeExpression(loop.Range!.Right);
                    break;
            }

            OptimizeStatementList(loop.Statements);
        }

        private static void OptimizeReturn(Return @return)
        {
            if (@return.Expression != null)
            {
                OptimizeExpression(@return.Expression);
            }
        }

        private static void OptimizeExpression(Expression expression)
        {
            ConstexprValue val;
            try
            {
                val = ConstexprValue.EvalExpression(expression);
            }
            catch (ConstexprValueError e)
            {
                throw new OptimizerError(expression.Location, e.Message);
            }

            if (val.Type == ConstexprValueType.NonConstexpr)
            {
                OptimizeRelation(expression.Left);
                if (expression.Right != null)
                {
                    OptimizeRelation(expression.Right);
                }
            }
            else
            {
                expression.ConstexprValue = val;
                expression.Left = null;
            }
        }

        private static void OptimizeRelation(Relation relation)
        {
            OptimizeFactor(relation.Left);
            if (relation.Right != null)
            {
                OptimizeFactor(relation.Right);
            }
        }

        private static void OptimizeFactor(Factor factor)
        {
            OptimizeTerm(factor.Left);
            if (factor.Right != null)
            {
                OptimizeTerm(factor.Right);
            }
        }

        private static void OptimizeTerm(Term term)
        {
            OptimizeUnary(term.Left);
            if (term.Right != null)
            {
                OptimizeUnary(term.Right);
            }
        }

        private static void OptimizeUnary(Unary unary)
        {
            switch (unary.Type)
            {
                case UnaryType.Reference:
                    OptimizeReference(unary.Reference!);
                    break;
                case UnaryType.Is:
                    OptimizeReference(unary.Reference!);
                    break;
                case UnaryType.Primary:
                    OptimizePrimary(unary.Primary!);
                    break;
            }
        }

        private static void OptimizeReference(Reference reference)
        {
            switch (reference.Type)
            {
                case ReferenceType.Identifier:
                    break;
                case ReferenceType.ArrayIndex:
                    OptimizeReference(reference.Subreference!);
                    OptimizeExpression(reference.Expression!);
                    break;
                case ReferenceType.FunctionCall:
                    OptimizeReference(reference.Subreference!);
                    foreach (var expression in reference.Expressions!.Expressions)
                    {
                        OptimizeExpression(expression);
                    }
                    break;
                case ReferenceType.TupleIndex:
                    OptimizeReference(reference.Subreference!);
                    break;
                case ReferenceType.Member:
                    OptimizeReference(reference.Subreference!);
                    break;
            }
        }

        private static void OptimizePrimary(Primary primary)
        {
            switch (primary.Type)
            {
                case PrimaryType.Literal:
                    OptimizeLiteral(primary.Literal!);
                    break;
                case PrimaryType.Read:
                    break;
                case PrimaryType.FunctionLiteral:
                    OptimizeFunctionLiteral(primary.FunctionLiteral!);
                    break;
                case PrimaryType.Expression:
                    OptimizeExpression(primary.Expression!);
                    break;
            }
        }

        private static void OptimizeLiteral(Literal literal)
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
                    OptimizeTuple(literal.TupleValue!);
                    break;
                case LiteralType.Array:
                    OptimizeArray(literal.ArrayValue!);
                    break;
                case LiteralType.Empty:
                    break;
            }
        }

        private static void OptimizeTuple(Parsing.AST.Tuple tuple)
        {
            foreach (var element in tuple.Elements.Elements)
            {
                OptimizeExpression(element.Expression);
            }
        }

        private static void OptimizeArray(Parsing.AST.Array array)
        {
            foreach (var element in array.Elements.Elements)
            {
                OptimizeExpression(element);
            }
        }

        private static void OptimizeFunctionLiteral(FunctionLiteral functionLiteral)
        {
            var body = functionLiteral.Body;
            switch (body.Type)
            {
                case FunctionBodyType.Full:
                    OptimizeStatementList(body.Statements!);
                    break;
                case FunctionBodyType.Simple:
                    OptimizeExpression(body.Expression!);
                    break;
            }
        }

        private static void StatementListRemoveUnreachableCode(StatementList statementList)
        {
            int returnPos = statementList.List.Count;
            for (int i = 0; i < statementList.List.Count; i++)
            {
                Statement statement = statementList.List[i];
                if (statement.Type == StatementType.Return)
                {
                    returnPos = i;
                    break;
                }
            }

            if (returnPos < statementList.List.Count - 1)
            {
                for (int i = statementList.List.Count - 1; i > returnPos; i--)
                {
                    statementList.List.RemoveAt(i);
                }
            }
        }
    }

}
