using System;
using System.Collections.Generic;

namespace BasicComputerGames.Dice
{
	public class RollGenerator
	{
		static Random _rnd = new Random();

		public static void ReseedRNG(int seed) => _rnd = new Random(seed);

		public IEnumerable<(int die1, int die2)> Rolls()
		{
			while (true)
			{
				yield return (_rnd.Next(1, 7), _rnd.Next(1, 7));
			}
		}
	}
}