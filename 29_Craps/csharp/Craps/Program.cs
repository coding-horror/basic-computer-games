using System.Diagnostics;



namespace Craps
{
    class Program
    {
        static void Main(string[] args)
        {
            var ui = new UserInterface();
            var game = new CrapsGame(ref ui);
            int winnings = 0;

            ui.Intro();

            do 
            {
	            var bet = ui.PlaceBet();
                var result = game.Play(out int diceRoll);

                switch (result)
                {
                    case Result.naturalWin:
                        winnings += bet;
                        break;

                    case Result.naturalLoss:
                    case Result.snakeEyesLoss:
                    case Result.pointLoss:
                        winnings -= bet;
                        break;

                    case Result.pointWin:
                        winnings += (2 * bet);
                        break;

                    // Include a default so that we will be warned if the values of the enum
                    // ever change and we forget to add code to handle the new value.
                    default:
                        Debug.Assert(false); // We should never get here.
                        break;
                }

                ui.ShowResult(result, diceRoll, bet);
            } while (ui.PlayAgain(winnings));

            ui.GoodBye(winnings);
        }
    }
}
