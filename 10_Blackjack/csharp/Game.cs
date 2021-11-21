using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{
    public class Game
    {
        private readonly Deck _deck = new Deck();
        private readonly int _numberOfPlayers;
        private readonly Player[] _players;
        private readonly Hand _dealerHand;

        public Game(int numberOfPlayers)
        {
            _numberOfPlayers = numberOfPlayers;
            _players = new Player[_numberOfPlayers];
            for (var playerIndex = 0; playerIndex < _numberOfPlayers; playerIndex++)
                _players[playerIndex] = new Player(playerIndex);
            _dealerHand = new Hand();
        }

        public void PlayGame()
        {
            while (true)
            {
                PlayRound();
                TallyResults();
                ResetRoundState();
                Console.WriteLine();
            }
        }

        public void PlayRound()
        {
            GetPlayerBets();

            DealHands();

            // Test for insurance
            var dealerIsShowingAce = _dealerHand.Cards[0].IsAce;
            if (dealerIsShowingAce && Prompt.ForYesNo("Any insurance?"))
            {
                Console.WriteLine("Insurance bets");
                var insuranceBets = new int[_numberOfPlayers];
                foreach (var player in _players)
                    insuranceBets[player.Index] = Prompt.ForInteger($"# {player.Index + 1} ?", 0, player.RoundBet / 2);

                var insuranceEffectMultiplier = _dealerHand.IsBlackjack ? 2 : -1;
                foreach (var player in _players)
                    player.RoundWinnings += insuranceBets[player.Index] * insuranceEffectMultiplier;
            }

            // Test for dealer blackjack
            var concealedCard = _dealerHand.Cards[0];
            if (_dealerHand.IsBlackjack)
            {
                Console.WriteLine();
                Console.WriteLine("Dealer has {0} {1} in the hole for blackjack.", concealedCard.IndefiniteArticle, concealedCard.Name);
                return;
            }
            else if (dealerIsShowingAce)
            {
                Console.WriteLine();
                Console.WriteLine("No dealer blackjack.");
            }

            foreach (var player in _players)
                PlayHand(player);

            // Dealer hand
            var allPlayersBusted = _players.All(p => p.Hand.IsBusted && (!p.SecondHand.Exists || p.SecondHand.IsBusted));
            if (allPlayersBusted)
                Console.WriteLine("Dealer had {0} {1} concealed.", concealedCard.IndefiniteArticle, concealedCard.Name);
            else
            {
                Console.WriteLine("Dealer has {0} {1} concealed for a total of {2}", concealedCard.IndefiniteArticle, concealedCard.Name, _dealerHand.Total);
                if (_dealerHand.Total < 17)
                {
                    Console.Write("Draws");
                    while (_dealerHand.Total < 17)
                    {
                        var card = _dealerHand.AddCard(_deck.DrawCard());
                        Console.Write("  {0}", card.Name);
                    }
                    if (_dealerHand.IsBusted)
                        Console.WriteLine("  ...Busted");
                    else
                        Console.WriteLine("  ---Total is {0}", _dealerHand.Total);
                }
            }
        }

        private void GetPlayerBets()
        {
            Console.WriteLine("Bets:");
            foreach (var player in _players)
                player.RoundBet = Prompt.ForInteger($"# {player.Name} ?", 1, 500);
        }

        private void DealHands()
        {
            Console.Write("Player ");
            foreach (var player in _players)
                Console.Write("{0}     ", player.Name);
            Console.WriteLine("Dealer");

            for (var cardIndex = 0; cardIndex < 2; cardIndex++)
            {
                Console.Write("      ");
                foreach (var player in _players)
                    Console.Write("  {0,-4}", player.Hand.AddCard(_deck.DrawCard()).Name);
                var dealerCard = _dealerHand.AddCard(_deck.DrawCard());
                Console.Write("  {0,-4}", (cardIndex == 0) ? "XX" : dealerCard.Name);

                Console.WriteLine();
            }
        }

        private void PlayHand(Player player)
        {
            var hand = player.Hand;

            Console.Write("Player {0} ", player.Name);

            var playerCanSplit = hand.Cards[0].Value == hand.Cards[1].Value;
            var command = Prompt.ForCommandCharacter("?", playerCanSplit ? "HSD/" : "HSD");
            switch (command)
            {
                case "D":
                    player.RoundBet *= 2;
                    goto case "H";

                case "H":
                    while (TakeHit(hand) && PromptForAnotherHit())
                    { }
                    if (!hand.IsBusted)
                        Console.WriteLine("Total is {0}", hand.Total);
                    break;

                case "S":
                    if (hand.IsBlackjack)
                    {
                        Console.WriteLine("Blackjack!");
                        player.RoundWinnings = (int)(1.5 * player.RoundBet + 0.5);
                        player.RoundBet = 0;
                    }
                    else
                        Console.WriteLine("Total is {0}", hand.Total);
                    break;

                case "/":
                    hand.SplitHand(player.SecondHand);
                    var card = hand.AddCard(_deck.DrawCard());
                    Console.WriteLine("First hand receives {0} {1}", card.IndefiniteArticle, card.Name);
                    card = player.SecondHand.AddCard(_deck.DrawCard());
                    Console.WriteLine("Second hand receives {0} {1}", card.IndefiniteArticle, card.Name);

                    for (int handNumber = 1; handNumber <= 2; handNumber++)
                    {
                        hand = (handNumber == 1) ? player.Hand : player.SecondHand;

                        Console.Write("Hand {0}", handNumber);
                        while (PromptForAnotherHit() && TakeHit(hand))
                        { }
                        if (!hand.IsBusted)
                            Console.WriteLine("Total is {0}", hand.Total);
                    }
                    break;
            }
        }

        private bool TakeHit(Hand hand)
        {
            var card = hand.AddCard(_deck.DrawCard());
            Console.Write("Received {0,-6}", $"{card.IndefiniteArticle} {card.Name}");
            if (hand.IsBusted)
            {
                Console.WriteLine("...Busted");
                return false;
            }
            return true;
        }

        private bool PromptForAnotherHit()
        {
            return String.Equals(Prompt.ForCommandCharacter(" Hit?", "HS"), "H");
        }

        private void TallyResults()
        {
            Console.WriteLine();
            foreach (var player in _players)
            {
                player.RoundWinnings += CalculateWinnings(player, player.Hand);
                if (player.SecondHand.Exists)
                    player.RoundWinnings += CalculateWinnings(player, player.SecondHand);
                player.TotalWinnings += player.RoundWinnings;

                Console.WriteLine("Player {0} {1,-6} {2,3}   Total= {3,5}",
                        player.Name,
                        (player.RoundWinnings > 0) ? "wins" : (player.RoundWinnings) < 0 ? "loses" : "pushes",
                        (player.RoundWinnings != 0) ? Math.Abs(player.RoundWinnings).ToString() : "",
                        player.TotalWinnings);
            }
            Console.WriteLine("Dealer's total= {0}", -_players.Sum(p => p.TotalWinnings));
        }

        private int CalculateWinnings(Player player, Hand hand)
        {
            if (hand.IsBusted)
                return -player.RoundBet;
            if (hand.Total == _dealerHand.Total)
                return 0;
            if (_dealerHand.IsBusted || hand.Total > _dealerHand.Total)
                return player.RoundBet;
            return -player.RoundBet;
        }

        private void ResetRoundState()
        {
            foreach (var player in _players)
            {
                player.RoundWinnings = 0;
                player.RoundBet = 0;
                player.Hand.Discard(_deck);
                player.SecondHand.Discard(_deck);
            }
            _dealerHand.Discard(_deck);
        }
    }
}
