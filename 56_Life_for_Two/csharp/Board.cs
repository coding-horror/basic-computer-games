namespace LifeforTwo;

internal class Board
{
    private const int PieceMask = 0x1100;
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
}