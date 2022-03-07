import random


def print_n_whitespaces(n: int):
    print(" " * n, end="")


def print_n_newlines(n: int):
    for _ in range(n):
        print()


print_n_whitespaces(32)
print("BULLSEYE")
print_n_whitespaces(15)
print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
print_n_newlines(3)
print("IN THIS GAME, UP TO 20 PLAYERS THROW DARTS AT A TARGET")
print("WITH 10, 20, 30, AND 40 POINT ZONES.  THE OBJECTIVE IS")
print("TO GET 200 POINTS.")
print()
print("THROW", end="")
print_n_whitespaces(20)
print("DESCRIPTION", end="")
print_n_whitespaces(45)
print("PROBABLE SCORE")
print(" 1", end="")
print_n_whitespaces(20)
print("FAST OVERARM", end="")
print_n_whitespaces(45)
print("BULLSEYE OR COMPLETE MISS")
print(" 2", end="")
print_n_whitespaces(20)
print("CONTROLLED OVERARM", end="")
print_n_whitespaces(45)
print("10, 20 OR 30 POINTS")
print(" 3", end="")
print_n_whitespaces(20)
print("UNDERARM", end="")
print_n_whitespaces(45)
print("ANYTHING")
print()

M = 0
R = 0

W = {}
for I in range(1, 11):
    W[I] = 0

S = {}
for I in range(1, 21):
    S[I] = 0

N = int(input("HOW MANY PLAYERS? "))
A = {}
for I in range(1, N + 1):
    Name = input("NAME OF PLAYER #")
    A[I] = Name

while M == 0:
    R = R + 1
    print()
    print(f"ROUND {R}---------")
    for I in range(1, N + 1):
        print()
        while True:
            T = int(input(f"{A[I]}'S THROW? "))
            if T < 1 or T > 3:
                print("INPUT 1, 2, OR 3!")
            else:
                break
        if T == 1:
            P1 = 0.65
            P2 = 0.55
            P3 = 0.5
            P4 = 0.5
        elif T == 2:
            P1 = 0.99
            P2 = 0.77
            P3 = 0.43
            P4 = 0.01
        elif T == 3:
            P1 = 0.95
            P2 = 0.75
            P3 = 0.45
            P4 = 0.05
        U = random.random()
        if U >= P1:
            print("BULLSEYE!!  40 POINTS!")
            B = 40
        elif U >= P2:
            print("30-POINT ZONE!")
            B = 30
        elif U >= P3:
            print("20-POINT ZONE")
            B = 20
        elif U >= P4:
            print("WHEW!  10 POINTS.")
            B = 10
        else:
            print("MISSED THE TARGET!  TOO BAD.")
            B = 0
        S[I] = S[I] + B
        print(f"TOTAL SCORE = {S[I]}")
    for I in range(1, N + 1):
        if S[I] > 200:
            M = M + 1
            W[M] = I

print()
print("WE HAVE A WINNER!!")
print()
for I in range(1, M + 1):
    print(f"{A[W[I]]} SCORED {S[W[I]]} POINTS.")
print()
print("THANKS FOR THE GAME.")
