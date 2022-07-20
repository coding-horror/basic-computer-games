using System.Text;

namespace Chomp;

internal class Cookie
{
    private readonly int _rowCount;
    private readonly int _columnCount;
    private readonly char[][] _bits;

    public Cookie(int rowCount, int columnCount)
    {
        _rowCount = rowCount;
        _columnCount = columnCount;

        // The calls to Math.Max here are to duplicate the original behaviour
        // when negative values are given for the row or column count.
        _bits = new char[Math.Max(_rowCount, 1)][];
        for (int row = 0; row < _bits.Length; row++)
        {
            _bits[row] = Enumerable.Repeat('*', Math.Max(_columnCount, 1)).ToArray();
        }
        _bits[0][0] = 'P';
    }

    public bool TryChomp(int row, int column, out char chomped)
    {
        if (row < 1 || row > _rowCount || column < 1 || column > _columnCount || _bits[row - 1][column - 1] == ' ')
        {
            chomped = default;
            return false;
        }

        chomped = _bits[row - 1][column - 1];

        for (int r = row; r <= _rowCount; r++)
        {
            for (int c = column; c <= _columnCount; c++)
            {
                _bits[r - 1][c - 1] = ' ';
            }
        }

        return true;
    }

    public override string ToString()
    {
        var builder = new StringBuilder().AppendLine("       1 2 3 4 5 6 7 8 9");
        for (int row = 1; row <= _bits.Length; row++)
        {
            builder.Append(' ').Append(row).Append("     ").AppendLine(string.Join(' ', _bits[row - 1]));
        }
        return builder.ToString();
    }
}