import math
import random


def print_n_whitespaces(n: int):
    print(" " * n, end="")


def print_n_newlines(n: int):
    for _ in range(n):
        print()


def subroutine_1610():
    B = 3 / A * random.random()
    if B < 0.37:
        C = 0.5
    elif B < 0.5:
        C = 0.4
    elif B < 0.63:
        C = 0.3
    elif B < 0.87:
        C = 0.2
    else:
        C = 0.1
    T = math.floor(10 * C + 0.2)
    print(f"THE {AS}{BS} DID A {LS[T]} JOB.")
    if T >= 4:
        if T == 5:
            # 1800 & 1810 are unreachable, so it's not presented here
            K = random.randint(1, 2)
            if K == 1:
                print(f"ONE OF THE {AS}{BS} WAS KILLED.")
            elif K == 2:
                print(f"NO {AS}{BS} WERE KILLED.")
        else:
            if AS != "TOREAD":
                K = random.randint(1, 2)
                print(f"{K} OF THE HORSES OF THE {AS}{BS} KILLED.")
            K = random.randint(1, 2)
            print(f"{K} OF THE {AS}{BS} KILLED.")
    print()
    return C


def FNC():
    Q = (
        4.5 + L / 6 - (D[1] + D[2]) * 2.5 + 4 * D[4] + 2 * D[5] - (D[3] ** 2) / 120 - A
    ) * random.random()
    return Q


print_n_whitespaces(34)
print("BULL")
print_n_whitespaces(15)
print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")

print_n_newlines(2)
L = 1

Z = input("DO YOU WANT INSTRUCTIONS? ")
if Z != "NO":
    print("HELLO, ALL YOU BLOODLOVERS AND AFICIONADOS.")
    print("HERE IS YOUR BIG CHANCE TO KILL A BULL.")
    print()
    print("ON EACH PASS OF THE BULL, YOU MAY TRY")
    print("0 - VERONICA (DANGEROUS INSIDE MOVE OF THE CAPE)")
    print("1 - LESS DANGEROUS OUTSIDE MOVE OF THE CAPE")
    print("2 - ORDINARY SWIRL OF THE CAPE.")
    print()
    print("INSTEAD OF THE ABOVE, YOU MAY TRY TO KILL THE BULL")
    print("ON ANY TURN: 4 (OVER THE HORNS), 5 (IN THE CHEST).")
    print("BUT IF I WERE YOU,")
    print("I WOULDN'T TRY IT BEFORE THE SEVENTH PASS.")
    print()
    print("THE CROWD WILL DETERMINE WHAT AWARD YOU DESERVE")
    print("(POSTHUMOUSLY IF NECESSARY).")
    print("THE BRAVER YOU ARE, THE BETTER THE AWARD YOU RECEIVE.")
    print()
    print("THE BETTER THE JOB THE PICADORES AND TOREADORES DO,")
    print("THE BETTER YOUR CHANCES ARE.")
print_n_newlines(2)

D = {4: 1, 5: 1}
LS = ["", "SUPERB", "GOOD", "FAIR", "POOR", "AWFUL"]
A = random.randint(1, 5)
print(f"YOU HAVE DRAWN A {LS[A]} BULL.")
if A > 4:
    print("YOU'RE LUCKY.")
elif A < 2:
    print("GOOD LUCK.  YOU'LL NEED IT.")
    print()
print()
AS = "PICADO"
BS = "RES"
C = subroutine_1610()
D[1] = C
AS = "TOREAD"
BS = "ORES"
subroutine_1610()
D[2] = C
print_n_newlines(2)
D[3] = 0
while True:
    D[3] = D[3] + 1  # 660
    print(f"PASS NUMBER {D[3]}")
    if D[3] >= 3:
        while True:  # 1930
            AS = input("HERE COMES THE BULL.  TRY FOR A KILL? ")
            if AS not in ["YES", "NO"]:
                print("INCORRECT ANSWER - - PLEASE TYPE 'YES' OR 'NO'.")
            else:
                break
        Z1 = 1 if AS == "YES" else 2
        if Z1 != 1:
            print("CAPE MOVE? ", end="")
        else:
            pass
            # goto 1130
    else:
        print("THE BULL IS CHARGING AT YOU!  YOU ARE THE MATADOR--")
        while True:  # 1930
            AS = input("DO YOU WANT TO KILL THE BULL? ")
            if AS not in ["YES", "NO"]:
                print("INCORRECT ANSWER - - PLEASE TYPE 'YES' OR 'NO'.")
            else:
                break
        Z1 = 1 if AS == "YES" else 2
        if Z1 != 1:
            print("WHAT MOVE DO YOU MAKE WITH THE CAPE? ", end="")
        else:
            # goto 1130
            pass
    gore = 0
    if Z1 != 1:  # NO
        while True:
            E = float(input())
            if E != float(int(abs(E))):
                print("DON'T PANIC, YOU IDIOT!  PUT DOWN A CORRECT NUMBER")
            elif E < 3:
                break
        if E == 0:
            M = 3
        elif E == 1:
            M = 2
        else:
            M = 0.5
        L = L + M
        F = (6 - A + M / 10) * random.random() / ((D[1] + D[2] + D[3] / 10) * 5)
        if F < 0.51:
            continue
        gore = 1
    else:  # YES
        print()
        print("IT IS THE MOMENT OF TRUTH.")
        print()
        H = int(input("HOW DO YOU TRY TO KILL THE BULL? "))
        if H not in [4, 5]:
            print("YOU PANICKED.  THE BULL GORED YOU.")
            gore = 2
            # goto 970
        else:
            K = (6 - A) * 10 * random.random() / ((D[1] + D[2]) * 5 * D[3])
            if H == 4:
                if K > 0.8:
                    gore = 1
            else:
                if K > 0.2:
                    gore = 1
            if gore == 0:
                print("YOU KILLED THE BULL!")
                D[5] = 2
                break
    if gore > 0:
        if gore == 1:
            print("THE BULL HAS GORED YOU!")
        death = False
        while True:
            _ = random.randint(1, 2)  # 970
            if _ == 1:
                print("YOU ARE DEAD.")
                D[4] = 1.5
                # goto 1320
                death = True
                break
            else:
                print("YOU ARE STILL ALIVE.")
                print()
                print("DO YOU RUN FROM THE RING? ", end="")
                while True:  # 1930
                    AS = input()
                    if AS not in ["YES", "NO"]:
                        print("INCORRECT ANSWER - - PLEASE TYPE 'YES' OR 'NO'.")
                    else:
                        break
                Z1 = 1 if AS == "YES" else 2
                if Z1 == 2:
                    print("YOU ARE BRAVE.  STUPID, BUT BRAVE.")
                    _ = random.randint(1, 2)
                    if _ == 1:
                        D[4] = 2
                        # goto 660, outter while loop
                        death = True
                        break
                    else:
                        print("YOU ARE GORED AGAIN!")
                        # goto 970
                else:
                    print("COWARD")
                    D[4] = 0
                    # goto 1310, break outter while loop
                    death = True
                    break

        if death:
            break


def main():
    # 1310
    print_n_newlines(3)
    if D[4] == 0:
        print("THE CROWD BOOS FOR TEN MINUTES.  IF YOU EVER DARE TO SHOW")
        print("YOUR FACE IN A RING AGAIN, THEY SWEAR THEY WILL KILL YOU--")
        print("UNLESS THE BULL DOES FIRST.")
    else:
        if D[4] == 2:
            print("THE CROWD CHEERS WILDLY!")
        elif D[5] == 2:
            print("THE CROWD CHEERS!")
            print()
        print("THE CROWD AWARDS YOU")
        if FNC() < 2.4:
            print("NOTHING AT ALL.")
        elif FNC() < 4.9:
            print("ONE EAR OF THE BULL.")
        elif FNC() < 7.4:
            print("BOTH EARS OF THE BULL!")
            print("OLE!")
        else:
            print("OLE!  YOU ARE 'MUY HOMBRE'!! OLE!  OLE!")
        print()
        print("ADIOS")
        print_n_newlines(3)


if __name__ == "__main__":
    main()
