"""Human player state and behavior."""

from .player import Player


class Human(Player):
    """
    The human player in the game.

    Note: As a state container, this class has no logic methods;
    decision-making is provided by the user via the game controller.
    """

    has_watch: bool
    has_tack: bool

    def __init__(self, money: int) -> None:
        """Initialize the human player with starting assets."""
        super().__init__(money)
        self.has_watch = True
        self.has_tack = True
