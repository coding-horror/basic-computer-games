using System;

namespace Stars
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayTitle();

            var game = new Game(maxNumber: 100, maxGuessCount: 7);

            game.DisplayInstructions();

            while (true)
            {
                game.Play();
            }
        }

        private static void DisplayTitle()
        {
            Console.WriteLine("                                  Stars");
            Console.WriteLine("               Creative Computing  Morristown, New Jersey");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
