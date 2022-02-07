using System.Collections;
using System.Text;

namespace Splat
{
    class Splat
    {
        private ArrayList DistanceLog = new ArrayList();

        private string[][] AccelerationData =
        {
            new string[] {"Fine. You're on Mercury. Acceleration={0} ft/sec/sec", "12.2"},
            new string[] {"All right.  You're on Venus. Acceleration={0} ft/sec/sec", "28.3"},
            new string[] {"Then you're on Earth. Acceleration={0} ft/sec/sec", "32.16"},
            new string[] {"Fine. You're on the Moon. Acceleration={0} ft/sec/sec", "5.15"},
            new string[] {"All right. You're on Mars. Acceleration={0} ft/sec/sec", "12.5"},
            new string[] {"Then you're on Jupiter. Acceleration={0} ft/sec/sec", "85.2"},
            new string[] {"Fine. You're on Saturn. Acceleration={0} ft/sec/sec", "37.6"},
            new string[] {"All right. You're on Uranus. Acceleration={0} ft/sec/sec", "33.8"},
            new string[] {"Then you're on Neptune. Acceleration={0} ft/sec/sec", "39.6"},
            new string[] {"Fine. You're on the Sun. Acceleration={0} ft/sec/sec", "896"}
        };

        private void DisplayIntro()
        {
            Console.WriteLine("");
            Console.WriteLine("SPLAT".PadLeft(23));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine("");
            Console.WriteLine("Welcome to 'Splat' -- the game that simulates a parachute");
            Console.WriteLine("jump.  Try to open your chute at the last possible");
            Console.WriteLine("moment without going splat.");
            Console.WriteLine("");
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

        private void WriteRandomBadResult()
        {
           string[] BadResults = {"Requiescat in pace.","May the Angel of Heaven lead you into paradise.",
                "Rest in peace.","Son-of-a-gun.","#$%&&%!$","A kick in the pants is a boost if you're headed right.",
                "Hmmm. Should have picked a shorter time.","Mutter. Mutter. Mutter.","Pushing up daisies.",
                "Easy come, easy go."};

            Random rand = new Random();

            Console.WriteLine(BadResults[rand.Next(BadResults.Length)]);
        }

        private void WriteColumnOutput(double Column1, double Column2)
        {

            Console.WriteLine("{0,-11:N3}    {1,-17:N2}", Column1, Column2);

        }

        private void WriteColumnOutput(double Column1, string Column2)
        {

            Console.WriteLine("{0,-11:N3}    {1,-17}", Column1, Column2);

        }

        private void WriteColumnOutput(string Column1, string Column2)
        {

            Console.WriteLine("{0,-11}    {1,-17}", Column1, Column2);

        }

        private void WriteSuccessfulResults(double Distance)
        {
            // Add new result
            DistanceLog.Add(Distance);

            // Sort by distance
            DistanceLog.Sort();

            int ArrayLength = DistanceLog.Count;

            // If 1st, 2nd, or 3rd jump then write a special message
            if (ArrayLength <= 3)
            {
                Console.Write("Amazing!!! Not bad for your ");
                if (ArrayLength == 1)
                    Console.Write("1st ");
                else if (ArrayLength == 2)
                    Console.Write("2nd ");
                else
                    Console.Write("3rd ");
                Console.WriteLine("successful jump!!!");
            }
            // Otherwise write a message based on where this jump falls in the list
            else
            {
                int JumpPosition = DistanceLog.IndexOf(Distance);
                

                if (ArrayLength - JumpPosition <= .1 * ArrayLength)
                {
                    Console.WriteLine("Wow! That's some jumping. Of the {0} successful jumps", ArrayLength);
                    Console.WriteLine("before yours, only {0} opened their chutes lower than", (ArrayLength - JumpPosition));
                    Console.WriteLine("you did.");
                }
                else if (ArrayLength - JumpPosition <= .25 * ArrayLength)
                {
                    Console.WriteLine("Pretty good! {0} successful jumps preceded yours and only", ArrayLength - 1);
                    Console.WriteLine("{0} of them got lower than you did before their chutes", (ArrayLength - 1 - JumpPosition));
                    Console.WriteLine("opened.");
                }
                else if (ArrayLength - JumpPosition <= .5 * ArrayLength)
                {
                    Console.WriteLine("Not bad. There have been  {0} successful jumps before yours.", ArrayLength - 1);
                    Console.WriteLine("You were beaten out by {0} of them.", (ArrayLength - 1 - JumpPosition));
                }
                else if (ArrayLength - JumpPosition <= .75 * ArrayLength)
                {
                    Console.WriteLine("Conservative aren't you? You ranked only {0} in the", (ArrayLength - JumpPosition));
                    Console.WriteLine("{0} successful jumps before yours.", ArrayLength - 1);
                }
                else if (ArrayLength - JumpPosition <= .9 * ArrayLength)
                {
                    Console.WriteLine("Humph! Don't you have any sporting blood? There were");
                    Console.WriteLine("{0} successful jumps before yours and you came in {1} jumps", ArrayLength - 1, JumpPosition);
                    Console.WriteLine("better than the worst. Shape up!!!");
                }
                else
                {
                    Console.WriteLine("Hey! You pulled the rip cord much too soon. {0} successful", ArrayLength - 1);
                    Console.WriteLine("jumps before yours and you came in number {0}! Get with it!", (ArrayLength - JumpPosition));
                }
            }

        }

        private void PlayOneRound()
        {
            bool InputSuccess = false;
            Random rand = new Random();
            double Velocity = 0;                                    
            double TerminalVelocity = 0;                            
            double Acceleration = 0;                                
            double AccelerationInput = 0;                           
            double Altitude = ((9001 * rand.NextDouble()) + 1000);  
            double SecondsTimer = 0;                                
            double Distance = 0;                                      
            bool TerminalVelocityReached = false;

            Console.WriteLine("");

            // Determine the terminal velocity (user or system)
            if (PromptYesNo("Select your own terminal velocity (yes or no)? "))
            {
                // Prompt user to enter the terminal velocity of their choice
                while (!InputSuccess)
                {
                    Console.Write("What terminal velocity (mi/hr)? ");
                    string Input = Console.ReadLine().Trim();
                    InputSuccess = double.TryParse(Input, out TerminalVelocity);
                    if (!InputSuccess)
                        Console.WriteLine("*** Please enter a valid number ***");
                 }   
            }
            else
            {
                TerminalVelocity = rand.NextDouble() * 1000;
                Console.WriteLine("OK.  Terminal Velocity = {0:N0} mi/hr", (TerminalVelocity));
            }

            // Convert Terminal Velocity to ft/sec
            TerminalVelocity = TerminalVelocity * 5280 / 3600;

            // Not sure what this calculation is
            Velocity = TerminalVelocity + ((TerminalVelocity * rand.NextDouble()) / 20) - ((TerminalVelocity * rand.NextDouble()) / 20);

            // Determine acceleration due to gravity (user or system)
            if (PromptYesNo("Want to select acceleration due to gravity (yes or no)? "))
            {
                 // Prompt user to enter the acceleration of their choice
                InputSuccess = false;
                while (!InputSuccess)
                {
                    Console.Write("What acceleration (ft/sec/sec)? ");
                    string Input = Console.ReadLine().Trim();
                    InputSuccess = double.TryParse(Input, out AccelerationInput);
                    if (!InputSuccess)
                        Console.WriteLine("*** Please enter a valid number ***");
                 }   
            }
            else
            {
                // Choose a random acceleration entry from the data array
                int Index = rand.Next(0, AccelerationData.Length);
                Double.TryParse(AccelerationData[Index][1], out AccelerationInput);

                // Display the corresponding planet this acceleration exists on and the value
                Console.WriteLine(AccelerationData[Index][0], AccelerationInput.ToString());
            }

            Acceleration = AccelerationInput + ((AccelerationInput * rand.NextDouble()) / 20) - ((AccelerationInput * rand.NextDouble()) / 20);

            Console.WriteLine("");
            Console.WriteLine("    Altitude         = {0:N0} ft", Altitude);
            Console.WriteLine("    Term. Velocity   = {0:N3} ft/sec +/-5%", TerminalVelocity);
            Console.WriteLine("    Acceleration     = {0:N2} ft/sec/sec +/-5%", AccelerationInput);
            Console.WriteLine("Set the timer for your freefall.");

            // Prompt for how many seconds the fall should be before opening the chute
            InputSuccess = false;
            while (!InputSuccess)
            {
                Console.Write("How many seconds? ");
                string Input = Console.ReadLine().Trim();
                InputSuccess = double.TryParse(Input, out SecondsTimer);
                if (!InputSuccess)
                    Console.WriteLine("*** Please enter a valid number ***");
            }   

            // Begin the drop!
            Console.WriteLine("Here we go.");
            Console.WriteLine("");

            WriteColumnOutput("Time (sec)", "Dist to Fall (ft)");
            WriteColumnOutput("==========", "=================");
            
            // Loop through the number of seconds stepping by 8 intervals
            for (double i = 0; i < SecondsTimer; i+=(SecondsTimer/8)) 
            {
                if (i > (Velocity / Acceleration))
                {
                    // Terminal Velocity achieved.  Only print out the warning once.
                    if (TerminalVelocityReached == false)
                        Console.WriteLine("Terminal velocity reached at T plus {0:N4} seconds.", (Velocity / Acceleration));

                    TerminalVelocityReached = true;
                }

                // Calculate distance dependent upon whether terminal velocity has been reached
                if (TerminalVelocityReached)
                {
                    Distance = Altitude - ((Math.Pow(Velocity,2) / (2 * Acceleration)) + (Velocity * (i - (Velocity / Acceleration))));
                }
                else
                {
                    Distance = Altitude - ((Acceleration / 2) * Math.Pow(i,2));
                }

                // Was the ground hit?  If so, then SPLAT!
                if (Distance <= 0)
                {
                    if (TerminalVelocityReached)
                    {
                        WriteColumnOutput((Velocity / Acceleration) + ((Altitude - (Math.Pow(Velocity,2) / (2 * Acceleration))) / Velocity).ToString(), "SPLAT");
                    }
                    else
                    {
                        WriteColumnOutput(Math.Sqrt(2 * Altitude / Acceleration), "SPLAT");
                    }

                    WriteRandomBadResult();

                    Console.WriteLine("I'll give you another chance.");
                    break;
                }
                else
                {
                    WriteColumnOutput(i, Distance);
                }
            }

            // If the number of seconds of drop ended and we are still above ground then success!
            if (Distance > 0)
            {
                // We made it!  Chutes open!
                Console.WriteLine("Chute Open");

                // Store succesful jump and write out a fun message
                WriteSuccessfulResults(Distance);
            }    

        }

        public void PlayTheGame()
        {
            bool ContinuePlay = false;
            
            DisplayIntro();

            do 
            {
                PlayOneRound();

                ContinuePlay = PromptYesNo("Do you want to play again? ");
                if (!ContinuePlay)
                    ContinuePlay = PromptYesNo("Please? ");
            }
            while (ContinuePlay);

            Console.WriteLine("SSSSSSSSSS.");

        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            new Splat().PlayTheGame();

        }
    }
}