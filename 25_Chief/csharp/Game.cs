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
        _io.Write(Streams.Title);
        if (!_io.ReadYes(Prompts.Ready))
        {
            _io.Write(Streams.ShutUp);
        }

        _io.Write(Streams.Instructions);

        var result = _io.ReadNumber(Prompts.Answer);

        if (_io.ReadYes(Formats.Bet, (result + 1 - 5) * 5 / 8 * 5 - 3))
        {
            _io.Write(Streams.Bye);
            return;
        }

        var original = _io.ReadNumber(Prompts.Original);

        _io.WriteLine(Formats.Working, GetStepValues(original).ToArray());

        if (_io.ReadYes(Prompts.Believe))
        {
            _io.Write(Streams.Bye);
            return;
        }

        _io.Write(Streams.Lightning);
    }

    private static IEnumerable<object> GetStepValues(float value)
    {
        yield return value;
        yield return value += 3;
        yield return value /= 5;
        yield return value *= 8;
        yield return value = value / 5 + 5;
        yield return value - 1;
    }
}
