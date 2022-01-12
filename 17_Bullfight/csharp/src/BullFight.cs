using System;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// Provides a method for simulating a bull fight.
    /// </summary>
    public static class BullFight
    {
        /// <summary>
        /// Begins a new fight.
        /// </summary>
        /// <param name="mediator">
        /// Object used to communicate with the player.
        /// </param>
        /// <returns>
        /// The sequence of events that take place during the fight.
        /// </returns>
        /// <remarks>
        /// After receiving each event, the caller must invoke the appropriate
        /// mediator method to inform this coroutine what to do next.  Failure
        /// to do so will result in an exception.
        /// </remarks>
        public static IEnumerable<Events.Event> Begin(Mediator mediator)
        {
            var random = new Random();
            var result = ActionResult.FightContinues;

            var bullQuality          = GetBullQuality();
            var toreadorePerformance = GetHelpQuality(bullQuality);
            var picadorePerformance  = GetHelpQuality(bullQuality);

            var bullStrength    = 6 - (int)bullQuality;
            var assistanceLevel = (12 - (int)toreadorePerformance - (int)picadorePerformance) * 0.1;
            var bravery         = 1.0;
            var style           = 1.0;
            var passNumber      = 0;

            yield return new Events.MatchStarted(
                bullQuality,
                toreadorePerformance,
                picadorePerformance,
                GetHumanCasualties(toreadorePerformance),
                GetHumanCasualties(picadorePerformance),
                GetHorseCasualties(picadorePerformance));

            while (result == ActionResult.FightContinues)
            {
                yield return new Events.BullCharging(++passNumber);

                var (action, riskLevel) = mediator.GetInput<(Action, RiskLevel)>();
                result = action switch
                {
                    Action.Dodge => TryDodge(riskLevel),
                    Action.Kill  => TryKill(riskLevel),
                    _            => Panic()
                };

                var first = true;
                while (result == ActionResult.BullGoresPlayer)
                {
                    yield return new Events.PlayerGored(action == Action.Panic, first);
                    first = false;

                    result = TrySurvive();
                    if (result == ActionResult.FightContinues)
                    {
                        yield return new Events.PlayerSurvived();

                        var runFromRing = mediator.GetInput<bool>();
                        if (runFromRing)
                            result = Flee();
                        else
                            result = IgnoreInjury(action);
                    }
                }
            }

            yield return new Events.MatchCompleted(
                result,
                bravery == 2,
                GetReward());

            Quality GetBullQuality() =>
                (Quality)random.Next(1, 6);

            Quality GetHelpQuality(Quality bullQuality) =>
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

            ActionResult TryDodge(RiskLevel riskLevel)
            {
                var difficultyModifier = riskLevel switch
                {
                    RiskLevel.High   => 3.0,
                    RiskLevel.Medium => 2.0,
                    _                => 0.5
                };

                var outcome = (bullStrength + (difficultyModifier / 10)) * random.NextDouble() /
                    ((assistanceLevel + (passNumber / 10.0)) * 5);

                if (outcome < 0.51)
                {
                    style += difficultyModifier;
                    return ActionResult.FightContinues;
                }
                else
                    return ActionResult.BullGoresPlayer;
            }

            ActionResult TryKill(RiskLevel riskLevel)
            {
                var luck = bullStrength * 10 * random.NextDouble() / (assistanceLevel * 5 * passNumber);

                return ((riskLevel == RiskLevel.High && luck > 0.2) || luck > 0.8) ?
                    ActionResult.BullGoresPlayer : ActionResult.PlayerKillsBull;
            }

            ActionResult Panic() =>
                ActionResult.BullGoresPlayer;

            ActionResult TrySurvive()
            {
                if (random.Next(2) == 0)
                {
                    bravery = 1.5;
                    return ActionResult.BullKillsPlayer;
                }
                else
                    return ActionResult.FightContinues;
            }

            ActionResult Flee()
            {
                bravery = 0.0;
                return ActionResult.PlayerFlees;
            }

            ActionResult IgnoreInjury(Action action)
            {
                if (random.Next(2) == 0)
                {
                    bravery = 2.0;
                    return action == Action.Dodge ? ActionResult.FightContinues : ActionResult.Draw;
                }
                else
                    return ActionResult.BullGoresPlayer;
            }

            Reward GetReward()
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
            }

            double CalculateScore()
            {
                var score = 4.5;

                // Style
                score += style / 6;

                // Assisstance
                score -= assistanceLevel * 2.5;

                // Courage
                score += 4 * bravery;

                // Kill bonus
                score += (result == ActionResult.PlayerKillsBull) ? 4 : 2;

                // Match length
                score -= Math.Pow(passNumber, 2) / 120;

                // Difficulty
                score -= (int)bullQuality;

                return score;
            }
        }
    }
}
