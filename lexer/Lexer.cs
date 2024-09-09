public class Lexer
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

    private TokenType GetKeywordType(string value)
    {
        switch (value)
        {
            case "var": return TokenType.VAR;
            case "func": return TokenType.FUNCTION;
            case "if": return TokenType.IF;
            case "then": return TokenType.THEN;
            case "else": return TokenType.ELSE;
            case "end": return TokenType.END;
            case "while": return TokenType.WHILE;
            case "for": return TokenType.FOR;
            case "in": return TokenType.IN;
            case "loop": return TokenType.LOOP;
            case "return": return TokenType.RETURN;
            case "print": return TokenType.PRINT;
            case "is": return TokenType.IS;
            case "int": return TokenType.INT;
            case "real": return TokenType.REAL;
            case "bool": return TokenType.BOOL;
            case "string": return TokenType.STRING;
            case "empty": return TokenType.EMPTY;
            default: return TokenType.IDENTIFIER;
        }
    }

    private Token ReadNumber()
    {
        int start = _position;
        bool isReal = false;
        while (_currentChar != '\0' && (char.IsDigit(_currentChar) || _currentChar == '.'))
        {
            if (_currentChar == '.')
            {
                Advance();
                if (_currentChar == '.')
                {
                    _position--;
                    break;
                }
                isReal = true;
            }

            if (!char.IsDigit(_currentChar))
            {
                throw new Exception($"Unexpected character after '.': {_currentChar}");
            }
            isReal = true;
            Advance();
        }
        string value = _input.Substring(start, _position - start);
        return new Token(isReal ? TokenType.REAL_LITERAL : TokenType.INTEGER_LITERAL, value);
    }

    private Token ReadString()
    {
        Advance(); // Skip the opening quote
        int start = _position;
        while (_currentChar != '\0' && _currentChar != '"')
        {
            Advance();
        }
        string value = _input.Substring(start, _position - start);
        Advance(); // Skip the closing quote
        return new Token(TokenType.STRING_LITERAL, value);
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
                return new Token(TokenType.DIVIDE, "/");
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
                    return new Token(TokenType.ASSIGN, ":=");
                }
                throw new Exception($"Unexpected character: {_currentChar}");
            }

            if (_currentChar == '.')
            {
                Advance();
                if (_currentChar == '.')
                {
                    Advance();
                    return new Token(TokenType.RANGE, "..");
                }
                return new Token(TokenType.DOT, ".");
            }

            switch (_currentChar)
            {
                case '+': Advance(); return new Token(TokenType.PLUS, "+");
                case '-': Advance(); return new Token(TokenType.MINUS, "-");
                case '*': Advance(); return new Token(TokenType.MULTIPLY, "*");
                case '=': Advance(); return new Token(TokenType.EQUAL, "=");
                case '<':
                    Advance();
                    if (_currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.LESS_EQUAL, "<=");
                    }
                    return new Token(TokenType.LESS, "<");
                case '>':
                    Advance();
                    if (_currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.GREATER_EQUAL, ">=");
                    }
                    return new Token(TokenType.GREATER, ">");
                case '!':
                    Advance();
                    if (_currentChar == '=')
                    {
                        Advance();
                        return new Token(TokenType.NOT_EQUAL, "!=");
                    }
                    return new Token(TokenType.NOT, "!");
                case '&':
                    Advance();
                    if (_currentChar == '&')
                    {
                        Advance();
                        return new Token(TokenType.AND, "&&");
                    }
                    break;
                case '|':
                    Advance();
                    if (_currentChar == '|')
                    {
                        Advance();
                        return new Token(TokenType.OR, "||");
                    }
                    break;
                case '^': Advance(); return new Token(TokenType.XOR, "^");
                case ',': Advance(); return new Token(TokenType.COMMA, ",");
                case ';': Advance(); return new Token(TokenType.SEMICOLON, ";");
                case '(': Advance(); return new Token(TokenType.LPAREN, "(");
                case ')': Advance(); return new Token(TokenType.RPAREN, ")");
                case '[': Advance(); return new Token(TokenType.LBRACKET, "[");
                case ']': Advance(); return new Token(TokenType.RBRACKET, "]");
                case '{': Advance(); return new Token(TokenType.LBRACE, "{");
                case '}': Advance(); return new Token(TokenType.RBRACE, "}");
            }

            throw new Exception($"Unexpected character: {_currentChar}");
        }

        return new Token(TokenType.EOF, string.Empty);
    }
}
