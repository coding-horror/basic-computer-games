namespace Guess;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    public void Play()
    {
        while (true)
        {
            _io.Write(Streams.Introduction);

            var limit = _io.ReadNumber(Prompts.Limit);
            _io.WriteLine();

            // There's a bug here that exists in the original code. 
            // If the limit entered is <= 0 then the program will crash.
            var targetGuessCount = checked((int)Math.Log2(limit) + 1);

            PlayGuessingRounds(limit, targetGuessCount);

            _io.Write(Streams.BlankLines);
        }
    }

    private void PlayGuessingRounds(float limit, int targetGuessCount)
    {
        while (true)
        {
            _io.WriteLine(Formats.Thinking, limit);

            // There's a bug here that exists in the original code. If a non-integer is entered as the limit
            // then it's possible for the secret number to be the next integer greater than the limit.
            var secretNumber = (int)_random.NextFloat(limit) + 1;

            var guessCount = 0;

            while (true)
            {
                var guess = _io.ReadNumber("");
                if (guess <= 0) { return; }
                guessCount++;
                if (IsGuessCorrect(guess, secretNumber)) { break; }
            }

            ReportResult(guessCount, targetGuessCount);

            _io.Write(Streams.BlankLines);
        }
    }

    private bool IsGuessCorrect(float guess, int secretNumber)
    {
        if (guess < secretNumber) { _io.Write(Streams.TooLow); }
        if (guess > secretNumber) { _io.Write(Streams.TooHigh); }

        return guess == secretNumber;
    }

    private void ReportResult(int guessCount, int targetGuessCount)
    {
        _io.WriteLine(Formats.ThatsIt, guessCount);
        _io.WriteLine(
            (guessCount - targetGuessCount) switch
            {
                < 0 => Strings.VeryGood,
                0 => Strings.Good,
                > 0 => string.Format(Formats.ShouldHave, targetGuessCount)
            });
    }
}