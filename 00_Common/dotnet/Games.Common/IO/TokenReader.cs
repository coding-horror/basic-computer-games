using System;
using System.Collections.Generic;

namespace Games.Common.IO
{
    internal class TokenReader
    {
        private const string NumberExpected = "!Number expected - retry input line";
        private const string ExtraInput = "!Extra input ignored";

        private readonly TextIO _io;
        private readonly Predicate<Token> _isTokenValid;

        private TokenReader(TextIO io, Predicate<Token> isTokenValid)
        {
            _io = io;
            _isTokenValid = isTokenValid ?? (t => true);
        }

        public static TokenReader ForStrings(TextIO io) => new(io, t => true);
        public static TokenReader ForNumbers(TextIO io) => new(io, t => t.IsNumber);

        public IEnumerable<Token> ReadTokens(string prompt, uint quantityNeeded)
        {
            if (quantityNeeded == 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantityNeeded),
                    $"'{nameof(quantityNeeded)}' must be greater than zero.");
            }

            var tokens = new List<Token>();

            while(tokens.Count < quantityNeeded)
            {
                tokens.AddRange(ReadValidTokens(prompt, quantityNeeded - (uint)tokens.Count));
                prompt = "?";
            }

            return tokens;
        }

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
}