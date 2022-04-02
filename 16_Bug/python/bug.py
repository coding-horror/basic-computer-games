import random
import time
from dataclasses import dataclass
from typing import Literal


@dataclass
class State:
    is_player: bool
    body: int = 0
    neck: int = 0
    head: int = 0
    feelers: int = 0
    tail: int = 0
    legs: int = 0

    def is_finished(self) -> bool:
        return (
            self.feelers == 2
            and self.tail == 1
            and self.legs == 6
            and self.head == 1
            and self.neck == 1
        )

    def display(self) -> None:
        if self.feelers != 0:
            print_feelers(self.feelers, is_player=self.is_player)
        if self.head != 0:
            print_head()
        if self.neck != 0:
            print_neck()
        if self.body != 0:
            print_body(True) if self.tail == 1 else print_body(False)
        if self.legs != 0:
            print_legs(self.legs)


def print_n_newlines(n: int) -> None:
    for _ in range(n):
        print()


def print_feelers(n_feelers: int, is_player: bool = True) -> None:
    for _ in range(4):
        print(" " * 10, end="")
        for _ in range(n_feelers):
            print("A " if is_player else "F ", end="")
        print()


def print_head() -> None:
    print("        HHHHHHH")
    print("        H     H")
    print("        H O O H")
    print("        H     H")
    print("        H  V  H")
    print("        HHHHHHH")


def print_neck() -> None:
    print("          N N")
    print("          N N")


def print_body(has_tail: bool = False) -> None:
    print("     BBBBBBBBBBBB")
    print("     B          B")
    print("     B          B")
    print("TTTTTB          B") if has_tail else ""
    print("     BBBBBBBBBBBB")


def print_legs(n_legs: int) -> None:
    for _ in range(2):
        print(" " * 5, end="")
        for _ in range(n_legs):
            print(" L", end="")
        print()


def handle_roll(diceroll: Literal[1, 2, 3, 4, 5, 6], state: State) -> bool:
    who = "YOU" if state.is_player else "I"
    changed = False

    print(f"{who} ROLLED A", diceroll)
    if diceroll == 1:
        print("1=BODY")
        if state.body:
            print(f"{who} DO NOT NEED A BODY.")
        else:
            print(f"{who} NOW HAVE A BODY.")
            state.body = 1
            changed = True
    elif diceroll == 2:
        print("2=NECK")
        if state.neck:
            print(f"{who} DO NOT NEED A NECK.")
        elif state.body == 0:
            print(f"{who} DO NOT HAVE A BODY.")
        else:
            print(f"{who} NOW HAVE A NECK.")
            state.neck = 1
            changed = True
    elif diceroll == 3:
        print("3=HEAD")
        if state.neck == 0:
            print(f"{who} DO NOT HAVE A NECK.")
        elif state.head:
            print(f"{who} HAVE A HEAD.")
        else:
            print(f"{who} NEEDED A HEAD.")
            state.head = 1
            changed = True
    elif diceroll == 4:
        print("4=FEELERS")
        if state.head == 0:
            print(f"{who} DO NOT HAVE A HEAD.")
        elif state.feelers == 2:
            print(f"{who} HAVE TWO FEELERS ALREADY.")
        else:
            if state.is_player:
                print("I NOW GIVE YOU A FEELER.")
            else:
                print(f"{who} GET A FEELER.")
            state.feelers += 1
            changed = True
    elif diceroll == 5:
        print("5=TAIL")
        if state.body == 0:
            print(f"{who} DO NOT HAVE A BODY.")
        elif state.tail:
            print(f"{who} ALREADY HAVE A TAIL.")
        else:
            if state.is_player:
                print("I NOW GIVE YOU A TAIL.")
            else:
                print(f"{who} NOW HAVE A TAIL.")
            state.tail = 1
            changed = True
    elif diceroll == 6:
        print("6=LEG")
        if state.legs == 6:
            print(f"{who} HAVE 6 FEET ALREADY.")
        elif state.body == 0:
            print(f"{who} DO NOT HAVE A BODY.")
        else:
            state.legs += 1
            changed = True
            print(f"{who} NOW HAVE {state.legs} LEGS")
    return changed


def main() -> None:
    print(" " * 34 + "BUG")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print_n_newlines(3)

    print("THE GAME BUG")
    print("I HOPE YOU ENJOY THIS GAME.")
    print()
    want_instructions = input("DO YOU WANT INSTRUCTIONS? ")
    if want_instructions != "NO":
        print("THE OBJECT OF BUG IS TO FINISH YOUR BUG BEFORE I FINISH")
        print("MINE. EACH NUMBER STANDS FOR A PART OF THE BUG BODY.")
        print("I WILL ROLL THE DIE FOR YOU, TELL YOU WHAT I ROLLED FOR YOU")
        print("WHAT THE NUMBER STANDS FOR, AND IF YOU CAN GET THE PART.")
        print("IF YOU CAN GET THE PART I WILL GIVE IT TO YOU.")
        print("THE SAME WILL HAPPEN ON MY TURN.")
        print("IF THERE IS A CHANGE IN EITHER BUG I WILL GIVE YOU THE")
        print("OPTION OF SEEING THE PICTURES OF THE BUGS.")
        print("THE NUMBERS STAND FOR PARTS AS FOLLOWS:")
        table = [
            ["NUMBER", "PART", "NUMBER OF PART NEEDED"],
            ["1", "BODY", "1"],
            ["2", "NECK", "1"],
            ["3", "HEAD", "1"],
            ["4", "FEELERS", "2"],
            ["5", "TAIL", "1"],
            ["6", "LEGS", "6"],
        ]
        for row in table:
            print(f"{row[0]:<16}{row[1]:<16}{row[2]:<20}")
        print_n_newlines(2)

    player = State(is_player=True)
    opponent = State(is_player=False)
    bugs_finished = 0

    while bugs_finished <= 0:
        diceroll = random.randint(1, 6)
        print()
        changed = handle_roll(diceroll, player)  # type: ignore

        diceroll = random.randint(1, 6)
        print()
        time.sleep(2)

        changed_op = handle_roll(diceroll, opponent)  # type: ignore

        changed = changed or changed_op

        if player.is_finished():
            print("YOUR BUG IS FINISHED.")
            bugs_finished += 1
        if opponent.is_finished():
            print("MY BUG IS FINISHED.")
            bugs_finished += 1
        if not changed:
            continue
        want_pictures = input("DO YOU WANT THE PICTURES? ")
        if want_pictures != "NO":
            print("*****YOUR BUG*****")
            print_n_newlines(2)
            player.display()
            print_n_newlines(4)
            print("*****MY BUG*****")
            print_n_newlines(3)
            opponent.display()

            if bugs_finished != 0:
                break

    print("I HOPE YOU ENJOYED THE GAME, PLAY IT AGAIN SOON!!")


if __name__ == "__main__":
    main()
