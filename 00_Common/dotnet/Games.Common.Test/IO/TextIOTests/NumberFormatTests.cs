using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Games.Common.IO.TextIOTests;

public class NumberFormatTests
{
    [Theory]
    [MemberData(nameof(WriteFloatTestCases))]
    public void Write_Float_FormatsNumberSameAsBasic(float value, string basicString)
    {
        var outputWriter = new StringWriter();
        var io = new TextIO(new StringReader(""), outputWriter);

        io.Write(value);

        outputWriter.ToString().Should().BeEquivalentTo(basicString);
    }

    [Theory]
    [MemberData(nameof(WriteFloatTestCases))]
    public void WriteLine_Float_FormatsNumberSameAsBasic(float value, string basicString)
    {
        var outputWriter = new StringWriter();
        var io = new TextIO(new StringReader(""), outputWriter);

        io.WriteLine(value);

        outputWriter.ToString().Should().BeEquivalentTo(basicString + Environment.NewLine);
    }

    public static TheoryData<float, string> WriteFloatTestCases()
        => new()
        {
            { 1000F, " 1000 " },
            { 3.1415927F, " 3.1415927 " },
            { 1F, " 1 " },
            { 0F, " 0 " },
            { -1F, "-1 " },
            { -3.1415927F, "-3.1415927 " },
            { -1000F, "-1000 " },
        };

    [Theory]
    [MemberData(nameof(WriteIntTestCases))]
    public void Write_Int_FormatsNumberSameAsBasic(int value, string basicString)
    {
        var outputWriter = new StringWriter();
        var io = new TextIO(new StringReader(""), outputWriter);

        io.Write(value);

        outputWriter.ToString().Should().BeEquivalentTo(basicString);
    }

    [Theory]
    [MemberData(nameof(WriteIntTestCases))]
    public void WriteLine_Int_FormatsNumberSameAsBasic(int value, string basicString)
    {
        var outputWriter = new StringWriter();
        var io = new TextIO(new StringReader(""), outputWriter);

        io.WriteLine(value);

        outputWriter.ToString().Should().BeEquivalentTo(basicString + Environment.NewLine);
    }

    public static TheoryData<int, string> WriteIntTestCases()
        => new()
        {
            { 1000, " 1000 " },
            { 1, " 1 " },
            { 0, " 0 " },
            { -1, "-1 " },
            { -1000, "-1000 " },
        };
}
