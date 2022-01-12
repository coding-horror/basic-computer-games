using System;
using System.Text;

namespace Reverse
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintTitle();
            Console.Write("DO YOU WANT THE RULES? ");
            var needRulesInput = Console.ReadLine();

            if (string.Equals(needRulesInput, "YES", StringComparison.OrdinalIgnoreCase))
            {
                DisplayRules();
            }

            var tryAgain = string.Empty;
            while (!string.Equals(tryAgain, "NO", StringComparison.OrdinalIgnoreCase))
            {
                var array = Reverser.CreateRandomArray(9);
                Console.WriteLine(PrintArrayContents(array));
                var arrayIsNotInAscendingOrder = true;
                var numberOfMoves = 0;
                while (arrayIsNotInAscendingOrder)
                {
                    int index = ReadNextInput();

                    if (index == 0)
                    {
                        break;
                    }

                    Reverser.Reverse(array, index);
                    Console.WriteLine(PrintArrayContents(array));

                    if (Reverser.IsArrayInAscendingOrder(array))
                    {
                        arrayIsNotInAscendingOrder = false;
                        Console.WriteLine($"YOU WON IT IN {numberOfMoves} MOVES!!!");
                    }
                    numberOfMoves++;
                }

                Console.Write("TRY AGAIN (YES OR NO)");
                tryAgain = Console.ReadLine();
            }

            Console.WriteLine("OK HOPE YOU HAD FUN");
        }

        private static int ReadNextInput()
        {
            Console.Write("HOW MANY SHALL I REVERSE? ");
            var input = ReadIntegerInput();
            while (input > 9 || input < 0)
            {
                if (input > 9)
                {
                    Console.WriteLine("OOPS! TOO MANY! I CAN REVERSE AT MOST THIS MANY");
                }

                if (input < 0)
                {
                    Console.WriteLine("OOPS! TOO FEW! I CAN REVERSE BETWEEN 1 AND THIS MANY");
                }
                Console.Write("HOW MANY SHALL I REVERSE? ");
                input = ReadIntegerInput();
            }

            return input;
        }

        private static int ReadIntegerInput()
        {
            var input = Console.ReadLine();
            int.TryParse(input, out var index);
            return index;
        }

        private static string PrintArrayContents(int[] arr)
        {
            var sb = new StringBuilder();

            foreach (int i in arr)
            {
                sb.Append(" " + i + " ");
            }

            return sb.ToString();
        }

        private static void PrintTitle()
        {
            Console.WriteLine("REVERSE");
            Console.WriteLine("CREATIVE COMPUTING  MORRISTON, NEW JERSEY");
        }

        private static void DisplayRules()
        {
            Console.WriteLine("RULES");
        }
    }
}
