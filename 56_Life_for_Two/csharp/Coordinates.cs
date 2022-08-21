namespace LifeforTwo;

internal class Coordinates
{
    private Coordinates (int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    public static bool TryCreate((float X, float Y) values, out Coordinates coordinates)
    {
        if (values.X <= 0 || values.X > 5 || values.Y <= 0 || values.Y > 5)
        {
            coordinates = new(0, 0);
            return false;
        }

        coordinates = new((int)values.X, (int)values.Y);
        return true;
    }

    public static Coordinates operator +(Coordinates coordinates, int value) =>
        new (coordinates.X + value, coordinates.Y + value);

    public static bool operator ==(Coordinates a, Coordinates b) => a.X == b.X && a.Y == b.Y;
    
    public static bool operator !=(Coordinates a, Coordinates b) => !(a == b);

    public override bool Equals(object? obj) => obj is Coordinates other && other == this;

    public override int GetHashCode() => HashCode.Combine(X, Y);
}
