namespace Poker.Strategies;

internal class Bluff : Bet
{
    public Bluff(int amount, int? keepMask)
        : base(amount)
    {
        KeepMask = keepMask;
    }

    public override int? KeepMask { get; }
}