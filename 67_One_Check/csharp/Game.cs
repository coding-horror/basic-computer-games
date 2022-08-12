namespace OneCheck;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly Board _board;

    public Game(IReadWrite io)
    {
        _io = io;
        _board = new Board();
    }

    public void Play()
    {
        _io.Write(Streams.Introduction);
        
        _io.WriteLine(_board);
    }
}
