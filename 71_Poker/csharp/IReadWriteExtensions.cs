using static System.StringComparison;

namespace Poker;

internal static class IReadWriteExtensions
{
    internal static bool ReadYesNo(this IReadWrite io, string prompt)
    {
        while (true)
        {
            var response = io.ReadString(prompt);
            if (response.Equals("YES", InvariantCultureIgnoreCase)) { return true; }
            if (response.Equals("NO", InvariantCultureIgnoreCase)) { return false; }
            io.WriteLine("Answer Yes or No, please.");
        }
    }

    internal static float ReadNumber(this IReadWrite io) => io.ReadNumber("");

    internal static int ReadNumber(this IReadWrite io, string prompt, int max, string maxPrompt)
    {
        io.Write(prompt);
        while (true)
        {
            var response = io.ReadNumber();
            if (response <= max) { return (int)response; }
            io.WriteLine(maxPrompt);
        }
    }

    internal static IAction ReadPlayerAction(this IReadWrite io, bool noCurrentBets)
    {
        while(true)
        {
            io.WriteLine();
            var bet = io.ReadNumber("What is your bet");
            if (bet != (int)bet)
            {
                if (noCurrentBets && bet == .5) { return new Bet(0); }
                io.WriteLine("No small change, please.");
                continue;
            }
            if (bet == 0) { return new Fold(); }
            return new Bet(bet);
        }
    }
}