namespace Salvo.Targetting;

internal abstract class ShotSelectionStrategy
{
    private readonly Grid _target;
    protected ShotSelectionStrategy(Grid target)
    {
        _target = target;
    }

    protected bool WasSelectedPreviously(Position position)
        => _target.WasTargetedAt(position, out _);

    protected bool WasSelectedPreviously(Position position, out int turn)
        => _target.WasTargetedAt(position, out turn);
}
