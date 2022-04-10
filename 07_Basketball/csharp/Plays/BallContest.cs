using Games.Common.IO;
using Games.Common.Randomness;

namespace Basketball.Plays;

internal class BallContest
{
    private readonly float _probability;
    private readonly string _messageFormat;
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    internal BallContest(float probability, string messageFormat, IReadWrite io, IRandom random)
    {
        _io = io;
        _probability = probability;
        _messageFormat = messageFormat;
        _random = random;
    }

    internal bool Resolve(Scoreboard scoreboard)
    {
        var winner = _random.NextFloat() <= _probability ? scoreboard.Home : scoreboard.Visitors;
        scoreboard.Offense = winner;
        _io.WriteLine(_messageFormat, winner);
        return false;
    }
}
