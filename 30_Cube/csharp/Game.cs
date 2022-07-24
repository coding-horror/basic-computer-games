namespace Cube;

internal class Game
{
    private const int _initialBalance = 500;
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
        return true;
    }
}
