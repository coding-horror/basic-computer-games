namespace Salvo.Targetting;

internal class SearchPatternShotSelectionStrategy : ShotSelectionStrategy
{
    private const int MaxSearchPatternAttempts = 100;
    private readonly IRandom _random;
    private readonly SearchPattern _searchPattern = new();
    private readonly List<Position> _shots = new();

    internal SearchPatternShotSelectionStrategy(ShotSelector shotSelector, IRandom random) 
        : base(shotSelector)
    {
        _random = random;
    }

    internal override IEnumerable<Position> GetShots(int numberOfShots)
    {
        _shots.Clear();
        while(_shots.Count < numberOfShots)
        {
            var (seed, _) = _random.NextShipPosition();
            SearchFrom(numberOfShots, seed);
        }
        return _shots;
    }

    private void SearchFrom(int numberOfShots, Position candidateShot)
    {
        var attemptsLeft = MaxSearchPatternAttempts;
        while (true)
        {
            _searchPattern.Reset();
            if (attemptsLeft-- == 0) { return; }
            candidateShot = candidateShot.BringIntoRange(_random);
            if (FindValidShots(numberOfShots, ref candidateShot)) { return; }
        }
    }

    private bool FindValidShots(int numberOfShots, ref Position candidateShot)
    {
        while (true)
        {
            if (IsValidShot(candidateShot))
            {
                _shots.Add(candidateShot);
                if (_shots.Count == numberOfShots) { return true; }
            }
            if (!_searchPattern.TryGetOffset(out var offset)) { return false; }
            candidateShot += offset;
        }
    }

    private bool IsValidShot(Position candidate)
        => candidate.IsInRange && !WasSelectedPreviously(candidate) && !_shots.Contains(candidate);
}