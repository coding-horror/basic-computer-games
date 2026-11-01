"""
From: BASIC Computer Games (1978)
Edited by David H. Ahl

You and the computer are opponents in this game of draw poker.
At the start of the game, each player is given $200. The game
ends when either player runs out of money, although if you go
broke the computer will offer to buy back your wristwatch or
diamond tie tack.

The computer opens the betting before the draw; you open the
betting after the draw. If you don't have a hand that's worth
anything and you want to fold, bet 0. Prior to the draw, to
check the draw, you may bet .5. Of course, if the computer has
made a bet, you must match it in order to draw or, if you have
a good hand, you may raise the bet at any time.

The author is A. Christopher Hall of Trinity College,
Hartford, Connecticut.


Python port by Alex Conconi, 2026.
"""

from game import GameOver, PokerGame


def main() -> None:
    """Entry point for the poker game application."""
    game = PokerGame()
    try:
        game.run()
    except GameOver:
        pass


if __name__ == "__main__":
    main()
