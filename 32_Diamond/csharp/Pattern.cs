using System.Text;
using static Diamond.Resources.Resource;

namespace Diamond;

internal class Pattern
{
    private readonly IReadWrite _io;

    public Pattern(IReadWrite io)
    {
        _io = io;
        io.Write(Streams.Introduction);
    }

    public void Draw()
    {
        var diamondSize = _io.ReadNumber(Prompts.TypeNumber);
        _io.WriteLine();

        var diamondCount = (int)(60 / diamondSize);

        var diamondLines = new List<string>(GetDiamondLines(diamondSize)).AsReadOnly();

        for (int patternRow = 0; patternRow < diamondCount; patternRow++)
        {
            for (int diamondRow = 0; diamondRow < diamondLines.Count; diamondRow++)
            {
                var line = new StringBuilder();
                for (int patternColumn = 0; patternColumn < diamondCount; patternColumn++)
                {
                    line.PadToLength((int)(patternColumn * diamondSize)).Append(diamondLines[diamondRow]);
                }
                _io.WriteLine(line);
            }
        }
    }

    public static IEnumerable<string> GetDiamondLines(float size)
    {
        for (var i = 1; i <= size; i += 2)
        {
            yield return GetLine(i);
        }

        for (var i = size - 2; i >= 1; i -= 2)
        {
            yield return GetLine(i);
        }

        string GetLine(float i) =>
            string.Concat(
                new string(' ', (int)(size - i) / 2),
                new string('C', Math.Min((int)i, 2)),
                new string('!', Math.Max(0, (int)i - 2)));
    }
}
