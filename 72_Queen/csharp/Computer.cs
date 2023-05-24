namespace Queen;

internal class Computer
{
    private static readonly HashSet<Position> _randomiseFrom = new() { 41, 44, 73, 75, 126, 127 };
    private static readonly HashSet<Position> _desirable = new() { 73, 75, 126, 127, 158 };
    private readonly IRandom _random;

    public Computer(IRandom random)
    {
        _random = random;
    }

    public Position GetMove(Position from)
        => from + (_randomiseFrom.Contains(from) ? _random.NextMove() : FindMove(from));

    private Move FindMove(Position from)
    {
        for (int i = 7; i > 0; i--)
        {
            if (IsOptimal(Move.Left, out var move)) { return move; }
            if (IsOptimal(Move.Down, out move)) { return move; }
            if (IsOptimal(Move.DownLeft, out move)) { return move; }

            bool IsOptimal(Move direction, out Move move)
            {
                move = direction * i;
                return _desirable.Contains(from + move);
            }
        }

        return _random.NextMove();
    }
}
