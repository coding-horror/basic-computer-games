namespace LifeforTwo;

internal record Coordinates (int X, int Y)
{
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

    public IEnumerable<Coordinates> GetNeighbors()
    {
        yield return new(X - 1, Y);
        yield return new(X + 1, Y);
        yield return new(X, Y - 1);
        yield return new(X, Y + 1);
        yield return new(X - 1, Y - 1);
        yield return new(X + 1, Y - 1);
        yield return new(X - 1, Y + 1);
        yield return new(X + 1, Y + 1);
    }
}
