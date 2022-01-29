using System;
using System.Reflection;

namespace Target
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayTitleAndInstructions();

            var firingRange = new FiringRange();

            while (true)
            {
                Game.Play(firingRange);

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Next target...");
                Console.WriteLine();

                firingRange.NextTarget();
            }
        }

        private static void DisplayTitleAndInstructions()
        {
            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Target.Strings.TitleAndInstructions.txt");
            using var stdout = Console.OpenStandardOutput();

            stream.CopyTo(stdout);
        }
    }
}
