namespace Salvo;

internal class Game 
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    internal void Play()
    {
        _io.Write(Streams.Title);

        var turnHandler = new TurnHandler(_io, _random);
        _io.WriteLine();

        Winner? winner;
        do 
        {
            winner = turnHandler.PlayTurn();
        } while (winner == null);

        _io.Write(winner == Winner.Computer ? Streams.IWon : Streams.YouWon);
    }
}
