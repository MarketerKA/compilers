using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLang.Parsing
{
    internal class ParsingError : Exception
    {
        public ParsingError(string message) : base(message) { }

        public ParsingError(string message, Exception innerException) : base(message, innerException) { }
    }
}
