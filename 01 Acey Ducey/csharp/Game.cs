using System;
using System.Collections.Generic;
using System.Text;

namespace AceyDucey
{
    /// <summary>
    /// The main class that implements all the game logic
    /// </summary>
    internal class Game
    {
        /// <summary>
        /// Our Random number generator object
        /// </summary>
        private Random Rnd { get; } = new Random();

        /// <summary>
        /// A line of underscores that we'll print between turns to separate them from one another on screen
        /// </summary>
        private string SeparatorLine { get; } = new string('_', 70);


        /// <summary>
        /// Main game loop function. This will play the game endlessly until the player chooses to quit.
        /// </summary>
        internal void GameLoop()
        {
            // First display instructions to the player
            DisplayIntroText();

            // We'll loop for each game until the player decides not to continue
            do
            {
                // Play a game!
                PlayGame();

                // Play again?
            } while (TryAgain());
        }

        /// <summary>
        /// Play the game
        /// </summary>
        private void PlayGame()
        {
            GameState state = new GameState();

            // Clear the display
            Console.Clear();
            // Keep looping until the player has no money left
            do
            {
                // Play the next turn. Pass in our state object so the turn can see the money available,
                // can update it after the player makes a bet, and can update the turn count.
                PlayTurn(state);

                // Keep looping until the player runs out of money
            } while (state.Money > 0);

            // Looks like the player is bankrupt, let them know how they did.
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine($"Sorry, friend, but you blew your wad. Your game is over after {state.TurnCount} {(state.TurnCount == 1 ? "turn" : "turns")}. Your highest balance was ${state.MaxMoney}.");
        }


        /// <summary>
        /// Play a turn
        /// </summary>
        /// <param name="state">The current game state</param>
        private void PlayTurn(GameState state)
        {
            // Let the player know what's happening
            Console.WriteLine("");
            Console.WriteLine(SeparatorLine);
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("Here are your next two cards:");

            // Generate two random cards
            int firstCard = GetCard();
            int secondCard = GetCard();

            // If the second card is lower than the first card, swap them over
            if (secondCard < firstCard)
            {
                (firstCard, secondCard) = (secondCard, firstCard);
            }

            // Display the cards
            DisplayCard(firstCard);
            DisplayCard(secondCard);

            // Ask the player what they want to do
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.Write("You currently have ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"${state.Money}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(". How much would you like to bet?");

            // Read the bet amount
            int betAmount = PlayTurn_GetBetAmount(state.Money);

            // Display a summary of their inpout
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("");
            Console.WriteLine($"You choose to {(betAmount == 0 ? "pass" : $"bet {betAmount}")}.");

            // Generate and display the final card
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
            Console.WriteLine("The next card is:");

            int thirdCard = GetCard();
            DisplayCard(thirdCard);
            Console.WriteLine("");

            // Was the third card between the first two cards?
            if (thirdCard > firstCard && thirdCard < secondCard)
            {
                // It was! Inform the player and add to their money
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You win!");
                if (betAmount == 0)
                {
                    Console.WriteLine("(It's just a shame you chose not to bet!)");
                }
                else
                {
                    state.Money += betAmount;
                    // If their money exceeds the MaxMoney, update that too
                    state.MaxMoney = Math.Max(state.Money, state.MaxMoney);
                }
            }
            else
            {
                // Oh dear, the player lost. Let them know the bad news and take their bet from their money
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You lose!");
                if (betAmount == 0)
                {
                    Console.WriteLine("(It's lucky you chose not to bet!)");
                }
                else
                {
                    state.Money -= betAmount;
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("You now have ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"${state.Money}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(".");

            // Update the turn count now that another turn has been played
            state.TurnCount += 1;

            // Ready for the next turn...
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Prompt the user for their bet amount and validate their input
        /// </summary>
        /// <param name="currentMoney">The player's current money</param>
        /// <returns>Returns the amount the player chooses to bet</returns>
        private int PlayTurn_GetBetAmount(int currentMoney)
        {
            int betAmount;
            // Loop until the user enters a valid value
            do
            {
                // Move this to a separate function...
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("> $");
                string input = Console.ReadLine();

                // Is this a valid number?
                if (!int.TryParse(input, out betAmount))
                {
                    // No
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, I didn't understand. Please enter how much you would like to bet.");
                    // Continue looping
                    continue;
                }

                // If the amount between 0 and their available money?
                if (betAmount < 0 || betAmount > currentMoney)
                {
                    // No
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Please enter a bet amount between $0 and ${currentMoney}.");
                    // Continue looping
                    continue;
                }

                // We have a valid bet, stop looping
                break;
            } while (true);

            // Return whatever the player entered
            return betAmount;
        }

        /// <summary>
        /// Generate a new random card.
        /// </summary>
        /// <returns>Will return a value between 2 and 14, inclusive.</returns>
        /// <remarks>Values 2 to 10 are their face values. 11 represents a Jack, 12 is a Queen, 13 a King and 14 an Ace.
        /// Even though this is a slightly offset sequence, it allows us to perform a simple greater-than/less-than
        /// comparison with the card values, treating an Ace as a high card.</remarks>
        private int GetCard()
        {
            return Rnd.Next(2, 15);
        }

        /// <summary>
        /// Display the card number on screen, translating values 11 through to 14 into their named equivalents.
        /// </summary>
        /// <param name="card"></param>
        private void DisplayCard(int card)
        {
            string cardText;
            switch (card)
            {
                case 11:
                    cardText = "Jack";
                    break;
                case 12:
                    cardText = "Queen";
                    break;
                case 13:
                    cardText = "King";
                    break;
                case 14:
                    cardText = "Ace";
                    break;
                default:
                    cardText = card.ToString();
                    break;
            }

            // Format as black text on a white background
            Console.Write("   ");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write($"  {cardText}  ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }

        /// <summary>
        /// Display instructions on how to play the game and wait for the player to press a key.
        /// </summary>
        private void DisplayIntroText()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Acey Ducey Gard Game.");
            Console.WriteLine("Creating Computing, Morristown, New Jersey.");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Originally published in 1978 in the book 'Basic Computer Games' by David Ahl.");
            Console.WriteLine("Modernised and converted to C# in 2021 by Adam Dawes (@AdamDawes575).");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Acey Ducey is played in the following manner:");
            Console.WriteLine("");
            Console.WriteLine("The dealer (computer) deals two cards, face up.");
            Console.WriteLine("");
            Console.WriteLine("You have an option to bet or pass, depending on whether or not you feel the next card will have a value between the");
            Console.WriteLine("first two.");
            Console.WriteLine("");
            Console.WriteLine("If the card is between, you will win your stake, otherwise you will lose it. Ace is 'high' (higher than a King).");
            Console.WriteLine("");
            Console.WriteLine("If you want to pass, enter a bet amount of $0.");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press any key start the game.");
            Console.ReadKey(true);

        }

        /// <summary>
        /// Prompt the player to try again, and wait for them to press Y or N.
        /// </summary>
        /// <returns>Returns true if the player wants to try again, false if they have finished playing.</returns>
        private bool TryAgain()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Would you like to try again? (Press 'Y' for yes or 'N' for no)");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("> ");

            char pressedKey;
            // Keep looping until we get a recognised input
            do
            {
                // Read a key, don't display it on screen
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Convert to upper-case so we don't need to care about capitalisation
                pressedKey = Char.ToUpper(key.KeyChar);
                // Is this a key we recognise? If not, keep looping
            } while (pressedKey != 'Y' && pressedKey != 'N');
            // Display the result on the screen
            Console.WriteLine(pressedKey);

            // Return true if the player pressed 'Y', false for anything else.
            return (pressedKey == 'Y');
        }

    }
}
