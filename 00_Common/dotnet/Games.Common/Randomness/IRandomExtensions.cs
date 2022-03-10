using System;

namespace Games.Common.Randomness;

/// <summary>
/// Provides extension methods to <see cref="IRandom" /> providing random numbers in a given range.
/// </summary>
/// <value></value>
public static class IRandomExtensions
{
    /// <summary>
    /// Gets a random <see cref="float" /> such that 0 &lt;= n &lt; exclusiveMaximum.
    /// </summary>
    /// <returns>The random number.</returns>
    public static float NextFloat(this IRandom random, float exclusiveMaximum) =>
        Scale(random.NextFloat(), exclusiveMaximum);

    /// <summary>
    /// Gets a random <see cref="float" /> such that inclusiveMinimum &lt;= n &lt; exclusiveMaximum.
    /// </summary>
    /// <returns>The random number.</returns>
    public static float NextFloat(this IRandom random, float inclusiveMinimum, float exclusiveMaximum) =>
        Scale(random.NextFloat(), inclusiveMinimum, exclusiveMaximum);

    /// <summary>
    /// Gets a random <see cref="int" /> such that 0 &lt;= n &lt; exclusiveMaximum.
    /// </summary>
    /// <returns>The random number.</returns>
    public static int Next(this IRandom random, int exclusiveMaximum) => ToInt(random.NextFloat(exclusiveMaximum));

    /// <summary>
    /// Gets a random <see cref="int" /> such that inclusiveMinimum &lt;= n &lt; exclusiveMaximum.
    /// </summary>
    /// <returns>The random number.</returns>
    public static int Next(this IRandom random, int inclusiveMinimum, int exclusiveMaximum) =>
        ToInt(random.NextFloat(inclusiveMinimum, exclusiveMaximum));

    /// <summary>
    /// Gets the previous unscaled <see cref="float" /> (between 0 and 1) scaled to a new range:
    /// 0 &lt;= x &lt; <paramref name="exclusiveMaximum" />.
    /// </summary>
    /// <returns>The random number.</returns>
    public static float PreviousFloat(this IRandom random, float exclusiveMaximum) =>
        Scale(random.PreviousFloat(), exclusiveMaximum);

    /// <summary>
    /// Gets the previous unscaled <see cref="float" /> (between 0 and 1) scaled to a new range:
    /// <paramref name="inclusiveMinimum" /> &lt;= n &lt; <paramref name="exclusiveMaximum" />.
    /// </summary>
    /// <returns>The random number.</returns>
    public static float PreviousFloat(this IRandom random, float inclusiveMinimum, float exclusiveMaximum) =>
        Scale(random.PreviousFloat(), inclusiveMinimum, exclusiveMaximum);

    /// <summary>
    /// Gets the previous unscaled <see cref="float" /> (between 0 and 1) scaled to an <see cref="int" /> in a new
    /// range: 0 &lt;= n &lt; <paramref name="exclusiveMaximum" />.
    /// </summary>
    /// <returns>The random number.</returns>
    public static int Previous(this IRandom random, int exclusiveMaximum) =>
        ToInt(random.PreviousFloat(exclusiveMaximum));

    /// <summary>
    /// Gets the previous unscaled <see cref="float" /> (between 0 and 1) scaled to an <see cref="int" /> in a new
    /// range: <paramref name="inclusiveMinimum" /> &lt;= n &lt; <paramref name="exclusiveMaximum" />.
    /// <returns>The random number.</returns>
    public static int Previous(this IRandom random, int inclusiveMinimum, int exclusiveMaximum) =>
        ToInt(random.PreviousFloat(inclusiveMinimum, exclusiveMaximum));

    private static float Scale(float zeroToOne, float exclusiveMaximum)
    {
        if (exclusiveMaximum <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(exclusiveMaximum), "Must be greater than 0.");
        }

        return Scale(zeroToOne, 0, exclusiveMaximum);
    }

    private static float Scale(float zeroToOne, float inclusiveMinimum, float exclusiveMaximum)
    {
        if (exclusiveMaximum <= inclusiveMinimum)
        {
            throw new ArgumentOutOfRangeException(nameof(exclusiveMaximum), "Must be greater than inclusiveMinimum.");
        }

        var range = exclusiveMaximum - inclusiveMinimum;
        return zeroToOne * range + inclusiveMinimum;
    }

    private static int ToInt(float value) => (int)Math.Floor(value);
}
