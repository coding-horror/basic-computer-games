namespace Queen;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private readonly Computer _computer;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
        _computer = new Computer(random);
    }

    internal void PlaySeries()
    {
        _io.Write(Streams.Title);
        if (_io.ReadYesNo(Prompts.Instructions)) { _io.Write(Streams.Instructions); }

        while (true)
        {
            var result = PlayGame();
            _io.Write(result switch
            {
                Result.HumanForfeits => Streams.Forfeit,
                Result.HumanWins => Streams.Congratulations,
                Result.ComputerWins => Streams.IWin,
                _ => throw new InvalidOperationException($"Unexpected result {result}")
            });

            if (!_io.ReadYesNo(Prompts.Anyone)) { break; }
        }

        _io.Write(Streams.Thanks);
    }

    private Result PlayGame()
    {
        _io.Write(Streams.Board);
        var humanPosition = _io.ReadPosition(Prompts.Start, p => p.IsStart, Streams.IllegalStart, repeatPrompt: true);
        if (humanPosition.IsZero) { return Result.HumanForfeits; }

        while (true)
        {
            var computerPosition = _computer.GetMove(humanPosition);
            _io.Write(Strings.ComputerMove(computerPosition));
            if (computerPosition.IsEnd) { return Result.ComputerWins; }

            humanPosition = _io.ReadPosition(Prompts.Move, p => (p - computerPosition).IsValid, Streams.IllegalMove);
            if (humanPosition.IsZero) { return Result.HumanForfeits; }
            if (humanPosition.IsEnd) { return Result.HumanWins; }
        }
    }

    private enum Result { ComputerWins, HumanWins, HumanForfeits };
}
