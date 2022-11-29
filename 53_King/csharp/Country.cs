namespace King;

internal class Country
{
    private const int InitialLand = 1000;

    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private float _rallods;
    private float _countrymen;
    private float _foreigners;
    private float _arableLand;
    private float _industryLand;

    public Country(IReadWrite io, IRandom random)
        : this(
            io,
            random,
            (int)(60000 + random.NextFloat(1000) - random.NextFloat(1000)),
            (int)(500 + random.NextFloat(10) - random.NextFloat(10)),
            0,
            InitialLand)
    {
    }

    public Country(IReadWrite io, IRandom random, float rallods, float countrymen, float foreigners, float land)
    {
        _io = io;
        _random = random;
        _rallods = rallods;
        _countrymen = countrymen;
        _foreigners = foreigners;
        _arableLand = land;
    }

    public string GetStatus(int landValue, int plantingCost) 
        => Resource.Status(_rallods, _countrymen, _foreigners, _arableLand, landValue, plantingCost);
    
    public float Countrymen => _countrymen;
    private float FarmLand => _arableLand;
    public bool HasRallods => _rallods > 0;
    public float Rallods => _rallods;
    public float IndustryLand => InitialLand - _arableLand;

    public bool SellLand(int landValue, out float landSold)
    {
        if (_io.TryReadValue(
                SellLandPrompt, 
                out landSold, 
                new ValidityTest(v => v <= FarmLand, () => SellLandError(FarmLand))))
        {
            _arableLand = (int)(_arableLand - landSold);
            _rallods = (int)(_rallods + landSold * landValue);
            return true;
        }

        return false;
    }

    public bool DistributeRallods(out float rallodsGiven)
    {
        if (_io.TryReadValue(
                GiveRallodsPrompt,
                out rallodsGiven, 
                new ValidityTest(v => v <= _rallods, () => GiveRallodsError(_rallods))))
        {
            _rallods = (int)(_rallods - rallodsGiven);
            return true;
        }

        return false;
    }

    public bool PlantLand(int plantingCost, out float landPlanted)
    {
        if (_io.TryReadValue(
                PlantLandPrompt, 
                out landPlanted, 
                new ValidityTest(v => v <= _countrymen * 2, PlantLandError1),
                new ValidityTest(v => v <= FarmLand, PlantLandError2(FarmLand)),
                new ValidityTest(v => v * plantingCost <= _rallods, PlantLandError3(_rallods))))
        {
            _rallods -= (int)(landPlanted * plantingCost);
            return true;
        }

        return false;
    }

    public bool ControlPollution(out float rallodsSpent)
    {
        if (_io.TryReadValue(
                PollutionPrompt,
                out rallodsSpent, 
                new ValidityTest(v => v <= _rallods, () => PollutionError(_rallods))))
        {
            _rallods = (int)(_rallods - rallodsSpent);
            return true;
        }

        return false;
    }

    public bool TrySpend(float amount, float landValue)
    {
        if (_rallods >= amount)
        {
            _rallods -= amount;
            return true;
        }
        
        _arableLand = (int)(_arableLand - (int)(amount - _rallods) / landValue);
        _rallods = 0;
        return false;
    }

    public void RemoveTheDead(int deaths) => _countrymen = (int)(_countrymen - deaths);
}
