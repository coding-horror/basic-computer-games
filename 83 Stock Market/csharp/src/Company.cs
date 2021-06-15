namespace Game
{
    /// <summary>
    /// Represents a company.
    /// </summary>
    public record Company
    {
        /// <summary>
        /// Gets the company's name.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the company's three letter stock symbol.
        /// </summary>
        public string StockSymbol { get; init; }

        /// <summary>
        /// Gets the company's current share price.
        /// </summary>
        public double SharePrice { get; init; }
    }
}
