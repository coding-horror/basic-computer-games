using System.Collections.Immutable;

namespace Game
{
    /// <summary>
    /// Stores the player's assets.
    /// </summary>
    public record Assets
    {
        /// <summary>
        /// Gets the player's amount of cash.
        /// </summary>
        public double Cash { get; init; }

        /// <summary>
        /// Gets the number of stocks owned of each company.
        /// </summary>
        public ImmutableArray<int> Portfolio { get; init; }
    }
}
