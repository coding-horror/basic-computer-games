namespace King;

internal class Country
{
    private readonly IRandom _random;
    private float _rallods;
    private float _countrymen;
    private float _foreigners;
    private float _land;
    private float _plantingCost;
    private float _landValue;

    public Country(IRandom random)
        : this(
            random,
            (int)(60000 + random.NextFloat(1000) - random.NextFloat(1000)),
            (int)(500 + random.NextFloat(10) - random.NextFloat(10)),
            0,
            2000)
    {
    }

    public Country(IRandom random, float rallods, float countrymen, float foreigners, float land)
    {
        _random = random;
        _rallods = rallods;
        _countrymen = countrymen;
        _foreigners = foreigners;
        _land = land;

        _plantingCost = random.Next(10, 15);
        _landValue = random.Next(95, 105);
    }
}
