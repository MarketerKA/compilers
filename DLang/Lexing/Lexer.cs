using DLang.Parsing;
using QUT.Gppg;
using System.Text.RegularExpressions;

namespace DLang.Lexing
{

    internal class Lexer
    {
        private readonly string _input;
        private int _position;
        private char _currentChar;
        private int _line;
        private int _col;

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
            _currentChar = _input.Length > 0 ? _input[0] : '\0';
            _line = 1;
            _col = 1;
        }

        private LexLocation GetLocation()
        {
            return new LexLocation(_line, _col, _line, _col);
        }

        private void Advance()
        {
            if (_currentChar == '\n')
            {
                _line++;
                _col = 0;
            }

            _position++;
            _col++;
            _currentChar = _position < _input.Length ? _input[_position] : '\0';
        }

        private void SkipWhitespace()
        {
            while (_currentChar != '\0' && char.IsWhiteSpace(_currentChar))
            {
                Advance();
            }
        }

        private void SkipComment()
        {
            while (_currentChar != '\0' && _currentChar != '\n')
            {
                Advance();
            }
        }

        private Token ReadIdentifier()
        {
            int start = _position;
            while (_currentChar != '\0' && (char.IsLetterOrDigit(_currentChar) || _currentChar == '_'))
            {
                Advance();
            }
            string value = _input.Substring(start, _position - start);
            return new Token(GetKeywordType(value), value);
        }
        

        private Tokens GetKeywordType(string value)
        {
            switch (value)
            {
                case "var": return Tokens.VAR;
                case "func": return Tokens.FUNC;
                case "if": return Tokens.IF;
                case "then": return Tokens.THEN;
                case "else": return Tokens.ELSE;
                case "end": return Tokens.END;
                case "while": return Tokens.WHILE;
                case "for": return Tokens.FOR;
                case "in": return Tokens.IN;
                case "loop": return Tokens.LOOP;
                case "return": return Tokens.RETURN;
                case "print": return Tokens.PRINT;
                case "is": return Tokens.IS;
                case "int": return Tokens.INT;
                case "real": return Tokens.REAL;
                case "bool": return Tokens.BOOL;
                case "string": return Tokens.STRING;
                case "empty": return Tokens.EMPTY;
                case "and": return Tokens.AND;
                case "or": return Tokens.OR;
                case "xor": return Tokens.XOR;
                case "not": return Tokens.NOT;
                case "true": return Tokens.BOOLEAN_LITERAL;
                case "false": return Tokens.BOOLEAN_LITERAL;
                case "readInt": return Tokens.READ_INT;
                case "readReal": return Tokens.READ_REAL;
                case "readString": return Tokens.READ_STRING;
                default: return Tokens.IDENTIFIER;
            }
        }

        private Token ReadNumber()
        {
            int start = _position;

            while (_currentChar != '\0' && char.IsDigit(_currentChar))
            {
                Advance();
            }

            if (_currentChar == '.')
            {
                Advance();
                
                if (char.IsDigit(_currentChar))
                {
                    while (_currentChar != '\0' && char.IsDigit(_currentChar))
                    {
                        Advance();
                    }

                    string realValue = _input.Substring(start, _position - start);
                    return new Token(Tokens.REAL_LITERAL, realValue);
                }
                else
                {
                    _position--;
                    _currentChar = '.';
                }
            }

            string intValue = _input.Substring(start, _position - start);
            return new Token(Tokens.INTEGER_LITERAL, intValue);
        }


        private Token ReadString()
        {
            Advance();
            int start = _position;
            bool escaped = false;
            while (true)
            {
                if (_currentChar == '\0' || (_currentChar == '"' && !escaped))
                {
                    break;
                }

                if (!escaped)
                {
                    escaped = _currentChar == '\\';
                }
                else
                {
                    escaped = false;
                }

                Advance();
            }
            string value = _input.Substring(start, _position - start);
            Advance();

            try
            {
                value = Regex.Unescape(value);
            }
            catch (ArgumentException)
            {
                throw new LexerError(GetLocation(), $"string literal \"{value}\" contains invalid escape sequence(s)");
            }

            return new Token(Tokens.STRING_LITERAL, value);
        }

        public Token GetNextToken()
        {
            while (_currentChar != '\0')
            {
                if (char.IsWhiteSpace(_currentChar))
                {
                    SkipWhitespace();
                    continue;
                }

                if (_currentChar == '/')
                {
                    Advance();
                    if (_currentChar == '/')
                    {
                        SkipComment();
                        continue;
                    }

                    if (_currentChar == '=')
                    {
                        return new Token(Tokens.NOT_EQUAL, "/=");
                    }
                    return new Token(Tokens.DIVIDE, "/");
                }

                if (char.IsLetter(_currentChar) || _currentChar == '_')
                {
                    return ReadIdentifier();
                }

                if (char.IsDigit(_currentChar))
                {
                    return ReadNumber();
                }

                if (_currentChar == '"')
                {
                    return ReadString();
                }

                if (_currentChar == ':')
                {
                    Advance();
                    if (_currentChar == '=')
                    {
                        Advance();
                        return new Token(Tokens.ASSIGN, ":=");
                    }
                    throw new LexerError(GetLocation(), $"unexpected character: {_currentChar}");
                }

                if (_currentChar == '=')
                {
                    Advance();
                    if (_currentChar == '>')
                    {
                        Advance();
                        return new Token(Tokens.ARROW, "=>");
                    }
                    return new Token(Tokens.EQUAL, "=");
                }

                if (_currentChar == '.')
                {
                    Advance();
                    if (_currentChar == '.')
                    {
                        Advance();
                        return new Token(Tokens.RANGE, "..");
                    }
                    return new Token(Tokens.DOT, ".");
                }

                switch (_currentChar)
                {
                    case '+': Advance(); return new Token(Tokens.PLUS, "+");
                    case '-': Advance(); return new Token(Tokens.MINUS, "-");
                    case '*': Advance(); return new Token(Tokens.MULTIPLY, "*");
                    case '<':
                        Advance();
                        if (_currentChar == '=')
                        {
                            Advance();
                            return new Token(Tokens.LESS_EQUAL, "<=");
                        }
                        return new Token(Tokens.LESS, "<");
                    case '>':
                        Advance();
                        if (_currentChar == '=')
                        {
                            Advance();
                            return new Token(Tokens.GREATER_EQUAL, ">=");
                        }
                        return new Token(Tokens.GREATER, ">");
                    case ',': Advance(); return new Token(Tokens.COMMA, ",");
                    case ';': Advance(); return new Token(Tokens.SEMICOLON, ";");
                    case '(': Advance(); return new Token(Tokens.LPAREN, "(");
                    case ')': Advance(); return new Token(Tokens.RPAREN, ")");
                    case '[': Advance(); return new Token(Tokens.LBRACKET, "[");
                    case ']': Advance(); return new Token(Tokens.RBRACKET, "]");
                    case '{': Advance(); return new Token(Tokens.LBRACE, "{");
                    case '}': Advance(); return new Token(Tokens.RBRACE, "}");
                }

                throw new LexerError(GetLocation(), $"unexpected character: {_currentChar}");
            }

            return new Token(Tokens.EOF, string.Empty);
        }

        public int GetCurrentLine() { return _line; }

        public int GetCurrentCol() { return _col; }
    }
}