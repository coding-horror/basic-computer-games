using Chief.Resources;
using static Chief.Resources.Resource;

namespace Chief;

internal class Game
{
    private readonly IReadWrite _io;

    public Game(IReadWrite io)
    {
        _io = io;
    }

    internal void Play()
    {
        DoIntroduction();

        var result = _io.ReadNumber(Prompts.Answer);

        if (_io.ReadYes(Formats.Bet, Math.CalculateOriginal(result)))
        {
            _io.Write(Streams.Bye);
            return;
        }

        var original = _io.ReadNumber(Prompts.Original);

        _io.WriteLine(Math.ShowWorking(original));

        if (_io.ReadYes(Prompts.Believe))
        {
            _io.Write(Streams.Bye);
            return;
        }

        _io.Write(Streams.Lightning);
    }

    private void DoIntroduction()
    {
        _io.Write(Streams.Title);
        if (!_io.ReadYes(Prompts.Ready))
        {
            _io.Write(Streams.ShutUp);
        }

        _io.Write(Streams.Instructions);
    }
}
