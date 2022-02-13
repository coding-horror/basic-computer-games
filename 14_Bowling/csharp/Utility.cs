using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bowling
{
    internal static class Utility
    {
        public static string PadInt(int value, int width)
        {
            return value.ToString().PadLeft(width);
        }
        public static int InputInt()
        {
            while (true)
            {
                if (int.TryParse(InputString(), out int i))
                    return i;
                else
                    PrintString("!NUMBER EXPECTED - RETRY INPUT LINE");
            }
        }
        public static string InputString()
        {
            PrintString("? ", false);
            var input = Console.ReadLine();
            return input == null ? string.Empty : input.ToUpper();
        }
        public static void PrintInt(int value, bool newLine = false)
        {
            PrintString($"{value} ", newLine);
        }
        public static void PrintString(bool newLine = true)
        {
            PrintString(0, string.Empty);
        }
        public static void PrintString(int tab, bool newLine = true)
        {
            PrintString(tab, string.Empty, newLine);
        }
        public static void PrintString(string value, bool newLine = true)
        {
            PrintString(0, value, newLine);
        }
        public static void PrintString(int tab, string value, bool newLine = true)
        {
            Console.Write(new String(' ', tab));
            Console.Write(value);
            if (newLine) Console.WriteLine();
        }
    }
}
