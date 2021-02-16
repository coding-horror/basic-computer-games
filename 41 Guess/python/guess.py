from math import log
from random import random


def insert_whitespaces():
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


limit, limit_goal = limit_set()
while True:
    guess_count = 1
    still_guessing = True
    won = False
    my_guess = int(limit * random() + 1)

    print("I'm thinking of a number between 1 and {}".format(limit))
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
            print("That's it! You got it in {} tries".format(guess_count))
            won = True
            still_guessing = False

    if won:
        if guess_count < limit_goal:
            print("Very good.")
        elif guess_count == limit_goal:
            print("Good.")
        else:
            print("You should have been able to get it in only {}".format(limit_goal))
        insert_whitespaces()
    else:
        insert_whitespaces()
        limit, limit_goal = limit_set()
