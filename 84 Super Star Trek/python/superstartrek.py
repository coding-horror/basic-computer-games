
# ****        **** STAR TREK ****        ****
# **** SIMULATION OF A MISSION OF THE STARSHIP ENTERPRISE,
# **** AS SEEN ON THE STAR TREK TV SHOW.
# **** ORIGIONAL PROGRAM BY MIKE MAYFIELD, MODIFIED VERSION
# **** PUBLISHED IN DEC'S "101 BASIC GAMES", BY DAVE AHL.
# **** MODIFICATIONS TO THE LATTER (PLUS DEBUGGING) BY BOB
# **** LEEDOM - APRIL & DECEMBER 1974,
# **** WITH A LITTLE HELP FROM HIS FRIENDS . . .
#
# Python translation by Jack Boyce - 2/2021
# Output is identical to BASIC version except for minor cleanup


import random
from math import sqrt


# -------------------------------------------------------------------------
#  Utility functions
# -------------------------------------------------------------------------


def fnd(i):
    return sqrt((k[i][0] - s1)**2 + (k[i][1] - s2)**2)


def fnr():
    return int(random.random() * 7.98 + 0.01)


def quadrant_name(row, col, region_only=False):
    region1 = ['ANTARES', 'RIGEL', 'PROCYON', 'VEGA', 'CANOPUS', 'ALTAIR',
               'SAGITTARIUS', 'POLLUX']
    region2 = ['SIRIUS', 'DENEB', 'CAPELLA', 'BETELGEUSE', 'ALDEBARAN',
               'REGULUS', 'ARCTURUS', 'SPICA']
    modifier = ['I', 'II', 'III', 'IV']

    quadrant = region1[row] if col < 4 else region2[row]

    if not region_only:
        quadrant += ' ' + modifier[col % 4]

    return quadrant


def insert_marker(row, col, marker):
    global qs

    if len(marker) != 3:
        print('ERROR')
        quit()

    pos = round(col) * 3 + round(row) * 24
    qs = qs[0:pos] + marker + qs[(pos + 3):192]


def compare_marker(row, col, test_marker):
    global qs

    pos = round(col) * 3 + round(row) * 24
    return qs[pos:(pos + 3)] == test_marker


def find_empty_place():
    while True:
        row = fnr()
        col = fnr()
        if compare_marker(row, col, '   '):
            return row, col


# -------------------------------------------------------------------------
#  Functions for individual player commands
# -------------------------------------------------------------------------


def navigation():
    global d, s, e, k, s1, s2, qs, t8, t9, t, w1, c, q1, q2

    c1 = float(input("COURSE (1-9) "))
    if c1 == 9:
        c1 = 1
    if c1 < 1 or c1 >= 9:
        print("   LT. SULU REPORTS, 'INCORRECT COURSE DATA, SIR!'")
        return
    xs = '0.2' if d[0] < 0 else '8'
    w1 = float(input(f"WARP FACTOR (0-{xs}) "))
    if d[0] < 0 and w1 > 0.2:
        print("WARP ENGINES ARE DAMAGED. MAXIMUM SPEED = WARP 0.2")
        return
    if w1 == 0:
        return
    if w1 < 0 or w1 > 8:
        print(f"   CHIEF ENGINEER SCOTT REPORTS 'THE ENGINES WON'T TAKE WARP {w1}!'")
        return

    n = int(w1 * 8 + 0.5)
    if e < n:
        print("ENGINEERING REPORTS   'INSUFFICIENT ENERGY AVAILABLE")
        print(f"                       FOR MANEUVERING AT WARP {w1}!'")
        if s >= n - e and d[6] >= 0:
            print(f"DEFLECTOR CONTROL ROOM ACKNOWLEDGES {s} UNITS OF ENERGY")
            print("                         PRESENTLY DEPLOYED TO SHIELDS.")
        return

    # klingons move and fire
    for i in range(3):
        if k[i][2] != 0:
            insert_marker(k[i][0], k[i][1], '   ')
            k[i][0], k[i][1] = find_empty_place()
            insert_marker(k[i][0], k[i][1], '+K+')

    klingons_fire()

    # print damage control report
    d1 = 0
    line = ''
    d6 = 1 if w1 >= 1 else w1
    for i in range(8):
        if d[i] < 0:
            d[i] += d6
            if d[i] > -0.1 and d[i] < 0:
                d[i] = -0.1
            elif d[i] >= 0:
                if d1 != 1:
                    d1 = 1
                    line = "DAMAGE CONTROL REPORT:"
                line += '   ' + devices[i] + ' REPAIR COMPLETED\n'
    if len(line) > 0:
        print(line)
    if random.random() <= 0.2:
        r1 = fnr()
        if random.random() < 0.6:
            d[r1] -= random.random() * 5 + 1
            print(f'DAMAGE CONTROL REPORT:   {devices[r1]} DAMAGED\n')
        else:
            d[r1] += random.random() * 3 + 1
            print(f'DAMAGE CONTROL REPORT:   {devices[r1]} STATE OF REPAIR IMPROVED\n')

    # begin moving starship
    insert_marker(int(s1), int(s2), '   ')
    ic1 = int(c1)
    x1 = c[ic1 - 1][0] + (c[ic1][0] - c[ic1 - 1][0]) * (c1 - ic1)
    x, y = s1, s2
    x2 = c[ic1 - 1][1] + (c[ic1][1] - c[ic1 - 1][1]) * (c1 - ic1)
    q4, q5 = q1, q2
    for i in range(1, n + 1):
        s1 += x1
        s2 += x2

        if s1 < 0 or s1 > 7 or s2 < 0 or s2 > 7:
            # exceeded quadrant limits
            x += 8 * q1 + n * x1
            y += 8 * q2 + n * x2
            q1 = int(x / 8)
            q2 = int(y / 8)
            s1 = int(x - q1 * 8)
            s2 = int(y - q2 * 8)
            if s1 < 0:
                q1 -= 1
                s1 = 7
            if s2 < 0:
                q2 -= 1
                s2 = 7
            x5 = 0
            if q1 < 0:
                x5 = 1
                q1 = s1 = 0
            if q1 > 7:
                x5 = 1
                q1 = s1 = 7
            if q2 < 0:
                x5 = 1
                q2 = s2 = 0
            if q2 > 7:
                x5 = 1
                q2 = s2 = 7
            if x5 == 1:
                print("LT. UHURA REPORTS MESSAGE FROM STARFLEET COMMAND:")
                print("  'PERMISSION TO ATTEMPT CROSSING OF GALACTIC PERIMETER")
                print("  IS HEREBY *DENIED*. SHUT DOWN YOUR ENGINES.'")
                print("CHIEF ENGINEER SCOTT REPORTS  'WARP ENGINES SHUT DOWN")
                print(f"  AT SECTOR {s1 + 1},{s2 + 1} OF QUADRANT {q1 + 1},{q2 + 1}.'")
                if t > t0 + t9:
                    end_game()
            if 8 * q1 + q2 == 8 * q4 + q5:
                break
            t += 1
            maneuver_energy(n)
            new_quadrant()
            return
        else:
            s8 = int(s1) * 24 + int(s2) * 3
            if qs[s8:(s8 + 2)] != '  ':
                s1 = int(s1 - x1)
                s2 = int(s2 - x2)
                print(
                    "WARP ENGINES SHUT DOWN AT SECTOR "
                    f"{s1 + 1},{s2 + 1} DUE TO BAD NAVAGATION"
                )
                break
    else:
        s1, s2 = int(s1), int(s2)

    insert_marker(int(s1), int(s2), '<*>')
    maneuver_energy(n)

    t8 = 1
    if w1 < 1:
        t8 = 0.1 * int(10 * w1)
    t += t8
    if t > t0 + t9:
        end_game()

    short_range_scan()


def maneuver_energy(n):
    global e, s

    e -= n + 10
    if e > 0:
        return

    print("SHIELD CONTROL SUPPLIES ENERGY TO COMPLETE THE MANEUVER.")
    s += e
    e = 0
    if s <= 0:
        s = 0


def short_range_scan():
    global d0, e, p, s

    d0 = 0
    for i in (s1 - 1, s1, s1 + 1):
        for j in (s2 - 1, s2, s2 + 1):
            if 0 <= i <= 7 and 0 <= j <= 7:
                if compare_marker(i, j, '>!<'):
                    d0 = 1
                    cs = 'DOCKED'
                    e = e0
                    p = p0
                    print('SHIELDS DROPPED FOR DOCKING PURPOSES')
                    s = 0
                    break
        else:
            continue
        break
    else:
        if k3 > 0:
            cs = '*RED*'
        elif e < e0 * 0.1:
            cs = 'YELLOW'
        else:
            cs = 'GREEN'

    if d[1] < 0:
        print('\n*** SHORT RANGE SENSORS ARE OUT ***\n')
        return

    sep = '---------------------------------'
    print(sep)
    for i in range(8):
        line = ''
        for j in range(8):
            pos = i * 24 + j * 3
            line = line + ' ' + qs[pos:(pos + 3)]

        if i == 0:
            line += f'        STARDATE           {int(t * 10) * 0.1}'
        elif i == 1:
            line += f'        CONDITION          {cs}'
        elif i == 2:
            line += f'        QUADRANT           {q1 + 1},{q2 + 1}'
        elif i == 3:
            line += f'        SECTOR             {s1 + 1},{s2 + 1}'
        elif i == 4:
            line += f'        PHOTON TORPEDOES   {int(p)}'
        elif i == 5:
            line += f'        TOTAL ENERGY       {int(e + s)}'
        elif i == 6:
            line += f'        SHIELDS            {int(s)}'
        else:
            line += f'        KLINGONS REMAINING {k9}'

        print(line)
    print(sep)


def long_range_scan():
    global n, z

    if d[2] < 0:
        print('LONG RANGE SENSORS ARE INOPERABLE')
        return

    print(f'LONG RANGE SCAN FOR QUADRANT {q1 + 1},{q2 + 1}')
    o1s = '-------------------'
    print(o1s)
    for i in (q1 - 1, q1, q1 + 1):
        n[0] = -1
        n[1] = -2
        n[2] = -3

        for j in (q2 - 1, q2, q2 + 1):
            if 0 <= i <= 7 and 0 <= j <= 7:
                n[j - q2 + 1] = g[i][j]
                z[i][j] = g[i][j]

        line = ': '
        for l in range(3):
            if n[l] < 0:
                line += '*** : '
            else:
                line += str(n[l] + 1000).rjust(4, ' ')[-3:] + ' : '
        print(line)
        print(o1s)


def phaser_control():
    global d, e, k, k3, k9, qs, g, z, q1, q2

    if d[3] < 0:
        print("PHASERS INOPERATIVE")
        return

    if k3 <= 0:
        print("SCIENCE OFFICER SPOCK REPORTS  'SENSORS SHOW NO ENEMY SHIPS")
        print("                                IN THIS QUADRANT'")
        return

    if d[7] < 0:
        print("COMPUTER FAILURE HAMPERS ACCURACY")

    print(f"PHASERS LOCKED ON TARGET; ENERGY AVAILABLE = {e} UNITS")
    x = 0
    while True:
        x = int(input("NUMBER OF UNITS TO FIRE "))
        if x <= 0:
            return
        if e - x >= 0:
            break
        print(f"ENERGY AVAILABLE = {e} UNITS")

    e -= x
    if d[7] < 0:  # typo in original, was d[6]
        x *= random.random()

    h1 = int(x / k3)
    for i in range(3):
        if k[i][2] <= 0:
            continue

        h = int((h1 / fnd(i)) * (random.random() + 2))
        if h <= 0.15 * k[i][2]:
            print(f"SENSORS SHOW NO DAMAGE TO ENEMY AT {k[i][0] + 1},{k[i][1] + 1}")
        else:
            k[i][2] -= h
            print(f"UNIT HIT ON KLINGON AT SECTOR {k[i][0] + 1},{k[i][1] + 1}")
            if k[i][2] <= 0:
                print("*** KLINGON DESTROYED ***")
                k3 -= 1
                k9 -= 1
                insert_marker(k[i][0], k[i][1], '   ')
                k[i][2] = 0
                g[q1][q2] -= 100
                z[q1][q2] = g[q1][q2]
                if k9 <= 0:
                    end_game(won=True)
            else:
                print(f"   (SENSORS SHOW {k[i][2]} UNITS REMAINING)")

    klingons_fire()


def photon_torpedoes():
    global p, d, c, e, s1, s2, k3, k9, b3, b9, t, t0, t9, d0, g, z

    if p <= 0:
        print("ALL PHOTON TORPEDOES EXPENDED")
        return
    if d[4] < 0:
        print("PHOTON TUBES ARE NOT OPERATIONAL")
        return

    c1 = float(input("PHOTON TORPEDO COURSE (1-9) "))
    if c1 == 9:
        c1 = 1
    if c1 < 1 or c1 >= 9:
        print("ENSIGN CHEKOV REPORTS, 'INCORRECT COURSE DATA, SIR!'")
        return

    ic1 = int(c1)
    x1 = c[ic1 - 1][0] + (c[ic1][0] - c[ic1 - 1][0]) * (c1 - ic1)
    e -= 2
    p -= 1
    x2 = c[ic1 - 1][1] + (c[ic1][1] - c[ic1 - 1][1]) * (c1 - ic1)
    x, y = s1, s2
    x3, y3 = x, y
    print("TORPEDO TRACK:")
    while True:
        x += x1
        y += x2
        x3 = round(x)
        y3 = round(y)
        if x3 < 0 or x3 > 7 or y3 < 0 or y3 > 7:
            print("TORPEDO MISSED")
            klingons_fire()
            return
        print(f"               {x3 + 1},{y3 + 1}")
        if not compare_marker(x3, y3, '   '):
            break

    if compare_marker(x3, y3, '+K+'):
        print("*** KLINGON DESTROYED ***")
        k3 -= 1
        k9 -= 1
        if k9 <= 0:
            end_game(won=True)
        for i in range(3):
            if x3 == k[i][0] and y3 == k[i][1]:
                k[i][2] = 0
    elif compare_marker(x3, y3, ' * '):
        print(f"STAR AT {x3 + 1},{y3 + 1} ABSORBED TORPEDO ENERGY.")
        klingons_fire()
        return
    elif compare_marker(x3, y3, '>!<'):
        print("*** STARBASE DESTROYED ***")
        b3 -= 1
        b9 -= 1
        if b9 == 0 and k9 <= t - t0 - t9:
            print("THAT DOES IT, CAPTAIN!! YOU ARE HEREBY RELIEVED OF COMMAND")
            print("AND SENTENCED TO 99 STARDATES AT HARD LABOR ON CYGNUS 12!!")
            end_game(quit=True)
        else:
            print("STARFLEET COMMAND REVIEWING YOUR RECORD TO CONSIDER")
            print("COURT MARTIAL!")
            d0 = 0

    insert_marker(x3, y3, '   ')
    g[q1][q2] = k3 * 100 + b3 * 10 + s3
    z[q1][q2] = g[q1][q2]
    klingons_fire()


def klingons_fire():
    global k3, s, k, d0, d

    if k3 <= 0:
        return
    if d0 != 0:
        print("STARBASE SHIELDS PROTECT THE ENTERPRISE")
        return

    for i in range(3):
        if k[i][2] <= 0:
            continue

        h = int((k[i][2] / fnd(i)) * (random.random() + 2))
        s -= h
        k[i][2] /= (random.random() + 3)
        print(f"{h} UNIT HIT ON ENTERPRISE FROM SECTOR {k[i][0] + 1},{k[i][1] + 1}")
        if s <= 0:
            end_game(enterprise_killed=True)
        print(f"      <SHIELDS DOWN TO {s} UNITS>")
        if h >= 20 and random.random() < 0.60 and h / s > 0.02:
            r1 = fnr()
            d[r1] -= h / s + 0.5 * random.random()
            print(f"DAMAGE CONTROL REPORTS {devices[r1]} DAMAGED BY THE HIT'")


def shield_control():
    global e, s, d

    if d[6] < 0:
        print('SHIELD CONTROL INOPERABLE')
        return

    x = input(f'ENERGY AVAILABLE = {e + s} NUMBER OF UNITS TO SHIELDS ')
    x = int(x)

    if x < 0 or s == x:
        print('<SHIELDS UNCHANGED>')
        return

    if x > e + s:
        print(
            "SHIELD CONTROL REPORTS  'THIS IS NOT THE FEDERATION TREASURY.'\n"
            "<SHIELDS UNCHANGED>"
        )
        return

    e += s - x
    s = x
    print('DEFLECTOR CONTROL ROOM REPORT:')
    print(f"  'SHIELDS NOW AT {s} UNITS PER YOUR COMMAND.'")


def damage_control():
    global d, d0, d4, t

    if d[5] < 0:
        print('DAMAGE CONTROL REPORT NOT AVAILABLE')
    else:
        print('\nDEVICE             STATE OF REPAIR')
        for r1 in range(8):
            print(f"{devices[r1].ljust(25, ' ')}{int(d[r1] * 100) * 0.01}")
        print()

    if d0 == 0:
        return

    d3 = sum(0.1 for i in range(8) if d[i] < 0)
    if d3 == 0:
        return

    d3 += d4
    if d3 >= 1:
        d3 = 0.9
    print("\nTECHNICIANS STANDING BY TO EFFECT REPAIRS TO YOUR SHIP;")
    print(f"ESTIMATED TIME TO REPAIR: {0.01 * int(100 * d3)} STARDATES")
    astr = input("WILL YOU AUTHORIZE THE REPAIR ORDER (Y/N) ").upper()
    if astr != 'Y':
        return

    for i in range(8):
        if d[i] < 0:
            d[i] = 0
    t += d3 + 0.1


def computer():
    global d, g5, z, k9, t0, t9, t, b9, s1, s2, b4, b5

    if d[7] < 0:
        print("COMPUTER DISABLED")
        return

    while True:
        coms = input("COMPUTER ACTIVE AND AWAITING COMMAND ")
        if len(coms) == 0:
            com = 6
        else:
            com = int(coms)
        if com < 0:
            return

        print()
        h8 = 1

        if com == 0 or com == 5:
            if com == 5:
                h8 = 0
                g5 = 1
                print("                        THE GALAXY")
            else:
                # hardcopy disabled:
                # 7540 REM INPUT"DO YOU WANT A HARDCOPY? IS THE TTY ON (Y/N)";A$
                # 7542 REM IFA$="Y"THENPOKE1229,2:POKE1237,3:NULL1

                print("\n        ");
                print(f"COMPUTER RECORD OF GALAXY FOR QUADRANT {q1 + 1},{q2 + 1}")
                print()

            print("       1     2     3     4     5     6     7     8")
            sep = "     ----- ----- ----- ----- ----- ----- ----- -----"
            print(sep)

            for i in range(8):
                line = str(i + 1) + '  '

                if h8 == 0:
                    g2s = quadrant_name(i, 0, True)
                    line += (' ' * int(14 - 0.5 * len(g2s))) + g2s
                    g2s = quadrant_name(i, 4, True)
                    line += (' ' * int(41 - 0.5 * len(g2s) - len(line))) + g2s
                else:
                    for j in range(8):
                        line += '   '
                        if z[i][j] == 0:
                            line += '***'
                        else:
                            line += str(z[i][j] + 1000)[-3:]

                print(line)
                print(sep)

            return
        elif com == 1:
            print("   STATUS REPORT:")
            print(f"KLINGON{'S' if k9 > 1 else ''} LEFT: {k9}")
            print(f"MISSION MUST BE COMPLETED IN {0.1 * int((t0+t9-t) * 10)} STARDATES")

            if b9 == 0:
                print("YOUR STUPIDITY HAS LEFT YOU ON YOUR ON IN")
                print("  THE GALAXY -- YOU HAVE NO STARBASES LEFT!")
            else:
                print(f"THE FEDERATION IS MAINTAINING {b9} STARBASE{'S' if b9 > 1 else ''} IN THE GALAXY")

            damage_control()
            return
        elif com == 2:
            if k3 <= 0:
                print("SCIENCE OFFICER SPOCK REPORTS  'SENSORS SHOW NO ENEMY SHIPS")
                print("                                IN THIS QUADRANT'")
                return

            print(f"FROM ENTERPRISE TO KLINGON BATTLE CRUSER{'S' if k3 > 1 else ''}")
            h8 = 0
            for i in range(3):
                if k[i][2] > 0:
                    print_direction(s1, s2, k[i][0], k[i][1])
            return
        elif com == 3:
            if b3 == 0:
                print("MR. SPOCK REPORTS, 'SENSORS SHOW NO STARBASES IN THIS QUADRANT.'")
                return

            print("FROM ENTERPRISE TO STARBASE:")
            print_direction(s1, s2, b4, b5)
            return
        elif com == 4:
            print("DIRECTION/DISTANCE CALCULATOR:")
            print(f"YOU ARE AT QUADRANT {q1},{q2} SECTOR {s1},{s2}")
            print("PLEASE ENTER")
            while True:
                ins = input("  INITIAL COORDINATES (X,Y)").split(',')
                if len(ins) == 2:
                    from1, from2 = int(ins[0]), int(ins[1])
                    break
            while True:
                ins = input("  FINAL COORDINATES (X,Y)").split(',')
                if len(ins) == 2:
                    to1, to2 = int(ins[0]), int(ins[1])
                    break
            print_direction(from1, from2, to1, to2)
            return
        else:
            print(
                "FUNCTIONS AVAILABLE FROM LIBRARY-COMPUTER:\n"
                "   0 = CUMULATIVE GALACTIC RECORD\n"
                "   1 = STATUS REPORT\n"
                "   2 = PHOTON TORPEDO DATA\n"
                "   3 = STARBASE NAV DATA\n"
                "   4 = DIRECTION/DISTANCE CALCULATOR\n"
                "   5 = GALAXY 'REGION NAME' MAP\n"
            )


def print_direction(from1, from2, to1, to2):
    to2 -= from2
    from2 = from1 - to1

    def f1(c1, a, x):
        if abs(a) >= abs(x):
            print(f"DIRECTION = {c1 + (abs(x) / abs(a))}")
        else:
            print(f"DIRECTION = {c1 + (((abs(x) - abs(a)) + abs(x)) / abs(x))}")

    def f2(c1, a, x):
        if abs(a) <= abs(x):
            print(f"DIRECTION = {c1 + (abs(a) / abs(x))}")
        else:
            print(f"DIRECTION = {c1 + (((abs(a) - abs(x)) + abs(a)) / abs(a))}")

    if to2 < 0:
        if from2 > 0:
            f1(3, from2, to2)
        elif to2 != 0:
            f2(5, from2, to2)
        else:
            f1(7, from2, to2)
    else:
        if from2 < 0:
            f1(7, from2, to2)
        elif to2 > 0:
            f2(1, from2, to2)
        elif from2 == 0:
            f2(5, from2, to2)
        else:
            f2(1, from2, to2)

    print(f"DISTANCE = {sqrt(to2 ** 2 + from2 ** 2)}")


# -------------------------------------------------------------------------
#  Game transitions
# -------------------------------------------------------------------------


def startup():
    global g, c, k, n, z, d, t, t0, t9, d0, e, e0, p, p0, s9
    global s, b9, k7, k9, devices
    global q1, q2, s1, s2

    print(
        "\n\n\n\n\n\n\n\n\n\n\n"
        "                                    ,------*------,\n"
        "                    ,-------------   '---  ------'\n"
        "                     '-------- --'      / /\n"
        "                         ,---' '-------/ /--,\n"
        "                          '----------------'\n\n"
        "                    THE USS ENTERPRISE --- NCC-1701\n"
        "\n\n\n\n\n"
    )

    # set up global game variables
    g = [[0] * 8 for _ in range(8)]
    c = [[0, 1], [-1, 1], [-1, 0], [-1, -1], [0, -1], [1, -1], [1, 0], [1, 1],
         [0, 1]]
    k = [[0, 0, 0], [0, 0, 0], [0, 0, 0]]
    n = [0, 0, 0]
    z = [[0] * 8 for _ in range(8)]
    d = [0] * 8
    t = 100 * random.randint(20, 39)
    t0 = t
    t9 = random.randint(25, 34)
    d0 = 0
    e = 3000
    e0 = e
    p = 10
    p0 = p
    s9 = 200
    s = 0
    b9 = 2
    k9 = 0
    devices = ['WARP ENGINES', 'SHORT RANGE SENSORS', 'LONG RANGE SENSORS',
               'PHASER CONTROL', 'PHOTON TUBES', 'DAMAGE CONTROL',
               'SHIELD CONTROL', 'LIBRARY-COMPUTER']

    # initialize Enterprise's position
    q1 = fnr()
    q2 = fnr()
    s1 = fnr()
    s2 = fnr()

    # initialize contents of galaxy
    for i in range(8):
        for j in range(8):
            k3 = 0
            r1 = random.random()

            if r1 > 0.98:
                k3 = 3
            elif r1 > 0.95:
                k3 = 2
            elif r1 > 0.80:
                k3 = 1
            k9 += k3

            b3 = 0
            if random.random() > 0.96:
                b3 = 1
                b9 += 1
            g[i][j] = k3 * 100 + b3 * 10 + fnr() + 1

    if k9 > t9:
        t9 = k9 + 1
    if b9 == 0:
        if g[q1][q2] < 200:
            g[q1][q2] += 120
            k9 += 1
        b9 = 1
        g[q1][q2] += 10
        q1 = fnr()
        q2 = fnr()

    k7 = k9
    print(
        "YOUR ORDERS ARE AS FOLLOWS:\n"
        f"   DESTROY THE {k9} KLINGON WARSHIPS WHICH HAVE INVADED\n"
        "   THE GALAXY BEFORE THEY CAN ATTACK FEDERATION HEADQUARTERS\n"
        f"   ON STARDATE {t0+t9}. THIS GIVES YOU {t9} DAYS. THERE {'IS' if b9 == 1 else 'ARE'}\n"
        f"   {b9} STARBASE{'' if b9 == 1 else 'S'} IN THE GALAXY FOR RESUPPLYING YOUR SHIP.\n"
    )


def new_quadrant():
    global z4, z5, k3, b3, s3, g5, d4, qs, b4, b5

    z4 = q1
    z5 = q2
    k3 = 0
    b3 = 0
    s3 = 0
    g5 = 0
    d4 = 0.5 * random.random()
    z[q1][q2] = g[q1][q2]

    if 0 <= q1 < 8 and 0 <= q2 < 8:
        g2s = quadrant_name(z4, z5, False)
        if t == t0:
            print('\nYOUR MISSION BEGINS WITH YOUR STARSHIP LOCATED')
            print(f"IN THE GALACTIC QUADRANT, '{g2s}'.\n")
        else:
            print(f'\nNOW ENTERING {g2s} QUADRANT . . .\n')

        k3 = int(g[q1][q2] * 0.01)
        b3 = int(g[q1][q2] * 0.1) - 10 * k3
        s3 = g[q1][q2] - 100 * k3 - 10 * b3
        if k3 != 0:
            print('COMBAT AREA      CONDITION RED')
            if s <= 200:
                print('   SHIELDS DANGEROUSLY LOW')

        for i in range(3):
            k[i][0] = k[i][1] = 0

    for i in range(3):
        k[i][2] = 0

    qs = ' ' * 192

    z1 = s1
    z2 = s2
    insert_marker(z1, z2, '<*>')
    if k3 > 0:
        for i in range(k3):
            r1, r2 = find_empty_place()
            insert_marker(r1, r2, '+K+')
            k[i] = [r1, r2, s9 * (0.5 + random.random())]
    if b3 > 0:
        r1, r2 = find_empty_place()
        insert_marker(r1, r2, '>!<')
        b4, b5 = r1, r2
    for i in range(s3):
        r1, r2 = find_empty_place()
        insert_marker(r1, r2, ' * ')

    short_range_scan()


def end_game(won=False, quit=False, enterprise_killed=False):
    global k7, t, t0

    if won:
        print("CONGRULATION, CAPTAIN! THE LAST KLINGON BATTLE CRUISER")
        print("MENACING THE FEDERATION HAS BEEN DESTROYED.\n")
        print(f"YOUR EFFICIENCY RATING IS {1000 * (k7 / (t - t0))**2}")
    else:
        if not quit:
            if enterprise_killed:
                print('\nTHE ENTERPRISE HAS BEEN DESTROYED. THEN FEDERATION WILL BE CONQUERED')

            print(f'IT IS STARDATE {t}')

        print(f'THERE WERE {k9} KLINGON BATTLE CRUISERS LEFT AT')
        print(f'THE END OF YOUR MISSION.\n\n')

        if b9 == 0:
            exit()

    print('THE FEDERATION IS IN NEED OF A NEW STARSHIP COMMANDER')
    print('FOR A SIMILAR MISSION -- IF THERE IS A VOLUNTEER,')
    astr = input("LET HIM STEP FORWARD AND ENTER 'AYE' ")
    if astr == 'AYE' or astr == 'aye':
        return
    exit()


# -------------------------------------------------------------------------
#  Game loop
# -------------------------------------------------------------------------


while True:
    startup()
    new_quadrant()

    restart = False

    while not restart:
        if s + e <= 10 or (e <= 10 and d[6] != 0):
            print("\n** FATAL ERROR **   YOU'VE JUST STRANDED YOUR SHIP IN SPACE")
            print("YOU HAVE INSUFFICIENT MANEUVERING ENERGY, AND SHIELD CONTROL")
            print("IS PRESENTLY INCAPABLE OF CROSS-CIRCUITING TO ENGINE ROOM!!")

        command = input('COMMAND ').upper()

        if command == 'NAV':
            navigation()
        elif command == 'SRS':
            short_range_scan()
        elif command == 'LRS':
            long_range_scan()
        elif command == 'PHA':
            phaser_control()
        elif command == 'TOR':
            photon_torpedoes()
        elif command == 'SHE':
            shield_control()
        elif command == 'DAM':
            damage_control()
        elif command == 'COM':
            computer()
        elif command == 'XXX':
            end_game(quit=True)  # only returns on restart
            restart = True
        else:
            print(
                'ENTER ONE OF THE FOLLOWING:\n'
                '  NAV  (TO SET COURSE)\n'
                '  SRS  (FOR SHORT RANGE SENSOR SCAN)\n'
                '  LRS  (FOR LONG RANGE SENSOR SCAN)\n'
                '  PHA  (TO FIRE PHASERS)\n'
                '  TOR  (TO FIRE PHOTON TORPEDOES)\n'
                '  SHE  (TO RAISE OR LOWER SHIELDS)\n'
                '  DAM  (FOR DAMAGE CONTROL REPORTS)\n'
                '  COM  (TO CALL ON LIBRARY-COMPUTER)\n'
                '  XXX  (TO RESIGN YOUR COMMAND)\n'
            )
