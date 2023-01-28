namespace Queen;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private readonly Computer _computer;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
        _computer = new Computer(random);
    }

    internal void PlaySeries()
    {
        _io.Write(Streams.Title);
        if (_io.ReadYesNo(Prompts.Instructions)) { _io.Write(Streams.Instructions); }

        while (true)
        {
            var result = PlayGame();
            _io.Write(result switch
            {
                Result.HumanForfeits => Streams.Forfeit,
                Result.HumanWins => Streams.Congratulations,
                Result.ComputerWins => Streams.IWin,
                _ => throw new InvalidOperationException($"Unexpected result {result}")
            });

            if (!_io.ReadYesNo(Prompts.Anyone)) { break; }
        }

        _io.Write(Streams.Thanks);
    }

    private Result PlayGame()
    {
        _io.Write(Streams.Board);
        var humanPosition = _io.ReadPosition(Prompts.Start, p => p.IsStart, Streams.IllegalStart, repeatPrompt: true);
        if (humanPosition.IsZero) { return Result.HumanForfeits; }

        while (true)
        {
            var computerPosition = _computer.GetMove(humanPosition);
            if (computerPosition.IsEnd) { return Result.ComputerWins; }
        }

    }

    private enum Result { ComputerWins, HumanWins, HumanForfeits };
}

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

internal static class IOExtensions
{
    internal static bool ReadYesNo(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var answer = io.ReadString(prompt).ToLower();
            if (answer == "yes") { return true; }
            if (answer == "no") { return false; }

            io.Write(Streams.YesOrNo);
        }
    }

    internal static Position ReadPosition(
        this IReadWrite io,
        string prompt,
        Predicate<Position> isValid,
        Stream error,
        bool repeatPrompt = false)
    {
        while (true)
        {
            var response = io.ReadNumber(prompt);
            var number = (int)response;
            var position = new Position(number);
            if (number == response && (position.IsZero || isValid(position)))
            {
                return position;
            }

            io.Write(error);
            if (!repeatPrompt) { prompt = ""; }
        }
    }
}

internal record struct Position(int Diagonal, int Row)
{
    public static readonly Position Zero = new(0);

    public Position(int number)
        : this(Diagonal: number / 10, Row: number % 10)
    {
    }

    public bool IsZero => Row == 0 && Diagonal == 0;
    public bool IsStart => Row == 1 || Row == Diagonal;
    public bool IsEnd => Row == 8 && Diagonal == 15;

    public override string ToString() => $"{Diagonal}{Row}";

    public static implicit operator Position(int value) => new(value);

    public static Position operator +(Position position, Move move)
        => new(Diagonal: position.Diagonal + move.Diagonal, Row: position.Row + move.Row);
}

internal static class RandomExtensions
{
    internal static Move NextMove(this IRandom random)
        => random.NextFloat() switch
        {
            > 0.6F => Move.Down,
            > 0.3F => Move.DownLeft,
            _ => Move.Left
        };
}

internal record struct Move(int Diagonal, int Row)
{
    public static readonly Move Left = new(1, 0);
    public static readonly Move DownLeft = new(2, 1);
    public static readonly Move Down = new(1, 1);

    public static Move operator *(Move move, int scale) => new(move.Diagonal * scale, move.Row * scale);
}