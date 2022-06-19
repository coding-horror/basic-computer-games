"""
Life for Two

From: BASIC Computer Games (1978),
      Edited by David H. Ahl.

BASIC program written by Brian Wyvill of Bradford University in Yorkshire, England.

Python port created by Sajid Al Sanai, 2022.
"""

def tab(spaces):
    out = ""
    for i in range(spaces):
        out += " "
    return out

na = []
ka = [, 3, 102, 103, 120, 130, 121, 112, 111, 12, 21, 30, 1020, 1030, 1011, 1021, 1003, 1002, 1012]
aa = [, -1, 0, 1, 0, 0, -1, 0, 1, -1, -1, 1, -1, -1, 1, 1, 1]
xa = []
ya = []
j, k, m2, m3 = 0, 0, 0, 0

def main():
    print(tab(33) + "LIFE2\n")
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
    print("\n\n\n")
    print(tab(10) + "U.B. LIFE GAME\n")
    m2 = 0
    m3 = 0
    for j in range(7):
        na[j] = []
        for k in range(7):
            na[j].append(0)
    for b in range(1, 3):
        p1 = 30 if (b == 2) else 3
        print("\n")
        print("PLAYER {} - 3 LIVE PIECES.\n".format(b))
        for k1 in range(1, 4):
            while 1:
                print("X,Y\n")
                str_ = input()
                ya[b] = int(str_)
                xa[b] = #int(str_.substr() + 1)
                if xa[b] > 0 and xa[b] < 6 and ya[b] > 0 and ya[b] < 5 and na[xa[b]][ya[b]] == 0:
                    break
                print("ILLEGAL COORDS. RETYPE\n")
            if b != 1:
                if xa[1] == xa[2] and ya[1] == ya[2]:
                    print("SAME COORD.  SET TO 0\n")
                    na[xa[b] + 1][ya[b] + 1] = 0
                    b = 99
            na[xa[b]][ya[b]] = p1

if __name__ == "__main__":
    main()
