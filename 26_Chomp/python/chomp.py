#!/usr/bin/env python3
# CHOMP
#
# Converted from BASIC to Python by Trevor Hobson


class Canvas:
    """ For drawing the cookie """

    def __init__(self, width=9, height=9, fill="*"):
        self._buffer = []
        for _ in range(height):
            line = []
            for _ in range(width):
                line.append(fill)
            self._buffer.append(line)
        self._buffer[0][0] = "P"

    def render(self):
        lines = ["       1 2 3 4 5 6 7 8 9"]
        row = 0
        for line in self._buffer:
            row += 1
            lines.append(" " + str(row) + " " * 5 + " ".join(line))
        return "\n".join(lines)

    def chomp(self, r, c):
        if not 1 <= r <= len(self._buffer) or not 1 <= c <= len(self._buffer[0]):
            return "Empty"
        elif self._buffer[r - 1][c - 1] == " ":
            return "Empty"
        elif self._buffer[r - 1][c - 1] == "P":
            return "Poison"
        else:
            for row in range(r - 1, len(self._buffer)):
                for column in range(c - 1, len(self._buffer[row])):
                    self._buffer[row][column] = " "
            return "Chomp"


def play_game():
    """Play one round of the game"""
    players = 0
    while players == 0:
        try:
            players = int(input("How many players "))

        except ValueError:
            print("Please enter a number.")
    rows = 0
    while rows == 0:
        try:
            rows = int(input("How many rows "))
            if rows > 9 or rows < 1:
                rows = 0
                print("Too many rows (9 is maximum).")

        except ValueError:
            print("Please enter a number.")
    columns = 0
    while columns == 0:
        try:
            columns = int(input("How many columns "))
            if columns > 9 or columns < 1:
                columns = 0
                print("Too many columns (9 is maximum).")

        except ValueError:
            print("Please enter a number.")
    cookie = Canvas(width=columns, height=rows)
    player = 0
    alive = True
    while alive:
        print("")
        print(cookie.render())
        print("")
        player += 1
        if player > players:
            player = 1
        while True:
            print("Player", player)
            player_row = -1
            player_column = -1
            while player_row == -1 or player_column == -1:
                try:
                    coordinates = [int(item) for item in input(
                        "Coordinates of chomp (Row, Column) ").split(",")]
                    player_row = coordinates[0]
                    player_column = coordinates[1]

                except (ValueError, IndexError):
                    print("Please enter valid coordinates.")
            result = cookie.chomp(player_row, player_column)
            if result == "Empty":
                print("No fair. You're trying to chomp on empty space!")
            elif result == "Poison":
                print("\nYou lose player", player)
                alive = False
                break
            else:
                break


def main():
    print(" " * 33 + "CHOMP")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
    print("THIS IS THE GAME OF CHOMP (SCIENTIFIC AMERICAN, JAN 1973)")
    if input("Do you want the rules (1=Yes, 0=No!) ") != "0":
        print("Chomp is for 1 or more players (Humans only).\n")
        print("Here's how a board looks (This one is 5 by 7):")
        example = Canvas(width=7, height=5)
        print(example.render())
        print("\nThe board is a big cookie - R rows high and C columns")
        print("wide. You input R and C at the start. In the upper left")
        print("corner of the cookie is a poison square (P). The one who")
        print("chomps the poison square loses. To take a chomp, type the")
        print("row and column of one of the squares on the cookie.")
        print("All of the squares below and to the right of that square")
        print("(Including that square, too) disappear -- CHOMP!!")
        print("No fair chomping squares that have already been chomped,")
        print("or that are outside the original dimensions of the cookie.\n")
        print("Here we go...")

    keep_playing = True
    while keep_playing:

        play_game()
        keep_playing = input("\nAgain (1=Yes, 0=No!) ") == "1"


if __name__ == "__main__":
    main()
