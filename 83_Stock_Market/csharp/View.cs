using System;
using System.Collections.Generic;
using System.Linq;
using Game.Extensions;

namespace Game
{
    /// <summary>
    /// Contains functions for displaying information to the user.
    /// </summary>
    public static class View
    {
        public static void ShowBanner()
        {
            Console.WriteLine("                             STOCK MARKET");
            Console.WriteLine("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void ShowInstructions()
        {
            Console.WriteLine("THIS PROGRAM PLAYS THE STOCK MARKET.  YOU WILL BE GIVEN");
            Console.WriteLine("$10,000 AND MAY BUY OR SELL STOCKS.  THE STOCK PRICES WILL");
            Console.WriteLine("BE GENERATED RANDOMLY AND THEREFORE THIS MODEL DOES NOT");
            Console.WriteLine("REPRESENT EXACTLY WHAT HAPPENS ON THE EXCHANGE.  A TABLE");
            Console.WriteLine("OF AVAILABLE STOCKS, THEIR PRICES, AND THE NUMBER OF SHARES");
            Console.WriteLine("IN YOUR PORTFOLIO WILL BE PRINTED.  FOLLOWING THIS, THE");
            Console.WriteLine("INITIALS OF EACH STOCK WILL BE PRINTED WITH A QUESTION");
            Console.WriteLine("MARK.  HERE YOU INDICATE A TRANSACTION.  TO BUY A STOCK");
            Console.WriteLine("TYPE +NNN, TO SELL A STOCK TYPE -NNN, WHERE NNN IS THE");
            Console.WriteLine("NUMBER OF SHARES.  A BROKERAGE FEE OF 1% WILL BE CHARGED");
            Console.WriteLine("ON ALL TRANSACTIONS.  NOTE THAT IF A STOCK'S VALUE DROPS");
            Console.WriteLine("TO ZERO IT MAY REBOUND TO A POSITIVE VALUE AGAIN.  YOU");
            Console.WriteLine("HAVE $10,000 TO INVEST.  USE INTEGERS FOR ALL YOUR INPUTS.");
            Console.WriteLine("(NOTE:  TO GET A 'FEEL' FOR THE MARKET RUN FOR AT LEAST");
            Console.WriteLine("10 DAYS)");
            Console.WriteLine("-----GOOD LUCK!-----");
        }

        public static void ShowCompanies(IEnumerable<Company> companies)
        {
            var maxNameLength = companies.Max(company => company.Name.Length);

            Console.WriteLine($"{"STOCK".PadRight(maxNameLength)} INITIALS      PRICE/SHARE");
            foreach (var company in companies)
                Console.WriteLine($"{company.Name.PadRight(maxNameLength)}   {company.StockSymbol}          {company.SharePrice:0.00}");

            Console.WriteLine();
            Console.WriteLine($"NEW YORK STOCK EXCHANGE AVERAGE: {companies.Average(company => company.SharePrice):0.00}");
            Console.WriteLine();
        }

        public static void ShowTradeResults(TradingDay day, TradingDay previousDay, Assets assets)
        {
            var results = EnumerableExtensions.Zip(
                day.Companies,
                previousDay.Companies,
                assets.Portfolio,
                (company, previous, shares) =>
                (
                    stockSymbol: company.StockSymbol,
                    price: company.SharePrice,
                    shares,
                    value: shares * company.SharePrice,
                    change: company.SharePrice - previous.SharePrice
                )).ToList();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("**********     END OF DAY'S TRADING     **********");
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("STOCK\tPRICE/SHARE\tHOLDINGS\tVALUE\tNET PRICE CHANGE");
            foreach (var result in results)
                Console.WriteLine($"{result.stockSymbol}\t{result.price}\t\t{result.shares}\t\t{result.value:0.00}\t\t{result.change:0.00}");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var averagePrice = day.AverageSharePrice;
            var averagePriceChange = averagePrice - previousDay.AverageSharePrice;

            Console.WriteLine($"NEW YORK STOCK EXCHANGE AVERAGE: {averagePrice:0.00} NET CHANGE {averagePriceChange:0.00}");
            Console.WriteLine();
        }

        public static void ShowAssets(Assets assets, IEnumerable<Company> companies)
        {
            var totalStockValue = Enumerable.Zip(
                assets.Portfolio,
                companies,
                (shares, company) => shares * company.SharePrice).Sum();

            Console.WriteLine($"TOTAL STOCK ASSETS ARE   ${totalStockValue:0.00}");
            Console.WriteLine($"TOTAL CASH ASSETS ARE    ${assets.Cash:0.00}");
            Console.WriteLine($"TOTAL ASSETS ARE         ${totalStockValue + assets.Cash:0.00}");
            Console.WriteLine();
        }

        public static void ShowOversold()
        {
            Console.WriteLine();
            Console.WriteLine("YOU HAVE OVERSOLD A STOCK; TRY AGAIN.");
        }

        public static void ShowOverspent(double amount)
        {
            Console.WriteLine();
            Console.WriteLine($"YOU HAVE USED ${amount:0.00} MORE THAN YOU HAVE.");
        }

        public static void ShowFarewell()
        {
            Console.WriteLine("HOPE YOU HAD FUN!!");
        }

        public static void ShowSeparator()
        {
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void ShowChar(char c)
        {
            Console.WriteLine(c);
        }

        public static void PromptShowInstructions()
        {
            Console.Write("DO YOU WANT THE INSTRUCTIONS (YES-TYPE 1, NO-TYPE 0)? ");
        }

        public static void PromptContinue()
        {
            Console.Write("DO YOU WISH TO CONTINUE (YES-TYPE 1, NO-TYPE 0)? ");
        }

        public static void PromptEnterTransactions()
        {
            Console.WriteLine("WHAT IS YOUR TRANSACTION IN");
        }

        public static void PromptBuySellCompany(Company company)
        {
            Console.Write($"{company.StockSymbol}? ");
        }

        public static void PromptValidInteger()
        {
            Console.WriteLine("PLEASE ENTER A VALID INTEGER");
        }
    }
}
