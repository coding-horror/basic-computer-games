namespace Salvo.Targetting;

internal abstract class ShotSelector
{
    private readonly Grid _source;
    private readonly Dictionary<Position, int> _previousShots = new();

    internal ShotSelector(Grid source)
    {
        _source = source;
    }

    internal int NumberOfShots => _source.Ships.Sum(s => s.Shots);
    internal bool CanTargetAllRemainingSquares => NumberOfShots >= 100 - _previousShots.Count;

    internal bool WasSelectedPreviously(Position position) => _previousShots.ContainsKey(position);

    internal bool WasSelectedPreviously(Position position, out int turn)
        => _previousShots.TryGetValue(position, out turn);

    internal IEnumerable<Position> GetShots(int turnNumber)
    {
        foreach (var shot in GetShots())
        {
            _previousShots.Add(shot, turnNumber);
            yield return shot;
        }
    }

    protected abstract IEnumerable<Position> GetShots();
}
