using System;

namespace hurkle
{
    internal class HurkleGame
    {
        private readonly Random _random = new Random();
        private readonly IHurkleView _view;
        private readonly int guesses;
        private readonly int gridSize;

        public HurkleGame(int guesses, int gridSize, IHurkleView view)
        {
            _view = view;
            this.guesses = guesses;
            this.gridSize = gridSize;
        }

        public void PlayGame()
        {
            // BASIC program was generating a float between 0 and 1
            // then multiplying by the size of the grid to to a number
            // between 1 and 10. C# allows you to do that directly.
            var hurklePoint = new GamePoint{
                X = _random.Next(0, gridSize),
                Y = _random.Next(0, gridSize)
            };
            
            for(var K=1;K<=guesses;K++)
            {
                var guessPoint = _view.GetGuess(new GuessViewModel{CurrentGuessNumber = K});

                var direction = guessPoint.GetDirectionTo(hurklePoint);
                switch(direction)
                {
                    case CardinalDirection.None:
                        _view.ShowVictory(new VictoryViewModel{CurrentGuessNumber = K});
                        return;
                    default:
                        _view.ShowDirection(new FailedGuessViewModel{Direction = direction});
                        continue;
                }
            }
            
            _view.ShowLoss(new LossViewModel{MaxGuesses = guesses, HurkleLocation = hurklePoint } );
        }
    }
}