using DLang.Parsing;

namespace DLang.Lexing
{

    internal class Token
    {
        public Tokens Type { get; }
        public string Value { get; }

        public Token(Tokens type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"Token({Type}, {Value})";
        }
    }

}