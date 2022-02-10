using System;
using System.Collections.Generic;

namespace Games.Common.IO
{
    internal class Tokenizer
    {
        private const char Quote = '"';
        private const char Separator = ',';

        private readonly Queue<char> _characters;

        private Tokenizer(string input) => _characters = new Queue<char>(input);

        public static IEnumerable<Token> ParseTokens(string input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return new Tokenizer(input).ParseTokens();
        }

        private IEnumerable<Token> ParseTokens()
        {
            while (true)
            {
                var (token, isLastToken) = Consume(_characters);
                yield return token;

                if (isLastToken) { break; }
            }
        }

        public (Token, bool) Consume(Queue<char> characters)
        {
            var token = Token.Create();
            var state = ITokenizerState.LookForStartOfToken;

            while (characters.TryDequeue(out var character))
            {
                (state, token) = state.Consume(character, token);
                if (state is AtEndOfTokenState) { return (token, false); }
            }

            return (token, true);
        }

        private interface ITokenizerState
        {
            public static ITokenizerState LookForStartOfToken { get; } = new LookForStartOfTokenState();

            (ITokenizerState, Token) Consume(char character, Token token);
        }

        private struct LookForStartOfTokenState : ITokenizerState
        {
            public (ITokenizerState, Token) Consume(char character, Token token) =>
                character switch
                {
                    Separator => (new AtEndOfTokenState(), token),
                    Quote => (new InQuotedTokenState(), Token.CreateQuoted()),
                    _ when char.IsWhiteSpace(character) => (this, token),
                    _ => (new InTokenState(), token.Append(character))
                };
        }

        private struct InTokenState : ITokenizerState
        {
            public (ITokenizerState, Token) Consume(char character, Token token) =>
                character == Separator ? (new AtEndOfTokenState(), token) : (this, token.Append(character));
        }

        private struct InQuotedTokenState : ITokenizerState
        {
            public (ITokenizerState, Token) Consume(char character, Token token) =>
                character == Quote ? (new ExpectSeparatorState(), token) : (this, token.Append(character));
        }

        private struct ExpectSeparatorState : ITokenizerState
        {
            public (ITokenizerState, Token) Consume(char character, Token token) =>
                character == Separator ? (new AtEndOfTokenState(), token) : (new IgnoreRestOfLineState(), token);
        }

        private struct IgnoreRestOfLineState : ITokenizerState
        {
            public (ITokenizerState, Token) Consume(char character, Token token) => (this, token);
        }

        private struct AtEndOfTokenState : ITokenizerState
        {
            public (ITokenizerState, Token) Consume(char character, Token token) =>
                throw new InvalidOperationException();
        }
    }
}