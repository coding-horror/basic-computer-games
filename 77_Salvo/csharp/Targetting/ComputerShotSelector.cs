namespace Salvo.Targetting;

internal class ComputerShotSelector : ShotSelector
{
    private readonly KnownHitsShotSelectionStrategy _knownHitsStrategy;
    private readonly SearchPatternShotSelector _searchPatternShotSelector;

    internal ComputerShotSelector(Grid source, Grid target, IRandom random) 
        : base(source, target)
    {
        _knownHitsStrategy = new KnownHitsShotSelectionStrategy(target);
        _searchPatternShotSelector = new SearchPatternShotSelector(source, target, random);
    }

    internal override IEnumerable<Position> GetShots()
    {
        return _knownHitsStrategy.GetShots(NumberOfShots) ?? _searchPatternShotSelector.GetShots();
    }

    internal void RecordHit(Ship ship, int turn) => _knownHitsStrategy.RecordHit(ship, turn);
}
