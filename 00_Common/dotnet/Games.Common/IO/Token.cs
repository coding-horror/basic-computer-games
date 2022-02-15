using System.Text;
using System.Text.RegularExpressions;

namespace Games.Common.IO;

internal class Token
{
    private static readonly Regex _numberPattern = new(@"^[+\-]?\d*(\.\d*)?([eE][+\-]?\d*)?");

    internal Token(string value)
    {
        String = value;

        var match = _numberPattern.Match(String);

        IsNumber = float.TryParse(match.Value, out var number);
        Number = (IsNumber, number) switch
        {
            (false, _) => float.NaN,
            (true, float.PositiveInfinity) => float.MaxValue,
            (true, float.NegativeInfinity) => float.MinValue,
            (true, _) => number
        };
    }

    public string String { get; }
    public bool IsNumber { get; }
    public float Number { get; }

    public override string ToString() => String;

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
