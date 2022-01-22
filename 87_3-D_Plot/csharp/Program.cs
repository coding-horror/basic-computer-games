using System;

namespace Plot
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintTitle();

            foreach (var row in Function.GetRows())
            {
                foreach (var z in row)
                {
                    Plot(z);
                }
                Console.WriteLine();
            }
        }

        private static void PrintTitle()
        {
            Console.WriteLine("                                3D Plot");
            Console.WriteLine("               Creative Computing  Morristown, New Jersey");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void Plot(int z)
        {
            var x = Console.GetCursorPosition().Top;
            Console.SetCursorPosition(z, x);
            Console.Write("*");
        }
    }
}
