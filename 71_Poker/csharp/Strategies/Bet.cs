namespace Poker.Strategies;

internal class Bet : Strategy
{
    public Bet(int amount) => Value = amount;

    public override int Value { get; }
}
