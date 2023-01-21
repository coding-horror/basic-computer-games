namespace King;

internal class Reign
{
    public const int MaxTerm = 8;

    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private readonly Country _country;
    private float _yearNumber;

    public Reign(IReadWrite io, IRandom random)
        : this(io, random, new Country(io, random), 1)
    {
    }

    public Reign(IReadWrite io, IRandom random, Country country, float year)
    {
        _io = io;
        _random = random;
        _country = country;
        _yearNumber = year;
    }

    public bool PlayYear()
    {
        var year = new Year(_country, _random, _io);

        _io.Write(year.Status);

        var result = year.GetPlayerActions() ?? year.EvaluateResults() ?? IsAtEndOfTerm();
        if (result.IsGameOver)
        {
            _io.WriteLine(result.Message);
            return false;
        }

        return true;
    }

    private Result IsAtEndOfTerm() 
        => _yearNumber == MaxTerm 
            ? Result.GameOver(EndCongratulations(MaxTerm)) 
            : Result.Continue;
}
