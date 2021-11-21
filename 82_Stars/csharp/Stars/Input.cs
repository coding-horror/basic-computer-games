using System;

namespace Stars
{
    internal static class Input
    {
        // Float, because that's what the BASIC input operation returns
        internal static float GetNumber(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                var response = Console.ReadLine();
                if (float.TryParse(response, out var value))
                {
                    return value;
                }

                Console.WriteLine("!Number expected - retry input line");
                Console.Write("? ");
            }
        }

        internal static string GetString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
