using System.Text;

namespace Synonym
{
    class Synonym
    {
        Random rand = new Random();

        // Initialize list of corrent responses
        private string[] Affirmations = { "Right", "Correct", "Fine", "Good!", "Check" };

        // Initialize list of words and their synonyms
        private string[][] Words =
        {
                new string[] {"first", "start", "beginning", "onset", "initial"},
                new string[] {"similar", "alike", "same", "like", "resembling"},
                new string[] {"model", "pattern", "prototype", "standard", "criterion"},
                new string[] {"small", "insignificant", "little", "tiny", "minute"},
                new string[] {"stop", "halt", "stay", "arrest", "check", "standstill"},
                new string[] {"house", "dwelling", "residence", "domicile", "lodging", "habitation"},
                new string[] {"pit", "hole", "hollow", "well", "gulf", "chasm", "abyss"},
                new string[] {"push", "shove", "thrust", "prod", "poke", "butt", "press"},
                new string[] {"red", "rouge", "scarlet", "crimson", "flame", "ruby"},
                new string[] {"pain", "suffering", "hurt", "misery", "distress", "ache", "discomfort"}
         };

        private void DisplayIntro()
        {
            Console.WriteLine("");
            Console.WriteLine("SYNONYM".PadLeft(23));
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine("");
            Console.WriteLine("A synonym of a word means another word in the English");
            Console.WriteLine("language which has the same or very nearly the same meaning.");
            Console.WriteLine("I choose a word -- you type a synonym.");
            Console.WriteLine("If you can't think of a synonym, type the word 'help'");
            Console.WriteLine("and I will tell you a synonym.");
            Console.WriteLine("");
        }

        private void DisplayOutro()
        {
            Console.WriteLine("Synonym drill completed.");
        }

        private void RandomizeTheList()
        {
            // Randomize the list of Words to pick from
            int[] Order = new int[Words.Length];
            foreach (int i in Order)
            {
                Order[i] = rand.Next();
            }
            Array.Sort(Order, Words);
        }

        private string GetAnAffirmation()
        {
            return Affirmations[rand.Next(Affirmations.Length)];
        }

        private bool CheckTheResponse(string WordName, int WordIndex, string LineInput, string[] WordList)
        {
            if (LineInput.Equals("help"))
            {
                // Choose a random correct synonym response that doesn't equal the current word given
                int HelpIndex = rand.Next(WordList.Length);
                while (HelpIndex == WordIndex)
                {
                    HelpIndex = rand.Next(0, WordList.Length);
                }
                Console.WriteLine("**** A synonym of {0} is {1}.", WordName, WordList[HelpIndex]);

                return false;
            }
            else
            {
                // Check to see if the response is one of the listed synonyms and not the current word prompt
                if (WordList.Contains(LineInput) && LineInput != WordName)
                {
                    // Randomly display one of the five correct answer exclamations
                    Console.WriteLine(GetAnAffirmation());
                    
                    return true;
                }
                else
                {
                    // Incorrect response.  Try again.
                    Console.WriteLine("     Try again.".PadLeft(5));

                    return false;
                }
            }
        }

        private string PromptForSynonym(string WordName)
        {
            Console.Write("     What is a synonym of {0}? ", WordName);
            string LineInput = Console.ReadLine().Trim().ToLower();

            return LineInput;
        }

        private void AskForSynonyms()
        {
            Random rand = new Random();

            // Loop through the now randomized list of Words and display a random word from each to prompt for a synonym
            foreach (string[] WordList in Words)
            {
                int WordIndex = rand.Next(WordList.Length);  // random word position in the current list of words
                string WordName = WordList[WordIndex];       // what is that actual word 
                bool Success = false;

                while (!Success)
                {
                    // Ask for the synonym of the current word
                    string LineInput = PromptForSynonym(WordName);

                    // Check the response 
                    Success = CheckTheResponse(WordName, WordIndex, LineInput, WordList);

                    // Add extra line space for formatting
                    Console.WriteLine("");
                }
            }
        }

        public void PlayTheGame()
        {
            RandomizeTheList();

            DisplayIntro();

            AskForSynonyms();

            DisplayOutro();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            new Synonym().PlayTheGame();

        }
    }
}