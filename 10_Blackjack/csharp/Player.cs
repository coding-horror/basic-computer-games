namespace Blackjack
{
    public class Player
    {
        public Player(int index)
        {
            Index = index;
            Name = (index + 1).ToString();
            Hand = new Hand();
            SecondHand = new Hand();
        }

        public int Index { get; private set; }

        public string Name { get; private set; }

        public Hand Hand { get; private set; }

        public Hand SecondHand { get; private set;}

        public int RoundBet { get; set; }

        public int RoundWinnings { get; set; }

        public int TotalWinnings { get; set; }
    }
}
