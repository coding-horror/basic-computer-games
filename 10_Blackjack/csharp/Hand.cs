using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Hand
    {
        private readonly List<Card> _cards = new List<Card>(12);
        private int _cachedTotal = 0;

        public Card AddCard(Card card)
        {
            _cards.Add(card);
            _cachedTotal = 0;
            return card;
        }

        public void Discard(Deck deck)
        {
            deck.Discard(_cards);
            _cards.Clear();
            _cachedTotal = 0;
        }

        public void SplitHand(Hand secondHand)
        {
            if (Count != 2 || secondHand.Count != 0)
                throw new InvalidOperationException();
            secondHand.AddCard(_cards[1]);
            _cards.RemoveAt(1);
            _cachedTotal = 0;
        }

        public IReadOnlyList<Card> Cards => _cards;

        public int Count => _cards.Count;

        public bool Exists => _cards.Count > 0;

        public int Total
        {
            get
            {
                if (_cachedTotal == 0)
                {
                    var aceCount = 0;
                    foreach (var card in _cards)
                    {
                        _cachedTotal += card.Value;
                        if (card.IsAce)
                            aceCount++;
                    }
                    while (_cachedTotal > 21 && aceCount > 0)
                    {
                        _cachedTotal -= 10;
                        aceCount--;
                    }
                }
                return _cachedTotal;
            }
        }

        public bool IsBlackjack => Total == 21 && Count == 2;

        public bool IsBusted => Total > 21;
    }
}
