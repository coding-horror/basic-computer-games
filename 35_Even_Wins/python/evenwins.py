"""
This version of evenwins.bas based on game decscription and does *not*
follow the source. The computer chooses marbles at random.

For simplicity, global variables are used to store the game state.
A good exercise would be to replace this with a class.
The code is not short, but hopefully it is easy for beginners to understand
and modify.

Infinite loops of the style "while True:" are used to simplify some of the
code. The "continue" keyword is used in a few places to jump back to the top
of the loop. The "return" keyword is also used to break out of functions.
This is generally considered poor style, but in this case it simplifies the
code and makes it easier to read (at least in my opinion). A good exercise
would be to remove these infinite loops, and uses of continue, to follow a
more structured style.
"""


from dataclasses import dataclass
from typing import Literal, Tuple

PlayerType = Literal["human", "computer"]


@dataclass
class MarbleCounts:
    middle: int
    human: int
    computer: int


def print_intro() -> None:
    print("Welcome to Even Wins!")
    print("Based on evenwins.bas from Creative Computing")
    print()
    print("Even Wins is a two-person game. You start with")
    print("27 marbles in the middle of the table.")
    print()
    print("Players alternate taking marbles from the middle.")
    print("A player can take 1 to 4 marbles on their turn, and")
    print("turns cannot be skipped. The game ends when there are")
    print("no marbles left, and the winner is the one with an even")
    print("number of marbles.")
    print()


def marbles_str(n: int) -> str:
    if n == 1:
        return "1 marble"
    return f"{n} marbles"


def choose_first_player() -> PlayerType:
    while True:
        ans = input("Do you want to play first? (y/n) --> ")
        if ans == "y":
            return "human"
        elif ans == "n":
            return "computer"
        else:
            print()
            print('Please enter "y" if you want to play first,')
            print('or "n" if you want to play second.')
            print()


def toggle_player(whose_turn: PlayerType) -> PlayerType:
    if whose_turn == "human":
        return "computer"
    else:
        return "human"


def to_int(s: str) -> Tuple[bool, int]:
    """Convert a string s to an int, if possible."""
    try:
        n = int(s)
        return True, n
    except Exception:
        return False, 0


def print_board(marbles: MarbleCounts) -> None:
    print()
    print(f" marbles in the middle: {marbles.middle} " + marbles.middle * "*")
    print(f"    # marbles you have: {marbles.human}")
    print(f"# marbles computer has: {marbles.computer}")
    print()


def human_turn(marbles: MarbleCounts) -> None:
    """get number in range 1 to min(4, marbles.middle)"""
    max_choice = min(4, marbles.middle)
    print("It's your turn!")
    while True:
        s = input(f"Marbles to take? (1 - {max_choice}) --> ")
        ok, n = to_int(s)
        if not ok:
            print(f"\n  Please enter a whole number from 1 to {max_choice}\n")
            continue
        if n < 1:
            print("\n  You must take at least 1 marble!\n")
            continue
        if n > max_choice:
            print(f"\n  You can take at most {marbles_str(max_choice)}\n")
            continue
        print(f"\nOkay, taking {marbles_str(n)} ...")
        marbles.middle -= n
        marbles.human += n
        return


def game_over(marbles: MarbleCounts) -> None:
    print()
    print("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
    print("!! All the marbles are taken: Game Over!")
    print("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
    print()
    print_board(marbles)
    if marbles.human % 2 == 0:
        print("You are the winner! Congratulations!")
    else:
        print("The computer wins: all hail mighty silicon!")
    print()


def computer_turn(marbles: MarbleCounts) -> None:
    marbles_to_take = 0

    print("It's the computer's turn ...")
    r = marbles.middle - 6 * int(marbles.middle / 6)

    if int(marbles.human / 2) == marbles.human / 2:
        if r < 1.5 or r > 5.3:
            marbles_to_take = 1
        else:
            marbles_to_take = r - 1

    elif marbles.middle < 4.2:
        marbles_to_take = marbles.middle
    elif r > 3.4:
        if r < 4.7 or r > 3.5:
            marbles_to_take = 4
    else:
        marbles_to_take = r + 1

    print(f"Computer takes {marbles_str(marbles_to_take)} ...")
    marbles.middle -= marbles_to_take
    marbles.computer += marbles_to_take


def play_game(whose_turn: PlayerType) -> None:
    marbles = MarbleCounts(middle=27, human=0, computer=0)
    print_board(marbles)

    while True:
        if marbles.middle == 0:
            game_over(marbles)
            return
        elif whose_turn == "human":
            human_turn(marbles)
            print_board(marbles)
            whose_turn = toggle_player(whose_turn)
        elif whose_turn == "computer":
            computer_turn(marbles)
            print_board(marbles)
            whose_turn = toggle_player(whose_turn)
        else:
            raise Exception(f"whose_turn={whose_turn} is not 'human' or 'computer'")


def main() -> None:
    print_intro()

    while True:
        whose_turn = choose_first_player()
        play_game(whose_turn)

        print()
        again = input("Would you like to play again? (y/n) --> ").lower()
        if again == "y":
            print("\nOk, let's play again ...\n")
        else:
            print("\nOk, thanks for playing ... goodbye!\n")
            return


if __name__ == "__main__":
    main()
