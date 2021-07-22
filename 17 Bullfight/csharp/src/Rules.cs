using System;

namespace Game
{
    /// <summary>
    /// Provides functions implementing the rules of the game.
    /// </summary>
    public static class Rules
    {
        /// <summary>
        /// Gets the state of a new match.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        public static MatchState StartMatch(Random random)
        {
            var bullQuality          = GetBullQuality();
            var toreadorePerformance = GetHelpQuality();
            var picadorePerformance  = GetHelpQuality();

            var conditions = new MatchConditions
            {
                BullQuality          = bullQuality,
                ToreadorePerformance = toreadorePerformance,
                PicadorePerformance  = picadorePerformance,
                ToreadoresKilled     = GetHumanCasualties(toreadorePerformance),
                PicadoresKilled      = GetHumanCasualties(picadorePerformance),
                HorsesKilled         = GetHorseCasualties(picadorePerformance)
            };

            return new MatchState(conditions)
            {
                Bravery = 1.0,
                Style   = 1.0
            };

            Quality GetBullQuality() =>
                (Quality)random.Next(1, 6);

            Quality GetHelpQuality() =>
                ((3.0 / (int)bullQuality) * random.NextDouble()) switch
                {
                    < 0.37 => Quality.Superb,
                    < 0.50 => Quality.Good,
                    < 0.63 => Quality.Fair,
                    < 0.87 => Quality.Poor,
                    _      => Quality.Awful
                };

            int GetHumanCasualties(Quality performance) =>
                performance switch
                {
                    Quality.Poor  => random.Next(0, 2),
                    Quality.Awful => random.Next(1, 3),
                    _             => 0
                };

            int GetHorseCasualties(Quality performance) =>
                performance switch
                {
                    // NOTE: The code for displaying a single horse casuality
                    //  following a poor picadore peformance was unreachable
                    //  in the original BASIC version.  I've assumed this was
                    //  a bug.
                    Quality.Poor  => 1,
                    Quality.Awful => random.Next(1, 3),
                    _             => 0
                };
        }

        /// <summary>
        /// Determines the result when the player attempts to dodge the bull.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <param name="riskLevel">
        /// The level of risk in the dodge manoeuvre chosen.
        /// </param>
        /// <param name="match">
        /// The current match state.
        /// </param>
        /// <returns>
        /// The updated match state.
        /// </returns>
        public static MatchState TryDodge(Random random, RiskLevel riskLevel, MatchState match)
        {
            var difficultyModifier = riskLevel switch
            {
                RiskLevel.High   => 3.0,
                RiskLevel.Medium => 2.0,
                _                => 0.5
            };

            var outcome = (GetBullStrength(match) + (difficultyModifier / 10)) * random.NextDouble() /
                ((GetAssisstance(match) + (match.PassNumber / 10.0)) * 5);

            return outcome < 0.51 ?
                match with { Result = ActionResult.FightContinues, Style = match.Style + difficultyModifier } :
                match with { Result = ActionResult.BullGoresPlayer };
        }

        /// <summary>
        /// Determines the result when the player attempts to kill the bull.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <param name="riskLevel">
        /// The level of risk in the manoeuvre chosen.
        /// </param>
        /// <param name="match">
        /// The current match state.
        /// </param>
        /// <returns>
        /// The updated match state.
        /// </returns>
        public static MatchState TryKill(Random random, RiskLevel riskLevel, MatchState match)
        {
            var K = GetBullStrength(match) * 10 * random.NextDouble() / (GetAssisstance(match) * 5 * match.PassNumber);

            return ((riskLevel == RiskLevel.High && K > 0.2) || K > 0.8) ?
                match with { Result = ActionResult.BullGoresPlayer } :
                match with { Result = ActionResult.PlayerKillsBull };
        }

        /// <summary>
        /// Determines if the player survives being gored by the bull.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <param name="match">
        /// The current match state.
        /// </param>
        /// <returns>
        /// The updated match state.
        /// </returns>
        public static MatchState TrySurvive(Random random, MatchState match) =>
            (random.Next(2) == 0) ?
                match with { Result = ActionResult.BullKillsPlayer, Bravery = 1.5 } :
                match with { Result = ActionResult.FightContinues };

        /// <summary>
        /// Determines the result when the player panics and fails to do anything.
        /// </summary>
        /// <param name="match">
        /// The match state.
        /// </param>
        public static MatchState Panic(MatchState match) =>
            match with { Result = ActionResult.BullGoresPlayer };

        /// <summary>
        /// Determines the result when the player flees the ring.
        /// </summary>
        /// <param name="match">
        /// The current match state.
        /// </param>
        /// <returns>
        /// The updated match state.
        /// </returns>
        public static MatchState Flee(MatchState match) =>
            match with { Result = ActionResult.PlayerFlees, Bravery = 0.0 };

        /// <summary>
        /// Determines the result when the player decides to continue fighting
        /// following an injury.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <param name="action">
        /// The action the player took that lead to the injury.
        /// </param>
        /// <param name="match">
        /// The current match state.
        /// </param>
        /// <returns>
        /// The updated match state.
        /// </returns>
        public static MatchState IgnoreInjury(Random random, Action action, MatchState match) =>
            (random.Next(2) == 0) ?
                match with { Result = action == Action.Dodge ? ActionResult.FightContinues : ActionResult.Draw, Bravery = 2.0 } :
                match with { Result = ActionResult.BullGoresPlayer };

        /// <summary>
        /// Gets the player's reward for completing a match.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <param name="match">
        /// The final match state.
        /// </param>
        public static Reward GetReward(Random random, MatchState match)
        {
            var score = CalculateScore();

            if (score * random.NextDouble() < 2.4)
                return Reward.Nothing;
            else
            if (score * random.NextDouble() < 4.9)
                return Reward.OneEar;
            else
            if (score * random.NextDouble() < 7.4)
                return Reward.TwoEars;
            else
                return Reward.CarriedFromRing;

            double CalculateScore()
            {
                var score = 4.5;

                // Style
                score += match.Style / 6;

                // Assisstance
                score -= GetAssisstance(match) * 2.5;

                // Courage
                score += 4 * match.Bravery;

                // Kill bonus
                score += (match.Result == ActionResult.PlayerKillsBull) ? 4 : 2;

                // Match length
                score -= Math.Pow(match.PassNumber, 2) / 120;

                // Difficulty
                score -= (int)match.Conditions.BullQuality;

                return score;
            }
        }

        /// <summary>
        /// Calculates the strength of the bull in a match.
        /// </summary>
        private static double GetBullStrength(MatchState match) =>
            6 - (int)match.Conditions.BullQuality;

        /// <summary>
        /// Gets the amount of assistance received from the toreadores and
        /// picadores in a match.
        /// </summary>
        private static double GetAssisstance(MatchState match) =>
            GetPerformanceBonus(match.Conditions.ToreadorePerformance) +
            GetPerformanceBonus(match.Conditions.PicadorePerformance);

        /// <summary>
        /// Gets the amount of assistance rendered by a performance of the
        /// given quality.
        /// </summary>
        private static double GetPerformanceBonus(Quality performance) =>
            (6 - (int)performance) * 0.1;
    }
}
