@safe: // Make @safe the default for this file, enforcing memory-safety.

void main()
{
    import std.stdio : write, writeln;
    import std.string : center, toUpper, wrap;
    import std.exception : ifThrown;

    enum width = 80;
    writeln(center("Acey Ducey Card Game", width));
    writeln(center("(After Creative Computing  Morristown, New Jersey)\n", width));
    writeln(wrap("Acey-Ducey is played in the following manner: The dealer (computer) deals two cards face up. " ~
                 "You have an option to bet or not bet depending on whether or not you feel the third card will " ~
                 "have a value between the first two. If you do not want to bet, input a 0.", width));

    enum Hand {low, middle, high}
    Card[Hand.max + 1] cards; // Three cards.
    bool play = true;

    while (play)
    {
        int cash = 100;
        while (cash > 0)
        {
            writeln("\nYou now have ", cash, " dollars.");
            int bet = 0;
            while (bet <= 0)
            {
                do // Draw new cards, until the first card has a smaller value than the last card.
                {
                    foreach (ref card; cards)
                        card.drawNew;
                } while (cards[Hand.low] >= cards[Hand.high]);
                writeln("Here are your next two cards:\n", cards[Hand.low], "\n", cards[Hand.high]);

                int askBet() // A nested function.
                {
                    import std.conv : to;

                    write("\nWhat is your bet? ");
                    int answer = readString.to!int.
                            ifThrown!Exception(askBet); // Try again when answer does not convert to int.
                    if (answer <= cash)
                        return answer;
                    writeln("Sorry, my friend, but you bet too much.\nYou have only ", cash, " dollars to bet.");
                    return askBet; // Recurse: Ask again.
                }
                bet = askBet;
                if (bet <= 0) // Negative bets are interpreted as 0.
                    writeln("CHICKEN!!");
            } // bet is now > 0.

            writeln(cards[Hand.middle]);
            if (cards[Hand.low] < cards[Hand.middle] && cards[Hand.middle] < cards[Hand.high])
            {
                writeln("YOU WIN!!!");
                cash += bet;
            } 
            else
            {
                writeln("Sorry, you lose.");
                cash -= bet;
                if (cash <= 0)
                {
                    writeln("\n\nSorry, friend, but you blew your wad.");
                    write("\n\nTry again (Yes or No)? ");
                    play = readString.toUpper == "YES";
                }
            }
        }
    }
    writeln("O.K., hope you had fun!");
}

struct Card
{
    int value = 2;
    alias value this; // Enables Card to stand in as an int, so that cards can be compared as ints.

    invariant
    {
        assert(2 <= value && value <= 14); // Ensure cards always have a valid value.
    }

    /// Adopt a new value.
    void drawNew()
    {
        import std.random : uniform;

        value = uniform!("[]", int, int)(2, 14); // A random int between inclusive bounds.
    }

    /// Called for implicit conversion to string.
    string toString() const pure
    {
        import std.conv : text;

        switch (value)
        {
            case 11: return "Jack";
            case 12: return "Queen";
            case 13: return "King";
            case 14: return "Ace";
            default: return text(" ", value); // Basic prepends a space.
        }
    }
}

/// Read a string from standard input, stripping newline and other enclosing whitespace.
string readString() nothrow
{
    import std.string : strip;

    try
        return trustedReadln.strip;
    catch (Exception)   // readln throws on I/O and Unicode errors, which we handle here.
        return "";
}

/** An @trusted wrapper around readln.
 *
 * This is the only function that formally requires manual review for memory-safety.
 * [Arguably readln should be safe already](https://forum.dlang.org/post/rab398$1up$1@digitalmars.com)
 * which would remove the need to have any @trusted code in this program.
 */
string trustedReadln() @trusted
{
    import std.stdio : readln;

    return readln;
}
