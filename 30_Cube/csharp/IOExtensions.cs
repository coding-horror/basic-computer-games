namespace Cube;

internal static class IOExtensions
{
    internal static float? ReadWager(this IReadWrite io, float balance)
    {
        io.Write(Streams.Wager);
        if (io.ReadNumber("") == 0) { return null; }

        var prompt = Prompts.HowMuch;

        while(true)
        {
            var wager = io.ReadNumber(prompt);
            if (wager <= balance) { return wager; }

            prompt = Prompts.BetAgain;
        }
    }
}