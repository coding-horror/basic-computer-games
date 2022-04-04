"""
Blackjack

Ported by Martin Thoma in 2022,
using the rust implementation of AnthonyMichaelTDM
"""

import enum
import random
from dataclasses import dataclass
from typing import List, NamedTuple


class PlayerType(enum.Enum):
    Player = "Player"
    Dealer = "Dealer"


class Play(enum.Enum):
    Stand = enum.auto()
    Hit = enum.auto()
    DoubleDown = enum.auto()
    Split = enum.auto()


class Card(NamedTuple):
    name: str

    @property
    def value(self) -> int:
        """
        returns the value associated with a card with the passed name
        return 0 if the passed card name doesn't exist
        """
        return {
            "ACE": 11,
            "2": 2,
            "3": 3,
            "4": 4,
            "5": 5,
            "6": 6,
            "7": 7,
            "8": 8,
            "9": 9,
            "10": 10,
            "JACK": 10,
            "QUEEN": 10,
            "KING": 10,
        }.get(self.name, 0)


class Hand(NamedTuple):
    cards: List[Card]

    def add_card(self, card: Card) -> None:
        """add a passed card to this hand"""
        self.cards.append(card)

    def get_total(self) -> int:
        """returns the total points of the cards in this hand"""
        total: int = 0
        for card in self.cards:
            total += int(card.value)

        # if there is an ACE, and the hand would otherwise bust,
        # treat the ace like it's worth 1
        if total > 21 and any(card.name == "ACE" for card in self.cards):
            total -= 10

        return total

    def discard_hand(self, deck: "Decks") -> None:
        """adds the cards in hand into the discard pile"""
        _len = len(self.cards)
        for _i in range(_len):
            if len(self.cards) == 0:
                raise ValueError("hand empty")
            deck.discard_pile.append(self.cards.pop())


class Decks(NamedTuple):
    deck: List[Card]
    discard_pile: List[Card]

    @classmethod
    def new(cls) -> "Decks":
        """creates a new full and shuffled deck, and an empty discard pile"""
        # returns a number of full decks of 52 cards, shuffles them
        deck = Decks(deck=[], discard_pile=[])
        number_of_decks = 3

        # fill deck
        for _n in range(number_of_decks):
            # fill deck with number_of_decks decks worth of cards
            for card_name in CARD_NAMES:
                # add 4 of each card, totaling one deck with 4 of each card
                for _ in range(4):
                    deck.deck.append(Card(name=card_name))

        deck.shuffle()
        return deck

    def shuffle(self) -> None:
        """shuffles the deck"""
        random.shuffle(self.deck)

    def draw_card(self) -> Card:
        """
        draw card from deck, and return it
        if deck is empty, shuffles discard pile into it and tries again
        """
        if len(self.deck) == 0:
            _len = len(self.discard_pile)

            if _len > 0:
                # deck is empty, shuffle discard pile into deck and try again
                print("deck is empty, shuffling")
                for _i in range(_len):
                    if len(self.discard_pile) == 0:
                        raise ValueError("discard pile is empty")
                    self.deck.append(self.discard_pile.pop())
                self.shuffle()
                return self.draw_card()
            else:
                # discard pile and deck are empty, should never happen
                raise Exception("discard pile empty")
        else:
            card = self.deck.pop()
            return card


@dataclass
class Player:
    hand: Hand
    balance: int
    bet: int
    wins: int
    player_type: PlayerType
    index: int

    @classmethod
    def new(cls, player_type: PlayerType, index: int) -> "Player":
        """creates a new player of the given type"""
        return Player(
            hand=Hand(cards=[]),
            balance=STARTING_BALANCE,
            bet=0,
            wins=0,
            player_type=player_type,
            index=index,
        )

    def get_name(self) -> str:
        return f"{self.player_type}{self.index}"

    def get_bet(self) -> None:
        """gets a bet from the player"""
        if PlayerType.Player == self.player_type:
            if self.balance < 1:
                print(f"{self.get_name()} is out of money :(")
                self.bet = 0
            self.bet = get_number_from_user_input(
                f"{self.get_name()}\tWhat is your bet", 1, self.balance
            )

    def hand_as_string(self, hide_dealer: bool) -> str:
        """
        returns a string of the players hand

        if player is a dealer, returns the first card in the hand followed
        by *'s for every other card
        if player is a player, returns every card and the total
        """
        if not hide_dealer:
            s = ""
            for cards_in_hand in self.hand.cards[::-1]:
                s += f"{cards_in_hand.name}\t"
            s += f"total points = {self.hand.get_total()}"
            return s
        else:
            if self.player_type == PlayerType.Dealer:
                s = ""
                for c in self.hand.cards[1::-1]:
                    s += f"{c.name}\t"
                return s
            elif self.player_type == PlayerType.Player:
                s = ""
                for cards_in_hand in self.hand.cards[::-1]:
                    s += f"{cards_in_hand.name}\t"
                s += f"total points = {self.hand.get_total()}"
                return s
        raise Exception("This is unreachable")

    def get_play(self) -> Play:
        """get the players 'play'"""
        # do different things depending on what type of player this is:
        # if it's a dealer, use an algorithm to determine the play
        # if it's a player, ask user for input
        if self.player_type == PlayerType.Dealer:
            if self.hand.get_total() > 16:
                return Play.Stand
            else:
                return Play.Hit
        elif self.player_type == PlayerType.Player:
            valid_results: List[str]
            if len(self.hand.cards) > 2:
                # if there are more than 2 cards in the hand,
                # at least one turn has happened, so splitting and
                # doubling down are not allowed
                valid_results = ["s", "h"]
            else:
                valid_results = ["s", "h", "d", "/"]
            play = get_char_from_user_input("\tWhat is your play?", valid_results)
            if play == "s":
                return Play.Stand
            elif play == "h":
                return Play.Hit
            elif play == "d":
                return Play.DoubleDown
            elif play == "/":
                return Play.Split
            else:
                raise ValueError(f"got invalid character {play}")
        raise Exception("This is unreachable")


@dataclass
class Game:
    players: List[Player]  # last item in this is the dealer
    decks: Decks
    games_played: int

    @classmethod
    def new(cls, num_players: int) -> "Game":
        players: List[Player] = []

        # add dealer
        players.append(Player.new(PlayerType.Dealer, 0))
        # create human player(s) (at least one)
        players.append(Player.new(PlayerType.Player, 1))
        for i in range(2, num_players):  # one less than num_players players
            players.append(Player.new(PlayerType.Player, i))

        if get_char_from_user_input("Do you want instructions", ["y", "n"]) == "y":
            print_instructions()
        print()

        return Game(players=players, decks=Decks.new(), games_played=0)

    def _print_stats(self) -> None:
        """prints the score of every player"""
        print(f"{self.stats_as_string()}")

    def stats_as_string(self) -> str:
        """returns a string of the wins, balance, and bets of every player"""
        s = ""
        for p in self.players:
            # format the presentation of player stats
            if p.player_type == PlayerType.Dealer:
                s += f"{p.get_name()} Wins:\t{p.wins}\n"
            elif p.player_type == PlayerType.Player:
                s += f"{p.get_name()} "
                s += f"Wins:\t{p.wins}\t\t"
                s += f"Balance:\t{p.balance}\t\tBet\t{p.bet}\n"
        return f"Scores:\n{s}"

    def play_game(self) -> None:
        """plays a round of blackjack"""
        game = self.games_played
        player_hands_message: str = ""

        # deal two cards to each player
        for _i in range(2):
            for player in self.players:
                player.hand.add_card(self.decks.draw_card())

        # get everyones bets
        for player in self.players:
            player.get_bet()
        scores = self.stats_as_string()

        # play game for each player
        for player in self.players:
            # turn loop, ends when player finishes their turn
            while True:
                clear()
                print_welcome_screen()
                print(f"\n\t\t\tGame {game}")
                print(scores)
                print(player_hands_message)
                print(f"{player.get_name()} Hand:\t{player.hand_as_string(True)}")

                if PlayerType.Player == player.player_type and player.bet == 0:
                    break

                # play through turn
                # check their hand value for a blackjack(21) or bust
                score = player.hand.get_total()
                if score >= 21:
                    if score == 21:
                        print("\tBlackjack! (21 points)")
                    else:
                        print(f"\tBust      ({score} points)")
                    break

                # get player move
                play = player.get_play()
                # process play
                if play == Play.Stand:
                    print(f"\t{play}")
                    break
                elif play == Play.Hit:
                    print(f"\t{play}")
                    player.hand.add_card(self.decks.draw_card())
                elif play == Play.DoubleDown:
                    print(f"\t{play}")

                    # double their balance if there's enough money,
                    # othewise go all-in
                    if player.bet * 2 < player.balance:
                        player.bet *= 2
                    else:
                        player.bet = player.balance
                    player.hand.add_card(self.decks.draw_card())
                elif play == Play.Split:
                    pass

            # add player to score cache thing
            player_hands_message += (
                f"{player.get_name()} Hand:\t{player.hand_as_string(True)}\n"
            )

        # determine winner
        top_score = 0

        # player with the highest points
        num_winners = 1

        non_burst_players = [
            player for player in self.players if player.hand.get_total() <= 21
        ]
        for player in non_burst_players:
            score = player.hand.get_total()
            if score > top_score:
                top_score = score
                num_winners = 1
            elif score == top_score:
                num_winners += 1

        # show winner(s)
        top_score_players = [
            player
            for player in non_burst_players
            if player.hand.get_total() == top_score
        ]
        for x in top_score_players:
            print(f"{x.get_name()} ")
            x.wins += 1
            # increment their wins
        if num_winners > 1:
            print(f"all tie with {top_score}\n\n\n")
        else:
            print(
                f"wins with {top_score}!\n\n\n",
            )

        # handle bets
        # remove money from losers
        losers = [
            player for player in self.players if player.hand.get_total() != top_score
        ]
        for loser in losers:
            loser.balance -= loser.bet
        # add money to winner
        winners = [
            player for player in self.players if player.hand.get_total() == top_score
        ]
        for winner in winners:
            winner.balance += winner.bet

        # discard hands
        for player in self.players:
            player.hand.discard_hand(self.decks)

        # increment games_played
        self.games_played += 1


CARD_NAMES: List[str] = [
    "ACE",
    "2",
    "3",
    "4",
    "5",
    "6",
    "7",
    "8",
    "9",
    "10",
    "JACK",
    "QUEEN",
    "KING",
]
STARTING_BALANCE: int = 100


def main() -> None:
    game: Game

    print_welcome_screen()

    # create game
    game = Game.new(
        get_number_from_user_input("How many players should there be", 1, 7)
    )

    # game loop, play game until user wants to stop
    char = "y"
    while char == "y":
        game.play_game()
        char = get_char_from_user_input("Play Again?", ["y", "n"])


def print_welcome_screen() -> None:
    print(
        """
                            BLACK JACK
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY
    \n\n"""
    )


def print_instructions() -> None:
    print(
        """
    THIS IS THE GAME OF 21. AS MANY AS 7 PLAYERS MAY PLAY THE
    GAME. ON EACH DEAL, BETS WILL BE ASKED FOR, AND THE
    PLAYERS' BETS SHOULD BE TYPED IN. THE CARDS WILL THEN BE
    DEALT, AND EACH PLAYER IN TURN PLAYS HIS HAND. THE
    FIRST RESPONSE SHOULD BE EITHER 'D', INDICATING THAT THE
    PLAYER IS DOUBLING DOWN, 'S', INDICATING THAT HE IS
    STANDING, 'H', INDICATING HE WANTS ANOTHER CARD, OR '/',
    INDICATING THAT HE WANTS TO SPLIT HIS CARDS. AFTER THE
    INITIAL RESPONSE, ALL FURTHER RESPONSES SHOULD BE 'S' OR
    'H', UNLESS THE CARDS WERE SPLIT, IN WHICH CASE DOUBLING
    DOWN IS AGAIN PERMITTED. IN ORDER TO COLLECT FOR
    BLACKJACK, THE INITIAL RESPONSE SHOULD BE 'S'.
    NUMBER OF PLAYERS

    NOTE:'/' (splitting) is not currently implemented, and does nothing

    PRESS ENTER TO CONTINUE
    """
    )
    input()


def get_number_from_user_input(prompt: str, min_value: int, max_value: int) -> int:
    """gets a int integer from user input"""
    # input loop
    user_input = None
    while user_input is None or user_input < min_value or user_input > max_value:
        raw_input = input(prompt + f" ({min_value}-{max_value})? ")

        try:
            user_input = int(raw_input)
            if user_input < min_value or user_input > max_value:
                print("Invalid input, please try again")
        except ValueError:
            print("Invalid input, please try again")
    return user_input


def get_char_from_user_input(prompt: str, valid_results: List[str]) -> str:
    """returns the first character they type"""
    user_input = None
    while user_input not in valid_results:
        user_input = input(prompt + f" {valid_results}? ").lower()
        if user_input not in valid_results:
            print("Invalid input, please try again")
    assert user_input is not None
    return user_input


def clear() -> None:
    """clear std out"""
    print("\x1b[2J\x1b[0;0H")


if __name__ == "__main__":
    main()
