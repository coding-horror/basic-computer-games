using System;

namespace Game
{
    /// <summary>
    /// Contains functions for getting input from the user.
    /// </summary>
    public static class Controller
    {
        /// <summary>
        /// Handles the initial interaction with the player.
        /// </summary>
        public static void StartGame()
        {
            View.ShowBanner();
            View.PromptShowInstructions();

            var input = Console.ReadLine();
            if (input is null)
                Environment.Exit(0);

            if (input.ToUpperInvariant() != "NO")
                View.ShowInstructions();

            View.ShowSeparator();
        }

        /// <summary>
        /// Gets the player's action for the current round.
        /// </summary>
        /// <param name="passNumber">
        /// The current pass number.
        /// </param>
        public static (Action action, RiskLevel riskLevel) GetPlayerIntention(int passNumber)
        {
            if (passNumber < 3)
                View.PromptKillBull();
            else
                View.PromptKillBullBrief();

            var attemptToKill = GetYesOrNo();

            if (attemptToKill)
            {
                View.PromptKillMethod();

                var input = Console.ReadLine();
                if (input is null)
                    Environment.Exit(0);

                return input switch
                {
                    "4" => (Action.Kill,  RiskLevel.High),
                    "5" => (Action.Kill,  RiskLevel.Low),
                    _   => (Action.Panic, default(RiskLevel))
                };
            }
            else
            {
                if (passNumber < 2)
                    View.PromptCapeMove();
                else
                    View.PromptCapeMoveBrief();

                var action = Action.Panic;
                var riskLevel = default(RiskLevel);

                while (action == Action.Panic)
                {
                    var input = Console.ReadLine();
                    if (input is null)
                        Environment.Exit(0);

                    (action, riskLevel) = input switch
                    {
                        "0" => (Action.Dodge, RiskLevel.High),
                        "1" => (Action.Dodge, RiskLevel.Medium),
                        "2" => (Action.Dodge, RiskLevel.Low),
                        _   => (Action.Panic, default(RiskLevel))
                    };

                    if (action == Action.Panic)
                        View.PromptDontPanic();
                }

                return (action, riskLevel);
            }
        }

        /// <summary>
        /// Gets the player's intention to flee (or not).
        /// </summary>
        /// <returns>
        /// True if the player flees; otherwise, false.
        /// </returns>
        public static bool GetPlayerRunsFromRing()
        {
            View.PromptRunFromRing();

            var playerFlees = GetYesOrNo();
            if (!playerFlees)
                View.ShowPlayerFoolhardy();

            return playerFlees;
        }

        /// <summary>
        /// Gets a yes or no response from the player.
        /// </summary>
        /// <returns>
        /// True if the user answered yes; otherwise, false.
        /// </returns>
        public static bool GetYesOrNo()
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input is null)
                    Environment.Exit(0);

                switch (input.ToUpperInvariant())
                {
                    case "YES":
                        return true;
                    case "NO":
                        return false;
                    default:
                        Console.WriteLine("INCORRECT ANSWER - - PLEASE TYPE 'YES' OR 'NO'.");
                        break;
                }
            }
        }
    }
}
