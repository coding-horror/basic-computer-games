using System;
using Games.Common.IO;
using Games.Common.Randomness;
using Stars.Resources;

namespace Stars;

internal class Game
{
    private readonly TextIO _io;
    private readonly IRandom _random;
    private readonly int _maxNumber;
    private readonly int _maxGuessCount;

    public Game(TextIO io, IRandom random, int maxNumber, int maxGuessCount)
    {
        _io = io;
        _random = random;
        _maxNumber = maxNumber;
        _maxGuessCount = maxGuessCount;
    }

    internal void Play(Func<bool> playAgain)
    {
        DisplayIntroduction();

        do
        {
            Play();
        } while (playAgain.Invoke());
    }

    private void DisplayIntroduction()
    {
        _io.Write(Resource.Streams.Title);

        if (_io.ReadString("Do you want instructions").Equals("N", StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        _io.WriteLine(Resource.Formats.Instructions, _maxNumber, _maxGuessCount);
    }

    private void Play()
    {
        _io.WriteLine();
        _io.WriteLine();

        var target = _random.Next(_maxNumber) + 1;

        _io.WriteLine("Ok, I am thinking of a number.  Start guessing.");

        AcceptGuesses(target);
    }

    private void AcceptGuesses(int target)
    {
        for (int guessCount = 1; guessCount <= _maxGuessCount; guessCount++)
        {
            _io.WriteLine();
            var guess = _io.ReadNumber("Your guess");

            if (guess == target)
            {
                DisplayWin(guessCount);
                return;
            }

            DisplayStars(target, guess);
        }

        DisplayLoss(target);
    }

    private void DisplayStars(int target, float guess)
    {
        var stars = Math.Abs(guess - target) switch
        {
            >= 64 => "*",
            >= 32 => "**",
            >= 16 => "***",
            >= 8  => "****",
            >= 4  => "*****",
            >= 2  => "******",
            _     => "*******"
        };

        _io.WriteLine(stars);
    }

    private void DisplayWin(int guessCount)
    {
        _io.WriteLine();
        _io.WriteLine(new string('*', 79));
        _io.WriteLine();
        _io.WriteLine($"You got it in {guessCount} guesses!!!  Let's play again...");
    }

    private void DisplayLoss(int target)
    {
        _io.WriteLine();
        _io.WriteLine($"Sorry, that's {_maxGuessCount} guesses. The number was {target}.");
    }
}
