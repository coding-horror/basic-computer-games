namespace LifeforTwo;

internal class Board
{
    private const int Empty = 0x0000;
    private const int Player1 = 0x0100;
    private const int Player2 = 0x1000;
    private const int PieceMask = Player1 | Player2;
    private const int NeighbourValueOffset = 8;
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