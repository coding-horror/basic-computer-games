using System;
using System.Threading;

namespace _23matches
{
    class Program
    {
        /// <summary>
        /// Mimics the "goto" version of the original program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Random random = new Random();
        StartNewGame:
            Console.WriteLine("23 MATCHES".PadLeft(31));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".PadLeft(15));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("THIS IS A GAME CALLED '23 MATCHES'.");
            Console.WriteLine();
            Console.WriteLine("WHEN IT IS YOUR TURN, YOU MAY TAKE ONE, TWO, OR THREE");
            Console.WriteLine("MATCHES. THE OBJECT OF THE GAME IS NOT TO HAVE TO TAKE");
            Console.WriteLine("THE LAST MATCH.");
            Console.WriteLine();
            Console.WriteLine("Input exit to close the program.");
            Console.WriteLine("Input cls Screen Clear.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("LET'S FLIP A COIN TO SEE WHO GOES FIRST.");
            Console.WriteLine("IF IT COMES UP HEADS, I WILL WIN THE TOSS.");
            Console.WriteLine();
        StartTheGame:
            string command;
            int N = 23;
            int K = 0;
            int Q = random.Next(2);
            if (Q == 1)
                goto ComputerFirst;
            else
                goto PlayerFirst;

            ComputerFirst:
            Console.WriteLine("HEADS! I WIN! HA! HA!");
            Console.WriteLine("PREPARE TO LOSE, MEATBALL-NOSE!!");
            Console.WriteLine();
            int ain = random.Next(1, 3);
            Console.WriteLine($"I TAKE {ain} MATCHES");
            N = N - ain;
            goto PlayersProceed;

        PlayerFirst:
            Console.WriteLine("TAILS! YOU GO FIRST. ");
            Console.WriteLine();
            goto PlayersSpeak;

        PlayersProceed:
            Console.WriteLine($"THE NUMBER OF MATCHES IS NOW {N}");
            Console.WriteLine();
            Console.WriteLine("YOUR TURN -- YOU MAY TAKE 1, 2 OR 3 MATCHES.");
            Console.WriteLine("HOW MANY DO YOU WISH TO REMOVE ");
            goto PlayersSpeak;

        PlayersSpeak:
            command = Console.ReadLine().ToLower();
            if (command.Equals("exit"))
            {
                System.Diagnostics.Process tt = System.Diagnostics.Process.GetProcessById(System.Diagnostics.Process.GetCurrentProcess().Id);
                tt.Kill();
            }
            if (command.Equals("cls"))
            {
                Console.Clear();
                goto PlayersProceed;
            }
            try
            {
                K = Convert.ToInt32(command);
            }
            catch (System.Exception)
            {
                goto PlayerInputError;
            }
            if (K > 3 || K <= 0)
                goto PlayerInputError;
            N = N - K;
            Console.WriteLine($"THERE ARE NOW {N} MATCHES REMAINING.");
            if (N == 4 || N == 3 || N == 2)
                goto TheComputerSpeaks;
            else if (N <= 1)
                goto ThePlayerWins;
            else
                goto TheComputerSpeaks;

            TheComputerSpeaks:
            int Z = 4 - K;
            Console.WriteLine($"MY TURN ! I REMOVE {Z} MATCHES");
            N = N - Z;
            if (N <= 1)
                goto TheComputerWins;
            else
                goto PlayersProceed;

            PlayerInputError:
            Console.WriteLine("VERY FUNNY! DUMMY!");
            Console.WriteLine("DO YOU WANT TO PLAY OR GOOF AROUND?");
            Console.WriteLine("NOW, HOW MANY MATCHES DO YOU WANT ");
            goto PlayersSpeak;
        ThePlayerWins:
            Console.WriteLine("YOU WON, FLOPPY EARS !");
            Console.WriteLine("THINK YOU'RE PRETTY SMART !");
            Console.WriteLine("LETS PLAY AGAIN AND I'LL BLOW YOUR SHOES OFF !!");
            Console.WriteLine();
            Console.WriteLine();
            goto StartTheGame;
        TheComputerWins:
            Console.WriteLine();
            Console.WriteLine("YOU POOR BOOB! YOU TOOK THE LAST MATCH! I GOTCHA!!");
            Console.WriteLine("HA ! HA ! I BEAT YOU !!!");
            Console.WriteLine();
            Console.WriteLine("GOOD BYE LOSER!");
            Console.WriteLine();
            Console.WriteLine();
            goto StartNewGame;

        }
    }
}
