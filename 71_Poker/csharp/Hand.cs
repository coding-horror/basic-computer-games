using System.Text;

namespace Poker;

internal class Hand
{
    public static readonly Hand Empty = new Hand();

    private readonly Card[] _cards;
    private readonly Card _highCard;
    private readonly string _name1;
    private readonly string _name2;
    private readonly int _keepMask;
    private readonly Func<int, int> _iTransform;

    private Hand()
    {
        _cards = Array.Empty<Card>();
        _name1 = "";
        _name2 = "";
        _iTransform = Identity;
        Name = "";
    }

    public Hand(IEnumerable<Card> cards)
    {
        _cards = cards.ToArray();
        (Rank, _name1, _name2, _highCard, _keepMask, _iTransform) = Analyze();
        Name = GetHandName();
    }

    public string Name { get; }
    public int Rank { get; }

    public Hand Replace(int cardNumber, Card newCard)
    {
        if (cardNumber < 1 || cardNumber > _cards.Length) { return this; }

        _cards[cardNumber - 1] = newCard;
        return new Hand(_cards);
    }

    public (int, int) Analyze(int i) => (_keepMask, _iTransform(i));

    private (int, string, string, Card, int, Func<int, int>) Analyze()
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
            return (15, "A Flus", "h in", _cards[0], 0b11111, Identity);
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
                    return (14, "Straig", "ht", sortedCards[4], 0b11111, Identity);
                }
                handRank=10;
                keepMask=0b11110;
            }
        }
        if (handRank < 10)
        {
            return (9, "Schmal", "tz, ", sortedCards[4], 0b11000, To6);
        }
        var iTransform = Identity;
        if (handRank == 10)
        {
            iTransform = To6If1;
        }
        else if (handRank <= 12 && highCard.Rank <= 6)
        {
            iTransform = To6;
        }
        return (handRank, handName1, handName2, highCard, keepMask, iTransform);
    }

    private int Identity(int x) => x;
    private int To6(int _) => 6;
    private int To6If1(int x) => x == 1 ? 6 : x;

    private string GetHandName()
    {
        var sb = new StringBuilder(_name1).Append(_name2);
        if (_name1 == "A Flus")
        {
            sb.Append(_highCard.Suit).AppendLine();
        }
        else
        {
            sb.Append(_highCard.Rank)
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
        x.Rank == y.Rank && x._highCard > y._highCard;

    public static bool operator <(Hand x, Hand y) =>
        x.Rank < y.Rank ||
        x.Rank == y.Rank && x._highCard < y._highCard;
}
