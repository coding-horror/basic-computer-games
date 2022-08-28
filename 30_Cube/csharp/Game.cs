namespace Cube;

internal class Game
{
    private const int _initialBalance = 500;
    private readonly IEnumerable<(int, int, int)> _seeds = new List<(int, int, int)>
    {
        (3, 2, 3), (1, 3, 3), (3, 3, 2), (3, 2, 3), (3, 1, 3)
    };
    private readonly (float, float, float) _startLocation = (1, 1, 1);
    private readonly (float, float, float) _goalLocation = (3, 3, 3);

    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    public void Play()
    {
        _io.Write(Streams.Introduction);

        if (_io.ReadNumber("") != 0)
        {
            _io.Write(Streams.Instructions);
        }

        PlaySeries(_initialBalance);

        _io.Write(Streams.Goodbye);
    }

    private void PlaySeries(float balance)
    {
        while (true)
        {
            var wager = _io.ReadWager(balance);

            var gameWon = PlayGame();

            if (wager.HasValue)
            {
                balance = gameWon ? (balance + wager.Value) : (balance - wager.Value);
                if (balance <= 0)
                {
                    _io.Write(Streams.Bust);
                    return;
                }
                _io.WriteLine(Formats.Balance, balance);
            }

            if (_io.ReadNumber(Prompts.TryAgain) != 1) { return; }
        }
    }

    private bool PlayGame()
    {
        var mineLocations = _seeds.Select(seed => _random.NextLocation(seed)).ToHashSet();
        var currentLocation = _startLocation;
        var prompt = Prompts.YourMove;

        while (true)
        {
            var newLocation = _io.Read3Numbers(prompt);

            if (!MoveIsLegal(currentLocation, newLocation)) { return Lose(Streams.IllegalMove); }

            currentLocation = newLocation;

            if (currentLocation == _goalLocation) { return Win(Streams.Congratulations); }

            if (mineLocations.Contains(currentLocation)) { return Lose(Streams.Bang); }

            prompt = Prompts.NextMove;
        }
    }

    private bool Lose(Stream text)
    {
        _io.Write(text);
        return false;
    }

    private bool Win(Stream text)
    {
        _io.Write(text);
        return true;
    }

    private bool MoveIsLegal((float, float, float) from, (float, float, float) to)
        => (to.Item1 - from.Item1, to.Item2 - from.Item2, to.Item3 - from.Item3) switch
        {
            ( > 1, _, _) => false,
            (_, > 1, _) => false,
            (_, _, > 1) => false,
            (1, 1, _) => false,
            (1, _, 1) => false,
            (_, 1, 1) => false,
            _ => true
        };
}
