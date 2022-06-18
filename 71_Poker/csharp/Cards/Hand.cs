using System.Text;

namespace Poker.Cards;

internal class Hand
{
    public static readonly Hand Empty = new Hand();

    private readonly Card[] _cards;
    private readonly string _name1;
    private readonly string _name2;

    private Hand()
    {
        _cards = Array.Empty<Card>();
        _name1 = "";
        _name2 = "";
        Name = "";
    }

    public Hand(IEnumerable<Card> cards)
        : this(cards, isAfterDraw: false)
    {
    }

    private Hand(IEnumerable<Card> cards, bool isAfterDraw)
    {
        _cards = cards.ToArray();
        (Rank, _name1, _name2, HighCard, KeepMask) = Analyze();
        Name = GetHandName();

        IsWeak = Rank < 10
            || Rank == 10 && isAfterDraw
            || Rank <= 12 && HighCard.Rank <= 6;
    }

    public string Name { get; }
    public int Rank { get; }
    public Card HighCard { get; }
    public int KeepMask { get; set; }
    public bool IsWeak { get; }

    public Hand Replace(int cardNumber, Card newCard)
    {
        if (cardNumber < 1 || cardNumber > _cards.Length) { return this; }

        _cards[cardNumber - 1] = newCard;
        return new Hand(_cards, isAfterDraw: true);
    }

    private (int, string, string, Card, int) Analyze()
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
            return (15, "A Flus", "h in", _cards[0], 0b11111);
        }
        var sortedCards = _cards.OrderBy(c => c.Rank).ToArray();

        var handRank = 0;
        var keepMask = 0;
        Card highCard = default;
        var handName1 = "";
        var handName2 = "";
        for (var i = 0; i < sortedCards.Length - 1; i++)
        {
            if (sortedCards[i].Rank == sortedCards[i+1].Rank)
            {
                keepMask |= 0b11 << i;
                highCard = sortedCards[i];
                (handRank, handName1, handName2) =
                    (handRank, i > 0 && sortedCards[i].Rank == sortedCards[i - 1].Rank) switch
                    {
                        (<11, _) => (11, "A Pair", " of "),
                        (11, true) => (13, "Three", " "),
                        (11, _) => (12, "Two P", "air, "),
                        (12, _) => (16, "Full H", "ouse, "),
                        (_, true) => (17, "Four", " "),
                        _ => (16, "Full H", "ouse, ")
                    };
            }
        }
        if (keepMask == 0)
        {
            if (sortedCards[3] - sortedCards[0] == 3)
            {
                keepMask=0b1111;
                handRank=10;
            }
            if (sortedCards[4] - sortedCards[1] == 3)
            {
                if (handRank == 10)
                {
                    return (14, "Straig", "ht", sortedCards[4], 0b11111);
                }
                handRank=10;
                keepMask=0b11110;
            }
        }
        return handRank < 10
            ? (9, "Schmal", "tz, ", sortedCards[4], 0b11000)
            : (handRank, handName1, handName2, highCard, keepMask);
    }

    private string GetHandName()
    {
        var sb = new StringBuilder(_name1).Append(_name2);
        if (_name1 == "A Flus")
        {
            sb.Append(HighCard.Suit).AppendLine();
        }
        else
        {
            sb.Append(HighCard.Rank)
                .AppendLine(_name1 == "Schmal" || _name1 == "Straig" ? " High" : "'s");
        }
        return sb.ToString();
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
