namespace Game
{
    /// <summary>
    /// Enumerates the different possible outcomes of applying a transaction.
    /// </summary>
    public enum TransactionResult
    {
        /// <summary>
        /// The transaction was successful.
        /// </summary>
        Ok,

        /// <summary>
        /// The transaction failed because the seller tried to sell more shares
        /// than he or she owns.
        /// </summary>
        Oversold,

        /// <summary>
        /// The transaction failed because the net cost was greater than the
        /// seller's available cash.
        /// </summary>
        Overspent
    }
}
