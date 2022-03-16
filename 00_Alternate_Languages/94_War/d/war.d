@safe: // Make @safe the default for this file, enforcing memory-safety.
import std;

void main()
{
    enum width = 50;
    writeln(center("War", width));
    writeln(center("(After Creative Computing  Morristown, New Jersey)\n\n", width));
    writeln(wrap("This is the card game of war.  Each card is given by suit-# " ~
                 "as 7♠ for seven of spades.  ", width));

    if ("Do you want instructions?".yes)
        writeln("\n", wrap("The computer gives you and it a 'card'.  The higher card " ~
                           "(numerically) wins.  The game ends when you choose not to " ~
                           "continue or when you have finished the pack.\n", width));

    static const cards = cartesianProduct(["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"],
                                          ["♠", "♥", "♦", "♣"]).map!(a => a.expand.only.join).array;
    const indices = iota(0, cards.length).array.randomShuffle;
    int yourScore = 0, compScore = 0, i = 0;
    while (i < indices.length)
    {
        size_t your = indices[i++], comp = indices[i++];
        writeln("\nYou: ", cards[your].leftJustify(9), "Computer: ", cards[comp]);
        your /= 4; comp /= 4;
        if (your == comp)
            writeln("Tie. No score change.");
        else if (your < comp)
            writeln("The computer wins!!! You have ", yourScore,
                    " and the computer has ", ++compScore, ".");
        else
            writeln("You win. You have ", ++yourScore,
                    " and the computer has ", compScore, ".");
        if (i == indices.length)
            writeln("\nWe have run out of cards. Final score: You: ", yourScore,
                    ", the computer: ", compScore, ".");
        else if (!"Do you want to continue?".yes)
            break;
    }
    writeln("\nThanks for playing. It was fun.");
}

/// Returns whether question was answered positively.
bool yes(string question)
{
    writef!"%s "(question);
    try
        return trustedReadln.strip.toLower.startsWith("y");
    catch (Exception)   // readln throws on I/O and Unicode errors, which we handle here.
        return false;
}

/** An @trusted wrapper around readln.
*
* This is the only function that formally requires manual review for memory-safety.
* [Arguably readln should be safe already](https://forum.dlang.org/post/rab398$1up$1@digitalmars.com)
* which would remove the need to have any @trusted code in this program.
*/
string trustedReadln() @trusted
{
    return readln;
}

version (Windows)
{
    // Make the Windows console do a better job at printing UTF-8 strings,
    // and restore the default upon termination.

    import core.sys.windows.windows;

    shared static this() @trusted
    {
        SetConsoleOutputCP(CP_UTF8);
    }

    shared static ~this() @trusted
    {
        SetConsoleOutputCP(GetACP);
    }
}
