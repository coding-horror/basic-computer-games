using System.Text;

namespace King;

internal class Year
{
    private readonly Country _country;
    private readonly IRandom _random;
    private readonly int _plantingCost;
    private readonly int _landValue;

    private float _landSold;
    private float _rallodsDistributed;
    private float _landPlanted;
    private float _pollutionControlCost;

    public Year(Country country, IRandom random)
    {
        _country = country;
        _random = random;
        
        _plantingCost = random.Next(10, 15);
        _landValue = random.Next(95, 105);
    }

    public string Status => _country.GetStatus(_landValue, _plantingCost);

    public bool GetPlayerActions()
    {
        var playerSoldLand = _country.SellLand(_landValue, out _landSold);
        var playerDistributedRallods = _country.DistributeRallods(out _rallodsDistributed);
        var playerPlantedLand = _country.HasRallods && _country.PlantLand(_plantingCost, out _landPlanted);
        var playerControlledPollution = _country.HasRallods && _country.ControlPollution(out _pollutionControlCost);

        return playerSoldLand || playerDistributedRallods || playerPlantedLand || playerControlledPollution;
    }

    public Result EvaluateResults(IReadWrite io)
    {
        var unspentRallods = _country.Rallods;
        var statusUpdate = new StringBuilder();

        var result = EvaluateDeaths(statusUpdate, out var deaths);

        io.Write(statusUpdate);

        return Result.Continue;
    }

    public Result? EvaluateDeaths(StringBuilder statusUpdate, out int deaths)
    {
        deaths = default;

        var supportedCountrymen = _rallodsDistributed / 100;
        var starvationDeaths = _country.Countrymen - supportedCountrymen;
        if (starvationDeaths > 0)
        {
            if (supportedCountrymen < 50) { return Result.GameOver(EndOneThirdDead(_random)); }
            statusUpdate.AppendLine(DeathsStarvation(starvationDeaths));
        }

        var pollutionControl = _pollutionControlCost >= 25 ? _pollutionControlCost / 25 : 1;
        var pollutionDeaths = (int)(_random.Next((int)_country.IndustryLand) / pollutionControl);
        if (pollutionDeaths > 0)
        {
            statusUpdate.AppendLine(DeathsPollution(pollutionDeaths));
        }

        deaths = (int)(starvationDeaths + pollutionDeaths);
        if (deaths > 0)
        {
            var funeralCosts = deaths * 9;
            statusUpdate.AppendLine(FuneralExpenses(funeralCosts));

            if (!_country.TrySpend(funeralCosts, _landValue))
            {
                statusUpdate.AppendLine(InsufficientReserves);
            }

            _country.RemoveTheDead(deaths);
        }

        return null;
    }


    internal record struct Result (bool IsGameOver, string Message)
    {
        internal static Result GameOver(string message) => new(true, message);
        internal static Result Continue => new(false, "");
    }
}

