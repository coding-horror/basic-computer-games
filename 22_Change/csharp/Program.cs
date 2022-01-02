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

        static (bool, double) GetInput()
        {
            Console.WriteLine("Cost of item? ");
            var priceString = Console.ReadLine();
            if (!double.TryParse(priceString, out double price))
            {
                Console.WriteLine($"{priceString} isn't a number!");
                return (false, 0);
            }

            Console.WriteLine("Amount of payment? ");
            var paymentString = Console.ReadLine();
            if (!double.TryParse(paymentString, out double payment))
            {
                Console.WriteLine($"{paymentString} isn't a number!");
                return (false, 0);
            }

            return (true, payment - price);
        }

        static void Main(string[] args)
        {
            Header();

            while (true)
            {
                (bool result, double change) = GetInput();
                if (!result)
                    continue;

                if (change == 0)
                {
                    Console.WriteLine("Correct amount, thank you!");
                    continue;
                }
            }
        }
    }
}
