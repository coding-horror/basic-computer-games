import random
from typing import Any, List, Tuple


def print_board(A: List[List[Any]], n: int) -> None:
    """PRINT THE BOARD"""
    for i in range(n):
        print(" ", end="")
        for j in range(n):
            print(A[i][j], end="")
            print(" ", end="")
        print()


def check_move(_I, _J, _N) -> bool:  # 910
    return _I >= 1 and _I <= _N and _J >= 1 and _J <= _N


def print_banner() -> None:
    print(" " * 33 + "GOMOKU")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")
    print("WELCOME TO THE ORIENTAL GAME OF GOMOKO.\n")
    print("THE GAME IS PLAYED ON AN N BY N GRID OF A SIZE")
    print("THAT YOU SPECIFY.  DURING YOUR PLAY, YOU MAY COVER ONE GRID")
    print("INTERSECTION WITH A MARKER. THE OBJECT OF THE GAME IS TO GET")
    print("5 ADJACENT MARKERS IN A ROW -- HORIZONTALLY, VERTICALLY, OR")
    print("DIAGONALLY.  ON THE BOARD DIAGRAM, YOUR MOVES ARE MARKED")
    print("WITH A '1' AND THE COMPUTER MOVES WITH A '2'.\n")
    print("THE COMPUTER DOES NOT KEEP TRACK OF WHO HAS WON.")
    print("TO END THE GAME, TYPE -1,-1 FOR YOUR MOVE.\n")


def get_board_dimensions() -> int:
    n = 0
    while True:
        n = int(input("WHAT IS YOUR BOARD SIZE (MIN 7/ MAX 19)? "))
        if n >= 7 and n <= 19:
            break
        print("I SAID, THE MINIMUM IS 7, THE MAXIMUM IS 19.")
        print()
    return n


def get_move() -> Tuple[int, int]:
    while True:
        xy = input("YOUR PLAY (I,J)? ")
        print()
        x_str, y_str = xy.split(",")
        try:
            x = int(x_str)
            y = int(y_str)
        except Exception:
            print("ILLEGAL MOVE.  TRY AGAIN...")
            continue
        return x, y


def initialize_board(n: int) -> List[List[int]]:
    # Initialize the board
    board = []
    for _x in range(n):
        sub_a = [0 for _y in range(n)]
        board.append(sub_a)
    return board


def main() -> None:
    print_banner()

    while True:
        n = get_board_dimensions()
        board = initialize_board(n)

        print()
        print()
        print("WE ALTERNATE MOVES. YOU GO FIRST...")
        print()

        while True:
            x, y = get_move()
            if x == -1:
                break
            elif not check_move(x, y, n):
                print("ILLEGAL MOVE.  TRY AGAIN...")
            elif board[x - 1][y - 1] == 0:
                board[x - 1][y - 1] = 1
                # COMPUTER TRIES AN INTELLIGENT MOVE
                skip_ef_loop = False
                for E in range(-1, 2):
                    for F in range(-1, 2):
                        if E + F - E * F == 0 or skip_ef_loop:
                            continue
                        X = x + F
                        Y = y + F
                        if not check_move(X, Y, n):
                            continue
                        if board[X - 1][Y - 1] == 1:
                            skip_ef_loop = True
                            X = x - E
                            Y = y - F
                            if not check_move(X, Y, n):  # 750
                                while True:  # 610
                                    X = random.randint(1, n)
                                    Y = random.randint(1, n)
                                    if (
                                        check_move(X, Y, n)
                                        and board[X - 1][Y - 1] == 0
                                    ):
                                        board[X - 1][Y - 1] = 2
                                        print_board(board, n)
                                        break
                            elif board[X - 1][Y - 1] == 0:
                                board[X - 1][Y - 1] = 2
                                print_board(board, n)
                            else:
                                while True:
                                    X = random.randint(1, n)
                                    Y = random.randint(1, n)
                                    if (
                                        check_move(X, Y, n)
                                        and board[X - 1][Y - 1] == 0
                                    ):
                                        board[X - 1][Y - 1] = 2
                                        print_board(board, n)
                                        break
            else:
                print("SQUARE OCCUPIED.  TRY AGAIN...")
        print()
        print("THANKS FOR THE GAME!!")
        repeat = int(input("PLAY AGAIN (1 FOR YES, 0 FOR NO)? "))
        if repeat == 0:
            break


if __name__ == "__main__":
    main()
