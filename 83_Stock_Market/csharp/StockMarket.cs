using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Game.Extensions;

namespace Game
{
    /// <summary>
    /// Provides a method for simulating a stock market.
    /// </summary>
    public static class StockMarket
    {
        /// <summary>
        /// Simulates changes in the stock market over time.
        /// </summary>
        /// <param name="companies">
        /// The collection of companies that will participate in the market.
        /// </param>
        /// <returns>
        /// An infinite sequence of trading days.  Each day represents the
        /// state of the stock market at the start of that day.
        /// </returns>
        public static IEnumerable<TradingDay> Simulate(ImmutableArray<Company> companies)
        {
            var random = new Random();

            var cyclicParameters = EnumerableExtensions.Zip(
                Trends(random, 1, 5),
                PriceSpikes(random, companies.Length, 1, 5),
                PriceSpikes(random, companies.Length, 1, 5),
                (trend, company1, company2) => (trend, positiveSpike: company1, negativeSpike: company2));

            return cyclicParameters.SelectAndAggregate(
                new TradingDay
                {
                    Companies = companies
                },
                (parameters, previousDay) => previousDay with
                {
                    Companies = previousDay.Companies.Map(
                        (company, index) => AdjustSharePrice(
                            random,
                            company,
                            parameters.trend,
                            parameters.positiveSpike == index,
                            parameters.negativeSpike == index))
                });
        }

        /// <summary>
        /// Creates a copy of a company with a randomly adjusted share price,
        /// based on the given parameters.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <param name="company">
        /// The company to adjust.
        /// </param>
        /// <param name="trend">
        /// The slope of the overall market price trend.
        /// </param>
        /// <param name="positiveSpike">
        /// True if the function should simulate a positive spike in the
        /// company's share price.
        /// </param>
        /// <param name="negativeSpike">
        /// True if the function should simulate a negative spike in the
        /// company's share price.
        /// </param>
        /// <returns>
        /// The adjusted company.
        /// </returns>
        private static Company AdjustSharePrice(Random random, Company company, double trend, bool positiveSpike, bool negativeSpike)
        {
            var boost = random.Next(4) * 0.25;

            var spikeAmount = 0.0;

            if (positiveSpike)
                spikeAmount = 10;

            if (negativeSpike)
                spikeAmount = spikeAmount - 10;

            var priceChange = (int)(trend * company.SharePrice) + boost + (int)(3.5 - (6 * random.NextDouble())) + spikeAmount;

            var newPrice = company.SharePrice + priceChange;
            if (newPrice < 0)
                newPrice = 0;

            return company with { SharePrice = newPrice };
        }

        /// <summary>
        /// Generates an infinite sequence of market trends.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <param name="minDays">
        /// The minimum number of days each trend should last.
        /// </param>
        /// <param name="maxDays">
        /// The maximum number of days each trend should last.
        /// </param>
        public static IEnumerable<double> Trends(Random random, int minDays, int maxDays) =>
            random.Integers(minDays, maxDays + 1).SelectMany(daysInCycle => Enumerable.Repeat(GenerateTrend(random), daysInCycle));

        /// <summary>
        /// Generates a random value for the market trend.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <returns>
        /// A trend value in the range [-0.1, 0.1].
        /// </returns>
        private static double GenerateTrend(Random random) =>
            ((int)(random.NextDouble() * 10 + 0.5) / 100.0) * (random.Next(2) == 0 ? 1 : -1) ;

        /// <summary>
        /// Generates an infinite sequence of price spikes.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <param name="companyCount">
        /// The number of companies.
        /// </param>
        /// <param name="minDays">
        /// The minimum number of days in between price spikes.
        /// </param>
        /// <param name="maxDays">
        /// The maximum number of days in between price spikes.
        /// </param>
        /// <returns>
        /// An infinite sequence of random company indexes and null values.
        /// A non-null value means that the corresponding company should
        /// experience a price spike.
        /// </returns>
        private static IEnumerable<int?> PriceSpikes(Random random, int companyCount, int minDays, int maxDays) =>
            random.Integers(minDays, maxDays + 1)
                .SelectMany(
                    daysInCycle => Enumerable.Range(0, daysInCycle),
                    (daysInCycle, dayNumber) => dayNumber == 0 ? random.Next(companyCount) : default(int?));
    }
}
