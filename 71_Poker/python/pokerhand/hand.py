"""Hand implementation."""

from collections.abc import Iterator

from cards.card import Card

from .evaluation import HandEvaluation


class Hand:
    """A 5-card poker hand that acts as a simple data container."""

    def __init__(self, cards: list[Card]):
        if len(cards) != 5:
            raise ValueError("Hand must have exactly 5 cards")
        self.cards = list(cards)

    def evaluate(self) -> HandEvaluation:
        """Factory method to return a hand evaluation."""
        return HandEvaluation(self)

    def __len__(self) -> int:
        return len(self.cards)

    def __iter__(self) -> Iterator[Card]:
        return iter(self.cards)

    def __getitem__(self, i: int) -> Card:
        return self.cards[i]

    def __setitem__(self, i: int, card: Card) -> None:
        self.cards[i] = card

    def __str__(self) -> str:
        """Return a multi-line formatted description of the hand."""
        entries = [f"{i} --  {card}" for i, card in enumerate(self.cards, 1)]
        lines = [f" {entries[i]:<32}{entries[i + 1]}" for i in range(0, 4, 2)]
        lines.append(f" {entries[4]}")
        return "\n".join(lines)
