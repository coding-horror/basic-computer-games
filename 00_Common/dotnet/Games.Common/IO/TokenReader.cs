using System;
using System.Collections.Generic;
using System.Linq;
using static Games.Common.IO.Strings;

namespace Games.Common.IO;

/// <summary>
/// Reads from input and assembles a given number of values, or tokens, possibly over a number of input lines.
/// </summary>
internal class TokenReader
{
    private readonly TextIO _io;
    private readonly Predicate<Token> _isTokenValid;

    private TokenReader(TextIO io, Predicate<Token> isTokenValid)
    {
        _io = io;
        _isTokenValid = isTokenValid ?? (t => true);
    }

    /// <summary>
    /// Creates a <see cref="TokenReader" /> which reads string tokens.
    /// </summary>
    /// <param name="io">A <see cref="TextIO" /> instance.</param>
    /// <returns>The new <see cref="TokenReader" /> instance.</returns>
    public static TokenReader ForStrings(TextIO io) => new(io, t => true);

    /// <summary>
    /// Creates a <see cref="TokenReader" /> which reads tokens and validates that they can be parsed as numbers.
    /// </summary>
    /// <param name="io">A <see cref="TextIO" /> instance.</param>
    /// <returns>The new <see cref="TokenReader" /> instance.</returns>
    public static TokenReader ForNumbers(TextIO io) => new(io, t => t.IsNumber);

    /// <summary>
    /// Reads valid tokens from one or more input lines and builds a list with the required quantity.
    /// </summary>
    /// <param name="prompt">The string used to prompt the user for input.</param>
    /// <param name="quantityNeeded">The number of tokens required.</param>
    /// <returns>The sequence of tokens read.</returns>
    public IEnumerable<Token> ReadTokens(string prompt, uint quantityNeeded)
    {
        if (quantityNeeded == 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantityNeeded),
                $"'{nameof(quantityNeeded)}' must be greater than zero.");
        }

        var tokens = new List<Token>();

        while (tokens.Count < quantityNeeded)
        {
            tokens.AddRange(ReadValidTokens(prompt, quantityNeeded - (uint)tokens.Count));
            prompt = "?";
        }

        return tokens;
    }

    /// <summary>
    /// Reads a line of tokens, up to <paramref name="maxCount" />, and rejects the line if any are invalid.
    /// </summary>
    /// <param name="prompt">The string used to prompt the user for input.</param>
    /// <param name="maxCount">The maximum number of tokens to read.</param>
    /// <returns>The sequence of tokens read.</returns>
    private IEnumerable<Token> ReadValidTokens(string prompt, uint maxCount)
    {
        while (true)
        {
            var tokensValid = true;
            var tokens = new List<Token>();
            foreach (var token in ReadLineOfTokens(prompt, maxCount))
            {
                if (!_isTokenValid(token))
                {
                    _io.WriteLine(NumberExpected);
                    tokensValid = false;
                    prompt = "";
                    break;
                }

                tokens.Add(token);
            }

            if (tokensValid) { return tokens; }
        }
    }

    /// <summary>
    /// Lazily reads up to <paramref name="maxCount" /> tokens from an input line.
    /// </summary>
    /// <param name="prompt">The string used to prompt the user for input.</param>
    /// <param name="maxCount">The maximum number of tokens to read.</param>
    /// <returns></returns>
    private IEnumerable<Token> ReadLineOfTokens(string prompt, uint maxCount)
    {
        var tokenCount = 0;

        foreach (var token in Tokenizer.ParseTokens(_io.ReadLine(prompt)))
        {
            if (++tokenCount > maxCount)
            {
                _io.WriteLine(ExtraInput);
                break;
            }

            yield return token;
        }
    }
}
