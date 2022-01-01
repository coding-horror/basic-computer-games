using System;

namespace Buzzword
{
    class Program
    {
        /// <summary>
        /// Displays header.
        /// </summary>
        static void Header()
        {
            Console.WriteLine("Buzzword generator".PadLeft(26));
            Console.WriteLine("Creating Computing Morristown, New Jersey".PadLeft(15));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        // Information for the user about possible key input.
        static string keys = "type a 'Y' for another phrase or 'N' to quit";

        /// <summary>
        /// Displays instructions.
        /// </summary>
        static void Instructions()
        {
            Console.WriteLine("This program prints highly acceptable phrases in\n"
            + "'educator-speak' that you can work into reports\n"
            + "and speeches. Whenever a question mark is printed,\n"
            + $"{keys}.");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Here's the first phrase:");
        }

        static string[] Words = new[]
            { "ability", "basal", "behavioral", "child-centered",
            "differentiated", "discovery", "flexible", "heterogenous",
            "homogeneous", "manipulative", "modular", "tavistock",
            "individualized", "learning", "evaluative", "objective",
            "cognitive", "enrichment", "scheduling", "humanistic",
            "integrated", "non-graded", "training", "vertical age",
            "motivational", "creative", "grouping", "modification",
            "accountability", "process", "core curriculum", "algorithm",
            "performance", "reinforcement", "open classroom", "resource",
            "structure", "facility", "environment" };

        /// <summary>
        /// Capitalizes first letter of given string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string</returns>
        static string Capitalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return char.ToUpper(input[0]) + input[1..];
        }

        // Seed has been calculated to get the same effect as in original,
        // at least in first phrase
        static readonly Random rnd = new Random(1486);

        /// <summary>
        /// Generates random phrase from words available in Words array.
        /// </summary>
        /// <returns>String representing random phrase where first letter is capitalized.</returns>
        static string GeneratePhrase()
        {
            // Indexing from 0, so had to decrease generated numbers
            return $"{Capitalize(Words[rnd.Next(13)])} "
                + $"{Words[rnd.Next(13, 26)]} "
                + $"{Words[rnd.Next(26, 39)]}";
        }

        /// <summary>
        /// Handles user input. On wrong input it displays information about 
        /// valid keys in infinite loop.
        /// </summary>
        /// <returns>True if user pressed 'Y', false if 'N'.</returns>
        static bool Decision()
        {
            while (true)
            {
                Console.Write("?");
                var answer = Console.ReadKey();
                if (answer.Key == ConsoleKey.Y)
                    return true;
                else if (answer.Key == ConsoleKey.N)
                    return false;
                else
                    Console.WriteLine($"\n{keys}");
            }
        }

        static void Main(string[] args)
        {
            Header();
            Instructions();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(GeneratePhrase());
                Console.WriteLine();

                if (!Decision())
                    break;
            }

            Console.WriteLine("\nCome back when you need help with another report!");
        }
    }
}
