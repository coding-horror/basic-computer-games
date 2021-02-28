"""
DIAMOND

Prints pretty diamond patterns to the screen.

Ported by Dave LeCompte
"""


def print_with_tab(space_count, msg):
    if space_count > 0:
        spaces = " " * space_count
    else:
        spaces = ""
    print(spaces + msg)


def print_diamond(begin_width, end_width, step, width, count):
    edgeString = "CC"
    fill = "!"

    n = begin_width
    while True:
        line_buffer = " " * ((width - n) // 2)
        for across in range(count):
            for a in range(n):
                if a >= len(edgeString):
                    line_buffer += fill
                else:
                    line_buffer += edgeString[a]
            line_buffer += " " * (
                (width * (across + 1) + (width - n) // 2) - len(line_buffer)
            )
        print(line_buffer)
        if n == end_width:
            return
        n += step


def main():
    print_with_tab(33, "DIAMOND")
    print_with_tab(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()
    print("FOR A PRETTY DIAMOND PATTERN,")
    print("TYPE IN AN ODD NUMBER BETWEEN 5 AND 21")
    width = int(input())
    print()

    PAGE_WIDTH = 60

    count = int(PAGE_WIDTH / width)

    for down in range(count):
        print_diamond(1, width, 2, width, count)
        print_diamond(width - 2, 1, -2, width, count)

    print()
    print()


if __name__ == "__main__":
    main()
