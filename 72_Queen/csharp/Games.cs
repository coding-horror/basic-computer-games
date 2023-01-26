namespace Queen;

internal class Games
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Games(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    internal void Play()
    {
        _io.Write(Streams.Title);
        if (_io.ReadYesNo(Prompts.Instructions)) { _io.Write(Streams.Instructions); }

        while (true)
        {
            PlayGame();

            if (!_io.ReadYesNo(Prompts.Anyone))
            {
                _io.Write(Streams.Thanks);
                return;
            }
        }
    }

    internal void PlayGame()
    {
        _io.Write(Streams.Board);
        var humanPosition = _io.ReadPosition(Prompts.Start, p => p.IsStart, Streams.IllegalStart, repeatPrompt: true)
        if (humanPosition.IsZero)
        {
            _io.Write(Streams.Forfeit);
            return;
        }


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
}