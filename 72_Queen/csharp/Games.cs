namespace Queen;

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
        if (_io.ShouldDisplayInstructions()) { _io.Write(Streams.Instructions); }
    }
}

internal static class IOExtensions
{
    internal static bool ShouldDisplayInstructions(this IReadWrite io)
    {
        while (true)
        {
            var answer = io.ReadString(Prompts.Instructions).ToLower();
            if (answer == "yes") { return true; }
            if (answer == "no") { return false; }

            io.Write(Streams.YesOrNo);
        }
    }
}
