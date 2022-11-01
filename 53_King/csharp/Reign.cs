namespace King;

internal class Reign
{
    public const int MaxTerm = 8;

    private readonly IReadWrite _io;
    private readonly Country _country;
    private float _year;

    public Reign(IReadWrite io, IRandom random)
        : this(io, new Country(io, random), 1)
    {
    }

    public Reign(IReadWrite io, Country country, float year)
    {
        _io = io;
        _country = country;
        _year = year;
    }

    public bool PlayYear()
    {
        _io.Write(_country.Status);

        var playerSoldLand = _country.SellLand();
        var playerDistributedRallods = _country.DistributeRallods();
        var playerPlantedLand = _country.PlantLand();
        var playerControlledPollution = _country.ControlPollution();

        if (playerSoldLand || playerDistributedRallods || playerPlantedLand || playerControlledPollution)
        {
            _year++;
            return true;
        }
        else
        {
            _io.Write(Goodbye);
            return false;
        }
    }
}
