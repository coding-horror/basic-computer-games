using System;

namespace Game
{
    /// <summary>
    /// Contains functions for interacting with the user.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// Gets the player's initial armed forces distribution.
        /// </summary>
        /// <param name="computerForces">
        /// The computer's initial armed forces.
        /// </param>
        public static ArmedForces GetInitialForces(ArmedForces computerForces)
        {
            var playerForces = default(ArmedForces);

            // BUG: This loop allows the player to assign negative values to
            //  some branches, leading to strange results.
            do
            {
                View.ShowDistributeForces();

                View.PromptArmySize(computerForces.Army);
                var army = InputInteger();

                View.PromptNavySize(computerForces.Navy);
                var navy = InputInteger();

                View.PromptAirForceSize(computerForces.AirForce);
                var airForce = InputInteger();

                playerForces = new ArmedForces
                {
                    Army     = army,
                    Navy     = navy,
                    AirForce = airForce
                };
            }
            while (playerForces.TotalTroops > computerForces.TotalTroops);

            return playerForces;
        }

        /// <summary>
        /// Gets the military branch for the user's next attack.
        /// </summary>
        public static MilitaryBranch GetAttackBranch(WarState state, bool isFirstTurn)
        {
            if (isFirstTurn)
                View.PromptFirstAttackBranch();
            else
                View.PromptNextAttackBranch(state.ComputerForces, state.PlayerForces);

            // If the user entered an invalid branch number in the original
            // game, the code fell through to the army case.  We'll preserve
            // that behaviour here.
            return Console.ReadLine() switch
            {
                "2" => MilitaryBranch.Navy,
                "3" => MilitaryBranch.AirForce,
                _   => MilitaryBranch.Army
            };
        }

        /// <summary>
        /// Gets a valid attack size from the player for the given branch
        /// of the armed forces.
        /// </summary>
        /// <param name="troopsAvailable">
        /// The number of troops available.
        /// </param>
        public static int GetAttackSize(int troopsAvailable)
        {
            var attackSize = 0;

            do
            {
                View.PromptAttackSize();
                attackSize = InputInteger();
            }
            while (attackSize < 0 || attackSize > troopsAvailable);

            return attackSize;
        }

        /// <summary>
        /// Gets an integer value from the user.
        /// </summary>
        public static int InputInteger()
        {
            var value = default(int);

            while (!Int32.TryParse(Console.ReadLine(), out value))
                View.PromptValidInteger();

            return value;
        }
    }
}
