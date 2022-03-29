# ONE CHECK

# Port to python by imiro


def tab(x):
    return " " * x


def main() -> None:

    # Initial instructions
    print(tab(30) + "ONE CHECK")
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()
    print("SOLITAIRE CHECKER PUZZLE BY DAVID AHL")
    print()
    print("48 CHECKERS ARE PLACED ON THE 2 OUTSIDE SPACES OF A")
    print("STANDARD 64-SQUARE CHECKERBOARD.  THE OBJECT IS TO")
    print("REMOVE AS MANY CHECKERS AS POSSIBLE BY DIAGONAL JUMPS")
    print("(AS IN STANDARD CHECKERS).  USE THE NUMBERED BOARD TO")
    print("INDICATE THE SQUARE YOU WISH TO JUMP FROM AND TO.  ON")
    print("THE BOARD PRINTED OUT ON EACH TURN '1' INDICATES A")
    print("CHECKER AND '0' AN EMPTY SQUARE.  WHEN YOU HAVE NO")
    print("POSSIBLE JUMPS REMAINING, INPUT A '0' IN RESPONSE TO")
    print("QUESTION 'JUMP FROM ?'")
    print()
    print("HERE IS THE NUMERICAL BOARD:")
    print()

    while True:
        for j in range(1, 64, 8):
            for i in range(j, j + 7):
                print(i, end=(" " * (3 if i < 10 else 2)))
            print(j + 7)
        print()
        print("AND HERE IS THE OPENING POSITION OF THE CHECKERS.")
        print()

        (jumps, left) = play_game()

        print()
        print("YOU MADE " + jumps + " JUMPS AND HAD " + left + " PIECES")
        print("REMAINING ON THE BOARD.")
        print()

        if not (try_again()):
            break

    print()
    print("O.K.  HOPE YOU HAD FUN!!")


def play_game():
    # Initialize board
    # Give more than 64 elements to accomodate 1-based indexing
    board = [1] * 70
    for j in range(19, 44, 8):
        for i in range(j, j + 4):
            board[i] = 0
    jumps = 0
    while True:
        # print board
        for j in range(1, 64, 8):
            for i in range(j, j + 7):
                print(board[i], end=" ")
            print(board[j + 7])
        print()

        while True:
            print("JUMP FROM", end=" ")
            f = input()
            f = int(f)
            if f == 0:
                break
            print("TO", end=" ")
            t = input()
            t = int(t)
            print()

            # Check legality of move
            f1 = (f - 1) // 8
            f2 = f - 8 * f1
            t1 = (t - 1) // 8
            t2 = t - 8 * t1
            if (
                f1 > 7
                or t1 > 7
                or f2 > 8
                or t2 > 8
                or abs(f1 - t1) != 2
                or abs(f2 - t2) != 2
                or board[(t + f) // 2] == 0
                or board[f] == 0
                or board[t] == 1
            ):
                print("ILLEGAL MOVE.  TRY AGAIN...")
                continue
            break

        if f == 0:
            break
        board[t] = 1
        board[f] = 0
        board[(t + f) // 2] = 0
        jumps = jumps + 1

    left = 0
    for i in range(1, 64 + 1):
        left = left + board[i]
    return (str(jumps), str(left))


def try_again():
    print("TRY AGAIN", end=" ")
    answer = input()
    if answer.upper() == "YES":
        return True
    elif answer.upper() == "NO":
        return False
    print("PLEASE ANSWER 'YES' OR 'NO'.")
    try_again()


if __name__ == "__main__":
    main()
