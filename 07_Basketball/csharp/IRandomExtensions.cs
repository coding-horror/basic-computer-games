using Games.Common.Randomness;

namespace Basketball;

internal static class IRandomExtensions
{
    internal static Shot NextShot(this IRandom random) => Shot.Get(random.NextFloat(1, 3.5f));
}
