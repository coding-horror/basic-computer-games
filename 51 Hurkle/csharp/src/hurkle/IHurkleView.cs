namespace hurkle
{
    internal interface IHurkleView
    {
        GamePoint GetGuess(GuessViewModel guessViewModel);
        void ShowVictory(VictoryViewModel victoryViewModel);
        void ShowDirection(FailedGuessViewModel failedGuessViewModel);
        void ShowLoss(LossViewModel lossViewModel);
    }
}