"""Dealer state and decision-making logic."""

import random
from enum import Enum, auto

from cards import CardRank, Deck
from pokerhand import Hand, HandEvaluation, HandRank

from .player import Player


class Dealer(Player):
    """Dealer state: bankroll, strategy, and per-round betting."""

    class Strategy(Enum):
        """The dealer's play style for the current round, set during the opening."""

        NORMAL = auto()
        WEAK = auto()
        BLUFFING = auto()

    strategy: Strategy
    bet_base: int
    was_bluffing: bool
    hand_is_weak: bool
    bluff_discard_count: int | None

    def receive_new_hand(self, hand: Hand) -> None:
        """Reset per-round state and receive a new hand."""
        super().receive_new_hand(hand)
        self.strategy = Dealer.Strategy.NORMAL
        self.bet_base = 0
        self.was_bluffing = False
        self.hand_is_weak = False
        self.bluff_discard_count = None

    def decide_postdraw_bet(self) -> None:
        """Update bluffing state, evaluate hand and set betting tier."""
        self.was_bluffing = self.strategy == Dealer.Strategy.BLUFFING

        result = self.hand.evaluate()
        rank = result.hand_rank
        self.bet = 0
        self.hand_is_weak = self._is_hand_weak(result, is_redeal=True)

        if self.was_bluffing:
            self.bet_base = 28
        elif self.hand_is_weak:
            self.bet_base = 1
        elif rank < HandRank.THREE_OF_A_KIND:
            self.bet_base = 2
            if self._chance(1):
                self.bet_base = 19
        elif rank <= HandRank.FLUSH:  # THREE_OF_A_KIND, STRAIGHT, FLUSH
            self.bet_base = 19
            if self._chance(1):
                self.bet_base = 11
        else:  # FULL_HOUSE, FOUR_OF_A_KIND
            self.bet_base = 2

    def _is_hand_weak(self, result: HandEvaluation, is_redeal: bool) -> bool:
        """Check whether the hand qualifies as 'weak'."""
        rank = result.hand_rank
        if rank < HandRank.PAIR:
            # Schmaltz is always weak; partial straight only on re-deal
            return rank != HandRank.PARTIAL_STRAIGHT or is_redeal
        if rank <= HandRank.TWO_PAIR:
            # Pair/two-pair is weak only when high card is Eight or lower
            return result.high_card.rank <= CardRank.EIGHT
        return False

    def get_opening_action(self) -> Player.Action:
        """Evaluate hand and set the dealer's pre-draw opening bet and strategy."""
        result = self.hand.evaluate()

        if self._is_hand_weak(result, is_redeal=False):
            strat, base, discard, action = self._decide_weak_opening()
        else:
            strat, base, discard, action = self._decide_normal_opening(result)

        self.strategy = strat
        self.bet_base = base
        self.bluff_discard_count = discard

        if action == Player.Action.RAISE:
            self.bet = self.bet_base + random.randint(0, 9)
        else:
            self.bet = 0

        return action

    def _decide_weak_opening(
        self,
    ) -> tuple[Strategy, int, int | None, Player.Action]:
        """Pure logic for weak-hand opening decisions."""
        if self._chance(8):
            return Dealer.Strategy.BLUFFING, 23, 2, Player.Action.RAISE
        if self._chance(8):
            return Dealer.Strategy.BLUFFING, 23, 1, Player.Action.RAISE
        if self._chance(9):
            return Dealer.Strategy.WEAK, 1, None, Player.Action.CHECK
        return Dealer.Strategy.BLUFFING, 23, 0, Player.Action.RAISE

    def _decide_normal_opening(
        self, result: HandEvaluation
    ) -> tuple[Strategy, int, int | None, Player.Action]:
        """Pure logic for normal-hand opening decisions."""
        rank = result.hand_rank
        if rank < HandRank.THREE_OF_A_KIND:
            if self._chance(8):
                return Dealer.Strategy.NORMAL, 0, None, Player.Action.CHECK
            return Dealer.Strategy.BLUFFING, 23, None, Player.Action.RAISE

        # Strong hand (THREE_OF_A_KIND and above)
        if rank > HandRank.FULL_HOUSE:  # FOUR_OF_A_KIND
            base = 2 if self._chance(9) else 35
        else:
            base = 35
        return Dealer.Strategy.NORMAL, base, None, Player.Action.RAISE

    def discard_and_draw(self, deck: Deck) -> int:
        """Evaluate hand and replace the dealer's discard cards."""
        if self.bluff_discard_count is not None:
            count = self.bluff_discard_count
            if count == 0:
                return 0
            ranked = sorted(range(5), key=lambda i: self.hand[i].rank.value)
            for i in ranked[:count]:
                self.replace_card(i, deck.deal())
            return count

        result = self.hand.evaluate()
        for i in result.discard_indices:
            self.replace_card(i, deck.deal())
        return len(result.discard_indices)

    def get_check_response(self) -> Player.Action:
        """Respond to a player's check. Returns the dealer's action."""
        if not self.was_bluffing and self.hand_is_weak:
            return Player.Action.CHECK

        self.bet = self.bet_base + random.randint(0, 9)
        return Player.Action.RAISE

    def get_raise_response(self, player_paid: int) -> Player.Action:
        """Respond to a player's raise. Returns the dealer's action."""
        if self.bet_base != 1:
            if player_paid > 3 * self.bet_base:
                if self.bet_base == 2 and self._chance(1):
                    return self._raise(player_paid)
                return self._call(player_paid)
            return self._raise(player_paid)

        if player_paid > 5:
            return Player.Action.FOLD

        if player_paid > 3:
            return self._call(player_paid)

        return self._raise(player_paid)

    def _call(self, player_paid: int) -> Player.Action:
        """Dealer calls the player's bet."""
        self.bet = player_paid
        return Player.Action.CALL

    def _raise(self, player_paid: int) -> Player.Action:
        """Raise the player's bet."""
        raise_amount = player_paid - self.bet + random.randint(0, 9)
        if raise_amount <= 0:
            return self._call(player_paid)

        self.bet = player_paid + raise_amount
        return Player.Action.RAISE

    def _chance(self, n: int) -> bool:
        """Return True with probability n/10."""
        return random.random() < n / 10
