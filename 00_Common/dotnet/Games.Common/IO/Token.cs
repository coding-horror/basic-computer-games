using System.Text;

namespace Games.Common.IO
{
    internal class Token
    {
        protected readonly StringBuilder _builder;
        private int _trailingWhiteSpaceCount;

        private Token()
        {
            _builder = new StringBuilder();
        }

        public Token Append(char character)
        {
            _builder.Append(character);

            _trailingWhiteSpaceCount = char.IsWhiteSpace(character) ? _trailingWhiteSpaceCount + 1 : 0;

            return this;
        }

        public override string ToString() => _builder.ToString(0, _builder.Length - _trailingWhiteSpaceCount);

        public static Token Create() => new();

        public static Token CreateQuoted() => new QuotedToken();

        public static implicit operator string(Token token) => token.ToString();

        internal class QuotedToken : Token
        {
            public override string ToString() => _builder.ToString();
        }
    }
}