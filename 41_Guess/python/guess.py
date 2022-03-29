########################################################
#
# Guess
#
# From: Basic Computer Games (1978)
#
#  "In program Guess, the computer  chooses a random
#   integer between 0 and any limit and any limit you
#   set. You must then try to guess the number the
#   computer has choosen using the clues provideed by
#   the computer.
#    You should be able to guess the number in one less
#   than the number of digits needed to  represent the
#   number in binary notation - i.e. in base 2. This ought
#   to give you a clue as to the optimum search technique.
#    Guess converted from the original program in FOCAL
#   which appeared in the book "Computers in the Classroom"
#   by Walt Koetke of Lexington High School, Lexington,
#   Massaschusetts.
#
########################################################

# Altough the introduction says that the computer chooses
# a number between 0 and any limit, it actually chooses
# a number between 1 and any limit. This due to the fact that
# for computing the number of digits the limit has in binary
# representation, it has to use log.

from math import log
from random import random


def insert_whitespaces() -> None:
    print("\n\n\n\n\n")


def limit_set():
    print("                   Guess")
    print("Creative Computing  Morristown, New Jersey")
    print("\n\n\n")
    print("This is a number guessing game. I'll think")
    print("of a number between 1 and any limit you want.\n")
    print("Then you have to guess what it is\n")
    print("What limit do you want?")

    limit = int(input())

    while limit <= 0:
        print("Please insert a number greater or equal to 1")
        limit = int(input())

    # limit_goal = Number of digits "limit" in binary has
    limit_goal = int((log(limit) / log(2)) + 1)

    return limit, limit_goal


def main() -> None:
    limit, limit_goal = limit_set()
    while True:
        guess_count = 1
        still_guessing = True
        won = False
        my_guess = int(limit * random() + 1)

        print(f"I'm thinking of a number between 1 and {limit}")
        print("Now you try to guess what it is.")

        while still_guessing:
            n = int(input())

            if n < 0:
                break

            insert_whitespaces()
            if n < my_guess:
                print("Too low. Try a bigger answer")
                guess_count += 1
            elif n > my_guess:
                print("Too high. Try a smaller answer")
                guess_count += 1
            else:
                print(f"That's it! You got it in {guess_count} tries")
                won = True
                still_guessing = False

        if won:
            if guess_count < limit_goal:
                print("Very good.")
            elif guess_count == limit_goal:
                print("Good.")
            else:
                print(f"You should have been able to get it in only {limit_goal}")
            insert_whitespaces()
        else:
            insert_whitespaces()
            limit, limit_goal = limit_set()


if __name__ == "__main__":
    main()
