using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BasicComputerGames.Bagels
{
	public class Game : GameBase
	{
		public void GameLoop()
		{
			DisplayIntroText();
			int points = 0;
			do
			{
				var result =PlayRound();
				if (result)
					++points;
			} while (TryAgain());

			Console.WriteLine();
			Console.WriteLine($"A {points} point Bagels buff!!");
			Console.WriteLine("Hope you had fun. Bye.");
		}

		private const int Length = 3;
		private const int MaxGuesses = 20;

		private bool  PlayRound()
		{
			var secret = BagelNumber.CreateSecretNumber(Length);
			Console.WriteLine("O.K. I have a number in mind.");
			for (int guessNo = 1; guessNo <= MaxGuesses; ++guessNo)
			{
				string strGuess;
				BagelValidation isValid;
				do
				{
					Console.WriteLine($"Guess #{guessNo}");
					strGuess = Console.ReadLine();
					isValid = BagelNumber.IsValid(strGuess, Length);
					PrintError(isValid);
				} while (isValid != BagelValidation.Valid);

				var guess = new BagelNumber(strGuess);
				var fermi = 0;
				var pico = 0;
				(pico, fermi) = secret.CompareTo(guess);
				if(pico + fermi == 0)
					Console.Write("BAGELS!");
				else if (fermi == Length)
				{
					Console.WriteLine("You got it!");
					return true;
				}
				else
				{
					PrintList("Pico ", pico);
					PrintList("Fermi ", fermi);
				}
				Console.WriteLine();
			}

			Console.WriteLine("Oh, well.");
			Console.WriteLine($"That's {MaxGuesses} guesses.  My Number was {secret}");

			return false;

		}

		private void PrintError(BagelValidation isValid)
		{
			switch (isValid)
			{
				case BagelValidation.NonDigit:
					Console.WriteLine("What?");
					break;

				case BagelValidation.NotUnique:
					Console.WriteLine("Oh, I forgot to tell you that the number I have in mind has no two digits the same.");
					break;

				case BagelValidation.WrongLength:
					Console.WriteLine($"Try guessing a {Length}-digit number.");
					break;

				case BagelValidation.Valid:
					break;
			}
		}

		private void PrintList(string msg, int repeat)
		{
			for(int i=0; i<repeat; ++i)
				Console.Write(msg);
		}

		private void DisplayIntroText()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Bagels");
			Console.WriteLine("Creating Computing, Morristown, New Jersey.");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine(
				"Original code author unknow but suspected to be from Lawrence Hall of Science, U.C. Berkley");
			Console.WriteLine("Originally published in 1978 in the book 'Basic Computer Games' by David Ahl.");
			Console.WriteLine("Modernized and converted to C# in 2021 by James Curran (noveltheory.com).");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("I am thinking of a three-digit number.  Try to guess");
			Console.WriteLine("my number and I will give you clues as follows:");
			Console.WriteLine("   pico   - One digit correct but in the wrong position");
			Console.WriteLine("   fermi  - One digit correct and in the right position");
			Console.WriteLine("   bagels - No digits correct");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Press any key start the game.");
			Console.ReadKey(true);
		}
	}
}