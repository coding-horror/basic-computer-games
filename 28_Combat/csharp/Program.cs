namespace Game
{
    class Program
    {
        static void Main()
        {
            View.ShowBanner();
            View.ShowInstructions();

            var computerForces = new ArmedForces { Army = 30000, Navy = 20000, AirForce = 22000 };
            var playerForces   = Controller.GetInitialForces(computerForces);

            var state = (WarState) new InitialCampaign(computerForces, playerForces);
            var isFirstTurn = true;

            while (!state.FinalOutcome.HasValue)
            {
                var branch = Controller.GetAttackBranch(state, isFirstTurn);
                var attackSize = Controller.GetAttackSize(state.PlayerForces[branch]);

                var (nextState, message) = state.LaunchAttack(branch, attackSize);
                View.ShowMessage(message);

                state = nextState;
                isFirstTurn = false;
            }

            View.ShowResult(state);
        }
    }
}
