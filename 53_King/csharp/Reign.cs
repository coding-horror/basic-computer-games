namespace King;

internal class Reign
{
    public const int MaxTerm = 8;

    private readonly IReadWrite _io;
    private readonly Country _country;
    private readonly float _year;

    public Reign(IReadWrite io, IRandom random)
        : this(io, new Country(random), 0)
    {

    }

    public Reign(IReadWrite io, Country country, float year)
    {
        _io = io;
        _country = country;
        _year = year;
    }
}
