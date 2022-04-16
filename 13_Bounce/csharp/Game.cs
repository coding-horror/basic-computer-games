using System.Text;
using static Bounce.Resources.Resource;

namespace Bounce;

internal class Game
{
    private const int _chartWidth = 70;
    private const float _acceleration = -32; // feet/s^2

    private readonly IReadWrite _io;

    public Game(IReadWrite io)
    {
        _io = io;
    }

    public void Play()
    {
        _io.Write(Streams.Title);
        _io.Write(Streams.Instructions);

        var increment = _io.ReadParameter("Time increment (sec)");
        var velocity = _io.ReadParameter("Velocity (fps)");
        var elasticity = _io.ReadParameter("Coefficient");

        var timeToFirstBounce = -2 * velocity / _acceleration;
        var bounceCount = (int)(Graph.Row.Width * increment / timeToFirstBounce);

        var maxHeight = (float)Math.Round(-velocity * velocity / 2 / _acceleration, MidpointRounding.AwayFromZero);

        var graph = new Graph(maxHeight, increment);

        var totalTime = 0f;
        for (var bounce = 0; bounce < bounceCount; bounce++, velocity *= elasticity)
        {
            var bounceDuration = -2 * velocity / _acceleration;
            for (var time = 0f; time <= bounceDuration; totalTime += increment, time += increment)
            {
                var height = velocity * time + _acceleration * time * time / 2;
                graph.Plot(totalTime, height);
            }
        }

        _io.WriteLine(graph);
    }
}

internal class Graph
{
    private readonly Dictionary<int, Row> _rows;
    private readonly float _increment;
    private float _maxTimePlotted;

    public Graph(float maxHeight, float increment)
    {
        // 1 row == 1/2 foot + 1 row for zero
        var rowCount = 2 * (int)Math.Round(maxHeight, MidpointRounding.AwayFromZero) + 1;
        _rows = Enumerable.Range(0, rowCount)
            .ToDictionary(x => x, x => new Row(x % 2 == 0 ? $" {x / 2} " : ""));
        _increment = increment;
    }

    public void Plot(float time, float height)
    {
        var rowIndex = (int)Math.Round(height * 2, MidpointRounding.AwayFromZero);
        var colIndex = (int)(time / _increment) + 1;
        if (_rows.TryGetValue(rowIndex, out var row))
        {
            row[colIndex] = '0';
        }
        _maxTimePlotted = Math.Max(time, _maxTimePlotted);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var (_, row) in _rows.OrderByDescending(x => x.Key))
        {
            sb.Append(row).AppendLine();
        }
        var maxTimeMark = (int)Math.Ceiling(_maxTimePlotted);
        sb.Append(' ').Append('.', (int)(maxTimeMark/_increment) + 1).AppendLine();

        var timeLabels = new Labels();
        for (var i = 1; i <= maxTimeMark; i++)
        {
            timeLabels.Add((int)(i / _increment), $" {i} ");
        }
        sb.Append(timeLabels).AppendLine();
        sb.Append(' ', (int)((int)(_maxTimePlotted + 1)/_increment/2 - 2)).AppendLine("Seconds");

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

internal static class IReadWriteExtensions
{
    internal static float ReadParameter(this IReadWrite io, string parameter)
    {
        var value = io.ReadNumber(parameter);
        io.WriteLine();
        return value;
    }
}