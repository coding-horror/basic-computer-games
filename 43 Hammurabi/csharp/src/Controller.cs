using System;

namespace Hammurabi
{
    /// <summary>
    /// Provides methods for reading input from the user.
    /// </summary>
    public static class Controller
    {
        /// <summary>
        /// Continuously prompts the user to enter a number until he or she
        /// enters a valid number and updates the game state.
        /// </summary>
        /// <param name="state">
        /// The current game state.
        /// </param>
        /// <param name="prompt">
        /// Action that will display the prompt to the user.
        /// </param>
        /// <param name="rule">
        /// The rule to invoke once input is retrieved.
        /// </param>
        /// <returns>
        /// The updated game state.
        /// </returns>
        public static GameState UpdateGameState(
            GameState state,
            Action prompt,
            Func<GameState, int, (GameState newState, ActionResult result)> rule)
        {
            while (true)
            {
                prompt();

                if (!Int32.TryParse(Console.ReadLine(), out var amount))
                {
                    View.ShowInvalidNumber();
                    continue;
                }

                var (newState, result) = rule(state, amount);

                switch (result)
                {
                    case ActionResult.InsufficientLand:
                        View.ShowInsufficientLand(state);
                        break;
                    case ActionResult.InsufficientPopulation:
                        View.ShowInsufficientPopulation(state);
                        break;
                    case ActionResult.InsufficientStores:
                        View.ShowInsufficientStores(state);
                        break;
                    case ActionResult.Offense:
                        // Not sure why we have to blow up the game here...
                        // Maybe this made sense in the 70's.
                        throw new GreatOffence();
                    default:
                        return newState;
                }
            }
        }
    }
}
