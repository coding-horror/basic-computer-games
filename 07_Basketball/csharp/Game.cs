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
            new Team("Dartmouth", HomeTeamPlay(clock, defense)),
            new Team(_io.ReadString("Choose your opponent"), VisitingTeamPlay(clock, defense)),
            _io);

        while (true)
        {
            _io.WriteLine("Center jump");
            ContestBall(0.4f, scoreboard, "{0} controls the tap");

            _io.WriteLine();

            while (scoreboard.Offense is not null)
            {
                var gameOver = scoreboard.Offense.ResolvePlay(scoreboard);
                if (gameOver) { return; }
                if (clock.IsHalfTime) { scoreboard.StartPeriod(); }
            }
        }
    }

    private Func<Scoreboard, bool> HomeTeamPlay(Clock clock, Defense defense) => scoreboard =>
    {
        var shot = _io.ReadShot("Your shot");

        if (_random.NextFloat() >= 0.5f && clock.IsFullTime)
        {
            _io.WriteLine();
            if (!scoreboard.ScoresAreEqual)
            {
                scoreboard.Display(Resource.Formats.EndOfGame);
                return true;
            }

            scoreboard.Display(Resource.Formats.EndOfSecondHalf);
            clock.StartOvertime();
            scoreboard.StartPeriod();
            return false;
        }

        if (shot == 0)
        {
            defense.Set(_io.ReadDefense("Your new defensive alignment is"));
            _io.WriteLine();
            return false;
        }

        var playContinues = shot >= 3;

        if (shot == 1 || shot == 2)
        {
            clock.Increment(scoreboard);
            if (clock.IsHalfTime) { return false; }

            Resolve("Jump shot", defense / 8)
                .Do(0.341f, () => scoreboard.AddBasket("Shot is good"))
                .Or(0.682f, () => ResolveShotOffTarget())
                .Or(0.782f, () => ContestBall(0.5f, scoreboard, "Shot is blocked.  Ball controlled by {0}."))
                .Or(0.843f, () => ResolveFreeThrows(scoreboard, "Shooter is fouled.  Two shots."))
                .Or(() => scoreboard.Turnover("Charging foul.  Dartmouth loses ball."));
        }

        while (playContinues)
        {
            clock.Increment(scoreboard);
            if (clock.IsHalfTime) { return false; }

            playContinues = false;

            Resolve(shot == 3 ? "Lay up." : "Set shot.", defense / 7)
                .Do(0.4f, () => scoreboard.AddBasket("Shot is good.  Two points."))
                .Or(0.7f, () => ResolveShotOffTheRim())
                .Or(0.875f, () => ResolveFreeThrows(scoreboard, "Shooter fouled.  Two shots."))
                .Or(0.925f, () => scoreboard.Turnover($"Shot blocked. {scoreboard.Visitors}'s ball."))
                .Or(() => scoreboard.Turnover("Charging foul.  Dartmouth loses ball."));
        }

        return false;

        void ResolveShotOffTarget() =>
            Resolve("Shot is off target", 6 / defense)
                .Do(0.45f, () => Resolve("Dartmouth controls the rebound.")
                    .Do(0.4f, () => playContinues = true)
                    .Or(() =>
                    {
                        if (defense == 6 && _random.NextFloat() > 0.6f)
                        {
                            scoreboard.Turnover();
                            scoreboard.AddBasket($"Pass stolen by {scoreboard.Visitors} easy layup.");
                            _io.WriteLine();
                        }
                        _io.Write("Ball passed back to you. ");
                    }))
                .Or(() => scoreboard.Turnover($"Rebound to {scoreboard.Visitors}"));

        void ResolveShotOffTheRim() =>
            Resolve("Shot is off the rim.")
                .Do(2 / 3f, () => scoreboard.Turnover($"{scoreboard.Visitors} controls the rebound."))
                .Or(() => Resolve("Dartmouth controls the rebound")
                    .Do(0.4f, () => playContinues = true)
                    .Or(() => _io.WriteLine("Ball passed back to you.")));
    };

    private Func<Scoreboard, bool> VisitingTeamPlay(Clock clock, Defense defense) => scoreboard =>
    {
        clock.Increment(scoreboard);
        if (clock.IsHalfTime) { return false; }


        _io.WriteLine();
        var shot = _random.NextFloat(1, 3.5f);
        var playContinues = shot > 2;

        if (shot <= 2)
        {
            Resolve("Jump shot.", defense / 8)
                .Do(0.35f, () => scoreboard.AddBasket("Shot is good."))
                .Or(0.75f, () => ResolveBadShot("Shot is off the rim.", defense * 6))
                .Or(0.9f, () => ResolveFreeThrows(scoreboard, "Player fouled.  Two shots."))
                .Or(() => _io.WriteLine("Offensive foul.  Dartmouth's ball."));

            _io.WriteLine();
        }

        while (playContinues)
        {
            playContinues = false;

            Resolve(shot > 3 ? "Set shot." : "Lay up.", defense / 7)
                .Do(0.413f, () => scoreboard.AddBasket("Shot is good."))
                .Or(() => ResolveBadShot("Shot is missed.", 6 / defense));
            _io.WriteLine();
        }

        return false;

        void ResolveBadShot(string message, float defenseFactor) =>
            Resolve("Shot is off the rim.", defense * 6)
                .Do(0.5f, () => scoreboard.Turnover("Dartmouth controls the rebound."))
                .Or(() => ResolveVisitorsRebound());

        void ResolveVisitorsRebound()
        {
            _io.Write($"{scoreboard.Visitors} controls the rebound.");
            if (defense == 6 && _random.NextFloat() <= 0.25f)
            {
                _io.WriteLine();
                scoreboard.Turnover();
                scoreboard.AddBasket("Ball stolen.  Easy lay up for Dartmouth.");
                return;
            }

            if (_random.NextFloat() <= 0.5f)
            {
                _io.WriteLine();
                _io.Write($"Pass back to {scoreboard.Visitors} guard.");
                return;
            }

            playContinues = true;
        }
    };

    void ContestBall(float probability, Scoreboard scoreboard, string messageFormat)
    {
        var winner = _random.NextFloat() <= probability ? scoreboard.Home : scoreboard.Visitors;
        scoreboard.Offense = winner;
        _io.WriteLine(messageFormat, winner);
    }

    void ResolveFreeThrows(Scoreboard scoreboard, string message) =>
        Resolve(message)
            .Do(0.49f, () => scoreboard.AddFreeThrows(2, "Shooter makes both shots."))
            .Or(0.75f, () => scoreboard.AddFreeThrows(1, "Shooter makes one shot and misses one."))
            .Or(() => scoreboard.AddFreeThrows(0, "Both shots missed."));

    private Probably Resolve(string message) => Resolve(message, 1f);

    private Probably Resolve(string message, float defenseFactor)
    {
        _io.WriteLine(message);
        return new Probably(defenseFactor, _random);
    }
}
