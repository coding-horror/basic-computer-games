namespace Roulette;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private readonly Table _table;
    private readonly Croupier _croupier;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
        _croupier = new();
        _table = new(_croupier, io, random);
    }

    public void Play()
    {
        _io.Write(Streams.Title);
        if (!_io.ReadString(Prompts.Instructions).ToLowerInvariant().StartsWith('n'))
        {
            _io.Write(Streams.Instructions);
        }

        while (_table.Play());

        if (_croupier.PlayerIsBroke)
        {
            _io.Write(Streams.LastDollar);
            _io.Write(Streams.Thanks);
            return;
        }

        if (_croupier.HouseIsBroke)
        {
            _io.Write(Streams.BrokeHouse);
        }

        _croupier.CutCheck(_io, _random);
    }
}
