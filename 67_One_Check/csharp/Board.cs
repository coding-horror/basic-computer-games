namespace OneCheck;

internal class Board
{
    private readonly bool[][] _checkers;
    private int _count;

    public Board()
    {
        _checkers = 
            Enumerable.Range(0, 8)
                .Select(r => Enumerable.Range(0, 8)
                    .Select(c => r <= 1 || r >= 6 || c <= 1 || c >= 6).ToArray())
                .ToArray();
        _count = 48;
    }

    private bool this[int index]
    {
        get => _checkers[(index - 1) / 8][(index-1) % 8];
        set => _checkers[(index - 1) / 8][(index-1) % 8] = value;
    }

    public int Count => _count;

    public bool TryMove(Move move)
    {
        if (move.IsInRange && move.IsTwoSpacesDiagonally && IsPieceJumpingPieceToEmptySpace(move))
        {
            this[move.From] = false;
            this[move.Jumped] = false;
            this[move.To] = true;
            _count--;
            return true;
        }

        return false;
    }

    private bool IsPieceJumpingPieceToEmptySpace(Move move) => this[move.From] && this[move.Jumped] && !this[move.To];

    public override string ToString() => 
        string.Join(Environment.NewLine, _checkers.Select(r => string.Join(" ", r.Select(c => c ? " 1" : " 0"))));
}
