using System.Text;

namespace Bounce;

/// <summary>
/// Provides support for plotting a graph of height vs time, and rendering it to a string.
/// </summary>
internal class Graph
{
    private readonly Dictionary<int, Row> _rows;

    public Graph(float maxHeight, float timeIncrement)
    {
        // 1 row == 1/2 foot + 1 row for zero
        var rowCount = 2 * (int)Math.Round(maxHeight, MidpointRounding.AwayFromZero) + 1;
        _rows = Enumerable.Range(0, rowCount)
            .ToDictionary(x => x, x => new Row(x % 2 == 0 ? $" {x / 2} " : ""));
        TimeIncrement = timeIncrement;
    }

    public float TimeIncrement { get; }
    public float MaxTimePlotted { get; private set; }

    public void Plot(float time, float height)
    {
        var rowIndex = (int)Math.Round(height * 2, MidpointRounding.AwayFromZero);
        var colIndex = (int)(time / TimeIncrement) + 1;
        if (_rows.TryGetValue(rowIndex, out var row))
        {
            row[colIndex] = '0';
        }
        MaxTimePlotted = Math.Max(time, MaxTimePlotted);
    }

    public override string ToString()
    {
        var sb = new StringBuilder().AppendLine("Feet").AppendLine();
        foreach (var (_, row) in _rows.OrderByDescending(x => x.Key))
        {
            sb.Append(row).AppendLine();
        }
        sb.Append(new Axis(MaxTimePlotted, TimeIncrement));

        return sb.ToString();
    }

    internal class Row
    {
        public const int Width = 70;

        private readonly char[] _chars = new char[Width + 2];
        private int nextColumn = 0;

        public Row(string label)
        {
            Array.Fill(_chars, ' ');
            Array.Copy(label.ToCharArray(), _chars, label.Length);
            nextColumn = label.Length;
        }

        public char this[int column]
        {
            set
            {
                if (column >= _chars.Length) { return; }
                if (column < nextColumn) { column = nextColumn; }
                _chars[column] = value;
                nextColumn = column + 1;
            }
        }

        public override string ToString() => new string(_chars);
    }

    internal class Axis
    {
        private readonly int _maxTimeMark;
        private readonly float _timeIncrement;
        private readonly Labels _labels;

        internal Axis(float maxTimePlotted, float timeIncrement)
        {
            _maxTimeMark = (int)Math.Ceiling(maxTimePlotted);
            _timeIncrement = timeIncrement;

            _labels = new Labels();
            for (var i = 1; i <= _maxTimeMark; i++)
            {
                _labels.Add((int)(i / _timeIncrement), $" {i} ");
            }
        }

        public override string ToString()
            => new StringBuilder()
                .Append(' ').Append('.', (int)(_maxTimeMark / _timeIncrement) + 1).AppendLine()
                .Append(_labels).AppendLine()
                .Append(' ', (int)(_maxTimeMark / _timeIncrement / 2 - 2)).AppendLine("Seconds")
                .ToString();
    }

    internal class Labels : Row
    {
        public Labels()
            : base(" 0")
        {
        }

        public void Add(int column, string label)
        {
            for (var i = 0; i < label.Length; i++)
            {
                this[column + i] = label[i];
            }
        }
    }
}
