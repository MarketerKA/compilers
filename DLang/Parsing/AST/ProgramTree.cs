using QUT.Gppg;

namespace DLang.Parsing.AST
{

    internal class ProgramTree : Locationed
    {
        public readonly StatementList Statements;

        public ProgramTree(LexLocation location, StatementList statements) : base(location)
        {
            Statements = statements;
        }
    }

}
