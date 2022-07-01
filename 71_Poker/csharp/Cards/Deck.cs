using static Poker.Cards.Rank;

namespace Poker.Cards;

internal class Deck
{
    private readonly Card[] _cards;
    private int _nextCard;

    public Deck()
    {
        _cards = Ranks.SelectMany(r => Enum.GetValues<Suit>().Select(s => new Card(r, s))).ToArray();
    }

    public void Shuffle(IRandom _random)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            var j = _random.Next(_cards.Length);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
        _nextCard = 0;
    }

    public Card DealCard() => _cards[_nextCard++];

    public Hand DealHand() => new Hand(Enumerable.Range(0, 5).Select(_ => DealCard()));
}
