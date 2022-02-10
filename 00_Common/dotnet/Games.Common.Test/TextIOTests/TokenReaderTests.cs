using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

using static System.Environment;
using TwoStrings = System.ValueTuple<string, string>;

namespace Games.Common.IO
{
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
            var sut = CreateTokenReader("");

            Action readTokens = () => sut.ReadTokens("", 0);

            readTokens.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("'quantityNeeded' must be greater than zero.*")
                .WithParameterName("quantityNeeded");
        }


        [Theory]
        [MemberData(nameof(ReadTokensTestCases))]
        public void ReadTokens_ReadingValuesHasExpectedPromptsAndResults<T>(
            string prompt,
            uint tokenCount,
            string input,
            string expectedOutput,
            T[] expectedResult)
        {
            var sut = CreateTokenReader(input);

            var result = sut.ReadTokens(prompt, tokenCount);
            var output = _outputWriter.ToString();

            using var _ = new AssertionScope();
            output.Should().Be(expectedOutput);
            result.Select(t => t.ToString()).Should().BeEquivalentTo(expectedResult);
        }

        private TokenReader CreateTokenReader(string input) =>
            new TokenReader(
                new TextIO(
                    new StringReader(input + NewLine),
                    _outputWriter));

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
                    $"Foo? ?? ?? ?? !Extra input ingored{NewLine}",
                    new[] { "1", "2", " a,b ", "", "", "d\"x" }
                }
            };
        }

        public static TheoryData<Func<IReadWrite, TwoStrings>, string, string, TwoStrings> Read2StringsTestCases()
        {
            static Func<IReadWrite, TwoStrings> Read2Strings(string prompt) => io => io.Read2Strings(prompt);

            return new()
            {
                { Read2Strings("2 strings"), ",", "2 strings? ", ("", "") },
                { Read2Strings("Input please"), "aBc ,  DeF ", "Input please? ", ("aBc", "DeF") },
            };
        }
    }
}