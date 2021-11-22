using System;

namespace DepthCharge
{
    /// <summary>
    /// Contains functions for reading input from the user.
    /// </summary>
    static class Controller
    {
        /// <summary>
        /// Retrives a dimension for the play area from the user.
        /// </summary>
        /// <remarks>
        /// Note that the original BASIC version would allow dimension values
        /// of 0 or less.  We're doing a little extra validation here in order
        /// to avoid strange behaviour.
        /// </remarks>
        public static int InputDimension()
        {
            View.PromptDimension();

            while (true)
            {
                if (!Int32.TryParse(Console.ReadLine(), out var dimension))
                    View.ShowInvalidNumber();
                else
                if (dimension < 1)
                    View.ShowInvalidDimension();
                else
                    return dimension;
            }
        }

        /// <summary>
        /// Retrieves a set of coordinates from the user.
        /// </summary>
        /// <param name="trailNumber">
        /// The current trail number.
        /// </param>
        public static (int x, int y, int depth) InputCoordinates(int trailNumber)
        {
            View.PromptGuess(trailNumber);

            while (true)
            {
                var coordinates = Console.ReadLine().Split(',');

                if (coordinates.Length < 3)
                    View.ShowTooFewCoordinates();
                else
                if (coordinates.Length > 3)
                    View.ShowTooManyCoordinates();
                else
                if (!Int32.TryParse(coordinates[0], out var x) ||
                    !Int32.TryParse(coordinates[1], out var y) ||
                    !Int32.TryParse(coordinates[2], out var depth))
                    View.ShowInvalidNumber();
                else
                    return (x, y, depth);
            }
        }

        /// <summary>
        /// Retrieves the user's intention to play again (or not).
        /// </summary>
        public static bool InputPlayAgain()
        {
            View.PromptPlayAgain();

            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "Y":
                        return true;
                    case "N":
                        return false;
                    default:
                        View.ShowInvalidYesOrNo();
                        break;
                }
            }
        }
    }
}
