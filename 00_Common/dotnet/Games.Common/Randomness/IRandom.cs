namespace Games.Common.Randomness;

/// <summary>
/// Provides access to a random number generator
/// </summary>
public interface IRandom
{
    /// <summary>
    /// Gets a random <see cref="float" /> such that 0 <= n < 1.
    /// </summary>
    /// <returns>The random number.</returns>
    float NextFloat();

    /// <summary>
    /// Gets a random <see cref="float" /> such that 0 <= n < exclusiveMaximum.
    /// </summary>
    /// <returns>The random number.</returns>
    float NextFloat(float exclusiveMaximum);

    /// <summary>
    /// Gets a random <see cref="float" /> such that inclusiveMinimum <= n < exclusiveMaximum.
    /// </summary>
    /// <returns>The random number.</returns>
    float NextFloat(float inclusiveMinimum, float exclusiveMaximum);

    /// <summary>
    /// Gets a random <see cref="int" /> such that 0 <= n < exclusiveMaximum.
    /// </summary>
    /// <returns>The random number.</returns>
    int Next(int exclusiveMaximum);

    /// <summary>
    /// Gets a random <see cref="int" /> such that inclusiveMinimum <= n < exclusiveMaximum.
    /// </summary>
    /// <returns>The random number.</returns>
    int Next(int inclusiveMinimum, int exclusiveMaximum);

    /// <summary>
    /// Gets the previous random number as a <see cref="float" />.
    /// </summary>
    /// <returns>The previous random number.</returns>
    float PreviousFloat();

    /// <summary>
    /// Gets the previous random number as an <see cref="int" />.
    /// </summary>
    /// <returns>The previous random number.</returns>
    int Previous();

    /// <summary>
    /// Reseeds the random number generator.
    /// </summary>
    /// <param name="seed">The seed.</param>
    void Reseed(int seed);
}
