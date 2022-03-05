using System;
using System.Collections.Generic;
using System.Linq;

namespace banner
{
    class Banner
    {
        private int Horizontal { get; set; }
        private int Vertical { get; set; }
        private bool Centered { get; set; }
        private string Character { get; set; }
        private string Statement { get; set; }

        // This provides a bit-ended representation of each symbol
        // that can be output.  Each symbol is defined by 7 parts -
        // where each part is an integer value that, when converted to
        // the binary representation, shows which section is filled in
        // with values and which are spaces.  i.e., the 'filled in'
        // parts represent the actual symbol on the paper.
        Dictionary<char, int[]> letters = new Dictionary<char, int[]>()
        {
            {' ', new int[] { 0, 0, 0, 0, 0, 0, 0 } },
            {'A', new int[] {505, 37, 35, 34, 35, 37, 505} },
            {'B', new int[] {512, 274, 274, 274, 274, 274, 239} },
            {'C', new int[] {125, 131, 258, 258, 258, 131, 69} },
            {'D', new int[] {512, 258, 258, 258, 258, 131, 125} },
            {'E', new int[] {512, 274, 274, 274, 274, 258, 258} },
            {'F', new int[] {512, 18, 18, 18, 18, 2, 2} },
            {'G', new int[] {125, 131, 258, 258, 290, 163, 101} },
            {'H', new int[] {512, 17, 17, 17, 17, 17, 512} },
            {'I', new int[] {258, 258, 258, 512, 258, 258, 258} },
            {'J', new int[] {65, 129, 257, 257, 257, 129, 128} },
            {'K', new int[] {512, 17, 17, 41, 69, 131, 258} },
            {'L', new int[] {512, 257, 257, 257, 257, 257, 257} },
            {'M', new int[] {512, 7, 13, 25, 13, 7, 512} },
            {'N', new int[] {512, 7, 9, 17, 33, 193, 512} },
            {'O', new int[] {125, 131, 258, 258, 258, 131, 125} },
            {'P', new int[] {512, 18, 18, 18, 18, 18, 15} },
            {'Q', new int[] {125, 131, 258, 258, 322, 131, 381} },
            {'R', new int[] {512, 18, 18, 50, 82, 146, 271} },
            {'S', new int[] {69, 139, 274, 274, 274, 163, 69} },
            {'T', new int[] {2, 2, 2, 512, 2, 2, 2} },
            {'U', new int[] {128, 129, 257, 257, 257, 129, 128} },
            {'V', new int[] {64, 65, 129, 257, 129, 65, 64} },
            {'W', new int[] {256, 257, 129, 65, 129, 257, 256} },
            {'X', new int[] {388, 69, 41, 17, 41, 69, 388} },
            {'Y', new int[] {8, 9, 17, 481, 17, 9, 8} },
            {'Z', new int[] {386, 322, 290, 274, 266, 262, 260} },
            {'0', new int[] {57, 69, 131, 258, 131, 69, 57} },
            {'1', new int[] {0, 0, 261, 259, 512, 257, 257} },
            {'2', new int[] {261, 387, 322, 290, 274, 267, 261} },
            {'3', new int[] {66, 130, 258, 274, 266, 150, 100} },
            {'4', new int[] {33, 49, 41, 37, 35, 512, 33} },
            {'5', new int[] {160, 274, 274, 274, 274, 274, 226} },
            {'6', new int[] {194, 291, 293, 297, 305, 289, 193} },
            {'7', new int[] {258, 130, 66, 34, 18, 10, 8} },
            {'8', new int[] {69, 171, 274, 274, 274, 171, 69} },
            {'9', new int[] {263, 138, 74, 42, 26, 10, 7} },
            {'?', new int[] {5, 3, 2, 354, 18, 11, 5} },
            {'*', new int[] {69, 41, 17, 512, 17, 41, 69} },
            {'=', new int[] {41, 41, 41, 41, 41, 41, 41} },
            {'!', new int[] {1, 1, 1, 384, 1, 1, 1} },
            {'.', new int[] {1, 1, 129, 449, 129, 1, 1} }
        };


        /// <summary>
        /// This displays the provided text on the screen and then waits for the user
        /// to enter a integer value greater than 0.
        /// </summary>
        /// <param name="DisplayText">Text to display on the screen asking for the input</param>
        /// <returns>The integer value entered by the user</returns>
        private int GetNumber(string DisplayText)
        {
            Console.Write(DisplayText);
            string TempStr = Console.ReadLine();

            Int32.TryParse(TempStr, out int TempInt);

            if (TempInt <= 0)
            {
                throw new ArgumentException($"{DisplayText} must be greater than zero");
            }

            return TempInt;
        }

        /// <summary>
        /// This displays the provided text on the screen and then waits for the user
        /// to enter a Y or N.  It cheats by just looking for a 'y' and returning that
        /// as true.  Anything else that the user enters is returned as false.
        /// </summary>
        /// <param name="DisplayText">Text to display on the screen asking for the input</param>
        /// <returns>Returns true or false</returns>
        private bool GetBool(string DisplayText)
        {
            Console.Write(DisplayText);
            return (Console.ReadLine().StartsWith("y", StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// This displays the provided text on the screen and then waits for the user
        /// to enter an arbitrary string.  That string is then returned 'as-is'.
        /// </summary>
        /// <param name="DisplayText">Text to display on the screen asking for the input</param>
        /// <returns>The string entered by the user.</returns>
        private string GetString(string DisplayText)
        {
            Console.Write(DisplayText);
            return (Console.ReadLine().ToUpper());
        }

        /// <summary>
        /// This queries the user for the various inputs needed by the program.
        /// </summary>
        private void GetInput()
        {
            Horizontal = GetNumber("Horizontal ");
            Vertical = GetNumber("Vertical ");
            Centered = GetBool("Centered ");
            Character = GetString("Character (type 'ALL' if you want character being printed) ");
            Statement = GetString("Statement ");
            // We don't care about what the user enters here.  This is just telling them
            // to set the page in the printer.
            _ = GetString("Set page ");
        }

        /// <summary>
        /// This prints out a single character of the banner - adding
        /// a few blanks lines as a spacer between characters.
        /// </summary>
        private void PrintChar(char ch)
        {
            // In the trivial case (a space character), just print out the spaces
            if (ch.Equals(' '))
            {
                Console.WriteLine(new string('\n', 7 * Horizontal));
                return;
            }

            // If a specific character to be printed was provided by the user,
            // then user that as our ouput character - otherwise take the
            // current character
            char outCh = Character == "ALL" ? ch : Character[0];
            int[] letter = new int[7];
            try
            {
                letters[outCh].CopyTo(letter, 0);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"The provided letter {outCh} was not found in the letters list");
            }

            // This iterates through each of the parts that make up
            // each letter.  Each part represents 1 * Horizontal lines
            // of actual output.
            for (int idx = 0; idx < 7; idx++)
            {
                // New int array declarations default to zeros
                // numSections decides how many 'sections' need to be printed
                // for a given line of each character
                int[] numSections = new int[7];
                // fillInSection decides whether each 'section' of the
                // character gets filled in with the character or with blanks
                int[] fillInSection = new int[9];

                // This uses the value in each part to decide which
                // sections are empty spaces in the letter or filled in
                // spaces.  For each section marked with 1 in fillInSection,
                // that will correspond to 1 * Vertical characters actually
                // being output.
                for (int exp = 8; exp >= 0; exp--)
                {
                    if (Math.Pow(2, exp) < letter[idx])
                    {
                        fillInSection[8 - exp] = 1;
                        letter[idx] -= (int)Math.Pow(2, exp);
                        if (letter[idx] == 1)
                        {
                            // Once we've exhausted all of the sections
                            // defined in this part of the letter, then
                            // we marked that number and break out of this
                            // for loop.
                            numSections[idx] = 8 - exp;
                            break;
                        }
                    }
                }

                // Now that we know which sections of this part of the letter
                // are filled in or spaces, we can actually create the string
                // to print out.
                string lineStr = "";

                if (Centered)
                    lineStr += new string(' ', (int)(63 - 4.5 * Vertical) * 1 / 1 + 1);

                for (int idx2 = 0; idx2 <= numSections[idx]; idx2++)
                {
                    lineStr = lineStr + new string(fillInSection[idx2] == 0 ? ' ' : outCh, Vertical);
                }

                // Then we print that string out 1 * Horizontal number of times
                for (int lineidx = 1; lineidx <= Horizontal; lineidx++)
                {
                    Console.WriteLine(lineStr);
                }
            }

            // Finally, add a little spacer after each character for readability.
            Console.WriteLine(new string('\n', 2 * Horizontal - 1));
        }

        /// <summary>
        /// This prints the entire banner based in the parameters
        /// the user provided.
        /// </summary>
        private void PrintBanner()
        {
            // Iterate through each character in the statement
            foreach (char ch in Statement)
            {
                PrintChar(ch);
            }

            // In the original version, it would print an additional 75 blank
            // lines in order to feed the printer paper...don't really need this
            // since we're not actually printing.
            // Console.WriteLine(new string('\n', 75));
        }

        /// <summary>
        /// Main entry point into the banner class and handles the main loop.
        /// </summary>
        public void Play()
        {
            GetInput();
            PrintBanner();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new Banner().Play();
        }
    }
}
