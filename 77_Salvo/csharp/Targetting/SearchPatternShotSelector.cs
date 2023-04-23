namespace Salvo.Targetting;

internal class SearchPatternShotSelector : ShotSelector
{
    private const int MaxSearchPatternAttempts = 100;
    private readonly IRandom _random;
    private readonly SearchPattern _searchPattern = new();
    private readonly List<Position> _shots = new();

    internal SearchPatternShotSelector(Grid source, Grid target, IRandom random) 
        : base(source, target)
    {
        _random = random;
    }

    internal override IEnumerable<Position> GetShots()
    {
        while(_shots.Count < NumberOfShots)
        {
            var (seed, _) = _random.NextShipPosition();
            SearchFrom(seed);
        }
        return _shots;
    }

    private void SearchFrom(Position candidateShot)
    {
        var attemptsLeft = MaxSearchPatternAttempts;
        while (true)
        {
            _searchPattern.Reset();
            if (attemptsLeft-- == 0) { return; }
            candidateShot = candidateShot.BringIntoRange(_random);
            if (FindValidShots(candidateShot)) { return; }
        }
    }

    private bool FindValidShots(Position candidateShot)
    {
        while (true)
        {
            if (IsValidShot(candidateShot))
            {
                _shots.Add(candidateShot);
                if (_shots.Count == NumberOfShots) { return true; }
            }
            if (!_searchPattern.TryGetOffset(out var offset)) { return false; }
            candidateShot += offset;
        }
    }

    private bool IsValidShot(Position candidate)
        => candidate.IsInRange && !Target.WasTargetedAt(candidate, out _) && !_shots.Contains(candidate);
}