"""Hand rank enumeration."""

from enum import IntEnum, auto


class HandRank(IntEnum):
    """Poker hand ranks ordered from weakest to strongest."""

    SCHMALTZ = auto()
    PARTIAL_STRAIGHT = auto()
    PAIR = auto()
    TWO_PAIR = auto()
    THREE_OF_A_KIND = auto()
    STRAIGHT = auto()
    FLUSH = auto()
    FULL_HOUSE = auto()
    FOUR_OF_A_KIND = auto()

    def __str__(self) -> str:
        """Return the display name of the rank."""
        return {
            HandRank.SCHMALTZ: "Schmaltz",
            HandRank.PARTIAL_STRAIGHT: "Schmaltz",
            HandRank.PAIR: "a pair of",
            HandRank.TWO_PAIR: "Two pair",
            HandRank.THREE_OF_A_KIND: "Three",
            HandRank.STRAIGHT: "Straight",
            HandRank.FLUSH: "a flush in",
            HandRank.FULL_HOUSE: "Full house",
            HandRank.FOUR_OF_A_KIND: "Four",
        }[self]
