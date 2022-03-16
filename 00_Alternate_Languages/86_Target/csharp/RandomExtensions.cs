using Games.Common.Randomness;

namespace Target
{
    internal static class RandomExtensions
    {
        public static Point NextPosition(this IRandom rnd) => new (
            Angle.InRotations(rnd.NextFloat()),
            Angle.InRotations(rnd.NextFloat()),
            100000 * rnd.NextFloat() + rnd.NextFloat());
    }
}
