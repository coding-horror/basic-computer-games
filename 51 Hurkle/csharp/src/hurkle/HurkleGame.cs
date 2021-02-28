using System;

namespace hurkle
{
    public partial class HurkleGame
    {
        private readonly Random _random = new Random();
        private readonly int guesses;
        private readonly int gridSize;

        public HurkleGame(int guesses, int gridSize)
        {
            this.guesses = guesses;
            this.gridSize = gridSize;
        }

        public void PlayGame()
        {
            // BASIC program was generating a float between 0 and 1
            // then multiplying by the size of the grid to to a number
            // between 1 and 10. C# allows you to do that directly.
            var hurklePoint = new GamePoint{
                X = _random.Next(0, gridSize),
                Y = _random.Next(0, gridSize)
            };
            
            for(var K=1;K<=guesses;K++)
            {
                var guessPoint = GetGuess(new GuessViewModel{CurrentGuessNumber = K});

                var direction = guessPoint.GetDirectionTo(hurklePoint);
                switch(direction)
                {
                    case CardinalDirection.None:
                        ShowVictory(new VictoryViewModel{CurrentGuessNumber = K});
                        return;
                    default:
                        ShowDirection(new FailedGuessViewModel{Direction = direction});
                        continue;
                }
            }
            
            ShowLoss(new LossViewModel{MaxGuesses = guesses, HurkleLocation = hurklePoint } );
        }

        private void ShowLoss(LossViewModel lossViewModel)
        {
            Console.WriteLine();
            Console.WriteLine($"SORRY, THAT'S {lossViewModel.MaxGuesses} GUESSES");
            Console.WriteLine($"THE HURKLE IS AT {lossViewModel.HurkleLocation.X},{lossViewModel.HurkleLocation.Y}");
        }

        private void ShowDirection(FailedGuessViewModel failedGuessViewModel)
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

        private void ShowVictory(VictoryViewModel victoryViewModel)
        {
            Console.WriteLine();
            Console.WriteLine($"YOU FOUND HIM IN {victoryViewModel.CurrentGuessNumber} GUESSES!");
        }

        private class GuessViewModel
        {
            public int CurrentGuessNumber {get;init;}
        }

        private static GamePoint GetGuess(GuessViewModel model)
        {
            Console.WriteLine($"GUESS #{model.CurrentGuessNumber}");
            var inputLine = Console.ReadLine();
            var seperateStrings = inputLine.Split(',', 2, StringSplitOptions.TrimEntries);
            var guessPoint = new GamePoint{
                X = int.Parse(seperateStrings[0]),
                Y = int.Parse(seperateStrings[1])
            };

            return guessPoint;
        }

        private static void PrintInfo(GamePoint guess, GamePoint target)
        {
            
        }
    }
}