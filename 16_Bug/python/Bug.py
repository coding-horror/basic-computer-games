import random
import time


def print_n_whitespaces(n: int):
    print(" " * n, end="")


def print_n_newlines(n: int):
    for _ in range(n):
        print()


def print_feelers(n_feelers, is_player=True):
    for _ in range(4):
        print_n_whitespaces(10)
        for _ in range(n_feelers):
            print("A " if is_player else "F ", end="")
        print()


def print_head():
    print("        HHHHHHH")
    print("        H     H")
    print("        H O O H")
    print("        H     H")
    print("        H  V  H")
    print("        HHHHHHH")


def print_neck():
    print("          N N")
    print("          N N")


def print_body(has_tail=False):
    print("     BBBBBBBBBBBB")
    print("     B          B")
    print("     B          B")
    print("TTTTTB          B") if has_tail else ""
    print("     BBBBBBBBBBBB")


def print_legs(n_legs):
    for _ in range(2):
        print_n_whitespaces(5)
        for _ in range(n_legs):
            print(" L", end="")
        print()


print_n_whitespaces(34)
print("BUG")
print_n_whitespaces(15)
print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
print_n_newlines(3)

print("THE GAME BUG")
print("I HOPE YOU ENJOY THIS GAME.")
print()
Z = input("DO YOU WANT INSTRUCTIONS? ")
if Z != "NO":
    print("THE OBJECT OF BUG IS TO FINISH YOUR BUG BEFORE I FINISH")
    print("MINE. EACH NUMBER STANDS FOR A PART OF THE BUG BODY.")
    print("I WILL ROLL THE DIE FOR YOU, TELL YOU WHAT I ROLLED FOR YOU")
    print("WHAT THE NUMBER STANDS FOR, AND IF YOU CAN GET THE PART.")
    print("IF YOU CAN GET THE PART I WILL GIVE IT TO YOU.")
    print("THE SAME WILL HAPPEN ON MY TURN.")
    print("IF THERE IS A CHANGE IN EITHER BUG I WILL GIVE YOU THE")
    print("OPTION OF SEEING THE PICTURES OF THE BUGS.")
    print("THE NUMBERS STAND FOR PARTS AS FOLLOWS:")
    table = [
        ["NUMBER", "PART", "NUMBER OF PART NEEDED"],
        ["1", "BODY", "1"],
        ["2", "NECK", "1"],
        ["3", "HEAD", "1"],
        ["4", "FEELERS", "2"],
        ["5", "TAIL", "1"],
        ["6", "LEGS", "6"],
    ]
    for row in table:
        print(f"{row[0]:<16}{row[1]:<16}{row[2]:<20}")
    print_n_newlines(2)

A = 0
B = 0
H = 0
L = 0
N = 0
P = 0
Q = 0
R = 0  # NECK
S = 0  # FEELERS
T = 0
U = 0
V = 0
Y = 0

while not (Y > 0):
    Z = random.randint(1, 6)
    print()
    C = 1
    print("YOU ROLLED A", Z)
    if Z == 1:
        print("1=BODY")
        if B == 1:
            print("YOU DO NOT NEED A BODY.")
            # goto 970
        else:
            print("YOU NOW HAVE A BODY.")
            B = 1
            C = 0
            # goto 970
    elif Z == 2:
        print("2=NECK")
        if N == 1:
            print("YOU DO NOT NEED A NECK.")
            # goto 970
        elif B == 0:
            print("YOU DO NOT HAVE A BODY.")
            # goto 970
        else:
            print("YOU NOW HAVE A NECK.")
            N = 1
            C = 0
            # goto 970
    elif Z == 3:
        print("3=HEAD")
        if N == 0:
            print("YOU DO NOT HAVE A NECK.")
            # goto 970
        elif H == 1:
            print("YOU HAVE A HEAD.")
            # goto 970
        else:
            print("YOU NEEDED A HEAD.")
            H = 1
            C = 0
            # goto 970
    elif Z == 4:
        print("4=FEELERS")
        if H == 0:
            print("YOU DO NOT HAVE A HEAD.")
            # goto 970
        elif A == 2:
            print("YOU HAVE TWO FEELERS ALREADY.")
            # goto 970
        else:
            print("I NOW GIVE YOU A FEELER.")
            A = A + 1
            C = 0
            # goto 970
    elif Z == 5:
        print("5=TAIL")
        if B == 0:
            print("YOU DO NOT HAVE A BODY.")
            # goto 970
        elif T == 1:
            print("YOU ALREADY HAVE A TAIL.")
            # goto 970
        else:
            print("I NOW GIVE YOU A TAIL.")
            T = T + 1
            C = 0
            # goto 970
    elif Z == 6:
        print("6=LEG")
        if L == 6:
            print("YOU HAVE 6 FEET ALREADY.")
            # goto 970
        elif B == 0:
            print("YOU DO NOT HAVE A BODY.")
            # goto 970
        else:
            L = L + 1
            C = 0
            print(f"YOU NOW HAVE {L} LEGS")
            # goto 970

    # 970
    X = random.randint(1, 6)
    print()
    time.sleep(2)

    print("I ROLLED A", X)
    if X == 1:
        print("1=BODY")
        if P == 1:
            print("I DO NOT NEED A BODY.")
            # goto 1630
        else:
            print("I NOW HAVE A BODY.")
            C = 0
            P = 1
            # goto 1630
    elif X == 2:
        print("2=NECK")
        if Q == 1:
            print("I DO NOT NEED A NECK.")
            # goto 1630
        elif P == 0:
            print("I DO NOT HAVE A BODY.")
            # goto 1630
        else:
            print("I NOW HAVE A NECK.")
            Q = 1
            C = 0
            # goto 1630
    elif X == 3:
        print("3=HEAD")
        if Q == 0:
            print("I DO NOT HAVE A NECK.")
            # goto 1630
        elif R == 1:
            print("I HAVE A HEAD.")
            # goto 1630
        else:
            print("I NEEDED A HEAD.")
            R = 1
            C = 0
            # goto 1630
    elif X == 4:
        print("4=FEELERS")
        if R == 0:
            print("I DO NOT HAVE A HEAD.")
            # goto 1630
        elif S == 2:
            print("I HAVE TWO FEELERS ALREADY.")
            # goto 1630
        else:
            print("I GET A FEELER.")
            S = S + 1
            C = 0
            # goto 1630
    elif X == 5:
        print("5=TAIL")
        if P == 0:
            print("I DO NOT HAVE A BODY.")
            # goto 1630
        elif U == 1:
            print("I ALREADY HAVE A TAIL.")
            # goto 1630
        else:
            print("I NOW HAVE A TAIL.")
            U = 1
            C = 0
            # goto 1630
    elif X == 6:
        print("6=LEG")
        if V == 6:
            print("I HAVE 6 FEET.")
            # goto 1630
        elif P == 0:
            print("I DO NOT HAVE A BODY.")
            # goto 1630
        else:
            V = V + 1
            C = 0
            print(f"I NOW HAVE {V} LEGS")
            # goto 1630

    # 1630
    if (A == 2) and (T == 1) and (L == 6):
        print("YOUR BUG IS FINISHED.")
        Y = Y + 1
    if (S == 2) and (P == 1) and (V == 6):
        print("MY BUG IS FINISHED.")
        Y = Y + 2
    if C == 1:
        continue
    Z = input("DO YOU WANT THE PICTURES? ")
    if Z != "NO":
        print("*****YOUR BUG*****")
        print_n_newlines(2)
        if A != 0:
            print_feelers(A, is_player=True)
        if H != 0:
            print_head()
        if N != 0:
            print_neck()
        if B != 0:
            print_body(True) if T == 1 else print_body(False)
        if L != 0:
            print_legs(L)
        print_n_newlines(4)
        print("*****MY BUG*****")
        print_n_newlines(3)
        if S != 0:
            print_feelers(S, is_player=False)
        if R == 1:
            print_head()
        if Q != 0:
            print_neck()
        if P != 0:
            print_body(True) if U == 1 else print_body(False)
        if V != 0:
            print_legs(V)

        if Y != 0:
            break

print("I HOPE YOU ENJOYED THE GAME, PLAY IT AGAIN SOON!!")
