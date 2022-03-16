using System;

namespace Name
{
    public static class StringExtensions
    {
        private const int ConsoleWidth = 120; // default console width

        public static string CentreAlign(this string value)
        {
            int spaces = ConsoleWidth - value.Length;
            int leftPadding = spaces / 2 + value.Length;

            return value.PadLeft(leftPadding).PadRight(ConsoleWidth);
        }

        public static string Reverse(this string value)
        {
            if (value is null)
            {
                return null;
            }

            char[] characterArray = value.ToCharArray();
            Array.Reverse(characterArray);
            return new String(characterArray);
        }

        public static string Sort(this string value)
        {
            if (value is null)
            {
                return null;
            }

            char[] characters = value.ToCharArray();
            Array.Sort(characters);
            return new string(characters);
        }
    }
}
