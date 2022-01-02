using System;

namespace Change
{
    class Program
    {
        static void Header()
        {
            Console.WriteLine("Change".PadLeft(33));
            Console.WriteLine("Creative Computing Morristown, New Jersey".PadLeft(15));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("I, your friendly microcomputer, will determine\n"
            + "the correct change for items costing up to $100.");
            Console.WriteLine();
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Header();
        }
    }
}
