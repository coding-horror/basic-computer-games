using System.Text;

namespace King;

internal class Year
{
    private readonly Country _country;
    private readonly IRandom _random;
    private readonly IReadWrite _io;
    private readonly int _plantingCost;
    private readonly int _landValue;

    private float _landSold;
    private float _rallodsDistributed;
    private float _landPlanted;
    private float _pollutionControlCost;

    private float _citizenSupport;
    private int _deaths;
    private float _starvationDeaths;
    private int _pollutionDeaths;
    private int _migration;

    public Year(Country country, IRandom random, IReadWrite io)
    {
        _country = country;
        _random = random;
        _io = io;
        
        _plantingCost = random.Next(10, 15);
        _landValue = random.Next(95, 105);
    }

    public string Status => _country.GetStatus(_landValue, _plantingCost);

    public Result? GetPlayerActions()
    {
        var playerSoldLand = _country.SellLand(_landValue, out _landSold);
        var playerDistributedRallods = _country.DistributeRallods(out _rallodsDistributed);
        var playerPlantedLand = _country.HasRallods && _country.PlantLand(_plantingCost, out _landPlanted);
        var playerControlledPollution = _country.HasRallods && _country.ControlPollution(out _pollutionControlCost);

        return playerSoldLand || playerDistributedRallods || playerPlantedLand || playerControlledPollution
            ? null
            : Result.GameOver(Goodbye);
    }

    public Result? EvaluateResults()
    {
        var rallodsUnspent = _country.Rallods;

        _io.WriteLine();
        _io.WriteLine();

        return EvaluateDeaths() 
            ?? EvaluateMigration() 
            ?? EvaluateAgriculture()
            ?? EvaluateTourism()
            ?? DetermineResult(rallodsUnspent);
    }

    public Result? EvaluateDeaths()
    {
        var supportedCountrymen = _rallodsDistributed / 100;
        _citizenSupport = supportedCountrymen - _country.Countrymen;
        _starvationDeaths = -_citizenSupport;
        if (_starvationDeaths > 0)
        {
            if (supportedCountrymen < 50) { return Result.GameOver(EndOneThirdDead(_random)); }
            _io.WriteLine(DeathsStarvation(_starvationDeaths));
        }

        var pollutionControl = _pollutionControlCost >= 25 ? _pollutionControlCost / 25 : 1;
        _pollutionDeaths = (int)(_random.Next((int)_country.IndustryLand) / pollutionControl);
        if (_pollutionDeaths > 0)
        {
            _io.WriteLine(DeathsPollution(_pollutionDeaths));
        }

        _deaths = (int)(_starvationDeaths + _pollutionDeaths);
        if (_deaths > 0)
        {
            var funeralCosts = _deaths * 9;
            _io.WriteLine(FuneralExpenses(funeralCosts));

            if (!_country.TrySpend(funeralCosts, _landValue))
            {
                _io.WriteLine(InsufficientReserves);
            }

            _country.RemoveTheDead(_deaths);
        }

        return null;
    }

    private Result? EvaluateMigration()
    {
        if (_landSold > 0)
        {
            var newWorkers = (int)(_landSold + _random.NextFloat(10) - _random.NextFloat(20));
            if (!_country.HasWorkers) { newWorkers += 20; }
            _io.Write(WorkerMigration(newWorkers));
            _country.AddWorkers(newWorkers);
        }

        _migration = 
            (int)(_citizenSupport / 10 + _pollutionControlCost / 25 - _country.IndustryLand / 50 - _pollutionDeaths / 2);
        _io.WriteLine(Migration(_migration));
        _country.Migration(_migration);

        return null;
    }

    private Result? EvaluateAgriculture()
    {
        var ruinedCrops = (int)Math.Min(_country.IndustryLand * (_random.NextFloat() + 1.5f) / 2, _landPlanted);
        var yield = (int)(_landPlanted - ruinedCrops);
        var income = (int)(yield * _landValue / 2f);

        _io.Write(LandPlanted(_landPlanted));
        _io.Write(Harvest(yield, income, _country.IndustryLand > 0));

        _country.SellCrops(income);

        return null;
    }

    private Result? EvaluateTourism()
    {
        var reputationValue = (int)((_country.Countrymen - _migration) * 22 + _random.NextFloat(500));
        var industryAdjustment = (int)(_country.IndustryLand * 15);
        var tourismIncome = Math.Abs(reputationValue - industryAdjustment);

        _io.WriteLine(TourismEarnings(tourismIncome));
        if (industryAdjustment > 0 && tourismIncome < _country.PreviousTourismIncome)
        {
            _io.Write(TourismDecrease(_random));
        }

        _country.EntertainTourists(tourismIncome);

        return null;
    }

    private Result? DetermineResult(float rallodsUnspent)
    {
        if (_deaths > 200) { return Result.GameOver(EndManyDead(_deaths, _random)); }
        if (_country.Countrymen < 343) { return Result.GameOver(EndOneThirdDead(_random)); }
        if (rallodsUnspent / 100 > 5 && _starvationDeaths >= 2) { return Result.GameOver(EndMoneyLeftOver()); }
        if (_country.Workers > _country.Countrymen) { return Result.GameOver(EndForeignWorkers(_random)); }
        return null;
    }
}
