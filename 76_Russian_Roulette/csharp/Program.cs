using System;

namespace RussianRoulette
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PrintTitle();

            var includeRevolver = true;
            while (true)
            {
                PrintInstructions(includeRevolver);
                switch (PlayGame())
                {
                    case GameResult.Win:
                        includeRevolver = true;
                        break;
                    case GameResult.Chicken:
                    case GameResult.Dead:
                        includeRevolver = false;
                        break;
                }
            }
        }

        private static void PrintTitle()
        {
            Console.WriteLine("           Russian Roulette");
            Console.WriteLine("Creative Computing  Morristown, New Jersey");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("This is a game of >>>>>>>>>>Russian Roulette.");
        }

        private static void PrintInstructions(bool includeRevolver)
        {
            Console.WriteLine();
            if (includeRevolver)
            {
                Console.WriteLine("Here is a revolver.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("...Next Victim...");
            }
            Console.WriteLine("Type '1' to spin chamber and pull trigger.");
            Console.WriteLine("Type '2' to give up.");
        }

        private static GameResult PlayGame()
        {
            var rnd = new Random();
            var round = 0;
            while (true)
            {
                round++;
                Console.Write("Go: ");
                var input = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (input != '2')
                {
                    // Random.Next will retun a value that is the same or greater than the minimum and
                    // less than the maximum.
                    // A revolver has 6 rounds.
                    if (rnd.Next(1, 7) == 6)
                    {
                        Console.WriteLine("     Bang!!!!!   You're dead!");
                        Console.WriteLine("Condolences will be sent to your relatives.");
                        return GameResult.Dead;
                    }
                    else
                    {
                        if (round > 10)
                        {
                            Console.WriteLine("You win!!!!!");
                            Console.WriteLine("Let someone else blow their brains out.");
                            return GameResult.Win;
                        }
                        else
                        {
                            Console.WriteLine("- CLICK -");
                            Console.WriteLine();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("     CHICKEN!!!!!");
                    return GameResult.Chicken;
                }
            }
        }

        private enum GameResult
        {
            Win,
            Chicken,
            Dead
        }
    }
}
