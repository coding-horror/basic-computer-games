namespace King;

internal class Reign
{
    public const int MaxTerm = 8;

    private readonly IReadWrite _io;
    private readonly Country _country;
    private readonly float _year;

    public Reign(IReadWrite io, IRandom random)
        : this(io, new Country(io, random), 0)
    {
    }

    public Reign(IReadWrite io, Country country, float year)
    {
        _io = io;
        _country = country;
        _year = year;
    }

    public void PlayYear()
    {
        _io.Write(_country.Status);

        var playerSoldLand = _country.SellLand();
        var playerDistributedRallods = _country.DistributeRallods();
        var playerPlantedLand = _country.PlantLand();
        var playerControlledPollution = _country.ControlPollution();
        
    }
}
