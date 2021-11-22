using System;
using System.Collections.Generic;

namespace Blackjack
{
    public class Deck
    {
        private static readonly Random _random = new Random();

        private readonly List<Card> _cards = new List<Card>(52);
        private readonly List<Card> _discards = new List<Card>(52);

        public Deck()
        {
            for (var index = 0; index < 12; index++)
            {
                for (var suit = 0; suit < 4; suit++)
                {
                    _discards.Add(new Card(index));
                }
            }
            Reshuffle();
        }

        private void Reshuffle()
        {
            Console.WriteLine("Reshuffling");

            _cards.AddRange(_discards);
            _discards.Clear();

            for (var index1 = _cards.Count - 1; index1 > 0; index1--)
            {
                var index2 = _random.Next(0, index1);
                var swapCard = _cards[index1];
                _cards[index1] = _cards[index2];
                _cards[index2] = swapCard;
            }
        }

        public Card DrawCard()
        {
            if (_cards.Count < 2)
                Reshuffle();

            var card = _cards[_cards.Count - 1];
            _cards.RemoveAt(_cards.Count - 1);
            return card;
        }

        public void Discard(IEnumerable<Card> cards)
        {
            _discards.AddRange(cards);
        }
    }
}
