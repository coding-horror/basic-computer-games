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


        _io.WriteLine("Center jump");
        var offense = _random.NextFloat() > 0.6 ? homeTeam : visitingTeam;

        _io.WriteLine($"{offense} controls the tap.");

        var time = 0;
        var scoreboard = new Scoreboard(homeTeam, visitingTeam, _io);
        scoreboard.Offense = offense;

        _io.WriteLine();

        if (offense == homeTeam)
        {
            var shot = _io.ReadShot("Your shot");

            if (_random.NextFloat() >= 0.5 && time >= 100)
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
                    if (_random.NextFloat() <= 0.341 * defense / 8)
                    {
                        scoreboard.AddBasket("Shot is good");
                        // over to opponent
                    }
                    else if (_random.NextFloat() <= 0.682 * defense / 8)
                    {
                        _io.WriteLine("Shot is off target");
                        if (defense / 6 * _random.NextFloat() > 0.45)
                        {
                            _io.WriteLine($"Rebound to {visitingTeam}");
                            scoreboard.Turnover();
                            // over to opponent
                        }
                        else
                        {
                            _io.WriteLine("Dartmouth controls the rebound.");
                            if (_random.NextFloat() <= 0.4)
                            {
                                // fall through to 1300
                            }
                            else
                            {
                                if (defense == 6)
                                {
                                    if (_random.NextFloat() > 0.6)
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
                    else if (_random.NextFloat() <= 0.782 * defense / 8)
                    {
                        offense = _random.NextFloat() <= 0.5 ? homeTeam : visitingTeam;
                        scoreboard.Offense = offense;
                        _io.WriteLine($"Shot is blocked.  Ball controlled by {offense}.");
                        // go to next shot
                    }
                    else if (_random.NextFloat() <= 0.843 * defense / 8)
                    {
                        _io.WriteLine("Shooter is fouled.  Two shots.");
                        FreeThrows();
                        // over to opponent
                    }
                    else
                    {
                        _io.WriteLine("Charging foul.  Dartmouth loses ball.");
                        scoreboard.Turnover();
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

                if (7 / defense * _random.NextFloat() <= 0.4)
                {
                    scoreboard.AddBasket("Shot is good.  Two points.");
                    // over to opponent
                }
                else if (7 / defense * _random.NextFloat() <= 0.7)
                {
                    _io.WriteLine("Shot is off the rim.");
                    if (_random.NextFloat() <= 2 / 3f)
                    {
                        _io.WriteLine($"{visitingTeam} controls the rebound.");
                        scoreboard.Turnover();
                        // over to opponent
                    }
                    else
                    {
                        _io.WriteLine("Dartmouth controls the rebound");
                        if (_random.NextFloat() <= 0.4)
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
                else if (7 / defense * _random.NextFloat() <= 0.875)
                {
                    _io.WriteLine("Shooter fouled.  Two shots.");
                    FreeThrows();
                    // over to opponent
                }
                else if (7 / defense * _random.NextFloat() <= 0.925)
                {
                    _io.WriteLine($"Shot blocked. {visitingTeam}'s ball.");
                    scoreboard.Turnover();
                    // over to opponent
                }
                else
                {
                    _io.WriteLine("Charging foul.  Dartmouth loses ball.");
                    scoreboard.Turnover();
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

                if (8 / defense * _random.NextFloat() <= 0.35)
                {
                    scoreboard.AddBasket("Shot is good.");
                    // over to Dartmouth
                }
                else if (8 / defense * _random.NextFloat() <= 0.75)
                {
                    _io.WriteLine("Shot is off the rim.");
                    if (defense / 6 * _random.NextFloat() <= 0.5)
                    {
                        _io.WriteLine("Dartmouth controls the rebound.");
                        scoreboard.Turnover();
                        // over to Dartmouth
                    }
                    else
                    {
                        _io.WriteLine($"{visitingTeam} controls the rebound.");
                        if (defense == 6)
                        {
                            if (_random.NextFloat() > 0.75)
                            {
                                scoreboard.Turnover();
                                scoreboard.AddBasket("Ball stolen.  Easy lay up for Dartmouth.");
                                _io.WriteLine();
                                // next opponent shot
                            }
                        }
                        if (_random.NextFloat() <= 0.5)
                        {
                            _io.WriteLine($"Pass back to {visitingTeam} guard.");
                            // next opponent shot
                        }
                        // goto 3500
                    }
                }
                else if (8 / defense * _random.NextFloat() <= 0.9)
                {
                    _io.WriteLine("Player fouled.  Two shots.");
                    FreeThrows();
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

            if (7 / defense * _random.NextFloat() <= 0.413)
            {
                scoreboard.AddBasket("Shot is good.");
                // over to Dartmouth
            }
            else
            {
                _io.WriteLine("Shot is missed.");
                if (defense / 6 * _random.NextFloat() <= 0.5)
                {
                    _io.WriteLine("Dartmouth controls the rebound.");
                    scoreboard.Turnover();
                    // over to Dartmouth
                }
                else
                {
                    _io.WriteLine($"{visitingTeam} controls the rebound.");
                    if (defense == 6)
                    {
                        if (_random.NextFloat() > 0.75)
                        {
                            scoreboard.Turnover();
                            scoreboard.AddBasket("Ball stolen.  Easy lay up for Dartmouth.");
                            _io.WriteLine();
                            // next opponent shot
                        }
                    }
                    if (_random.NextFloat() <= 0.5)
                    {
                        _io.WriteLine($"Pass back to {visitingTeam} guard.");
                        // next opponent shot
                    }
                    // goto 3500
                }
            }
        }

        void FreeThrows()
        {
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
