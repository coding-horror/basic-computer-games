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

        if (year.GetPlayerActions())
        {
            _io.WriteLine();
            _io.WriteLine();
            year.EvaluateResults();
            _yearNumber++;
            return true;
        }
        else
        {
            _io.WriteLine();
            _io.Write(Goodbye);
            return false;
        }
    }
}
