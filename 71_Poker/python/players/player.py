"""Base Player class for both Human and Dealer."""

from enum import Enum, auto

from cards import Card
from pokerhand import Hand


class Player:
    """Base class representing a participant in the poker game."""

    money: int
    bet: int
    hand: Hand

    class Action(Enum):
        """Standard actions a player can take."""

        FOLD = auto()
        CHECK = auto()
        CALL = auto()
        RAISE = auto()

    def __init__(self, money: int) -> None:
        """Initialize the player with a starting bankroll."""
        self.money = money
        self.bet = 0

    def receive_new_hand(self, hand: Hand) -> None:
        """Reset per-round state and receive a new hand."""
        self.bet = 0
        self.hand = hand

    def pay_ante(self, amount: int) -> None:
        """Pay the ante."""
        self.money -= amount

    def win_pot(self, amount: int) -> None:
        """Add the pot to the player's money."""
        self.money += amount

    def settle_bet(self) -> int:
        """Move the current bet out of the player's money. Returns the bet amount."""
        amount = self.bet
        self.money -= amount
        self.bet = 0
        return amount

    def replace_card(self, index: int, card: Card) -> None:
        """Replace a card in the player's hand."""
        self.hand[index] = card
