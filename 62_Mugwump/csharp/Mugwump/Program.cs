using System;
using System.Reflection;

namespace Mugwump
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayIntro();

            var random = new Random();

            while (true)
            {
                Game.Play(random);

                Console.WriteLine();
                Console.WriteLine("That was fun! Let's play again.......");
                Console.WriteLine("Four more mugwumps are now in hiding.");
            }
        }

        private static void DisplayIntro()
        {
            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Mugwump.Strings.Intro.txt");
            using var stdout = Console.OpenStandardOutput();

            stream.CopyTo(stdout);
        }
    }
}
