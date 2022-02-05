using System;
using System.Linq;

namespace Train
{
    public class TrainGame
    {
        private Random Rnd { get; } = new Random();
        private readonly int ALLOWED_PERCENTAGE_DIFFERENCE = 5;

        static void Main()
        {
            TrainGame train = new TrainGame();
            train.GameLoop();
        }

        public void GameLoop()
        {
            DisplayIntroText();

            do
            {
                PlayGame();
            } while (TryAgain());
        }

        private void PlayGame()
        {
            int carSpeed = (int)GenerateRandomNumber(40, 25);
            int timeDifference = (int)GenerateRandomNumber(5, 15);
            int trainSpeed = (int)GenerateRandomNumber(20, 19);

            Console.WriteLine($"A CAR TRAVELING {carSpeed} MPH CAN MAKE A CERTAIN TRIP IN");
            Console.WriteLine($"{timeDifference} HOURS LESS THAN A TRAIN TRAVELING AT {trainSpeed} MPH");
            Console.WriteLine("HOW LONG DOES THE TRIP TAKE BY CAR?");

            double userInputCarJourneyDuration = double.Parse(Console.ReadLine());
            double actualCarJourneyDuration = CalculateCarJourneyDuration(carSpeed, timeDifference, trainSpeed);
            int percentageDifference = CalculatePercentageDifference(userInputCarJourneyDuration, actualCarJourneyDuration);

            if (IsWithinAllowedDifference(percentageDifference, ALLOWED_PERCENTAGE_DIFFERENCE))
            {
                Console.WriteLine($"GOOD! ANSWER WITHIN {percentageDifference} PERCENT.");
            }
            else
            {
                Console.WriteLine($"SORRY.  YOU WERE OFF BY {percentageDifference} PERCENT.");
            }
            Console.WriteLine($"CORRECT ANSWER IS {actualCarJourneyDuration} HOURS.");
        }

        public static bool IsWithinAllowedDifference(int percentageDifference, int allowedDifference)
        {
            return percentageDifference <= allowedDifference;
        }

        private static int CalculatePercentageDifference(double userInputCarJourneyDuration, double carJourneyDuration)
        {
            return (int)(Math.Abs((carJourneyDuration - userInputCarJourneyDuration) * 100 / userInputCarJourneyDuration) + .5);
        }

        public static double CalculateCarJourneyDuration(double carSpeed, double timeDifference, double trainSpeed)
        {
            return timeDifference * trainSpeed / (carSpeed - trainSpeed);
        }

        public double GenerateRandomNumber(int baseSpeed, int multiplier)
        {
            return multiplier * Rnd.NextDouble() + baseSpeed;
        }

        private bool TryAgain()
        {
            Console.WriteLine("ANOTHER PROBLEM (YES OR NO)? ");
            return IsInputYes(Console.ReadLine());
        }

        public static bool IsInputYes(string consoleInput)
        {
            var options = new string[] { "Y", "YES" };
            return options.Any(o => o.Equals(consoleInput, StringComparison.CurrentCultureIgnoreCase));
        }

        private void DisplayIntroText()
        {
            Console.WriteLine("TRAIN");
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine("TIME - SPEED DISTANCE EXERCISE");
            Console.WriteLine();
        }
    }
}
