namespace Salvo.Targetting;

internal abstract class ShotSelector
{
    internal ShotSelector(Grid source, Grid target)
    {
        Source = source;
        Target = target;
    }

    protected Grid Source { get; }
    protected Grid Target { get; }

    public int GetShotCount() => Source.Ships.Sum(s => s.Shots);
}
