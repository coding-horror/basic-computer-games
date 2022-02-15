using System.Linq;
using FluentAssertions;
using Xunit;

namespace Games.Common.IO;

public class TokenizerTests
{
    [Theory]
    [MemberData(nameof(TokenizerTestCases))]
    public void ParseTokens_SplitsStringIntoExpectedTokens(string input, string[] expected)
    {
        var result = Tokenizer.ParseTokens(input);

        result.Select(t => t.ToString()).Should().BeEquivalentTo(expected);
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
        { "\"a\"bc,de", new[] { "a" } },
        { "a\"b,\" c,d\", f ,,g", new[] { "a\"b", " c,d", "f", "", "g" } }
    };
}
