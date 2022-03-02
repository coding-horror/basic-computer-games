using System;

namespace Games.Common.Randomness;

public class RandomNumberGenerator : IRandom
{
    private Random _random;
    private float _previous;

    public RandomNumberGenerator()
    {
        // The BASIC RNG is seeded based on time with a 1 second resolution
        _random = new Random((int)(DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond));
    }

    public float NextFloat() => NextFloat(1);

    public float NextFloat(float exclusiveMaximum)
    {
        if (exclusiveMaximum <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(exclusiveMaximum), "Must be greater than 0.");
        }

        return NextFloat(0, exclusiveMaximum);
    }

    public float NextFloat(float inclusiveMinimum, float exclusiveMaximum)
    {
        if (exclusiveMaximum <= inclusiveMinimum)
        {
            throw new ArgumentOutOfRangeException(nameof(exclusiveMaximum), "Must be greater than inclusiveMinimum.");
        }

        var range = exclusiveMaximum - inclusiveMinimum;
        return _previous = ((float)_random.NextDouble()) * range + inclusiveMinimum;
    }

    public int Next(int exclusiveMaximum) => ToInt(NextFloat(exclusiveMaximum));

    public int Next(int inclusiveMinimum, int exclusiveMaximum) => ToInt(NextFloat(inclusiveMinimum, exclusiveMaximum));

    public float PreviousFloat() => _previous;

    public int Previous() => ToInt(_previous);

    private static int ToInt(float value) => (int)Math.Floor(value);

    public void Reseed(int seed) => _random = new Random(seed);
}