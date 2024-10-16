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
        private string _filename;
        public override LexLocation yylloc { get; set; }

        public Scanner(Lexer lexer, string filename, bool print = false)
        {
            _lexer = lexer;
            _print = print;
            _filename = filename;
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
            string position = $"{_filename}:{yylloc.EndLine}:{yylloc.EndColumn}: ";
            throw new ParsingError(position + string.Format(format, args));
        }
    }
}
