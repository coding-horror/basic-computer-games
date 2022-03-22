using System;
using SuperStarTrek.Utils;

namespace SuperStarTrek.Space;

// Represents the corrdintate of a quadrant in the galaxy, or a sector in a quadrant.
// Note that the origin is top-left, x increase downwards, and y increases to the right.
internal record Coordinates
{
    internal Coordinates(int x, int y)
    {
        X = Validated(x, nameof(x));
        Y = Validated(y, nameof(y));

        RegionIndex = (X << 1) + (Y >> 2);
        SubRegionIndex = Y % 4;
    }

    internal int X { get; }

    internal int Y { get; }

    internal int RegionIndex { get; }

    internal int SubRegionIndex { get; }

    private static int Validated(int value, string argumentName)
    {
        if (value >= 0 && value <= 7) { return value; }

        throw new ArgumentOutOfRangeException(argumentName, value, "Must be 0 to 7 inclusive");
    }

    private static bool IsValid(int value) => value >= 0 && value <= 7;

    public override string ToString() => $"{X+1} , {Y+1}";

    internal void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    internal static bool TryCreate(float x, float y, out Coordinates coordinates)
    {
        var roundedX = Round(x);
        var roundedY = Round(y);

        if (IsValid(roundedX) && IsValid(roundedY))
        {
            coordinates = new Coordinates(roundedX, roundedY);
            return true;
        }

        coordinates = default;
        return false;

        static int Round(float value) => (int)Math.Round(value, MidpointRounding.AwayFromZero);
    }

    internal (float Direction, float Distance) GetDirectionAndDistanceTo(Coordinates destination) =>
        DirectionAndDistance.From(this).To(destination);

    internal float GetDistanceTo(Coordinates destination)
    {
        var (_, distance) = GetDirectionAndDistanceTo(destination);
        return distance;
    }
}
