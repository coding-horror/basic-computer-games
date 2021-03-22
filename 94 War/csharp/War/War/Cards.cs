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

    class Card
    {
        // TODO Turn into properties, maybe??
        private Suit suit;
        private Rank rank;

        public Card(Suit suit, Rank rank) // immutable
        {
            this.suit = suit;
            this.rank = rank;
        }

        // would normally consider suit and rank but in this case we only want to compare rank.
        public static bool operator ==(Card lhs, Card rhs)
        {
            return lhs.rank == rhs.rank;
        }

        public static bool operator !=(Card lhs, Card rhs)
        {
            return !(lhs == rhs);
        }

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
            // TODO No need to create the dictionaries each time this is called.
            // Also make it static.
            // Is there a C# equivalent of an initializer list?
            var suitNames = new Dictionary<Suit, string>();

            suitNames[Suit.none]     = "N";
            suitNames[Suit.clubs]    = "C";
            suitNames[Suit.diamonds] = "D";
            suitNames[Suit.hearts]   = "H";
            suitNames[Suit.spades]   = "S";

            var rankNames = new Dictionary<Rank, string>();
            rankNames[Rank.none]  = "0";
            rankNames[Rank.two]   = "2";
            rankNames[Rank.three] = "3";
            rankNames[Rank.four]  = "4";
            rankNames[Rank.five]  = "5";
            rankNames[Rank.six]   = "6";
            rankNames[Rank.seven] = "7";
            rankNames[Rank.eight] = "8";
            rankNames[Rank.nine]  = "9";
            rankNames[Rank.ten]   = "10";
            rankNames[Rank.jack]  = "J";
            rankNames[Rank.queen] = "Q";
            rankNames[Rank.king]  = "K";
            rankNames[Rank.ace]  = "A";

            return $"{suitNames[suit]}-{rankNames[rank]}"; // string interpolation
        }
    }

    class Deck
    {
    }
}
