internal class Game
{
    private readonly IReadWrite _io;

    public Game(IReadWrite io)
    {
        _io = io;
    }

    public void Play()
    {
        _io.Write(Streams.Title);

        var life = new Life(_io);

        _io.Write(life.FirstGeneration);

        foreach (var generation in life)
        {
            _io.WriteLine();
            _io.Write(generation);
        }

        _io.WriteLine(life.Result ?? "No result");
    }
}
