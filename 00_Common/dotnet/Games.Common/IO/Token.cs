using System.Text;

namespace Games.Common.IO
{
    internal class Token
    {
        private readonly string _value;

        private Token(string value)
        {
            _value = value;
        }

        public override string ToString() => _value;

        internal class Builder
        {
            private readonly StringBuilder _builder = new();
            private bool _isQuoted;
            private int _trailingWhiteSpaceCount;

            public Builder Append(char character)
            {
                _builder.Append(character);

                _trailingWhiteSpaceCount = char.IsWhiteSpace(character) ? _trailingWhiteSpaceCount + 1 : 0;

                return this;
            }

            public Builder SetIsQuoted()
            {
                _isQuoted = true;
                return this;
            }

            public Token Build()
            {
                if (!_isQuoted) { _builder.Length -= _trailingWhiteSpaceCount; }
                return new Token(_builder.ToString());
            }
        }
    }
}