using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Games.Common.IO;

/// <inheritdoc />
/// <summary>
/// Implements <see cref="IReadWrite" /> with input read from a <see cref="TextReader" /> and output written to a
/// <see cref="TextWriter" />.
/// </summary>
/// <remarks>
/// This implementation reproduces the Vintage BASIC input experience, prompting multiple times when partial input
/// supplied, rejecting non-numeric input as needed, warning about extra input being ignored, etc.
/// </remarks>
public class TextIO : IReadWrite
{
    private readonly TextReader _input;
    private readonly TextWriter _output;
    private readonly TokenReader _stringTokenReader;
    private readonly TokenReader _numberTokenReader;

    public TextIO(TextReader input, TextWriter output)
    {
        _input = input ?? throw new ArgumentNullException(nameof(input));
        _output = output ?? throw new ArgumentNullException(nameof(output));
        _stringTokenReader = TokenReader.ForStrings(this);
        _numberTokenReader = TokenReader.ForNumbers(this);
    }

    public virtual char ReadCharacter()
    {
        while(true)
        {
            var ch = _input.Read();
            if (ch != -1) { return (char)ch; }
        }
    }

    public float ReadNumber(string prompt) => ReadNumbers(prompt, 1)[0];

    public (float, float) Read2Numbers(string prompt)
    {
        var numbers = ReadNumbers(prompt, 2);
        return (numbers[0], numbers[1]);
    }

    public (float, float, float) Read3Numbers(string prompt)
    {
        var numbers = ReadNumbers(prompt, 3);
        return (numbers[0], numbers[1], numbers[2]);
    }

    public (float, float, float, float) Read4Numbers(string prompt)
    {
        var numbers = ReadNumbers(prompt, 4);
        return (numbers[0], numbers[1], numbers[2], numbers[3]);
    }

    public void ReadNumbers(string prompt, float[] values)
    {
        if (values.Length == 0)
        {
            throw new ArgumentException($"'{nameof(values)}' must have a non-zero length.", nameof(values));
        }

        var numbers = _numberTokenReader.ReadTokens(prompt, (uint)values.Length).Select(t => t.Number).ToArray();
        numbers.CopyTo(values.AsSpan());
    }

    private IReadOnlyList<float> ReadNumbers(string prompt, uint quantity) =>
        (quantity > 0)
            ? _numberTokenReader.ReadTokens(prompt, quantity).Select(t => t.Number).ToList()
            : throw new ArgumentOutOfRangeException(
                nameof(quantity),
                $"'{nameof(quantity)}' must be greater than zero.");

    public string ReadString(string prompt)
    {
        return ReadStrings(prompt, 1)[0];
    }

    public (string, string) Read2Strings(string prompt)
    {
        var values = ReadStrings(prompt, 2);
        return (values[0], values[1]);
    }

    private IReadOnlyList<string> ReadStrings(string prompt, uint quantityRequired) =>
        _stringTokenReader.ReadTokens(prompt, quantityRequired).Select(t => t.String).ToList();

    internal string ReadLine(string prompt)
    {
        Write(prompt + "? ");
        return _input.ReadLine();
    }

    public void Write(string value) => _output.Write(value);

    public void WriteLine(string value = "") => _output.WriteLine(value);

    public void Write(float value) => _output.Write(GetString(value));

    public void WriteLine(float value) => _output.WriteLine(GetString(value));

    public void Write(object value) => _output.Write(value.ToString());

    public void WriteLine(object value) => _output.WriteLine(value.ToString());

    public void Write(string format, params object[] values) => _output.Write(format, values);

    public void WriteLine(string format, params object[] values) => _output.WriteLine(format, values);

    public void Write(Stream stream, bool keepOpen = false)
    {
        using var reader = new StreamReader(stream);
        while (!reader.EndOfStream)
        {
            _output.WriteLine(reader.ReadLine());
        }

        if (!keepOpen) { stream?.Dispose(); }
    }

    private string GetString(float value) => value < 0 ? $"{value} " : $" {value} ";
}
