namespace Hammurabi
{
    /// <summary>
    /// Enumerates the different possible outcomes of attempting the various
    /// actions in the game.
    /// </summary>
    public enum ActionResult
    {
        /// <summary>
        /// The action was a success.
        /// </summary>
        Success,

        /// <summary>
        /// The action could not be completed because the city does not have
        /// enough bushels of grain.
        /// </summary>
        InsufficientStores,

        /// <summary>
        /// The action could not be completed because the city does not have
        /// sufficient acreage.
        /// </summary>
        InsufficientLand,

        /// <summary>
        /// The action could not be completed because the city does not have
        /// sufficient population.
        /// </summary>
        InsufficientPopulation,

        /// <summary>
        /// The requested action offended the city steward.
        /// </summary>
        Offense
    }
}
