using System;
using System.Collections.Generic;

namespace Target
{
    // Provides input methods which emulate the BASIC interpreter's keyboard input routines
    internal static class Input
    {
        internal static void Prompt(string text = "") => Console.Write($"{text}? ");

        internal static List<float> ReadNumbers(string prompt, int requiredCount)
        {
            var numbers = new List<float>();

            while (!TryReadNumbers(prompt, requiredCount, numbers))
            {
                numbers.Clear();
                prompt = "";
            }

            return numbers;
        }

        private static bool TryReadNumbers(string prompt, int requiredCount, List<float> numbers)
        {
            Prompt(prompt);
            var inputValues = ReadStrings();

            foreach (var value in inputValues)
            {
                if (numbers.Count == requiredCount)
                {
                    Console.WriteLine("!Extra input ingored");
                    break;
                }

                if (!TryParseNumber(value, out var number))
                {
                    return false;
                }

                numbers.Add(number);
            }

            return numbers.Count == requiredCount || TryReadNumbers("?", requiredCount, numbers);
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
