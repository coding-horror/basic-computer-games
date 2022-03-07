using System;
using System.Collections.Generic;

namespace Bombardment
{
    // <summary>
    // Game of Bombardment
    // Based on the Basic game of Bombardment here
    // https://github.com/coding-horror/basic-computer-games/blob/main/11%20Bombardment/bombardment.bas
    // Note:  The idea was to create a version of the 1970's Basic game in C#, without introducing
    // new features - no additional text, error checking, etc has been added.
    // </summary>
    internal class Bombardment
    {
        private static int MAX_GRID_SIZE = 25;
        private static int MAX_PLATOONS = 4;
        private static Random random = new Random();
        private List<int> computerPositions = new List<int>();
        private List<int> playerPositions = new List<int>();
        private List<int> computerGuesses = new List<int>();

        private void PrintStartingMessage()
        {
            Console.WriteLine("{0}BOMBARDMENT", new string(' ', 33));
            Console.WriteLine("{0}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", new string(' ', 15));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("YOU ARE ON A BATTLEFIELD WITH 4 PLATOONS AND YOU");
            Console.WriteLine("HAVE 25 OUTPOSTS AVAILABLE WHERE THEY MAY BE PLACED.");
            Console.WriteLine("YOU CAN ONLY PLACE ONE PLATOON AT ANY ONE OUTPOST.");
            Console.WriteLine("THE COMPUTER DOES THE SAME WITH ITS FOUR PLATOONS.");
            Console.WriteLine();
            Console.WriteLine("THE OBJECT OF THE GAME IS TO FIRE MISSLES AT THE");
            Console.WriteLine("OUTPOSTS OF THE COMPUTER.  IT WILL DO THE SAME TO YOU.");
            Console.WriteLine("THE ONE WHO DESTROYS ALL FOUR OF THE ENEMY'S PLATOONS");
            Console.WriteLine("FIRST IS THE WINNER.");
            Console.WriteLine();
            Console.WriteLine("GOOD LUCK... AND TELL US WHERE YOU WANT THE BODIES SENT!");
            Console.WriteLine();
            Console.WriteLine("TEAR OFF MATRIX AND USE IT TO CHECK OFF THE NUMBERS.");

            // As an alternative to repeating the call to WriteLine(),
            // we can print the new line character five times.
            Console.Write(new string('\n', 5));

            // Print a sample board (presumably the game was originally designed to be
            // physically printed on paper while played).
            for (var i = 1; i <= 25; i += 5)
            {
                // The token replacement can be padded by using the format {tokenPosition, padding}
                // Negative values for the padding cause the output to be left-aligned.
                Console.WriteLine("{0,-3}{1,-3}{2,-3}{3,-3}{4,-3}", i, i + 1, i + 2, i + 3, i + 4);
            }

            Console.WriteLine("\n");
        }

        // Generate 5 random positions for the computer's platoons.
        private void PlaceComputerPlatoons()
        {
            do
            {
                var nextPosition = random.Next(1, MAX_GRID_SIZE);
                if (!computerPositions.Contains(nextPosition))
                {
                    computerPositions.Add(nextPosition);
                }

            } while (computerPositions.Count < MAX_PLATOONS);
        }

        private void StoreHumanPositions()
        {
            Console.WriteLine("WHAT ARE YOUR FOUR POSITIONS");

            // The original game assumed that the input would be five comma-separated values, all on one line.
            // For example: 12,22,1,4,17
            var input = Console.ReadLine();
            var playerPositionsAsStrings = input.Split(",");
            foreach (var playerPosition in playerPositionsAsStrings) {
                playerPositions.Add(int.Parse(playerPosition));
            }
        }

        private void HumanTurn()
        {
            Console.WriteLine("WHERE DO YOU WISH TO FIRE YOUR MISSLE");
            var input = Console.ReadLine();
            var humanGuess = int.Parse(input);

            if(computerPositions.Contains(humanGuess))
            {
                Console.WriteLine("YOU GOT ONE OF MY OUTPOSTS!");
                computerPositions.Remove(humanGuess);

                switch(computerPositions.Count)
                {
                    case 3:
                        Console.WriteLine("ONE DOWN, THREE TO GO.");
                        break;
                    case 2:
                        Console.WriteLine("TWO DOWN, TWO TO GO.");
                        break;
                    case 1:
                        Console.WriteLine("THREE DOWN, ONE TO GO.");
                        break;
                    case 0:
                        Console.WriteLine("YOU GOT ME, I'M GOING FAST.");
                        Console.WriteLine("BUT I'LL GET YOU WHEN MY TRANSISTO&S RECUP%RA*E!");
                        break;
                }
            }
            else
            {
                Console.WriteLine("HA, HA YOU MISSED. MY TURN NOW:");
            }
        }

        private int GenerateComputerGuess()
        {
            int computerGuess;
            do
            {
                computerGuess = random.Next(1, 25);
            }
            while(computerGuesses.Contains(computerGuess));
            computerGuesses.Add(computerGuess);

            return computerGuess;
        }

        private void ComputerTurn()
        {
            var computerGuess = GenerateComputerGuess();

            if (playerPositions.Contains(computerGuess))
            {
                Console.WriteLine("I GOT YOU. IT WON'T BE LONG NOW. POST {0} WAS HIT.", computerGuess);
                playerPositions.Remove(computerGuess);

                switch(playerPositions.Count)
                {
                    case 3:
                        Console.WriteLine("YOU HAVE ONLY THREE OUTPOSTS LEFT.");
                        break;
                    case 2:
                        Console.WriteLine("YOU HAVE ONLY TWO OUTPOSTS LEFT.");
                        break;
                    case 1:
                        Console.WriteLine("YOU HAVE ONLY ONE OUTPOST LEFT.");
                        break;
                    case 0:
                        Console.WriteLine("YOU'RE DEAD. YOUR LAST OUTPOST WAS AT {0}. HA, HA, HA.", computerGuess);
                        Console.WriteLine("BETTER LUCK NEXT TIME.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("I MISSED YOU, YOU DIRTY RAT. I PICKED {0}. YOUR TURN:", computerGuess);
            }
        }

        public void Play()
        {
            PrintStartingMessage();
            PlaceComputerPlatoons();
            StoreHumanPositions();

            while (playerPositions.Count > 0 && computerPositions.Count > 0)
            {
                HumanTurn();

                if (computerPositions.Count > 0)
                {
                    ComputerTurn();
                }
            }
        }
    }
}
