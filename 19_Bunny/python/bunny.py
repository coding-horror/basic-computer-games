#!/usr/bin/env python3


# This data is meant to be read-only, so we are storing it in a tuple
DATA = (2,21,14,14,25,
        1,2,-1,0,2,45,50,-1,0,5,43,52,-1,0,7,41,52,-1,
        1,9,37,50,-1,2,11,36,50,-1,3,13,34,49,-1,4,14,32,48,-1,
        5,15,31,47,-1,6,16,30,45,-1,7,17,29,44,-1,8,19,28,43,-1,
        9,20,27,41,-1,10,21,26,40,-1,11,22,25,38,-1,12,22,24,36,-1,
        13,34,-1,14,33,-1,15,31,-1,17,29,-1,18,27,-1,
        19,26,-1,16,28,-1,13,30,-1,11,31,-1,10,32,-1,
        8,33,-1,7,34,-1,6,13,16,34,-1,5,12,16,35,-1,
        4,12,16,35,-1,3,12,15,35,-1,2,35,-1,1,35,-1,
        2,34,-1,3,34,-1,4,33,-1,6,33,-1,10,32,34,34,-1,
        14,17,19,25,28,31,35,35,-1,15,19,23,30,36,36,-1,
        14,18,21,21,24,30,37,37,-1,13,18,23,29,33,38,-1,
        12,29,31,33,-1,11,13,17,17,19,19,22,22,24,31,-1,
        10,11,17,18,22,22,24,24,29,29,-1,
        22,23,26,29,-1,27,29,-1,28,29,-1,4096)


def display_intro():
    print(tab(33) + "BUNNY")
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print("\n\n")


def tab(column):
    """ Emulates the TAB command in BASIC. Returns a string with ASCII
        codes for setting the cursor to the specified column. """
    return "\r\33[{}C".format(column)


def play():
    display_intro()

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
        print(tab(start), end="")

        # The following number, indicates the end of the segment.
        end = next(data)
        # Unlike FOR I=X TO Y, the 'stop' argument of 'range' is non-inclusive, so we must add 1
        for i in range(start, end+1, 1):
            # Cycle through the letters in "BUNNY" as we draw line
            j = i - 5 * int(i / 5)
            print(chr(L + bunny[j]), end="")


if __name__ == "__main__":
    play()
