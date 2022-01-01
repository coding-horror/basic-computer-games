using System;

namespace Buzzword
{
    class Program
    {
        static void Header()
        {
            Console.WriteLine("Buzzword generator".PadLeft(26));
            Console.WriteLine("Creating Computing Morristown, New Jersey".PadLeft(15));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        static void Instructions()
        {
            Console.WriteLine("This program prints highly acceptable phrases in\n"
            + "'Educator-speak'that you can work into reports\n"
            + "and speeches. Whenever a question mark is printed,\n"
            + "type a 'Y' for another phrase or 'N' to quit.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Here's the first phrase:");
        }

        static void Main(string[] args)
        {
            Header();
            Instructions();
        }
    }
}
