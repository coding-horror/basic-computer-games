namespace Salvo;

internal record struct Coordinate(int Value)
{
    public const int MinValue = 1;
    public const int MaxValue = 10;

    public static IEnumerable<Coordinate> Range => Enumerable.Range(1, 10).Select(v => new Coordinate(v));

    public bool IsInRange => Value is >= MinValue and <= MaxValue;

    public static Coordinate Create(float value) => new((int)value);

    public static bool TryCreateValid(float value, out Coordinate coordinate)
    {
        coordinate = default;
        if (value != (int)value) { return false; }

        var result = Create(value);

        if (result.IsInRange)
        {
            coordinate = result;
            return true;
        }

        return false;
    }

    public Coordinate BringIntoRange(IRandom random)
        => Value switch
        {
            < MinValue => new(MinValue + (int)random.NextFloat(2.5F)),
            > MaxValue => new(MaxValue - (int)random.NextFloat(2.5F)),
            _ => this
        };

    public static implicit operator Coordinate(float value) => Create(value);
    public static implicit operator int(Coordinate coordinate) => coordinate.Value;

    public static Coordinate operator +(Coordinate coordinate, int offset) => new(coordinate.Value + offset);
    public static int operator -(Coordinate a, Coordinate b) => a.Value - b.Value;

    public override string ToString() => $" {Value} ";
}
