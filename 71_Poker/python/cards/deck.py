"""Deck implementation."""

import random

from .card import Card, CardRank, CardSuit


class Deck:  # pylint: disable=too-few-public-methods
    """Standard 52-card deck, shuffled on creation."""

    def __init__(self) -> None:
        self._cards = [Card(suit, rank) for suit in CardSuit for rank in CardRank]
        random.shuffle(self._cards)

    def deal(self) -> Card:
        """Remove and return the top card."""
        return self._cards.pop()
