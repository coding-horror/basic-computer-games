from enum import Enum
from typing import Tuple, Union


class WinOptions(Enum):
    Undefined = 0
    TakeLast = 1
    AvoidLast = 2


class StartOptions(Enum):
    Undefined = 0
    ComputerFirst = 1
    PlayerFirst = 2


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


def get_params() -> Tuple[int, int, int, int, int]:
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
    pile_size = 0
    win_option: Union[WinOptions, int] = WinOptions.Undefined
    min_select = 0
    max_select = 0
    start_option: Union[StartOptions, int] = StartOptions.Undefined

    while pile_size < 1:
        pile_size = int(input("ENTER PILE SIZE "))
    while win_option == WinOptions.Undefined:
        win_option = int(input("ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST: "))
    assert isinstance(win_option, int)
    while min_select < 1 or max_select < 1 or min_select > max_select:
        (min_select, max_select) = (
            int(x) for x in input("ENTER MIN AND MAX ").split(" ")
        )
    while start_option == StartOptions.Undefined:
        start_option = int(input("ENTER START OPTION - 1 COMPUTER FIRST, 2 YOU FIRST "))
    assert isinstance(start_option, int)
    return (pile_size, min_select, max_select, start_option, win_option)


def player_move(
    pile_size, min_select, max_select, start_option, win_option
) -> Tuple[bool, int]:
    """This handles the player's turn - asking the player how many objects
    to take and doing some basic validation around that input.  Then it
    checks for any win conditions.

    Returns a boolean indicating whether the game is over and the new pileSize."""
    playerDone = False
    while not playerDone:
        playerMove = int(input("YOUR MOVE "))
        if playerMove == 0:
            print("I TOLD YOU NOT TO USE ZERO!  COMPUTER WINS BY FORFEIT.")
            return (True, pile_size)
        if playerMove > max_select or playerMove < min_select:
            print("ILLEGAL MOVE, REENTER IT")
            continue
        pile_size = pile_size - playerMove
        playerDone = True
        if pile_size <= 0:
            if win_option == WinOptions.AvoidLast:
                print("TOUGH LUCK, YOU LOSE.")
            else:
                print("CONGRATULATIONS, YOU WIN.")
            return (True, pile_size)
    return (False, pile_size)


def computer_pick(pile_size, min_select, max_select, start_option, win_option) -> int:
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
    pile_size, min_select, max_select, start_option, win_option
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
    currSel = computer_pick(pile_size, min_select, max_select, start_option, win_option)
    pile_size = pile_size - currSel
    print(f"COMPUTER TAKES {currSel} AND LEAVES {pile_size}")
    return (False, pile_size)


def play_game(pile_size, min_select, max_select, start_option, win_option) -> None:
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
                pile_size, min_select, max_select, start_option, win_option
            )
            players_turn = False
            if game_over:
                return
        if not players_turn:
            (game_over, pile_size) = computer_move(
                pile_size, min_select, max_select, start_option, win_option
            )
            players_turn = True


if __name__ == "__main__":

    pileSize = 0
    minSelect = 0
    maxSelect = 0
    # 1 = to take last, 2 = to avoid last
    winOption = 0
    # 1 = computer first, 2 = user first
    startOption = 0

    while True:
        print_intro()
        (pileSize, minSelect, maxSelect, startOption, winOption) = get_params()
        # Just keep playing the game until the user kills it with ctrl-C
        play_game(pileSize, minSelect, maxSelect, startOption, winOption)
