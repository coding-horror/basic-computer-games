using System;

namespace DepthCharge
{
    class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();

            View.ShowBanner();

            var dimension = Controller.InputDimension();
            var maximumGuesses = CalculateMaximumGuesses();

            View.ShowInstructions(maximumGuesses);

            do
            {
                View.ShowStartGame();

                var submarineCoordinates = PlaceSubmarine();
                var trailNumber = 1;
                var guess = (0, 0, 0);

                do
                {
                    guess = Controller.InputCoordinates(trailNumber);
                    if (guess != submarineCoordinates)
                        View.ShowGuessPlacement(submarineCoordinates, guess);
                }
                while (guess != submarineCoordinates && trailNumber++ < maximumGuesses);

                View.ShowGameResult(submarineCoordinates, guess, trailNumber);
            }
            while (Controller.InputPlayAgain());

            View.ShowFarewell();

            int CalculateMaximumGuesses() =>
                (int)Math.Log2(dimension) + 1;

            (int x, int y, int depth) PlaceSubmarine() =>
                (random.Next(dimension), random.Next(dimension), random.Next(dimension));
        }
    }
}
