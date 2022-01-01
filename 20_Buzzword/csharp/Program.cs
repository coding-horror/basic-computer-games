using System;

namespace Buzzword
{
    class Program
    {
        static void Header()
        {
            Console.WriteLine("Buzzword generator".PadLeft(26));
            Console.WriteLine("Creating Computing Morristown, New Jersey".PadLeft(15));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        static string keys = "type a 'Y' for another phrase or 'N' to quit";

        static void Instructions()
        {
            Console.WriteLine("This program prints highly acceptable phrases in\n"
            + "'Educator-speak'that you can work into reports\n"
            + "and speeches. Whenever a question mark is printed,\n"
            + $"{keys}.");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Here's the first phrase:");
        }

        static string[] Phrases = new[]
            { "ability", "basal", "behavioral", "child-centered",
            "differentiated", "discovery", "flexible", "heterogenous",
            "homogenous", "manipulative", "modular", "tavistock",
            "individualized", "learning", "evaluative", "objective",
            "cognitive", "enrichment", "scheduling", "humanistic",
            "integrated", "non-graded", "training", "vertical age",
            "motivational", "creative", "grouping", "modification",
            "accountability", "process", "core curriculum", "algorithm",
            "performance", "reinforcement", "open classroom", "resource",
            "structure", "facility", "environment" };

        static string Capitalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return input.Substring(0, 1).ToUpper() + input[1..];
        }

        static Random rnd = new Random(1);

        static string GeneratePhrase()
        {
            return $"{Capitalize(Phrases[(int)(13 * rnd.NextDouble() + 1) % Phrases.Length])} "
                + $"{Phrases[(int)(13 * rnd.NextDouble() + 14) % Phrases.Length]} "
                + $"{Phrases[(int)(13 * rnd.NextDouble() + 27) % Phrases.Length]}";
        }

        static bool Decision()
        {
            while (true)
            {
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
