namespace Blackjack
{
    public class Card
    {
        private static readonly string[] _names = new[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};

        public Card(int index)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public string Name => _names[Index];

        public string IndefiniteArticle => (Index == 0 || Index == 7) ? "an" : "a";

        public bool IsAce => Index == 0;

        public int Value
        {
            get
            {
                if (IsAce)
                    return 11;
                if (Index > 8)
                    return 10;
                return Index + 1;
            }
        }
    }
}
