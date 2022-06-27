# Variable Initialisation
gn, gk, ga, gx, gy = [], [], [], [], []
gk = [3, 102, 103, 120, 130, 121, 112, 111, 12, 21, 30, 1020, 1030, 1011, 1021, 1003, 1002, 1012]
ga = [-1, 0, 1, 0, 0, -1, 0, 1, -1, -1, 1, -1, -1, 1, 1, 1]
m2, m3 = 0, 0

# Initialise the board
for j in range(6):
    gn[j] = []
    for k in range(6):
        gn[j][k] = 0

# Helper Functions
def tab(number) -> str:
    t = ""
    while len(t) < number:
        t += " "
    return t

def display_header() -> None:
    print("{}LIFE2\n".format(tab(33)))
    print("{}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n\n".format(tab(15)))
    print("{}U.B. LIFE GAME".format(tab(10)))

# Board Functions
def setup_board() -> None:
    # Players add symbols to initially setup the board
    for b in range(1, 3):
        p1 = 30 if b == 2 else 3
        print("\n\nPLAYER {} - 3 LIVE PIECES.".format(b))
        for k1 in range(1, 3):
            query_player(b)
            gn[gx[b]][gy[b]] = p1

def modify_board() -> None:
    # Players take turns to add symbols and modify the board
    for b in range(1, 3):
        print("\n\nPLAYER {} ".format(b))
        query_player(b)
        if b == 99:
            break
    if b <= 2:
        gn[gx[1]][gy[1]] = 100
        gn[gx[2]][gy[2]] = 1000

def simulate_board() -> None:
    # Simulate the board for one step
    for j in range(1, 6):
        for k in range(1, 6):
            if gn[j][k] > 99:
                b = 1 if gn[j][k] <= 999 else 10
                for o1 in range(1, 16, 2):
                    gn[j + ga[o1]][k + ga[o1 + 1]] = gn[j + ga[o1]][k + ga[o1 + 1]] + b

def display_board() -> None:
    # Draws the board with all symbols
    m2, m3 = 0, 0
    for j in range(7):
        print("\n")
        for k in range(7):
            if j == 0 or j == 6:
                if k != 6:
                    print(" " + str(k) + " ")
                else:
                    print(" 0 ")
            if k == 0 or k == 6:
                if j != 6:
                    print(" " + str(j) + " ")
                else:
                    print(" 0\n")
            else:
                if gn[j][k] < 3:
                    gn[j][k] = 0
                    print("   ")
                else:
                    for o1 in range(19):
                        if gn[j][k] == gk[o1]:
                            if o1 > 9:
                                gn[j][k] = 1000
                                m3 += 1
                                print(" # ")
                            else:
                                gn[j, k] = 100
                                m2 += 1
                                print(" * ")
                    gn[j][k] = 0
                    print("   ")

# Player Functions
def query_player(b) -> None:
    # Query player for symbol placement coordinates
    while True:
        print("X,Y\nXXXXXX\n$$$$$$\n&&&&&&\n")
        x_ = input("")
        y_ = input(" ")
        x_ = [int(num) for num in x_.split() if num.isdigit()][0]
        y_ = [int(num) for num in y_.split() if num.isdigit()][0]
        gx[b], gy[b] = x_, y_
        if gx[b] in range(1, 6) and gy[b] in range(1, 6) and gn[gx[b]][gy[b]] != 0:
            break
        print("ILLEGAL COORDS. RETYPE\n")
    if b != 1:
        if gx[1] == gx[2] and gy[1] == gy[2]:
            print("SAME COORD. SET TO 0")
            gn[gx[b] + 1][gy[b] + 1] = 0
            b = 99

# Game Functions
def check_winner(m2, m3) -> None:
    # Check if the game has been won
    if m2 == 0 and m3 == 0:
        print("\nA DRAW\n")
        return
    if m3 == 0:
        print("\nPLAYER 1 IS THE WINNER\n")
        return
    if m2 == 0:
        print("\nPLAYER 2 IS THE WINNER\n")
        return

# Program Flow
def main() -> None:
    display_header()

    setup_board()
    display_board()
    
    while True:
        print("\n")
        simulate_board()
        display_board()
        check_winner()
        modify_board()

if __name__ == "__main__":
    main()
