using DLang.Parsing;

namespace DLang.Lexing
{

    internal class Lexer
    {
        private readonly string _input;
        private int _position;
        private char _currentChar;

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
            _currentChar = _input.Length > 0 ? _input[0] : '\0';
        }

        private void Advance()
        {
            _position++;
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
                case "func": return Tokens.FUNCTION;
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
                case "true": return Tokens.BOOLEAN_LITERAL;
                case "false": return Tokens.BOOLEAN_LITERAL;
                default: return Tokens.IDENTIFIER;
            }
        }

        private Token ReadNumber()
        {
            int start = _position;
            bool isReal = false;
            while (_currentChar != '\0')
            {
                if (char.IsDigit(_currentChar))
                {
                    Advance();
                }
                else if (_currentChar == '.')
                {
                    Advance();
                    if (_currentChar == '.')
                    {
                        _position--;
                        break;
                    }
                    isReal = true;
                }
                else if (char.IsLetter(_currentChar))
                {
                    throw new Exception($"Unexpected character after number: {_currentChar}");
                }
                else
                {
                    break;
                }
            }
            string value = _input.Substring(start, _position - start);
            return new Token(isReal ? Tokens.REAL_LITERAL : Tokens.INTEGER_LITERAL, value);
        }

        private Token ReadString()
        {
            Advance();
            int start = _position;
            while (_currentChar != '\0' && _currentChar != '"')
            {
                Advance();
            }
            string value = _input.Substring(start, _position - start);
            Advance();
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
                    throw new Exception($"Unexpected character: {_currentChar}");
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

                throw new Exception($"Unexpected character: {_currentChar}");
            }

            return new Token(Tokens.EOF, string.Empty);
        }
    }
}