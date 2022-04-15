using Basketball.Plays;
using Basketball.Resources;
using Games.Common.IO;
using Games.Common.Randomness;

namespace Basketball;

internal class Game
{
    private readonly Clock _clock;
    private readonly Scoreboard _scoreboard;
    private readonly TextIO _io;
    private readonly IRandom _random;

    private Game(Clock clock, Scoreboard scoreboard, TextIO io, IRandom random)
    {
        _clock = clock;
        _scoreboard = scoreboard;
        _io = io;
        _random = random;
    }

    public static Game Create(TextIO io, IRandom random)
    {
        io.Write(Resource.Streams.Introduction);

        var defense = new Defense(io.ReadDefense("Your starting defense will be"));
        var clock = new Clock(io);

        io.WriteLine();

        var scoreboard = new Scoreboard(
            new Team("Dartmouth", new HomeTeamPlay(io, random, clock, defense)),
            new Team(io.ReadString("Choose your opponent"), new VisitingTeamPlay(io, random, clock, defense)),
            io);

        return new Game(clock, scoreboard, io, random);
    }

    public void Play()
    {
        var ballContest = new BallContest(0.4f, "{0} controls the tap", _io, _random);

        while (true)
        {
            _io.WriteLine("Center jump");
            ballContest.Resolve(_scoreboard);

            _io.WriteLine();

            while (true)
            {
                var isFullTime = _scoreboard.Offense.ResolvePlay(_scoreboard);
                if (isFullTime && IsGameOver()) { return; }
                if (_clock.IsHalfTime) { break; }
            }
        }
    }

    private bool IsGameOver()
    {
        _io.WriteLine();
        if (_scoreboard.ScoresAreEqual)
        {
            _scoreboard.Display(Resource.Formats.EndOfSecondHalf);
            _clock.StartOvertime();
            return false;
        }

        _scoreboard.Display(Resource.Formats.EndOfGame);
        return true;
    }
}
