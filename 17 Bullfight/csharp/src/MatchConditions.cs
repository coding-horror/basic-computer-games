namespace Game
{
    /// <summary>
    /// Stores the initial conditions of a match.
    /// </summary>
    public record MatchConditions
    {
        /// <summary>
        /// Gets the quality of the bull.
        /// </summary>
        public Quality BullQuality { get; init; }

        /// <summary>
        /// Gets the quality of help received from the toreadores.
        /// </summary>
        public Quality ToreadorePerformance { get; init; }

        /// <summary>
        /// Gets the quality of help received from the picadores.
        /// </summary>
        public Quality PicadorePerformance { get; init; }

        /// <summary>
        /// Gets the number of toreadores killed while preparing for the
        /// final round.
        /// </summary>
        public int ToreadoresKilled { get; init; }

        /// <summary>
        /// Gets the number of picadores killed while preparing for the
        /// final round.
        /// </summary>
        public int PicadoresKilled { get; init; }

        /// <summary>
        /// Gets the number of horses killed while preparing for the final
        /// round.
        /// </summary>
        public int HorsesKilled { get; init; }
    }
}
