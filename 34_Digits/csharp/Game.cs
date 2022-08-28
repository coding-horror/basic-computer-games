namespace Digits;

internal class GameSeries
{
    private readonly IReadOnlyList<int> _weights = new List<int> { 0, 1, 3 }.AsReadOnly();

    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public GameSeries(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    internal void Play()
    {
        _io.Write(Streams.Introduction);

        if (_io.ReadNumber(Prompts.ForInstructions) != 0)
        {
            _io.Write(Streams.Instructions);
        }

        do
        {
            new Game(_io, _random).Play();
        } while (_io.ReadNumber(Prompts.WantToTryAgain) == 1);

        _io.Write(Streams.Thanks);
    }
}

internal class Game
{
    private readonly IReadWrite _io;
    private readonly Guesser _guesser;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _guesser = new Guesser(random);
    }

    public void Play()
    {
        var correctGuesses = 0;

        for (int round = 0; round < 3; round++)
        {
            var digits = _io.Read10Digits(Prompts.TenNumbers, Streams.TryAgain);

            correctGuesses = GuessDigits(digits, correctGuesses);
        }

        _io.Write(correctGuesses switch
        {
            < 10 => Streams.YouWin,
            10 => Streams.ItsATie,
            > 10 => Streams.IWin
        });
    }

    private int GuessDigits(IEnumerable<int> digits, int correctGuesses)
    {
        _io.Write(Streams.Headings);

        foreach (var digit in digits)
        {
            var guess = _guesser.GuessNextDigit();
            if (guess == digit) { correctGuesses++; }

            _io.WriteLine(Formats.GuessResult, guess, digit, guess == digit ? "Right" : "Wrong", correctGuesses);

            _guesser.ObserveActualDigit(digit);
        }

        return correctGuesses;
    }
}
