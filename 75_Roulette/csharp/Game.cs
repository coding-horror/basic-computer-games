using System.Diagnostics.CodeAnalysis;

namespace Roulette;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;
    private readonly Table _table;
    private readonly Croupier _house;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
        _house = new();
        _table = new(_house, io, random);
    }

    public void Play()
    {
        _io.Write(Streams.Title);
        if (!_io.ReadString(Prompts.Instructions).ToLowerInvariant().StartsWith('n'))
        {
            _io.Write(Streams.Instructions);
        }

        while (_table.Play());

        if (!_house.PlayerIsBroke)
        {
            _house.CutCheck(_io, _random);
        }
        else
        {
            _io.Write(Streams.Thanks);
        }
    }
}
