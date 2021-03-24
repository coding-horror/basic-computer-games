using System;
using System.Collections.Generic;
using System.Text;

namespace War
{
    public enum Suit
    {
        none = 0,
        clubs,
        diamonds,
        hearts,
        spades
    }

    public enum Rank
    {
        none = 0,
        // Skip 1 because ace is high.
        two = 2,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        jack,
        queen,
        king,
        ace
    }

    // TODO Testing

    public class Card
    {
        private readonly Suit suit;
        private readonly Rank rank;

        private static Dictionary<Suit, string> suitNames = new Dictionary<Suit, string>()
        {
            { Suit.none, "N"},
            { Suit.clubs, "C"},
            { Suit.diamonds, "D"},
            { Suit.hearts, "H"},
            { Suit.spades, "S"},
        };

        private static Dictionary<Rank, string> rankNames = new Dictionary<Rank, string>()
        {
            { Rank.none, "0"},
            { Rank.two, "2"},
            { Rank.three, "3"},
            { Rank.four, "4"},
            { Rank.five, "5"},
            { Rank.six, "6"},
            { Rank.seven, "7"},
            { Rank.eight, "8"},
            { Rank.nine, "9"},
            { Rank.ten, "10"},
            { Rank.jack, "J"},
            { Rank.queen, "Q"},
            { Rank.king, "K"},
            { Rank.ace, "A"},
        };

        public Card(Suit suit, Rank rank) // immutable
        {
            this.suit = suit;
            this.rank = rank;
        }

        // would normally consider suit and rank but in this case we only want to compare rank.
        //public static bool operator ==(Card lhs, Card rhs)
        //{
        //    return lhs.rank == rhs.rank;
        //}

        //public static bool operator !=(Card lhs, Card rhs)
        //{
        //    return !(lhs == rhs);
        //}

        public static bool operator <(Card lhs, Card rhs)
        {
            return lhs.rank < rhs.rank;
        }

        public static bool operator >(Card lhs, Card rhs)
        {
            return rhs < lhs;
        }

        public static bool operator <=(Card lhs, Card rhs)
        {
            return !(lhs > rhs);
        }

        public static bool operator >=(Card lhs, Card rhs)
        {
            return !(lhs < rhs);
        }

        public override string ToString()
        {
            return $"{suitNames[suit]}-{rankNames[rank]}"; // string interpolation
        }
    }

    public class Deck
    {
        public const int deckSize = 52;

        private Card[] theDeck = new Card[deckSize];

        public Deck()
        {
            int i = 0;
            for (Suit suit = Suit.clubs; suit <= Suit.spades; suit++)
            {
                for (Rank rank = Rank.two; rank <= Rank.ace; rank++)
                {
                    theDeck[i] = new Card(suit, rank);
                    i++;
                }
            }
        }

        public Card GetCard(int i) => theDeck[i];

        public void Shuffle()
        {
            // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
            for (int i = deckSize - 1; i >= 1; i--)
            {
                var rand = new Random();
                int j = rand.Next(0, i);

                // Swap the cards at i and j
                Card temp = theDeck[j];
                theDeck[j] = theDeck[i];
                theDeck[i] = temp;
            }
        }
    }
}
