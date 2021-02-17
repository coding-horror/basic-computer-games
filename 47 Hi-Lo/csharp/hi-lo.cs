using System;

Console.WriteLine(Tab(34) +                 "HI LO");
Console.WriteLine(Tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("THIS IS THE GAME OF HI LO.");
Console.WriteLine();
Console.WriteLine("YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE");
Console.WriteLine("HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU");
Console.WriteLine("GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!");
Console.WriteLine("THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,");
Console.WriteLine("IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.");
Console.WriteLine();

// rnd is our random number generator
Random rnd = new();

bool playAgain = false;
int totalWinnings = 0;

do // Our game loop
{
    int jackpot = rnd.Next(100) + 1; // [0..99] + 1 -> [1..100]
    int guess = 1;

    while (true) // Our guessing loop
    {
        Console.WriteLine();
        int amount = ReadInt("YOUR GUESS ");

        if (amount == jackpot)
        {
            Console.WriteLine($"GOT IT!!!!!!!!!!   YOU WIN {jackpot} DOLLARS.");
            totalWinnings += jackpot;
            Console.WriteLine($"YOUR TOTAL WINNINGS ARE NOW {totalWinnings} DOLLARS.");
            break;
        }
        else if (amount > jackpot)
        {
            Console.WriteLine("YOUR GUESS IS TOO HIGH.");
        }
        else
        {
            Console.WriteLine("YOUR GUESS IS TOO LOW.");
        }

        guess++;
        if (guess > 6)
        {
            Console.WriteLine($"YOU BLEW IT...TOO BAD...THE NUMBER WAS {jackpot}");
            break;
        }
    }

    Console.WriteLine();
    Console.Write("PLAY AGAIN (YES OR NO) ");
    playAgain = Console.ReadLine().ToUpper().StartsWith("Y");

} while (playAgain);

Console.WriteLine();
Console.WriteLine("SO LONG.  HOPE YOU ENJOYED YOURSELF!!!");

// Tab(n) returns n spaces
static string Tab(int n) => new String(' ', n);

// ReadInt asks the user to enter a number
static int ReadInt(string question)
{
    while (true)
    {
        Console.Write(question);
        var input = Console.ReadLine().Trim();
        if (int.TryParse(input, out int value))
        {
            return value;
        }
        Console.WriteLine("!Invalid Number Entered.");
    }
}

