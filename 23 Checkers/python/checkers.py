"""
CHECKERS

How about a nice game of checkers?

Ported by Dave LeCompte
"""


PAGE_WIDTH = 64

HUMAN_PLAYER = 1
COMPUTER_PLAYER = -1
HUMAN_PIECE = 1
HUMAN_KING = 2
COMPUTER_PIECE = -1
COMPUTER_KING = -2
EMPTY_SPACE = 0

INVALID_MOVE = -99


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title):
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


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


def get_spaces():
    for x in range(0, 8):
        for y in range(0, 8):
            yield x, y


def get_spaces_with_computer_pieces(board):
    for x, y in get_spaces():
        contents = board.spaces[x][y]
        if contents < 0:
            yield x, y


def get_spaces_with_human_pieces(board):
    for x, y in get_spaces():
        contents = board.spaces[x][y]
        if contents > 0:
            yield x, y


def pick_computer_move(board):
    r = [INVALID_MOVE] * 5
    for x, y in get_spaces_with_computer_pieces(board):
        contents = board.spaces[x][y]
        if contents == COMPUTER_PIECE:
            for dx in (-1, 1):
                dy = -1
                sub_650(board, x, y, dx, dy, r)
        else:
            for dx in (-1, 1):
                for dy in (-1, 1):
                    sub_650(board, x, y, dx, dy, r)

    if r[0] != INVALID_MOVE:
        dx = r[3] - r[1]
        dy = r[4] - r[2]
        if abs(dx) != abs(dy):
            print(r)
        assert abs(dx) == abs(dy)
    return r


def sub_650(board, x, y, dx, dy, r):
    new_x = x + dx
    new_y = y + dy
    if not ((0 <= new_x <= 7) and (0 <= new_y <= 7)):
        return

    contents = board.spaces[new_x][new_y]
    if contents == 0:
        sub_910(board, x, y, new_x, new_y, r)
        return
    if contents < 0:
        return

    # check landing space
    landing_x = new_x + dx
    landing_y = new_y + dy

    # line 790
    if not ((0 <= landing_x <= 7) and (0 <= landing_y <= 7)):
        return
    if board.spaces[landing_x][landing_y] == 0:
        sub_910(board, x, y, landing_x, landing_y, r)


def sub_910(board, start_x, start_y, dest_x, dest_y, r):
    q = 0
    if dest_y == 0 and board.spaces[start_x][start_y] == COMPUTER_PIECE:
        q += 2
    if abs(start_y - dest_y) == 2:
        q += 5
    if start_y == 7:
        q -= 2
    if dest_x in (0, 7):
        q += 1
    for c in (-1, 1):
        if (0 <= dest_x + c <= 7) and (1 <= dest_y):
            # line 1035
            if board.spaces[dest_x + c][dest_y - 1] < 0:
                q += 1
            # line 1040
            elif (0 <= dest_x - c <= 7) and (dest_y + 1 <= 7):
                # line 1045
                if (
                    (board.spaces[dest_x + c][dest_y - 1] > 0)
                    and (board.spaces[dest_x - c][dest_y + 1] == EMPTY_SPACE)
                    or ((dest_x - c == start_x) and (dest_y + 1 == start_y))
                ):
                    q -= 2
    # line 1080

    if q > r[0]:
        r[0] = q
        r[1] = start_x
        r[2] = start_y
        r[3] = dest_x
        r[4] = dest_y


def remove_r_pieces(board, r):
    remove_pieces(board, r[1], r[2], r[3], r[4])


def remove_pieces(board, start_x, start_y, dest_x, dest_y):
    board.spaces[dest_x][dest_y] = board.spaces[start_x][start_y]
    board.spaces[start_x][start_y] = EMPTY_SPACE

    if abs(dest_x - start_x) == 2:
        mid_x = (start_x + dest_x) // 2
        mid_y = (start_y + dest_y) // 2
        board.spaces[mid_x][mid_y] = EMPTY_SPACE


def play_computer_move(board, r):
    print(f"FROM {r[1]} {r[2]} TO {r[3]} {r[4]}")

    while True:
        if r[4] == 0:
            # KING ME
            board.spaces[r[3]][r[4]] = COMPUTER_KING
            remove_r_pieces(board, r)
            return
        else:
            # line 1250
            board.spaces[r[3]][r[4]] = board.spaces[r[1]][r[2]]
            remove_r_pieces(board, r)

            if abs(r[1] - r[3]) != 2:
                return

            # line 1340
            x = r[3]
            y = r[4]
            r[0] = INVALID_MOVE
            if board.spaces[x][y] == COMPUTER_PIECE:
                for a in (-2, 2):
                    try_extend(board, r, x, y, a, -2)
            else:
                assert board.spaces[x][y] == COMPUTER_KING
                for a in (-2, 2):
                    for b in (-2, 2):
                        try_extend(board, r, x, y, a, b)
            if r[0] != INVALID_MOVE:
                print(f"TO {r[3]} {r[4]}")
            else:
                return


def try_extend(board, r, x, y, a, b):
    # line 1370
    nx = x + a
    ny = y + b
    if not ((0 <= nx <= 7) and (0 <= ny <= 7)):
        return
    if (board.spaces[nx][ny] == EMPTY_SPACE) and (
        board.spaces[nx + a // 2][ny + b // 2] > 0
    ):
        sub_910(board, x, y, nx, ny, r)


def get_human_move(board):
    is_king = False

    while True:
        print("FROM?")
        from_response = input()
        x, y = [int(c) for c in from_response.split(",")]

        if board.spaces[x][y] > 0:
            break

    is_king = board.spaces[x][y] == HUMAN_KING

    while True:
        print("TO?")
        to_response = input()
        a, b = [int(c) for c in to_response.split(",")]

        if (not is_king) and (b < y):
            # CHEATER! Trying to move non-king backwards
            continue
        if (
            (board.spaces[a][b] == 0)
            and (abs(a - x) <= 2)
            and (abs(a - x) == abs(b - y))
        ):
            break
    return x, y, a, b


def get_human_extension(board, sx, sy):
    is_king = board.spaces[sx][sy] == HUMAN_KING

    while True:
        print("+TO?")
        to_response = input()
        a1, b1 = [int(c) for c in to_response.split(",")]
        if a1 < 0:
            return False, None
        if (not is_king) and (b1 < sy):
            # CHEATER! Trying to move non-king backwards
            continue
        if (
            (board.spaces[a1][b1] == EMPTY_SPACE)
            and (abs(a1 - sx) == 2)
            and (abs(b1 - sy) == 2)
        ):
            return True, (sx, sy, a1, b1)


def play_human_move(board, start_x, start_y, dest_x, dest_y):
    remove_pieces(board, start_x, start_y, dest_x, dest_y)

    if dest_y == 7:
        # KING ME
        board.spaces[dest_x][dest_y] = HUMAN_KING


def print_human_won():
    print()
    print("YOU WIN.")


def print_computer_won():
    print()
    print("I WIN.")


def check_pieces(board):
    if len(list(get_spaces_with_computer_pieces(board))) == 0:
        print_human_won()
        return False
    if len(list(get_spaces_with_computer_pieces(board))) == 0:
        print_computer_won()
        return False
    return True


def play_game():
    board = Board()

    while True:
        r = pick_computer_move(board)
        if r[0] == INVALID_MOVE:
            print_human_won()
            return
        play_computer_move(board, r)

        print(board)

        if not check_pieces(board):
            return

        sx, sy, dx, dy = get_human_move(board)
        play_human_move(board, sx, sy, dx, dy)
        if abs(dx - sx) == 2:
            while True:
                extend, move = get_human_extension(board, dx, dy)
                if not extend:
                    break
                sx, sy, dx, dy = move
                play_human_move(board, sx, sy, dx, dy)


def main():
    print_header("CHECKERS")
    print_instructions()

    play_game()


if __name__ == "__main__":
    main()
