namespace Craps
{
    public enum Result
    {
        // It's not used in this program but it's often a good idea to include a "none"
        // value in an enum so that you can set an instance of the enum to "invalid" or
        // initialise it to "none of the valid values".
        noResult,
        naturalWin,
        snakeEyesLoss,
        naturalLoss,
        pointLoss,
        pointWin,
    };

    class CrapsGame
    {
        private readonly UserInterface ui;

        public CrapsGame(ref UserInterface ui)
        {
            this.ui = ui;
        }

        public Result Play(out int diceRoll)
        {

            var dice1 = new Dice();
            var dice2 = new Dice();
            diceRoll = dice1.Roll() + dice2.Roll();

            if (Win(diceRoll))
            {
                return Result.naturalWin;
            }
            else if (Lose(diceRoll))
            {
                return (diceRoll == 2) ? Result.snakeEyesLoss : Result.naturalLoss;
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
                        return Result.pointWin;
                    }
                    else if (newRoll == 7)
                    {
                        return Result.pointLoss;
                    }

                    ui.NoPoint(diceRoll);
                }
            }
        }

        private bool Lose(int diceRoll)
        {
            return diceRoll == 2 || diceRoll == 3 || diceRoll == 12;
        }

        private bool Win(int diceRoll)
        {
            return diceRoll == 7 || diceRoll == 11;
        }
    }
}
