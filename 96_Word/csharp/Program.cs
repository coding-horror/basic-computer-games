using System;
using System.Linq;
using System.Text;

namespace word
{
    class Word
    {
        // Here's the list of potential words that could be selected
        // as the winning word.
        private string[] words = { "DINKY", "SMOKE", "WATER", "GRASS", "TRAIN", "MIGHT", "FIRST",
         "CANDY", "CHAMP", "WOULD", "CLUMP", "DOPEY" };

        /// <summary>
        /// Outputs the instructions of the game.
        /// </summary>
        private void intro()
        {
            Console.WriteLine("WORD".PadLeft(37));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".PadLeft(59));

            Console.WriteLine("I am thinking of a word -- you guess it. I will give you");
            Console.WriteLine("clues to help you get it. Good luck!!");
        }

        /// <summary>
        /// This allows the user to enter a guess - doing some basic validation
        /// on those guesses.
        /// </summary>
        /// <returns>The guess entered by the user</returns>
        private string get_guess()
        {
            string guess = "";

            while (guess.Length == 0)
            {
                Console.WriteLine($"{Environment.NewLine}Guess a five letter word. ");
                guess = Console.ReadLine().ToUpper();

                if ((guess.Length != 5) || (guess.Equals("?")) || (!guess.All(char.IsLetter)))
                {
                    guess = "";
                    Console.WriteLine("You must guess a five letter word. Start again.");
                }
            }

            return guess;
        }

        /// <summary>
        /// This checks the user's guess against the target word - capturing
        /// any letters that match up between the two as well as the specific
        /// letters that are correct.
        /// </summary>
        /// <param name="guess">The user's guess</param>
        /// <param name="target">The 'winning' word</param>
        /// <param name="progress">A string showing which specific letters have already been guessed</param>
        /// <returns>The integer value showing the number of character matches between guess and target</returns>
        private int check_guess(string guess, string target, StringBuilder progress)
        {
            // Go through each letter of the guess and see which
            // letters match up to the target word.
            // For each position that matches, update the progress
            // to reflect the guess
            int matches = 0;
            string common_letters = "";

            for (int ctr = 0; ctr < 5; ctr++)
            {
                // First see if this letter appears anywhere in the target
                // and, if so, add it to the common_letters list.
                if (target.Contains(guess[ctr]))
                {
                    common_letters.Append(guess[ctr]);
                }
                // Then see if this specific letter matches the
                // same position in the target. And, if so, update
                // the progress tracker
                if (guess[ctr].Equals(target[ctr]))
                {
                    progress[ctr] = guess[ctr];
                    matches++;
                }
            }

            Console.WriteLine($"There were {matches} matches and the common letters were... {common_letters}");
            Console.WriteLine($"From the exact letter matches, you know......... {progress}");
            return matches;
        }

        /// <summary>
        /// This plays one full game.
        /// </summary>
        private void play_game()
        {
            string guess_word, target_word;
            StringBuilder guess_progress = new StringBuilder("-----");
            Random rand = new Random();
            int count = 0;

            Console.WriteLine("You are starting a new game...");

            // Randomly select a word from the list of words
            target_word = words[rand.Next(words.Length)];

            // Just run as an infinite loop until one of the
            // endgame conditions are met.
            while (true)
            {
                // Ask the user for their guess
                guess_word = get_guess();
                count++;

                // If they enter a question mark, then tell them
                // the answer and quit the game
                if (guess_word.Equals("?"))
                {
                    Console.WriteLine($"The secret word is {target_word}");
                    return;
                }

                // Otherwise, check the guess against the target - noting progress
                if (check_guess(guess_word, target_word, guess_progress) == 0)
                {
                    Console.WriteLine("If you give up, type '?' for your next guess.");
                }

                // Once they've guess the word, end the game.
                if (guess_progress.Equals(guess_word))
                {
                    Console.WriteLine($"You have guessed the word.  It took {count} guesses!");
                    return;
                }
            }
        }

        /// <summary>
        /// The main entry point for the class - just keeps
        /// playing the game until the user decides to quit.
        /// </summary>
        public void play()
        {
            intro();

            bool keep_playing = true;

            while (keep_playing)
            {
                play_game();
                Console.WriteLine($"{Environment.NewLine}Want to play again? ");
                keep_playing = Console.ReadLine().StartsWith("y", StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new Word().play();
        }
    }
}
