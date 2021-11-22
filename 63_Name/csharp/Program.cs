using System;

namespace Name
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NAME".CentreAlign());
            Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".CentreAlign());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("HELLO.");
            Console.WriteLine("MY NAME IS CREATIVE COMPUTER.");
            Console.Write("WHAT'S YOUR NAME (FIRST AND LAST? ");
            var name = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine($"THANK YOU, {name.Reverse()}.");
            Console.WriteLine("OOPS!  I GUESS I GOT IT BACKWARDS.  A SMART");
            Console.WriteLine("COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!");
            Console.WriteLine();
            Console.WriteLine("BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER.");
            Console.WriteLine($"LET'S PUT THEM IN ORDER LIKE THIS: {name.Sort()}");
            Console.WriteLine();
            Console.Write("DON'T YOU LIKE THAT BETTER? ");
            var like = Console.ReadLine();
            Console.WriteLine();

            if (like.ToUpperInvariant() == "YES")
            {
                Console.WriteLine("I KNEW YOU'D AGREE!!");
            }
            else
            {
                Console.WriteLine("I'M SORRY YOU DON'T LIKE IT THAT WAY.");
            }

            Console.WriteLine();
            Console.WriteLine($"I REALLY ENJOYED MEETING YOU {name}.");
            Console.WriteLine("HAVE A NICE DAY!");
        }
    }
}
