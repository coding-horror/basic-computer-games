using System;
using System.Threading;

namespace AceyDucey
{
    /// <summary>
    /// The application's entry point
    /// </summary>
    class Program
    {

        /// <summary>
        /// This function will be called automatically when the application begins
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Create an instance of our main Game class
            Game game = new Game();

            // Call its GameLoop function. This will play the game endlessly in a loop until the player chooses to quit.
            game.GameLoop();
        }


    }
}
