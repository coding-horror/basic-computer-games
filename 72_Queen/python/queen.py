"""Implementation of queen.bas to Python."""

from typing import Final, Optional
from random import random


########################################################################################
#                                  Optional configs
########################################################################################
# You can edit these variables to change the behavior of the game.
#
# The original implementation has a bug that allows a player to move off the board,
# e.g. start at the nonexistant space 91. Change the variable FIX_BOARD_BUG to ``True``
# to fix this behavior.
#

FIX_BOARD_BUG: Final[bool] = False

# In the original implementation, the board is only printed once. Change the variable
# SHOW_BOARD_ALWAYS to ``True`` to display the board every time.

SHOW_BOARD_ALWAYS: Final[bool] = False

# In the original implementaiton, the board is printed a bit wonky because of the
# differing widths of the numbers. Change the variable ALIGNED_BOARD to ``True`` to
# fix this.

ALIGNED_BOARD: Final[bool] = False

########################################################################################

INSTR_TXT: Final[
    str
] = """WE ARE GOING TO PLAY A GAME BASED ON ONE OF THE CHESS
MOVES.  OUR QUEEN WILL BE ABLE TO MOVE ONLY TO THE LEFT,
DOWN, OR DIAGONALLY DOWN AND TO THE LEFT.

THE OBJECT OF THE GAME IS TO PLACE THE QUEEN IN THE LOWER
LEFT HAND SQUARE BY ALTERNATING MOVES BETWEEN YOU AND THE
COMPUTER.  THE FIRST ONE TO PLACE THE QUEEN THERE WINS.

YOU GO FIRST AND PLACE THE QUEEN IN ANY ONE OF THE SQUARES
ON THE TOP ROW OR RIGHT HAND COLUMN.
THAT WILL BE YOUR FIRST MOVE.
WE ALTERNATE MOVES.
YOU MAY FORFEIT BY TYPING '0' AS YOUR MOVE.
BE SURE TO PRESS THE RETURN KEY AFTER EACH RESPONSE.

"""


WIN_MSG: Final[
    str
] = """C O N G R A T U L A T I O N S . . .

YOU HAVE WON--VERY WELL PLAYED.
IT LOOKS LIKE I HAVE MET MY MATCH.
THANKS FOR PLAYING---I CAN'T WIN ALL THE TIME.

"""

LOSE_MSG: Final[
    str
] = """
NICE TRY, BUT IT LOOKS LIKE I HAVE WON.
THANKS FOR PLAYING.

"""


def loc_to_num(location: tuple[int, int], fix_align: bool = False) -> str:
    """Convert a position given by row, column into a space number."""
    row, col = location
    out_str: str = f"{row + 8 - col}{row + 1}"
    if not fix_align or len(out_str) == 3:
        return out_str
    else:
        return out_str + " "


GAME_BOARD: Final[str] = (
    "\n"
    + "\n\n\n".join(
        "".join(f" {loc_to_num((row, col), ALIGNED_BOARD)} " for col in range(8))
        for row in range(8)
    )
    + "\n\n\n"
)


def num_to_loc(num: int) -> tuple[int, int]:
    """Convert a space number into a position given by row, column."""
    row: int = num % 10 - 1
    col: int = row + 8 - (num - row - 1) // 10
    return row, col


# The win location
WIN_LOC: Final[tuple[int, int]] = (7, 0)

# These are the places (other than the win condition) that the computer will always
# try to move into.
COMPUTER_SAFE_SPOTS: Final[frozenset[tuple[int, int]]] = frozenset(
    [
        (2, 3),
        (4, 5),
        (5, 1),
        (6, 2),
    ]
)

# These are the places that the computer will always try to move into.
COMPUTER_PREF_MOVES: Final[
    frozenset[tuple[int, int]]
] = COMPUTER_SAFE_SPOTS | frozenset([WIN_LOC])

# These are the locations (not including the win location) from which either player can
# force a win (but the computer will always choose one of the COMPUTER_PREF_MOVES).
SAFE_SPOTS: Final[frozenset[tuple[int, int]]] = COMPUTER_SAFE_SPOTS | frozenset(
    [
        (0, 4),
        (3, 7),
    ]
)


def str_with_tab(indent: int, text: str, uppercase: bool = True) -> str:
    """Create a string with ``indent`` spaces followed by ``text``."""
    if uppercase:
        text = text.upper()
    return " " * indent + text


def intro():
    """Print the intro and print instructions if desired."""
    print(str_with_tab(33, "Queen"))
    print(str_with_tab(15, "Creative Computing  Morristown, New Jersey"))
    print("\n" * 2)
    if ask("DO YOU WANT INSTRUCTIONS"):
        print(INSTR_TXT)


def get_move(current_loc: Optional[tuple[int, int]]) -> tuple[int, int]:
    """Get the next move from the player."""
    prompt: str
    player_resp: str
    move_raw: int
    new_row: int
    new_col: int
    if current_loc is None:  # It's the first turn
        prompt = "WHERE WOULD YOU LIKE TO START? "
    else:
        prompt = "WHAT IS YOUR MOVE? "
        row, col = current_loc
    while True:
        player_resp = input(prompt).strip()
        try:
            move_raw = int(player_resp)
            if move_raw == 0:  # Forfeit
                return 8, 8
            new_row, new_col = num_to_loc(move_raw)
            if current_loc is None:
                if (new_row == 0 or new_col == 7) and (
                    not FIX_BOARD_BUG or (new_col >= 0 and new_row < 8)
                ):
                    return new_row, new_col
                else:
                    prompt = (
                        "PLEASE READ THE DIRECTIONS AGAIN.\n"
                        + "YOU HAVE BEGUN ILLEGALLY.\n\n"
                        + "WHERE WOULD YOU LIKE TO START? "
                    )
            else:
                if (
                    (new_row == row and new_col < col)  # move left
                    or (new_col == col and new_row > row)  # move down
                    or (new_row - row == col - new_col)  # move diag left and down
                ) and (not FIX_BOARD_BUG or (new_col >= 0 and new_row < 8)):
                    return new_row, new_col
                else:
                    prompt = "Y O U   C H E A T . . .  TRY AGAIN? "

        except ValueError:
            prompt = "!NUMBER EXPECTED - RETRY INPUT LINE\n? "


def random_computer_move(location: tuple[int, int]) -> tuple[int, int]:
    """Make a random move."""
    row, col = location
    if (z := random()) > 0.6:
        # Move down one space
        return row + 1, col
    elif z > 0.3:
        # Move diagonaly (left and down) one space
        return row + 1, col - 1
    else:
        # Move left one space
        return row, col - 1


def computer_move(location: tuple[int, int]) -> tuple[int, int]:
    """Get the computer's move."""
    # If the player has made an optimal move, then choose a random move
    if location in SAFE_SPOTS:
        return random_computer_move(location)
    # We don't need to implmement the logic of checking for the player's win,
    # because that is checked before this function is called.
    row, col = location
    for k in range(7, 0, -1):
        # If the computer can move left k spaces and end in up in a safe spot or win,
        # do it.
        if (new_loc := (row, col - k)) in COMPUTER_PREF_MOVES:
            return new_loc
        # If the computer can move down k spaces and end up in a safe spot or win, do it.
        if (new_loc := (row + k, col)) in COMPUTER_PREF_MOVES:
            return new_loc
        # If the computer can move diagonally k spaces and end up in a safe spot or win,
        # do it.
        if (new_loc := (row + k, col - k)) in COMPUTER_PREF_MOVES:
            return new_loc
        # As a fallback, do a random move. (NOTE: This shouldn't actally happen--it
        # should always be possible to make an optimal move if the player doesn't play
        # in a location in SAFE_SPOTS.
    return random_computer_move(location)


def main_game() -> None:
    """Execute the main game."""
    game_over: bool = False
    location: Optional[tuple[int, int]] = None  # Indicate it is the first turn
    while not game_over:
        location = get_move(location)
        if location == (8, 8):  # (8, 8) is returned when the player enters 0
            print("\nIT LOOKS LIKE I HAVE WON BY FORFEIT.\n")
            game_over = True
        elif location == WIN_LOC:  # Player wins (in lower left corner)
            print(WIN_MSG)
            game_over = True
        else:
            location = computer_move(location)
            print(f"COMPUTER MOVES TO SQUARE {loc_to_num(location)}")
            if location == WIN_LOC:  # Computer wins (in lower left corner)
                print(LOSE_MSG)
                game_over = True
        # The default behavior is not to show the board each turn, but
        # this can be modified by changing a flag at the start of the file.
        if not game_over and SHOW_BOARD_ALWAYS:
            print(GAME_BOARD)


def ask(prompt: str) -> bool:
    """Ask a yes/no question until user gives an understandable response."""
    inpt: str
    while True:
        # Normalize input to uppercase, no whitespace, then get first character
        inpt = input(prompt + "? ").upper().strip()[0]
        print()
        if inpt == "Y":
            return True
        elif inpt == "N":
            return False
        print("PLEASE ANSWER 'YES' OR 'NO'.")
    return False


if __name__ == "__main__":
    intro()
    still_playing: bool = True
    while still_playing:
        print(GAME_BOARD)
        main_game()
        still_playing = ask("ANYONE ELSE CARE TO TRY")
    print("\nOK --- THANKS AGAIN.")
