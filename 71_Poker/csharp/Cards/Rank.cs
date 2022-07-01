namespace Poker.Cards;

internal struct Rank : IComparable<Rank>
{
    public static IEnumerable<Rank> Ranks => new[]
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace
    };

    public static Rank Two = new(2);
    public static Rank Three = new(3);
    public static Rank Four = new(4);
    public static Rank Five = new(5);
    public static Rank Six = new(6);
    public static Rank Seven = new(7);
    public static Rank Eight = new(8);
    public static Rank Nine = new(9);
    public static Rank Ten = new(10);
    public static Rank Jack = new(11, "Jack");
    public static Rank Queen = new(12, "Queen");
    public static Rank King = new(13, "King");
    public static Rank Ace = new(14, "Ace");

    private readonly int _value;
    private readonly string _name;

    private Rank(int value, string? name = null)
    {
        _value = value;
        _name = name ?? $" {value} ";
    }

    public override string ToString() => _name;

    public int CompareTo(Rank other) => this - other;

    public static bool operator <(Rank x, Rank y) => x._value < y._value;
    public static bool operator >(Rank x, Rank y) => x._value > y._value;
    public static bool operator ==(Rank x, Rank y) => x._value == y._value;
    public static bool operator !=(Rank x, Rank y) => x._value != y._value;

    public static int operator -(Rank x, Rank y) => x._value - y._value;

    public static bool operator <=(Rank rank, int value) => rank._value <= value;
    public static bool operator >=(Rank rank, int value) => rank._value >= value;

    public override bool Equals(object? obj) => obj is Rank other && this == other;

    public override int GetHashCode() => _value.GetHashCode();
}
