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
        _io.Write(Title);

        var reign = SetUpReign();
        if (reign != null)
        {
            while (reign.PlayYear());
        }

        _io.WriteLine();
        _io.WriteLine();
    }

    private Reign? SetUpReign()
    {
        var response = _io.ReadString(InstructionsPrompt).ToUpper();

        if (response.Equals("Again", StringComparison.InvariantCultureIgnoreCase))
        {
            return _io.TryReadGameData(_random, out var reign) ? reign : null;
        }
        
        if (!response.StartsWith("N", StringComparison.InvariantCultureIgnoreCase))
        {
            _io.Write(InstructionsText(TermOfOffice));
        }

        _io.WriteLine();
        return new Reign(_io, _random);
    }
}
