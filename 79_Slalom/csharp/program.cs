using System.Text;

namespace Slalom
{
    class Slalom
    {
        private int[] GateMaxSpeed = { 14,18,26,29,18,25,28,32,29,20,29,29,25,21,26,29,20,21,20,
                                       18,26,25,33,31,22 };

        private int GoldMedals = 0;
        private int SilverMedals = 0;
        private int BronzeMedals = 0;
        private void DisplayIntro()
        {
            Console.WriteLine("");
            Console.WriteLine("SLALOM".PadLeft(23));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine("");
        }

        private void DisplayInstructions()
        {
            Console.WriteLine();
            Console.WriteLine("*** Slalom: This is the 1976 Winter Olympic Giant Slalom.  You are");
            Console.WriteLine("            the American team's only hope of a gold medal.");
            Console.WriteLine();
            Console.WriteLine("     0 -- Type this if you want to see how long you've taken.");
            Console.WriteLine("     1 -- Type this if you want to speed up a lot.");
            Console.WriteLine("     2 -- Type this if you want to speed up a little.");
            Console.WriteLine("     3 -- Type this if you want to speed up a teensy.");
            Console.WriteLine("     4 -- Type this if you want to keep going the same speed.");
            Console.WriteLine("     5 -- Type this if you want to check a teensy.");
            Console.WriteLine("     6 -- Type this if you want to check a litte.");
            Console.WriteLine("     7 -- Type this if you want to check a lot.");
            Console.WriteLine("     8 -- Type this if you want to cheat and try to skip a gate.");
            Console.WriteLine();
            Console.WriteLine(" The place to use these options is when the computer asks:");
            Console.WriteLine();
            Console.WriteLine("Option?");
            Console.WriteLine();
            Console.WriteLine("               Good Luck!");
            Console.WriteLine();
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
                    Console.WriteLine("Please type 'YES' or 'NO'");
            }

            return false;
        }

        private int PromptForGates()
        {
            bool Success = false;
            int NumberOfGates = 0;

            while (!Success)
            {
                Console.Write("How many gates does this course have (1 to 25) ");
                string LineInput = Console.ReadLine().Trim().ToLower();

                if (int.TryParse(LineInput, out NumberOfGates))
                {
                    if (NumberOfGates >= 1 && NumberOfGates <= 25)
                    {
                        Success = true;
                    }
                    else if (NumberOfGates < 1)
                    {
                        Console.WriteLine("Try again,");
                    }
                    else // greater than 25
                    {
                        Console.WriteLine("25 is the limit.");
                        NumberOfGates = 25;
                        Success = true;
                    }
                }
                else
                {
                    Console.WriteLine("Try again,");
                }
            }

            return NumberOfGates;
        }

        private int PromptForRate()
        {
            bool Success = false;
            int Rating = 0;

            while (!Success)
            {
                Console.Write("Rate yourself as a skier, (1=worst, 3=best) ");
                string LineInput = Console.ReadLine().Trim().ToLower();

                if (int.TryParse(LineInput, out Rating))
                {
                    if (Rating >= 1 && Rating <= 3)
                    {
                        Success = true;
                    }
                    else 
                    {
                        Console.WriteLine("The bounds are 1-3");
                    }
                }
                else
                {
                    Console.WriteLine("The bounds are 1-3");
                }
            }

            return Rating;
        }

        private int PromptForOption()
        {
            bool Success = false;
            int Option = 0;

            while (!Success)
            {
                Console.Write("Option? ");
                string LineInput = Console.ReadLine().Trim().ToLower();

                if (int.TryParse(LineInput, out Option))
                {
                    if (Option >= 0 && Option <= 8)
                    {
                        Success = true;
                    }
                    else if (Option > 8)
                    {
                        Console.WriteLine("What?");
                    }
                }
                else
                {
                    Console.WriteLine("What?");
                }
            }

            return Option;
        }
        
        private string PromptForCommand()
        {
            bool Success = false;
            string Result = "";

            Console.WriteLine();
            Console.WriteLine("Type \"INS\" for intructions");
            Console.WriteLine("Type \"MAX\" for approximate maximum speeds");
            Console.WriteLine("Type \"RUN\" for the beginning of the race");

            while (!Success)
            {

                Console.Write("Command--? ");
                string LineInput = Console.ReadLine().Trim().ToLower();

                if (LineInput.Equals("ins") || LineInput.Equals("max") || LineInput.Equals("run"))
                {
                    Result = LineInput;
                    Success = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("\"{0}\" is an illegal command--retry", LineInput);
                }
            }

            return Result;
        }

        private bool ExceedGateSpeed(double MaxGateSpeed, double MPH, double Time)
        {
            Random rand = new Random();

            Console.WriteLine("{0:N0} M.P.H.", MPH);
            if (MPH > MaxGateSpeed)
            {
                Console.Write("You went over the maximum speed ");
                if (rand.NextDouble() < ((MPH - (double)MaxGateSpeed) * 0.1) + 0.2)
                {
                    Console.WriteLine("and made it!");
                }
                else
                {
                    if (rand.NextDouble() < 0.5) 
                    {
                        Console.WriteLine("snagged a flag!");
                    }
                    else
                    {
                        Console.WriteLine("wiped out!");
                    }

                    Console.WriteLine("You took {0:N2} seconds", rand.NextDouble() + Time);

                    return false;
                }
            }   
            else if (MPH > (MaxGateSpeed - 1))
            {
                Console.WriteLine("Close one!");
            }         

            return true;
        }
        private void DoARun(int NumberOfGates, int Rating)
        {
            Random rand = new Random();
            double MPH = 0;
            double Time = 0;
            int Option = 0;
            double MaxGateSpeed = 0; // Q
            double PreviousMPH = 0;
            double Medals = 0;
            
            Console.WriteLine("The starter counts down...5...4...3...2...1...GO!");

            MPH = rand.NextDouble() * (18-9)+9;

            Console.WriteLine();
            Console.WriteLine("You're off!");

            for (int GateNumber = 1; GateNumber <= NumberOfGates; GateNumber++)
            {
                MaxGateSpeed = GateMaxSpeed[GateNumber-1];

                Console.WriteLine();
                Console.WriteLine("Here comes Gate # {0}:", GateNumber);
                Console.WriteLine("{0:N0} M.P.H.", MPH);

                PreviousMPH = MPH;

                Option = PromptForOption();
                while (Option == 0) 
                {
                    Console.WriteLine("You've taken {0:N2} seconds.", Time);
                    Option = PromptForOption();
                }

                switch (Option)
                {
                    case 1:
                        MPH = MPH + (rand.NextDouble() * (10-5)+5);
                        if (ExceedGateSpeed(MaxGateSpeed, MPH, Time))
                            break;
                        else
                            return;
                    case 2:
                        MPH = MPH + (rand.NextDouble() * (5-3)+3);
                        if (ExceedGateSpeed(MaxGateSpeed, MPH, Time))
                            break;
                        else
                            return;
                    case 3:
                        MPH = MPH + (rand.NextDouble() * (4-1)+1);
                        if (ExceedGateSpeed(MaxGateSpeed, MPH, Time))
                            break;
                        else
                            return;
                    case 4:
                        if (ExceedGateSpeed(MaxGateSpeed, MPH, Time))
                            break;
                        else
                            return;
                    case 5:
                        MPH = MPH - (rand.NextDouble() * (4-1)+1);
                        if (ExceedGateSpeed(MaxGateSpeed, MPH, Time))
                            break;
                        else
                            return;
                    case 6:
                        MPH = MPH - (rand.NextDouble() * (5-3)+3);
                        if (ExceedGateSpeed(MaxGateSpeed, MPH, Time))
                            break;
                        else
                            return;
                    case 7:
                        MPH = MPH - (rand.NextDouble() * (10-5)+5);
                        if (ExceedGateSpeed(MaxGateSpeed, MPH, Time))
                            break;
                        else
                            return;
                    case 8:  // Cheat!
                        Console.WriteLine("***Cheat");
                        if (rand.NextDouble() < 0.7)
                        {
                            Console.WriteLine("An official caught you!");
                            Console.WriteLine("You took {0:N2} seconds.", Time);

                            return;
                        }
                        else
                        {
                            Console.WriteLine("You made it!");
                            Time = Time + 1.5;
                        }
                        break;
                }

                if (MPH < 7)
                {
                    Console.WriteLine("Let's be realistic, OK?  Let's go back and try again...");
                    MPH = PreviousMPH;
                }
                else
                {
                    Time = Time + (MaxGateSpeed - MPH + 1);
                    if (MPH > MaxGateSpeed)
                    {
                        Time = Time + 0.5;

                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("You took {0:N2} seconds.", Time);

            Medals = Time;
            Medals = Medals / NumberOfGates;

            if (Medals < (1.5 - (Rating * 0.1)))
            {
                Console.WriteLine("You won a gold medal!");
                GoldMedals++;
            }
            else if (Medals < (2.9 - (Rating * 0.1)))
            {
                Console.WriteLine("You won a silver medal!");
                SilverMedals++;
            }
            else if (Medals < (4.4 - (Rating * 0.01)))
            {
                Console.WriteLine("You won a bronze medal!");
                BronzeMedals++;
            }
        }

        private void PlayOneRound()
        {
            int NumberOfGates = 0;
            string Command = "first";
            bool KeepPlaying = false;
            int Rating = 0;

            Console.WriteLine("");

            NumberOfGates = PromptForGates();

            while (!Command.Equals(""))
            {
                Command = PromptForCommand();

                // Display instructions
                if (Command.Equals("ins"))
                {
                    DisplayInstructions();
                }
                else if (Command.Equals("max"))
                {
                    Console.WriteLine("Gate Max");
                    Console.WriteLine(" #  M.P.H.");
                    Console.WriteLine("----------");
                    for (int i = 0; i < NumberOfGates; i++)
                    {
                        Console.WriteLine(" {0}     {1}", i+1, GateMaxSpeed[i]);
                    }
                }
                else // do a run!
                {
                    Rating = PromptForRate();

                    do
                    {
                        DoARun(NumberOfGates, Rating);

                        KeepPlaying = PromptYesNo("Do you want to race again? ");
                    }
                    while (KeepPlaying);

                    Console.WriteLine("Thanks for the race");

                    if (GoldMedals > 0)
                        Console.WriteLine("Gold Medals: {0}", GoldMedals);
                    if (SilverMedals > 0)
                        Console.WriteLine("Silver Medals: {0}", SilverMedals);
                    if (BronzeMedals > 0)
                        Console.WriteLine("Bronze Medals: {0}", BronzeMedals);

                    return;
                }
            }
        }

        public void PlayTheGame()
        {
            DisplayIntro();

            PlayOneRound();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            new Slalom().PlayTheGame();

        }
    }
}