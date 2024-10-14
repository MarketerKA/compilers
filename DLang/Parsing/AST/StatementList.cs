namespace DLang.Parsing.AST
{

    internal class StatementList
    {
        public readonly List<Statement> List;

        public StatementList() { List = []; }

        public StatementList(Statement statement) { List = [statement]; }

        public void Add(Statement statement) { List.Add(statement); }
    }

}
