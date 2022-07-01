namespace Poker.Cards;

internal record struct Card (Rank Rank, Suit Suit)
{
    public override string ToString() => $"{Rank} of {Suit}";

    public static bool operator <(Card x, Card y) => x.Rank < y.Rank;
    public static bool operator >(Card x, Card y) => x.Rank > y.Rank;

    public static int operator -(Card x, Card y) => x.Rank - y.Rank;
}
