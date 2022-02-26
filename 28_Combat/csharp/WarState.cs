using System;

namespace Game
{
    /// <summary>
    /// Represents the current state of the war.
    /// </summary>
    public abstract class WarState
    {
        /// <summary>
        /// Gets the computer's armed forces.
        /// </summary>
        public ArmedForces ComputerForces { get; }

        /// <summary>
        /// Gets the player's armed forces.
        /// </summary>
        public ArmedForces PlayerForces { get; }

        /// <summary>
        /// Gets a flag indicating whether this state represents absolute
        /// victory for the player.
        /// </summary>
        public virtual bool IsAbsoluteVictory => false;

        /// <summary>
        /// Gets the final outcome of the war.
        /// </summary>
        /// <remarks>
        /// If the war is ongoing, this property will be null.
        /// </remarks>
        public virtual WarResult? FinalOutcome => null;

        /// <summary>
        /// Initializes a new instance of the state class.
        /// </summary>
        /// <param name="computerForces">
        /// The computer's forces.
        /// </param>
        /// <param name="playerForces">
        /// The player's forces.
        /// </param>
        public WarState(ArmedForces computerForces, ArmedForces playerForces) =>
            (ComputerForces, PlayerForces) = (computerForces, playerForces);

        /// <summary>
        /// Launches an attack.
        /// </summary>
        /// <param name="branch">
        /// The branch of the military to use for the attack.
        /// </param>
        /// <param name="attackSize">
        /// The number of men and women to use for the attack.
        /// </param>
        /// <returns>
        /// The new state of the game resulting from the attack and a message
        /// describing the result.
        /// </returns>
        public (WarState nextState, string message) LaunchAttack(MilitaryBranch branch, int attackSize) =>
            branch switch
            {
                MilitaryBranch.Army     => AttackWithArmy(attackSize),
                MilitaryBranch.Navy     => AttackWithNavy(attackSize),
                MilitaryBranch.AirForce => AttackWithAirForce(attackSize),
                _               => throw new ArgumentException("INVALID BRANCH")
            };

        /// <summary>
        /// Conducts an attack with the player's army.
        /// </summary>
        /// <param name="attackSize">
        /// The number of men and women used in the attack.
        /// </param>
        /// <returns>
        /// The new game state and a message describing the result.
        /// </returns>
        protected abstract (WarState nextState, string message) AttackWithArmy(int attackSize);

        /// <summary>
        /// Conducts an attack with the player's navy.
        /// </summary>
        /// <param name="attackSize">
        /// The number of men and women used in the attack.
        /// </param>
        /// <returns>
        /// The new game state and a message describing the result.
        /// </returns>
        protected abstract (WarState nextState, string message) AttackWithNavy(int attackSize);

        /// <summary>
        /// Conducts an attack with the player's air force.
        /// </summary>
        /// <param name="attackSize">
        /// The number of men and women used in the attack.
        /// </param>
        /// <returns>
        /// The new game state and a message describing the result.
        /// </returns>
        protected abstract (WarState nextState, string message) AttackWithAirForce(int attackSize);
    }
}
