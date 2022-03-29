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

    public static int ReadShot(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var shot = io.ReadNumber(prompt);
            if ((int)shot == shot && shot >= 0 && shot <= 4) { return (int)shot; }
            io.Write("Incorrect answer.  Retype it. ");
        }
    }

    public static void WriteScore(this IReadWrite io, string format, string opponent, Dictionary<string ,int> score) =>
        io.WriteLine(format, "Dartmouth", score["Dartmouth"], opponent, score[opponent]);
}