using System;

namespace hurkle
{
    public class HurkleGame
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
            
            /*
            310 FOR K=1 TO N
            320 PRINT "GUESS #";K;
            330 INPUT X,Y
            340 IF ABS(X-A)+ABS(Y-B)=0 THEN 500
            350 REM PRINT INFO
            360 GOSUB 610
            370 PRINT
            380 NEXT K
            */
            for(var K=1;K<=guesses;K++)
            {
                var guessPoint = GetGuess(new GuessViewModel{CurrentGuessNumber = K});
                
                if(guessPoint.GetDirectionTo(hurklePoint) == CardinalDirection.None)
                {
                    /*
                    500 REM
                    510 PRINT
                    520 PRINT "YOU FOUND HIM IN";K;GUESSES!"
                    540 GOTO 440
                    */
                    Console.WriteLine();
                    Console.WriteLine($"YOU FOUND HIM IN {K} GUESSES!");
                    return;
                }

                PrintInfo(guessPoint,hurklePoint);
            }
            
            /*
            410 PRINT
            420 PRINT "SORRY, THAT'S;N;"GUESSES."
            430 PRINT "THE HURKLE IS AT ";A;",";B
            */
            Console.WriteLine();
            Console.WriteLine($"SORRY, THAT'S {guesses} GUESSES");
            Console.WriteLine($"THE HURKLE IS AT {hurklePoint.X},{hurklePoint.Y}");
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
            Console.Write("GO ");
            switch(guess.GetDirectionTo(target))
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

        private enum CardinalDirection
        {
            None,
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West,
            NorthWest
        }

        private class GamePoint
        {
            public int X {get;init;}
            public int Y {get;init;}

            public CardinalDirection GetDirectionTo(GamePoint target)
            {   
                if(X == target.X)
                {
                    if(Y > target.Y)
                    {
                        return CardinalDirection.South;
                    }
                    else if(Y < target.Y)
                    {
                        return CardinalDirection.North;
                    }
                    else
                    {
                        return CardinalDirection.None;
                    }
                }
                else if(X > target.X)
                {
                    if(Y == target.Y)
                    {
                        return CardinalDirection.West;
                    }
                    else if(Y > target.Y)
                    {
                        return CardinalDirection.SouthWest;
                    }
                    else
                    {
                        return CardinalDirection.NorthWest;
                    }
                }
                else
                {
                    if(Y == target.Y)
                    {
                        return CardinalDirection.East;
                    }
                    else if(Y > target.Y)
                    {
                        return CardinalDirection.SouthEast;
                    }
                    else{
                        return CardinalDirection.NorthEast;
                    }
                }
            }
        }
    }
}