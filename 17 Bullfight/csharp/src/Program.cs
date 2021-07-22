using System;

namespace Game
{
    class Program
    {
        static void Main()
        {
            Controller.StartGame();

            var random = new Random();
            var match = Rules.StartMatch(random);
            View.ShowStartingConditions(match.Conditions);

            while (match.Result == ActionResult.FightContinues)
            {
                match = match with { PassNumber = match.PassNumber + 1 };

                View.StartOfPass(match.PassNumber);

                var (action, riskLevel) = Controller.GetPlayerIntention(match.PassNumber);
                match = action switch
                {
                    Action.Dodge => Rules.TryDodge(random, riskLevel, match),
                    Action.Kill  => Rules.TryKill(random, riskLevel, match),
                    _            => Rules.Panic(match)
                };

                var first = true;
                while (match.Result == ActionResult.BullGoresPlayer)
                {
                    View.ShowPlayerGored(action == Action.Panic, first);
                    first = false;

                    match = Rules.TrySurvive(random, match);
                    if (match.Result == ActionResult.FightContinues)
                    {
                        View.ShowPlayerSurvives();

                        if (Controller.PlayerRunsFromRing())
                        {
                            match = Rules.Flee(match);
                        }
                        else
                        {
                            View.ShowPlayerFoolhardy();
                            match = Rules.IgnoreInjury(random, action, match);
                        }
                    }
                }
            }

            View.ShowFinalResult(match.Result, match.Bravery, Rules.GetReward(random, match));
        }
    }
}
