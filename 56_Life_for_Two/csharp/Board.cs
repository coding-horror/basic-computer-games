namespace LifeforTwo;

internal class Board
{
    private readonly Piece[,] _cells = new Piece[7,7];

    private readonly Dictionary<int, int> _cellCounts = new();

    public Piece this[Coordinates coordinates]
    {
        get => _cells[coordinates.X, coordinates.Y];
        set => _cells[coordinates.X, coordinates.Y] = value;
    }

    public Piece this[int x, int y]
    {
        get => _cells[x, y];
        set => _cells[x, y] = value;
    }

    public int Player1Count => _cellCounts[Piece.Player1];
    public int Player2Count => _cellCounts[Piece.Player2];

    internal bool IsEmptyAt(Coordinates coordinates) => this[coordinates].IsEmpty;

    public string? Result => 
        (Player1Count, Player2Count) switch
        {
            (0, 0) => Strings.Draw,
            (_, 0) => string.Format(Formats.Winner, 1),
            (0, _) => string.Format(Formats.Winner, 2),
            _ => null
        };

    internal void ClearCell(Coordinates coordinates) => this[coordinates] = Piece.NewEmpty();

    internal void AddPlayer1Piece(Coordinates coordinates) => this[coordinates] = Piece.NewPlayer1();

    internal void AddPlayer2Piece(Coordinates coordinates) => this[coordinates] = Piece.NewPlayer2();

    public void CalculateNextGeneration()
    {
        _cellCounts[Piece.None] = _cellCounts[Piece.Player1] = _cellCounts[Piece.Player2] = 0;

        for (var x = 1; x <= 5; x++)
        {
            for (var y = 1; y <= 5; y++)
            {
                this[x, y] = this[x, y].GetNext();
                _cellCounts[this[x, y].Value]++;
            }
        }

        CountNeighbours();
    }

    public void CountNeighbours()
    {
        for (var x = 1; x <= 5; x++)
        {
            for (var y = 1; y <= 5; y++)
            {
                var coordinates = new Coordinates(x, y);
                var piece = this[coordinates];
                if (!piece.IsEmpty)
                {
                    foreach (var neighbour in coordinates.GetNeighbors())
                    {
                        this[neighbour] = this[neighbour].AddNeighbour(piece);
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
        (x, y) switch
        {
            (0 or 6, _) => $" {y % 6} ",
            (_, 0 or 6) => $" {x % 6} ",
            _ => $" {this[x, y]} "
        };
}
