using FluentAssertions;
using Xunit;

namespace Games.Common.IO
{
    public class TokenizerTests
    {
        [Theory]
        [MemberData(nameof(TokenizerTestCases))]
        public void ParseTokens_SplitsStringIntoExpectedTokens(string input, string[] expected)
        {
            var result = Tokenizer.ParseTokens(input);

            result.Should().BeEquivalentTo(expected);
        }

        public static TheoryData<string, string[]> TokenizerTestCases() => new()
        {
            { "", new[] { "" } },
            { "aBc", new[] { "aBc" } },
            { "  Foo   ", new[] { "Foo" } },
            { "  \" Foo  \"  ", new[] { " Foo  " } },
            { "  \" Foo    ", new[] { " Foo    " } },
            { "\"\"abc", new[] { "" } },
            { "a\"\"bc", new[] { "a\"\"bc" } },
            { "\"\"", new[] { "" } },
            { ",", new[] { "", "" } },
            { " foo  ,bar", new[] { "foo", "bar" } },
            { "\"\"bc,de", new[] { "", "de" } },
            { "a\"b,\" c,d\"e, f ,,g", new[] { "a\"b", " c,d", "f", "", "g" } }
        };
    }
}