using Games.Common.Randomness;
using SuperStarTrek.Space;

namespace SuperStarTrek;

internal static class IRandomExtensions
{
    internal static Coordinates NextCoordinate(this IRandom random) =>
        new Coordinates(random.Next1To8Inclusive() - 1, random.Next1To8Inclusive() - 1);

    // Duplicates the algorithm used in the original code to get an integer value from 1 to 8, inclusive:
    //     475 DEF FNR(R)=INT(RND(R)*7.98+1.01)
    // Returns a value from 1 to 8, inclusive.
    // Note there's a slight bias away from the extreme values, 1 and 8.
    internal static int Next1To8Inclusive(this IRandom random) => (int)(random.NextFloat() * 7.98 + 1.01);
}
