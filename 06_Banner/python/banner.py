#!/usr/bin/env python3

# BANNER
#
# Converted from BASIC to Python by Trevor Hobson

letters = {
    " ": [0, 0, 0, 0, 0, 0, 0],
    "A": [505, 37, 35, 34, 35, 37, 505],
    "G": [125, 131, 258, 258, 290, 163, 101],
    "E": [512, 274, 274, 274, 274, 258, 258],
    "T": [2, 2, 2, 512, 2, 2, 2],
    "W": [256, 257, 129, 65, 129, 257, 256],
    "L": [512, 257, 257, 257, 257, 257, 257],
    "S": [69, 139, 274, 274, 274, 163, 69],
    "O": [125, 131, 258, 258, 258, 131, 125],
    "N": [512, 7, 9, 17, 33, 193, 512],
    "F": [512, 18, 18, 18, 18, 2, 2],
    "K": [512, 17, 17, 41, 69, 131, 258],
    "B": [512, 274, 274, 274, 274, 274, 239],
    "D": [512, 258, 258, 258, 258, 131, 125],
    "H": [512, 17, 17, 17, 17, 17, 512],
    "M": [512, 7, 13, 25, 13, 7, 512],
    "?": [5, 3, 2, 354, 18, 11, 5],
    "U": [128, 129, 257, 257, 257, 129, 128],
    "R": [512, 18, 18, 50, 82, 146, 271],
    "P": [512, 18, 18, 18, 18, 18, 15],
    "Q": [125, 131, 258, 258, 322, 131, 381],
    "Y": [8, 9, 17, 481, 17, 9, 8],
    "V": [64, 65, 129, 257, 129, 65, 64],
    "X": [388, 69, 41, 17, 41, 69, 388],
    "Z": [386, 322, 290, 274, 266, 262, 260],
    "I": [258, 258, 258, 512, 258, 258, 258],
    "C": [125, 131, 258, 258, 258, 131, 69],
    "J": [65, 129, 257, 257, 257, 129, 128],
    "1": [0, 0, 261, 259, 512, 257, 257],
    "2": [261, 387, 322, 290, 274, 267, 261],
    "*": [69, 41, 17, 512, 17, 41, 69],
    "3": [66, 130, 258, 274, 266, 150, 100],
    "4": [33, 49, 41, 37, 35, 512, 33],
    "5": [160, 274, 274, 274, 274, 274, 226],
    "6": [194, 291, 293, 297, 305, 289, 193],
    "7": [258, 130, 66, 34, 18, 10, 8],
    "8": [69, 171, 274, 274, 274, 171, 69],
    "9": [263, 138, 74, 42, 26, 10, 7],
    "=": [41, 41, 41, 41, 41, 41, 41],
    "!": [1, 1, 1, 384, 1, 1, 1],
    "0": [57, 69, 131, 258, 131, 69, 57],
    ".": [1, 1, 129, 449, 129, 1, 1],
}


def print_banner():
    """Print the banner"""

    f = [0] * 7
    j = [0] * 9

    while True:
        try:
            x = int(input("Horizontal "))
            if x < 1:
                raise ValueError("Horizontal must be greater than zero")
            break

        except ValueError:
            print("Please enter a number greater than zero")
    while True:
        try:
            y = int(input("Vertical "))
            if y < 1:
                raise ValueError("Vertical must be greater than zero")
            break

        except ValueError:
            print("Please enter a number greater than zero")
    g1 = 0
    if input("Centered ").lower().startswith("y"):
        g1 = 1
    mStr = input("Character (type 'ALL' if you want character being printed) ").upper()
    aStr = input("Statement ")
    # This means to prepare printer, just press Enter
    oStr = input("Set page ")
    for lStr in aStr:
        s = letters[lStr].copy()
        xStr = mStr
        if mStr == "ALL":
            xStr = lStr
        if xStr == " ":
            print("\n" * (7 * x))
        else:
            for u in range(0, 7):
                for k in range(8, -1, -1):
                    if 2**k >= s[u]:
                        j[8 - k] = 0
                    else:
                        j[8 - k] = 1
                        s[u] = s[u] - 2**k
                        if s[u] == 1:
                            f[u] = 8 - k
                            break
                for t1 in range(1, x + 1):
                    lineStr = " " * int((63 - 4.5 * y) * g1 / len(xStr) + 1)
                    for b in range(0, f[u] + 1):
                        if j[b] == 0:
                            for i in range(1, y + 1):
                                lineStr = lineStr + " " * len(xStr)
                        else:
                            lineStr = lineStr + xStr * y
                    print(lineStr)
            print("\n" * (2 * x - 1))
    # print("\n" * 75)  # Feed some more paper from the printer


def main():
    """Main"""

    print_banner()


if __name__ == "__main__":
    main()
