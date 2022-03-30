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

        var defense = _io.ReadDefense("Your starting defense will be");

        var homeTeam = new Team("Dartmouth");

        _io.WriteLine();
        var visitingTeam = new Team(_io.ReadString("Choose your opponent"));

        var time = 0;
        var scoreboard = new Scoreboard(homeTeam, visitingTeam, _io);

        _io.WriteLine("Center jump");
        scoreboard.Offense = ContestBall(0.6f, visitingTeam, homeTeam, "{0} controls the tap");

        _io.WriteLine();

        if (scoreboard.Offense == homeTeam)
        {
            var shot = _io.ReadShot("Your shot");

            if (_random.NextFloat() >= 0.5f && time >= 100)
            {
                _io.WriteLine();
                if (scoreboard.ScoresAreEqual)
                {
                    scoreboard.Display(Resource.Formats.EndOfSecondHalf);
                    time = 93;
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
                    defense = _io.ReadDefense("Your new defensive alignment is");
                    // go to next shot
                }

                if (shot == 1 || shot == 2)
                {
                    time++;
                    if (time == 50)
                    {
                        scoreboard.Display(Resource.Formats.EndOfFirstHalf);
                        // Loop back to center jump;
                    }
                    if (time == 92)
                    {
                        _io.Write(Resource.Streams.TwoMinutesLeft);
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
                            scoreboard.Turnover($"Rebound to {visitingTeam}");
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
                                        scoreboard.AddBasket($"Pass stolen by {visitingTeam} easy layup.");
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
                        scoreboard.Offense =
                            ContestBall(0.5f, homeTeam, visitingTeam, "Shot is blocked.  Ball controlled by {0}.");
                        // go to next shot
                    }
                    else if (_random.NextFloat() <= 0.843f * defense / 8)
                    {
                        FreeThrows("Shooter is fouled.  Two shots.");
                        // over to opponent
                    }
                    else
                    {
                        scoreboard.Turnover("Charging foul.  Dartmouth loses ball.");
                        // over to opponent
                    }
                }
                // 1300
                time++;
                if (time == 50)
                {
                    scoreboard.Display(Resource.Formats.EndOfFirstHalf);
                    // Loop back to center jump;
                }
                if (time == 92)
                {
                    _io.Write(Resource.Streams.TwoMinutesLeft);
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
                        scoreboard.Turnover($"{visitingTeam} controls the rebound.");
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
                    FreeThrows("Shooter fouled.  Two shots.");
                    // over to opponent
                }
                else if (_random.NextFloat() <= 0.925f * defense / 7)
                {
                    scoreboard.Turnover($"Shot blocked. {visitingTeam}'s ball.");
                    // over to opponent
                }
                else
                {
                    scoreboard.Turnover("Charging foul.  Dartmouth loses ball.");
                    // over to opponent
                }
            }
        }
        else
        {
            time++;
            if (time == 50)
            {
                scoreboard.Display(Resource.Formats.EndOfFirstHalf);
                // Loop back to center jump;
            }
            if (time == 92)
            {
                _io.Write(Resource.Streams.TwoMinutesLeft);
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
                        _io.WriteLine($"{visitingTeam} controls the rebound.");
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
                            _io.WriteLine($"Pass back to {visitingTeam} guard.");
                            // next opponent shot
                        }
                        // goto 3500
                    }
                }
                else if (_random.NextFloat() <= 0.9f * defense / 8)
                {
                    FreeThrows("Player fouled.  Two shots.");
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
                    _io.WriteLine($"{visitingTeam} controls the rebound.");
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
                        _io.WriteLine($"Pass back to {visitingTeam} guard.");
                        // next opponent shot
                    }
                    // goto 3500
                }
            }
        }

        Team ContestBall(float probability, Team a, Team b, string messageFormat)
        {
            var winner = _random.NextFloat() <= probability ? a : b;
            _io.WriteLine(messageFormat, winner);
            return winner;
        }

        void FreeThrows(string message)
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
}
