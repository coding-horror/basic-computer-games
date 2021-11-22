namespace Game
{
    /// <summary>
    /// Enumerates the different levels of quality in the game.
    /// </summary>
    /// <remarks>
    /// Quality applies both to the bull and to the help received from the
    /// toreadores and picadores.  Note that the ordinal values are significant
    /// (these are used in various calculations).
    /// </remarks>
    public enum Quality
    {
        Superb  = 1,
        Good    = 2,
        Fair    = 3,
        Poor    = 4,
        Awful   = 5
    }
}
