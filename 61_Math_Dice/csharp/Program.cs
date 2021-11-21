using System;

namespace MathDice
{
    public static class Program
    {
        readonly static Random random = new Random();

        static int DieOne = 0;
        static int DieTwo = 0;

        private const string NoPips = "I     I";
        private const string LeftPip = "I *   I";
        private const string CentrePip = "I  *  I";
        private const string RightPip = "I   * I";
        private const string TwoPips = "I * * I";
        private const string Edge = " ----- ";

        static void Main(string[] args)
        {
            int answer;

            GameState gameState = GameState.FirstAttempt;

            Console.WriteLine("MATH DICE".CentreAlign());
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".CentreAlign());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("THIS PROGRAM GENERATES SUCCESSIVE PICTURES OF TWO DICE.");
            Console.WriteLine("WHEN TWO DICE AND AN EQUAL SIGN FOLLOWED BY A QUESTION");
            Console.WriteLine("MARK HAVE BEEN PRINTED, TYPE YOUR ANSWER AND THE RETURN KEY.");
            Console.WriteLine("TO CONCLUDE THE LESSON, TYPE CONTROL-C AS YOUR ANSWER.");
            Console.WriteLine();
            Console.WriteLine();

            while (true)
            {
                if (gameState == GameState.FirstAttempt)
                {
                    Roll(ref DieOne);
                    Roll(ref DieTwo);

                    DrawDie(DieOne);
                    Console.WriteLine("   +");
                    DrawDie(DieTwo);
                }

                answer = GetAnswer();

                if (answer == DieOne + DieTwo)
                {
                    Console.WriteLine("RIGHT!");
                    Console.WriteLine();
                    Console.WriteLine("THE DICE ROLL AGAIN...");

                    gameState = GameState.FirstAttempt;
                }
                else
                {
                    if (gameState == GameState.FirstAttempt)
                    {
                        Console.WriteLine("NO, COUNT THE SPOTS AND GIVE ANOTHER ANSWER.");
                        gameState = GameState.SecondAttempt;
                    }
                    else
                    {
                        Console.WriteLine($"NO, THE ANSWER IS{DieOne + DieTwo}");
                        Console.WriteLine();
                        Console.WriteLine("THE DICE ROLL AGAIN...");
                        gameState = GameState.FirstAttempt;
                    }
                }
            }
        }

        private static int GetAnswer()
        {
            int answer;

            Console.Write("      =?");
            var input = Console.ReadLine();

            int.TryParse(input, out answer);

            return answer;
        }

        private static void DrawDie(int pips)
        {
            Console.WriteLine(Edge);
            Console.WriteLine(OuterRow(pips, true));
            Console.WriteLine(CentreRow(pips));
            Console.WriteLine(OuterRow(pips, false));
            Console.WriteLine(Edge);
            Console.WriteLine();
        }

        private static void Roll(ref int die) => die = random.Next(1, 7);

        private static string OuterRow(int pips, bool top)
        {
            return pips switch
            {
                1 => NoPips,
                var x when x == 2 || x == 3 => top ? LeftPip : RightPip,
                _ => TwoPips
            };
        }

        private static string CentreRow(int pips)
        {
            return pips switch
            {
                var x when x == 2 || x == 4 => NoPips,
                6 => TwoPips,
                _ => CentrePip
            };
        }
    }
}
