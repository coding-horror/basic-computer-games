using System;

namespace Game
{
    /// <summary>
    /// Contains functions for displaying information to the user.
    /// </summary>
    public static class View
    {
        public static void ShowBanner()
        {
            Console.WriteLine("                                 COMBAT");
            Console.WriteLine("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        }

        public static void ShowInstructions()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("I AM AT WAR WITH YOU.");
            Console.WriteLine("WE HAVE 72000 SOLDIERS APIECE.");
        }

        public static void ShowDistributeForces()
        {
            Console.WriteLine();
            Console.WriteLine("DISTRIBUTE YOUR FORCES.");
            Console.WriteLine("\tME\t  YOU");
        }

        public static void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void ShowResult(WarState finalState)
        {
            if (!finalState.IsAbsoluteVictory)
            {
                Console.WriteLine();
                Console.WriteLine("FROM THE RESULTS OF BOTH OF YOUR ATTACKS,");
            }

            switch (finalState.FinalOutcome)
            {
            case WarResult.ComputerVictory:
                Console.WriteLine("YOU LOST-I CONQUERED YOUR COUNTRY.  IT SERVES YOU");
                Console.WriteLine("RIGHT FOR PLAYING THIS STUPID GAME!!!");
                break;
            case WarResult.PlayerVictory:
                Console.WriteLine("YOU WON, OH! SHUCKS!!!!");
                break;
            case WarResult.PeaceTreaty:
                Console.WriteLine("THE TREATY OF PARIS CONCLUDED THAT WE TAKE OUR");
                Console.WriteLine("RESPECTIVE COUNTRIES AND LIVE IN PEACE.");
                break;
            }
        }

        public static void PromptArmySize(int computerArmySize)
        {
            Console.Write($"ARMY\t{computerArmySize}\t? ");
        }

        public static void PromptNavySize(int computerNavySize)
        {
            Console.Write($"NAVY\t{computerNavySize}\t? ");
        }

        public static void PromptAirForceSize(int computerAirForceSize)
        {
            Console.Write($"A. F.\t{computerAirForceSize}\t? ");
        }

        public static void PromptFirstAttackBranch()
        {
            Console.WriteLine("YOU ATTACK FIRST. TYPE (1) FOR ARMY; (2) FOR NAVY;");
            Console.WriteLine("AND (3) FOR AIR FORCE.");
            Console.Write("? ");
        }

        public static void PromptNextAttackBranch(ArmedForces computerForces, ArmedForces playerForces)
        {
            // BUG: More of a nit-pick really, but the order of columns in the
            //  table is reversed from what we showed when distributing troops.
            //  The tables should be consistent.
            Console.WriteLine();
            Console.WriteLine("\tYOU\tME");
            Console.WriteLine($"ARMY\t{playerForces.Army}\t{computerForces.Army}");
            Console.WriteLine($"NAVY\t{playerForces.Navy}\t{computerForces.Navy}");
            Console.WriteLine($"A. F.\t{playerForces.AirForce}\t{computerForces.AirForce}");

            Console.WriteLine("WHAT IS YOUR NEXT MOVE?");
            Console.WriteLine("ARMY=1  NAVY=2  AIR FORCE=3");
            Console.Write("? ");
        }

        public static void PromptAttackSize()
        {
            Console.WriteLine("HOW MANY MEN");
            Console.Write("? ");
        }

        public static void PromptValidInteger()
        {
            Console.WriteLine("ENTER A VALID INTEGER VALUE");
        }
    }
}
