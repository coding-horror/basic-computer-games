namespace Salvo.Targetting;

internal class ComputerShotSelector : ShotSelector
{
    private readonly KnownHitsShotSelectionStrategy _knownHitsStrategy;
    private readonly SearchPatternShotSelectionStrategy _searchPatternStrategy;

    internal ComputerShotSelector(Grid source, IRandom random) 
        : base(source)
    {
        _knownHitsStrategy = new KnownHitsShotSelectionStrategy(this);
        _searchPatternStrategy = new SearchPatternShotSelectionStrategy(this, random);
    }

    protected override IEnumerable<Position> GetShots() => GetSelectionStrategy().GetShots(NumberOfShots);

    internal void RecordHit(Ship ship, int turn) => _knownHitsStrategy.RecordHit(ship, turn);

    private ShotSelectionStrategy GetSelectionStrategy()
        => _knownHitsStrategy.KnowsOfDamagedShips ? _knownHitsStrategy : _searchPatternStrategy;
}
