using System;

namespace Game
{
    /// <summary>
    /// Contains functions for displaying information to the user.
    /// </summary>
    public static class View
    {
        private static readonly string[] QualityString = { "SUPERB", "GOOD", "FAIR", "POOR", "AWFUL" };

        public static void ShowBanner()
        {
            Console.WriteLine("                                  BULL");
            Console.WriteLine("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void ShowInstructions()
        {
            Console.WriteLine("HELLO, ALL YOU BLOODLOVERS AND AFICIONADOS.");
            Console.WriteLine("HERE IS YOUR BIG CHANCE TO KILL A BULL.");
            Console.WriteLine();
            Console.WriteLine("ON EACH PASS OF THE BULL, YOU MAY TRY");
            Console.WriteLine("0 - VERONICA (DANGEROUS INSIDE MOVE OF THE CAPE)");
            Console.WriteLine("1 - LESS DANGEROUS OUTSIDE MOVE OF THE CAPE");
            Console.WriteLine("2 - ORDINARY SWIRL OF THE CAPE.");
            Console.WriteLine();
            Console.WriteLine("INSTEAD OF THE ABOVE, YOU MAY TRY TO KILL THE BULL");
            Console.WriteLine("ON ANY TURN: 4 (OVER THE HORNS), 5 (IN THE CHEST).");
            Console.WriteLine("BUT IF I WERE YOU,");
            Console.WriteLine("I WOULDN'T TRY IT BEFORE THE SEVENTH PASS.");
            Console.WriteLine();
            Console.WriteLine("THE CROWD WILL DETERMINE WHAT AWARD YOU DESERVE");
            Console.WriteLine("(POSTHUMOUSLY IF NECESSARY).");
            Console.WriteLine("THE BRAVER YOU ARE, THE BETTER THE AWARD YOU RECEIVE.");
            Console.WriteLine();
            Console.WriteLine("THE BETTER THE JOB THE PICADORES AND TOREADORES DO,");
            Console.WriteLine("THE BETTER YOUR CHANCES ARE.");
        }

        public static void ShowSeparator()
        {
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void ShowStartingConditions(Events.MatchStarted matchStarted)
        {
            ShowBullQuality();
            ShowHelpQuality("TOREADORES", matchStarted.ToreadorePerformance, matchStarted.ToreadoresKilled, 0);
            ShowHelpQuality("PICADORES", matchStarted.PicadorePerformance, matchStarted.PicadoresKilled, matchStarted.HorsesKilled);

            void ShowBullQuality()
            {
                Console.WriteLine($"YOU HAVE DRAWN A {QualityString[(int)matchStarted.BullQuality - 1]} BULL.");

                if (matchStarted.BullQuality > Quality.Poor)
                {
                    Console.WriteLine("YOU'RE LUCKY");
                }
                else
                if (matchStarted.BullQuality < Quality.Good)
                {
                    Console.WriteLine("GOOD LUCK.  YOU'LL NEED IT.");
                    Console.WriteLine();
                }

                Console.WriteLine();
            }

            static void ShowHelpQuality(string helperName, Quality helpQuality, int helpersKilled, int horsesKilled)
            {
                Console.WriteLine($"THE {helperName} DID A {QualityString[(int)helpQuality - 1]} JOB.");

                // NOTE: The code below makes some *strong* assumptions about
                //  how the casualty numbers were generated.  It is written
                //  this way to preserve the behaviour of the original BASIC
                //  version, but it would make more sense ignore the helpQuality
                //  parameter and just use the provided numbers to decide what
                //  to display.
                switch (helpQuality)
                {
                    case Quality.Poor:
                        if (horsesKilled > 0)
                            Console.WriteLine($"ONE OF THE HORSES OF THE {helperName} WAS KILLED.");

                        if (helpersKilled > 0)
                            Console.WriteLine($"ONE OF THE {helperName} WAS KILLED.");
                        else
                            Console.WriteLine($"NO {helperName} WERE KILLED.");
                        break;

                    case Quality.Awful:
                        if (horsesKilled > 0)
                            Console.WriteLine($" {horsesKilled} OF THE HORSES OF THE {helperName} KILLED.");

                        Console.WriteLine($" {helpersKilled} OF THE {helperName} KILLED.");
                        break;
                }
            }
        }

        public static void ShowStartOfPass(int passNumber)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"PASS NUMBER {passNumber}");
        }

        public static void ShowPlayerGored(bool playerPanicked, bool firstGoring)
        {
            Console.WriteLine((playerPanicked, firstGoring) switch
            {
                (true,  true) => "YOU PANICKED.  THE BULL GORED YOU.",
                (false, true) => "THE BULL HAS GORED YOU!",
                (_, false)    => "YOU ARE GORED AGAIN!"
            });
        }

        public static void ShowPlayerSurvives()
        {
            Console.WriteLine("YOU ARE STILL ALIVE.");
            Console.WriteLine();
        }

        public static void ShowPlayerFoolhardy()
        {
            Console.WriteLine("YOU ARE BRAVE.  STUPID, BUT BRAVE.");
        }

        public static void ShowFinalResult(ActionResult result, bool extremeBravery, Reward reward)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            switch (result)
            {
                case ActionResult.PlayerFlees:
                    Console.WriteLine("COWARD");
                    break;
                case ActionResult.BullKillsPlayer:
                    Console.WriteLine("YOU ARE DEAD.");
                    break;
                case ActionResult.PlayerKillsBull:
                    Console.WriteLine("YOU KILLED THE BULL!");
                    break;
            }

            if (result == ActionResult.PlayerFlees)
            {
                Console.WriteLine("THE CROWD BOOS FOR TEN MINUTES.  IF YOU EVER DARE TO SHOW");
                Console.WriteLine("YOUR FACE IN A RING AGAIN, THEY SWEAR THEY WILL KILL YOU--");
                Console.WriteLine("UNLESS THE BULL DOES FIRST.");
            }
            else
            {
                if (extremeBravery)
                    Console.WriteLine("THE CROWD CHEERS WILDLY!");
                else
                if (result == ActionResult.PlayerKillsBull)
                {
                    Console.WriteLine("THE CROWD CHEERS!");
                    Console.WriteLine();
                }

                Console.WriteLine("THE CROWD AWARDS YOU");
                switch (reward)
                {
                    case Reward.Nothing:
                        Console.WriteLine("NOTHING AT ALL.");
                        break;
                    case Reward.OneEar:
                        Console.WriteLine("ONE EAR OF THE BULL.");
                        break;
                    case Reward.TwoEars:
                        Console.WriteLine("BOTH EARS OF THE BULL!");
                        Console.WriteLine("OLE!");
                        break;
                    default:
                        Console.WriteLine("OLE!  YOU ARE 'MUY HOMBRE'!! OLE!  OLE!");
                        break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("ADIOS");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void PromptShowInstructions()
        {
            Console.Write("DO YOU WANT INSTRUCTIONS? ");
        }

        public static void PromptKillBull()
        {
            Console.WriteLine("THE BULL IS CHARGING AT YOU!  YOU ARE THE MATADOR--");
            Console.Write("DO YOU WANT TO KILL THE BULL? ");
        }

        public static void PromptKillBullBrief()
        {
            Console.Write("HERE COMES THE BULL.  TRY FOR A KILL? ");
        }

        public static void PromptKillMethod()
        {
            Console.WriteLine();
            Console.WriteLine("IT IS THE MOMENT OF TRUTH.");
            Console.WriteLine();

            Console.Write("HOW DO YOU TRY TO KILL THE BULL? ");
        }

        public static void PromptCapeMove()
        {
            Console.Write("WHAT MOVE DO YOU MAKE WITH THE CAPE? ");
        }

        public static void PromptCapeMoveBrief()
        {
            Console.Write("CAPE MOVE? ");
        }

        public static void PromptDontPanic()
        {
            Console.WriteLine("DON'T PANIC, YOU IDIOT!  PUT DOWN A CORRECT NUMBER");
            Console.Write("? ");
        }

        public static void PromptRunFromRing()
        {
            Console.Write("DO YOU RUN FROM THE RING? ");
        }
    }
}
