namespace Gunner
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintIntro();

            string keepPlaying = "Y";

            while (keepPlaying == "Y") {
                PlayGame();
                Console.WriteLine("TRY AGAIN (Y OR N)");
                keepPlaying = Console.ReadLine();
            }
        }

        static void PlayGame()
        {
            int totalAttempts = 0;
            int amountOfGames = 0;

            while (amountOfGames < 4) {

                int maximumRange = new Random().Next(0, 40000) + 20000;
                Console.WriteLine($"MAXIMUM RANGE OF YOUR GUN IS {maximumRange} YARDS." + Environment.NewLine + Environment.NewLine + Environment.NewLine);

                int distanceToTarget = (int) (maximumRange * (0.1 + 0.8 * new Random().NextDouble()));
                Console.WriteLine($"DISTANCE TO THE TARGET IS {distanceToTarget} YARDS.");

                (bool gameWon, int attempts) = HitTheTarget(maximumRange, distanceToTarget);

                if(!gameWon) {
                    Console.WriteLine(Environment.NewLine + "BOOM !!!!   YOU HAVE JUST BEEN DESTROYED" + Environment.NewLine +
                        "BY THE ENEMY." + Environment.NewLine + Environment.NewLine + Environment.NewLine
                    );
                    PrintReturnToBase();
                    break;
                } else {
                    amountOfGames += 1;
                    totalAttempts += attempts;

                    Console.WriteLine($"TOTAL ROUNDS EXPENDED WERE:{totalAttempts}");

                    if (amountOfGames < 4) {
                        Console.WriteLine("THE FORWARD OBSERVER HAS SIGHTED MORE ENEMY ACTIVITY...");
                    } else {
                        if (totalAttempts > 18) {
                            PrintReturnToBase();
                        } else {
                            Console.WriteLine($"NICE SHOOTING !!");
                        }
                    }
                }
            }
        }

        static (bool, int) HitTheTarget(int maximumRange, int distanceToTarget)
        {
            int attempts = 0;

            while (attempts < 6)
            {
                int elevation = GetElevation();

                int differenceBetweenTargetAndImpact = CalculateDifferenceBetweenTargetAndImpact(maximumRange, distanceToTarget, elevation);

                if (Math.Abs(differenceBetweenTargetAndImpact) < 100)
                {
                    Console.WriteLine($"*** TARGET DESTROYED *** {attempts} ROUNDS OF AMMUNITION EXPENDED.");
                    return (true, attempts);
                }
                else if (differenceBetweenTargetAndImpact > 100)
                {
                    Console.WriteLine($"OVER TARGET BY {Math.Abs(differenceBetweenTargetAndImpact)} YARDS.");
                }
                else
                {
                    Console.WriteLine($"SHORT OF TARGET BY {Math.Abs(differenceBetweenTargetAndImpact)} YARDS.");
                }

                attempts += 1;
            }
            return (false, attempts);
        }

        static int CalculateDifferenceBetweenTargetAndImpact(int maximumRange, int distanceToTarget, int elevation)
        {
            double weirdNumber = 2 * elevation / 57.3;
            double distanceShot = maximumRange * Math.Sin(weirdNumber);
            return (int)distanceShot - distanceToTarget;
        }

        static void PrintReturnToBase()
        {
            Console.WriteLine("BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!");
        }

        static int GetElevation()
        {
            Console.WriteLine("ELEVATION");
            int elevation = int.Parse(Console.ReadLine());
            if (elevation > 89) {
                Console.WriteLine("MAXIMUM ELEVATION IS 89 DEGREES");
                return GetElevation();
            }
            if (elevation < 1) {
                Console.WriteLine("MINIMUM ELEVATION IS 1 DEGREE");
                return GetElevation();
            }
            return elevation;
        }

        static void PrintIntro()
        {
            Console.WriteLine(new String(' ', 30) + "GUNNER");
            Console.WriteLine(new String(' ', 15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
            Console.WriteLine("YOU ARE THE OFFICER-IN-CHARGE, GIVING ORDERS TO A GUN");
            Console.WriteLine("CREW, TELLING THEM THE DEGREES OF ELEVATION YOU ESTIMATE");
            Console.WriteLine("WILL PLACE A PROJECTILE ON TARGET.  A HIT WITHIN 100 YARDS");
            Console.WriteLine("OF THE TARGET WILL DESTROY IT." + Environment.NewLine);
        }
    }
}
