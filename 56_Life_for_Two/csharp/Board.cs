using System.Text;

namespace LifeforTwo;

internal class Board
{
    private readonly Piece[,] _cells = new Piece[7, 7];
    private readonly Dictionary<int, int> _cellCounts = 
        new() { [Piece.None] = 0, [Piece.Player1] = 0, [Piece.Player2] = 0 };

    public Piece this[Coordinates coordinates]
    {
        get => this[coordinates.X, coordinates.Y];
        set => this[coordinates.X, coordinates.Y] = value;
    }

    public Piece this[int x, int y]
    {
        get => _cells[x, y];
        set
        {
            if (!_cells[x, y].IsEmpty) { _cellCounts[_cells[x, y]] -= 1; }
            _cells[x, y] = value;
            _cellCounts[value] += 1;
        }
    }

    public int Player1Count => _cellCounts[Piece.Player1];
    public int Player2Count => _cellCounts[Piece.Player2];

    internal bool IsEmptyAt(Coordinates coordinates) => this[coordinates].IsEmpty;

    internal void ClearCell(Coordinates coordinates) => this[coordinates] = Piece.NewNone();
    internal void AddPlayer1Piece(Coordinates coordinates) => this[coordinates] = Piece.NewPlayer1();
    internal void AddPlayer2Piece(Coordinates coordinates) => this[coordinates] = Piece.NewPlayer2();

    public override string ToString()
    {
        var builder = new StringBuilder();

        for (var y = 0; y <= 6; y++)
        {
            builder.AppendLine();
            for (var x = 0; x <= 6; x++)
            {
                builder.Append(GetCellDisplay(x, y));
            }
        }

        return builder.ToString();
    }

    private string GetCellDisplay(int x, int y) =>
        (x, y) switch
        {
            (0 or 6, _) => $" {y % 6} ",
            (_, 0 or 6) => $" {x % 6} ",
            _ => $" {this[x, y]} "
        };
}
