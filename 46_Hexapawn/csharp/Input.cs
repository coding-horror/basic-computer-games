using System;
using System.Linq;

namespace Hexapawn
{
    // Provides input methods which emulate the BASIC interpreter's keyboard input routines
    internal static class Input
    {
        internal static char GetYesNo(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (Y-N)? ");
                var response = Console.ReadLine().FirstOrDefault();
                if ("YyNn".Contains(response))
                {
                    return char.ToUpperInvariant(response);
                }
            }
        }

        // Implements original code:
        //   120 PRINT "YOUR MOVE";
        //   121 INPUT M1,M2
        //   122 IF M1=INT(M1)AND M2=INT(M2)AND M1>0 AND M1<10 AND M2>0 AND M2<10 THEN 130
        //   123 PRINT "ILLEGAL CO-ORDINATES."
        //   124 GOTO 120
        internal static Move GetMove(string prompt)
        {
            while(true)
            {
                ReadNumbers(prompt, out var from, out var to);

                if (Move.TryCreate(from, to, out var move))
                {
                    return move;
                }

                Console.WriteLine("Illegal Coordinates.");
            }
        }

        internal static void Prompt(string text = "") => Console.Write($"{text}? ");

        internal static void ReadNumbers(string prompt, out float number1, out float number2)
        {
            while (!TryReadNumbers(prompt, out number1, out number2))
            {
                prompt = "";
            }
        }

        private static bool TryReadNumbers(string prompt, out float number1, out float number2)
        {
            Prompt(prompt);
            var inputValues = ReadStrings();

            if (!TryParseNumber(inputValues[0], out number1))
            {
                number2 = default;
                return false;
            }

            if (inputValues.Length == 1)
            {
                return TryReadNumber("?", out number2);
            }

            if (!TryParseNumber(inputValues[1], out number2))
            {
                number2 = default;
                return false;
            }

            if (inputValues.Length > 2)
            {
                Console.WriteLine("!Extra input ingored");
            }

            return true;
        }

        private static bool TryReadNumber(string prompt, out float number)
        {
            Prompt(prompt);
            var inputValues = ReadStrings();

            if (!TryParseNumber(inputValues[0], out number))
            {
                return false;
            }

            if (inputValues.Length > 1)
            {
                Console.WriteLine("!Extra input ingored");
            }

            return true;
        }

        private static string[] ReadStrings() => Console.ReadLine().Split(',', StringSplitOptions.TrimEntries);

        private static bool TryParseNumber(string text, out float number)
        {
            if (float.TryParse(text, out number)) { return true; }

            Console.WriteLine("!Number expected - retry input line");
            number = default;
            return false;
        }
    }
}
