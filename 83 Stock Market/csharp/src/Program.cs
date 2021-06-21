using System;
using System.Collections.Immutable;
using System.Linq;

namespace Game
{
    class Program
    {
        /// <summary>
        /// Defines the set of companies that will be simulated in the game.
        /// </summary>
        private static ImmutableArray<Company> Companies = ImmutableArray.CreateRange(new[]
        {
            new Company { Name = "INT. BALLISTIC MISSILES",     StockSymbol = "IBM", SharePrice = 100 },
            new Company { Name = "RED CROSS OF AMERICA",        StockSymbol = "RCA", SharePrice = 85  },
            new Company { Name = "LICHTENSTEIN, BUMRAP & JOKE", StockSymbol = "LBJ", SharePrice = 150 },
            new Company { Name = "AMERICAN BANKRUPT CO.",       StockSymbol = "ABC", SharePrice = 140 },
            new Company { Name = "CENSURED BOOKS STORE",        StockSymbol = "CBS", SharePrice = 110 }
        });

        static void Main()
        {
            var assets = new Assets
            {
                Cash      = 10000.0,
                Portfolio = ImmutableArray.CreateRange(Enumerable.Repeat(0, Companies.Length))
            };

            var previousDay = default(TradingDay);

            Controller.StartGame();

            foreach (var day in StockMarket.Simulate(Companies))
            {
                if (previousDay is null)
                    View.ShowCompanies(day.Companies);
                else
                    View.ShowTradeResults(day, previousDay, assets);

                View.ShowAssets(assets, day.Companies);

                if (previousDay is not null && !Controller.GetYesOrNo(View.PromptContinue))
                    break;

                assets      = Controller.UpdateAssets(assets, day.Companies);
                previousDay = day;
            }

            View.ShowFarewell();
        }
    }
}
