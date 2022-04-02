"""
NUMBER

A number guessing (gambling) game.

Ported by Dave LeCompte
"""

import random


def print_instructions() -> None:
    print("YOU HAVE 100 POINTS.  BY GUESSING NUMBERS FROM 1 TO 5, YOU")
    print("CAN GAIN OR LOSE POINTS DEPENDING UPON HOW CLOSE YOU GET TO")
    print("A RANDOM NUMBER SELECTED BY THE COMPUTER.")
    print()
    print("YOU OCCASIONALLY WILL GET A JACKPOT WHICH WILL DOUBLE(!)")
    print("YOUR POINT COUNT.  YOU WIN WHEN YOU GET 500 POINTS.")
    print()


def fnr() -> int:
    return random.randint(1, 5)


def main() -> None:
    print(" " * 33 + "NUMBER")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")

    print_instructions()

    points: float = 100

    while points <= 500:
        print("GUESS A NUMBER FROM 1 TO 5")
        guess = int(input())

        if (guess < 1) or (guess > 5):
            continue

        r = fnr()
        s = fnr()
        t = fnr()
        u = fnr()
        v = fnr()

        if guess == r:
            # lose 5
            points -= 5
        elif guess == s:
            # gain 5
            points += 5
        elif guess == t:
            # double!
            points += points
            print("YOU HIT THE JACKPOT!!!")
        elif guess == u:
            # gain 1
            points += 1
        elif guess == v:
            # lose half
            points = points - (points * 0.5)

        print(f"YOU HAVE {points} POINTS.")
        print()
    print(f"!!!!YOU WIN!!!! WITH {points} POINTS.")


if __name__ == "__main__":
    main()
