using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

using static System.Environment;
using static Games.Common.IO.Strings;

namespace Games.Common.IO;

public class TokenReaderTests
{
    private readonly StringWriter _outputWriter;

    public TokenReaderTests()
    {
        _outputWriter = new StringWriter();
    }

    [Fact]
    public void ReadTokens_QuantityNeededZero_ThrowsArgumentException()
    {
        var sut = TokenReader.ForStrings(new TextIO(new StringReader(""), _outputWriter));

        Action readTokens = () => sut.ReadTokens("", 0);

        readTokens.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("'quantityNeeded' must be greater than zero.*")
            .WithParameterName("quantityNeeded");
    }


    [Theory]
    [MemberData(nameof(ReadTokensTestCases))]
    public void ReadTokens_ReadingValuesHasExpectedPromptsAndResults(
        string prompt,
        uint tokenCount,
        string input,
        string expectedOutput,
        string[] expectedResult)
    {
        var sut = TokenReader.ForStrings(new TextIO(new StringReader(input + NewLine), _outputWriter));

        var result = sut.ReadTokens(prompt, tokenCount);
        var output = _outputWriter.ToString();

        using var _ = new AssertionScope();
        output.Should().Be(expectedOutput);
        result.Select(t => t.String).Should().BeEquivalentTo(expectedResult);
    }

    [Theory]
    [MemberData(nameof(ReadNumericTokensTestCases))]
    public void ReadTokens_Numeric_ReadingValuesHasExpectedPromptsAndResults(
        string prompt,
        uint tokenCount,
        string input,
        string expectedOutput,
        float[] expectedResult)
    {
        var sut = TokenReader.ForNumbers(new TextIO(new StringReader(input + NewLine), _outputWriter));

        var result = sut.ReadTokens(prompt, tokenCount);
        var output = _outputWriter.ToString();

        using var _ = new AssertionScope();
        output.Should().Be(expectedOutput);
        result.Select(t => t.Number).Should().BeEquivalentTo(expectedResult);
    }

    public static TheoryData<string, uint, string, string, string[]> ReadTokensTestCases()
    {
        return new()
        {
            { "Name", 1, "Bill", "Name? ", new[] { "Bill" } },
            { "Names", 2, " Bill , Bloggs ", "Names? ", new[] { "Bill", "Bloggs" } },
            { "Names", 2, $" Bill{NewLine}Bloggs ", "Names? ?? ", new[] { "Bill", "Bloggs" } },
            {
                "Foo",
                6,
                $"1,2{NewLine}\" a,b \"{NewLine},\"\"c,d{NewLine}d\"x,e,f",
                $"Foo? ?? ?? ?? {ExtraInput}{NewLine}",
                new[] { "1", "2", " a,b ", "", "", "d\"x" }
            }
        };
    }

    public static TheoryData<string, uint, string, string, float[]> ReadNumericTokensTestCases()
    {
        return new()
        {
            { "Age", 1, "23", "Age? ", new[] { 23F } },
            { "Constants", 2, " 3.141 , 2.71 ", "Constants? ", new[] { 3.141F, 2.71F } },
            { "Answer", 1, $"Forty-two{NewLine}42 ", $"Answer? {NumberExpected}{NewLine}? ", new[] { 42F } },
            {
                "Foo",
                6,
                $"1,2{NewLine}\" a,b \"{NewLine}3, 4  {NewLine}5.6,7,a, b",
                $"Foo? ?? {NumberExpected}{NewLine}? ?? {ExtraInput}{NewLine}",
                new[] { 1, 2, 3, 4, 5.6F, 7 }
            }
        };
    }
}
