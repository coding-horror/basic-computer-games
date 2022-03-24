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

        _io.WriteLine();
        var opponent = _io.ReadString("Choose your opponent");

        _io.WriteLine("Center jump");
        var offense = _random.NextFloat() > 0.6 ? "Dartmouth" : opponent;

        _io.WriteLine($"{offense} controls the tap.");

        var time = 0;
        var score = new Dictionary<string, int> { ["Dartmouth"] = 0, [opponent] = 0 };

        _io.WriteLine();

        if (offense == "Dartmouth")
        {
            var shot = _io.ReadShot("Your shot");

            if (_random.NextFloat() >= 0.5 && time >= 100)
            {
                _io.WriteLine();
                if (score["Dartmouth"] == score[opponent])
                {
                    _io.WriteScore(Resource.Formats.EndOfSecondHalf, opponent, score);
                    time = 93;
                    // Loop back to center jump
                }
                else
                {
                    _io.WriteScore(Resource.Formats.EndOfGame, opponent, score);
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
                        _io.WriteScore(Resource.Formats.EndOfFirstHalf, opponent, score);
                        // Loop back to center jump;
                    }
                    if (time == 92)
                    {
                        _io.Write(Resource.Streams.TwoMinutesLeft);
                    }
                    _io.WriteLine("Jump shot");
                    if (_random.NextFloat() <= 0.341 * defense / 8)
                    {
                        _io.WriteLine("Shot is good");
                        score["Dartmouth"] += 2;
                        _io.WriteScore(Resource.Formats.Score, opponent, score);
                        // over to opponent
                    }
                    else if (_random.NextFloat() <= 0.682 * defense / 8)
                    {
                        _io.WriteLine("Shot is off target");
                        if (defense / 6 * _random.NextFloat() > 0.45)
                        {
                            _io.WriteLine($"Rebound to {opponent}");
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
                                        _io.WriteLine($"Pass stolen be {opponent} easy layup.");
                                        score[opponent] += 2;
                                        _io.WriteScore(Resource.Formats.Score, opponent, score);
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
                        offense = _random.NextFloat() <= 0.5 ? "Dartmouth" : opponent;
                        _io.WriteLine($"Shot is blocked.  Ball controlled by {offense}.");
                        // go to next shot
                    }
                    else if (_random.NextFloat() <= 0.843 * defense / 8)
                    {
                        _io.WriteLine("Shooter is fouled.  Two shots.");
                        FreeShots();
                        // over to opponent
                    }
                    else
                    {
                        _io.WriteLine("Charging foul.  Dartmouth loses ball.");
                        // over to opponent
                    }
                }
                // 1300
                time++;
                if (time == 50)
                {
                    _io.WriteScore(Resource.Formats.EndOfFirstHalf, opponent, score);
                    // Loop back to center jump;
                }
                if (time == 92)
                {
                    _io.Write(Resource.Streams.TwoMinutesLeft);
                }

                _io.WriteLine(shot == 3 ? "Lay up." : "Set shot.");

                if (7 / defense * _random.NextFloat() <= 0.4)
                {
                    _io.WriteLine("Shot is good.  Two points.");
                    score["Dartmouth"] += 2;
                    _io.WriteScore(Resource.Formats.Score, opponent, score);
                    // over to opponent
                }
                else if (7 / defense * _random.NextFloat() <= 0.7)
                {
                    _io.WriteLine("Shot is off the rim.");
                    if (_random.NextFloat() <= 2 / 3f)
                    {
                        _io.WriteLine($"{opponent} controls the rebound.");
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
                    FreeShots();
                    // over to opponent
                }
                else if (7 / defense * _random.NextFloat() <= 0.925)
                {
                    _io.WriteLine($"Shot blocked. {opponent}'s ball.");
                    // over to opponent
                }
                else
                {
                    _io.WriteLine("Charging foul.  Dartmouth loses ball.");
                    // over to opponent
                }
            }
        }
        else
        {
            time++;
            if (time == 50)
            {
                _io.WriteScore(Resource.Formats.EndOfFirstHalf, opponent, score);
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
                    _io.WriteLine("Shot is good.");
                    score[opponent] += 2;
                    _io.WriteScore(Resource.Formats.Score, opponent, score);
                    // over to Dartmouth
                }
                else if (8 / defense * _random.NextFloat() <= 0.75)
                {
                    _io.WriteLine("Shot is off the rim.");
                    if (defense / 6 * _random.NextFloat() <= 0.5)
                    {
                        _io.WriteLine("Dartmouth controls the rebound.");
                        // over to Dartmouth
                    }
                    else
                    {
                        _io.WriteLine($"{opponent} controls the rebound.");
                        if (defense == 6)
                        {
                            if (_random.NextFloat() > 0.75)
                            {
                                _io.WriteLine("Ball stolen.  Easy lay up for Dartmouth.");
                                score["Dartmouth"] += 2;
                                _io.WriteScore(Resource.Formats.Score, opponent, score);
                                _io.WriteLine();
                                // next opponent shot
                            }
                        }
                        if (_random.NextFloat() <= 0.5)
                        {
                            _io.WriteLine($"Pass back to {opponent} guard.");
                            // next opponent shot
                        }
                        // goto 3500
                    }
                }
                else if (8 / defense * _random.NextFloat() <= 0.9)
                {
                    _io.WriteLine("Player fouled.  Two shots.");
                    FreeShots();
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
                _io.WriteLine("Shot is good.");
                score[opponent] += 2;
                _io.WriteScore(Resource.Formats.Score, opponent, score);
                // over to Dartmouth
            }
            else
            {
                _io.WriteLine("Shot is missed.");
                if (defense / 6 * _random.NextFloat() <= 0.5)
                {
                    _io.WriteLine("Dartmouth controls the rebound.");
                    // over to Dartmouth
                }
                else
                {
                    _io.WriteLine($"{opponent} controls the rebound.");
                    if (defense == 6)
                    {
                        if (_random.NextFloat() > 0.75)
                        {
                            _io.WriteLine("Ball stolen.  Easy lay up for Dartmouth.");
                            score["Dartmouth"] += 2;
                            _io.WriteScore(Resource.Formats.Score, opponent, score);
                            _io.WriteLine();
                            // next opponent shot
                        }
                    }
                    if (_random.NextFloat() <= 0.5)
                    {
                        _io.WriteLine($"Pass back to {opponent} guard.");
                        // next opponent shot
                    }
                    // goto 3500
                }
            }
        }

        void FreeShots()
        {
            if (_random.NextFloat() <= 0.49)
            {
                _io.WriteLine("Shooter makes both shots.");
                score[offense] += 2;
            }
            else if (_random.NextFloat() <= 0.75)
            {
                _io.WriteLine("Shooter makes one shot and misses one.");
                score[offense] += 1;
            }
            else
            {
                _io.WriteLine("Both shots missed.");
            }
            _io.WriteScore(Resource.Formats.Score, opponent, score);
        }
    }
}

internal record Team(string Name);

internal static class IReadWriteExtensions
{
    public static float ReadDefense(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var defense = io.ReadNumber(prompt);
            if (defense >= 6) { return defense; }
        }
    }

    public static int ReadShot(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var shot = io.ReadNumber(prompt);
            if ((int)shot == shot && shot >= 0 && shot <= 4) { return (int)shot; }
            io.Write("Incorrect answer.  Retype it. ");
        }
    }

    public static void WriteScore(this IReadWrite io, string format, string opponent, Dictionary<string ,int> score) =>
        io.WriteLine(format, "Dartmouth", score["Dartmouth"], opponent, score[opponent]);
}