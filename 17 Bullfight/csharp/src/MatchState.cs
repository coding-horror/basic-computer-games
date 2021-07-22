namespace Game
{
    /// <summary>
    /// Stores the current state of the match.
    /// </summary>
    public record MatchState(MatchConditions Conditions)
    {
        /// <summary>
        /// Gets the number of times the bull has charged.
        /// </summary>
        public int PassNumber { get; init; }

        /// <summary>
        /// Measures the player's bravery during the match.
        /// </summary>
        public double Bravery { get; init; }

        /// <summary>
        /// Measures how much style the player showed during the match.
        /// </summary>
        public double Style { get; init; }

        /// <summary>
        /// Gets the result of the player's last action.
        /// </summary>
        public ActionResult Result { get; init; }
    }
}
