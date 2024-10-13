namespace DLang.Lexer
{

    internal enum TokenType
    {
        // Keywords
        VAR, FUNCTION, IF, THEN, ELSE, END, WHILE, FOR, IN, LOOP, RETURN, PRINT, IS,

        // Operators and Punctuation
        DECLARE, ASSIGN, PLUS, MINUS, MULTIPLY, DIVIDE, EQUAL, NOT_EQUAL, LESS, LESS_EQUAL, GREATER, GREATER_EQUAL, AND, OR, XOR, NOT,
        DOT, COMMA, SEMICOLON, LPAREN, RPAREN, LBRACKET, RBRACKET, LBRACE, RBRACE, ARROW,

        // Literals
        IDENTIFIER, INTEGER_LITERAL, REAL_LITERAL, BOOLEAN_LITERAL, STRING_LITERAL,

        // Types
        INT, REAL, BOOL, STRING, EMPTY, VECTOR, TUPLE, FUNC,
        // Special
        EOF, RANGE
    }

}