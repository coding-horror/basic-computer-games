namespace Roulette;

internal record struct Bet(BetType Type, int Number, int Wager)
{
    public int Payout => Wager * Type.Payout;
}
