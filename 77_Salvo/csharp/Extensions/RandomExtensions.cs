namespace Games.Common.Randomness;

internal static class RandomExtensions
{
    internal static (Position, Offset) NextShipPosition(this IRandom random)
    {
        var startX = random.NextCoordinate();
        var startY = random.NextCoordinate();
        var deltaY = random.NextOffset();
        var deltaX = random.NextOffset();
        return (new(startX, startY), new(deltaX, deltaY));
    }

    private static Coordinate NextCoordinate(this IRandom random)
        => random.Next(Coordinate.MinValue, Coordinate.MaxValue + 1);

    private static int NextOffset(this IRandom random) => random.Next(-1, 2);

    internal static (Position, Offset) GetRandomShipPositionInRange(this IRandom random, int shipSize)
    {
        while (true)
        {
            var (start, delta) = random.NextShipPosition();
            var shipSizeLessOne = shipSize - 1;
            var end = start + delta * shipSizeLessOne;
            if (delta != 0 && end.IsInRange) 
            {
                return (start, delta);
            }
        }
    }
}
