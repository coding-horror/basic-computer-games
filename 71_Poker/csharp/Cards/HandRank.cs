namespace Poker.Cards;

internal class HandRank
{
    public static HandRank None = new(0, "");
    public static HandRank Schmaltz = new(1, "schmaltz, ", c => $"{c.Rank} high");
    public static HandRank PartialStraight = new(2, ""); // The original code does not assign a display string here
    public static HandRank Pair = new(3, "a pair of ", c => $"{c.Rank}'s");
    public static HandRank TwoPair = new(4, "two pair, ", c => $"{c.Rank}'s");
    public static HandRank Three = new(5, "three ", c => $"{c.Rank}'s");
    public static HandRank Straight = new(6, "straight", c => $"{c.Rank} high");
    public static HandRank Flush = new(7, "a flush in ", c => c.Suit.ToString());
    public static HandRank FullHouse = new(8, "full house, ", c => $"{c.Rank}'s");
    public static HandRank Four = new(9, "four ", c => $"{c.Rank}'s");
    // The original code does not detect a straight flush or royal flush

    private readonly int _value;
    private readonly string _displayName;
    private readonly Func<Card, string> _suffixSelector;

    private HandRank(int value, string displayName, Func<Card, string>? suffixSelector = null)
    {
        _value = value;
        _displayName = displayName;
        _suffixSelector = suffixSelector ?? (_ => "");
    }

    public string ToString(Card highCard) => $"{_displayName}{_suffixSelector.Invoke(highCard)}";

    public static bool operator >(HandRank x, HandRank y) => x._value > y._value;
    public static bool operator <(HandRank x, HandRank y) => x._value < y._value;
    public static bool operator >=(HandRank x, HandRank y) => x._value >= y._value;
    public static bool operator <=(HandRank x, HandRank y) => x._value <= y._value;
}
