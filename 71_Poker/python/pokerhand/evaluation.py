"""Hand evaluation logic."""

import functools
from collections import Counter
from collections.abc import Callable
from typing import TYPE_CHECKING

from cards.card import Card, CardRank

from .rank import HandRank

if TYPE_CHECKING:
    from .hand import Hand


@functools.total_ordering
class HandEvaluation:
    """
    Evaluator and Result container for a poker hand.

    When initialized with a Hand, it immediately performs the evaluation
    and stores the resulting rank, high card, and recommended discards.
    """

    hand_rank: HandRank
    high_card: Card
    discard_indices: list[int]
    _ctx: "_EvaluationContext"

    def __init__(self, hand: "Hand"):
        """Perform evaluation of the provided hand."""
        self.discard_indices = []
        self._evaluate(hand)

    def __str__(self) -> str:
        rank = self.hand_rank
        card = self.high_card

        if rank in (HandRank.SCHMALTZ, HandRank.PARTIAL_STRAIGHT):
            return f"{rank}, {card.rank} high"
        if rank in (
            HandRank.PAIR,
            HandRank.TWO_PAIR,
            HandRank.THREE_OF_A_KIND,
            HandRank.FULL_HOUSE,
            HandRank.FOUR_OF_A_KIND,
        ):
            return f"{rank} {card.rank}'s"
        if rank == HandRank.STRAIGHT:
            return f"{rank}, {card.rank} high"
        if rank == HandRank.FLUSH:
            return f"{rank} {card.suit}"
        return f"{rank} {card.rank}"

    def __lt__(self, other: "HandEvaluation") -> bool:
        return (self.hand_rank, self.high_card.rank.value) < (
            other.hand_rank,
            other.high_card.rank.value,
        )

    def __eq__(self, other: object) -> bool:
        # this object could be compared to any other object with ==
        if not isinstance(other, HandEvaluation):
            return NotImplemented
        return (self.hand_rank, self.high_card.rank.value) == (
            other.hand_rank,
            other.high_card.rank.value,
        )

    def __hash__(self) -> int:
        return hash((self.hand_rank, self.high_card.rank.value))

    class _EvaluationContext:  # pylint: disable=too-few-public-methods
        """Intermediate calculations for hand evaluation."""

        def __init__(self, hand: "Hand"):
            self.cards = hand.cards
            self.rank_indexed = sorted(
                enumerate(self.cards), key=lambda item: item[1].rank.value
            )

            # Frequency analysis
            rank_counts = Counter(c.rank for c in self.cards)
            sorted_groups = sorted(
                rank_counts.items(),
                key=lambda rc: (rc[1], rc[0].value),
                reverse=True,
            )
            self.freq_pattern = tuple(count for _, count in sorted_groups)
            self.ranks_by_freq = [rank for rank, _ in sorted_groups]
            self.sorted_ranks: list[CardRank] = [
                c.rank for _, c in self.rank_indexed
            ]

    def _evaluate(self, hand: "Hand") -> None:
        """Internal algorithm to determine hand strength."""
        self._ctx = self._EvaluationContext(hand)

        checks: list[Callable[[], bool]] = [
            self._check_four_of_a_kind,
            self._check_full_house,
            self._check_flush,
            self._check_straight,
            self._check_three_of_a_kind,
            self._check_two_pair,
            self._check_pair,
            self._check_partial_straight,
            self._check_schmaltz,
        ]

        for check in checks:
            if check():
                break

    def _check_four_of_a_kind(self) -> bool:
        if self._ctx.freq_pattern != (4, 1):
            return False
        self.hand_rank = HandRank.FOUR_OF_A_KIND
        self.high_card = next(
            c
            for _, c in reversed(self._ctx.rank_indexed)
            if c.rank == self._ctx.ranks_by_freq[0]
        )
        return True

    def _check_full_house(self) -> bool:
        if self._ctx.freq_pattern != (3, 2):
            return False
        self.hand_rank = HandRank.FULL_HOUSE
        self.high_card = next(
            c
            for _, c in reversed(self._ctx.rank_indexed)
            if c.rank == self._ctx.ranks_by_freq[0]
        )
        return True

    def _check_flush(self) -> bool:
        if len({c.suit for c in self._ctx.cards}) != 1:
            return False
        self.hand_rank = HandRank.FLUSH
        self.high_card = self._ctx.rank_indexed[4][1]
        return True

    def _check_straight(self) -> bool:
        if (
            len(set(self._ctx.sorted_ranks)) == 5
            and self._ctx.sorted_ranks[4].value
            - self._ctx.sorted_ranks[0].value
            == 4
        ):
            self.hand_rank = HandRank.STRAIGHT
            self.high_card = self._ctx.rank_indexed[4][1]
            return True
        return False

    def _check_three_of_a_kind(self) -> bool:
        if self._ctx.freq_pattern != (3, 1, 1):
            return False
        self.hand_rank = HandRank.THREE_OF_A_KIND
        self.high_card = next(
            c
            for _, c in reversed(self._ctx.rank_indexed)
            if c.rank == self._ctx.ranks_by_freq[0]
        )
        keep_rank = self._ctx.ranks_by_freq[0]
        self.discard_indices = [
            i for i, c in enumerate(self._ctx.cards) if c.rank != keep_rank
        ]
        return True

    def _check_two_pair(self) -> bool:
        if self._ctx.freq_pattern != (2, 2, 1):
            return False
        self.hand_rank = HandRank.TWO_PAIR
        self.high_card = next(
            c
            for _, c in reversed(self._ctx.rank_indexed)
            if c.rank == self._ctx.ranks_by_freq[0]
        )
        keep = {self._ctx.ranks_by_freq[0], self._ctx.ranks_by_freq[1]}
        self.discard_indices = [
            i for i, c in enumerate(self._ctx.cards) if c.rank not in keep
        ]
        return True

    def _check_pair(self) -> bool:
        if self._ctx.freq_pattern != (2, 1, 1, 1):
            return False
        self.hand_rank = HandRank.PAIR
        self.high_card = next(
            c
            for _, c in reversed(self._ctx.rank_indexed)
            if c.rank == self._ctx.ranks_by_freq[0]
        )
        keep_rank = self._ctx.ranks_by_freq[0]
        self.discard_indices = [
            i for i, c in enumerate(self._ctx.cards) if c.rank != keep_rank
        ]
        return True

    def _check_partial_straight(self) -> bool:
        if (
            self._ctx.sorted_ranks[3].value - self._ctx.sorted_ranks[0].value
            == 3
        ):
            self.hand_rank = HandRank.PARTIAL_STRAIGHT
            self.high_card = self._ctx.rank_indexed[3][1]
            self.discard_indices = [self._ctx.rank_indexed[4][0]]
            return True
        if (
            self._ctx.sorted_ranks[4].value - self._ctx.sorted_ranks[1].value
            == 3
        ):
            self.hand_rank = HandRank.PARTIAL_STRAIGHT
            self.high_card = self._ctx.rank_indexed[4][1]
            self.discard_indices = [self._ctx.rank_indexed[0][0]]
            return True
        return False

    def _check_schmaltz(self) -> bool:
        self.hand_rank = HandRank.SCHMALTZ
        self.high_card = self._ctx.rank_indexed[4][1]
        self.discard_indices = [idx for idx, _ in self._ctx.rank_indexed[0:3]]
        return True
