using System;

namespace hurkle
{
    internal class ConsoleHurkleView : IHurkleView
    {
        public GamePoint GetGuess(GuessViewModel guessViewModel)
        {
            Console.WriteLine($"GUESS #{guessViewModel.CurrentGuessNumber}");
            var inputLine = Console.ReadLine();
            var seperateStrings = inputLine.Split(',', 2, StringSplitOptions.TrimEntries);
            var guessPoint = new GamePoint{
                X = int.Parse(seperateStrings[0]),
                Y = int.Parse(seperateStrings[1])
            };

            return guessPoint;
        }

        public void ShowDirection(FailedGuessViewModel failedGuessViewModel)
        {
            Console.Write("GO ");
            switch(failedGuessViewModel.Direction)
            {
                case CardinalDirection.East:
                    Console.WriteLine("EAST");
                    break;
                case CardinalDirection.North:
                    Console.WriteLine("NORTH");
                    break;
                case CardinalDirection.South:
                    Console.WriteLine("SOUTH");
                    break;
                case CardinalDirection.West:
                    Console.WriteLine("WEST");
                    break;
                case CardinalDirection.NorthEast:
                    Console.WriteLine("NORTHEAST");
                    break;
                case CardinalDirection.NorthWest:
                    Console.WriteLine("NORTHWEST");
                    break;
                case CardinalDirection.SouthEast:
                    Console.WriteLine("SOUTHEAST");
                    break;
                case CardinalDirection.SouthWest:
                    Console.WriteLine("SOUTHWEST");
                    break;
            }

            Console.WriteLine();
        }

        public void ShowLoss(LossViewModel lossViewModel)
        {
            Console.WriteLine();
            Console.WriteLine($"SORRY, THAT'S {lossViewModel.MaxGuesses} GUESSES");
            Console.WriteLine($"THE HURKLE IS AT {lossViewModel.HurkleLocation.X},{lossViewModel.HurkleLocation.Y}");
        }

        public void ShowVictory(VictoryViewModel victoryViewModel)
        {
            Console.WriteLine();
            Console.WriteLine($"YOU FOUND HIM IN {victoryViewModel.CurrentGuessNumber} GUESSES!");
        }
    }
}