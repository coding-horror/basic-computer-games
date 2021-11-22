using System;
using System.Collections.Generic;



namespace War
{
    // These enums define the card's suit and rank.
    public enum Suit
    {
        clubs,
        diamonds,
        hearts,
        spades
    }

    public enum Rank
    {
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

    // A class to represent a playing card.
    public class Card
    {
        // A card is an immutable object (i.e. it can't be changed) so its suit
        // and rank value are readonly; they can only be set in the constructor.
        private readonly Suit suit;
        private readonly Rank rank;

        // These dictionaries are used to convert a suit or rank value into a string.
        private readonly Dictionary<Suit, string> suitNames = new Dictionary<Suit, string>()
        {
            { Suit.clubs, "C"},
            { Suit.diamonds, "D"},
            { Suit.hearts, "H"},
            { Suit.spades, "S"},
        };

        private readonly Dictionary<Rank, string> rankNames = new Dictionary<Rank, string>()
        {
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

        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }

        // Relational Operator Overloading.
        //
        // You would normally expect the relational operators to consider both the suit and the
        // rank of a card, but in this program suit doesn't matter so we define the operators to just
        // compare rank.

        // When adding relational operators we would normally include == and != but they are not
        // relevant to this program so haven't been defined. Note that if they were defined we
        // should also override the Equals() and GetHashCode() methods. See, for example:
        // http://www.blackwasp.co.uk/CSharpRelationalOverload.aspx

        // If the == and != operators were defined they would look like this:
        //
        //public static bool operator ==(Card lhs, Card rhs)
        //{
        //    return lhs.rank == rhs.rank;
        //}
        //
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
            // N.B. We are using string interpolation to create the card name.
            return $"{suitNames[suit]}-{rankNames[rank]}";
        }
    }

    // A class to represent a deck of cards.
    public class Deck
    {
        public const int deckSize = 52;

        private Card[] theDeck = new Card[deckSize];

        public Deck()
        {
            // Populate theDeck with all the cards in order.
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

        // Return the card at a particular position in the deck.
        // N.B. As this is such a short method, we make it an
        // expression-body method.
        public Card GetCard(int i) => theDeck[i];

        // Shuffle the cards, this uses the modern version of the
        // Fisher-Yates shuffle, see:
        // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm
        public void Shuffle()
        {
            var rand = new Random();

            // Iterate backwards through the deck.
            for (int i = deckSize - 1; i >= 1; i--)
            {
                int j = rand.Next(0, i);

                // Swap the cards at i and j
                Card temp = theDeck[j];
                theDeck[j] = theDeck[i];
                theDeck[i] = temp;
            }
        }
    }
}
