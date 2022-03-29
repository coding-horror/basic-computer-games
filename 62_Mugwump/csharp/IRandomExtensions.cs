namespace Mugwump;

internal static class IRandomExtensions
{
    internal static Position NextPosition(this IRandom random, int maxX, int maxY) =>
        new(random.Next(maxX), random.Next(maxY));
}
