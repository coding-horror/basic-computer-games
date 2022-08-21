namespace LifeforTwo;

internal class Board
{
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
}