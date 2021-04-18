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
        private Dice dice1 = new Dice();
        private Dice dice2 = new Dice();

        public CrapsGame(ref UserInterface ui)
        {
            this.ui = ui;
        }

        public Result Play(out int diceRoll)
        {
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
                        diceRoll = newRoll;
                        return Result.pointWin;
                    }
                    else if (newRoll == 7)
                    {
                        diceRoll = newRoll;
                        return Result.pointLoss;
                    }

                    ui.NoPoint(newRoll);
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
