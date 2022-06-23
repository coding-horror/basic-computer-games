namespace Poker.Strategies;

internal abstract class Strategy
{
    public static Strategy Fold = new Fold();
    public static Strategy Check = new Check();
    public static Strategy Raise = new Raise();
    public static Strategy Bet(float amount) => new Bet((int)amount);
    public static Strategy Bet(int amount) => new Bet(amount);
    public static Strategy Bluff(int amount, int? keepMask = null) => new Bluff(amount, keepMask);

    public abstract int Value { get; }
    public virtual int? KeepMask { get; }
}
