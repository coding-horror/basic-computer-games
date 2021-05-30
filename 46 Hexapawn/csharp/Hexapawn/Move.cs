using static Hexapawn.Pawn;

namespace Hexapawn
{
    /// <summary>
    /// Represents a move which may, or may not, be legal.
    /// </summary>
    internal class Move
    {
        private readonly Cell _from;
        private readonly Cell _to;
        private readonly int _metric;

        public Move(Cell from, Cell to)
        {
            _from = from;
            _to = to;
            _metric = _from - _to;
        }

        public void Deconstruct(out Cell from, out Cell to)
        {
            from = _from;
            to = _to;
        }

        public Cell From => _from;

        // Produces the mirror image of the current moved, reflected around the central column of the board.
        public Move Reflected => (_from.Reflected, _to.Reflected);

        // Allows a tuple of two ints to be implicitly converted to a Move.
        public static implicit operator Move((int From, int To) value) => new(value.From, value.To);

        // Takes floating point coordinates, presumably from keyboard input, and attempts to create a Move object.
        public static bool TryCreate(float input1, float input2, out Move move)
        {
            if (Cell.TryCreate(input1, out var from) &&
                Cell.TryCreate(input2, out var to))
            {
                move = (from, to);
                return true;
            }

            move = default;
            return false;
        }

        public static Move Right(Cell from) => (from, from - 2);
        public static Move Straight(Cell from) => (from, from - 3);
        public static Move Left(Cell from) => (from, from - 4);

        public bool IsStraightMoveToEmptySpace(Board board) => _metric == 3 && board[_to] == None;

        public bool IsLeftDiagonalToCapture(Board board) => _metric == 4 && _from != 7 && board[_to] == Black;

        public bool IsRightDiagonalToCapture(Board board) =>
            _metric == 2 && _from != 9 && _from != 6 && board[_to] == Black;

        public void Execute(Board board)
        {
            board[_to] = board[_from];
            board[_from] = None;
        }

        public override string ToString() => $"from {_from} to {_to}";
    }
}
