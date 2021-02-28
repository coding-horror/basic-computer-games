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

        private int Validated(int value, string argumentName)
        {
            if (value >= 1 && value <= 8) { return value; }

            throw new ArgumentOutOfRangeException(argumentName, value, "Must be 1 to 8 inclusive");
        }

        public override string ToString() => $"{X} , {Y}";
    }
}
