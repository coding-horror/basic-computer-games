using Awari;

Console.WriteLine(Tab(34) + "AWARI");
Console.WriteLine(Tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");

Game game = new();

while (true)
{
    game.Reset();
    DisplayGame();

    while (game.State != GameState.Done)
    {
        switch (game.State)
        {
            case GameState.PlayerMove:
                PlayerMove(second: false);
                break;
            case GameState.PlayerSecondMove:
                PlayerMove(second: true);
                break;
            case GameState.ComputerMove:
                ComputerTurn();
                break;
        }

        DisplayGame();
    }

    var outcome = game.GetOutcome();

    string outcomeLabel =
        outcome.Winner switch
        {
            GameWinner.Computer => $"I WIN BY {outcome.Difference} POINTS",
            GameWinner.Draw => "DRAWN GAME",
            GameWinner.Player => $"YOU WIN BY {outcome.Difference} POINTS",
            _ => throw new InvalidOperationException($"Unexpected winner {outcome.Winner}."),
        };
    Console.WriteLine(outcomeLabel);
    Console.WriteLine();
}

void DisplayGame()
{
    // display the computer's pits
    Console.Write("   ");
    foreach (var pit in game.ComputerPits.Reverse())
        Console.Write($"{pit,2} ");
    Console.WriteLine();

    // display both homes
    Console.WriteLine($"{game.ComputerHome,2}{Tab(19)}{game.PlayerHome,2}");

    // display the player's pits
    Console.Write("   ");
    foreach (var pit in game.PlayerPits)
        Console.Write($"{pit,2} ");
    Console.WriteLine();

    Console.WriteLine();
}

void PlayerMove(bool second = false)
{
    int move = GetMove(second);
    game.PlayerMove(move);
}

int GetMove(bool second)
{
    string prompt = second ? "AGAIN? " : "YOUR MOVE? ";

    while (true)
    {
        Console.Write(prompt);

        string input = Console.ReadLine() ?? "";

        // input must be a number between 1 and 6, and the pit must have > 0 beans
        if (int.TryParse(input, out int move)
         && game.IsLegalPlayerMove(move))
            return move;

        Console.WriteLine("ILLEGAL MOVE");
    }
}

void ComputerTurn()
{
    var moves = game.ComputerTurn();
    string movesString = string.Join(",", moves);

    Console.WriteLine($"MY MOVE IS {movesString}");
}

string Tab(int n) => new(' ', n);