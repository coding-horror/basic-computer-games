namespace Cube;

internal static class RandomExtensions
{
    internal static (float, float, float) NextLocation(this IRandom random, (int, int, int) bias)
        => (random.NextCoordinate(bias.Item1), random.NextCoordinate(bias.Item2), random.NextCoordinate(bias.Item3));

    private static float NextCoordinate(this IRandom random, int bias)
    {
        var value = random.Next(3);
        if (value == 0) { value = bias; }
        return value;
    }
}