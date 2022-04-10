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
        var shot = _random.NextShot();

        if (shot is JumpShot jumpShot)
        {
            Resolve(jumpShot, scoreboard);
            _io.WriteLine();
        }

        _playContinues |= shot is not JumpShot;

        while (_playContinues)
        {
            _playContinues = false;
            Resolve(shot, scoreboard);
            _io.WriteLine();
        }

        return false;
    }

    private void Resolve(JumpShot shot, Scoreboard scoreboard) =>
        Resolve(shot.ToString(), _defense / 8)
            .Do(0.35f, () => scoreboard.AddBasket("Shot is good."))
            .Or(0.75f, () => ResolveBadShot(scoreboard, "Shot is off the rim.", _defense * 6))
            .Or(0.9f, () => ResolveFreeThrows(scoreboard, "Player fouled.  Two shots."))
            .Or(() => _io.WriteLine("Offensive foul.  Dartmouth's ball."));

    private void Resolve(Shot shot, Scoreboard scoreboard) =>
        Resolve(shot.ToString(), _defense / 7)
            .Do(0.413f, () => scoreboard.AddBasket("Shot is good."))
            .Or(() => ResolveBadShot(scoreboard, "Shot is missed.", 6 / _defense));

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
