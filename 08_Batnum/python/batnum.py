from enum import IntEnum
from typing import Tuple, Any


class WinOptions(IntEnum):
    Undefined = 0
    TakeLast = 1
    AvoidLast = 2

    @classmethod
    def _missing_(cls, value: Any) -> "WinOptions":
        try:
            int_value = int(value)
        except Exception:
            return WinOptions.Undefined
        if int_value == 1:
            return WinOptions.TakeLast
        elif int_value == 2:
            return WinOptions.AvoidLast
        else:
            return WinOptions.Undefined


class StartOptions(IntEnum):
    Undefined = 0
    ComputerFirst = 1
    PlayerFirst = 2

    @classmethod
    def _missing_(cls, value: Any) -> "StartOptions":
        try:
            int_value = int(value)
        except Exception:
            return StartOptions.Undefined
        if int_value == 1:
            return StartOptions.ComputerFirst
        elif int_value == 2:
            return StartOptions.PlayerFirst
        else:
            return StartOptions.Undefined


def print_intro() -> None:
    """Prints out the introduction and rules for the game."""
    print("BATNUM".rjust(33, " "))
    print("CREATIVE COMPUTING  MORRISSTOWN, NEW JERSEY".rjust(15, " "))
    print()
    print()
    print()
    print("THIS PROGRAM IS A 'BATTLE OF NUMBERS' GAME, WHERE THE")
    print("COMPUTER IS YOUR OPPONENT.")
    print()
    print("THE GAME STARTS WITH AN ASSUMED PILE OF OBJECTS. YOU")
    print("AND YOUR OPPONENT ALTERNATELY REMOVE OBJECTS FROM THE PILE.")
    print("WINNING IS DEFINED IN ADVANCE AS TAKING THE LAST OBJECT OR")
    print("NOT. YOU CAN ALSO SPECIFY SOME OTHER BEGINNING CONDITIONS.")
    print("DON'T USE ZERO, HOWEVER, IN PLAYING THE GAME.")
    print("ENTER A NEGATIVE NUMBER FOR NEW PILE SIZE TO STOP PLAYING.")
    print()
    return


def get_params() -> Tuple[int, int, int, StartOptions, WinOptions]:
    """This requests the necessary parameters to play the game.

    Returns a set with the five game parameters:
        pileSize - the starting size of the object pile
        minSelect - minimum selection that can be made on each turn
        maxSelect - maximum selection that can be made on each turn
        startOption - 1 if the computer is first
                      or 2 if the player is first
        winOption - 1 if the goal is to take the last object
                    or 2 if the goal is to not take the last object
    """
    pile_size = get_pile_size()
    if pile_size < 0:
        return (-1, 0, 0, StartOptions.Undefined, WinOptions.Undefined)
    win_option = get_win_option()
    min_select, max_select = get_min_max()
    start_option = get_start_option()
    return (pile_size, min_select, max_select, start_option, win_option)


def get_pile_size() -> int:
    # A negative number will stop the game.
    pile_size = 0
    while pile_size == 0:
        try:
            pile_size = int(input("ENTER PILE SIZE "))
        except ValueError:
            pile_size = 0
    return pile_size


def get_win_option() -> WinOptions:
    win_option: WinOptions = WinOptions.Undefined
    while win_option == WinOptions.Undefined:
        win_option = WinOptions(input("ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST: "))  # type: ignore
    return win_option


def get_min_max() -> Tuple[int, int]:
    min_select = 0
    max_select = 0
    while min_select < 1 or max_select < 1 or min_select > max_select:
        (min_select, max_select) = (
            int(x) for x in input("ENTER MIN AND MAX ").split(" ")
        )
    return min_select, max_select


def get_start_option() -> StartOptions:
    start_option: StartOptions = StartOptions.Undefined
    while start_option == StartOptions.Undefined:
        start_option = StartOptions(input("ENTER START OPTION - 1 COMPUTER FIRST, 2 YOU FIRST "))  # type: ignore
    return start_option


def player_move(
    pile_size: int, min_select: int, max_select: int, win_option: WinOptions
) -> Tuple[bool, int]:
    """This handles the player's turn - asking the player how many objects
    to take and doing some basic validation around that input.  Then it
    checks for any win conditions.

    Returns a boolean indicating whether the game is over and the new pileSize."""
    player_done = False
    while not player_done:
        player_move = int(input("YOUR MOVE "))
        if player_move == 0:
            print("I TOLD YOU NOT TO USE ZERO!  COMPUTER WINS BY FORFEIT.")
            return (True, pile_size)
        if player_move > max_select or player_move < min_select:
            print("ILLEGAL MOVE, REENTER IT")
            continue
        pile_size = pile_size - player_move
        player_done = True
        if pile_size <= 0:
            if win_option == WinOptions.AvoidLast:
                print("TOUGH LUCK, YOU LOSE.")
            else:
                print("CONGRATULATIONS, YOU WIN.")
            return (True, pile_size)
    return (False, pile_size)


def computer_pick(
    pile_size: int, min_select: int, max_select: int, win_option: WinOptions
) -> int:
    """This handles the logic to determine how many objects the computer
    will select on its turn.
    """
    q = pile_size - 1 if win_option == WinOptions.AvoidLast else pile_size
    c = min_select + max_select
    computer_pick = q - (c * int(q / c))
    if computer_pick < min_select:
        computer_pick = min_select
    if computer_pick > max_select:
        computer_pick = max_select
    return computer_pick


def computer_move(
    pile_size: int, min_select: int, max_select: int, win_option: WinOptions
) -> Tuple[bool, int]:
    """This handles the computer's turn - first checking for the various
    win/lose conditions and then calculating how many objects
    the computer will take.

    Returns a boolean indicating whether the game is over and the new pileSize."""
    # First, check for win conditions on this move
    # In this case, we win by taking the last object and
    # the remaining pile is less than max select
    # so the computer can grab them all and win
    if win_option == WinOptions.TakeLast and pile_size <= max_select:
        print(f"COMPUTER TAKES {pile_size} AND WINS.")
        return (True, pile_size)
    # In this case, we lose by taking the last object and
    # the remaining pile is less than minsize and the computer
    # has to take all of them.
    if win_option == WinOptions.AvoidLast and pile_size <= min_select:
        print(f"COMPUTER TAKES {min_select} AND LOSES.")
        return (True, pile_size)

    # Otherwise, we determine how many the computer selects
    curr_sel = computer_pick(pile_size, min_select, max_select, win_option)
    pile_size = pile_size - curr_sel
    print(f"COMPUTER TAKES {curr_sel} AND LEAVES {pile_size}")
    return (False, pile_size)


def play_game(
    pile_size: int,
    min_select: int,
    max_select: int,
    start_option: StartOptions,
    win_option: WinOptions,
) -> None:
    """This is the main game loop - repeating each turn until one
    of the win/lose conditions is met.
    """
    game_over = False
    # playersTurn is a boolean keeping track of whether it's the
    # player's or computer's turn
    players_turn = start_option == StartOptions.PlayerFirst

    while not game_over:
        if players_turn:
            (game_over, pile_size) = player_move(
                pile_size, min_select, max_select, win_option
            )
            players_turn = False
            if game_over:
                return
        if not players_turn:
            (game_over, pile_size) = computer_move(
                pile_size, min_select, max_select, win_option
            )
            players_turn = True


def main() -> None:
    while True:
        print_intro()
        (pile_size, min_select, max_select, start_option, win_option) = get_params()

        if pile_size < 0:
            return

        # Just keep playing the game until the user kills it with ctrl-C
        play_game(pile_size, min_select, max_select, start_option, win_option)


if __name__ == "__main__":
    main()
