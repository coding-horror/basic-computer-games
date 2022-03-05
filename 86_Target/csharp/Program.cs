using System;
using System.Reflection;
using Games.Common.IO;
using Games.Common.Randomness;

namespace Target
{
    class Program
    {
        static void Main()
        {
            var io = new ConsoleIO();
            var game = new Game(io, new FiringRange(new RandomNumberGenerator()));

            Play(game, io, () => true);
        }

        public static void Play(Game game, TextIO io, Func<bool> playAgain)
        {
            DisplayTitleAndInstructions(io);

            while (playAgain())
            {
                game.Play();

                io.WriteLine();
                io.WriteLine();
                io.WriteLine();
                io.WriteLine();
                io.WriteLine();
                io.WriteLine("Next target...");
                io.WriteLine();
            }
        }

        private static void DisplayTitleAndInstructions(TextIO io)
        {
            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Target.Strings.TitleAndInstructions.txt");
            io.Write(stream);
        }
    }
}
