using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicComputerGames.Bagels
{
	public enum BagelValidation
	{
		Valid,
		WrongLength,
		NotUnique,
		NonDigit
	};
	public class BagelNumber
	{
		private static readonly Random Rnd = new Random();

		private readonly int[] _digits;
		public override string ToString()
		{
			return String.Join('-', _digits);
		}

		public static BagelNumber CreateSecretNumber(int numDigits)
		{
			if (numDigits < 3 || numDigits > 9)
				throw new ArgumentOutOfRangeException(nameof(numDigits),
					"Number of digits must be between 3 and 9, inclusive");

			var digits = GetDigits(numDigits);
			return new BagelNumber(digits);
		}



		public static BagelValidation IsValid(string number, int length)
		{
			if (number.Length != length)
				return BagelValidation.WrongLength;

			if (!number.All(Char.IsDigit))
				return BagelValidation.NonDigit;

			if (new HashSet<char>(number).Count != length)
				return BagelValidation.NotUnique;

			return BagelValidation.Valid;
		}

		public BagelNumber(string number)
		{
			if (number.Any(d => !Char.IsDigit(d)))
				throw new ArgumentException("Number must be all unique digits", nameof(number));

			_digits = number.Select(d => d - '0').ToArray();
		}
			
		//public BagelNumber(long number)
		//{
		//	var digits = new List<int>();
		//	if (number >= 1E10)
		//		throw new ArgumentOutOfRangeException(nameof(number), "Number can be no more than 9 digits");

		//	while (number > 0)
		//	{
		//		long num = number / 10;
		//		int digit = (int)(number - (num * 10));
		//		number = num;
		//		digits.Add(digit);
		//	}

		//	_digits = digits.ToArray();
		//}

		public BagelNumber(int[] digits)
		{
			_digits = digits;
		}

		private static  int[] GetDigits(int numDigits)
		{
			int[] digits = {1, 2, 3, 4, 5, 6, 7, 8, 9};
			Shuffle(digits);
			return digits.Take(numDigits).ToArray();

		}

		private static void Shuffle(int[] digits)
		{
			for (int i = digits.Length - 1; i > 0; --i)
			{
				int pos = Rnd.Next(i);
				var t = digits[i];
				digits[i] = digits[pos];
				digits[pos] = t;
			}

		}

		public (int pico, int fermi) CompareTo(BagelNumber other)
		{
			int pico = 0;
			int fermi = 0;
			for (int i = 0; i < _digits.Length; i++)
			{
				for (int j = 0; j < other._digits.Length; j++)
				{
					if (_digits[i] == other._digits[j])
					{
						if (i == j)
							++fermi;
						else
							++pico;
					}
				}
			}

			return (pico, fermi);
		}
	}
}