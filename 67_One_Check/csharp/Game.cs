namespace OneCheck;

internal class Game
{
    private readonly IReadWrite _io;

    public Game(IReadWrite io)
    {
        _io = io;
    }

    public void Play()
    {
        _io.Write(Streams.Introduction);
        
        do
        {
            var board = new Board();
            do
            {
                _io.WriteLine(board);
                _io.WriteLine();
            } while (board.PlayMove(_io));

            _io.WriteLine(board.GetReport());
        } while (_io.ReadYesNo(Prompts.TryAgain) == "yes");

        _io.Write(Streams.Bye);
    }
}

internal static class IOExtensions
{
    internal static string ReadYesNo(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var response = io.ReadString(prompt).ToLower();

            if (response == "yes" || response == "no") { return response; }

            io.Write(Streams.YesOrNo);
        }
    }
}
