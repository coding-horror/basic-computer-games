using System;

WL(Tab(34) + "HI LO");
WL(Tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
WL();
WL();
WL();
WL("THIS IS THE GAME OF HI LO.");
WL();
WL("YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE");
WL("HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU");
WL("GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!");
WL("THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,");
WL("IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.");
WL();

// rnd is our random number generator
Random rnd = new();

bool playAgain = false;
int totalWinnings = 0;

do
{
    int jackpot = rnd.Next(100) + 1; // [0..99] + 1 -> [1..100]
    int guess = 1;

    while (true)
    {
        W("YOUR GUESS ");
        int amount = int.Parse(Console.ReadLine().Trim());

        if (amount == jackpot)
        {
            WL($"GOT IT!!!!!!!!!!   YOU WIN {jackpot} DOLLARS.");
            totalWinnings += jackpot;
            WL($"YOUR TOTAL WINNINGS ARE NOW {totalWinnings} DOLLARS.");
            break;
        }
        else if (amount > jackpot)
        {
            WL("YOUR GUESS IS TOO HIGH.");
        }
        else
        {
            WL("YOUR GUESS IS TOO LOW.");
        }

        guess++;
        if (guess > 6)
        {
            WL($"YOU BLEW IT...TOO BAD...THE NUMBER WAS {jackpot}");
            break;
        }
    }

    WL(); W("PLAY AGAIN (YES OR NO) ");
    playAgain = Console.ReadLine().ToUpper().StartsWith("Y");

} while (playAgain);

// Tab(n) returns n white spaces
static string Tab(int n) => new String(' ', n);
// W is an alias for Console.Write
static void W(string text) => Console.Write(text);
// WL is an alias for Console.WriteLine
static void WL(string text = "") => Console.WriteLine(text);

