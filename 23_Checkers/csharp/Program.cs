const int LineLength = 80;
Dictionary<int, string> Pieces = new Dictionary<int, string>()
{
    { -2, "X*" },
    { -1, "X " },
    {  0, ". " },
    {  1, "O " },
    {  2, "O*" },
};

void PrintBoard(int[,] state)
{
    SkipLines(3);
    for (int y = 7; y >= 0; y--)
    {
        for (int x = 0; x < 8; x++)
        {
            Console.Write(Pieces[state[x, y]]);
            Console.Write("   ");
        }
        Console.WriteLine();
    }
}

void WriteCenter(string text)
{
    var spaces = (LineLength - text.Length) / 2;
    Console.WriteLine($"{"".PadLeft(spaces)}{text}");
}

void SkipLines(int count)
{
    for (int i = 0; i < count; i++)
    {
        Console.WriteLine();
    }
}

bool IsPointOutOfBounds(int x)
{
    return x < 0 || x > 7;
}

bool IsOutOfBounds((int x, int y) position)
{
    return IsPointOutOfBounds(position.x) || IsPointOutOfBounds(position.y);
}

(int x, int y)? GetCandidateMove(int[,] state, (int x, int y) from, (int x, int y) direction)
{
    var to = (x: from.x + direction.x, y: from.y + direction.y);
    if (IsOutOfBounds(to))
        return null;
    if (state[to.x, to.y] > 0)
    {
        // potential jump
        to = (x: to.x + direction.x, y: to.y + direction.y);
        if (IsOutOfBounds(to))
            return null;
    }
    if (state[to.x, to.y] != 0)
        // space already occupied by another piece
        return null;
    
    return to;
}
bool IsJumpMove((int x, int y) from, (int x, int y) to)
{
    return Math.Abs(from.y - to.y) == 2;
}

int AnalyzeMove(int[,] state, (int x, int y) from, (int x, int y) to)
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
        // move to edge
        rank += 1;
    }
    for (int c = -1; c <=1; c++)
    {
        var inFront = (x: to.x + c, y: to.y - 1);
        if (IsOutOfBounds(inFront))
            continue;
        if (state[inFront.x, inFront.y] < 0)
        {
            // protected by our piece in front
            rank++;
            continue;
        }
        var inBack = (x: to.x - c, y: to.y + 1);
        if (IsOutOfBounds(inBack))
        {
            continue;
        }
        if (inBack == from || 
            (state[inFront.x, inFront.y] > 0 && state[inBack.x, inBack.y] == 0))
        {
            // the player can jump us
            rank -= 2;
        }
    }
    return rank;
};

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

((int x, int y) from, (int x, int y) to)? GetBestMove(int[,] state, IEnumerable<((int x, int y) from, (int x, int y) to)> possibleMoves)
{
    int? bestRank = null;
    ((int x, int y) from, (int x, int y) to)? bestMove = null;

    foreach (var move in possibleMoves)
    {
        int rank = AnalyzeMove(state, move.from, move.to);

        if (rank > bestRank)
        {
            bestRank = rank;
            bestMove = move;
        }
    }

    return bestMove;
}

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
(int x, int y) GetJumpedPiece((int x, int y) from, (int x, int y) to)
{
    var midX = (to.x + from.x) / 2;
    var midY = (to.y + from.y) / 2;
    return (midX, midY);
}
int[,] ApplyMove(int[,] state, (int x, int y) from, (int x, int y) to)
{
    state[to.x, to.y] = state[from.x, from.y];
    state[from.x, from.y] = 0;
    if (  (to.y == 0 && state[to.x, to.y] == -1)
        ||(to.y == 7 && state[to.x, to.y] == 1))
    {
        // make the piece a king
        state[to.x, to.y] *= 2;
    }

    if (IsJumpMove(from, to))
    {
        // a jump was made
        // remove the jumped piece from the board
        var jump = GetJumpedPiece(from, to);
        state[jump.x, jump.y] = 0;
    }
    return state;
}

(bool moveMade, int[,] state) ComputerTurn(int[,] state)
{
    var move = CalculateMove(state);
    if (move == null)
    {
        // No move can be made
        return (false, state);
    }
    Console.Write($"FROM {move.Value.from.x} {move.Value.from.y} ");
    while (move != null)
    {
        Console.WriteLine($"TO {move.Value.to.x} {move.Value.to.y}");
        state = ApplyMove(state, move.Value.from, move.Value.to);
        if (IsJumpMove(move.Value.from, move.Value.to))
        {
            // check for double / triple / etc. jump
            var possibleMoves = new List<((int x, int y) from, (int x, int y) to)>();
            var from = move.Value.to;
            foreach (var to in GetPossibleMoves(state, from))
            {
                if (IsJumpMove(from, to))
                {
                    possibleMoves.Add((from, to));
                }
            }
            move = GetBestMove(state, possibleMoves);
        }
    }
    return (true, state);
}

(int x, int y)? GetCoordinate(string prompt)
{
    Console.Write(prompt + "? ");
    var input = Console.ReadLine();
    var parts = input.Split(",");
    if (parts.Length != 2)
        return null;
    int x;
    if (!int.TryParse(parts[0], out x))
        return null;
    int y;
    if (!int.TryParse(parts[1], out y))
        return null;

    return (x, y);
}

bool IsValidMove(int[,] state, (int x, int y) from, (int x, int y) to)
{
    if (state[to.x, to.y] != 0)
    {
        return false;
    }
    var deltaX = Math.Abs(to.x - from.x);
    var deltaY = Math.Abs(to.y - from.y);
    if (deltaX != 1 || deltaX != 2)
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

int [,] PlayerTurn(int[,] state)
{
    // The original program has some issues regarding user input
    // 1)  There is minimal data sanity checks
    //     a)  FROM piece must be owned by player
    //     b)  TO location must be empty
    //     c)  the FROM and TO x's must be less than 2 squares away
    //     d)  the FROM and TO y's must be same distance as x's
    //     No checks are made for direction, if a jump is valid, or
    //     if the piece even moves.
    // 2)  Once a valid FROM is selected, a TO must be selected.
    //     If there are no valid TO locations, you are soft-locked
    // This approach is intentionally different
    // 1)  Select a FROM location
    // 2)  If FROM is invalid, return to step 1
    // 3)  Select a TO location
    // 4)  If TO is invalid or the implied move is invalid,
    //     return to step 1
    
    (int x, int y)? from = null;
    (int x, int y)? to = null;
    var valid = false;
    do
    {
        from = GetCoordinate("FROM");
        if ((from != null)
            && !IsOutOfBounds(from.Value)
            && (state[from.Value.x, from.Value.y] > 0))
        {
            to = GetCoordinate("TO");
            if ((to != null)
                && !IsOutOfBounds(to.Value)
                && IsValidMove(state, from.Value, to.Value))
            {
                valid = true;
            }
        }
    } while (!valid);
    bool jumping = false;
    do
    {
        state = ApplyMove(state, from.Value, to.Value);
        jumping = IsJumpMove(from.Value, to.Value);
        if (jumping)
        {
            from = to;
            valid = false;
            do
            {
                to = GetCoordinate("+TO");
                if ((to != null)
                    && !IsOutOfBounds(to.Value)
                    && IsValidMove(state, from.Value, to.Value)
                    && IsJumpMove(from.Value, to.Value))
                {
                    valid = true;
                }

                if (to != null && to.Value.x < 0 && to.Value.y < 0)
                {
                    jumping = false;
                    break;
                }
            }
            while (!valid);
        }
    }
    while (jumping);
    return state;
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

void ComputerWins()
{
    Console.WriteLine("I WIN.");
}
void PlayerWins()
{
    Console.WriteLine("YOU WIN.");
}

// Main program starts here

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

// initalize state -  empty spots initialize to 0
// set player pieces to 1, computer pieces to -1
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
        // in the original program the computer wins if it cannot make a move
        // I believe the player should win in this case, assuming the player can make a move
        // if neither player can make a move, the game should be draw.
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
