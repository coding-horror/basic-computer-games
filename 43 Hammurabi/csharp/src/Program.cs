using System;
using System.Collections.Immutable;

namespace Hammurabi
{
    public static class Program
    {
        public const int GameLength = 10;

        public static void Main(string[] args)
        {
            var random  = new Random ((int) (DateTime.UtcNow.Ticks / 10000)) ;
            var state   = Rules.BeginGame();
            var history = ImmutableList<GameState>.Empty;

            View.ShowBanner();

            try
            {
                while (state.Year <= GameLength && !state.IsImpeached)
                {
                    state = Rules.BeginTurn(state, random);
                    View.ShowCitySummary(state);

                    View.ShowLandPrice(state);
                    var newState = Controller.TryUntilSuccess(state, View.PromptBuyLand, Rules.BuyLand);
                    state = newState.Acres != state.Acres ?
                        newState : Controller.TryUntilSuccess(state, View.PromptSellLand, Rules.SellLand);

                    View.ShowSeparator();
                    state = Controller.TryUntilSuccess(state, View.PromptFeedPeople, Rules.FeedPeople);

                    View.ShowSeparator();
                    state = Controller.TryUntilSuccess(state, View.PromptPlantCrops, Rules.PlantCrops);

                    state = Rules.EndTurn(state, random);
                    history = history.Add(state);
                }

                var result = Rules.GetGameResult(history, random);
                View.ShowGameResult(result);
            }
            catch (GreatOffence)
            {
                View.ShowGreatOffence();
            }

            View.ShowFarewell();
        }
    }
}
