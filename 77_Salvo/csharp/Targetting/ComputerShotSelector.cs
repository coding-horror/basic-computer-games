namespace Salvo.Targetting;

internal class ComputerShotSelector : ShotSelector
{
    private readonly KnownHitsShotSelectionStrategy _knownHitsStrategy;
    private readonly SearchPatternShotSelectionStrategy _searchPatternStrategy;
    private readonly IReadWrite _io;
    private readonly bool _showShots;

    internal ComputerShotSelector(Fleet source, IRandom random, IReadWrite io) 
        : base(source)
    {
        _knownHitsStrategy = new KnownHitsShotSelectionStrategy(this);
        _searchPatternStrategy = new SearchPatternShotSelectionStrategy(this, random);
        _io = io;
        _showShots = io.ReadString(Prompts.SeeShots).Equals("yes", StringComparison.InvariantCultureIgnoreCase);
    }

    protected override IEnumerable<Position> GetShots()
    {
        var shots = GetSelectionStrategy().GetShots(NumberOfShots).ToArray();
        if (_showShots)
        {
            _io.WriteLine(string.Join(Environment.NewLine, shots));
        }
        return shots;
    }

    internal void RecordHit(Ship ship, int turn) => _knownHitsStrategy.RecordHit(ship, turn);

    private ShotSelectionStrategy GetSelectionStrategy()
        => _knownHitsStrategy.KnowsOfDamagedShips ? _knownHitsStrategy : _searchPatternStrategy;
}
