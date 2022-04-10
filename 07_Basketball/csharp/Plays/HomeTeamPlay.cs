using Games.Common.IO;
using Games.Common.Randomness;

namespace Basketball.Plays;

internal class HomeTeamPlay : Play
{
    private readonly TextIO _io;
    private readonly IRandom _random;
    private readonly Clock _clock;
    private readonly Defense _defense;
    private readonly BallContest _ballContest;

    public HomeTeamPlay(TextIO io, IRandom random, Clock clock, Defense defense)
        : base(io, random, clock)
    {
        _io = io;
        _random = random;
        _clock = clock;
        _defense = defense;
        _ballContest = new BallContest(0.5f, "Shot is blocked.  Ball controlled by {0}.", _io, _random);
    }

    internal override bool Resolve(Scoreboard scoreboard)
    {
        var shot = _io.ReadShot("Your shot");

        if (_random.NextFloat() >= 0.5f && _clock.IsFullTime) { return true; }

        if (shot is null)
        {
            _defense.Set(_io.ReadDefense("Your new defensive alignment is"));
            _io.WriteLine();
            return false;
        }

        if (shot is JumpShot jumpShot)
        {
            if (ClockIncrementsToHalfTime(scoreboard)) { return false; }
            if (!Resolve(jumpShot, scoreboard)) { return false; }
        }

        do
        {
            if (ClockIncrementsToHalfTime(scoreboard)) { return false; }
        } while (Resolve(shot, scoreboard));

        return false;
    }

    // The Resolve* methods resolve the probabilistic outcome of the current game state.
    // They return true if the Home team should continue the play and attempt a layup, false otherwise.
    private bool Resolve(JumpShot shot, Scoreboard scoreboard) =>
        Resolve(shot.ToString(), _defense / 8)
            .Do(0.341f, () => scoreboard.AddBasket("Shot is good"))
            .Or(0.682f, () => ResolveShotOffTarget(scoreboard))
            .Or(0.782f, () => _ballContest.Resolve(scoreboard))
            .Or(0.843f, () => ResolveFreeThrows(scoreboard, "Shooter is fouled.  Two shots."))
            .Or(() => scoreboard.Turnover($"Charging foul.  {scoreboard.Home} loses ball."));

    private bool Resolve(Shot shot, Scoreboard scoreboard) =>
        Resolve(shot.ToString(), _defense / 7)
            .Do(0.4f, () => scoreboard.AddBasket("Shot is good.  Two points."))
            .Or(0.7f, () => ResolveShotOffTheRim(scoreboard))
            .Or(0.875f, () => ResolveFreeThrows(scoreboard, "Shooter fouled.  Two shots."))
            .Or(0.925f, () => scoreboard.Turnover($"Shot blocked. {scoreboard.Visitors}'s ball."))
            .Or(() => scoreboard.Turnover($"Charging foul.  {scoreboard.Home} loses ball."));

    private bool ResolveShotOffTarget(Scoreboard scoreboard) =>
        Resolve("Shot is off target", 6 / _defense)
            .Do(0.45f, () => ResolveHomeRebound(scoreboard, ResolvePossibleSteal))
            .Or(() => scoreboard.Turnover($"Rebound to {scoreboard.Visitors}"));

    private bool ResolveHomeRebound(Scoreboard scoreboard, Action<Scoreboard> endOfPlayAction) =>
        Resolve($"{scoreboard.Home} controls the rebound.")
            .Do(0.4f, () => true)
            .Or(() => endOfPlayAction.Invoke(scoreboard));
    private void ResolvePossibleSteal(Scoreboard scoreboard)
    {
        if (_defense == 6 && _random.NextFloat() > 0.6f)
        {
            scoreboard.Turnover();
            scoreboard.AddBasket($"Pass stolen by {scoreboard.Visitors} easy layup.");
            _io.WriteLine();
        }
        _io.Write("Ball passed back to you. ");
    }

    private void ResolveShotOffTheRim(Scoreboard scoreboard) =>
        Resolve("Shot is off the rim.")
            .Do(2 / 3f, () => scoreboard.Turnover($"{scoreboard.Visitors} controls the rebound."))
            .Or(() => ResolveHomeRebound(scoreboard, _ => _io.WriteLine("Ball passed back to you.")));
}
