namespace King;

internal class Country
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private float _rallods;
    private float _countrymen;
    private float _foreigners;
    private float _land;
    private float _plantingCost;
    private float _landValue;

    public Country(IReadWrite io, IRandom random)
        : this(
            io,
            random,
            (int)(60000 + random.NextFloat(1000) - random.NextFloat(1000)),
            (int)(500 + random.NextFloat(10) - random.NextFloat(10)),
            0,
            2000)
    {
    }

    public Country(IReadWrite io, IRandom random, float rallods, float countrymen, float foreigners, float land)
    {
        _io = io;
        _random = random;
        _rallods = rallods;
        _countrymen = countrymen;
        _foreigners = foreigners;
        _land = land;

        _plantingCost = random.Next(10, 15);
        _landValue = random.Next(95, 105);
    }

    public string Status => Resource.Status(_rallods, _countrymen, _foreigners, _land, _landValue, _plantingCost);
    private float FarmLand => _land - 1000;

    public bool SellLand()
    {
        if (_io.TryReadValue(
                SellLandPrompt, 
                out var landSold, 
                new ValidityTest(v => v <= FarmLand, () => SellLandError(FarmLand))))
        {
            _land = (int)(_land - landSold);
            _rallods = (int)(_rallods + landSold * _landValue);
            return true;
        }

        return false;
    }

    public bool DistributeRallods()
    {
        if (_io.TryReadValue(
                GiveRallodsPrompt,
                out var rallodsGiven, 
                new ValidityTest(v => v <= _rallods, () => GiveRallodsError(_rallods))))
        {
            _rallods = (int)(_rallods - rallodsGiven);
            return true;
        }

        return false;
    }

    public bool PlantLand()
    {
        if (_rallods > 0 && 
            _io.TryReadValue(
                PlantLandPrompt, 
                out var landPlanted, 
                new ValidityTest(v => v <= _countrymen * 2, PlantLandError1),
                new ValidityTest(v => v <= FarmLand, PlantLandError2(FarmLand)),
                new ValidityTest(v => v * _plantingCost <= _rallods, PlantLandError3(_rallods))))
        {
            _rallods -= (int)(landPlanted * _plantingCost);
            return true;
        }

        return false;
    }

    public bool ControlPollution()
    {
        if (_rallods > 0 &&
            _io.TryReadValue(
                PollutionPrompt,
                out var rallodsGiven, 
                new ValidityTest(v => v <= _rallods, () => PollutionError(_rallods))))
        {
            _rallods = (int)(_rallods - rallodsGiven);
            return true;
        }

        return false;
    }
}
