using System;

namespace Blackjack
{
    public static class Prompt
    {
        public static bool ForYesNo(string prompt)
        {
            while(true)
            {
                Console.Write("{0} ", prompt);
                var input = Console.ReadLine();
                if (input.StartsWith("y", StringComparison.InvariantCultureIgnoreCase))
                    return true;
                if (input.StartsWith("n", StringComparison.InvariantCultureIgnoreCase))
                    return false;
                WriteNotUnderstood();
            }
        }

        public static int ForInteger(string prompt, int minimum = 1, int maximum = int.MaxValue)
        {
            while (true)
            {
                Console.Write("{0} ", prompt);
                if (!int.TryParse(Console.ReadLine(), out var number))
                    WriteNotUnderstood();
                else if (number < minimum || number > maximum)
                    Console.WriteLine("Sorry, I need a number between {0} and {1}.", minimum, maximum);
                else
                    return number;
            }
        }

        public static string ForCommandCharacter(string prompt, string allowedCharacters)
        {
            while (true)
            {
                Console.Write("{0} ", prompt);
                var input = Console.ReadLine();
                if (input.Length > 0)
                {
                    var character = input.Substring(0, 1);
                    var characterIndex = allowedCharacters.IndexOf(character, StringComparison.InvariantCultureIgnoreCase);
                    if (characterIndex != -1)
                        return allowedCharacters.Substring(characterIndex, 1);
                }

                Console.WriteLine("Type one of {0} please", String.Join(", ", allowedCharacters.ToCharArray()));
            }
        }

        private static void WriteNotUnderstood()
        {
            Console.WriteLine("Sorry, I didn't understand.");
        }
    }
}
