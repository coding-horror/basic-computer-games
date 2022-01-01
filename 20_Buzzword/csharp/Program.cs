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

        static void Instructions()
        {
            Console.WriteLine("This program prints highly acceptable phrases in\n"
            + "'Educator-speak'that you can work into reports\n"
            + "and speeches. Whenever a question mark is printed,\n"
            + "type a 'Y' for another phrase or 'N' to quit.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Here's the first phrase:");
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

        static Random rnd = new Random(1);

        static string GeneratePhrase()
        {
            return $"{Phrases[(int)(13 * rnd.NextDouble() + 1) % Phrases.Length]} "
                + $"{Phrases[(int)(13 * rnd.NextDouble() + 14) % Phrases.Length]} "
                + $"{Phrases[(int)(13 * rnd.NextDouble() + 27) % Phrases.Length]}.";
        }

        static void Main(string[] args)
        {
            Header();
            Instructions();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine(GeneratePhrase());
                var answer = Console.ReadKey();

                if (answer.Key == ConsoleKey.N)
                    break;
            }
        }
    }
}
