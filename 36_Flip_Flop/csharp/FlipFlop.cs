// Flip Flop Game

PrintGameInfo();

bool startNewGame = true;

string[] board = new string[] { "X", "X", "X", "X", "X", "X", "X", "X", "X", "X" };

do
{
    int stepsCount = 0;
    int lastMove = -1;
    int moveIndex;
    int gameSum;
    double gameEntropyRate = Rnd();
    bool toPlay = false;
    bool setNewBoard = true;

    Print();
    Print("HERE IS THE STARTING LINE OF X'S.");
    Print();

    do
    {
        bool illegalEntry;
        bool equalToLastMove;

        if (setNewBoard)
        {
            PrintNewBoard();
            board = new string[] { "X", "X", "X", "X", "X", "X", "X", "X", "X", "X" };
            setNewBoard = false;
            toPlay = true;
        }

        stepsCount++;
        gameSum = 0;

        // Read User's move
        do
        {
            Write("INPUT THE NUMBER? ");
            var input = Console.ReadLine();
            illegalEntry = !int.TryParse(input, out moveIndex);

            if (illegalEntry || moveIndex > 11)
            {
                illegalEntry = true;
                Print("ILLEGAL ENTRY--TRY AGAIN.");
            }
        }
        while (illegalEntry);

        if (moveIndex == 11)
        {
            // Run new game, To start a new game at any point
            toPlay = false;
            stepsCount = 12;
            startNewGame = true;
        }

        
        if (moveIndex == 0)
        {
            // To reset the line to all X, same game
            setNewBoard = true;
            toPlay = false;
        }

        if (toPlay)
        {
            board[moveIndex - 1] = board[moveIndex - 1] == "O" ? "X" : "O";

            if (lastMove == moveIndex)
            {
                equalToLastMove = true;
            }
            else
            {
                equalToLastMove = false;
                lastMove = moveIndex;
            }

            do
            {
                moveIndex = equalToLastMove 
                    ? GetMoveIndexWhenEqualeLastMove(moveIndex, gameEntropyRate) 
                    : GetMoveIndex(moveIndex, gameEntropyRate);

                board[moveIndex] = board[moveIndex] == "O" ? "X" : "O";
            }
            while (lastMove == moveIndex && board[moveIndex] == "X");

            PrintGameBoard(board);

            foreach (var item in board)
            {
                if (item == "O")
                {
                    gameSum++;
                }
            }
        }
    }
    while (stepsCount < 12 && gameSum < 10);

    if (toPlay)
    {
        PrintGameResult(gameSum, stepsCount);

        Write("DO YOU WANT TO TRY ANOTHER PUZZLE ");

        var toContinue = Console.ReadLine();

        if (!string.IsNullOrEmpty(toContinue) && toContinue?.ToUpper()[0] == 'N')
        {
            startNewGame = false;
        }

        Print();
    }
}
while (startNewGame);

void Print(string str = "") => Console.WriteLine(str);

void Write(string value) => Console.Write(value);

string Tab(int pos) => new(' ', pos);

double Rnd() => new Random().NextDouble();

int GetMoveIndex(int moveIndex, double gameEntropyRate)
{
    double rate = Math.Tan(gameEntropyRate + moveIndex / gameEntropyRate - moveIndex) - Math.Sin(gameEntropyRate / moveIndex) + 336 * Math.Sin(8 * moveIndex);
    return Convert.ToInt32(Math.Floor(10 * (rate - Math.Floor(rate))));
}

int GetMoveIndexWhenEqualeLastMove(int moveIndex, double gameEntropyRate)
{
    double rate = 0.592 * (1 / Math.Tan(gameEntropyRate / moveIndex + gameEntropyRate)) / Math.Sin(moveIndex * 2 + gameEntropyRate) - Math.Cos(moveIndex);
    return Convert.ToInt32(Math.Floor(10 * (rate - Math.Floor(rate))));
}

void PrintNewBoard()
{
    Print("1 2 3 4 5 6 7 8 9 10");
    Print("X X X X X X X X X X");
    Print();
}

void PrintGameBoard(string[] board)
{
    Print("1 2 3 4 5 6 7 8 9 10");

    foreach (var item in board)
    {
        Write($"{item} ");
    }

    Print();
    Print();
}

void PrintGameResult(int gameSum, int stepsCount)
{
    if (gameSum == 10)
    {
        Print($"VERY GOOD.  YOU GUESSED IT IN ONLY {stepsCount} GUESSES.");
    }
    else
    {
        Print($"TRY HARDER NEXT TIME.  IT TOOK YOU {stepsCount} GUESSES.");
    }
}

void PrintGameInfo()
{
    Print(Tab(32) + "FLIPFLOP");
    Print(Tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    Print();
    Print("THE OBJECT OF THIS PUZZLE IS TO CHANGE THIS:");
    Print();

    Print("X X X X X X X X X X");
    Print();
    Print("TO THIS:");
    Print();
    Print("O O O O O O O O O O");
    Print();

    Print("BY TYPING THE NUMBER CORRESPONDING TO THE POSITION OF THE");
    Print("LETTER ON SOME NUMBERS, ONE POSITION WILL CHANGE, ON");
    Print("OTHERS, TWO WILL CHANGE.  TO RESET LINE TO ALL X'S, TYPE 0");
    Print("(ZERO) AND TO START OVER IN THE MIDDLE OF A GAME, TYPE ");
    Print("11 (ELEVEN).");
}