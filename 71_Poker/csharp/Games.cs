using Poker.Resources;

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
        _io.Write(Resource.Streams.Title);
        _io.Write(Resource.Streams.Instructions);
    }
}