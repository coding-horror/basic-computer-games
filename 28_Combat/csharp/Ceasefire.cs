using System;

namespace Game
{
    /// <summary>
    /// Represents the state of the game after reaching a ceasefire.
    /// </summary>
    public sealed class Ceasefire : WarState
    {
        /// <summary>
        /// Gets a flag indicating whether the player achieved absolute victory.
        /// </summary>
        public override bool IsAbsoluteVictory { get; }

        /// <summary>
        /// Gets the outcome of the war.
        /// </summary>
        public override WarResult? FinalOutcome
        {
            get
            {
                if (IsAbsoluteVictory || PlayerForces.TotalTroops > 3 / 2 * ComputerForces.TotalTroops)
                    return WarResult.PlayerVictory;
                else
                if (PlayerForces.TotalTroops < 2 / 3 * ComputerForces.TotalTroops)
                    return WarResult.ComputerVictory;
                else
                    return WarResult.PeaceTreaty;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Ceasefire class.
        /// </summary>
        /// <param name="computerForces">
        /// The computer's forces.
        /// </param>
        /// <param name="playerForces">
        /// The player's forces.
        /// </param>
        /// <param name="absoluteVictory">
        /// Indicates whether the player acheived absolute victory (defeating
        /// the computer without destroying its military).
        /// </param>
        public Ceasefire(ArmedForces computerForces, ArmedForces playerForces, bool absoluteVictory = false)
            : base(computerForces, playerForces)
        {
            IsAbsoluteVictory = absoluteVictory;
        }

        protected override (WarState nextState, string message) AttackWithArmy(int attackSize) =>
            throw new InvalidOperationException("THE WAR IS OVER");

        protected override (WarState nextState, string message) AttackWithNavy(int attackSize) =>
            throw new InvalidOperationException("THE WAR IS OVER");

        protected override (WarState nextState, string message) AttackWithAirForce(int attackSize) =>
            throw new InvalidOperationException("THE WAR IS OVER");
    }
}
