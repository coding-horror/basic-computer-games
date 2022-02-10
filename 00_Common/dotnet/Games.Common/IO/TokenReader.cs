using System;
using System.Collections.Generic;

namespace Games.Common.IO
{
    internal class TokenReader
    {
        private readonly TextIO _io;
        private readonly Func<Token, bool> _isTokenValid;

        public TokenReader(TextIO io, Func<Token, bool>? isTokenValid = null)
        {
            _io = io;
            _isTokenValid = isTokenValid ?? (t => true);
        }

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
                        tokensValid = false;
                        prompt = "?";
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
                    _io.WriteLine("!Extra input ingored");
                    break;
                }

                yield return token;
            }
        }
    }
}