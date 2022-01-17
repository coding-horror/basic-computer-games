using System;
using System.Collections.Generic;

namespace Love
{
    // Provides input methods which emulate the BASIC interpreter's keyboard input routines
    internal static class Input
    {
        private static void Prompt(string text = "") => Console.Write($"{text}? ");

        public static string ReadLine(string prompt)
        {
            Prompt(prompt);
            var values = ReadStrings();

            if (values.Length > 1)
            {
                Console.WriteLine("!Extra input ingored");
            }

            return values[0];
        }

        private static string[] ReadStrings() => Console.ReadLine().Split(',', StringSplitOptions.TrimEntries);
    }
}
