namespace OneCheck;

internal class Board
{
    private readonly bool[][] _checkers;
    private int _pieceCount;
    private int _moveCount;

    public Board()
    {
        _checkers = 
            Enumerable.Range(0, 8)
                .Select(r => Enumerable.Range(0, 8)
                    .Select(c => r <= 1 || r >= 6 || c <= 1 || c >= 6).ToArray())
                .ToArray();
        _pieceCount = 48;
    }

    private bool this[int index]
    {
        get => _checkers[index / 8][index % 8];
        set => _checkers[index / 8][index % 8] = value;
    }

    public bool PlayMove(IReadWrite io)
    {
        while (true)
        {
            var from = (int)io.ReadNumber(Prompts.From);
            if (from == 0) { return false; }

            var move = new Move { From = from - 1, To = (int)io.ReadNumber(Prompts.To) - 1 };

            if (TryMove(move)) 
            { 
                _moveCount++;
                return true; 
            }

            io.Write(Streams.IllegalMove);
        }
    }

    public bool TryMove(Move move)
    {
        if (move.IsInRange && move.IsTwoSpacesDiagonally && IsPieceJumpingPieceToEmptySpace(move))
        {
            this[move.From] = false;
            this[move.Jumped] = false;
            this[move.To] = true;
            _pieceCount--;
            return true;
        }

        return false;
    }

    private bool IsPieceJumpingPieceToEmptySpace(Move move) => this[move.From] && this[move.Jumped] && !this[move.To];

    public string GetReport() => string.Format(Formats.Results, _moveCount, _pieceCount);

    public override string ToString() => 
        string.Join(Environment.NewLine, _checkers.Select(r => string.Join(" ", r.Select(c => c ? " 1" : " 0"))));
}
