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
            var tokenBuilder = new Token.Builder();
            var state = ITokenizerState.LookForStartOfToken;

            while (characters.TryDequeue(out var character))
            {
                (state, tokenBuilder) = state.Consume(character, tokenBuilder);
                if (state is AtEndOfTokenState) { return (tokenBuilder.Build(), false); }
            }

            return (tokenBuilder.Build(), true);
        }

        private interface ITokenizerState
        {
            public static ITokenizerState LookForStartOfToken { get; } = new LookForStartOfTokenState();

            (ITokenizerState, Token.Builder) Consume(char character, Token.Builder tokenBuilder);
        }

        private struct LookForStartOfTokenState : ITokenizerState
        {
            public (ITokenizerState, Token.Builder) Consume(char character, Token.Builder tokenBuilder) =>
                character switch
                {
                    Separator => (new AtEndOfTokenState(), tokenBuilder),
                    Quote => (new InQuotedTokenState(), tokenBuilder.SetIsQuoted()),
                    _ when char.IsWhiteSpace(character) => (this, tokenBuilder),
                    _ => (new InTokenState(), tokenBuilder.Append(character))
                };
        }

        private struct InTokenState : ITokenizerState
        {
            public (ITokenizerState, Token.Builder) Consume(char character, Token.Builder tokenBuilder) =>
                character == Separator
                    ? (new AtEndOfTokenState(), tokenBuilder)
                    : (this, tokenBuilder.Append(character));
        }

        private struct InQuotedTokenState : ITokenizerState
        {
            public (ITokenizerState, Token.Builder) Consume(char character, Token.Builder tokenBuilder) =>
                character == Quote
                    ? (new ExpectSeparatorState(), tokenBuilder)
                    : (this, tokenBuilder.Append(character));
        }

        private struct ExpectSeparatorState : ITokenizerState
        {
            public (ITokenizerState, Token.Builder) Consume(char character, Token.Builder tokenBuilder) =>
                character == Separator
                    ? (new AtEndOfTokenState(), tokenBuilder)
                    : (new IgnoreRestOfLineState(), tokenBuilder);
        }

        private struct IgnoreRestOfLineState : ITokenizerState
        {
            public (ITokenizerState, Token.Builder) Consume(char character, Token.Builder tokenBuilder) =>
                (this, tokenBuilder);
        }

        private struct AtEndOfTokenState : ITokenizerState
        {
            public (ITokenizerState, Token.Builder) Consume(char character, Token.Builder tokenBuilder) =>
                throw new InvalidOperationException();
        }
    }
}