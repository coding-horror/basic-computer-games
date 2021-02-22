using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Hangman
{
    /// <summary>
    /// C# version of the game "Hangman" from the book BASIC Computer Games.
    /// </summary>
    static class Program
    {
        static void Main()
        {
            Console.WriteLine(Tab(32) + "HANGMAN");
            Console.WriteLine(Tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            MainLoop();
            Console.WriteLine();
            Console.WriteLine("IT'S BEEN FUN!  BYE FOR NOW.");
        }

        static void MainLoop()
        {
            var words = GetWords();
            var stillPlaying = true;

            while (stillPlaying)
            {
                if (words.Count == 0)
                {
                    Console.WriteLine("YOU DID ALL THE WORDS!!");
                    break;
                }

                // Get a random number from 0 to the number of words we have minus one (C# arrays are zero-based).
                var rnd = new Random();
                var randomNumber = rnd.Next(words.Count - 1);

                // Pick a random word and remove it from the list.
                var word = words[randomNumber];
                words.Remove(word);

                GameLoop(word);
                
                // Game finished. Ask if player wants another one.
                Console.WriteLine("WANT ANOTHER WORD? ");
                var response = Console.ReadLine();
                if (response == null || response.ToUpper() != "YES")
                {
                    stillPlaying = false;   // Exit the loop if the player didn't answer "yes".
                }
            }
        }

        static void GameLoop(string word)
        {
            var graphic = new Graphic();
            var wrongGuesses = 0;
            var numberOfGuesses = 0;
            var usedLetters = new List<char>();

            // The word that the user sees. Since we just started, it's just dashes.
            var displayedWord = new char[word.Length];
            for (var i = 0; i < word.Length; i++)
            {
                displayedWord[i] = '-';
            }

            var stillPlaying = true;
            while (stillPlaying)
            {
                var guess = GetLetterFromPlayer(displayedWord, usedLetters);
                usedLetters.Add(guess);
                numberOfGuesses++;
                var correctLetterCount = 0;
                // Now we check every letter in the word to see if the player guessed any of them correctly.
                for(var i = 0; i < word.Length; i++)
                {
                    if (word[i] == guess)
                    {
                        correctLetterCount++;
                        displayedWord[i] = guess;
                    }
                }

                if (correctLetterCount == 0)
                {
                    // Wrong guess.
                    Console.WriteLine("SORRY, THAT LETTER ISN'T IN THE WORD.");
                    wrongGuesses++;
                    DrawBody(graphic, wrongGuesses);
                    if (wrongGuesses == 10)
                    {
                        // Player exhausted all their guesses. Finish the game loop.
                        Console.WriteLine($"SORRY, YOU LOSE.  THE WORD WAS {word}");
                        Console.Write("YOU MISSED THAT ONE.  DO YOU ");
                        stillPlaying = false;
                    }
                }
                else
                {
                    // Player guessed a correct letter. Let's see if there are any unguessed letters left in the word.
                    if (displayedWord.Contains('-'))
                    {
                        Console.WriteLine(displayedWord);
                        
                        // Give the player a chance to guess the whole word.
                        var wordGuess = GetWordFromPlayer();
                        if (word == wordGuess)
                        {
                            // Player found the word. Mark it found.
                            Console.WriteLine("YOU FOUND THE WORD!");
                            stillPlaying = false;   // Exit game loop.
                        }
                        else
                        {
                            // Player didn't guess the word. Continue the game loop.
                            Console.WriteLine("WRONG.  TRY ANOTHER LETTER.");
                        }
                    }
                    else
                    {
                        // Player guessed all the letters.
                        Console.WriteLine("YOU FOUND THE WORD!");
                        stillPlaying = false;   // Exit game loop.
                    }
                }
            } // End of game loop.
        }

        /// <summary>
        /// Display the current state of the word and all the already guessed letters, and get a new guess from the player
        /// </summary>
        /// <param name="displayedWord">A char array that represents the current state of the guessed word</param>
        /// <param name="usedLetters">A list of chars that represents all the letters guessed so far</param>
        /// <returns>The letter that the player has just entered as a guess</returns>
        private static char GetLetterFromPlayer(char[] displayedWord, List<char> usedLetters)
        {
            while (true)    // Infinite loop, unless the player enters an unused letter.
            {
                Console.WriteLine();
                Console.WriteLine(displayedWord);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("HERE ARE THE LETTERS YOU USED:");
                for (var i = 0; i < usedLetters.Count; i++)
                {
                    Console.Write(usedLetters[i]);

                    // If it's not the last letter, print a comma.
                    if (i != usedLetters.Count - 1)
                    {
                        Console.Write(",");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("WHAT IS YOUR GUESS?");
                var guess = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
                
                if (usedLetters.Contains(guess))
                {
                    // After this the loop will continue.
                    Console.WriteLine("YOU GUESSED THAT LETTER BEFORE!");
                }
                else
                {
                    // Break out of the loop by returning guessed letter.
                    return guess;
                }
            }
        }

        /// <summary>
        /// Gets a word guess from the player.
        /// </summary>
        /// <returns>The guessed word.</returns>
        private static string GetWordFromPlayer()
        {
            while (true)    // Infinite loop, unless the player enters something.
            {
                Console.WriteLine("WHAT IS YOUR GUESS FOR THE WORD? ");
                var guess = Console.ReadLine();
                if (guess != null)
                {
                    return guess.ToUpper();
                }
            }
        }

        /// <summary>
        /// Draw body after wrong guess.
        /// </summary>
        /// <param name="graphic">The instance of the Graphic class being used.</param>
        /// <param name="wrongGuesses">Number of wrong guesses.</param>
        private static void DrawBody(Graphic graphic, int wrongGuesses)
        {
            switch (wrongGuesses)
                    {
                        case 1:
                            Console.WriteLine("FIRST, WE DRAW A HEAD.");
                            graphic.AddHead();
                            break;
                        case 2:
                            Console.WriteLine("NOW WE DRAW A BODY.");
                            graphic.AddBody();
                            break;
                        case 3:
                            Console.WriteLine("NEXT WE DRAW AN ARM.");
                            graphic.AddRightArm();
                            break;
                        case 4:
                            Console.WriteLine("THIS TIME IT'S THE OTHER ARM.");
                            graphic.AddLeftArm();
                            break;
                        case 5:
                            Console.WriteLine("NOW, LET'S DRAW THE RIGHT LEG.");
                            graphic.AddRightLeg();
                            break;
                        case 6:
                            Console.WriteLine("THIS TIME WE DRAW THE LEFT LEG.");
                            graphic.AddLeftLeg();
                            break;
                        case 7:
                            Console.WriteLine("NOW WE PUT UP A HAND.");
                            graphic.AddRightHand();
                            break;
                        case 8:
                            Console.WriteLine("NEXT THE OTHER HAND.");
                            graphic.AddLeftHand();
                            break;
                        case 9:
                            Console.WriteLine("NOW WE DRAW ONE FOOT.");
                            graphic.AddRightFoot();
                            break;
                        case 10:
                            Console.WriteLine("HERE'S THE OTHER FOOT -- YOU'RE HUNG!!");
                            graphic.AddLeftFoot();
                            break;
                    }
                    graphic.Print();
        }
        
        /// <summary>
        /// Get a list of words to use in the game.
        /// </summary>
        /// <returns>List of strings.</returns>
        private static List<string> GetWords() => new()
        {
            "GUM",
            "SIN",
            "FOR",
            "CRY",
            "LUG",
            "BYE",
            "FLY",
            "UGLY",
            "EACH",
            "FROM",
            "WORK",
            "TALK",
            "WITH",
            "SELF",
            "PIZZA",
            "THING",
            "FEIGN",
            "FIEND",
            "ELBOW",
            "FAULT",
            "DIRTY",
            "BUDGET",
            "SPIRIT",
            "QUAINT",
            "MAIDEN",
            "ESCORT",
            "PICKAX",
            "EXAMPLE",
            "TENSION",
            "QUININE",
            "KIDNEY",
            "REPLICA",
            "SLEEPER",
            "TRIANGLE",
            "KANGAROO",
            "MAHOGANY",
            "SERGEANT",
            "SEQUENCE",
            "MOUSTACHE",
            "DANGEROUS",
            "SCIENTIST",
            "DIFFERENT",
            "QUIESCENT",
            "MAGISTRATE",
            "ERRONEOUSLY",
            "LOUDSPEAKER",
            "PHYTOTOXIC",
            "MATRIMONIAL",
            "PARASYMPATHOMIMETIC",
            "THIGMOTROPISM"
        };

        /// <summary>
        /// Leave a number of spaces empty.
        /// </summary>
        /// <param name="length">Number of spaces.</param>
        /// <returns>The result string.</returns>
        private static string Tab(int length) => new string(' ', length);
    }
}