@safe: // Make @safe the default for this file, enforcing memory-safety.
import std;

void main()
{
    enum width = 50;
    writeln(center("H-I-Q", width));
    writeln(center("(After Creative Computing  Morristown, New Jersey)\n\n", width));
    writeln(wrap("Fields are identified by 2-digit numbers, each between 1 and 7. " ~
                 "Example: the middle field is 44, the bottom middle is 74.", width));

    Board board;

    while (true)
    {
        while (!board.isGameOver)
        {
            writeln(board); // Calls board.toString().
            board.makeMove;
        }
        writeln(board, "\nThe game is over.\nYou had ", board.numPegs, " pieces remaining.");
        if (board.numPegs == 1)
            writeln("Bravo!  You made a perfect score!\n",
                    "Make a screen dump as a record of your accomplishment!\n");
        write("Play again? (Yes or No) ");
        if (readString.toLower != "yes")
            break;
        writeln; writeln;
        board = Board.init;
    }
    writeln("\nSo long for now.\n");
}

/// Representation of the game board with pegs.
struct Board
{
    enum {outside, taken, empty};
    int[8][8] state = [
        1: [                    3: taken, 4: taken, 5: taken],
        2: [                    3: taken, 4: taken, 5: taken],
        3: [1: taken, 2: taken, 3: taken, 4: taken, 5: taken, 6: taken, 7: taken],
        4: [1: taken, 2: taken, 3: taken, 4: empty, 5: taken, 6: taken, 7: taken],
        5: [1: taken, 2: taken, 3: taken, 4: taken, 5: taken, 6: taken, 7: taken],
        6: [                    3: taken, 4: taken, 5: taken],
        7: [                    3: taken, 4: taken, 5: taken]
    ]; // Row 0 and column 0 are unused. Default is 0 (outside).

    /// Returns a string representing the board and its current state.
    string toString() const
    {
        dchar[][] lines = [("      _1  _2  _3  _4  _5  _6  _7 ").to!(dchar[]),
                           ("            ┌───┬───┬───┐        ").to!(dchar[]),
                           (" 1_         │   │   │   │        ").to!(dchar[]),
                           ("            ├───┼───┼───┤        ").to!(dchar[]),
                           (" 2_         │   │   │   │        ").to!(dchar[]),
                           ("    ┌───┬───┼───┼───┼───┼───┬───┐").to!(dchar[]),
                           (" 3_ │   │   │   │   │   │   │   │").to!(dchar[]),
                           ("    ├───┼───┼───┼───┼───┼───┼───┤").to!(dchar[]),
                           (" 4_ │   │   │   │   │   │   │   │").to!(dchar[]),
                           ("    ├───┼───┼───┼───┼───┼───┼───┤").to!(dchar[]),
                           (" 5_ │   │   │   │   │   │   │   │").to!(dchar[]),
                           ("    └───┴───┼───┼───┼───┼───┴───┘").to!(dchar[]),
                           (" 6_         │   │   │   │        ").to!(dchar[]),
                           ("            ├───┼───┼───┤        ").to!(dchar[]),
                           (" 7_         │   │   │   │        ").to!(dchar[]),
                           ("            └───┴───┴───┘        ").to!(dchar[])];
        foreach (y, row; state)
            foreach (x, field; row)
                if (field == taken)
                    lines[y * 2][x * 4 + 2] = '■';
        return lines.join("\n").to!string;
    }

    /// Tests for possible moves.
    bool isGameOver() const
    {
        foreach (r, row; state)
            foreach (c, field; row)
                if (field == taken && canMoveFrom(r, c))
                    return false;
        return true;
    }

    bool canMoveFrom(int row, int col) const
    {
        if (row >= 3 && state[row - 2][col] == empty)   // Up
            return state[row - 1][col] == taken;
        if (row <= 5 && state[row + 2][col] == empty)   // Down
            return state[row + 1][col] == taken;
        if (col >= 3 && state[row][col - 2] == empty)   // Left
            return state[row][col - 1] == taken;
        if (col <= 5 && state[row][col + 2] == empty)   // Right
            return state[row][col + 1] == taken;
        return false;
    }

    /// Asks for input, validates the move and updates the board.
    void makeMove()
    {
        bool isOutside(int row, int col)
        {
            if (row < 1 || row > 7 ||
                col < 1 || col > 7 ||
                state[row][col] == outside)
            {
                writeln("Field ", row, col, " is ouside the board. Try again.");
                return true;
            }
            return false;
        }

        while (true)
        {
            auto from = (){
                while (true)
                {
                    write("\nMove which peg? ");
                    int field = readInt;
                    int row = field / 10, col = field % 10;
                    if (isOutside(row, col))
                        continue;
                    if (state[row][col] != taken)
                    {
                        writeln("There is no peg at ", field, ". Try again.");
                        continue;
                    }
                    if (!canMoveFrom(row, col))
                    {
                        writeln("The peg at ", field, " has nowhere to go. Try again.");
                        continue;
                    }
                    return tuple!("row", "col")(row, col);
                }
            }();
            auto to = (){
                while (true)
                {
                    write("To where? ");
                    int field = readInt;
                    int row = field / 10, col = field % 10;
                    if (isOutside(row, col))
                        continue;
                    if (state[row][col] == taken)
                    {
                        writeln("Field ", field, " is occupied. Try again.");
                        continue;
                    }
                    if (row != from.row && col != from.col)
                    {
                        writeln("You cannot move diagonally. Try again.");
                        continue;
                    }
                    if (row == from.row && col == from.col)
                    {
                        writeln("You aren't going anywhere. Try again.");
                        continue;
                    }
                    if (abs(row - from.row) + abs(col - from.col) > 2)
                    {
                        writeln("You can't jump that far. Try again.");
                        continue;
                    }
                    if (abs(row - from.row) + abs(col - from.col) < 2 ||
                        state[(row + from.row) / 2][(col + from.col) / 2] != taken)
                    {
                        writeln("You need to jump over another peg. Try again.");
                        continue;
                    }
                    return tuple!("row", "col")(row, col);
                }
            }();
            // The move is legal. Update the board state.
            state[from.row][from.col] = empty;
            state[  to.row][  to.col] = taken;
            state[(from.row + to.row) / 2][(from.col + to.col) / 2] = empty;
            writeln;
            break;
        }
    }

    /// Returns the number of remaining pegs on the board.
    int numPegs() const
    {
        int num = 0;
        foreach (row; state)
            foreach (field; row)
                if (field == taken)
                    num++;
        return num;
    }
}

/// Reads an integer from standard input.
int readInt() nothrow
{
    try
        return readString.to!int;
    catch (Exception)   // Not an integer.
        return 0;
}

/// Reads a string from standard input.
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
