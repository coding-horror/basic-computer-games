namespace LifeforTwo;

internal class Board
{
    private const int Player1Piece = 100;
    private const int Player2Piece = 1000;
    private const int Player1Neighbour = 1;
    private const int Player2Neighbour = 10;
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
                if (this[coordinates] >= Player1Piece)
                {
                    int _playerPiece = this[coordinates] > Player2Piece ? Player2Neighbour : Player1Neighbour;
                    foreach (var neighbour in coordinates.GetNeighbors())
                    {
                        this[neighbour] += _playerPiece;
                    }
                }
            }
        }
    }
}