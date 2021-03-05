"""
CHECKERS

How about a nice game of checkers?

Ported by Dave LeCompte
"""

import collections

PAGE_WIDTH = 64

HUMAN_PLAYER = 1
COMPUTER_PLAYER = -1
HUMAN_PIECE = 1
HUMAN_KING = 2
COMPUTER_PIECE = -1
COMPUTER_KING = -2
EMPTY_SPACE = 0

TOP_ROW = 7
BOTTOM_ROW = 0

MoveRecord = collections.namedtuple(
    "MoveRecord", ["quality", "start_x", "start_y", "dest_x", "dest_y"]
)


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title):
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def get_coordinates(prompt):
    err_msg = "ENTER COORDINATES in X,Y FORMAT"
    while True:
        print(prompt)
        response = input()
        if "," not in response:
            print(err_msg)
            continue

        try:
            x, y = [int(c) for c in response.split(",")]
        except ValueError as ve:
            print(err_msg)
            continue

        return x, y


def is_legal_board_coordinate(x, y):
    return (0 <= x <= 7) and (0 <= y <= 7)


class Board:
    def __init__(self):
        self.spaces = [[0 for y in range(8)] for x in range(8)]
        for x in range(8):
            if (x % 2) == 0:
                self.spaces[x][6] = COMPUTER_PIECE
                self.spaces[x][2] = HUMAN_PIECE
                self.spaces[x][0] = HUMAN_PIECE
            else:
                self.spaces[x][7] = COMPUTER_PIECE
                self.spaces[x][5] = COMPUTER_PIECE
                self.spaces[x][1] = HUMAN_PIECE

    def __str__(self):
        pieces = {
            EMPTY_SPACE: ".",
            HUMAN_PIECE: "O",
            HUMAN_KING: "O*",
            COMPUTER_PIECE: "X",
            COMPUTER_KING: "X*",
        }

        s = "\n\n\n"
        for y in range(7, -1, -1):
            for x in range(0, 8):
                piece_str = pieces[self.spaces[x][y]]
                piece_str += " " * (5 - len(piece_str))
                s += piece_str
            s += "\n"
        s += "\n\n"

        return s

    def get_spaces(self):
        for x in range(0, 8):
            for y in range(0, 8):
                yield x, y

    def get_spaces_with_computer_pieces(self):
        for x, y in self.get_spaces():
            contents = self.spaces[x][y]
            if contents < 0:
                yield x, y

    def get_spaces_with_human_pieces(self):
        for x, y in self.get_spaces():
            contents = self.spaces[x][y]
            if contents > 0:
                yield x, y

    def get_legal_deltas_for_space(self, x, y):
        contents = self.spaces[x][y]
        if contents == COMPUTER_PIECE:
            for delta_x in (-1, 1):
                yield (delta_x, -1)
        else:
            for delta_x in (-1, 1):
                for delta_y in (-1, 1):
                    yield (delta_x, delta_y)

    def pick_computer_move(self):
        move_record = None

        for start_x, start_y in self.get_spaces_with_computer_pieces():
            for delta_x, delta_y in self.get_legal_deltas_for_space(start_x, start_y):
                new_move_record = self.check_move(start_x, start_y, delta_x, delta_y)

                if new_move_record is None:
                    continue

                if (move_record is None) or (
                    new_move_record.quality > move_record.quality
                ):
                    move_record = new_move_record

        return move_record

    def check_move(self, start_x, start_y, delta_x, delta_y):
        new_x = start_x + delta_x
        new_y = start_y + delta_y
        if not is_legal_board_coordinate(new_x, new_y):
            return None

        contents = self.spaces[new_x][new_y]
        if contents == EMPTY_SPACE:
            return self.evaluate_move(start_x, start_y, new_x, new_y)
        if contents < 0:
            return None

        # check jump landing space, which is an additional dx, dy from new_x, newy
        landing_x = new_x + delta_x
        landing_y = new_y + delta_y

        if not is_legal_board_coordinate(landing_x, landing_y):
            return None
        if self.spaces[landing_x][landing_y] == EMPTY_SPACE:
            return self.evaluate_move(start_x, start_y, landing_x, landing_y)

    def evaluate_move(self, start_x, start_y, dest_x, dest_y):
        quality = 0
        if dest_y == 0 and self.spaces[start_x][start_y] == COMPUTER_PIECE:
            # promoting is good
            quality += 2
        if abs(dest_y - start_y) == 2:
            # jumps are good
            quality += 5
        if start_y == 7:
            # prefer to defend back row
            quality -= 2
        if dest_x in (0, 7):
            # moving to edge column
            quality += 1
        for delta_x in (-1, 1):
            if not is_legal_board_coordinate(dest_x + delta_x, dest_y - 1):
                continue

            if self.spaces[dest_x + delta_x][dest_y - 1] < 0:
                # moving into "shadow" of another computer piece
                quality += 1

            if not is_legal_board_coordinate(dest_x - delta_x, dest_y + 1):
                continue

            if (
                (self.spaces[dest_x + delta_x][dest_y - 1] > 0)
                and (self.spaces[dest_x - delta_x][dest_y + 1] == EMPTY_SPACE)
                or ((dest_x - delta_x == start_x) and (dest_y + 1 == start_y))
            ):
                # we are moving up to a human checker that could jump us
                quality -= 2
        return MoveRecord(quality, start_x, start_y, dest_x, dest_y)

    def remove_r_pieces(self, move_record):
        self.remove_pieces(
            move_record.start_x,
            move_record.start_y,
            move_record.dest_x,
            move_record.dest_y,
        )

    def remove_pieces(self, start_x, start_y, dest_x, dest_y):
        self.spaces[dest_x][dest_y] = self.spaces[start_x][start_y]
        self.spaces[start_x][start_y] = EMPTY_SPACE

        if abs(dest_x - start_x) == 2:
            mid_x = (start_x + dest_x) // 2
            mid_y = (start_y + dest_y) // 2
            self.spaces[mid_x][mid_y] = EMPTY_SPACE

    def play_computer_move(self, move_record):
        print(
            f"FROM {move_record.start_x} {move_record.start_y} TO {move_record.dest_x} {move_record.dest_y}"
        )

        while True:
            if move_record.dest_y == BOTTOM_ROW:
                # KING ME
                self.remove_r_pieces(move_record)
                self.spaces[move_record.dest_x][move_record.dest_y] = COMPUTER_KING
                return
            else:
                self.spaces[move_record.dest_x][move_record.dest_y] = self.spaces[
                    move_record.start_x
                ][move_record.start_y]
                self.remove_r_pieces(move_record)

                if abs(move_record.dest_x - move_record.start_x) != 2:
                    return

                landing_x = move_record.dest_x
                landing_y = move_record.dest_y

                best_move = None
                if self.spaces[landing_x][landing_y] == COMPUTER_PIECE:
                    for delta_x in (-2, 2):
                        test_record = self.try_extend(landing_x, landing_y, delta_x, -2)
                        if not (move_record is None):
                            if (best_move is None) or (
                                move_record.quality > best_move.quality
                            ):
                                best_move = test_record
                else:
                    assert self.spaces[landing_x][landing_y] == COMPUTER_KING
                    for delta_x in (-2, 2):
                        for delta_y in (-2, 2):
                            test_record = self.try_extend(
                                landing_x, landing_y, delta_x, delta_y
                            )
                            if not (move_record is None):
                                if (best_move is None) or (
                                    move_record.quality > best_move.quality
                                ):
                                    best_move = test_record

                if best_move is None:
                    return
                else:
                    print(f"TO {best_move.dest_x} {best_move.dest_y}")
                    move_record = best_move

    def try_extend(self, start_x, start_y, delta_x, delta_y):
        new_x = start_x + delta_x
        new_y = start_y + delta_y

        if not is_legal_board_coordinate(new_x, new_y):
            return None

        jumped_x = start_x + delta_x // 2
        jumped_y = start_y + delta_y // 2

        if (self.spaces[new_x][new_y] == EMPTY_SPACE) and (
            self.spaces[jumped_x][jumped_y] > 0
        ):
            return self.evaluate_move(start_x, start_y, new_x, new_y)

    def get_human_move(self):
        is_king = False

        while True:
            start_x, start_y = get_coordinates("FROM?")

            if self.spaces[start_x][start_y] > 0:
                break

        is_king = self.spaces[start_x][start_y] == HUMAN_KING

        while True:
            dest_x, dest_y = get_coordinates("TO?")

            if (not is_king) and (dest_y < start_y):
                # CHEATER! Trying to move non-king backwards
                continue
            if (
                (self.spaces[dest_x][dest_y] == 0)
                and (abs(dest_x - start_x) <= 2)
                and (abs(dest_x - start_x) == abs(dest_y - start_y))
            ):
                break
        return start_x, start_y, dest_x, dest_y

    def get_human_extension(self, start_x, start_y):
        is_king = self.spaces[start_x][start_y] == HUMAN_KING

        while True:
            dest_x, dest_y = get_coordinates("+TO?")

            if dest_x < 0:
                return False, None
            if (not is_king) and (dest_y < start_y):
                # CHEATER! Trying to move non-king backwards
                continue
            if (
                (self.spaces[dest_x][dest_y] == EMPTY_SPACE)
                and (abs(dest_x - start_x) == 2)
                and (abs(dest_y - start_y) == 2)
            ):
                return True, (start_x, start_y, dest_x, dest_y)

    def play_human_move(self, start_x, start_y, dest_x, dest_y):
        self.remove_pieces(start_x, start_y, dest_x, dest_y)

        if dest_y == TOP_ROW:
            # KING ME
            self.spaces[dest_x][dest_y] = HUMAN_KING

    def check_pieces(self):
        if len(list(self.get_spaces_with_computer_pieces())) == 0:
            print_human_won()
            return False
        if len(list(self.get_spaces_with_computer_pieces())) == 0:
            print_computer_won()
            return False
        return True


def print_instructions():
    print("THIS IS THE GAME OF CHECKERS.  THE COMPUTER IS X,")
    print("AND YOU ARE O.  THE COMPUTER WILL MOVE FIRST.")
    print("SQUARES ARE REFERRED TO BY A COORDINATE SYSTEM.")
    print("(0,0) IS THE LOWER LEFT CORNER")
    print("(0,7) IS THE UPPER LEFT CORNER")
    print("(7,0) IS THE LOWER RIGHT CORNER")
    print("(7,7) IS THE UPPER RIGHT CORNER")
    print("THE COMPUTER WILL TYPE '+TO' WHEN YOU HAVE ANOTHER")
    print("JUMP.  TYPE TWO NEGATIVE NUMBERS IF YOU CANNOT JUMP.")
    print()
    print()
    print()


def print_human_won():
    print()
    print("YOU WIN.")


def print_computer_won():
    print()
    print("I WIN.")


def play_game():
    board = Board()

    while True:
        move_record = board.pick_computer_move()
        if move_record is None:
            print_human_won()
            return
        board.play_computer_move(move_record)

        print(board)

        if not board.check_pieces():
            return

        start_x, start_y, dest_x, dest_y = board.get_human_move()
        board.play_human_move(start_x, start_y, dest_x, dest_y)
        if abs(dest_x - start_x) == 2:
            while True:
                extend, move = board.get_human_extension(dest_x, dest_y)
                if not extend:
                    break
                start_x, start_y, dest_x, dest_y = move
                board.play_human_move(start_x, start_y, dest_x, dest_y)


def main():
    print_header("CHECKERS")
    print_instructions()

    play_game()


if __name__ == "__main__":
    main()
