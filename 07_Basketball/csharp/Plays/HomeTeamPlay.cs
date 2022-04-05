using Basketball.Resources;
using Games.Common.IO;
using Games.Common.Randomness;

namespace Basketball.Plays;

internal class HomeTeamPlay : Play
{
    private readonly TextIO _io;
    private readonly IRandom _random;
    private readonly Clock _clock;
    private readonly Defense _defense;

    public HomeTeamPlay(TextIO io, IRandom random, Clock clock, Defense defense)
        : base(io, random)
    {
        _io = io;
        _random = random;
        _clock = clock;
        _defense = defense;
    }

    internal override bool Resolve(Scoreboard scoreboard)
    {
        var shot = _io.ReadShot("Your shot");

        if (_random.NextFloat() >= 0.5f && _clock.IsFullTime)
        {
            _io.WriteLine();
            if (!scoreboard.ScoresAreEqual)
            {
                scoreboard.Display(Resource.Formats.EndOfGame);
                return true;
            }

            scoreboard.Display(Resource.Formats.EndOfSecondHalf);
            _clock.StartOvertime();
            scoreboard.StartPeriod();
            return false;
        }

        if (shot == 0)
        {
            _defense.Set(_io.ReadDefense("Your new defensive alignment is"));
            _io.WriteLine();
            return false;
        }

        var playContinues = shot >= 3;

        if (shot == 1 || shot == 2)
        {
            _clock.Increment(scoreboard);
            if (_clock.IsHalfTime) { return false; }

            var ballContest = new BallContest(0.5f, "Shot is blocked.  Ball controlled by {0}.", _io, _random);

            Resolve("Jump shot", _defense / 8)
                .Do(0.341f, () => scoreboard.AddBasket("Shot is good"))
                .Or(0.682f, () => ResolveShotOffTarget())
                .Or(0.782f, () => ballContest.Resolve(scoreboard))
                .Or(0.843f, () => ResolveFreeThrows(scoreboard, "Shooter is fouled.  Two shots."))
                .Or(() => scoreboard.Turnover("Charging foul.  Dartmouth loses ball."));
        }

        while (playContinues)
        {
            _clock.Increment(scoreboard);
            if (_clock.IsHalfTime) { return false; }

            playContinues = false;

            Resolve(shot == 3 ? "Lay up." : "Set shot.", _defense / 7)
                .Do(0.4f, () => scoreboard.AddBasket("Shot is good.  Two points."))
                .Or(0.7f, () => ResolveShotOffTheRim())
                .Or(0.875f, () => ResolveFreeThrows(scoreboard, "Shooter fouled.  Two shots."))
                .Or(0.925f, () => scoreboard.Turnover($"Shot blocked. {scoreboard.Visitors}'s ball."))
                .Or(() => scoreboard.Turnover("Charging foul.  Dartmouth loses ball."));
        }

        return false;

        void ResolveShotOffTarget() =>
            Resolve("Shot is off target", 6 / _defense)
                .Do(0.45f, () => Resolve("Dartmouth controls the rebound.")
                    .Do(0.4f, () => playContinues = true)
                    .Or(() =>
                    {
                        if (_defense == 6 && _random.NextFloat() > 0.6f)
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
    }
}
