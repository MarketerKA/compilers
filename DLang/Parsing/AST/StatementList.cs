using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class StatementList : Locationed
    {
        public readonly List<Statement> List;

        public StatementList(LexLocation location) : base(location) 
        { 
            List = []; 
        }

        public StatementList(LexLocation location, Statement statement) : base(location) 
        { 
            List = [statement]; 
        }

        public void Add(Statement statement) { List.Add(statement); }
    }

}
