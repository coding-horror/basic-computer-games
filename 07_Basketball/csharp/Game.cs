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
            scoreboard.Offense = ContestBall(0.4f, scoreboard, "{0} controls the tap");

            _io.WriteLine();

            while (true)
            {
                scoreboard.Offense.ResolvePlay(scoreboard);
            }
        }
    }

    private Action<Scoreboard> HomeTeamPlay(Clock clock, Defense defense) => scoreboard =>
    {
        var shot = _io.ReadShot("Your shot");

        if (_random.NextFloat() >= 0.5f && clock.IsFullTime)
        {
            _io.WriteLine();
            if (scoreboard.ScoresAreEqual)
            {
                scoreboard.Display(Resource.Formats.EndOfSecondHalf);
                clock.StartOvertime();
                // Loop back to center jump
            }
            else
            {
                scoreboard.Display(Resource.Formats.EndOfGame);
                return;
            }
        }
        else
        {
            if (shot == 0)
            {
                defense.Set(_io.ReadDefense("Your new defensive alignment is"));
                // go to next shot
            }

            if (shot == 1 || shot == 2)
            {
                clock.Increment(scoreboard);
                if (clock.IsHalfTime)
                {
                    // Loop back to center jump;
                }
                _io.WriteLine("Jump shot");
                if (_random.NextFloat() <= 0.341f * defense / 8)
                {
                    scoreboard.AddBasket("Shot is good");
                    // over to opponent
                }
                else if (_random.NextFloat() <= 0.682f * defense / 8)
                {
                    _io.WriteLine("Shot is off target");
                    if (defense / 6 * _random.NextFloat() > 0.45f)
                    {
                        scoreboard.Turnover($"Rebound to {scoreboard.Visitors}");
                        // over to opponent
                    }
                    else
                    {
                        _io.WriteLine("Dartmouth controls the rebound.");
                        if (_random.NextFloat() <= 0.4f)
                        {
                            // fall through to 1300
                        }
                        else
                        {
                            if (defense == 6)
                            {
                                if (_random.NextFloat() > 0.6f)
                                {
                                    scoreboard.Turnover();
                                    scoreboard.AddBasket($"Pass stolen by {scoreboard.Visitors} easy layup.");
                                    _io.WriteLine();
                                }
                            }
                            _io.Write("Ball passed back to you. ");
                            // next shot without writeline
                        }
                    }
                }
                else if (_random.NextFloat() <= 0.782f * defense / 8)
                {
                    scoreboard.Offense = ContestBall(0.5f, scoreboard, "Shot is blocked.  Ball controlled by {0}.");
                    // go to next shot
                }
                else if (_random.NextFloat() <= 0.843f * defense / 8)
                {
                    FreeThrows(scoreboard, "Shooter is fouled.  Two shots.");
                    // over to opponent
                }
                else
                {
                    scoreboard.Turnover("Charging foul.  Dartmouth loses ball.");
                    // over to opponent
                }
            }
            // 1300
            clock.Increment(scoreboard);
            if (clock.IsHalfTime)
            {
                // Loop back to center jump;
            }

            _io.WriteLine(shot == 3 ? "Lay up." : "Set shot.");

            if (_random.NextFloat() <= 0.4f * defense / 7)
            {
                scoreboard.AddBasket("Shot is good.  Two points.");
                // over to opponent
            }
            else if (_random.NextFloat() <= 0.7f * defense / 7)
            {
                _io.WriteLine("Shot is off the rim.");
                if (_random.NextFloat() <= 2 / 3f)
                {
                    scoreboard.Turnover($"{scoreboard.Visitors} controls the rebound.");
                    // over to opponent
                }
                else
                {
                    _io.WriteLine("Dartmouth controls the rebound");
                    if (_random.NextFloat() <= 0.4f)
                    {
                        // goto 1300
                    }
                    else
                    {
                        _io.WriteLine("Ball passed back to you.");
                        // go to next shot
                    }
                }
            }
            else if (_random.NextFloat() <= 0.875f * defense / 7)
            {
                FreeThrows(scoreboard, "Shooter fouled.  Two shots.");
                // over to opponent
            }
            else if (_random.NextFloat() <= 0.925f * defense / 7)
            {
                scoreboard.Turnover($"Shot blocked. {scoreboard.Visitors}'s ball.");
                // over to opponent
            }
            else
            {
                scoreboard.Turnover("Charging foul.  Dartmouth loses ball.");
                // over to opponent
            }
        }
    };

    private Action<Scoreboard> VisitingTeamPlay(Clock clock, Defense defense) => scoreboard =>
    {
        clock.Increment(scoreboard);
        if (clock.IsHalfTime)
        {
            // Loop back to center jump;
        }

        _io.WriteLine();
        var shot = _random.NextFloat(1, 3.5f);
        if (shot <= 2)
        {
            _io.WriteLine("Jump shot.");

            if (_random.NextFloat() <= 0.35f * defense / 8)
            {
                scoreboard.AddBasket("Shot is good.");
                // over to Dartmouth
            }
            else if (_random.NextFloat() <= 0.75f * defense / 8)
            {
                _io.WriteLine("Shot is off the rim.");
                if (_random.NextFloat() <= 0.5f / defense * 6)
                {
                    scoreboard.Turnover("Dartmouth controls the rebound.");
                    // over to Dartmouth
                }
                else
                {
                    _io.WriteLine($"{scoreboard.Visitors} controls the rebound.");
                    if (defense == 6)
                    {
                        if (_random.NextFloat() <= 0.25f)
                        {
                            scoreboard.Turnover();
                            scoreboard.AddBasket("Ball stolen.  Easy lay up for Dartmouth.");
                            _io.WriteLine();
                            // next opponent shot
                        }
                    }
                    if (_random.NextFloat() <= 0.5f)
                    {
                        _io.WriteLine($"Pass back to {scoreboard.Visitors} guard.");
                        // next opponent shot
                    }
                    // goto 3500
                }
            }
            else if (_random.NextFloat() <= 0.9f * defense / 8)
            {
                FreeThrows(scoreboard, "Player fouled.  Two shots.");
                // next Dartmouth shot
            }
            else
            {
                _io.WriteLine("Offensive foul.  Dartmouth's ball.");
                // next Dartmouth shot
            }
        }

        // 3500
        _io.WriteLine(shot > 3 ? "Set shot." : "Lay up.");

        if (_random.NextFloat() <= 0.413f * defense / 7)
        {
            scoreboard.AddBasket("Shot is good.");
            // over to Dartmouth
        }
        else
        {
            _io.WriteLine("Shot is missed.");
            if (_random.NextFloat() <= 0.5f * 6 / defense)
            {
                scoreboard.Turnover("Dartmouth controls the rebound.");
                // over to Dartmouth
            }
            else
            {
                _io.WriteLine($"{scoreboard.Visitors} controls the rebound.");
                if (defense == 6)
                {
                    if (_random.NextFloat() <= 0.25f)
                    {
                        scoreboard.Turnover();
                        scoreboard.AddBasket("Ball stolen.  Easy lay up for Dartmouth.");
                        _io.WriteLine();
                        // next opponent shot
                    }
                }
                if (_random.NextFloat() <= 0.5f)
                {
                    _io.WriteLine($"Pass back to {scoreboard.Visitors} guard.");
                    // next opponent shot
                }
                // goto 3500
            }
        }
    };

    Team ContestBall(float probability, Scoreboard scoreboard, string messageFormat)
    {
        var winner = _random.NextFloat() <= probability ? scoreboard.Home : scoreboard.Visitors;
        _io.WriteLine(messageFormat, winner);
        return winner;
    }

    void FreeThrows(Scoreboard scoreboard, string message)
    {
        _io.WriteLine(message);

        if (_random.NextFloat() <= 0.49)
        {
            scoreboard.AddFreeThrows(2, "Shooter makes both shots.");
        }
        else if (_random.NextFloat() <= 0.75)
        {
            scoreboard.AddFreeThrows(1, "Shooter makes one shot and misses one.");
        }
        else
        {
            scoreboard.AddFreeThrows(0, "Both shots missed.");
        }
    }
}
