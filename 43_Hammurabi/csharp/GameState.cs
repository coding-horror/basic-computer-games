namespace Hammurabi
{
    /// <summary>
    /// Stores the state of the game.
    /// </summary>
    public record GameState
    {
        /// <summary>
        /// Gets the current game year.
        /// </summary>
        public int Year { get; init; }

        /// <summary>
        /// Gets the city's population.
        /// </summary>
        public int Population { get; init; }

        /// <summary>
        /// Gets the population increase this year.
        /// </summary>
        public int PopulationIncrease { get; init; }

        /// <summary>
        /// Gets the number of people who starved.
        /// </summary>
        public int Starvation { get; init; }

        /// <summary>
        /// Gets the city's size in acres.
        /// </summary>
        public int Acres { get; init; }

        /// <summary>
        /// Gets the price for an acre of land (in bushels).
        /// </summary>
        public int LandPrice { get; init; }

        /// <summary>
        /// Gets the number of bushels of grain in the city stores.
        /// </summary>
        public int Stores { get; init; }

        /// <summary>
        /// Gets the amount of food distributed to the people.
        /// </summary>
        public int FoodDistributed { get; init; }

        /// <summary>
        /// Gets the number of acres that were planted.
        /// </summary>
        public int AcresPlanted { get; init; }

        /// <summary>
        /// Gets the number of bushels produced per acre.
        /// </summary>
        public int Productivity { get; init; }

        /// <summary>
        /// Gets the amount of food lost to rats.
        /// </summary>
        public int Spoilage { get; init; }

        /// <summary>
        /// Gets a flag indicating whether the current year is a plague year.
        /// </summary>
        public bool IsPlagueYear { get; init; }

        /// <summary>
        /// Gets a flag indicating whether the player has been impeached.
        /// </summary>
        public bool IsPlayerImpeached { get; init; }
    }
}
