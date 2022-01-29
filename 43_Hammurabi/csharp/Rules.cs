using System;
using System.Collections.Generic;
using System.Linq;

namespace Hammurabi
{
    public static class Rules
    {
        /// <summary>
        /// Creates the initial state for a new game.
        /// </summary>
        public static GameState BeginGame() =>
            new GameState
            {
                Year                = 0,
                Population          = 95,
                PopulationIncrease  = 5,
                Starvation          = 0,
                Acres               = 1000,
                Stores              = 0,
                AcresPlanted        = 1000,
                Productivity        = 3,
                Spoilage            = 200,
                IsPlagueYear        = false,
                IsPlayerImpeached   = false
            };

        /// <summary>
        /// Updates the game state to start a new turn.
        /// </summary>
        public static GameState BeginTurn(GameState state, Random random) =>
            state with
            {
                Year            = state.Year + 1,
                Population      = (state.Population + state.PopulationIncrease - state.Starvation) / (state.IsPlagueYear ? 2 : 1),
                LandPrice       = random.Next(10) + 17,
                Stores          = state.Stores + (state.AcresPlanted * state.Productivity) - state.Spoilage,
                AcresPlanted    = 0,
                FoodDistributed = 0
            };

        /// <summary>
        /// Attempts to purchase the given number of acres.
        /// </summary>
        /// <returns>
        /// The updated game state and action result.
        /// </returns>
        public static (GameState newState, ActionResult result) BuyLand(GameState state, int amount)
        {
            var price = state.LandPrice * amount;

            if (price < 0)
                return (state, ActionResult.Offense);
            else
            if (price > state.Stores)
                return (state, ActionResult.InsufficientStores);
            else
                return (state with { Acres = state.Acres + amount, Stores = state.Stores - price }, ActionResult.Success);
        }

        /// <summary>
        /// Attempts to sell the given number of acres.
        /// </summary>
        /// <returns>
        /// The updated game state and action result.
        /// </returns>
        public static (GameState newState, ActionResult result) SellLand(GameState state, int amount)
        {
            var price = state.LandPrice * amount;

            if (price < 0)
                return (state, ActionResult.Offense);
            else
            if (amount >= state.Acres)
                return (state, ActionResult.InsufficientLand);
            else
                return (state with { Acres = state.Acres - amount, Stores = state.Stores + price }, ActionResult.Success);
        }

        /// <summary>
        /// Attempts to feed the people the given number of buschels.
        /// </summary>
        /// <returns>
        /// <returns>
        /// The updated game state and action result.
        /// </returns>
        public static (GameState newState, ActionResult result) FeedPeople(GameState state, int amount)
        {
            if (amount < 0)
                return (state, ActionResult.Offense);
            else
            if (amount > state.Stores)
                return (state, ActionResult.InsufficientStores);
            else
                return (state with { Stores = state.Stores - amount, FoodDistributed = state.FoodDistributed + amount }, ActionResult.Success);
        }

        /// <summary>
        /// Attempts to plant crops on the given number of acres.
        /// </summary>
        /// <returns>
        /// The updated game state and action result.
        /// </returns>
        public static (GameState newState, ActionResult result) PlantCrops(GameState state, int amount)
        {
            var storesRequired = amount / 2;
            var maxAcres       = state.Population * 10;

            if (amount < 0)
                return (state, ActionResult.Offense);
            else
            if (amount > state.Acres)
                return (state, ActionResult.InsufficientLand);
            else
            if (storesRequired > state.Stores)
                return (state, ActionResult.InsufficientStores);
            else
            if ((state.AcresPlanted + amount) > maxAcres)
                return (state, ActionResult.InsufficientPopulation);
            else
                return (state with
                {
                    AcresPlanted = state.AcresPlanted + amount,
                    Stores       = state.Stores - storesRequired,
                }, ActionResult.Success);
        }

        /// <summary>
        /// Ends the current turn and returns the updated game state.
        /// </summary>
        public static GameState EndTurn(GameState state, Random random)
        {
            var productivity = random.Next(1, 6);
            var harvest = productivity * state.AcresPlanted;

            var spoilage = random.Next(1, 6) switch
            {
                2 => state.Stores / 2,
                4 => state.Stores / 4,
                _ => 0
            };

            var populationIncrease= (int)((double)random.Next(1, 6) * (20 * state.Acres + state.Stores + harvest - spoilage) / state.Population / 100 + 1);

            var plagueYear = random.Next(20) < 3;

            var peopleFed  = state.FoodDistributed / 20;
            var starvation = peopleFed < state.Population ? state.Population - peopleFed : 0;
            var impeached  = starvation > state.Population * 0.45;

            return state with
            {
                Productivity       = productivity,
                Spoilage           = spoilage,
                PopulationIncrease = populationIncrease,
                Starvation         = starvation,
                IsPlagueYear       = plagueYear,
                IsPlayerImpeached  = impeached
            };
        }

        /// <summary>
        /// Examines the game's history to arrive at the final result.
        /// </summary>
        public static GameResult GetGameResult(IEnumerable<GameState> history, Random random)
        {
            var (_, averageStarvationRate, totalStarvation, finalState) = history.Aggregate(
                (count: 0, starvationRate: 0, totalStarvation: 0, finalState: default(GameState)),
                (stats, state) =>
                (
                    stats.count + 1,
                    ((stats.starvationRate * stats.count) + (state.Starvation * 100 / state.Population)) / (stats.count + 1),
                    stats.totalStarvation + state.Starvation,
                    state
                ));

            var acresPerPerson = finalState.Acres / finalState.Population;

            var rating = finalState.IsPlayerImpeached ?
                PerformanceRating.Disgraceful :
                (averageStarvationRate, acresPerPerson) switch
                {
                    (> 33, _) => PerformanceRating.Disgraceful,
                    (_, < 7)  => PerformanceRating.Disgraceful,
                    (> 10, _) => PerformanceRating.Bad,
                    (_, < 9)  => PerformanceRating.Bad,
                    (> 3, _)  => PerformanceRating.Ok,
                    (_, < 10) => PerformanceRating.Ok,
                    _         => PerformanceRating.Terrific
                };

            var assassins = rating == PerformanceRating.Ok ?
                random.Next(0, (int)(finalState.Population * 0.8)) : 0;

            return new GameResult
            {
                Rating                = rating,
                AcresPerPerson        = acresPerPerson,
                FinalStarvation       = finalState.Starvation,
                TotalStarvation       = totalStarvation,
                AverageStarvationRate = averageStarvationRate,
                Assassins             = assassins,
                WasPlayerImpeached    = finalState.IsPlayerImpeached
            };
        }
    }
}
