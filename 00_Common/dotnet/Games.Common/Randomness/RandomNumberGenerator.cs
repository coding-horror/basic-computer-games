using System;

namespace Games.Common.Randomness;

/// <inheritdoc />
public class RandomNumberGenerator : IRandom
{
    private Random _random;
    private float _previous;

    public RandomNumberGenerator()
    {
        // The BASIC RNG is seeded based on time with a 1 second resolution
        _random = new Random((int)(DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond));
    }

    public float NextFloat() => _previous = (float)_random.NextDouble();

    public float PreviousFloat() => _previous;

    public void Reseed(int seed) => _random = new Random(seed);
}
