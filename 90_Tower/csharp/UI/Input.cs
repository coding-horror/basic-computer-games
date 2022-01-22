using System;
using System.Collections.Generic;

namespace Tower.UI
{
    // Provides input methods which emulate the BASIC interpreter's keyboard input routines
    internal static class Input
    {
        private static void Prompt(string text = "") => Console.Write($"{text}? ");

        internal static bool ReadYesNo(string prompt, string retryPrompt)
        {
            var response = ReadString(prompt);

            while (true)
            {
                if (response.Equals("No", StringComparison.InvariantCultureIgnoreCase)) { return false; }
                if (response.Equals("Yes", StringComparison.InvariantCultureIgnoreCase)) { return true; }
                response = ReadString(retryPrompt);
            }
        }

        internal static bool TryReadNumber(Prompt prompt, out int number)
        {
            var message = prompt.Message;

            for (int retryCount = 0; retryCount <= prompt.RetriesAllowed; retryCount++)
            {
                if (retryCount > 0) { Console.WriteLine(prompt.RetryMessage); }

                if (prompt.TryValidateResponse(ReadNumber(message), out number)) { return true; }

                if (!prompt.RepeatPrompt) { message = ""; }
            }

            Console.WriteLine(prompt.QuitMessage);

            number = 0;
            return false;
        }

        private static float ReadNumber(string prompt)
        {
            Prompt(prompt);

            while (true)
            {
                var inputValues = ReadStrings();

                if (TryParseNumber(inputValues[0], out var number))
                {
                    if (inputValues.Length > 1)
                    {
                        Console.WriteLine("!Extra input ingored");
                    }

                    return number;
                }
            }
        }

        private static string ReadString(string prompt)
        {
            Prompt(prompt);

            var inputValues = ReadStrings();
            if (inputValues.Length > 1)
            {
                Console.WriteLine("!Extra input ingored");
            }
            return inputValues[0];
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
