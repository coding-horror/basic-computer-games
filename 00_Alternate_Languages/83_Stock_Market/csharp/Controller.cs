using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class Controller
    {
        /// <summary>
        /// Manages the initial interaction with the user.
        /// </summary>
        public static void StartGame()
        {
            View.ShowBanner();

            var showInstructions = GetYesOrNo(View.PromptShowInstructions);
            View.ShowSeparator();
            if (showInstructions)
                View.ShowInstructions();

            View.ShowSeparator();
        }

        /// <summary>
        /// Gets a yes or no answer from the user.
        /// </summary>
        /// <param name="prompt">
        /// Displays the prompt.
        /// </param>
        /// <returns>
        /// True if the user answered yes and false if he or she answered no.
        /// </returns>
        public static bool GetYesOrNo(Action prompt)
        {
            prompt();

            var response = default(char);
            do
            {
                response = Console.ReadKey(intercept: true).KeyChar;
            }
            while (response != '0' && response != '1');

            View.ShowChar(response);
            return response == '1';
        }

        /// <summary>
        /// Gets a transaction amount for each company in the given collection
        /// of companies and returns the updated assets.
        /// </summary>
        /// <param name="assets">
        /// The assets to update.
        /// </param>
        /// <param name="companies">
        /// The collection of companies.
        /// </param>
        /// <returns>
        /// The updated assets.
        /// </returns>
        public static Assets UpdateAssets(Assets assets, IEnumerable<Company> companies)
        {
            while (true)
            {
                View.PromptEnterTransactions();

                var result = Broker.Apply (
                    assets,
                    companies.Select(GetTransactionAmount).ToList(),
                    companies);

                switch (result)
                {
                    case (Assets newAssets, TransactionResult.Ok):
                        return newAssets;
                    case (_, TransactionResult.Oversold):
                        View.ShowOversold();
                        break;
                    case (Assets newAssets, TransactionResult.Overspent):
                        View.ShowOverspent(-newAssets.Cash);
                        break;
                }
            }
        }

        /// <summary>
        /// Gets a transaction amount for the given company.
        /// </summary>
        /// <param name="company">
        /// The company to buy or sell.
        /// </param>
        /// <returns>
        /// The number of shares to buy or sell.
        /// </returns>
        public static int GetTransactionAmount(Company company)
        {
            while (true)
            {
                View.PromptBuySellCompany(company);

                var input = Console.ReadLine();
                if (input is null)
                    Environment.Exit(0);
                else
                if (!Int32.TryParse(input, out var amount))
                    View.PromptValidInteger();
                else
                    return amount;
            }
        }
    }
}
