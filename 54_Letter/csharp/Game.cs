namespace Letter
{
    internal static class Game
    {
        /// <summary>
        /// Maximum number of guesses.
        /// Note the program doesn't enforce this - it just displays a message if this is exceeded.
        /// </summary>
        private const int MaximumGuesses = 5;

        /// <summary>
        /// Main game loop.
        /// </summary>
        public static void Play()
        {
            DisplayIntroductionText();

            // Keep playing forever, or until the user quits.
            while (true)
            {
                PlayRound();
            }
        }

        /// <summary>
        /// Play a single round.
        /// </summary>
        internal static void PlayRound()
        {
            var gameState = new GameState();
            DisplayRoundIntroduction();

            char letterInput = '\0'; // Set the initial character to something that's not A-Z.
            while (letterInput != gameState.Letter)
            {
                letterInput = GetCharacterFromKeyboard();
                gameState.GuessesSoFar++;
                DisplayGuessResult(gameState.Letter, letterInput);
            }
            DisplaySuccessMessage(gameState);
        }

        /// <summary>
        /// Display an introduction when the game loads.
        /// </summary>
        internal static void DisplayIntroductionText()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("LETTER");
            Console.WriteLine("Creative Computing, Morristown, New Jersey.");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Letter Guessing Game");
            Console.WriteLine("I'll think of a letter of the alphabet, A to Z.");
            Console.WriteLine("Try to guess my letter and I'll give you clues");
            Console.WriteLine("as to how close you're getting to my letter.");
            Console.WriteLine("");

            Console.ResetColor();
        }

        /// <summary>
        /// Display introductionary text for each round.
        /// </summary>
        internal static void DisplayRoundIntroduction()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("O.K., I have a letter. Start guessing.");

            Console.ResetColor();
        }

        /// <summary>
        /// Display text depending whether the guess is lower or higher.
        /// </summary>
        internal static void DisplayGuessResult(char letterToGuess, char letterInput)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(" " + letterInput + " ");

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" ");
            if (letterInput != letterToGuess)
            {
                if (letterInput > letterToGuess)
                {
                    Console.WriteLine("Too high. Try a lower letter");
                }
                else
                {
                    Console.WriteLine("Too low. Try a higher letter");
                }
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Display success, and the number of guesses.
        /// </summary>
        internal static void DisplaySuccessMessage(GameState gameState)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"You got it in {gameState.GuessesSoFar} guesses!!");
            if (gameState.GuessesSoFar > MaximumGuesses)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"But it shouldn't take more than {MaximumGuesses} guesses!");
            }
            else
            {
                Console.WriteLine("Good job !!!!!");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine("Let's play again.....");

            Console.ResetColor();
        }

        /// <summary>
        /// Get valid input from the keyboard: must be an alpha character. Converts to upper case if necessary.
        /// </summary>
        internal static char GetCharacterFromKeyboard()
        {
            char letterInput;
            do
            {
                var keyPressed = Console.ReadKey(true);
                letterInput = Char.ToUpper(keyPressed.KeyChar); // Convert to upper case immediately.
            } while (!Char.IsLetter(letterInput)); // If the input is not a letter, wait for another letter to be pressed.
            return letterInput;
        }
    }
}
