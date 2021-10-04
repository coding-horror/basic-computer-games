using System;

namespace Target
{
    internal static class RandomExtensions
    {
        public static float NextFloat(this Random rnd) => (float)rnd.NextDouble();

        public static Point NextPosition(this Random rnd) => new (
            Angle.InRotations(rnd.NextFloat()),
            Angle.InRotations(rnd.NextFloat()),
            100000 * rnd.NextFloat() + rnd.NextFloat());
    }
}
