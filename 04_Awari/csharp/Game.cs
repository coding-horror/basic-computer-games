namespace Awari;

public class Game
{
    public int[] PlayerPits => _beans[0..6];
    public int[] ComputerPits => _beans[7..13];
    public int PlayerHome => _beans[_playerHome];
    public int ComputerHome => _beans[_computerHome];

    private bool IsDone =>
        PlayerPits.All(b => b == 0) // if all the player's pits are empty
     || ComputerPits.All(b => b == 0); // or if all the computer's pits are empty

    public GameState State { get; private set; }

    public void Reset()
    {
        State = GameState.PlayerMove;

        Array.Fill(_beans, _initialPitValue);
        _beans[_playerHome] = 0;
        _beans[_computerHome] = 0;

        _moveCount = 0;
        _notWonGameMoves[^1] = 0;
    }

    public bool IsLegalPlayerMove(int move) =>
        move is > 0 and < 7
     && _beans[move - 1] > 0; // arrays are zero-based, but moves are one-based

    public void PlayerMove(int move) => MoveAndRegister(move - 1, _playerHome);

    public List<int> ComputerTurn()
    {
        // keep a list of moves made by the computer in a single turn (1 or 2)
        List<int> moves = new();

        moves.Add(ComputerMove()); // ComputerMove() returns the move made

        // only if a second move is possible, do it
        if (State == GameState.ComputerSecondMove)
            moves.Add(ComputerMove());

        return moves;
    }

    public GameOutcome GetOutcome()
    {
        if (State != GameState.Done)
            throw new InvalidOperationException("Game is not yet done.");

        int difference = _beans[_playerHome] - _beans[_computerHome];
        var winner = difference switch
        {
            < 0 => GameWinner.Computer,
            0 => GameWinner.Draw,
            > 0 => GameWinner.Player,
        };

        return new GameOutcome(winner, Math.Abs(difference));
    }

    private void MoveAndRegister(int pit, int homePosition)
    {
        int lastMovedBean = Move(_beans, pit, homePosition);

        // encode moves by player and computer into a 'base 6' number
        // e.g. if the player moves 5, the computer moves 2, and the player moves 4,
        // that would be encoded as ((5 * 6) * 6) + (2 * 6) + 4 = 196
        if (pit > 6) pit -= 7;
        _moveCount++;
        if (_moveCount < 9)
            _notWonGameMoves[^1] = _notWonGameMoves[^1] * 6 + pit;

        // determine next state based on current state, whether the game's done, and whether the last moved bean moved
        // into the player's home position
        State = (State, IsDone, lastMovedBean == homePosition) switch
        {
            (_, true, _) => GameState.Done,
            (GameState.PlayerMove, _, true) => GameState.PlayerSecondMove,
            (GameState.PlayerMove, _, false) => GameState.ComputerMove,
            (GameState.PlayerSecondMove, _, _) => GameState.ComputerMove,
            (GameState.ComputerMove, _, true) => GameState.ComputerSecondMove,
            (GameState.ComputerMove, _, false) => GameState.PlayerMove,
            (GameState.ComputerSecondMove, _, _) => GameState.PlayerMove,
            _ => throw new InvalidOperationException("Unexpected game state"),
        };

        // do some bookkeeping if the game is done, but not won by the computer
        if (State == GameState.Done
         && _beans[_playerHome] >= _beans[_computerHome])
            // add an entry for the next game
            _notWonGameMoves.Add(0);
    }

    private static int Move(int[] beans, int pit, int homePosition)
    {
        int beansToMove = beans[pit];
        beans[pit] = 0;

        // add the beans that were in the pit to other pits, moving clockwise around the board
        for (; beansToMove >= 1; beansToMove--)
        {
            // wrap around if pit exceeds 13
            pit = (pit + 1) % 14;

            beans[pit]++;
        }

        if (beans[pit] == 1 // if the last bean was sown in an empty pit
         && pit is not _playerHome and not _computerHome // which is not either player's home
         && beans[12 - pit] != 0) // and the pit opposite is not empty
        {
            // move the last pit sown and the _beans in the pit opposite to the player's home
            beans[homePosition] = beans[homePosition] + beans[12 - pit] + 1;
            beans[pit] = 0;
            beans[12 - pit] = 0;
        }

        return pit;
    }

    private int ComputerMove()
    {
        int move = DetermineComputerMove();
        MoveAndRegister(move, homePosition: _computerHome);

        // the result is only used to return it to the application, so translate it from an array index (between 7 and
        // 12) to a pit number (between 1 and 6)
        return move - 6;
    }

    private int DetermineComputerMove()
    {
        int bestScore = -99;
        int move = 0;

        // for each of the computer's possible moves, simulate them to calculate a score and pick the best one
        for (int j = 7; j < 13; j++)
        {
            if (_beans[j] <= 0)
                continue;

            int score = SimulateMove(j);

            if (score >= bestScore)
            {
                move = j;
                bestScore = score;
            }
        }

        return move;
    }

    private int SimulateMove(int move)
    {
        // make a copy of the current state, so we can safely mess with it
        var hypotheticalBeans = new int[14];
        _beans.CopyTo(hypotheticalBeans, 0);

        // simulate the move in our copy
        Move(hypotheticalBeans, move, homePosition: _computerHome);

        // determine the 'best' move the player could make after this (best for them, not for the computer)
        int score = ScoreBestNextPlayerMove(hypotheticalBeans);

        // score this move by calculating how far ahead we would be after the move, and subtracting the player's next
        // move score
        score = hypotheticalBeans[_computerHome] - hypotheticalBeans[_playerHome] - score;

        // have we seen the current set of moves before in a drawn/lost game? after 8 moves it's unlikely we'll find any
        // matches, since games will have diverged. also we don't have space to store that many moves.
        if (_moveCount < 8)
        {
            int translatedMove = move - 7;  // translate from 7 through 12 to 0 through 5

            // if the first two moves in this game were 1 and 2, and this hypothetical third move would be a 3,
            // movesSoFar would be (1 * 36) + (2 * 6) + 3 = 51
            int movesSoFar = _notWonGameMoves[^1] * 6 + translatedMove;

            // since we store moves as a 'base 6' number, we need to divide stored moves by a power of 6
            // let's say we've a stored lost game where the moves were, in succession, 1 through 8, the value stored
            // would be:
            // 8 + (7 * 6) + (6 * 36) + (5 * 216) + (4 * 1296) + (3 * 7776) + (2 * 46656) + (1 * 279936) = 403106
            // to figure out the first three moves, we'd need to divide by 7776, resulting in 51.839...
            double divisor = Math.Pow(6.0, 7 - _moveCount);

            foreach (int previousGameMoves in _notWonGameMoves)
                // if this combination of moves so far ultimately resulted in a draw/loss, give it a lower score
                // note that this can happen multiple times
                if (movesSoFar == (int) (previousGameMoves / divisor + 0.1))
                    score -= 2;
        }

        return score;
    }

    private static int ScoreBestNextPlayerMove(int[] hypotheticalBeans)
    {
        int bestScore = 0;

        for (int i = 0; i < 6; i++)
        {
            if (hypotheticalBeans[i] <= 0)
                continue;

            int score = ScoreNextPlayerMove(hypotheticalBeans, i);

            if (score > bestScore)
                bestScore = score;
        }

        return bestScore;
    }

    private static int ScoreNextPlayerMove(int[] hypotheticalBeans, int move)
    {
        // figure out where the last bean will land
        int target = hypotheticalBeans[move] + move;
        int score = 0;

        // if it wraps around, that means the player is adding to his own pits, which is good
        if (target > 13)
        {
            // prevent overrunning the number of pits we have
            target %= 14;
            score = 1;
        }

        // if the player's move ends up in an empty pit, add the value of the pit on the opposite side to the score
        if (hypotheticalBeans[target] == 0 && target is not _playerHome and not _computerHome)
            score += hypotheticalBeans[12 - target];

        return score;
    }

    private const int _playerHome = 6;
    private const int _computerHome = 13;
    private const int _initialPitValue = 3;

    private readonly int[] _beans = new int[14];
    private readonly List<int> _notWonGameMoves = new() { 0 };    // not won means draw or lose
    private int _moveCount;
}

public enum GameState
{
    PlayerMove,
    PlayerSecondMove,
    ComputerMove,
    ComputerSecondMove,
    Done,
}

public enum GameWinner
{
    Player,
    Computer,
    Draw,
}

public record struct GameOutcome(GameWinner Winner, int Difference);