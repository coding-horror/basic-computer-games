using System;

namespace Change
{
    class Program
    {
        /// <summary>
        /// Prints header.
        /// </summary>
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

        /// <summary>
        /// Gets user input for price and payment.
        /// </summary>
        /// <returns>
        /// False if any input can't be parsed to double. Price and payment returned would be 0.
        /// True if it was possible to parse inputs into doubles. Price and payment returned 
	    /// would be as provided by the user.
	    /// </returns>
        static (bool status, double price, double payment) GetInput()
        {
            Console.Write("Cost of item? ");
            var priceString = Console.ReadLine();
            if (!double.TryParse(priceString, out double price))
            {
                Console.WriteLine($"{priceString} isn't a number!");
                return (false, 0, 0);
            }

            Console.Write("Amount of payment? ");
            var paymentString = Console.ReadLine();
            if (!double.TryParse(paymentString, out double payment))
            {
                Console.WriteLine($"{paymentString} isn't a number!");
                return (false, 0, 0);
            }

            return (true, price, payment);
        }

        /// <summary>
        /// Prints bills and coins for given change.
        /// </summary>
        /// <param name="change"></param>
        static void PrintChange(double change)
        {
            var tens = (int)(change / 10);
            if (tens > 0)
                Console.WriteLine($"{tens} ten dollar bill(s)");

            var temp = change - (tens * 10);
            var fives = (int)(temp / 5);
            if (fives > 0)
                Console.WriteLine($"{fives} five dollar bill(s)");

            temp -= fives * 5;
            var ones = (int)temp;
            if (ones > 0)
                Console.WriteLine($"{ones} one dollar bill(s)");

            temp -= ones;
            var cents = temp * 100;
            var half = (int)(cents / 50);
            if (half > 0)
                Console.WriteLine($"{half} one half dollar(s)");

            temp = cents - (half * 50);
            var quarters = (int)(temp / 25);
            if (quarters > 0)
                Console.WriteLine($"{quarters} quarter(s)");

            temp -= quarters * 25;
            var dimes = (int)(temp / 10);
            if (dimes > 0)
                Console.WriteLine($"{dimes} dime(s)");

            temp -= dimes * 10;
            var nickels = (int)(temp / 5);
            if (nickels > 0)
                Console.WriteLine($"{nickels} nickel(s)");

            temp -= nickels * 5;
            var pennies = (int)(temp + 0.5);
            if (pennies > 0)
                Console.WriteLine($"{pennies} penny(s)");
        }

        static void Main(string[] args)
        {
            Header();

            while (true)
            {
                (bool result, double price, double payment) = GetInput();
                if (!result)
                    continue;

                var change = payment - price;
                if (change == 0)
                {
                    Console.WriteLine("Correct amount, thank you!");
                    continue;
                }

                if (change < 0)
                {
                    Console.WriteLine($"Sorry, you have short-changed me ${price - payment:N2}!");
                    continue;
                }

                Console.WriteLine($"Your change ${change:N2}");
                PrintChange(change);
                Console.WriteLine("Thank you, come again!");
                Console.WriteLine();
            }
        }
    }
}
