namespace Chomp;

internal class Game
{
    private readonly IReadWrite _io;

    public Game(IReadWrite io)
    {
        _io = io;
    }

    internal void Play()
    {
        _io.Write(Resource.Streams.Introduction);
        if (_io.ReadNumber("Do you want the rules (1=Yes, 0=No!)") != 0)
        {
            _io.Write(Resource.Streams.Rules);
        }


        while (true)
        {
            _io.Write(Resource.Streams.HereWeGo);

            var (playerCount, rowCount, columnCount) = _io.ReadParameters();

            if (_io.ReadNumber("Again (1=Yes, 0=No!)") != 1) { break; }
        }
    }
}
