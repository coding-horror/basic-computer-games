using System;

namespace Game
{
    /// <summary>
    /// Represents the armed forces for a country.
    /// </summary>
    public record ArmedForces
    {
        /// <summary>
        /// Gets the number of men and women in the army.
        /// </summary>
        public int Army { get; init; }

        /// <summary>
        /// Gets the number of men and women in the navy.
        /// </summary>
        public int Navy { get; init; }

        /// <summary>
        /// Gets the number of men and women in the air force.
        /// </summary>
        public int AirForce { get; init; }

        /// <summary>
        /// Gets the total number of troops in the armed forces.
        /// </summary>
        public int TotalTroops => Army + Navy + AirForce;

        /// <summary>
        /// Gets the number of men and women in the given branch.
        /// </summary>
        public int this[MilitaryBranch branch] =>
            branch switch
            {
                MilitaryBranch.Army     => Army,
                MilitaryBranch.Navy     => Navy,
                MilitaryBranch.AirForce => AirForce,
                _                       => throw new ArgumentException("INVALID BRANCH")
            };
    }
}
