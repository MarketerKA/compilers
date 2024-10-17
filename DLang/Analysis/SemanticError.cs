using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLang.Analysis
{
    internal class SemanticError : Exception
    {
        public SemanticError(string message) : base(message) { }

        public SemanticError(string message, Exception innerException) : base(message, innerException) { }
    }
}
