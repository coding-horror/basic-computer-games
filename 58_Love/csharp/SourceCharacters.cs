using System;

namespace Love
{
    internal class SourceCharacters
    {
        private readonly int _lineLength;
        private readonly char[][] _chars;
        private int _currentRow;
        private int _currentIndex;

        public SourceCharacters(int lineLength, string message)
        {
            _lineLength = lineLength;
            _chars = new[] { new char[lineLength], new char[lineLength] };

            for (int i = 0; i < lineLength; i++)
            {
                _chars[0][i] = message[i % message.Length];
                _chars[1][i] = ' ';
            }
        }

        public ReadOnlySpan<char> GetCharacters(int count)
        {
            var span = new ReadOnlySpan<char>(_chars[_currentRow], _currentIndex, count);

            _currentRow = 1 - _currentRow;
            _currentIndex += count;
            if (_currentIndex >= _lineLength)
            {
                _currentIndex = _currentRow = 0;
            }

            return span;
        }
    }
}
