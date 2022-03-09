namespace Games.Common.Randomness;

/// <summary>
/// Provides access to a random number generator
/// </summary>
public interface IRandom
{
    /// <summary>
    /// Gets a random <see cref="float" /> such that 0 &lt;= n &lt; 1.
    /// </summary>
    /// <returns>The random number.</returns>
    float NextFloat();

    /// <summary>
    /// Gets the <see cref="float" /> returned by the previous call to <see cref="NextFloat" />.
    /// </summary>
    /// <returns>The previous random number.</returns>
    float PreviousFloat();

    /// <summary>
    /// Reseeds the random number generator.
    /// </summary>
    /// <param name="seed">The seed.</param>
    void Reseed(int seed);
}
