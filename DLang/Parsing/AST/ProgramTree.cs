namespace DLang.Parsing.AST
{

    internal class ProgramTree
    {
        public readonly StatementList Statements;

        public ProgramTree(StatementList statements)
        {
            Statements = statements;
        }
    }

}
