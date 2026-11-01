"""Card primitives: Suits, Ranks, and the Card class."""

from dataclasses import dataclass
from enum import Enum, IntEnum


class CardSuit(Enum):
    """Enumeration of card suits."""

    CLUBS = 0
    DIAMONDS = 1
    HEARTS = 2
    SPADES = 3

    def __str__(self) -> str:
        return self.name.capitalize()


class CardRank(IntEnum):
    """Card ranks from Two (0) to Ace (12)."""

    TWO = 0
    THREE = 1
    FOUR = 2
    FIVE = 3
    SIX = 4
    SEVEN = 5
    EIGHT = 6
    NINE = 7
    TEN = 8
    JACK = 9
    QUEEN = 10
    KING = 11
    ACE = 12

    def __str__(self) -> str:
        if self.value <= 8:  # TWO(0) through TEN(8)
            return f" {str(self.value + 2)} "
        return ("Jack", "Queen", "King", "Ace")[self.value - 9]


@dataclass(frozen=True)
class Card:
    """A single playing card."""

    suit: CardSuit
    rank: CardRank

    def __str__(self) -> str:
        return f"{self.rank} of {self.suit}"
