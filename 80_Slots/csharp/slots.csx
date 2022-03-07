Print("SLOTS");
Print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
Print(); Print(); Print();
Print("YOU ARE IN THE H&M CASINO,IN FRONT OF ONE OF OUR");
Print("ONE-ARM BANDITS. BET FROM $1 TO $100.");
Print("TO PULL THE ARM, PUNCH THE RETURN KEY AFTER MAKING YOUR BET.");

var _standings = 0;

var play = true;
while(play)
{
    Play();
    play = PlayAgain();
}

Done();

public void Play()
{
    var bet = GetBet();
    Print();
    Ring();

    var random = new Random();
    var x = GetSlot();
    var y = GetSlot();
    var z = GetSlot();

    Print();
    Print($"{x.ToString()} {y.ToString()} {z.ToString()}");

    if(x == y && x == z)
    {
        if(z == Slot.BAR)
        {
            // BAR BAR BAR
            Print();
            Print("***JACKPOT***");
            Print("YOU WON!");
            _standings = (100*bet) + bet + _standings;
        }
        else
        {
            Print();
            Print("**TOP DOLLAR**");
            Print("YOU WON!");
            _standings = (10*bet) + bet + _standings;
        }
    }
    else if(x == y)
    {
        if(y == Slot.BAR)
        {
            DoubleBar(bet);
        }
        else
        {
            Double(bet);
        }
    }
    else if(x == z)
    {
        if(z == Slot.BAR)
        {
            DoubleBar(bet);
        }
        else
        {
            Lost(bet);
        }
    }
    else if(y == z)
    {
        if(z == Slot.BAR)
        {
            DoubleBar(bet);
        }
        else
        {
            Double(bet);
        }
    }
    else
    {
        Lost(bet);
    }

    Print($"YOUR STANDINGS ARE ${_standings}");
}

public bool PlayAgain()
{
    Console.Write("AGAIN? (Y) ");
    var playAgain = Console.ReadKey(true);
    Print();
    return playAgain.Key == ConsoleKey.Y || playAgain.Key == ConsoleKey.Enter;
}

public void Done()
{
    Print();
    if(_standings < 0)
    {
        Print("PAY UP!  PLEASE LEAVE YOUR MONEY ON THE TERMINAL.");
    }
    else if (_standings == 0)
    {
        Print("HEY, YOU BROKE EVEN.");
    }
    else
    {
        Print("COLLECT YOUR WINNINGS FROM THE H&M CASHIER");
    }
}

// Prints the text provided.  Default is a blank line
public void Print(string line = "")
{
    Console.WriteLine(line);
}

public int GetBet()
{
    Print();
    Console.Write("YOUR BET ");
    var betInput = ReadLine();
    int bet;
    var inputValid = int.TryParse(betInput, out bet);
    if (!inputValid)
    {
        Print("NUMBER EXPECTED - RETRY");
        return GetBet();
    }

    if(bet > 100)
    {
        Print("HOUSE LIMITS ARE $100");
        inputValid = false;
    }
    else if(bet < 1)
    {
        Print("MINIMUM BET IS $1");
        inputValid = false;
    }

    return inputValid ? bet : GetBet();
}

public enum Slot { BAR, BELL, ORANGE, LEMON, PLUM, CHERRY };

public Slot GetSlot()
{
    var rand = new Random();
    var num = rand.Next(0, 5);
    return (Slot)num;
}

public void DoubleBar(int bet)
{
    Print();
    Print("*DOUBLE BAR*");
    Print("YOU WON!");
    _standings = (5*bet) + bet + _standings;
}

public void Double(int bet)
{
    Print();
    Print("DOUBLE!!");
    Print("YOU WON!");
    _standings = (2*bet) + bet + _standings;
}

public void Lost(int bet)
{
    Print();
    Print("YOU LOST.");
    _standings = _standings - bet;
}

public void Ring()
{
    for(int i = 1; i <= 10; i++)
    {
        // https://stackoverflow.com/a/321148/1497
        Console.Beep();
        // Console.Beep(800, 501 - (i * 50)); // Uncomment for a fancier bell
    }
}
