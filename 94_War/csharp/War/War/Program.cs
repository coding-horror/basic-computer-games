namespace War
{
    class Program
    {
        static void Main(string[] args)
        {
            var ui = new UserInterface();
            ui.WriteIntro();

            var deck = new Deck();
            deck.Shuffle();

            int yourScore = 0;
            int computersScore = 0;
            bool usedAllCards = true;

            for (int i = 0; i < Deck.deckSize; i += 2)
            {
                // Play the next hand.
                var yourCard = deck.GetCard(i);
                var computersCard = deck.GetCard(i + 1);

                ui.WriteAResult(yourCard, computersCard, ref computersScore, ref yourScore);

                if (!ui.AskAQuestion("DO YOU WANT TO CONTINUE? "))
                {
                    usedAllCards = false;
                    break;
                }
            }

            ui.WriteClosingRemarks(usedAllCards, yourScore, computersScore);
        }
    }
}
