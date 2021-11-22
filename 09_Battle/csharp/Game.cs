using System;
using System.Linq;

namespace Battle
{
    public class Game
    {
        private int[,] field = new int[7, 7];

        private Random random = new Random();

        public void Run()
        {
            DisplayIntro();

            while (true)
            {
                field = new int[7, 7];

                foreach (var shipType in new []{ 1, 2, 3})
                {
                    foreach (var ship in new int[] { 1, 2 })
                    {
                        while (!SetShip(shipType, ship)) { }
                    }
                }

                UserInteraction();
            }
        }

        private bool SetShip(int shipType, int shipNum)
        {
            var shipSize = 4 - shipType;
            int direction;
            int[] A = new int[5];
            int[] B = new int[5];
            int row, col;

            do
            {
                row = Rnd(6) + 1;
                col = Rnd(6) + 1;
                direction = Rnd(4) + 1;
            } while (field[row, col] > 0);

            var M = 0;

            switch (direction)
            {
                case 1:
                    B[1] = col;
                    B[2] = 7;
                    B[3] = 7;

                    for (var K = 1; K <= shipSize; K++)
                    {
                        if (!(M > 1 || B[K] == 6 || field[row, B[K] + 1] > 0))
                        {
                            B[K + 1] = B[K] + 1;
                            continue;
                        }

                        M = 2;
                        var Z = 1;

                        if (B[1] < B[2] && B[1] < B[3]) Z = B[1];
                        if (B[2] < B[1] && B[2] < B[3]) Z = B[2];
                        if (B[3] < B[1] && B[3] < B[2]) Z = B[3];

                        if (Z == 1 || field[row, Z - 1] > 0) return false;

                        B[K + 1] = Z - 1;
                    }

                    field[row, col] = 9 - 2 * shipType - shipNum;

                    for (var K = 1; K <= shipSize; K++)
                    {
                        field[row, B[K + 1]] = field[row, col];
                    }
                    break;

                case 2:
                    A[1] = row;
                    B[1] = col;
                    A[2] = 0;
                    A[3] = 0;
                    B[2] = 0;
                    B[3] = 0;

                    for (var K = 1; K <= shipSize; K++)
                    {
                        if (!(M > 1
                            || A[K] == 1 || B[K] == 1
                            || field[A[K] - 1, B[K] - 1] > 0
                            || (field[A[K] - 1, B[K]] > 0 && field[A[K] - 1, B[K]] == field[A[K], B[K] - 1])))
                        {
                            A[K + 1] = A[K] - 1;
                            B[K + 1] = B[K] - 1;
                            continue;
                        }

                        M = 2;
                        var Z1 = 1;
                        var Z2 = 1;

                        if (A[1] > A[2] && A[1] > A[3]) Z1 = A[1];
                        if (A[2] > A[1] && A[2] > A[3]) Z1 = A[2];
                        if (A[3] > A[1] && A[3] > A[2]) Z1 = A[3];
                        if (B[1] > B[2] && B[1] > B[3]) Z2 = B[1];
                        if (B[2] > B[1] && B[2] > B[3]) Z2 = B[2];
                        if (B[3] > B[1] && B[3] > B[2]) Z2 = B[3];

                        if (Z1 == 6 || Z2 == 6
                            || field[Z1 + 1, Z2 + 1] > 0
                            || (field[Z1, Z2 + 1] > 0 && field[Z1, Z2 + 1] == field[Z1 + 1, Z2])) return false;

                        A[K + 1] = Z1 + 1;
                        B[K + 1] = Z2 + 1;
                    }

                    field[row, col] = 9 - 2 * shipType - shipNum;

                    for (var K = 1; K <= shipSize; K++)
                    {
                        field[A[K + 1], B[K + 1]] = field[row, col];
                    }
                    break;

                case 3:
                    A[1] = row;
                    A[2] = 7;
                    A[3] = 7;

                    for (var K = 1; K <= shipSize; K++)
                    {
                        if (!(M > 1 || A[K] == 6
                            || field[A[K] + 1, col] > 0))
                        {
                            A[K + 1] = A[K] + 1;
                            continue;
                        }

                        M = 2;
                        var Z = 1;

                        if (A[1] < A[2] && A[1] < A[3]) Z = A[1];
                        if (A[2] < A[1] && A[2] < A[3]) Z = A[2];
                        if (A[3] < A[1] && A[3] < A[2]) Z = A[3];

                        if (Z == 1 || field[Z - 1, col] > 0) return false;

                        A[K + 1] = Z - 1;
                    }

                    field[row, col] = 9 - 2 * shipType - shipNum;

                    for (var K = 1; K <= shipSize; K++)
                    {
                        field[A[K + 1], col] = field[row, col];
                    }
                    break;

                case 4:
                default:
                    A[1] = row;
                    B[1] = col;
                    A[2] = 7;
                    A[3] = 7;
                    B[2] = 0;
                    B[3] = 0;

                    for (var K = 1; K <= shipSize; K++)
                    {
                        if (!(M > 1 || A[K] == 6 || B[K] == 1
                            || field[A[K] + 1, B[K] - 1] > 0
                            || (field[A[K] + 1, B[K]] > 0 && field[A[K] + 1, B[K]] == field[A[K], B[K] - 1])))
                        {
                            A[K + 1] = A[K] + 1;
                            B[K + 1] = B[K] - 1;
                            continue;
                        }

                        M = 2;
                        var Z1 = 1;
                        var Z2 = 1;

                        if (A[1] < A[2] && A[1] < A[3]) Z1 = A[1];
                        if (A[2] < A[1] && A[2] < A[3]) Z1 = A[2];
                        if (A[3] < A[1] && A[3] < A[2]) Z1 = A[3];
                        if (B[1] > B[2] && B[1] > B[3]) Z2 = B[1];
                        if (B[2] > B[1] && B[2] > B[3]) Z2 = B[2];
                        if (B[3] > B[1] && B[3] > B[2]) Z2 = B[3];

                        if (Z1 == 1 || Z2 == 6
                            || field[Z1 - 1, Z2 + 1] > 0
                            || (field[Z1, Z2 + 1] > 0 && field[Z1, Z2 + 1] == field[Z1 - 1, Z2])) return false;

                        A[K + 1] = Z1 - 1;
                        B[K + 1] = Z2 + 1;
                    }

                    field[row, col] = 9 - 2 * shipType - shipNum;

                    for (var K = 1; K <= shipSize; K++)
                    {
                        field[A[K + 1], B[K + 1]] = field[row, col];
                    }

                    break;
            }

            return true;
        }

        public void DisplayIntro()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Print(Tab(33) + "BATTLE");
            Print(Tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            //-- BATTLE WRITTEN BY RAY WESTERGARD  10 / 70
            // COPYRIGHT 1971 BY THE REGENTS OF THE UNIV.OF CALIF.
            // PRODUCED AT THE LAWRENCE HALL OF SCIENCE, BERKELEY
        }

        public void UserInteraction()
        {
            Print();
            Print("THE FOLLOWING CODE OF THE BAD GUYS' FLEET DISPOSITION");
            Print("HAS BEEN CAPTURED BUT NOT DECODED:");
            Print();

            for (var row = 1; row <= 6; row++)
            {
                for (var col = 1; col <= 6; col++)
                {
                    Write(field[col, row].ToString());
                }

                Print();
            }

            Print();
            Print("DE-CODE IT AND USE IT IF YOU CAN");
            Print("BUT KEEP THE DE-CODING METHOD A SECRET.");
            Print();

            var hit = new int[7, 7];
            var lost = new int[4];
            var shipHits = new[] { 0, 2, 2, 1, 1, 0, 0 };
            var splashes = 0;
            var hits = 0;

            Print("START GAME");

            do
            {
                var input = Console.ReadLine().Split(',').Select(x => int.TryParse(x, out var num) ? num : 0).ToArray();

                if (!IsValid(input))
                {
                    Print("INVALID INPUT.  TRY AGAIN.");
                    continue;
                }

                var col = input[0];
                var row = 7 - input[1];
                var shipNum = field[row, col];

                if (shipNum == 0)
                {
                    splashes = splashes + 1;
                    Print("SPLASH!  TRY AGAIN.");
                    continue;
                }

                if (shipHits[shipNum] > 3)
                {
                    Print("THERE USED TO BE A SHIP AT THAT POINT, BUT YOU SUNK IT.");
                    Print("SPLASH!  TRY AGAIN.");
                    splashes = splashes + 1;
                    continue;
                }

                if (hit[row, col] > 0)
                {
                    Print($"YOU ALREADY PUT A HOLE IN SHIP NUMBER {shipNum} AT THAT POINT.");
                    Print("SPLASH!  TRY AGAIN.");
                    splashes = splashes + 1;
                    continue;
                }

                hits = hits + 1;
                hit[row, col] = shipNum;

                Print($"A DIRECT HIT ON SHIP NUMBER {shipNum}");
                shipHits[shipNum] = shipHits[shipNum] + 1;

                if (shipHits[shipNum] < 4)
                {
                    Print("TRY AGAIN.");
                    continue;
                }

                var shipType = (shipNum - 1) / 2 + 1;
                lost[shipType] = lost[shipType] + 1;
    
                Print("AND YOU SUNK IT.  HURRAH FOR THE GOOD GUYS.");
                Print("SO FAR, THE BAD GUYS HAVE LOST");
                Write($"{lost[1]} DESTROYER(S), {lost[2]} CRUISER(S), AND ");
                Print($"{lost[3]} AIRCRAFT CARRIER(S).");
                Print($"YOUR CURRENT SPLASH/HIT RATIO IS {splashes / hits}");

                if ((lost[1] + lost[2] + lost[3]) < 6) continue;

                Print();
                Print("YOU HAVE TOTALLY WIPED OUT THE BAD GUYS' FLEET");
                Print($"WITH A FINAL SPLASH/HIT RATIO OF {splashes / hits}");

                if ((splashes / hits) == 0)
                {
                    Print("CONGRATULATIONS -- A DIRECT HIT EVERY TIME.");
                }

                Print();
                Print("****************************");
                Print();

                return;

            } while (true);
        }

        public bool IsValid(int[] input) => input.Length == 2 && input.All(Valid);

        public bool Valid(int value) => value > 0 && value < 7;

        public void Print(string str = "") => Console.WriteLine(str);

        public void Write(string value) => Console.Write(value);

        public string Tab(int pos) => new String(' ', pos);

        public int Rnd(int seed) => random.Next(seed);
    }
}
