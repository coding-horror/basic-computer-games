using System;

namespace Blackjack
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("{0}BLACK JACK", new string(' ', 31));
            Console.WriteLine("{0}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", new string(' ', 15));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            OfferInstructions();

            var numberOfPlayers = Prompt.ForInteger("Number of players?", 1, 6);
            var game = new Game(numberOfPlayers);
            game.PlayGame();
        }

        private static void OfferInstructions()
        {
            if (!Prompt.ForYesNo("Do you want instructions?"))
                return;

            Console.WriteLine("This is the game of 21. As many as 7 players may play the");
            Console.WriteLine("game. On each deal, bets will be asked for, and the");
            Console.WriteLine("players' bets should be typed in. The cards will then be");
            Console.WriteLine("dealt, and each player in turn plays his hand. The");
            Console.WriteLine("first response should be either 'D', indicating that the");
            Console.WriteLine("player is doubling down, 'S', indicating that he is");
            Console.WriteLine("standing, 'H', indicating he wants another card, or '/',");
            Console.WriteLine("indicating that he wants to split his cards. After the");
            Console.WriteLine("initial response, all further responses should be 's' or");
            Console.WriteLine("'H', unless the cards were split, in which case doubling");
            Console.WriteLine("down is again permitted. In order to collect for");
            Console.WriteLine("Blackjack, the initial response should be 'S'.");
        }
    }
}
