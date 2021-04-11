using System;



namespace Craps
{
    class Program
    {
        enum Result
        {
            naturalWin,
            snakeEyesLoss,
            loss,
            pointLoss,
            pointWin,
        };

        static void Main(string[] args)
        {
            var dice1 = new Dice();
            var dice2 = new Dice();
            var ui = new UserInterface();

            int winnings = 0;

            ui.Intro();

            do 
            {
	            var bet = ui.PlaceBet();
                var diceRoll = dice1.Roll() + dice2.Roll();
                bool isWinner = false;

                if (Win(diceRoll))
                {
                    winnings += bet;
                    isWinner = true;
                }
                else if (Lose(diceRoll))
                {
                    winnings -= bet;
                    isWinner = false;
                }
                else
                {
	                var point = diceRoll;
                    ui.Point(point);

                    while (true)
                    {
                        var newRoll = dice1.Roll() + dice2.Roll();
                        if (newRoll == point)
                        {
                            winnings += bet;
                            isWinner = true;
                            break;
                        }
                        else if (newRoll == 7)
                        {
                            winnings -= bet;
                            isWinner = false;
                            break;
                        }

                        ui.RollAgain();
                    }
                }

                ui.ShowResult(isWinner, bet);
            } while (ui.PlayAgain(winnings));
        }

        private static bool Lose(int diceRoll)
        {
            throw new NotImplementedException();
        }

        private static bool Win(int diceRoll)
        {
            throw new NotImplementedException();
        }
    }
}
