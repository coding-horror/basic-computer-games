namespace Salvo.Ships;

internal abstract class Ship
{
    private readonly List<Position> _positions = new();

    protected Ship(IReadWrite io, string? nameSuffix = null)
    {
        Name = GetType().Name + nameSuffix;
        _positions = io.ReadPositions(Name, Size).ToList();
    }

    protected Ship(IRandom random, string? nameSuffix = null)
    {
        Name = GetType().Name + nameSuffix;

        var (start, delta) = random.GetRandomShipPositionInRange(Size);
        for (var i = 0; i < Size; i++)
        {
            _positions.Add(start + delta * i);
        }
    }

    internal string Name { get; }
    internal abstract int Shots { get; }
    internal abstract int Size { get; }
    internal abstract float Value { get; }
    internal IEnumerable<Position> Positions => _positions;
    internal bool IsDestroyed => _positions.Count == 0;

    internal bool IsHit(Position position) => _positions.Remove(position);

    internal float DistanceTo(Ship other)
        => _positions.SelectMany(a => other._positions.Select(b => a.DistanceTo(b))).Min();

    public override string ToString() 
        => string.Join(Environment.NewLine, _positions.Select(p => p.ToString()).Prepend(Name));
}
