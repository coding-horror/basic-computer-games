using Games.Common.IO;
using Games.Common.Randomness;

namespace Basketball.Plays;

internal abstract class Play
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private readonly Clock _clock;

    public Play(IReadWrite io, IRandom random, Clock clock)
    {
        _io = io;
        _random = random;
        _clock = clock;
    }

    protected bool ClockIncrementsToHalfTime(Scoreboard scoreboard)
    {
        _clock.Increment(scoreboard);
        return _clock.IsHalfTime;
    }

    internal abstract bool Resolve(Scoreboard scoreboard);

    protected void ResolveFreeThrows(Scoreboard scoreboard, string message) =>
        Resolve(message)
            .Do(0.49f, () => scoreboard.AddFreeThrows(2, "Shooter makes both shots."))
            .Or(0.75f, () => scoreboard.AddFreeThrows(1, "Shooter makes one shot and misses one."))
            .Or(() => scoreboard.AddFreeThrows(0, "Both shots missed."));

    protected Probably Resolve(string message) => Resolve(message, 1f);

    protected Probably Resolve(string message, float defenseFactor)
    {
        _io.WriteLine(message);
        return new Probably(defenseFactor, _random);
    }
}
