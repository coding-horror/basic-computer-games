namespace Salvo;

internal record struct Position(Coordinate X, Coordinate Y)
{
    public bool IsInRange => X.IsInRange && Y.IsInRange;
    public bool IsOnDiagonal => X == Y;

    public static Position Create((float X, float Y) coordinates) => new(coordinates.X, coordinates.Y);

    public static bool TryCreateValid((float X, float Y) coordinates, out Position position)
    {
        if (Coordinate.TryCreateValid(coordinates.X, out var x) && Coordinate.TryCreateValid(coordinates.Y, out var y))
        {
            position = new(x, y);
            return true;
        }

        position = default;
        return false;
    }

    public static IEnumerable<Position> All
        => Coordinate.Range.SelectMany(x => Coordinate.Range.Select(y => new Position(x, y)));

    public IEnumerable<Position> Neighbours
    {
        get
        {
            foreach (var offset in Offset.Units)
            {
                var neighbour = this + offset;
                if (neighbour.IsInRange) { yield return neighbour; }
            }
        }
    }

    internal float DistanceTo(Position other)
    {
        var (deltaX, deltaY) = (X - other.X, Y - other.Y);
        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    internal Position BringIntoRange(IRandom random)
        => IsInRange ? this : new(X.BringIntoRange(random), Y.BringIntoRange(random));

    public static Position operator +(Position position, Offset offset) 
        => new(position.X + offset.X, position.Y + offset.Y);

    public static implicit operator Position(int value) => new(value, value);

    public override string ToString() => $"{X}{Y}";
}
