using System;

namespace FurTrader
{
    public class Program
    {
        /// <summary>
        /// This function will be called automatically when the application begins
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Create an instance of our main Game class
            var game = new Game();

            // Call its GameLoop function. This will play the game endlessly in a loop until the player chooses to quit.
            game.GameLoop();
        }
    }
}
