"""
LIFE

An implementation of John Conway's popular cellular automaton

Ported by Dave LeCompte
"""


PAGE_WIDTH = 64

MAX_WIDTH = 70
MAX_HEIGHT = 24


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title):
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def get_pattern():
    print("ENTER YOUR PATTERN:")
    c = 0

    pattern = {}
    while True:
        line = input()
        if line == "DONE":
            return pattern

        # BASIC input would strip of leading whitespace.
        # Python input does not. The following allows you to start a
        # line with a dot to disable the whitespace stripping. This is
        # unnecessary for Python, but for historical accuracy, it's
        # staying in.

        if line[0] == ".":
            line = " " + line[1:]
        pattern[c] = line
        c += 1


def main():
    print_header("LIFE")

    pattern = get_pattern()

    pattern_height = len(pattern)
    pattern_width = 0
    for line_num, line in pattern.items():
        pattern_width = max(pattern_width, len(line))

    min_x = 11 - pattern_height // 2
    min_y = 33 - pattern_width // 2
    max_x = MAX_HEIGHT - 1
    max_y = MAX_WIDTH - 1

    a = [[0 for y in range(MAX_WIDTH)] for x in range(MAX_HEIGHT)]
    p = 0
    g = 0
    invalid = False

    # line 140
    # transcribe the input pattern into the active array
    for x in range(0, pattern_height):
        for y in range(0, len(pattern[x])):
            if pattern[x][y] != " ":
                a[min_x + x][min_y + y] = 1
                p += 1

    print()
    print()
    print()
    while True:
        if invalid:
            inv_str = "INVALID!"
        else:
            inv_str = ""

        print(f"GENERATION: {g}\tPOPULATION: {p} {inv_str}")

        next_min_x = MAX_HEIGHT - 1
        next_min_y = MAX_WIDTH - 1
        next_max_x = 0
        next_max_y = 0

        p = 0
        g += 1
        for x in range(0, min_x):
            print()

        for x in range(min_x, max_x + 1):
            print
            line = [" "] * MAX_WIDTH
            for y in range(min_y, max_y + 1):
                if a[x][y] == 2:
                    a[x][y] = 0
                    continue
                elif a[x][y] == 3:
                    a[x][y] = 1
                elif a[x][y] != 1:
                    continue

                # line 261
                line[y] = "*"

                next_min_x = min(x, next_min_x)
                next_max_x = max(x, next_max_x)
                next_min_y = min(y, next_min_y)
                next_max_y = max(y, next_max_y)

            print("".join(line))

        # line 295
        for x in range(max_x + 1, MAX_HEIGHT):
            print()

        print()

        min_x = next_min_x
        max_x = next_max_x
        min_y = next_min_y
        max_y = next_max_y

        if min_x < 3:
            min_x = 3
            invalid = True
        if max_x > 22:
            max_x = 22
            invalid = True
        if min_y < 3:
            min_y = 3
            invalid = True
        if max_y > 68:
            max_y = 68
            invalid = True

        # line 309
        p = 0

        for x in range(min_x - 1, max_x + 2):
            for y in range(min_y - 1, max_y + 2):
                count = 0
                for i in range(x - 1, x + 2):
                    for j in range(y - 1, y + 2):
                        if a[i][j] == 1 or a[i][j] == 2:
                            count += 1
                if a[x][y] == 0:
                    if count == 3:
                        a[x][y] = 3
                        p += 1
                elif (count < 3) or (count > 4):
                    a[x][y] = 2
                else:
                    p += 1

        # line 635
        min_x = min_x - 1
        min_y = min_y - 1
        max_x = max_x + 1
        max_y = max_y + 1


if __name__ == "__main__":
    main()
