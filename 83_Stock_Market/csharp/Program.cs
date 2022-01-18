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
        private readonly static ImmutableArray<Company> Companies = ImmutableArray.CreateRange(new[]
        {
            new Company("INT. BALLISTIC MISSILES",     "IBM", sharePrice:100),
            new Company("RED CROSS OF AMERICA",        "RCA", sharePrice:85 ),
            new Company("LICHTENSTEIN, BUMRAP & JOKE", "LBJ", sharePrice:150),
            new Company("AMERICAN BANKRUPT CO.",       "ABC", sharePrice:140),
            new Company("CENSURED BOOKS STORE",        "CBS", sharePrice:110)
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
