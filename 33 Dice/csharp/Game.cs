using System;
using System.Linq;

namespace BasicComputerGames.Dice
{
	public class Game
	{
		private readonly RollGenerator _roller = new RollGenerator();

		public void GameLoop()
		{
			DisplayIntroText();

			// RollGenerator.ReseedRNG(1234);		// hard-code seed for repeatabilty during testing

			do
			{
				int numRolls = GetInput();
				var counter = CountRolls(numRolls);
				DisplayCounts(counter);
			} while (TryAgain());
		}

		private void DisplayIntroText()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Dice");
			Console.WriteLine("Creating Computing, Morristown, New Jersey."); Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine("Original code by Danny Freidus.");
			Console.WriteLine("Originally published in 1978 in the book 'Basic Computer Games' by David Ahl.");
			Console.WriteLine("Modernized and converted to C# in 2021 by James Curran (noveltheory.com).");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("This program simulates the rolling of a pair of dice.");
			Console.WriteLine("You enter the number of times you want the computer to");
			Console.WriteLine("'roll' the dice. Watch out, very large numbers take");
			Console.WriteLine("a long time. In particular, numbers over 10 million.");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Press any key start the game.");
			Console.ReadKey(true);
		}

		private int GetInput()
		{
			int num = -1;
			Console.WriteLine();
			do
			{
				Console.WriteLine();
				Console.Write("How many rolls? ");
			} while (!Int32.TryParse(Console.ReadLine(), out num));

			return num;
		}

		private  void DisplayCounts(int[] counter)
		{
			Console.WriteLine();
			Console.WriteLine($"\tTotal\tTotal Number");
			Console.WriteLine($"\tSpots\tof Times");
			Console.WriteLine($"\t===\t=========");
			for (var n = 1; n < counter.Length; ++n)
			{
				Console.WriteLine($"\t{n + 1,2}\t{counter[n],9:#,0}");
			}
			Console.WriteLine();
		}

		private  int[] CountRolls(int x)
		{
			var counter = _roller.Rolls().Take(x).Aggregate(new int[12], (cntr, r) =>
			{
				cntr[r.die1 + r.die2 - 1]++;
				return cntr;
			});
			return counter;
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