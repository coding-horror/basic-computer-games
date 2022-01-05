@safe: // Make @safe the default for this file, enforcing memory-safety.
import std;

void main()
{
    enum width = 80;
    writeln(center("Word", width));
    writeln(center("(After Creative Computing  Morristown, New Jersey)\n\n\n", width));
    writeln(wrap("I am thinking of a word -- you guess it.  I will give you " ~
                 "clues to help you get it.  Good luck!!\n\n", width));

    string[] words = ["dinky", "smoke", "water", "grass", "train", "might", "first",
                      "candy", "champ", "would", "clump", "dopey"];

    playLoop: while (true)
    {
        writeln("\n\nYou are starting a new game...");

        string word = words[uniform(0, $-1)];  // $ is a short-hand for words.length.
        int guesses = 0;
        string knownLetters = '-'.repeat(word.length).array;

        while (true)
        {
            writeln("Guess a ", word.length, " letter word");
            string guess = readString.toLower;
            if (guess == "?")
            {
                writeln("The secret word is ", word, "\n");
                continue playLoop;  // Start a new game.
            }
            /* Uncomment this for equivalence with Basic.
            if (guess.length != 5)
            {
                writeln("You must guess a 5 letter word.  Start again.");
                continue;           // Ask for new guess.
            }
            */
            guesses++;
            if (guess == word)
                break;  // Done guessing
            string commonLetters;
            foreach (i, wordLetter; word)
                foreach (j, guessLetter; guess)
                    if (guessLetter == wordLetter)
                    {
                        commonLetters ~= guessLetter;
                        if (i == j)
                            knownLetters.replaceInPlace(i, i + 1, [guessLetter]);
                    }
            writeln("There were ", commonLetters.length, " matches and the common letters were... ", commonLetters);
            writeln("From the exact letter matches, you know................ ", knownLetters);
            if (knownLetters == word)
                break;  // Done guessing
            if (commonLetters.length < 2)
                writeln("If you give up, type '?' for your next guess.");
            writeln;
        }

        writeln("You have guessed the word.  It took ", guesses, " guesses!");
        write("\n\nWant to play again? ");
        if (readString.toLower != "yes")
            break;  // Terminate playLoop
    }
}

/// Read a string from standard input, stripping newline and other enclosing whitespace.
string readString() nothrow
{
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
    return readln;
}
