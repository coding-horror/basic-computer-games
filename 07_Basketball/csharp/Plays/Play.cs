using Games.Common.IO;
using Games.Common.Randomness;

namespace Basketball.Plays;

internal abstract class Play
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Play(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
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
