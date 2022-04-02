#!/usr/bin/env python3


# This data is meant to be read-only, so we are storing it in a tuple
import json

with open("data.json") as f:
    DATA = tuple(json.load(f))


def print_intro() -> None:
    print(" " * 33 + "BUNNY")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print("\n\n")


def main() -> None:
    print_intro()

    # Using an iterator will give us a similar interface to BASIC's READ
    # command. Instead of READ, we will call 'next(data)' to fetch the next element.
    data = iter(DATA)

    # Read the first 5 numbers. These correspond to letters of the alphabet.
    # B=2, U=21, N=14, N=14, Y=25

    # Usually, list comprehensions are good for transforming each element in a sequence.
    # In this case, we are using range to repeat the call to next(data) 5 times. The underscore (_)
    # indicates that the values from range are discarded.
    bunny = [next(data) for _ in range(5)]
    L = 64

    # Interpretting a stream of data is a very common software task. We've already intepretted
    # the first 5 numbers as letters of the alphabet (with A being 1). Now, we are going to
    # combine this with a different interpretation of the following data to draw on the screen.
    # The drawing data is essentially a series of horizontal line segments given as begin and end
    # offsets.
    while True:
        command = next(data)

        if command < 0:
            print()
            continue

        if command > 128:
            break

        # If we've reached this portion of the code, 'command' indicates the 'start'
        # position of a line segment.
        start = command
        # Position cursor at start
        print(" " * start, end="")

        # The following number, indicates the end of the segment.
        end = next(data)
        # Unlike FOR I=X TO Y, the 'stop' argument of 'range' is non-inclusive, so we must add 1
        for i in range(start, end + 1, 1):
            # Cycle through the letters in "BUNNY" as we draw line
            j = i - 5 * int(i / 5)
            print(chr(L + bunny[j]), end="")


if __name__ == "__main__":
    main()
