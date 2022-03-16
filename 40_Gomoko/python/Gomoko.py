import random
from typing import Any, List, Tuple


def print_n_whitespaces(n: int) -> None:
    print(" " * n, end="")


def print_board(A: List[List[Any]], n):
    """PRINT THE BOARD"""
    for i in range(n):
        print(" ", end="")
        for j in range(n):
            print(A[i][j], end="")
            print(" ", end="")
        print()


def check_move(_I, _J, _N) -> bool:  # 910
    if _I < 1 or _I > _N or _J < 1 or _J > _N:
        return False
    return True


def print_banner():
    print_n_whitespaces(33)
    print("GOMOKU")
    print_n_whitespaces(15)
    print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()
    print("WELCOME TO THE ORIENTAL GAME OF GOMOKO.")
    print()
    print("THE GAME IS PLAYED ON AN N BY N GRID OF A SIZE")
    print("THAT YOU SPECIFY.  DURING YOUR PLAY, YOU MAY COVER ONE GRID")
    print("INTERSECTION WITH A MARKER. THE OBJECT OF THE GAME IS TO GET")
    print("5 ADJACENT MARKERS IN A ROW -- HORIZONTALLY, VERTICALLY, OR")
    print("DIAGONALLY.  ON THE BOARD DIAGRAM, YOUR MOVES ARE MARKED")
    print("WITH A '1' AND THE COMPUTER MOVES WITH A '2'.")
    print()
    print("THE COMPUTER DOES NOT KEEP TRACK OF WHO HAS WON.")
    print("TO END THE GAME, TYPE -1,-1 FOR YOUR MOVE.")
    print()


def get_board_dimensions() -> int:
    n = 0
    while True:
        n = input("WHAT IS YOUR BOARD SIZE (MIN 7/ MAX 19)? ")
        n = int(n)
        if n < 7 or n > 19:
            print("I SAID, THE MINIMUM IS 7, THE MAXIMUM IS 19.")
            print()
        else:
            break
    return n


def get_move() -> Tuple[int, int]:
    while True:
        xy = input("YOUR PLAY (I,J)? ")
        print()
        x, y = xy.split(",")
        try:
            x = int(x)
            y = int(y)
        except Exception:
            print("ILLEGAL MOVE.  TRY AGAIN...")
            continue
        return x, y


def initialize_board(n: int) -> List[List[int]]:
    # Initialize the board
    board = []
    for _x in range(n):
        subA = []
        for _y in range(n):
            subA.append(0)
        board.append(subA)
    return board


def main():
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
            else:
                if board[x - 1][y - 1] != 0:
                    print("SQUARE OCCUPIED.  TRY AGAIN...")
                else:
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
                                else:
                                    if board[X - 1][Y - 1] != 0:
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
                                        board[X - 1][Y - 1] = 2
                                        print_board(board, n)
        print()
        print("THANKS FOR THE GAME!!")
        repeat = input("PLAY AGAIN (1 FOR YES, 0 FOR NO)? ")
        repeat = int(repeat)
        if repeat == 0:
            break


if __name__ == "__main__":
    main()
