using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUT.Gppg;
using DLang.Lexing;

namespace DLang.Parsing
{
    internal class Scanner : AbstractScanner<ValueType, LexLocation>
    {
        private Lexer _lexer;

        public Scanner(Lexer lexer)
        {
            _lexer = lexer;
        }

        public override int yylex()
        {
            Token token = _lexer.GetNextToken();
            yylval = new()
            {
                Token = token
            };
            return (int)token.Type;
        }
    }
}
