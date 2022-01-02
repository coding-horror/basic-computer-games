# Flip Flop
#
# The object of this game is to change a row of ten X's
# X X X X X X X X X X
# to a row of ten O's:
# O O O O O O O O O O
# by typing in a number corresponding
# to the position of an "X" in the line. On
# some numbers one position will
# change while on other numbers, two
# will change. For example, inputting a 3
# may reverse the X and O in position 3,
# but it might possibly reverse some
# other position too! You ought to be able
# to change all 10 in 12 or fewer
# moves. Can you figure out a good win-
# ning strategy?
# To reset the line to all X's (same
# game), type 0 (zero). To start a new
# game at any point, type 11.
# The original author of this game was
# Michael Kass of New Hyde Park, New
# York.
import random
import math
from typing import Callable, List, Tuple

flip_dict = {"X": "O", "O": "X"}


def flip_bits(
    row: List[str], m: int, n: int, r_function: Callable[[int], float]
) -> Tuple[List[str], int]:
    """
    Function that flips the positions at the computed steps
    """
    while m == n:
        r = r_function(n)
        n = r - int(math.floor(r))
        n = int(10 * n)
        if row[n] == "X":
            row[n] = "O"
            break
        elif row[n] == "O":
            row[n] = "X"
    return row, n


def print_instructions():
    print(" " * 32 + "FLIPFLOP")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print("\n" * 2)
    print("THE OBJECT OF THIS PUZZLE IS TO CHANGE THIS:\n")
    print("X X X X X X X X X X\n")
    print("TO THIS:\n")
    print("O O O O O O O O O O\n")
    print("BY TYPING TH NUMBER CORRESPONDING TO THE POSITION OF THE")
    print("LETTER ON SOME NUMBERS, ONE POSITION WILL CHANGE, ON")
    print("OTHERS, TWO WILL CHANGE. TO RESET LINE TO ALL X'S, TYPE 0")
    print("(ZERO) AND TO START OVER IN THE MIDDLE OF A GAME, TYPE ")
    print("11 (ELEVEN).\n")


def main():
    q = random.random()

    print("HERE IS THE STARTING LINE OF X'S.\n")
    # We add an extra 0-th item because this sometimes is set to something
    # but we never check what it is for completion of the puzzle
    row = [""] + ["X"] * 10
    counter_turns = 0
    n = -1
    legal_move = True
    while row[1:] != ["O"] * 10:
        if legal_move:
            print(" ".join([str(i) for i in range(1, 11)]))
            print(" ".join(row[1:]) + "\n")
        m = input("INPUT THE NUMBER\n")
        try:
            m = int(m)
            if m > 11 or m < 0:
                raise ValueError()
        except ValueError:
            print("ILLEGAL ENTRY--TRY AGAIN")
            legal_move = False
            continue
        legal_move = True
        if m == 11:
            # completely reset the puzzle
            counter_turns = 0
            row = [""] + ["X"] * 10
            q = random.random()
            continue
        elif m == 0:
            # reset the board, but not the counter or the random number
            row = [""] + ["X"] * 10
        elif m == n:
            row[n] = flip_dict[row[n]]
            r_function = lambda n_t: 0.592 * (1 / math.tan(q / n_t + q)) / math.sin(
                n_t * 2 + q
            ) - math.cos(n_t)
            row, n = flip_bits(row, m, n, r_function)
        else:
            n = m
            row[n] = flip_dict[row[n]]
            r_function = lambda n_t: (
                math.tan(q + n_t / q - n_t)
                - math.sin(n_t * 2 + q)
                + 336 * math.sin(8 * n_t)
            )
            row, n = flip_bits(row, m, n, r_function)

        counter_turns += 1
        print()

    if counter_turns <= 12:
        print(f"VERY GOOD. YOU GUESSED IT IN ONLY {counter_turns} GUESSES.")
    else:
        print(f"TRY HARDER NEXT TIME. IT TOOK YOU {counter_turns} GUESSES.")
    return


if __name__ == "__main__":
    print_instructions()

    another = ""
    while another != "NO":
        main()
        another = input("DO YOU WANT TO TRY ANOTHER PUZZLE\n")
