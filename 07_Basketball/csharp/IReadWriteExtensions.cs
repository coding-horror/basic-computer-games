using Games.Common.IO;

namespace Basketball;

internal static class IReadWriteExtensions
{
    public static float ReadDefense(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var defense = io.ReadNumber(prompt);
            if (defense >= 6) { return defense; }
        }
    }

    private static bool TryReadInteger(this IReadWrite io, string prompt, out int value)
    {
        var floatValue = io.ReadNumber(prompt);
        value = (int)floatValue;
        return value == floatValue;
    }

    public static Shot? ReadShot(this IReadWrite io, string prompt)
    {
        while (true)
        {
            if (io.TryReadInteger(prompt, out var value) && Shot.TryGet(value, out var shot))
            {
                return shot;
            }
            io.Write("Incorrect answer.  Retype it. ");
        }
    }
}
