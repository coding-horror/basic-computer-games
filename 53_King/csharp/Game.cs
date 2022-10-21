namespace King;

internal class Game
{
    const int TermOfOffice = 8;

    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    public void Play()
    {
        _io.Write(Resource.Title);

        var response = _io.ReadString(Resource.Instructions_Prompt).ToUpper();
        if (!response.StartsWith('N'))
        {
            _io.Write(Resource.Instructions_Text(TermOfOffice));
        }

        _io.WriteLine();
    }
}