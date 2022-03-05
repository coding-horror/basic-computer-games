/*********************************************************************************
 * CHECKERS
 * ported from BASIC https://www.atariarchives.org/basicgames/showpage.php?page=41
 * 
 * Porting philosophy
 * 1) Adhere to the original as much as possible
 * 2) Attempt to be understandable by Novice progammers
 * 
 * There are no classes or Object Oriented design patterns used in this implementation.
 * Everything is written procedurally, using only top-level functions. Hopefully, this
 * will be approachable for someone who wants to learn C# syntax without experience with
 * Object Oriented concepts. Similarly, basic data structures have been chosen over more
 * powerful collection types.  Linq/lambda syntax is also excluded.
 * 
 * C# Concepts contained in this example:
 *    Loops (for, foreach, while, and do)
 *    Multidimensional arrays
 *    Tuples
 *    Nullables
 *    IEnumerable (yield return / yield break) 
 *    
 * The original had multiple implementations of logic, like determining second jump locations.
 * This has been refactored to reduce unnecessary code duplication.
 *********************************************************************************/
#region Display functions
void SkipLines(int count)
{
    for (int i = 0; i < count; i++)
    {
        Console.WriteLine();
    }
}

void PrintBoard(int[,] state)
{
    SkipLines(3);
    for (int y = 7; y >= 0; y--)
    {
        for (int x = 0; x < 8; x++)
        {
            switch(state[x,y])
            {
                case -2:
                    Console.Write("X*");
                    break;
                case -1:
                    Console.Write("X ");
                    break;
                case 0:
                    Console.Write(". ");
                    break;
                case 1:
                    Console.Write("O ");
                    break;
                case 2:
                    Console.Write("O*");
                    break;
            }
            Console.Write("   ");
        }
        Console.WriteLine();
    }
}

void WriteCenter(string text)
{
    const int LineLength = 80;
    var spaces = (LineLength - text.Length) / 2;
    Console.WriteLine($"{"".PadLeft(spaces)}{text}");
}

void ComputerWins()
{
    Console.WriteLine("I WIN.");
}
void PlayerWins()
{
    Console.WriteLine("YOU WIN.");
}

void WriteIntroduction()
{
    WriteCenter("CHECKERS");
    WriteCenter("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    SkipLines(3);
    Console.WriteLine("THIS IS THE GAME OF CHECKERS. THE COMPUTER IS X,");
    Console.WriteLine("AND YOU ARE O.  THE COMPUTER WILL MOVE FIRST.");
    Console.WriteLine("SQUARES ARE REFERRED TO BY A COORDINATE SYSTEM.");
    Console.WriteLine("(0,0) IS THE LOWER LEFT CORNER");
    Console.WriteLine("(0,7) IS THE UPPER LEFT CORNER");
    Console.WriteLine("(7,0) IS THE LOWER RIGHT CORNER");
    Console.WriteLine("(7,7) IS THE UPPER RIGHT CORNER");
    Console.WriteLine("THE COMPUTER WILL TYPE '+TO' WHEN YOU HAVE ANOTHER");
    Console.WriteLine("JUMP.  TYPE TWO NEGATIVE NUMBERS IF YOU CANNOT JUMP.");
    SkipLines(3);
}
#endregion

#region State validation functions
bool IsPointOutOfBounds(int x)
{
    return x < 0 || x > 7;
}

bool IsOutOfBounds((int x, int y) position)
{
    return IsPointOutOfBounds(position.x) || IsPointOutOfBounds(position.y);
}

bool IsJumpMove((int x, int y) from, (int x, int y) to)
{
    return Math.Abs(from.y - to.y) == 2;
}

bool IsValidPlayerMove(int[,] state, (int x, int y) from, (int x, int y) to)
{
    if (state[to.x, to.y] != 0)
    {
        return false;
    }
    var deltaX = Math.Abs(to.x - from.x);
    var deltaY = Math.Abs(to.y - from.y);
    if (deltaX != 1 && deltaX != 2)
    {
        return false;
    }
    if (deltaX != deltaY)
    {
        return false;
    }
    if (state[from.x, from.y] == 1 && Math.Sign(to.y - from.y) <= 0)
    {
        // only kings can move downwards
        return false;
    }
    if (deltaX == 2)
    {
        var jump = GetJumpedPiece(from, to);
        if (state[jump.x, jump.y] >= 0)
        {
            // no valid piece to jump
            return false;
        }
    }
    return true;
}

bool CheckForComputerWin(int[,] state)
{
    bool playerAlive = false;
    foreach (var piece in state)
    {
        if (piece > 0)
        {
            playerAlive = true;
            break;
        }
    }
    return !playerAlive;
}

bool CheckForPlayerWin(int[,] state)
{
    bool computerAlive = false;
    foreach (var piece in state)
    {
        if (piece < 0)
        {
            computerAlive = true;
            break;
        }
    }
    return !computerAlive;
}
#endregion

#region Board "arithmetic"
/// <summary>
/// Get the Coordinates of a jumped piece
/// </summary>
(int x, int y) GetJumpedPiece((int x, int y) from, (int x, int y) to)
{
    var midX = (to.x + from.x) / 2;
    var midY = (to.y + from.y) / 2;
    return (midX, midY);
}
/// <summary>
/// Apply a directional vector "direction" to location "from"
/// return resulting location
/// direction will contain: (-1,-1), (-1, 1), ( 1,-1), ( 1, 1)
/// /// </summary>
(int x, int y) GetLocation((int x , int y) from, (int x, int y) direction)
{
    return (x: from.x + direction.x, y: from.y + direction.y);
}
#endregion

#region State change functions
/// <summary>
/// Alter current "state" by moving a piece from "from" to "to"
/// This method does not verify that the move being made is valid
/// This method works for both player moves and computer moves
/// </summary>
int[,] ApplyMove(int[,] state, (int x, int y) from, (int x, int y) to)
{
    state[to.x, to.y] = state[from.x, from.y];
    state[from.x, from.y] = 0;

    if (IsJumpMove(from, to))
    {
        // a jump was made
        // remove the jumped piece from the board
        var jump = GetJumpedPiece(from, to);
        state[jump.x, jump.y] = 0;
    }
    return state;
}
/// <summary>
/// At the end of a turn (either player or computer) check to see if any pieces
/// reached the final row.  If so, change them to kings (crown)
/// </summary>
int[,] CrownKingPieces(int[,] state)
{
    for (int x = 0; x < 8; x++)
    {
        // check the bottom row if computer has a piece in it
        if (state[x, 0] == -1)
        {
            state[x, 0] = -2;
        }
        // check the top row if the player has a piece in it
        if (state[x, 7] == 1)
        {
            state[x, 7] = 2;
        }
    }
    return state;
}
#endregion

#region Computer Logic
/// <summary>
/// Given a current location "from", determine if a move exists in a given vector, "direction"
/// direction will contain: (-1,-1), (-1, 1), ( 1,-1), ( 1, 1)
/// return "null" if no move is possible in this direction
/// </summary>
(int x, int y)? GetCandidateMove(int[,] state, (int x, int y) from, (int x, int y) direction)
{
    var to = GetLocation(from, direction);
    if (IsOutOfBounds(to))
        return null;
    if (state[to.x, to.y] > 0)
    {
        // potential jump
        to = GetLocation(to, direction);
        if (IsOutOfBounds(to))
            return null;
    }
    if (state[to.x, to.y] != 0)
        // space already occupied by another piece
        return null;
    
    return to;
}
/// <summary>
/// Calculate a rank for a given potential move
/// The higher the rank value, the better the move is considered to be
/// </summary>
int RankMove(int[,] state, (int x, int y) from, (int x, int y) to)
{
    int rank = 0;

    if (to.y == 0 && state[from.x, from.y] == -1)
    {
        // getting a king
        rank += 2;
    }
    if (IsJumpMove(from, to))
    {
        // making a jump
        rank += 5;
    }
    if (from.y == 7)
    {
        // leaving home row
        rank -= 2;
    }
    if (to.x == 0 || to.x == 7)
    {
        // move to edge of board
        rank += 1;
    }
    // look to the row in front of the potential destination for 
    for (int c = -1; c <=1; c+=2)
    {
        var inFront = GetLocation(to, (c, -1));
        if (IsOutOfBounds(inFront))
            continue;
        if (state[inFront.x, inFront.y] < 0)
        {
            // protected by our piece in front
            rank++;
            continue;
        }
        var inBack = GetLocation(to, (-c, 1));
        if (IsOutOfBounds(inBack))
        {
            continue;
        }
        if ((state[inFront.x, inFront.y] > 0) && 
            (state[inBack.x, inBack.y] == 0) || (inBack == from))
        {
            // the player can jump us
            rank -= 2;
        }
    }
    return rank;
};

/// <summary>
/// Returns an enumeration of possible moves that can be made by the given piece "from"
/// If no moves, can be made, the enumeration will be empty
/// </summary>
IEnumerable<(int x, int y)> GetPossibleMoves(int[,] state, (int x, int y) from)
{
    int maxB;
    switch (state[from.x, from.y])
    {
        case -2:
            // kings can go backwards too
            maxB = 1;
            break;
        case -1:
            maxB = -1;
            break;
        default:
            // not one of our pieces
            yield break;
    }

    for (int a = -1; a <= 1; a += 2)
    {
        // a
        // -1 = left
        // +1 = right
        for (int b = -1; b <= maxB; b += 2)
        {
            // b
            // -1 = forwards
            // +1 = backwards (only kings allowed to make this move)
            var to = GetCandidateMove(state, from, (a, b));
            if (to == null)
            {
                // no valid move in this direction
                continue;
            }
            yield return to.Value;
        }
    }
}
/// <summary>
/// Determine the best move from a list of candidate moves "possibleMoves"
/// Returns "null" if no move can be made
/// </summary>
((int x, int y) from, (int x, int y) to)? GetBestMove(int[,] state, IEnumerable<((int x, int y) from, (int x, int y) to)> possibleMoves)
{
    int? bestRank = null;
    ((int x, int y) from, (int x, int y) to)? bestMove = null;

    foreach (var move in possibleMoves)
    {
        int rank = RankMove(state, move.from, move.to);

        if (bestRank == null || rank > bestRank)
        {
            bestRank = rank;
            bestMove = move;
        }
    }

    return bestMove;
}

/// <summary>
/// Examine the entire board and record all possible moves
/// Return the best move found, if one exists
/// Returns "null" if no move found
/// </summary>
((int x, int y) from, (int x, int y) to)? CalculateMove(int[,] state)
{
    var possibleMoves = new List<((int x, int y) from, (int x, int y) to)>(); 
    for (int x = 0; x < 8; x++)
    {
        for (int y = 0; y < 8; y++)
        {
            var from = (x, y);
            foreach (var to in GetPossibleMoves(state, from))
            {
                possibleMoves.Add((from, to));
            }
        }
    }
    var bestMove = GetBestMove(state, possibleMoves);
    return bestMove;
}

/// <summary>
/// The logic behind the Computer's turn
/// Look for valid moves and possible subsequent moves
/// </summary>
(bool moveMade, int[,] state) ComputerTurn(int[,] state)
{
    // Get best move available
    var move = CalculateMove(state);
    if (move == null)
    {
        // No move can be made
        return (false, state);
    }
    var from = move.Value.from;
    Console.Write($"FROM {from.x} {from.y} ");
    // Continue to make moves until no more valid moves can be made
    while (move != null)
    {
        var to = move.Value.to;
        Console.WriteLine($"TO {to.x} {to.y}");
        state = ApplyMove(state, from, to);
        if (!IsJumpMove(from, to))
            break;

        // check for double / triple / etc. jump
        var possibleMoves = new List<((int x, int y) from, (int x, int y) to)>();
        from = to;
        foreach (var candidate in GetPossibleMoves(state, from))
        {
            if (IsJumpMove(from, candidate))
            {
                possibleMoves.Add((from, candidate));
            }
        }
        // Get best jump move
        move = GetBestMove(state, possibleMoves);
    }
    // apply crowns to any new Kings
    state = CrownKingPieces(state);
    return (true, state);
}
#endregion

#region Player Logic
/// <summary>
/// Get input from the player in the form "x,y" where x and y are integers
/// If invalid input is received, return null
/// If input is valid, return the coordinate of the location
/// </summary>
(int x, int y)? GetCoordinate(string prompt)
{
    Console.Write(prompt + "? ");
    var input = Console.ReadLine();
    // split the string into multiple parts
    var parts = input?.Split(",");
    if (parts?.Length != 2)
        // must be exactly 2 parts
        return null;
    int x;
    if (!int.TryParse(parts[0], out x))
        // first part is not a number
        return null;
    int y;
    if (!int.TryParse(parts[1], out y))
        //second part is not a number
        return null;

    return (x, y);
}

/// <summary>
/// Get the move from the player.
/// return a tuple of "from" and "to" representing a valid move
/// 
/// </summary>
((int x, int y) from, (int x,int y) to) GetPlayerMove(int[,] state)
{
    // The original program has some issues regarding user input
    // 1)  There are minimal data sanity checks in the original:
    //     a)  FROM piece must be owned by player
    //     b)  TO location must be empty
    //     c)  the FROM and TO x's must be less than 2 squares away
    //     d)  the FROM and TO y's must be same distance as x's
    //     No checks are made for direction, if a jump is valid, or
    //     if the piece even moves.
    // 2)  Once a valid FROM is selected, a TO must be selected.
    //     If there are no valid TO locations, you are soft-locked
    // This approach is intentionally different from the original
    // but maintains the original intent as much as possible
    // 1)  Select a FROM location
    // 2)  If FROM is invalid, return to step 1
    // 3)  Select a TO location
    // 4)  If TO is invalid or the implied move is invalid,
    //     return to step 1
    
    
    // There is still currently no way for the player to indicate that no move can be made
    // This matches the original logic, but is a candidate for a refactor

    do
    {
        var from = GetCoordinate("FROM");
        if ((from != null)
            && !IsOutOfBounds(from.Value)
            && (state[from.Value.x, from.Value.y] > 0))
        {
            // we have a valid "from" location
            var to = GetCoordinate("TO");
            if ((to != null)
                && !IsOutOfBounds(to.Value)
                && IsValidPlayerMove(state, from.Value, to.Value))
            {
                // we have a valid "to" location
                return (from.Value, to.Value);
            }
        }
    } while (true);
}

/// <summary>
/// Get a subsequent jump from the player if they can / want to
/// returns a move ("from", "to") if a player jumps
/// returns null if a player does not make another move
/// The player must input negative numbers for the coordinates to indicate
/// that no more moves are to be made.  This matches the original implementation
/// </summary>
((int x, int y) from, (int x, int y) to)? GetPlayerSubsequentJump(int[,] state, (int x, int y) from)
{
    do
    {
        var to = GetCoordinate("+TO");
        if ((to != null)
            && !IsOutOfBounds(to.Value)
            && IsValidPlayerMove(state, from, to.Value)
            && IsJumpMove(from, to.Value))
        {
            // we have a valid "to" location
            return (from, to.Value); ;
        }

        if (to != null && to.Value.x < 0 && to.Value.y < 0)
        {
            // player has indicated to not make any more moves
            return null;
        }
    }
    while (true);
}

/// <summary>
/// The logic behind the Player's turn
/// Get the player input for a move
/// Get subsequent jumps, if possible
/// </summary>
int [,] PlayerTurn(int[,] state)
{
    var move = GetPlayerMove(state);
    do
    {
        state = ApplyMove(state, move.from, move.to);
        if (!IsJumpMove(move.from, move.to))
        {
            // If player doesn't make a jump move, no further moves are possible
            break;
        }
        var nextMove = GetPlayerSubsequentJump(state, move.to);
        if (nextMove == null)
        {
            // another jump is not made
            break;
        }
        move = nextMove.Value;
    }
    while (true);
    // check to see if any kings need crowning
    state = CrownKingPieces(state);
    return state;
}
#endregion

/*****************************************************************************
 * 
 * Main program starts here
 * 
 ****************************************************************************/

WriteIntroduction();

// initalize state -  empty spots initialize to 0
// set player pieces to 1, computer pieces to -1
// turn your head to the right to visualize the board.
// kings will be represented by -2 (for computer) and 2 (for player)
int[,] state = new int[8, 8] { 
    { 1, 0, 1, 0, 0, 0,-1, 0 },
    { 0, 1, 0, 0, 0,-1, 0,-1 },
    { 1, 0, 1, 0, 0, 0,-1, 0 }, 
    { 0, 1, 0, 0, 0,-1, 0,-1 },
    { 1, 0, 1, 0, 0, 0,-1, 0 },
    { 0, 1, 0, 0, 0,-1, 0,-1 },
    { 1, 0, 1, 0, 0, 0,-1, 0 }, 
    { 0, 1, 0, 0, 0,-1, 0,-1 },
};

while (true)
{
    bool moveMade;
    (moveMade, state) = ComputerTurn(state);
    if (!moveMade)
    {
        // In the original program the computer wins if it cannot make a move
        // I believe the player should win in this case, assuming the player can make a move.
        // if neither player can make a move, the game should be draw.
        // I have left it as the original logic for now.
        ComputerWins();
        break;
    }
    PrintBoard(state);
    if (CheckForComputerWin(state))
    {
        ComputerWins();
        break;
    }
    state = PlayerTurn(state);
    if (CheckForPlayerWin(state))
    {
        PlayerWins();
        break;
    }
}
