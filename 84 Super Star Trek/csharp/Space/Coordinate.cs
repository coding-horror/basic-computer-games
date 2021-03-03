using System;

namespace SuperStarTrek.Space
{
    // Represents the corrdintate of a quadrant in the galaxy, or a sector in a quadrant.
    // Note that the origin is top-left, x increase downwards, and y increases to the right.
    internal record Coordinates
    {
        public Coordinates(int x, int y)
        {
            X = Validated(x, nameof(x));
            Y = Validated(y, nameof(y));
        }

        public int X { get; }
        public int Y { get; }
        public int RegionIndex => (X << 1) + (Y >> 2);
        public int SubRegionIndex => Y % 4;

        private int Validated(int value, string argumentName)
        {
            if (value >= 0 && value <= 7) { return value; }

            throw new ArgumentOutOfRangeException(argumentName, value, "Must be 0 to 7 inclusive");
        }

        public override string ToString() => $"{X+1} , {Y+1}";
    }
}
