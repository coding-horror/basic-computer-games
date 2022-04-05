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

            while (scoreboard.Offense is not null)
            {
                var gameOver = scoreboard.Offense.ResolvePlay(scoreboard);
                if (gameOver) { return; }
                if (clock.IsHalfTime) { scoreboard.StartPeriod(); }
            }
        }
    }
}
