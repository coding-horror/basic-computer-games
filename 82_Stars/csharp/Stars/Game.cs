using System;

namespace Stars
{
    internal class Game
    {
        private readonly int _maxNumber;
        private readonly int _maxGuessCount;
        private readonly Random _random;

        public Game(int maxNumber, int maxGuessCount)
        {
            _maxNumber = maxNumber;
            _maxGuessCount = maxGuessCount;
            _random = new Random();
        }

        internal void DisplayInstructions()
        {
            if (Input.GetString("Do you want instructions? ").Equals("N", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            Console.WriteLine($"I am thinking of a number between 1 and {_maxNumber}.");
            Console.WriteLine("Try to guess my number.  After you guess, I");
            Console.WriteLine("will type one or more stars (*).  The more");
            Console.WriteLine("stars I type, the close you are to my number.");
            Console.WriteLine("One star (*) means far away, seven stars (*******)");
            Console.WriteLine($"means really close!  You get {_maxGuessCount} guesses.");
        }

        internal void Play()
        {
            Console.WriteLine();
            Console.WriteLine();

            var target = _random.Next(_maxNumber) + 1;

            Console.WriteLine("Ok, I am thinking of a number.  Start guessing.");

            AcceptGuesses(target);
        }

        private void AcceptGuesses(int target)
        {
            for (int guessCount = 1; guessCount <= _maxGuessCount; guessCount++)
            {
                Console.WriteLine();
                var guess = Input.GetNumber("Your guess? ");

                if (guess == target)
                {
                    DisplayWin(guessCount);
                    return;
                }

                DisplayStars(target, guess);
            }

            DisplayLoss(target);
        }

        private static void DisplayStars(int target, float guess)
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

            Console.WriteLine(stars);
        }

        private static void DisplayWin(int guessCount)
        {
            Console.WriteLine();
            Console.WriteLine(new string('*', 79));
            Console.WriteLine();
            Console.WriteLine($"You got it in {guessCount} guesses!!!  Let's play again...");
        }

        private void DisplayLoss(int target)
        {
            Console.WriteLine();
            Console.WriteLine($"Sorry, that's {_maxGuessCount} guesses. The number was {target}.");
        }
    }
}