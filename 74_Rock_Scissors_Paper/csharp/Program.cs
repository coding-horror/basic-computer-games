using System;

namespace RockScissorsPaper
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("GAME OF ROCK, SCISSORS, PAPER");
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var numberOfGames = GetNumberOfGames();

            var game = new Game();
            for (var gameNumber = 1; gameNumber <= numberOfGames; gameNumber++) {
                Console.WriteLine();
                Console.WriteLine("Game number {0}", gameNumber);

                game.PlayGame();
            }

            game.WriteFinalScore();

            Console.WriteLine();
            Console.WriteLine("Thanks for playing!!");
        }

        static int GetNumberOfGames()
        {
            while (true) {
                Console.WriteLine("How many games");
                if (int.TryParse(Console.ReadLine(), out var number))
                {
                    if (number < 11 && number > 0)
                        return number;
                    Console.WriteLine("Sorry, but we aren't allowed to play that many.");
                }
                else
                {
                    Console.WriteLine("Sorry, I didn't understand.");
                }
            }
        }
    }
}
