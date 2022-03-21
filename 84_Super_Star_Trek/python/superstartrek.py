# ****        **** STAR TREK ****        ****
# **** SIMULATION OF A MISSION OF THE STARSHIP ENTERPRISE,
# **** AS SEEN ON THE STAR TREK TV SHOW.
# **** ORIGINAL PROGRAM BY MIKE MAYFIELD, MODIFIED VERSION
# **** PUBLISHED IN DEC'S "101 BASIC GAMES", BY DAVE AHL.
# **** MODIFICATIONS TO THE LATTER (PLUS DEBUGGING) BY BOB
# **** LEEDOM - APRIL & DECEMBER 1974,
# **** WITH A LITTLE HELP FROM HIS FRIENDS . . .
#
# Python translation by Jack Boyce - February 2021
#   Output is identical to BASIC version except for a few
#   fixes (as noted, search `bug`) and minor cleanup.


import random
from math import sqrt
from typing import Any, Callable, Dict, List, Tuple

# Global variables
restart = False
s = 0
e = 0
d: List[int] = []
k: List[List[float]] = [[0, 0, 0], [0, 0, 0], [0, 0, 0]]  # Klingons in current quadrant
devices = [
    "WARP ENGINES",
    "SHORT RANGE SENSORS",
    "LONG RANGE SENSORS",
    "PHASER CONTROL",
    "PHOTON TUBES",
    "DAMAGE CONTROL",
    "SHIELD CONTROL",
    "LIBRARY-COMPUTER",
]
c = [
    [0, 1],
    [-1, 1],
    [-1, 0],
    [-1, -1],
    [0, -1],
    [1, -1],
    [1, 0],
    [1, 1],
    [0, 1],
]  # vectors in cardinal directions
q1 = s1 = 0
q2 = s2 = 0
k3 = b3 = s3 = 0  # Klingons, bases, stars in quad.

b4 = b5 = 0
qs = " " * 192  # quadrant string
# set up global game variables
g = [[0] * 8 for _ in range(8)]  # galaxy map
z = [[0] * 8 for _ in range(8)]  # charted galaxy map
d = [0] * 8  # damage stats for devices
t = t0 = 100 * random.randint(20, 39)  # stardate (current, initial)
t9 = random.randint(25, 34)  # mission duration (stardates)
docked = False  # true when docked at starbase
e = e0 = 3000  # energy (current, initial)
p = p0 = 10  # torpedoes (current, initial)
s = 0  # shields
k9, b9 = 0, 0  # total Klingons, bases in galaxy
# ^ bug in original, was b9 = 2
s9 = 200  # avg. Klingon shield strength

k7 = k9  # Klingons at start of game
d4 = 0.5 * random.random()  # extra delay in repairs at base

# -------------------------------------------------------------------------
#  Utility functions
# -------------------------------------------------------------------------


def fnr():
    # Generate a random integer from 0 to 7 inclusive.
    return random.randint(0, 7)


def quadrant_name(row, col, region_only=False):
    # Return quadrant name visible on scans, etc.
    region1 = [
        "ANTARES",
        "RIGEL",
        "PROCYON",
        "VEGA",
        "CANOPUS",
        "ALTAIR",
        "SAGITTARIUS",
        "POLLUX",
    ]
    region2 = [
        "SIRIUS",
        "DENEB",
        "CAPELLA",
        "BETELGEUSE",
        "ALDEBARAN",
        "REGULUS",
        "ARCTURUS",
        "SPICA",
    ]
    modifier = ["I", "II", "III", "IV"]

    quadrant = region1[row] if col < 4 else region2[row]

    if not region_only:
        quadrant += " " + modifier[col % 4]

    return quadrant


def insert_marker(row, col, marker):
    # Insert a marker into a given position in the quadrant string `qs`. The
    # contents of a quadrant (Enterprise, stars, etc.) are stored in `qs`.
    global qs

    if len(marker) != 3:
        print("ERROR")
        exit()

    pos = round(col) * 3 + round(row) * 24
    qs = qs[0:pos] + marker + qs[(pos + 3) : 192]


def compare_marker(row, col, test_marker):
    # Check whether the position in the current quadrant is occupied with a
    # given marker.
    pos = round(col) * 3 + round(row) * 24
    return qs[pos : (pos + 3)] == test_marker


def find_empty_place() -> Tuple[int, int]:
    # Find an empty location in the current quadrant.
    while True:
        row, col = fnr(), fnr()
        if compare_marker(row, col, "   "):
            return row, col


# -------------------------------------------------------------------------
#  Functions for individual player commands
# -------------------------------------------------------------------------


def navigation() -> None:
    # Take navigation input and move the Enterprise.
    global d, s, e, k, qs, t, q1, q2, s1, s2

    while True:
        c1s = input("COURSE (1-9)? ")
        if len(c1s) > 0:
            c1 = float(c1s)
            break
    if c1 == 9:
        c1 = 1
    if c1 < 1 or c1 >= 9:
        print("   LT. SULU REPORTS, 'INCORRECT COURSE DATA, SIR!'")
        return

    while True:
        warps = input(f"WARP FACTOR (0-{'0.2' if d[0] < 0 else '8'})? ")
        if len(warps) > 0:
            warp = float(warps)
            break
    if d[0] < 0 and warp > 0.2:
        print("WARP ENGINES ARE DAMAGED. MAXIMUM SPEED = WARP 0.2")
        return
    if warp == 0:
        return
    if warp < 0 or warp > 8:
        print(f"   CHIEF ENGINEER SCOTT REPORTS 'THE ENGINES WON'T TAKE WARP {warp}!'")
        return

    n = round(warp * 8)
    if e < n:
        print("ENGINEERING REPORTS   'INSUFFICIENT ENERGY AVAILABLE")
        print(f"                       FOR MANEUVERING AT WARP {warp}!'")
        if s >= n - e and d[6] >= 0:
            print(f"DEFLECTOR CONTROL ROOM ACKNOWLEDGES {s} UNITS OF ENERGY")
            print("                         PRESENTLY DEPLOYED TO SHIELDS.")
        return

    # klingons move and fire
    for i in range(3):
        if k[i][2] != 0:
            insert_marker(k[i][0], k[i][1], "   ")
            k[i][0], k[i][1] = find_empty_place()
            insert_marker(k[i][0], k[i][1], "+K+")

    klingons_fire()

    # repair damaged devices and print damage report
    line = ""
    for i in range(8):
        if d[i] < 0:
            d[i] += min(warp, 1)  # type: ignore
            if -0.1 < d[i] < 0:
                d[i] = -0.1  # type: ignore
            elif d[i] >= 0:
                if len(line) == 0:
                    line = "DAMAGE CONTROL REPORT:"
                line += "   " + devices[i] + " REPAIR COMPLETED\n"
    if len(line) > 0:
        print(line)
    if random.random() <= 0.2:
        r1 = fnr()
        if random.random() < 0.6:
            d[r1] -= random.random() * 5 + 1
            print(f"DAMAGE CONTROL REPORT:   {devices[r1]} DAMAGED\n")
        else:
            d[r1] += random.random() * 3 + 1
            print(f"DAMAGE CONTROL REPORT:   {devices[r1]} STATE OF REPAIR IMPROVED\n")

    # begin moving starship
    insert_marker(int(s1), int(s2), "   ")
    ic1 = int(c1)
    x1 = c[ic1 - 1][0] + (c[ic1][0] - c[ic1 - 1][0]) * (c1 - ic1)
    x2 = c[ic1 - 1][1] + (c[ic1][1] - c[ic1 - 1][1]) * (c1 - ic1)
    q1_start, q2_start = q1, q2
    x, y = s1, s2

    for _ in range(n):
        s1 += x1  # type: ignore
        s2 += x2  # type: ignore

        if s1 < 0 or s1 > 7 or s2 < 0 or s2 > 7:
            # exceeded quadrant limits; calculate final position
            x += 8 * q1 + n * x1  # type: ignore
            y += 8 * q2 + n * x2  # type: ignore
            q1, q2 = int(x / 8), int(y / 8)
            s1, s2 = int(x - q1 * 8), int(y - q2 * 8)
            if s1 < 0:
                q1 -= 1
                s1 = 7
            if s2 < 0:
                q2 -= 1
                s2 = 7

            hit_edge = False
            if q1 < 0:
                hit_edge = True
                q1 = s1 = 0
            if q1 > 7:
                hit_edge = True
                q1 = s1 = 7
            if q2 < 0:
                hit_edge = True
                q2 = s2 = 0
            if q2 > 7:
                hit_edge = True
                q2 = s2 = 7
            if hit_edge:
                print("LT. UHURA REPORTS MESSAGE FROM STARFLEET COMMAND:")
                print("  'PERMISSION TO ATTEMPT CROSSING OF GALACTIC PERIMETER")
                print("  IS HEREBY *DENIED*. SHUT DOWN YOUR ENGINES.'")
                print("CHIEF ENGINEER SCOTT REPORTS  'WARP ENGINES SHUT DOWN")
                print(
                    f"  AT SECTOR {s1 + 1} , {s2 + 1} OF QUADRANT "
                    f"{q1 + 1} , {q2 + 1}.'"
                )
                if t > t0 + t9:
                    end_game(won=False, quit=False)
                    return

            if q1 == q1_start and q2 == q2_start:
                break
            t += 1
            maneuver_energy(n)
            new_quadrant()
            return
        else:
            pos = int(s1) * 24 + int(s2) * 3
            if qs[pos : (pos + 2)] != "  ":
                s1, s2 = int(s1 - x1), int(s2 - x2)
                print(
                    "WARP ENGINES SHUT DOWN AT SECTOR "
                    f"{s1 + 1} , {s2 + 1} DUE TO BAD NAVAGATION"
                )
                break
    else:
        s1, s2 = int(s1), int(s2)

    insert_marker(int(s1), int(s2), "<*>")
    maneuver_energy(n)

    t += 0.1 * int(10 * warp) if warp < 1 else 1  # type: ignore
    if t > t0 + t9:
        end_game(won=False, quit=False)
        return

    short_range_scan()


def maneuver_energy(n):
    # Deduct the energy for navigation from energy/shields.
    global e, s

    e -= n + 10

    if e <= 0:
        print("SHIELD CONTROL SUPPLIES ENERGY TO COMPLETE THE MANEUVER.")
        s += e
        e = 0
        s = max(0, s)


def short_range_scan() -> None:
    # Print a short range scan.
    global docked, e, p, s

    docked = False
    for i in (s1 - 1, s1, s1 + 1):
        for j in (s2 - 1, s2, s2 + 1):
            if 0 <= i <= 7 and 0 <= j <= 7 and compare_marker(i, j, ">!<"):
                docked = True
                cs = "DOCKED"
                e, p = e0, p0
                print("SHIELDS DROPPED FOR DOCKING PURPOSES")
                s = 0
                break
        else:
            continue
        break
    else:
        if k3 > 0:
            cs = "*RED*"
        elif e < e0 * 0.1:
            cs = "YELLOW"
        else:
            cs = "GREEN"

    if d[1] < 0:
        print("\n*** SHORT RANGE SENSORS ARE OUT ***\n")
        return

    sep = "---------------------------------"
    print(sep)
    for i in range(8):
        line = ""
        for j in range(8):
            pos = i * 24 + j * 3
            line = line + " " + qs[pos : (pos + 3)]

        if i == 0:
            line += f"        STARDATE           {round(int(t * 10) * 0.1, 1)}"
        elif i == 1:
            line += f"        CONDITION          {cs}"
        elif i == 2:
            line += f"        QUADRANT           {q1 + 1} , {q2 + 1}"
        elif i == 3:
            line += f"        SECTOR             {s1 + 1} , {s2 + 1}"
        elif i == 4:
            line += f"        PHOTON TORPEDOES   {int(p)}"
        elif i == 5:
            line += f"        TOTAL ENERGY       {int(e + s)}"
        elif i == 6:
            line += f"        SHIELDS            {int(s)}"
        else:
            line += f"        KLINGONS REMAINING {k9}"

        print(line)
    print(sep)


def long_range_scan() -> None:
    # Print a long range scan.
    global z, g

    if d[2] < 0:
        print("LONG RANGE SENSORS ARE INOPERABLE")
        return

    print(f"LONG RANGE SCAN FOR QUADRANT {q1 + 1} , {q2 + 1}")
    print_scan_results(q1, q2, g, z)


def print_scan_results(
    q1: int, q2: int, g: List[List[Any]], z: List[List[Any]]
) -> None:
    sep = "-------------------"
    print(sep)
    for i in (q1 - 1, q1, q1 + 1):
        n = [-1, -2, -3]

        for j in (q2 - 1, q2, q2 + 1):
            if 0 <= i <= 7 and 0 <= j <= 7:
                n[j - q2 + 1] = g[i][j]
                z[i][j] = g[i][j]

        line = ": "
        for line_index in range(3):
            if n[line_index] < 0:
                line += "*** : "
            else:
                line += str(n[line_index] + 1000).rjust(4, " ")[-3:] + " : "
        print(line)
        print(sep)


def phaser_control() -> None:
    # Take phaser control input and fire phasers.
    global e, k, g, z, k3, k9

    if d[3] < 0:
        print("PHASERS INOPERATIVE")
        return

    if k3 <= 0:
        print("SCIENCE OFFICER SPOCK REPORTS  'SENSORS SHOW NO ENEMY SHIPS")
        print("                                IN THIS QUADRANT'")
        return

    if d[7] < 0:
        print("COMPUTER FAILURE HAMPERS ACCURACY")

    print(f"PHASERS LOCKED ON TARGET;  ENERGY AVAILABLE = {e} UNITS")
    x = 0
    while True:
        while True:
            units_to_fire = input("NUMBER OF UNITS TO FIRE? ")
            if len(units_to_fire) > 0:
                x = int(units_to_fire)
                break
        if x <= 0:
            return
        if e >= x:
            break
        print(f"ENERGY AVAILABLE = {e} UNITS")

    e -= x
    if d[7] < 0:  # bug in original, was d[6]
        x *= random.random()  # type: ignore

    h1 = int(x / k3)
    for i in range(3):
        if k[i][2] <= 0:
            continue

        h = int((h1 / fnd(i)) * (random.random() + 2))
        if h <= 0.15 * k[i][2]:
            print(f"SENSORS SHOW NO DAMAGE TO ENEMY AT {k[i][0] + 1} , {k[i][1] + 1}")
        else:
            k[i][2] -= h
            print(f" {h} UNIT HIT ON KLINGON AT SECTOR {k[i][0] + 1} , {k[i][1] + 1}")
            if k[i][2] <= 0:
                print("*** KLINGON DESTROYED ***")
                k3 -= 1
                k9 -= 1
                insert_marker(k[i][0], k[i][1], "   ")
                k[i][2] = 0
                g[q1][q2] -= 100
                z[q1][q2] = g[q1][q2]
                if k9 <= 0:
                    end_game(won=True, quit=False)
                    return
            else:
                print(f"   (SENSORS SHOW {round(k[i][2],6)} UNITS REMAINING)")

    klingons_fire()


def photon_torpedoes() -> None:
    # Take photon torpedo input and process firing of torpedoes.
    global e, p, k3, k9, k, b3, b9, docked, g, z

    if p <= 0:
        print("ALL PHOTON TORPEDOES EXPENDED")
        return
    if d[4] < 0:
        print("PHOTON TUBES ARE NOT OPERATIONAL")
        return

    while True:
        torpedo_course = input("PHOTON TORPEDO COURSE (1-9)? ")
        if len(torpedo_course) > 0:
            c1 = float(torpedo_course)
            break
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
        x += x1  # type: ignore
        y += x2  # type: ignore
        x3, y3 = round(x), round(y)
        if x3 < 0 or x3 > 7 or y3 < 0 or y3 > 7:
            print("TORPEDO MISSED")
            klingons_fire()
            return
        print(f"                {x3 + 1} , {y3 + 1}")
        if not compare_marker(x3, y3, "   "):
            break

    if compare_marker(x3, y3, "+K+"):
        print("*** KLINGON DESTROYED ***")
        k3 -= 1
        k9 -= 1
        if k9 <= 0:
            end_game(won=True, quit=False)
            return
        for i in range(3):
            if x3 == k[i][0] and y3 == k[i][1]:
                k[i][2] = 0
    elif compare_marker(x3, y3, " * "):
        print(f"STAR AT {x3 + 1} , {y3 + 1} ABSORBED TORPEDO ENERGY.")
        klingons_fire()
        return
    elif compare_marker(x3, y3, ">!<"):
        print("*** STARBASE DESTROYED ***")
        b3 -= 1
        b9 -= 1
        if b9 == 0 and k9 <= t - t0 - t9:
            print("THAT DOES IT, CAPTAIN!! YOU ARE HEREBY RELIEVED OF COMMAND")
            print("AND SENTENCED TO 99 STARDATES AT HARD LABOR ON CYGNUS 12!!")
            end_game(won=False)
            return
        else:
            print("STARFLEET COMMAND REVIEWING YOUR RECORD TO CONSIDER")
            print("COURT MARTIAL!")
            docked = False

    insert_marker(x3, y3, "   ")
    g[q1][q2] = k3 * 100 + b3 * 10 + s3
    z[q1][q2] = g[q1][q2]
    klingons_fire()


def fnd(i):
    # Find distance between Enterprise and i'th Klingon warship.
    return sqrt((k[i][0] - s1) ** 2 + (k[i][1] - s2) ** 2)


def klingons_fire():
    # Process nearby Klingons firing on Enterprise.
    global s, k, d

    if k3 <= 0:
        return
    if docked:
        print("STARBASE SHIELDS PROTECT THE ENTERPRISE")
        return

    for i in range(3):
        if k[i][2] <= 0:
            continue

        h = int((k[i][2] / fnd(i)) * (random.random() + 2))
        s -= h
        k[i][2] /= random.random() + 3
        print(f" {h} UNIT HIT ON ENTERPRISE FROM SECTOR {k[i][0] + 1} , {k[i][1] + 1}")
        if s <= 0:
            end_game(won=False, quit=False, enterprise_killed=True)
            return
        print(f"      <SHIELDS DOWN TO {s} UNITS>")
        if h >= 20 and random.random() < 0.60 and h / s > 0.02:
            r1 = fnr()
            d[r1] -= h / s + 0.5 * random.random()
            print(f"DAMAGE CONTROL REPORTS  '{devices[r1]} DAMAGED BY THE HIT'")


def shield_control() -> None:
    # Raise or lower the shields.
    global e, s

    if d[6] < 0:
        print("SHIELD CONTROL INOPERABLE")
        return

    while True:
        energy_to_shield = input(
            f"ENERGY AVAILABLE = {e + s} NUMBER OF UNITS TO SHIELDS? "
        )
        if len(energy_to_shield) > 0:
            x = int(energy_to_shield)
            break

    if x < 0 or s == x:
        print("<SHIELDS UNCHANGED>")
        return

    if x > e + s:
        print(
            "SHIELD CONTROL REPORTS  'THIS IS NOT THE FEDERATION "
            "TREASURY.'\n"
            "<SHIELDS UNCHANGED>"
        )
        return

    e += s - x
    s = x
    print("DEFLECTOR CONTROL ROOM REPORT:")
    print(f"  'SHIELDS NOW AT {s} UNITS PER YOUR COMMAND.'")


def damage_control():
    # Print a damage control report.
    global d, t

    if d[5] < 0:
        print("DAMAGE CONTROL REPORT NOT AVAILABLE")
    else:
        print("\nDEVICE             STATE OF REPAIR")
        for r1 in range(8):
            print(f"{devices[r1].ljust(26, ' ')}{int(d[r1] * 100) * 0.01:g}")
        print()

    if not docked:
        return

    d3 = sum(0.1 for i in range(8) if d[i] < 0)
    if d3 == 0:
        return

    d3 += d4
    if d3 >= 1:
        d3 = 0.9
    print("\nTECHNICIANS STANDING BY TO EFFECT REPAIRS TO YOUR SHIP;")
    print(f"ESTIMATED TIME TO REPAIR: {round(0.01 * int(100 * d3), 2)} STARDATES")
    if input("WILL YOU AUTHORIZE THE REPAIR ORDER (Y/N)? ").upper().strip() != "Y":
        return

    for i in range(8):
        if d[i] < 0:
            d[i] = 0
    t += d3 + 0.1


def computer() -> None:
    # Perform the various functions of the library computer.
    global d, z, k9, t0, t9, t, b9, s1, s2, b4, b5

    if d[7] < 0:
        print("COMPUTER DISABLED")
        return

    while True:
        command = input("COMPUTER ACTIVE AND AWAITING COMMAND? ")
        if len(command) == 0:
            com = 6
        else:
            com = int(command)
        if com < 0:
            return

        print()

        if com == 0 or com == 5:
            if com == 5:
                print("                        THE GALAXY")
            else:
                print(
                    "\n        COMPUTER RECORD OF GALAXY FOR "
                    f"QUADRANT {q1 + 1} , {q2 + 1}\n"
                )

            print("       1     2     3     4     5     6     7     8")
            sep = "     ----- ----- ----- ----- ----- ----- ----- -----"
            print(sep)

            for i in range(8):
                line = " " + str(i + 1) + " "

                if com == 5:
                    g2s = quadrant_name(i, 0, True)
                    line += (" " * int(12 - 0.5 * len(g2s))) + g2s
                    g2s = quadrant_name(i, 4, True)
                    line += (" " * int(39 - 0.5 * len(g2s) - len(line))) + g2s
                else:
                    for j in range(8):
                        line += "   "
                        if z[i][j] == 0:
                            line += "***"
                        else:
                            line += str(z[i][j] + 1000)[-3:]

                print(line)
                print(sep)

            print()
            return
        elif com == 1:
            print("   STATUS REPORT:")
            print(f"KLINGON{'S' if k9 > 1 else ''} LEFT: {k9}")
            print(
                "MISSION MUST BE COMPLETED IN "
                f"{round(0.1 * int((t0+t9-t) * 10), 1)} STARDATES"
            )

            if b9 == 0:
                print("YOUR STUPIDITY HAS LEFT YOU ON YOUR OWN IN")
                print("  THE GALAXY -- YOU HAVE NO STARBASES LEFT!")
            else:
                print(
                    f"THE FEDERATION IS MAINTAINING {b9} "
                    f"STARBASE{'S' if b9 > 1 else ''} IN THE GALAXY"
                )

            damage_control()
            return
        elif com == 2:
            if k3 <= 0:
                print(
                    "SCIENCE OFFICER SPOCK REPORTS  'SENSORS SHOW NO ENEMY "
                    "SHIPS\n"
                    "                                IN THIS QUADRANT'"
                )
                return

            print(f"FROM ENTERPRISE TO KLINGON BATTLE CRUISER{'S' if k3 > 1 else ''}")

            for i in range(3):
                if k[i][2] > 0:
                    print_direction(s1, s2, k[i][0], k[i][1])
            return
        elif com == 3:
            if b3 == 0:
                print(
                    "MR. SPOCK REPORTS,  'SENSORS SHOW NO STARBASES IN THIS "
                    "QUADRANT.'"
                )
                return

            print("FROM ENTERPRISE TO STARBASE:")
            print_direction(s1, s2, b4, b5)
            return
        elif com == 4:
            print("DIRECTION/DISTANCE CALCULATOR:")
            print(f"YOU ARE AT QUADRANT {q1+1} , {q2+1} SECTOR {s1+1} , {s2+1}")
            print("PLEASE ENTER")
            while True:
                coordinates = input("  INITIAL COORDINATES (X,Y)? ").split(",")
                if len(coordinates) == 2:
                    from1, from2 = int(coordinates[0]) - 1, int(coordinates[1]) - 1
                    if 0 <= from1 <= 7 and 0 <= from2 <= 7:
                        break
            while True:
                coordinates = input("  FINAL COORDINATES (X,Y)? ").split(",")
                if len(coordinates) == 2:
                    to1, to2 = int(coordinates[0]) - 1, int(coordinates[1]) - 1
                    if 0 <= to1 <= 7 and 0 <= to2 <= 7:
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


def print_direction(from1, from2, to1, to2) -> None:
    # Print direction and distance between two locations in the grid.
    delta1 = -(to1 - from1)  # flip so positive is up (heading = 3)
    delta2 = to2 - from2

    if delta2 > 0:
        if delta1 < 0:
            base = 7
        else:
            base = 1
            delta1, delta2 = delta2, delta1
    else:
        if delta1 > 0:
            base = 3
        else:
            base = 5
            delta1, delta2 = delta2, delta1

    delta1, delta2 = abs(delta1), abs(delta2)

    if delta1 > 0 or delta2 > 0:  # bug in original; no check for divide by 0
        if delta1 >= delta2:
            print(f"DIRECTION = {round(base + delta2 / delta1, 6)}")
        else:
            print(f"DIRECTION = {round(base + 2 - delta1 / delta2, 6)}")

    print(f"DISTANCE = {round(sqrt(delta1 ** 2 + delta2 ** 2), 6)}")


# -------------------------------------------------------------------------
#  Game transitions
# -------------------------------------------------------------------------


def startup() -> None:
    # Initialize the game variables and map, and print startup messages.
    global g, z, d, t, t0, t9, docked, e, e0, p, p0, s, k9, b9, s9, c
    global devices, q1, q2, s1, s2, k7

    print(
        "\n\n\n\n\n\n\n\n\n\n\n"
        "                                    ,------*------,\n"
        "                    ,-------------   '---  ------'\n"
        "                     '-------- --'      / /\n"
        "                         ,---' '-------/ /--,\n"
        "                          '----------------'\n\n"
        "                    THE USS ENTERPRISE --- NCC-1701\n"
        "\n\n\n\n"
    )

    # set up global game variables
    g = [[0] * 8 for _ in range(8)]  # galaxy map
    z = [[0] * 8 for _ in range(8)]  # charted galaxy map
    d = [0] * 8  # damage stats for devices
    t = t0 = 100 * random.randint(20, 39)  # stardate (current, initial)
    t9 = random.randint(25, 34)  # mission duration (stardates)
    docked = False  # true when docked at starbase
    e = e0 = 3000  # energy (current, initial)
    p = p0 = 10  # torpedoes (current, initial)
    s = 0  # shields
    k9, b9 = 0, 0  # total Klingons, bases in galaxy
    # ^ bug in original, was b9 = 2
    s9 = 200  # avg. Klingon shield strength

    c = [
        [0, 1],
        [-1, 1],
        [-1, 0],
        [-1, -1],
        [0, -1],
        [1, -1],
        [1, 0],
        [1, 1],
        [0, 1],
    ]  # vectors in cardinal directions

    devices = [
        "WARP ENGINES",
        "SHORT RANGE SENSORS",
        "LONG RANGE SENSORS",
        "PHASER CONTROL",
        "PHOTON TUBES",
        "DAMAGE CONTROL",
        "SHIELD CONTROL",
        "LIBRARY-COMPUTER",
    ]

    # initialize Enterprise's position
    q1, q2 = fnr(), fnr()  # Enterprise's quadrant
    s1, s2 = fnr(), fnr()  # ...and sector

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

    if b9 == 0:  # original has buggy extra code here
        b9 = 1
        g[q1][q2] += 10
        q1, q2 = fnr(), fnr()

    k7 = k9  # Klingons at start of game

    print(
        "YOUR ORDERS ARE AS FOLLOWS:\n"
        f"     DESTROY THE {k9} KLINGON WARSHIPS WHICH HAVE INVADED\n"
        "   THE GALAXY BEFORE THEY CAN ATTACK FEDERATION HEADQUARTERS\n"
        f"   ON STARDATE {t0+t9}. THIS GIVES YOU {t9} DAYS. THERE "
        f"{'IS' if b9 == 1 else 'ARE'}\n"
        f"   {b9} STARBASE{'' if b9 == 1 else 'S'} IN THE GALAXY FOR "
        "RESUPPLYING YOUR SHIP.\n"
    )


def new_quadrant() -> None:
    # Enter a new quadrant: populate map and print a short range scan.
    global z, k3, b3, s3, d4, k, qs, b4, b5

    k3 = b3 = s3 = 0  # Klingons, bases, stars in quad.
    d4 = 0.5 * random.random()  # extra delay in repairs at base
    z[q1][q2] = g[q1][q2]

    if 0 <= q1 <= 7 and 0 <= q2 <= 7:
        quad = quadrant_name(q1, q2, False)
        if t == t0:
            print("\nYOUR MISSION BEGINS WITH YOUR STARSHIP LOCATED")
            print(f"IN THE GALACTIC QUADRANT, '{quad}'.\n")
        else:
            print(f"\nNOW ENTERING {quad} QUADRANT . . .\n")

        k3 = g[q1][q2] // 100
        b3 = g[q1][q2] // 10 - 10 * k3
        s3 = g[q1][q2] - 100 * k3 - 10 * b3

        if k3 != 0:
            print("COMBAT AREA      CONDITION RED")
            if s <= 200:
                print("   SHIELDS DANGEROUSLY LOW")

    k = [[0, 0, 0], [0, 0, 0], [0, 0, 0]]  # Klingons in current quadrant
    qs = " " * 192  # quadrant string

    # build quadrant string
    insert_marker(s1, s2, "<*>")
    for i in range(k3):
        r1, r2 = find_empty_place()
        insert_marker(r1, r2, "+K+")
        k[i] = [r1, r2, s9 * (0.5 + random.random())]
    if b3 > 0:
        b4, b5 = find_empty_place()  # position of starbase (sector)
        insert_marker(b4, b5, ">!<")
    for _ in range(s3):
        r1, r2 = find_empty_place()
        insert_marker(r1, r2, " * ")

    short_range_scan()


def end_game(
    won: bool = False, quit: bool = True, enterprise_killed: bool = False
) -> None:
    # Handle end-of-game situations.
    global restart

    if won:
        print("CONGRATULATIONS, CAPTAIN! THE LAST KLINGON BATTLE CRUISER")
        print("MENACING THE FEDERATION HAS BEEN DESTROYED.\n")
        print(f"YOUR EFFICIENCY RATING IS {round(1000 * (k7 / (t - t0))**2, 4)}\n\n")
    else:
        if not quit:
            if enterprise_killed:
                print(
                    "\nTHE ENTERPRISE HAS BEEN DESTROYED. THE FEDERATION "
                    "WILL BE CONQUERED."
                )
            print(f"IT IS STARDATE {round(t, 1)}")

        print(f"THERE WERE {k9} KLINGON BATTLE CRUISERS LEFT AT")
        print("THE END OF YOUR MISSION.\n\n")

        if b9 == 0:
            exit()

    print("THE FEDERATION IS IN NEED OF A NEW STARSHIP COMMANDER")
    print("FOR A SIMILAR MISSION -- IF THERE IS A VOLUNTEER,")
    if input("LET HIM STEP FORWARD AND ENTER 'AYE'? ").upper().strip() != "AYE":
        exit()
    restart = True


# -------------------------------------------------------------------------
#  Entry point and main game loop
# -------------------------------------------------------------------------


def main() -> None:
    global restart

    f: Dict[str, Callable[[], None]] = {
        "NAV": navigation,
        "SRS": short_range_scan,
        "LRS": long_range_scan,
        "PHA": phaser_control,
        "TOR": photon_torpedoes,
        "SHE": shield_control,
        "DAM": damage_control,
        "COM": computer,
        "XXX": end_game,
    }

    while True:
        startup()
        new_quadrant()
        restart = False

        while not restart:
            if s + e <= 10 or (e <= 10 and d[6] != 0):
                print(
                    "\n** FATAL ERROR **   YOU'VE JUST STRANDED YOUR SHIP "
                    "IN SPACE.\nYOU HAVE INSUFFICIENT MANEUVERING ENERGY, "
                    "AND SHIELD CONTROL\nIS PRESENTLY INCAPABLE OF CROSS-"
                    "CIRCUITING TO ENGINE ROOM!!"
                )

            command = input("COMMAND? ").upper().strip()

            if command in f:
                f[command]()
            else:
                print(
                    "ENTER ONE OF THE FOLLOWING:\n"
                    "  NAV  (TO SET COURSE)\n"
                    "  SRS  (FOR SHORT RANGE SENSOR SCAN)\n"
                    "  LRS  (FOR LONG RANGE SENSOR SCAN)\n"
                    "  PHA  (TO FIRE PHASERS)\n"
                    "  TOR  (TO FIRE PHOTON TORPEDOES)\n"
                    "  SHE  (TO RAISE OR LOWER SHIELDS)\n"
                    "  DAM  (FOR DAMAGE CONTROL REPORTS)\n"
                    "  COM  (TO CALL ON LIBRARY-COMPUTER)\n"
                    "  XXX  (TO RESIGN YOUR COMMAND)\n"
                )


if __name__ == "__main__":
    main()
