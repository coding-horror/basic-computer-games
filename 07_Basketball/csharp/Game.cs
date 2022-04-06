using Basketball.Plays;
using Basketball.Resources;
using Games.Common.IO;
using Games.Common.Randomness;

namespace Basketball;

internal class Game
{
    private readonly TextIO _io;
    private readonly IRandom _random;

    public Game(TextIO io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    public void Play()
    {
        _io.Write(Resource.Streams.Introduction);

        var defense = new Defense(_io.ReadDefense("Your starting defense will be"));
        var clock = new Clock(_io);

        _io.WriteLine();

        var scoreboard = new Scoreboard(
            new Team("Dartmouth", new HomeTeamPlay(_io, _random, clock, defense)),
            new Team(_io.ReadString("Choose your opponent"), new VisitingTeamPlay(_io, _random, clock, defense)),
            _io);

        var ballContest = new BallContest(0.4f, "{0} controls the tap", _io, _random);

        while (true)
        {
            _io.WriteLine("Center jump");
            ballContest.Resolve(scoreboard);

            _io.WriteLine();

            while (true)
            {
                var isFullTime = scoreboard.Offense.ResolvePlay(scoreboard);
                if (isFullTime && IsGameOver(scoreboard, clock)) { return; }
                if (clock.IsHalfTime) { break; }
            }
        }
    }

    private bool IsGameOver(Scoreboard scoreboard, Clock clock)
    {
        _io.WriteLine();
        if (scoreboard.ScoresAreEqual)
        {
            scoreboard.Display(Resource.Formats.EndOfSecondHalf);
            clock.StartOvertime();
            return false;
        }

        scoreboard.Display(Resource.Formats.EndOfGame);
        return true;
    }
}
