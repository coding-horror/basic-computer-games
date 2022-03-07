import random


def print_n_whitespaces(n: int):
    print(" " * n, end="")


def print_board():
    """PRINT THE BOARD"""
    for I in range(N):
        print(" ", end="")
        for J in range(N):
            print(A[I][J], end="")
            print(" ", end="")
        print()


def check_move(_I, _J, _N) -> bool:  # 910
    if _I < 1 or _I > _N or _J < 1 or _J > _N:
        return False
    return True


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

while True:
    N = 0
    while True:
        N = input("WHAT IS YOUR BOARD SIZE (MIN 7/ MAX 19)? ")
        N = int(N)
        if N < 7 or N > 19:
            print("I SAID, THE MINIMUM IS 7, THE MAXIMUM IS 19.")
            print()
        else:
            break

    # Initialize the board
    A = []
    for I in range(N):
        subA = []
        for J in range(N):
            subA.append(0)
        A.append(subA)
    print()
    print()
    print("WE ALTERNATE MOVES. YOU GO FIRST...")
    print()

    while True:
        IJ = input("YOUR PLAY (I,J)? ")
        print()
        I, J = IJ.split(",")
        try:
            I = int(I)
            J = int(J)
        except:
            print("ILLEGAL MOVE.  TRY AGAIN...")
            continue
        if I == -1:
            break
        elif check_move(I, J, N) == False:
            print("ILLEGAL MOVE.  TRY AGAIN...")
        else:
            if A[I - 1][J - 1] != 0:
                print("SQUARE OCCUPIED.  TRY AGAIN...")
            else:
                A[I - 1][J - 1] = 1
                # COMPUTER TRIES AN INTELLIGENT MOVE
                SkipEFLoop = False
                for E in range(-1, 2):
                    for F in range(-1, 2):
                        if E + F - E * F == 0 or SkipEFLoop:
                            continue
                        X = I + F
                        Y = J + F
                        if check_move(X, Y, N) == False:
                            continue
                        if A[X - 1][Y - 1] == 1:
                            SkipEFLoop = True
                            X = I - E
                            Y = J - F
                            if check_move(X, Y, N) == False:  # 750
                                while True:  # 610
                                    X = random.randint(1, N)
                                    Y = random.randint(1, N)
                                    if check_move(X, Y, N) and A[X - 1][Y - 1] == 0:
                                        A[X - 1][Y - 1] = 2
                                        print_board()
                                        break
                            else:
                                if A[X - 1][Y - 1] != 0:
                                    while True:
                                        X = random.randint(1, N)
                                        Y = random.randint(1, N)
                                        if check_move(X, Y, N) and A[X - 1][Y - 1] == 0:
                                            A[X - 1][Y - 1] = 2
                                            print_board()
                                            break
                                else:
                                    A[X - 1][Y - 1] = 2
                                    print_board()
    print()
    print("THANKS FOR THE GAME!!")
    Repeat = input("PLAY AGAIN (1 FOR YES, 0 FOR NO)? ")
    Repeat = int(Repeat)
    if Repeat == 0:
        break
# print_board()
