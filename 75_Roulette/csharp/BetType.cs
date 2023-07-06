namespace Roulette;

internal record struct BetType(int Value)
{
    public static implicit operator BetType(int value) => new(value);

    public int Payout => Value switch
        {
            <= 36 or >= 49 => 35,
            <= 42 => 2,
            <= 48 => 1
        };
}
