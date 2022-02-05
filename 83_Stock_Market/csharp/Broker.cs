using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Game
{
    /// <summary>
    /// Contains functions for exchanging assets.
    /// </summary>
    public static class Broker
    {
        /// <summary>
        /// Applies the given set of transactions to the given set of assets.
        /// </summary>
        /// <param name="assets">
        /// The assets to update.
        /// </param>
        /// <param name="transactions">
        /// The set of stocks to purchase or sell.  Positive values indicate
        /// purchaes and negative values indicate sales.
        /// </param>
        /// <param name="companies">
        /// The collection of companies.
        /// </param>
        /// <returns>
        /// Returns the sellers new assets and a code indicating the result
        /// of the transaction.
        /// </returns>
        public static (Assets newAssets, TransactionResult result) Apply(Assets assets, IEnumerable<int> transactions, IEnumerable<Company> companies)
        {
            var (netCost, transactionSize) = Enumerable.Zip(
                    transactions,
                    companies,
                    (amount, company) => (amount * company.SharePrice))
                .Aggregate(
                    (netCost: 0.0, transactionSize: 0.0),
                    (accumulated, amount) => (accumulated.netCost + amount, accumulated.transactionSize + Math.Abs(amount)));

            var brokerageFee = 0.01 * transactionSize;

            var newAssets = assets with
            {
                Cash      = assets.Cash - netCost - brokerageFee,
                Portfolio = ImmutableArray.CreateRange(Enumerable.Zip(
                    assets.Portfolio,
                    transactions,
                    (sharesOwned, delta) => sharesOwned + delta))
            };

            if (newAssets.Portfolio.Any(amount => amount < 0))
                return (newAssets, TransactionResult.Oversold);
            else
            if (newAssets.Cash < 0)
                return (newAssets, TransactionResult.Overspent);
            else
                return (newAssets, TransactionResult.Ok);
        }
    }
}
