namespace Roulette;

internal static class IOExtensions
{
    internal static int ReadBetCount(this IReadWrite io)
    {
        while (true)
        {
            var betCount = io.ReadNumber(Prompts.HowManyBets);
            if (betCount.IsValidInt(1)) { return (int)betCount; }
        }
    }

    internal static Bet ReadBet(this IReadWrite io, int number)
    {
        while (true)
        {
            var (type, amount) = io.Read2Numbers(Prompts.Bet(number));

            if (type.IsValidInt(1, 50) && amount.IsValidInt(5, 500))
            {
                return new()
                {
                    Type = (int)type, 
                    Number = number, 
                    Wager = (int)amount
                };
            }
        }
    }

    internal static bool IsValidInt(this float value, int minValue, int maxValue = int.MaxValue)
        => value == (int)value && value >= minValue && value <= maxValue;
}