using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUT.Gppg;
using DLang.Lexing;
using DLang.Parsing.AST;

namespace DLang.Parsing
{
    internal class Scanner : AbstractScanner<ValueType, LexLocation>
    {
        private Lexer _lexer;
        private bool _print;

        public Scanner(Lexer lexer, bool print = false)
        {
            _lexer = lexer;
            _print = print;
        }

        public override int yylex()
        {
            Token token = _lexer.GetNextToken();
            if (_print)
            {
                Console.WriteLine(token.ToString());
            }
            yylval = new()
            {
                Token = token
            };
            return (int)token.Type;
        }
    }
}
