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
        public string Name { get; }

        /// <summary>
        /// Gets the company's three letter stock symbol.
        /// </summary>
        public string StockSymbol { get; }

        /// <summary>
        /// Gets the company's current share price.
        /// </summary>
        public double SharePrice { get; init; }

        /// <summary>
        /// Initializes a new Company record.
        /// </summary>
        public Company(string name, string stockSymbol, double sharePrice) =>
            (Name, StockSymbol, SharePrice) = (name, stockSymbol, sharePrice);
    }
}
