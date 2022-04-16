using Games.Common.Numbers;

namespace Games.Common.IO;

/// <summary>
/// Provides for input and output of strings and numbers.
/// </summary>
public interface IReadWrite
{
    /// <summary>
    /// Reads a character from input.
    /// </summary>
    /// <returns>The character read.</returns>
    char ReadCharacter();

    /// <summary>
    /// Reads a <see cref="float" /> value from input.
    /// </summary>
    /// <param name="prompt">The text to display to prompt for the value.</param>
    /// <returns>A <see cref="float" />, being the value entered.</returns>
    float ReadNumber(string prompt);

    /// <summary>
    /// Reads 2 <see cref="float" /> values from input.
    /// </summary>
    /// <param name="prompt">The text to display to prompt for the values.</param>
    /// <returns>A <see cref="ValueTuple{float, float}" />, being the values entered.</returns>
    (float, float) Read2Numbers(string prompt);

    /// <summary>
    /// Reads 3 <see cref="float" /> values from input.
    /// </summary>
    /// <param name="prompt">The text to display to prompt for the values.</param>
    /// <returns>A <see cref="ValueTuple{float, float, float}" />, being the values entered.</returns>
    (float, float, float) Read3Numbers(string prompt);

    /// <summary>
    /// Reads 4 <see cref="float" /> values from input.
    /// </summary>
    /// <param name="prompt">The text to display to prompt for the values.</param>
    /// <returns>A <see cref="ValueTuple{float, float, float, float}" />, being the values entered.</returns>
    (float, float, float, float) Read4Numbers(string prompt);

    /// <summary>
    /// Read numbers from input to fill an array.
    /// </summary>
    /// <param name="prompt">The text to display to prompt for the values.</param>
    /// <param name="values">A <see cref="float[]" /> to be filled with values from input.</param>
    void ReadNumbers(string prompt, float[] values);

    /// <summary>
    /// Reads a <see cref="string" /> value from input.
    /// </summary>
    /// <param name="prompt">The text to display to prompt for the value.</param>
    /// <returns>A <see cref="string" />, being the value entered.</returns>
    string ReadString(string prompt);

    /// <summary>
    /// Reads 2 <see cref="string" /> values from input.
    /// </summary>
    /// <param name="prompt">The text to display to prompt for the values.</param>
    /// <returns>A <see cref="ValueTuple{string, string}" />, being the values entered.</returns>
    (string, string) Read2Strings(string prompt);

    /// <summary>
    /// Writes a <see cref="string" /> to output.
    /// </summary>
    /// <param name="message">The <see cref="string" /> to be written.</param>
    void Write(string message);

    /// <summary>
    /// Writes a <see cref="string" /> to output, followed by a new-line.
    /// </summary>
    /// <param name="message">The <see cref="string" /> to be written.</param>
    void WriteLine(string message = "");

    /// <summary>
    /// Writes a <see cref="Number" /> to output.
    /// </summary>
    /// <param name="value">The <see cref="Number" /> to be written.</param>
    void Write(Number value);

    /// <summary>
    /// Writes a <see cref="Number" /> to output.
    /// </summary>
    /// <param name="value">The <see cref="Number" /> to be written.</param>
    void WriteLine(Number value);

    /// <summary>
    /// Writes an <see cref="object" /> to output.
    /// </summary>
    /// <param name="value">The <see cref="object" /> to be written.</param>
    void Write(object value);

    /// <summary>
    /// Writes an <see cref="object" /> to output.
    /// </summary>
    /// <param name="value">The <see cref="object" /> to be written.</param>
    void WriteLine(object value);

    /// <summary>
    /// Writes a formatted string to output.
    /// </summary>
    /// <param name="format">The format <see cref="string" /> to be written.</param>
    /// <param name="value">The values to be inserted into the format.</param>
    void Write(string format, params object[] values);

    /// <summary>
    /// Writes a formatted string to output followed by a new-line.
    /// </summary>
    /// <param name="format">The format <see cref="string" /> to be written.</param>
    /// <param name="value">The values to be inserted into the format.</param>
    void WriteLine(string format, params object[] values);

    /// <summary>
    /// Writes the contents of a <see cref="Stream" /> to output.
    /// </summary>
    /// <param name="stream">The <see cref="Stream" /> to be written.</param>
    void Write(Stream stream, bool keepOpen = false);
}
