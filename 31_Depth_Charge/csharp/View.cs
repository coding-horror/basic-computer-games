using System;

namespace DepthCharge
{
    /// <summary>
    /// Contains methods for displaying information to the user.
    /// </summary>
    static class View
    {
        public static void ShowBanner()
        {
            Console.WriteLine("                             DEPTH CHARGE");
            Console.WriteLine("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void ShowInstructions(int maximumGuesses)
        {
            Console.WriteLine("YOU ARE THE CAPTAIN OF THE DESTROYER USS COMPUTER");
            Console.WriteLine("AN ENEMY SUB HAS BEEN CAUSING YOU TROUBLE.  YOUR");
            Console.WriteLine($"MISSION IS TO DESTROY IT.  YOU HAVE {maximumGuesses} SHOTS.");
            Console.WriteLine("SPECIFY DEPTH CHARGE EXPLOSION POINT WITH A");
            Console.WriteLine("TRIO OF NUMBERS -- THE FIRST TWO ARE THE");
            Console.WriteLine("SURFACE COORDINATES; THE THIRD IS THE DEPTH.");
            Console.WriteLine();
        }

        public static void ShowStartGame()
        {
            Console.WriteLine("GOOD LUCK !");
            Console.WriteLine();
        }

        public static void ShowGuessPlacement((int x, int y, int depth) actual, (int x, int y, int depth) guess)
        {
            Console.Write("SONAR REPORTS SHOT WAS ");
            if (guess.y > actual.y)
                Console.Write("NORTH");
            if (guess.y < actual.y)
                Console.Write("SOUTH");
            if (guess.x > actual.x)
                Console.Write("EAST");
            if (guess.x < actual.x)
                Console.Write("WEST");
            if (guess.y != actual.y || guess.x != actual.y)
                Console.Write(" AND");
            if (guess.depth > actual.depth)
                Console.Write (" TOO LOW.");
            if (guess.depth < actual.depth)
                Console.Write(" TOO HIGH.");
            if (guess.depth == actual.depth)
                Console.Write(" DEPTH OK.");

            Console.WriteLine();
        }

        public static void ShowGameResult((int x, int y, int depth) submarineLocation, (int x, int y, int depth) finalGuess, int trailNumber)
        {
            Console.WriteLine();

            if (submarineLocation == finalGuess)
            {
                Console.WriteLine($"B O O M ! ! YOU FOUND IT IN {trailNumber} TRIES!");
            }
            else
            {
                Console.WriteLine("YOU HAVE BEEN TORPEDOED!  ABANDON SHIP!");
                Console.WriteLine($"THE SUBMARINE WAS AT {submarineLocation.x}, {submarineLocation.y}, {submarineLocation.depth}");
            }
        }

        public static void ShowFarewell()
        {
            Console.WriteLine ("OK.  HOPE YOU ENJOYED YOURSELF.");
        }

        public static void ShowInvalidNumber()
        {
            Console.WriteLine("PLEASE ENTER A NUMBER");
        }

        public static void ShowInvalidDimension()
        {
            Console.WriteLine("PLEASE ENTER A VALID DIMENSION");
        }

        public static void ShowTooFewCoordinates()
        {
            Console.WriteLine("TOO FEW COORDINATES");
        }

        public static void ShowTooManyCoordinates()
        {
            Console.WriteLine("TOO MANY COORDINATES");
        }

        public static void ShowInvalidYesOrNo()
        {
            Console.WriteLine("PLEASE ENTER Y OR N");
        }

        public static void PromptDimension()
        {
            Console.Write("DIMENSION OF SEARCH AREA? ");
        }

        public static void PromptGuess(int trailNumber)
        {
            Console.WriteLine();
            Console.Write($"TRIAL #{trailNumber}? ");
        }

        public static void PromptPlayAgain()
        {
            Console.WriteLine();
            Console.Write("ANOTHER GAME (Y OR N)? ");
        }
    }
}
