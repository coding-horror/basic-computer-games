using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

using TwoStrings = System.ValueTuple<string, string>;
using TwoNumbers = System.ValueTuple<float, float>;
using ThreeNumbers = System.ValueTuple<float, float, float>;
using FourNumbers = System.ValueTuple<float, float, float, float>;

using static System.Environment;
using static Games.Common.IO.Strings;

namespace Games.Common.IO.TextIOTests
{
    public class ReadMethodTests
    {
        [Theory]
        [MemberData(nameof(ReadStringTestCases))]
        [MemberData(nameof(Read2StringsTestCases))]
        [MemberData(nameof(ReadNumberTestCases))]
        [MemberData(nameof(Read2NumbersTestCases))]
        [MemberData(nameof(Read3NumbersTestCases))]
        [MemberData(nameof(Read4NumbersTestCases))]
        [MemberData(nameof(ReadNumbersTestCases))]
        public void ReadingValuesHasExpectedPromptsAndResults<T>(
            Func<IReadWrite, T> read,
            string input,
            string expectedOutput,
            T expectedResult)
        {
            var inputReader = new StringReader(input + Environment.NewLine);
            var outputWriter = new StringWriter();
            var io = new TextIO(inputReader, outputWriter);

            var result = read.Invoke(io);
            var output = outputWriter.ToString();

            using var _ = new AssertionScope();
            output.Should().Be(expectedOutput);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ReadNumbers_ArrayEmpty_ThrowsArgumentException()
        {
            var io = new TextIO(new StringReader(""), new StringWriter());

            Action readNumbers = () => io.ReadNumbers("foo", Array.Empty<float>());

            readNumbers.Should().Throw<ArgumentException>()
                .WithMessage("'values' must have a non-zero length.*")
                .WithParameterName("values");
        }

        public static TheoryData<Func<IReadWrite, string>, string, string, string> ReadStringTestCases()
        {
            static Func<IReadWrite, string> ReadString(string prompt) => io => io.ReadString(prompt);

            return new()
            {
                { ReadString("Name"), "", "Name? ", "" },
                { ReadString("prompt"), " foo  ,bar", $"prompt? {ExtraInput}{NewLine}", "foo" }
            };
        }

        public static TheoryData<Func<IReadWrite, TwoStrings>, string, string, TwoStrings> Read2StringsTestCases()
        {
            static Func<IReadWrite, TwoStrings> Read2Strings(string prompt) => io => io.Read2Strings(prompt);

            return new()
            {
                { Read2Strings("2 strings"), ",", "2 strings? ", ("", "") },
                {
                    Read2Strings("Input please"),
                    $"{NewLine}x,y",
                    $"Input please? ?? {ExtraInput}{NewLine}",
                    ("", "x")
                }
            };
        }

        public static TheoryData<Func<IReadWrite, float>, string, string, float> ReadNumberTestCases()
        {
            static Func<IReadWrite, float> ReadNumber(string prompt) => io => io.ReadNumber(prompt);

            return new()
            {
                { ReadNumber("Age"), $"{NewLine}42,", $"Age? {NumberExpected}{NewLine}? {ExtraInput}{NewLine}", 42 },
                { ReadNumber("Guess"), "3,4,5", $"Guess? {ExtraInput}{NewLine}", 3 }
            };
        }

        public static TheoryData<Func<IReadWrite, TwoNumbers>, string, string, TwoNumbers> Read2NumbersTestCases()
        {
            static Func<IReadWrite, TwoNumbers> Read2Numbers(string prompt) => io => io.Read2Numbers(prompt);

            return new()
            {
                { Read2Numbers("Point"), "3,4,5", $"Point? {ExtraInput}{NewLine}", (3, 4) },
                {
                    Read2Numbers("Foo"),
                    $"x,4,5{NewLine}4,5,x",
                    $"Foo? {NumberExpected}{NewLine}? {ExtraInput}{NewLine}",
                    (4, 5)
                }
            };
        }

        public static TheoryData<Func<IReadWrite, ThreeNumbers>, string, string, ThreeNumbers> Read3NumbersTestCases()
        {
            static Func<IReadWrite, ThreeNumbers> Read3Numbers(string prompt) => io => io.Read3Numbers(prompt);

            return new()
            {
                { Read3Numbers("Point"), "3.2, 4.3, 5.4, 6.5", $"Point? {ExtraInput}{NewLine}", (3.2F, 4.3F, 5.4F) },
                {
                    Read3Numbers("Bar"),
                    $"x,4,5{NewLine}4,5,x{NewLine}6,7,8,y",
                    $"Bar? {NumberExpected}{NewLine}? {NumberExpected}{NewLine}? {ExtraInput}{NewLine}",
                    (6, 7, 8)
                }
            };
        }

        public static TheoryData<Func<IReadWrite, FourNumbers>, string, string, FourNumbers> Read4NumbersTestCases()
        {
            static Func<IReadWrite, FourNumbers> Read4Numbers(string prompt) => io => io.Read4Numbers(prompt);

            return new()
            {
                { Read4Numbers("Point"), "3,4,5,6,7", $"Point? {ExtraInput}{NewLine}", (3, 4, 5, 6) },
                {
                    Read4Numbers("Baz"),
                    $"x,4,5,6{NewLine} 4, 5 , 6,7  ,x",
                    $"Baz? {NumberExpected}{NewLine}? {ExtraInput}{NewLine}",
                    (4, 5, 6, 7)
                }
            };
        }

        public static TheoryData<Func<IReadWrite, IReadOnlyList<float>>, string, string, float[]> ReadNumbersTestCases()
        {
            static Func<IReadWrite, IReadOnlyList<float>> ReadNumbers(string prompt) =>
                io =>
                {
                    var numbers = new float[6];
                    io.ReadNumbers(prompt, numbers);
                    return numbers;
                };

            return new()
            {
                { ReadNumbers("Primes"), "2, 3, 5, 7, 11, 13", $"Primes? ", new float[] { 2, 3, 5, 7, 11, 13 } },
                {
                    ReadNumbers("Qux"),
                    $"42{NewLine}3.141, 2.718{NewLine}3.0e8, 6.02e23{NewLine}9.11E-28",
                    $"Qux? ?? ?? ?? ",
                    new[] { 42, 3.141F, 2.718F, 3.0e8F, 6.02e23F, 9.11E-28F }
                }
            };
        }
    }
}