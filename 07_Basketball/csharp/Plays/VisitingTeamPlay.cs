using Games.Common.IO;
using Games.Common.Randomness;

namespace Basketball.Plays;

internal class VisitingTeamPlay : Play
{
    private readonly TextIO _io;
    private readonly IRandom _random;
    private readonly Defense _defense;
    private bool _playContinues;

    public VisitingTeamPlay(TextIO io, IRandom random, Clock clock, Defense defense)
        : base(io, random, clock)
    {
        _io = io;
        _random = random;
        _defense = defense;
    }

    internal override bool Resolve(Scoreboard scoreboard)
    {
        if (ClockIncrementsToHalfTime(scoreboard)) { return false; }

        _io.WriteLine();
        var shot = _random.NextFloat(1, 3.5f);

        if (shot <= 2)
        {
            ResolveJumpShot(scoreboard);
        }

        _playContinues |= shot > 2;

        while (_playContinues)
        {
            ResolveLayupOrSetShot(scoreboard, shot);
        }

        return false;
    }

    private void ResolveJumpShot(Scoreboard scoreboard)
    {
        Resolve("Jump shot.", _defense / 8)
            .Do(0.35f, () => scoreboard.AddBasket("Shot is good."))
            .Or(0.75f, () => ResolveBadShot(scoreboard, "Shot is off the rim.", _defense * 6))
            .Or(0.9f, () => ResolveFreeThrows(scoreboard, "Player fouled.  Two shots."))
            .Or(() => _io.WriteLine("Offensive foul.  Dartmouth's ball."));
        _io.WriteLine();
    }

    private void ResolveLayupOrSetShot(Scoreboard scoreboard, float shot)
    {
        _playContinues = false;

        Resolve(shot > 3 ? "Set shot." : "Lay up.", _defense / 7)
            .Do(0.413f, () => scoreboard.AddBasket("Shot is good."))
            .Or(() => ResolveBadShot(scoreboard, "Shot is missed.", 6 / _defense));
        _io.WriteLine();
    }

    void ResolveBadShot(Scoreboard scoreboard, string message, float defenseFactor) =>
        Resolve(message, defenseFactor)
            .Do(0.5f, () => scoreboard.Turnover("Dartmouth controls the rebound."))
            .Or(() => ResolveVisitorsRebound(scoreboard));

    void ResolveVisitorsRebound(Scoreboard scoreboard)
    {
        _io.Write($"{scoreboard.Visitors} controls the rebound.");
        if (_defense == 6 && _random.NextFloat() <= 0.25f)
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

        _playContinues = true;
    }
}
