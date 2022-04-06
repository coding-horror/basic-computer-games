using Basketball.Resources;
using Games.Common.IO;

namespace Basketball;

internal class Clock
{
    private readonly IReadWrite _io;
    private int time;

    public Clock(IReadWrite io) => _io = io;

    public bool IsHalfTime => time == 50;
    public bool IsFullTime => time >= 100;
    public bool TwoMinutesLeft => time == 92;

    public void Increment(Scoreboard scoreboard)
    {
        time += 1;
        if (IsHalfTime) { scoreboard.Display(Resource.Formats.EndOfFirstHalf); }
        if (TwoMinutesLeft) { _io.Write(Resource.Streams.TwoMinutesLeft); }
    }

    public void StartOvertime() => time = 93;
}