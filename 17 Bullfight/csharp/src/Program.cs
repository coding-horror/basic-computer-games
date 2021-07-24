namespace Game
{
    class Program
    {
        static void Main()
        {
            Controller.StartGame();

            var mediator = new Mediator();
            foreach (var evt in BullFight.Begin(mediator))
            {
                switch (evt)
                {
                    case Events.MatchStarted matchStarted:
                        View.ShowStartingConditions(matchStarted);
                        break;

                    case Events.BullCharging bullCharging:
                        View.ShowStartOfPass(bullCharging.PassNumber);
                        var (action, riskLevel) = Controller.GetPlayerIntention(bullCharging.PassNumber);
                        switch (action)
                        {
                            case Action.Dodge:
                                mediator.Dodge(riskLevel);
                                break;
                            case Action.Kill:
                                mediator.Kill(riskLevel);
                                break;
                            case Action.Panic:
                                mediator.Panic();
                                break;
                        }
                        break;

                    case Events.PlayerGored playerGored:
                        View.ShowPlayerGored(playerGored.Panicked, playerGored.FirstGoring);
                        break;

                    case Events.PlayerSurvived:
                        View.ShowPlayerSurvives();
                        if (Controller.GetPlayerRunsFromRing())
                            mediator.RunFromRing();
                        else
                            mediator.ContinueFighting();
                        break;

                    case Events.MatchCompleted matchCompleted:
                        View.ShowFinalResult(matchCompleted.Result, matchCompleted.ExtremeBravery, matchCompleted.Reward);
                        break;
                }
            }
        }
    }
}
