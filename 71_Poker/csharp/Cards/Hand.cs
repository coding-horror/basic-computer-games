using System.Text;
using static Poker.Cards.HandRank;
namespace Poker.Cards;

internal class Hand
{
    public static readonly Hand Empty = new Hand();

    private readonly Card[] _cards;

    private Hand()
    {
        _cards = Array.Empty<Card>();
        Rank = None;
    }

    public Hand(IEnumerable<Card> cards)
        : this(cards, isAfterDraw: false)
    {
    }

    private Hand(IEnumerable<Card> cards, bool isAfterDraw)
    {
        _cards = cards.ToArray();
        (Rank, HighCard, KeepMask) = Analyze();

        IsWeak = Rank < PartialStraight
            || Rank == PartialStraight && isAfterDraw
            || Rank <= TwoPair && HighCard.Rank <= 6;
    }

    public string Name => Rank.ToString(HighCard);
    public HandRank Rank { get; }
    public Card HighCard { get; }
    public int KeepMask { get; set; }
    public bool IsWeak { get; }

    public Hand Replace(int cardNumber, Card newCard)
    {
        if (cardNumber < 1 || cardNumber > _cards.Length) { return this; }

        _cards[cardNumber - 1] = newCard;
        return new Hand(_cards, isAfterDraw: true);
    }

    private (HandRank, Card, int) Analyze()
    {
        var suitMatchCount = 0;
        for (var i = 0; i < _cards.Length; i++)
        {
            if (i < _cards.Length-1 && _cards[i].Suit == _cards[i+1].Suit)
            {
                suitMatchCount++;
            }
        }
        if (suitMatchCount == 4)
        {
            return (Flush, _cards[0], 0b11111);
        }
        var sortedCards = _cards.OrderBy(c => c.Rank).ToArray();

        var handRank = Schmaltz;
        var keepMask = 0;
        Card highCard = default;
        for (var i = 0; i < sortedCards.Length - 1; i++)
        {
            var matchesNextCard = sortedCards[i].Rank == sortedCards[i+1].Rank;
            var matchesPreviousCard = i > 0 && sortedCards[i].Rank == sortedCards[i - 1].Rank;

            if (matchesNextCard)
            {
                keepMask |= 0b11 << i;
                highCard = sortedCards[i];
                handRank = matchesPreviousCard switch
                {
                    _ when handRank < Pair => Pair,
                    true when handRank == Pair => Three,
                    _ when handRank == Pair => TwoPair,
                    _ when handRank == TwoPair => FullHouse,
                    true => Four,
                    _ => FullHouse
                };
            }
        }
        if (keepMask == 0)
        {
            if (sortedCards[3] - sortedCards[0] == 3)
            {
                keepMask=0b1111;
                handRank=PartialStraight;
            }
            if (sortedCards[4] - sortedCards[1] == 3)
            {
                if (handRank == PartialStraight)
                {
                    return (Straight, sortedCards[4], 0b11111);
                }
                handRank=PartialStraight;
                keepMask=0b11110;
            }
        }
        return handRank < PartialStraight
            ? (Schmaltz, sortedCards[4], 0b11000)
            : (handRank, highCard, keepMask);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < _cards.Length; i++)
        {
            var cardDisplay = $" {i+1} --  {_cards[i]}";
            // Emulates the effect of the BASIC PRINT statement using the ',' to align text to 14-char print zones
            sb.Append(cardDisplay.PadRight(cardDisplay.Length + 14 - cardDisplay.Length % 14));
            if (i % 2 == 1)
            {
                sb.AppendLine();
            }
        }
        sb.AppendLine();
        return sb.ToString();
    }

    public static bool operator >(Hand x, Hand y) =>
        x.Rank > y.Rank ||
        x.Rank == y.Rank && x.HighCard > y.HighCard;

    public static bool operator <(Hand x, Hand y) =>
        x.Rank < y.Rank ||
        x.Rank == y.Rank && x.HighCard < y.HighCard;
}
