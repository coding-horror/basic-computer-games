namespace Game
{
    /// <summary>
    /// Stores the result of a player's turn.
    /// </summary>
    public record TurnResult
    {
        /// <summary>
        /// Gets the code guessed by the player.
        /// </summary>
        public Code Guess { get; }

        /// <summary>
        /// Gets the number of black pegs resulting from the guess.
        /// </summary>
        public int Blacks { get; }

        /// <summary>
        /// Gets the number of white pegs resulting from the guess.
        /// </summary>
        public int Whites { get; }

        /// <summary>
        /// Initializes a new instance of the TurnResult record.
        /// </summary>
        /// <param name="guess">
        /// The player's guess.
        /// </param>
        /// <param name="blacks">
        /// The number of black pegs.
        /// </param>
        /// <param name="whites">
        /// The number of white pegs.
        /// </param>
        public TurnResult(Code guess, int blacks, int whites) =>
            (Guess, Blacks, Whites) = (guess, blacks, whites);
    }
}
