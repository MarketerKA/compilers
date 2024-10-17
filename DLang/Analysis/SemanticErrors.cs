namespace DLang.Analysis
{

    internal class SemanticErrors : Exception
    {
        public readonly IEnumerable<SemanticError> Errors;

        public SemanticErrors(IEnumerable<SemanticError> errors) : base()
        {
            Errors = errors;
        }
    }

}
