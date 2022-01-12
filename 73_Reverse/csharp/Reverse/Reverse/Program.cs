using System;

namespace Reverse
{
    class Program
    {
        private static int arrayLength = 9;
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
                var reverser = new Reverser(arrayLength);

                Console.WriteLine(reverser.GetArrayString());

                var arrayIsNotInAscendingOrder = true;
                var numberOfMoves = 0;
                while (arrayIsNotInAscendingOrder)
                {
                    int index = ReadNextInput();

                    if (index == 0)
                    {
                        break;
                    }

                    reverser.Reverse(index);
                    Console.WriteLine(reverser.GetArrayString());

                    if (reverser.IsArrayInAscendingOrder())
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
                    Console.WriteLine($"OOPS! TOO MANY! I CAN REVERSE AT MOST {arrayLength}");
                }

                if (input < 0)
                {
                    Console.WriteLine($"OOPS! TOO FEW! I CAN REVERSE BETWEEN 1 AND {arrayLength}");
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
