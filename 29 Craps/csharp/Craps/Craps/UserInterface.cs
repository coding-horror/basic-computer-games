using System;
using System.Diagnostics;



namespace Craps
{
    public class UserInterface
	{
        public void Intro()
        {
            Console.WriteLine("                                 CRAPS");
            Console.WriteLine("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n");
            Console.WriteLine("2,3,12 ARE LOSERS; 4,5,6,8,9,10 ARE POINTS; 7,11 ARE NATURAL WINNERS.");

            // In the original game a random number would be generated and then thrown away for as many
            // times as the number the user entered. This is presumably something to do with ensuring
            // that different random numbers will be generated each time the program is run.
            //
            // This is not necessary in C#; the random number generator uses the current time as a seed
            // so the results will always be different every time it is run.
            //
            // So that the game exactly matches the original game we ask the question but then ignore
            // the answer.
            Console.Write("PICK A NUMBER AND INPUT TO ROLL DICE ");
            GetInt();
        }

        public int PlaceBet()
        {
            Console.Write("INPUT THE AMOUNT OF YOUR WAGER. ");
            int n = GetInt();
            Console.WriteLine("I WILL NOW THROW THE DICE");

            return n;
        }

        public bool PlayAgain(int winnings)
        {
            // Goodness knows why we have to enter 5 to play
            // again but that's what the original game asked.
            Console.Write("IF YOU WANT TO PLAY AGAIN PRINT 5 IF NOT PRINT 2 ");

            bool playAgain = (GetInt() == 5);

            if (winnings < 0)
            {
                Console.WriteLine($"YOU ARE NOW UNDER ${-winnings}");
            }
            else if (winnings > 0)
            {
                Console.WriteLine($"YOU ARE NOW OVER ${winnings}");
            }
            else
            {
                Console.WriteLine($"YOU ARE NOW EVEN AT ${winnings}");
            }

            return playAgain;
        }

        public void GoodBye(int winnings)
        {
            if (winnings < 0)
            {
                Console.WriteLine("TOO BAD, YOU ARE IN THE HOLE. COME AGAIN.");
            }
            else if (winnings > 0)
            {
                Console.WriteLine("CONGRATULATIONS---YOU CAME OUT A WINNER. COME AGAIN!");
            }
            else
            {
                Console.WriteLine("CONGRATULATIONS---YOU CAME OUT EVEN, NOT BAD FOR AN AMATEUR");
            }
        }

        public void NoPoint(int diceRoll)
        {
            Console.WriteLine($"{diceRoll} - NO POINT. I WILL ROLL AGAIN ");
        }

        public void Point(int point)
        {
            Console.WriteLine($"{point} IS THE POINT. I WILL ROLL AGAIN");
        }

        public void ShowResult(Result result, int diceRoll, int bet)
        {
            switch (result)
            {
                case Result.naturalWin:
                    Console.WriteLine($"{diceRoll} - NATURAL....A WINNER!!!!");
                    Console.WriteLine($"{diceRoll} PAYS EVEN MONEY, YOU WIN {bet} DOLLARS");
                    break;

                case Result.naturalLoss:
                    Console.WriteLine($"{diceRoll} - CRAPS...YOU LOSE.");
                    Console.WriteLine($"YOU LOSE {bet} DOLLARS.");
                    break;

                case Result.snakeEyesLoss:
                    Console.WriteLine($"{diceRoll} - SNAKE EYES....YOU LOSE.");
                    Console.WriteLine($"YOU LOSE {bet} DOLLARS.");
                    break;

                case Result.pointLoss:
                    Console.WriteLine($"{diceRoll} - CRAPS. YOU LOSE.");
                    Console.WriteLine($"YOU LOSE ${bet}");
                    break;

                case Result.pointWin:
                    Console.WriteLine($"{diceRoll} - A WINNER.........CONGRATS!!!!!!!!");
                    Console.WriteLine($"AT 2 TO 1 ODDS PAYS YOU...LET ME SEE... {2 * bet} DOLLARS");
                    break;

                // Include a default so that we will be warned if the values of the enum
                // ever change and we forget to add code to handle the new value.
                default:
                    Debug.Assert(false); // We should never get here.
                    break;
            }
        }

        private int GetInt()
        {
            while (true)
            {
	            string input = Console.ReadLine();
                if (int.TryParse(input, out int n))
                {
                    return n;
                }
                else
                {
                    Console.Write("ENTER AN INTEGER ");
                }
            }
        }
    }
}


