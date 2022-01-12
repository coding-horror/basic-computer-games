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
            Console.WriteLine();
            if (string.Equals(needRulesInput, "YES", StringComparison.OrdinalIgnoreCase))
            {
                DisplayRules();
            }

            var tryAgain = string.Empty;
            while (!string.Equals(tryAgain, "NO", StringComparison.OrdinalIgnoreCase))
            {
                var reverser = new Reverser(arrayLength);

                Console.WriteLine("HERE WE GO ... THE LIST IS:\n");
                PrintList(reverser.GetArrayString());
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
                    PrintList(reverser.GetArrayString());

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

        private static void PrintList(string list)
        {
            Console.WriteLine();
            Console.WriteLine(list);
            Console.WriteLine();
        }

        private static void PrintTitle()
        {
            Console.WriteLine("REVERSE");
            Console.WriteLine("CREATIVE COMPUTING  MORRISTON, NEW JERSEY");
        }

        private static void DisplayRules()
        {
            Console.WriteLine("THIS IS THE GAME OF 'REVERSE'. TO WIN, ALL YOU HAVE");
            Console.WriteLine("TO DO IS ARRANGE A LIST OF NUMBERS (1 THOUGH 9 )");
            Console.WriteLine("IN NUMBERICAL ORDER FROM LEFT TO RIGHT. TO MOVE, YOU");
            Console.WriteLine("TELL ME HOW MANY NUMBERS (COUNTING FROM THE LEFT) TO");
            Console.WriteLine("REVERSE. FOR EXAMPLE, IF THE CURRENT LIST IS:");
            Console.WriteLine();
            Console.WriteLine(" 2  3  4  5  1  6  7  8  9");
            Console.WriteLine();
            Console.WriteLine("AND YOU REVERSE 4, THE RESULT WILL BE:");
            Console.WriteLine();
            Console.WriteLine(" 5  4  3  2  1  6  7  8  9");
            Console.WriteLine();
            Console.WriteLine("NOW IF YOU REVERSE 5, YOU WIN!");
            Console.WriteLine();
            Console.WriteLine(" 1  2  3  4  5  6  7  8  9");
            Console.WriteLine();
            Console.WriteLine("NO DOUBT YOU WILL LIKE THIS GAME, BUT ");
            Console.WriteLine("IF YOU WANT TO QUIT, REVERSE 0 (ZERO)");
            Console.WriteLine();
        }
    }
}
