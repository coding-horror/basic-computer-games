namespace Letter
{
    /// <summary>
    /// Holds the current state.
    /// </summary>
    internal class GameState
    {
        /// <summary>
        /// Initialise the game state with a random letter.
        /// </summary>
        public GameState()
        {
            Letter = GetRandomLetter();
            GuessesSoFar = 0;
        }

        /// <summary>
        /// The letter that the user is guessing.
        /// </summary>
        public char Letter { get; set; }

        /// <summary>
        /// The number of guesses the user has had so far.
        /// </summary>
        public int GuessesSoFar { get; set; }

        /// <summary>
        /// Get a random character (A-Z) for the user to guess.
        /// </summary>
        internal static char GetRandomLetter()
        {
            var random = new Random();
            var randomNumber = random.Next(0, 26);
            return (char)('A' + randomNumber);
        }
    }
}
