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
        public override LexLocation yylloc { get; set; }

        public Scanner(Lexer lexer, bool print = false)
        {
            _lexer = lexer;
            _print = print;
            yylloc = new LexLocation(1, 1, 1, 1);
        }

        public override int yylex()
        {
            int sl = _lexer.GetCurrentLine();
            int sc = _lexer.GetCurrentCol();

            Token token = _lexer.GetNextToken();

            int el = _lexer.GetCurrentLine();
            int ec = _lexer.GetCurrentCol();

            yylloc = new(sl, sc, el, ec);
          
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

        public override void yyerror(string format, params object[] args)
        {
            throw new ParsingError(string.Format(format, args));
        }
    }
}
