using Bug.Resources;
using Games.Common.IO;
using Games.Common.Randomness;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    public void Play()
    {
        _io.WriteLine(Resource.Streams.Introduction);
        var response = _io.ReadString("Do you want instructions");
        if (!response.Equals("no", StringComparison.InvariantCultureIgnoreCase))
        {
            _io.WriteLine(Resource.Streams.Instructions);
        }
    }
}