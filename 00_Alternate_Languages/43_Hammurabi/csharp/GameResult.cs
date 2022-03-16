namespace Hammurabi
{
    /// <summary>
    /// Stores the final game result.
    /// </summary>
    public record GameResult
    {
        /// <summary>
        /// Gets the player's performance rating.
        /// </summary>
        public PerformanceRating Rating { get; init; }

        /// <summary>
        /// Gets the number of acres in the city per person.
        /// </summary>
        public int AcresPerPerson { get; init; }

        /// <summary>
        /// Gets the number of people who starved the final year in office.
        /// </summary>
        public int FinalStarvation { get; init; }

        /// <summary>
        /// Gets the total number of people who starved.
        /// </summary>
        public int TotalStarvation { get; init; }

        /// <summary>
        /// Gets the average starvation rate per year (as a percentage
        /// of population).
        /// </summary>
        public int AverageStarvationRate { get; init; }

        /// <summary>
        /// Gets the number of people who want to assassinate the player.
        /// </summary>
        public int Assassins { get; init; }

        /// <summary>
        /// Gets a flag indicating whether the player was impeached for
        /// starving too many people.
        /// </summary>
        public bool WasPlayerImpeached { get; init; }
    }
}
