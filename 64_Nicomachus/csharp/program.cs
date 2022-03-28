using System.Text;
using System.Threading;

namespace Nicomachus
{
    class Nicomachus
    {
        private void DisplayIntro()
        {
            Console.WriteLine();
            Console.WriteLine("NICOMA".PadLeft(23));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Boomerang puzzle from Arithmetica of Nicomachus -- A.D. 90!");
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
                    Console.WriteLine("Eh?  I don't understand '{0}'  Try 'Yes' or 'No'.", LineInput);
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
            int A_Number = 0;
            int B_Number = 0;
            int C_Number = 0;
            int D_Number = 0;

            Console.WriteLine();
            Console.WriteLine("Please think of a number between 1 and 100.");

            A_Number = PromptForNumber("Your number divided by 3 has a remainder of? ");
            B_Number = PromptForNumber("Your number divided by 5 has a remainder of? ");
            C_Number = PromptForNumber("Your number divided by 7 has a remainder of? ");

            Console.WriteLine();
            Console.WriteLine("Let me think a moment...");

            Thread.Sleep(2000);

            D_Number = 70 * A_Number + 21 * B_Number + 15 * C_Number;

            while (D_Number > 105)
            {
                D_Number -= 105;
            }

            if (PromptYesNo("Your number was " + D_Number.ToString() + ", right? "))
            {
                Console.WriteLine();
                Console.WriteLine("How about that!!");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("I feel your arithmetic is in error.");
            }

            Console.WriteLine();

       }

        public void Play()
        {
            bool ContinuePlay = true;

            DisplayIntro();

            do 
            {
                PlayOneRound();

                ContinuePlay = PromptYesNo("Let's try another? ");
            }
            while (ContinuePlay);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            new Nicomachus().Play();

        }
    }
}