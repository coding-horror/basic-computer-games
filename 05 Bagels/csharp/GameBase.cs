using System;

namespace BasicComputerGames.Bagels
{
	public class GameBase
	{
		protected Random Rnd { get; } = new Random();

		/// <summary>
		/// Prompt the player to try again, and wait for them to press Y or N.
		/// </summary>
		/// <returns>Returns true if the player wants to try again, false if they have finished playing.</returns>
		protected bool TryAgain()
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