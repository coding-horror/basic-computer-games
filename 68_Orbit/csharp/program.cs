using System.Text;

namespace Orbit
{
    class Orbit
    {
        private void DisplayIntro()
        {
            Console.WriteLine();
            Console.WriteLine("ORBIT".PadLeft(23));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("");
            Console.WriteLine("SOMEWHERE ABOVE YOUR PLANET IS A ROMULAN SHIP.");
            Console.WriteLine();
            Console.WriteLine("THE SHIP IS IN A CONSTANT POLAR ORBIT.  ITS");
            Console.WriteLine("DISTANCE FROM THE CENTER OF YOUR PLANET IS FROM");
            Console.WriteLine("10,000 TO 30,000 MILES AND AT ITS PRESENT VELOCITY CAN");
            Console.WriteLine("CIRCLE YOUR PLANET ONCE EVERY 12 TO 36 HOURS.");
            Console.WriteLine();
            Console.WriteLine("UNFORTUNATELY, THEY ARE USING A CLOAKING DEVICE SO");
            Console.WriteLine("YOU ARE UNABLE TO SEE THEM, BUT WITH A SPECIAL");
            Console.WriteLine("INSTRUMENT YOU CAN TELL HOW NEAR THEIR SHIP YOUR");
            Console.WriteLine("PHOTON BOMB EXPLODED.  YOU HAVE SEVEN HOURS UNTIL THEY");
            Console.WriteLine("HAVE BUILT UP SUFFICIENT POWER IN ORDER TO ESCAPE");
            Console.WriteLine("YOUR PLANET'S GRAVITY.");
            Console.WriteLine();
            Console.WriteLine("YOUR PLANET HAS ENOUGH POWER TO FIRE ONE BOMB AN HOUR.");
            Console.WriteLine();
            Console.WriteLine("AT THE BEGINNING OF EACH HOUR YOU WILL BE ASKED TO GIVE AN");
            Console.WriteLine("ANGLE (BETWEEN 0 AND 360) AND A DISTANCE IN UNITS OF");
            Console.WriteLine("100 MILES (BETWEEN 100 AND 300), AFTER WHICH YOUR BOMB'S");
            Console.WriteLine("DISTANCE FROM THE ENEMY SHIP WILL BE GIVEN.");
            Console.WriteLine();
            Console.WriteLine("AN EXPLOSION WITHIN 5,000 MILES OF THE ROMULAN SHIP");
            Console.WriteLine("WILL DESTROY IT.");
            Console.WriteLine();
            Console.WriteLine("BELOW IS A DIAGRAM TO HELP YOU VISUALIZE YOUR PLIGHT.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                          90");
            Console.WriteLine("                    0000000000000");
            Console.WriteLine("                 0000000000000000000");
            Console.WriteLine("               000000           000000");
            Console.WriteLine("             00000                 00000");
            Console.WriteLine("            00000    XXXXXXXXXXX    00000");
            Console.WriteLine("           00000    XXXXXXXXXXXXX    00000");
            Console.WriteLine("          0000     XXXXXXXXXXXXXXX     0000");
            Console.WriteLine("         0000     XXXXXXXXXXXXXXXXX     0000");
            Console.WriteLine("        0000     XXXXXXXXXXXXXXXXXXX     0000");
            Console.WriteLine("180<== 00000     XXXXXXXXXXXXXXXXXXX     00000 ==>0");
            Console.WriteLine("        0000     XXXXXXXXXXXXXXXXXXX     0000");
            Console.WriteLine("         0000     XXXXXXXXXXXXXXXXX     0000");
            Console.WriteLine("          0000     XXXXXXXXXXXXXXX     0000");
            Console.WriteLine("           00000    XXXXXXXXXXXXX    00000");
            Console.WriteLine("            00000    XXXXXXXXXXX    00000");
            Console.WriteLine("             00000                 00000");
            Console.WriteLine("               000000           000000");
            Console.WriteLine("                 0000000000000000000");
            Console.WriteLine("                    0000000000000");
            Console.WriteLine("                         270");
            Console.WriteLine();
            Console.WriteLine("X - YOUR PLANET");
            Console.WriteLine("O - THE ORBIT OF THE ROMULAN SHIP");
            Console.WriteLine();
            Console.WriteLine("ON THE ABOVE DIAGRAM, THE ROMULAN SHIP IS CIRCLING");
            Console.WriteLine("COUNTERCLOCKWISE AROUND YOUR PLANET.  DON'T FORGET THAT");
            Console.WriteLine("WITHOUT SUFFICIENT POWER THE ROMULAN SHIP'S ALTITUDE");
            Console.WriteLine("AND ORBITAL RATE WILL REMAIN CONSTANT.");
            Console.WriteLine();
            Console.WriteLine("GOOD LUCK.  THE FEDERATION IS COUNTING ON YOU.");
       }

        private bool PromptYesNo(string Prompt)
        {
            bool Success = false;

            while (!Success)
            {
                Console.Write(Prompt);
                string LineInput = Console.ReadLine().Trim().ToLower();

                if (LineInput.Equals("yes"))
                    return true;
                else if (LineInput.Equals("no"))
                    return false;
                else
                    Console.WriteLine("Yes or No");
            }

            return false;
        }

        private int PromptForNumber(string Prompt)
        {
            bool InputSuccess = false;
            int ReturnResult = 0;

            while (!InputSuccess)
            {
                Console.Write(Prompt);
                string Input = Console.ReadLine().Trim();
                InputSuccess = int.TryParse(Input, out ReturnResult);
                if (!InputSuccess)
                    Console.WriteLine("*** Please enter a valid number ***");
            }   

            return ReturnResult;
        }

        private void PlayOneRound()
        {
            Random rand = new Random();
            string Prompt = "";

            int A_AngleToShip = 0;
            int D_DistanceFromBombToShip = 0;
            int R_DistanceToShip = 0;
            int H_Hour = 0;
            int A1_Angle = 0;
            int D1_DistanceForDetonation = 0;
            int T = 0;
            double C_ExplosionDistance = 0;

            A_AngleToShip = Convert.ToInt32(360 * rand.NextDouble());
            D_DistanceFromBombToShip = Convert.ToInt32(200 * rand.NextDouble()) + 200;
            R_DistanceToShip = Convert.ToInt32(20 * rand.NextDouble()) + 10;

            while (H_Hour < 7)
            {
                H_Hour++;

                Console.WriteLine();
                Console.WriteLine();
                Prompt = "This is hour " + H_Hour.ToString() + ", at what angle do you wish to send\nyour photon bomb? ";
                A1_Angle = PromptForNumber(Prompt);

                D1_DistanceForDetonation = PromptForNumber("How far out do you wish to detonate it? ");

                Console.WriteLine();
                Console.WriteLine();

                A_AngleToShip += R_DistanceToShip;
                if (A_AngleToShip >= 360)
                    A_AngleToShip -= 360;

                T = Math.Abs(A_AngleToShip = A1_Angle);
                if (T >= 180)
                    T = 360 - T;

                C_ExplosionDistance = Math.Sqrt(D_DistanceFromBombToShip * D_DistanceFromBombToShip + D1_DistanceForDetonation * 
                                                D1_DistanceForDetonation - 2 * D_DistanceFromBombToShip * D1_DistanceForDetonation * 
                                                Math.Cos(T * 3.14159 / 180));
                
                Console.WriteLine("Your photon bomb exploded {0:N3}*10^2 miles from the", C_ExplosionDistance);
                Console.WriteLine("Romulan ship.");

                if (C_ExplosionDistance <= 50)
                {
                    Console.WriteLine("You have successfully completed your mission.");
                    return;
                }
            }

            Console.WriteLine("You allowed the Romulans to escape.");
            return;
 
        }

        public void Play()
        {
            bool ContinuePlay = true;

            DisplayIntro();

            do 
            {
                PlayOneRound();

                Console.WriteLine("Another Romulan ship has gone in to orbit.");
                ContinuePlay = PromptYesNo("Do you wish to try to destroy it? ");
            }
            while (ContinuePlay);
            
            Console.WriteLine("Good bye.");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            new Orbit().Play();

        }
    }
}