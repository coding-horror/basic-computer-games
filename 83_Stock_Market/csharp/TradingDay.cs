using System.Collections.Immutable;
using System.Linq;

namespace Game
{
    /// <summary>
    /// Represents a single trading day.
    /// </summary>
    public record TradingDay
    {
        /// <summary>
        /// Gets the average share price of all companies in the market this
        /// day.
        /// </summary>
        public double AverageSharePrice =>
            Companies.Average (company => company.SharePrice);

        /// <summary>
        /// Gets the collection of public listed companies in the stock market
        /// this day.
        /// </summary>
        public ImmutableArray<Company> Companies { get; init; }
    }
}
