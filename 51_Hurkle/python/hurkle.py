#!/usr/bin/env python3
#
# Ported to Python by @iamtraction

from random import random


def direction(A, B, X, Y):
    """Prints the direction hint for finding the hurkle."""

    print("GO ", end="")
    if Y < B:
        print("NORTH", end="")
    elif Y > B:
        print("SOUTH", end="")

    if X < A:
        print("EAST", end="")
    elif X > A:
        print("WEST", end="")

    print()


if __name__ == "__main__":
    print(" " * 33 + "HURKLE")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")

    print("\n\n\n")

    N = 5
    G = 10

    print()
    print("A HURKLE IS HIDING ON A", G, "BY", G, "GRID. HOMEBASE")
    print("ON THE GRID IS POINT 0,0 IN THE SOUTHWEST CORNER,")
    print("AND ANY POINT ON THE GRID IS DESIGNATED BY A")
    print("PAIR OF WHOLE NUMBERS SEPERATED BY A COMMA. THE FIRST")
    print("NUMBER IS THE HORIZONTAL POSITION AND THE SECOND NUMBER")
    print("IS THE VERTICAL POSITION. YOU MUST TRY TO")
    print("GUESS THE HURKLE'S GRIDPOINT. YOU GET", N, "TRIES.")
    print("AFTER EACH TRY, I WILL TELL YOU THE APPROXIMATE")
    print("DIRECTION TO GO TO LOOK FOR THE HURKLE.")
    print()

    while True:
        A = int(G * random())
        B = int(G * random())

        for k in range(0, N):
            print("\nGUESS #" + str(k))

            # read coordinates in `X, Y` format, split the string
            # at `,`, and then parse the coordinates to `int` and
            # store them in `X` and `Y` respectively.
            [ X, Y ] = [int(c) for c in input("X,Y? ").split(",")]

            if abs(X - A) + abs(Y - B) == 0:
                print("\nYOU FOUND HIM IN", k + 1, "GUESSES!")
                break
            else:
                direction(A, B, X, Y)
                continue

        print("\n\nLET'S PLAY AGAIN, HURKLE IS HIDING.\n")
