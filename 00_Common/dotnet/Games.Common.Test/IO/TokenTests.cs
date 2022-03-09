using FluentAssertions;
using Xunit;

namespace Games.Common.IO;

public class TokenTests
{
    [Theory]
    [MemberData(nameof(TokenTestCases))]
    public void Ctor_PopulatesProperties(string value, bool isNumber, float number)
    {
        var expected = new { String = value, IsNumber = isNumber, Number = number };

        var token = new Token(value);

        token.Should().BeEquivalentTo(expected);
    }

    public static TheoryData<string, bool, float> TokenTestCases() => new()
    {
        { "", false, float.NaN },
        { "abcde", false, float.NaN },
        { "123  ", true, 123 },
        { "+42  ", true, 42 },
        { "-42  ", true, -42 },
        { "+3.14159  ", true, 3.14159F },
        { "-3.14159  ", true, -3.14159F },
        { "   123", false, float.NaN },
        { "1.2e4", true, 12000 },
        { "2.3e-5", true, 0.000023F },
        { "1e100", true, float.MaxValue },
        { "-1E100", true, float.MinValue },
        { "1E-100", true, 0 },
        { "-1e-100", true, 0 },
        { "100abc", true, 100 },
        { "1,2,3", true, 1 },
        { "42,a,b", true, 42 },
        { "1.2.3", true, 1.2F },
        { "12e.5", false, float.NaN },
        { "12e0.5", true, 12 }
    };
}
