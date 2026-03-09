"""Poker game logic — port of the 1978 Creative Computing BASIC original."""

import random
from typing import NoReturn

from cards import Deck
from players import Dealer, Human, Player
from pokerhand import Hand


class GameOver(Exception):
    """Raised when the game ends (either side busted, player quits, or walks away)."""


class PokerGame:  # pylint: disable=too-few-public-methods
    """Five-card draw poker against the house."""

    INITIAL_MONEY: int = 200
    ANTE: int = 5

    human: Human
    dealer: Dealer
    deck: Deck
    pot: int

    def __init__(self) -> None:
        """Initialise game state and allocate both sides' starting stack."""
        self.human = Human(PokerGame.INITIAL_MONEY)
        self.dealer = Dealer(PokerGame.INITIAL_MONEY)
        self.deck: Deck
        self.pot = 0

    # ------------------------------------------------------------------
    # Intro and main loop
    # ------------------------------------------------------------------

    def run(self) -> None:
        """Execute the primary game loop until a GameOver exception occurs."""
        self._display_intro()
        while True:
            self._play_round()

    # ------------------------------------------------------------------
    # Round
    # ------------------------------------------------------------------

    def _play_round(self) -> None:
        """Deal one complete round: ante, bet, draw, bet, showdown."""
        print()

        # Reset per-round state
        self.deck = Deck()

        # --- Ante ---
        if self.dealer.money <= self.ANTE:
            self._dealer_goes_bust()

        print(f"The ante is ${self.ANTE}.  I will deal:\n")

        if self.human.money <= self.ANTE:
            self._human_try_raise_funds(self.ANTE)

        self.pot += self.ANTE * 2
        self.human.pay_ante(self.ANTE)
        self.dealer.pay_ante(self.ANTE)

        # --- Deal ---
        self.human.receive_new_hand(Hand([self.deck.deal() for _ in range(5)]))
        self.dealer.receive_new_hand(Hand([self.deck.deal() for _ in range(5)]))

        print("Your hand:")
        print(self.human.hand)
        print()

        # --- Opening bet ---
        dealer_action = self.dealer.get_opening_action()
        if self.dealer.money < self.dealer.bet:
            self._dealer_try_raise_funds()

        if dealer_action == Player.Action.CHECK:
            print("I check.")
        else:
            print(f"I'll open with $ {self.dealer.bet}")

        if not self._conduct_betting_round(post_draw=False):
            return

        # --- Draw ---
        print()
        self._player_discard_draw()
        draw_count = self.dealer.discard_and_draw(self.deck)
        print(
            f"\nI am taking {draw_count} card{'s' if draw_count != 1 else ''}"
        )

        # --- Post-draw bet ---
        self.dealer.decide_postdraw_bet()
        if not self._conduct_betting_round(post_draw=True):
            return

        # --- Showdown ---
        self._showdown()

    # ------------------------------------------------------------------
    # Player draw
    # ------------------------------------------------------------------

    def _player_discard_draw(self) -> None:
        """Ask the player how many cards to replace, then deal replacements."""
        count = self._read_int(
            "Now we draw -- how many cards do you want? ",
            low=0,
            high=3,
            too_high_msg="You can't draw more than three cards.",
        )

        if count == 0:
            return

        print("What are their numbers:")
        for _ in range(count):
            pos = self._read_int("", low=1, high=5)
            self.human.replace_card(pos - 1, self.deck.deal())

        print("Your new hand:")
        print(self.human.hand)

    # ------------------------------------------------------------------
    # Betting round
    # ------------------------------------------------------------------

    def _conduct_betting_round(self, post_draw: bool) -> bool:
        """
        Executes a betting round.
        Returns True if the betting phase completed (hand continues).
        Returns False if a player folded (hand ends).
        """
        while True:
            human_action = self._get_human_action()

            match human_action:
                case Player.Action.FOLD:
                    # Human fold always ends betting loop (hand ends)
                    self._settle_bets()
                    self._award_pot(dealer_wins=True)
                    return False

                case Player.Action.CALL:
                    # Human call always ends betting loop (proceed to next phase)
                    self._settle_bets()
                    return True

                case Player.Action.CHECK:
                    # Pre-draw: human check ends human's turn.
                    # Post-draw: dealer gets a chance to bet/check back.
                    if not post_draw or self._handle_human_post_draw_check():
                        return True
                    # If dealer bet, we fall through and loop for human response

                case Player.Action.RAISE:
                    if self._handle_human_raise():
                        return True
                    # If dealer re-raised, we fall through and loop for human response

                case _:
                    raise ValueError(f"Unknown player action: {human_action}")

    def _handle_human_post_draw_check(self) -> bool:
        """Process dealer response to human check. Returns True if phase ends."""
        dealer_action = self.dealer.get_check_response()
        if self.dealer.money < self.dealer.bet:
            self._dealer_try_raise_funds()

        if dealer_action == Player.Action.CHECK:
            print("I'll check.")
            self._settle_bets()
            return True

        print(f"I'll bet $ {self.dealer.bet}")
        return False

    def _handle_human_raise(self) -> bool:
        """Process dealer response to human raise. Returns True if phase ends."""
        dealer_action = self.dealer.get_raise_response(self.human.bet)
        if self.dealer.money < self.dealer.bet:
            self._dealer_try_raise_funds()

        match dealer_action:
            case Player.Action.FOLD:
                print("I fold.")
                self._settle_bets()
                self._award_pot(dealer_wins=False)
                return True  # Round ends

            case Player.Action.CALL:
                print("I'll see you.")
                self._settle_bets()
                return True

            case Player.Action.RAISE:
                raise_amount = self.dealer.bet - self.human.bet
                print(f"I'll see you, and raise you {raise_amount}")
                return False

            case _:
                raise ValueError(f"Unknown dealer action: {dealer_action}")

    def _get_human_action(self) -> Player.Action:
        """Prompt user for a bet and return the resulting Action."""
        while True:
            print()
            raw = input("What is your bet? ").strip()

            try:
                bet = float(raw)
            except ValueError:
                continue

            # Fractional bet: only .5 is valid, and only as a check
            if bet != int(bet):
                if self.dealer.bet == 0 and self.human.bet == 0 and bet == 0.5:
                    return Player.Action.CHECK
                print("No small change, please.")
                continue

            amount = int(bet)

            if amount == 0:
                return Player.Action.FOLD

            if self.human.bet + amount > self.human.money:
                self._human_try_raise_funds(self.human.bet + amount)
                continue

            if self.human.bet + amount < self.dealer.bet:
                print("If you can't see my bet, then fold.")
                continue

            self.human.bet += amount

            if self.human.bet == self.dealer.bet:
                return Player.Action.CALL
            return Player.Action.RAISE

    def _dealer_try_raise_funds(self) -> None:
        """Attempt to raise funds for the dealer. Raises GameOver if failed."""
        if not self.human.has_watch:
            if self._read_yes_no(
                "Would you like to buy back your watch for $50? "
            ):
                self.dealer.money += 50
                self.human.has_watch = True
                if self.dealer.money >= self.dealer.bet:
                    return

        if not self.human.has_tack:
            if self._read_yes_no(
                "Would you like to buy back your tie tack for $50? "
            ):
                self.dealer.money += 50
                self.human.has_tack = True
                if self.dealer.money >= self.dealer.bet:
                    return

        self._dealer_goes_bust()

    def _dealer_goes_bust(self) -> NoReturn:
        """Inform the player and terminate the game."""
        print("I'm busted.  Congratulations!")
        raise GameOver

    def _human_try_raise_funds(self, target_money: int) -> None:
        """Offer the player's assets for cash. Raises GameOver if short of target."""
        print("\nYou can't bet with what you haven't got.")

        if self.human.has_watch:
            if self._read_yes_no("Would you like to sell your watch? "):
                if self._chance(3):
                    print("That's a pretty crummy watch - I'll give you $25.")
                    self.human.money += 25
                else:
                    print("I'll give you $75 for it.")
                    self.human.money += 75
                self.human.has_watch = False
                if self.human.money >= target_money:
                    return

        if self.human.has_tack:
            if self._read_yes_no("Will you part with that diamond tie tack? "):
                if self._chance(4):
                    print("It's paste.  $25.")
                    self.human.money += 25
                else:
                    print("You are now $100 richer.")
                    self.human.money += 100
                self.human.has_tack = False
                if self.human.money >= target_money:
                    return

        print("Your wad is shot.  So long, sucker!")
        raise GameOver

    def _settle_bets(self) -> None:
        """Move both sides' committed chips into the pot."""
        self.pot += self.human.settle_bet()
        self.pot += self.dealer.settle_bet()

    # ------------------------------------------------------------------
    # Showdown
    # ------------------------------------------------------------------

    def _showdown(self) -> None:
        """Reveal hands, compare scores, and award the pot."""
        print("\nNow we compare hands:")

        print("My hand:")
        print(self.dealer.hand)
        dealer_hand_eval = self.dealer.hand.evaluate()

        player_hand_eval = self.human.hand.evaluate()

        print(f"\nYou have {player_hand_eval}")
        print(f"And I have {dealer_hand_eval}")

        if dealer_hand_eval == player_hand_eval:
            print("The hand is drawn.")
            print(f"All ${self.pot} remains in the pot.")
        else:
            self._award_pot(dealer_wins=dealer_hand_eval > player_hand_eval)

    def _award_pot(self, dealer_wins: bool) -> None:
        """Announce the winner, award the pot, and prompt to continue."""
        if dealer_wins:
            print("I win.")
            self.dealer.win_pot(self.pot)
        else:
            print("You win.")
            self.human.win_pot(self.pot)
        print(
            f"Now I have $ {self.dealer.money} and you have $ {self.human.money}"
        )
        if not self._read_yes_no("Do you wish to continue? "):
            raise GameOver
        self.pot = 0  # Reset pot after awarding it

    # ------------------------------------------------------------------
    # Helpers
    # ------------------------------------------------------------------

    # pylint: disable=too-many-arguments
    def _read_int(
        self,
        prompt: str,
        *,
        low: int | None = None,
        high: int | None = None,
        too_low_msg: str | None = None,
        too_high_msg: str | None = None,
    ) -> int:
        """Read and validate an integer within an optional range."""
        while True:
            raw = input(prompt).strip()
            if not raw:
                continue
            try:
                val = int(raw)
            except ValueError:
                continue

            if low is not None and val < low:
                if too_low_msg:
                    print(too_low_msg)
                continue

            if high is not None and val > high:
                if too_high_msg:
                    print(too_high_msg)
                continue

            return val

    def _read_yes_no(self, prompt: str) -> bool:
        """Read yes/no response. Returns True for yes, False for no."""
        while True:
            response = input(prompt).strip().lower()
            if response in ["y", "yes"]:
                return True
            if response in ["n", "no"]:
                return False
            print("Answer yes or no, please.")

    def _chance(self, n: int) -> bool:
        """Return True with probability n/10."""
        return random.random() < n / 10

    def _display_intro(self) -> None:
        """Print the welcome banner."""
        print("                                POKER")
        print("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n")
        print(
            f"Welcome to the casino.  We each have ${PokerGame.INITIAL_MONEY}."
        )
        print("I will open the betting before the draw; you open after.")
        print("To fold bet 0; to check bet .5.")
        print("Enough talk -- let's get down to business.")
