namespace Salvo.Targetting;

internal abstract class ShotSelectionStrategy
{
    private readonly ShotSelector _shotSelector;
    protected ShotSelectionStrategy(ShotSelector shotSelector)
    {
        _shotSelector = shotSelector;
    }

    internal abstract IEnumerable<Position> GetShots(int numberOfShots);

    protected bool WasSelectedPreviously(Position position) => _shotSelector.WasSelectedPreviously(position);

    protected bool WasSelectedPreviously(Position position, out int turn)
        => _shotSelector.WasSelectedPreviously(position, out turn);
}
