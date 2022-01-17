using System;

namespace Hexapawn
{
    // Hexapawn:  Interpretation of hexapawn game as presented in
    // Martin Gardner's "The Unexpected Hanging and Other Mathematic
    // al Diversions", Chapter Eight:  A Matchbox Game-Learning Machine.
    // Original version for H-P timeshare system by R.A. Kaapke 5/5/76
    // Instructions by Jeff Dalton
    // Conversion to MITS BASIC by Steve North
    // Conversion to C# by Andrew Cooper
    class Program
    {
        static void Main()
        {
            DisplayTitle();

            if (Input.GetYesNo("Instructions") == 'Y')
            {
                DisplayInstructions();
            }

            var games = new GameSeries();

            games.Play();
        }

        private static void DisplayTitle()
        {
            Console.WriteLine("                                Hexapawn");
            Console.WriteLine("               Creative Computing  Morristown, New Jersey");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void DisplayInstructions()
        {
            Console.WriteLine();
            Console.WriteLine("This program plays the game of Hexapawn.");
            Console.WriteLine("Hexapawn is played with Chess pawns on a 3 by 3 board.");
            Console.WriteLine("The pawns are move as in Chess - one space forward to");
            Console.WriteLine("an empty space, or one space forward and diagonally to");
            Console.WriteLine("capture an opposing man.  On the board, your pawns");
            Console.WriteLine("are 'O', the computer's pawns are 'X', and empty");
            Console.WriteLine("squares are '.'.  To enter a move, type the number of");
            Console.WriteLine("the square you are moving from, followed by the number");
            Console.WriteLine("of the square you will move to.  The numbers must be");
            Console.WriteLine("separated by a comma.");
            Console.WriteLine();
            Console.WriteLine("The computer starts a series of games knowing only when");
            Console.WriteLine("the game is won (a draw is impossible) and how to move.");
            Console.WriteLine("It has no strategy at first and just moves randomly.");
            Console.WriteLine("However, it learns from each game.  Thus winning becomes");
            Console.WriteLine("more and more difficult.  Also, to help offset your");
            Console.WriteLine("initial advantage, you will not be told how to win the");
            Console.WriteLine("game but must learn this by playing.");
            Console.WriteLine();
            Console.WriteLine("The numbering of the board is as follows:");
            Console.WriteLine("          123");
            Console.WriteLine("          456");
            Console.WriteLine("          789");
            Console.WriteLine();
            Console.WriteLine("For example, to move your rightmost pawn forward,");
            Console.WriteLine("you would type 9,6 in response to the question");
            Console.WriteLine("'Your move ?'.  Since I'm a good sport, you'll always");
            Console.WriteLine("go first.");
            Console.WriteLine();
        }
    }
}
