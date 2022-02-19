using System.Text;

namespace Number
{
    class Number
    {
        private void DisplayIntro()
        {
            Console.WriteLine();
            Console.WriteLine("NUMBER".PadLeft(23));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("You have 100 points.  By guessing numbers from 1 to 5, you");
            Console.WriteLine("can gain or lose points depending upon how close you get to");
            Console.WriteLine("a random number selected by the computer.");
            Console.WriteLine();
            Console.WriteLine("You occaisionally will get a jackpot which will double(!)");
            Console.WriteLine("your point count.  You win when you get 500 points.");
            Console.WriteLine();

        }
        private int PromptForGuess()
        {
            bool Success = false;
            int Guess = 0;

            while (!Success)
            {
                Console.Write("Guess a number from 1 to 5? ");
                string LineInput = Console.ReadLine().Trim().ToLower();

                if (int.TryParse(LineInput, out Guess))
                {
                    if (Guess >= 0 && Guess <= 5)
                        Success = true;
                }
                else
                    Console.WriteLine("Please enter a number between 1 and 5.");
            }

            return Guess;
        }

        private void GetRandomNumbers(out int Random1, out int Random2, out int Random3, out int Random4, out int Random5)
        {
            Random rand = new Random();

            // Get a unique set of random numbers between 1 and 5 
            // I assume this is what the original BASIC  FNR(X)=INT(5*RND(1)+1) is doing
            Random1 = (int)(5 * rand.NextDouble() + 1);
            do
            {
                Random2 = (int)(5 * rand.NextDouble() + 1);
            } while (Random2 == Random1);
            do
            {
                Random3 = (int)(5 * rand.NextDouble() + 1);
            } while (Random3 == Random1 || Random3 == Random2);
            do
            {
                Random4 = (int)(5 * rand.NextDouble() + 1);
            } while (Random4 == Random1 || Random4 == Random2 || Random4 == Random3);
            do
            {
                Random5 = (int)(5 * rand.NextDouble() + 1);
            } while (Random5 == Random1 || Random5 == Random2 || Random5 == Random3 || Random5 == Random4);

        }
        private void Play()
        {

            int Points = 100;
            bool Win = false;
            int Random1, Random2, Random3, Random4, Random5;
            int Guess = 0;

            GetRandomNumbers(out Random1, out Random2, out Random3, out Random4, out Random5);
        
            while (!Win)
            {
                
                Guess = PromptForGuess();

                if (Guess == Random1)
                    Points -= 5;
                else if (Guess == Random2)
                    Points += 5;
                else if (Guess == Random3)
                {
                    Points += Points;
                    Console.WriteLine("You hit the jackpot!!!");
                }
                else if (Guess == Random4)
                    Points += 1;
                else if (Guess == Random5)
                    Points -= (int)(Points * 0.5);
                
                if (Points > 500)
                {
                    Console.WriteLine("!!!!You Win!!!! with {0} points.", Points);
                    Win = true;
                }
                else
                    Console.WriteLine("You have {0} points.", Points);
            }
        }

        public void PlayTheGame()
        {
            DisplayIntro();

            Play();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            new Number().PlayTheGame();

        }
    }
}