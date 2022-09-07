using System.Collections.Immutable;

namespace LifeforTwo;

internal class Board
{
    private const int Empty = 0x0000;
    private const int Player1 = 0x0100;
    private const int Player2 = 0x1000;
    private const int PieceMask = Player1 | Player2;
    private const int NeighbourValueOffset = 8;

    private readonly ImmutableHashSet<int> _willBePlayer1 = 
        new[] { 0x0003, 0x0102, 0x0103, 0x0120, 0x0130, 0x0121, 0x0112, 0x0111, 0x0012 }.ToImmutableHashSet();
    private readonly ImmutableHashSet<int> _willBePlayer2 = 
        new[] { 0x0021, 0x0030, 0x1020, 0x1030, 0x1011, 0x1021, 0x1003, 0x1002, 0x1012 }.ToImmutableHashSet();

    private readonly int[,] _cells = new int[7,7];

    public int this[Coordinates coordinates]
    {
        get => _cells[coordinates.X, coordinates.Y];
        set => _cells[coordinates.X, coordinates.Y] = value;
    }

    public int this[int x, int y]
    {
        get => _cells[x, y];
        set => _cells[x, y] = value;
    }

    public (int Player1Count, int Player2Count) CalculateNextGeneration()
    {
        var _cellCounts = new Dictionary<int, int>() { [Empty] = 0, [Player1] = 0, [Player2] = 0 };

        for (var x = 1; x <= 5; x++)
        {
            for (var y = 1; y <= 5; y++)
            {
                var currentValue = this[x, y];
                var newValue = currentValue switch
                {
                    _ when _willBePlayer1.Contains(currentValue) => Player1,
                    _ when _willBePlayer2.Contains(currentValue) => Player2,
                    _ => Empty
                };

                this[x, y] = newValue;
                _cellCounts[newValue]++;
            }
        }

        return (_cellCounts[Player1], _cellCounts[Player2]);
    }

    public void CalculateNeighbours()
    {
        for (var x = 1; x <= 5; x++)
        {
            for (var y = 1; y <= 5; y++)
            {
                var coordinates = new Coordinates(x, y);
                var neighbourValue = (this[coordinates] & PieceMask) >> NeighbourValueOffset;
                if (neighbourValue > 0)
                {
                    foreach (var neighbour in coordinates.GetNeighbors())
                    {
                        this[neighbour] += neighbourValue;
                    }
                }
            }
        }
    }

    public void Display(IReadWrite io)
    {
        for (var y = 0; y <= 6; y++)
        {
            io.WriteLine();
            for (var x = 0; x <= 6; x++)
            {
                io.Write(GetDisplay(x, y));
            }
        }
    }

    private string GetDisplay(int x, int y) =>
        (x, y, this[x, y]) switch
        {
            (0 or 6, _, _) => $" {y % 6} ",
            (_, 0 or 6, _) => $" {x % 6} ",
            (_, _, Empty) => "   ",
            (_, _, Player1) => " * ",
            (_, _, Player2) => " # ",
            _ => throw new InvalidOperationException($"Unexpected cell value at ({x}, {y}): {this[x, y]}")
        };
}