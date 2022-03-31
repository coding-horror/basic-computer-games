"""
****        **** STAR TREK ****        ****
**** SIMULATION OF A MISSION OF THE STARSHIP ENTERPRISE,
**** AS SEEN ON THE STAR TREK TV SHOW.
**** ORIGINAL PROGRAM BY MIKE MAYFIELD, MODIFIED VERSION
**** PUBLISHED IN DEC'S "101 BASIC GAMES", BY DAVE AHL.
**** MODIFICATIONS TO THE LATTER (PLUS DEBUGGING) BY BOB
**** LEEDOM - APRIL & DECEMBER 1974,
**** WITH A LITTLE HELP FROM HIS FRIENDS . . .

Python translation by Jack Boyce - February 2021
  Output is identical to BASIC version except for a few
  fixes (as noted, search `bug`) and minor cleanup.
"""


import random
from math import sqrt
from typing import Any, Callable, Dict, List, Tuple


class Ship:
    def __init__(self, energy: int = 3000, shields: int = 0, torpedoes: int = 10):
        self.energy: int = energy
        self.devices: Tuple[str, ...] = (
            "WARP ENGINES",
            "SHORT RANGE SENSORS",
            "LONG RANGE SENSORS",
            "PHASER CONTROL",
            "PHOTON TUBES",
            "DAMAGE CONTROL",
            "SHIELD CONTROL",
            "LIBRARY-COMPUTER",
        )
        self.damage_stats: List[float] = [0] * len(
            self.devices
        )  # damage stats for devices
        self.shields = shields
        self.torpedoes = torpedoes
        self.docked: bool = False  # true when docked at starbase

    def maneuver_energy(self, n: int) -> None:
        """Deduct the energy for navigation from energy/shields."""
        self.energy -= n + 10

        if self.energy <= 0:
            print("SHIELD CONTROL SUPPLIES ENERGY TO COMPLETE THE MANEUVER.")
            self.shields += self.energy
            self.energy = 0
            self.shields = max(0, self.shields)

    def shield_control(self) -> None:
        """Raise or lower the shields."""
        if self.damage_stats[6] < 0:
            print("SHIELD CONTROL INOPERABLE")
            return

        while True:
            energy_to_shield = input(
                f"ENERGY AVAILABLE = {self.energy + self.shields} NUMBER OF UNITS TO SHIELDS? "
            )
            if len(energy_to_shield) > 0:
                x = int(energy_to_shield)
                break

        if x < 0 or self.shields == x:
            print("<SHIELDS UNCHANGED>")
            return

        if x > self.energy + self.shields:
            print(
                "SHIELD CONTROL REPORTS  'THIS IS NOT THE FEDERATION "
                "TREASURY.'\n"
                "<SHIELDS UNCHANGED>"
            )
            return

        self.energy += self.shields - x
        self.shields = x
        print("DEFLECTOR CONTROL ROOM REPORT:")
        print(f"  'SHIELDS NOW AT {ship.shields} UNITS PER YOUR COMMAND.'")

    def short_range_scan(self) -> None:
        """Print a short range scan."""
        self.docked = False
        for i in (s1 - 1, s1, s1 + 1):
            for j in (s2 - 1, s2, s2 + 1):
                if 0 <= i <= 7 and 0 <= j <= 7 and compare_marker(i, j, ">!<"):
                    self.docked = True
                    cs = "DOCKED"
                    self.energy, self.torpedoes = initial_energy, initial_torpedoes
                    print("SHIELDS DROPPED FOR DOCKING PURPOSES")
                    self.shields = 0
                    break
            else:
                continue
            break
        else:
            if k3 > 0:
                cs = "*RED*"
            elif self.energy < initial_energy * 0.1:
                cs = "YELLOW"
            else:
                cs = "GREEN"

        if self.damage_stats[1] < 0:
            print("\n*** SHORT RANGE SENSORS ARE OUT ***\n")
            return

        sep = "---------------------------------"
        print(sep)
        for i in range(8):
            line = ""
            for j in range(8):
                pos = i * 24 + j * 3
                line = line + " " + quadrant_string[pos : (pos + 3)]

            if i == 0:
                line += f"        STARDATE           {round(int(current_stardate * 10) * 0.1, 1)}"
            elif i == 1:
                line += f"        CONDITION          {cs}"
            elif i == 2:
                line += f"        QUADRANT           {q1 + 1} , {q2 + 1}"
            elif i == 3:
                line += f"        SECTOR             {s1 + 1} , {s2 + 1}"
            elif i == 4:
                line += f"        PHOTON TORPEDOES   {int(self.torpedoes)}"
            elif i == 5:
                line += f"        TOTAL ENERGY       {int(self.energy + self.shields)}"
            elif i == 6:
                line += f"        SHIELDS            {int(self.shields)}"
            else:
                line += f"        KLINGONS REMAINING {total_klingons}"

            print(line)
        print(sep)


# Global variables
restart = False
klingons: List[List[float]] = [
    [0, 0, 0],
    [0, 0, 0],
    [0, 0, 0],
]  # Klingons in current quadrant
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
quadrant_string = " " * 192  # quadrant string
# set up global game variables
galaxy_map = [[0] * 8 for _ in range(8)]  # galaxy map
charted_galaxy_map = [[0] * 8 for _ in range(8)]  # charted galaxy map
current_stardate: float
current_stardate = initial_stardate = 100 * random.randint(
    20, 39
)  # stardate (current, initial)
mission_duration = random.randint(25, 34)  # mission duration (stardates)
initial_energy = 3000
initial_torpedoes = 10
ship = Ship(energy=initial_energy, torpedoes=initial_torpedoes)
total_klingons, bases_in_galaxy = 0, 0  # total Klingons, bases in galaxy
# ^ bug in original, was b9 = 2
klingon_shield_strength = 200  # avg. Klingon shield strength

current_klingons = total_klingons  # Klingons at start of game
delay_in_repairs_at_base = 0.5 * random.random()  # extra delay in repairs at base

# -------------------------------------------------------------------------
#  Utility functions
# -------------------------------------------------------------------------
def fnr() -> int:
    """Generate a random integer from 0 to 7 inclusive."""
    return random.randint(0, 7)


def quadrant_name(row: int, col: int, region_only: bool = False) -> str:
    """Return quadrant name visible on scans, etc."""
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


def insert_marker(row: float, col: float, marker: str) -> None:
    """
    Insert a marker into a given position in the quadrant string `qs`. The
    contents of a quadrant (Enterprise, stars, etc.) are stored in `qs`.
    """
    global quadrant_string

    if len(marker) != 3:
        print("ERROR")
        exit()

    pos = round(col) * 3 + round(row) * 24
    quadrant_string = quadrant_string[0:pos] + marker + quadrant_string[(pos + 3) : 192]


def compare_marker(row: float, col: float, test_marker: str) -> bool:
    """
    Check whether the position in the current quadrant is occupied with a
    given marker.
    """
    pos = round(col) * 3 + round(row) * 24
    return quadrant_string[pos : (pos + 3)] == test_marker


def find_empty_place() -> Tuple[int, int]:
    """Find an empty location in the current quadrant."""
    while True:
        row, col = fnr(), fnr()
        if compare_marker(row, col, "   "):
            return row, col


# -------------------------------------------------------------------------
#  Functions for individual player commands
# -------------------------------------------------------------------------
def navigation() -> None:
    """Take navigation input and move the Enterprise."""
    global ship, klingons, quadrant_string, current_stardate, q1, q2, s1, s2

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
        warps = input(f"WARP FACTOR (0-{'0.2' if ship.damage_stats[0] < 0 else '8'})? ")
        if len(warps) > 0:
            warp = float(warps)
            break
    if ship.damage_stats[0] < 0 and warp > 0.2:
        print("WARP ENGINES ARE DAMAGED. MAXIMUM SPEED = WARP 0.2")
        return
    if warp == 0:
        return
    if warp < 0 or warp > 8:
        print(f"   CHIEF ENGINEER SCOTT REPORTS 'THE ENGINES WON'T TAKE WARP {warp}!'")
        return

    n = round(warp * 8)
    if ship.energy < n:
        print("ENGINEERING REPORTS   'INSUFFICIENT ENERGY AVAILABLE")
        print(f"                       FOR MANEUVERING AT WARP {warp}!'")
        if ship.shields >= n - ship.energy and ship.damage_stats[6] >= 0:
            print(f"DEFLECTOR CONTROL ROOM ACKNOWLEDGES {ship.shields} UNITS OF ENERGY")
            print("                         PRESENTLY DEPLOYED TO SHIELDS.")
        return

    # klingons move and fire
    for i in range(3):
        if klingons[i][2] != 0:
            insert_marker(klingons[i][0], klingons[i][1], "   ")
            klingons[i][0], klingons[i][1] = find_empty_place()
            insert_marker(klingons[i][0], klingons[i][1], "+K+")

    klingons_fire()

    # repair damaged devices and print damage report
    line = ""
    for i in range(8):
        if ship.damage_stats[i] < 0:
            ship.damage_stats[i] += min(warp, 1)
            if -0.1 < ship.damage_stats[i] < 0:
                ship.damage_stats[i] = -0.1
            elif ship.damage_stats[i] >= 0:
                if len(line) == 0:
                    line = "DAMAGE CONTROL REPORT:"
                line += "   " + ship.devices[i] + " REPAIR COMPLETED\n"
    if len(line) > 0:
        print(line)
    if random.random() <= 0.2:
        r1 = fnr()
        if random.random() < 0.6:
            ship.damage_stats[r1] -= random.random() * 5 + 1
            print(f"DAMAGE CONTROL REPORT:   {ship.devices[r1]} DAMAGED\n")
        else:
            ship.damage_stats[r1] += random.random() * 3 + 1
            print(
                f"DAMAGE CONTROL REPORT:   {ship.devices[r1]} STATE OF REPAIR IMPROVED\n"
            )

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
                if current_stardate > initial_stardate + mission_duration:
                    end_game(won=False, quit=False)
                    return

            if q1 == q1_start and q2 == q2_start:
                break
            current_stardate += 1
            ship.maneuver_energy(n)
            new_quadrant()
            return
        else:
            pos = int(s1) * 24 + int(s2) * 3
            if quadrant_string[pos : (pos + 2)] != "  ":
                s1, s2 = int(s1 - x1), int(s2 - x2)
                print(
                    "WARP ENGINES SHUT DOWN AT SECTOR "
                    f"{s1 + 1} , {s2 + 1} DUE TO BAD NAVAGATION"
                )
                break
    else:
        s1, s2 = int(s1), int(s2)

    insert_marker(int(s1), int(s2), "<*>")
    ship.maneuver_energy(n)

    current_stardate += 0.1 * int(10 * warp) if warp < 1 else 1
    if current_stardate > initial_stardate + mission_duration:
        end_game(won=False, quit=False)
        return

    ship.short_range_scan()


def long_range_scan() -> None:
    """Print a long range scan."""
    global charted_galaxy_map, galaxy_map

    if ship.damage_stats[2] < 0:
        print("LONG RANGE SENSORS ARE INOPERABLE")
        return

    print(f"LONG RANGE SCAN FOR QUADRANT {q1 + 1} , {q2 + 1}")
    print_scan_results(q1, q2, galaxy_map, charted_galaxy_map)


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
    """Take phaser control input and fire phasers."""
    global ship, klingons, galaxy_map, charted_galaxy_map, k3, total_klingons

    if ship.damage_stats[3] < 0:
        print("PHASERS INOPERATIVE")
        return

    if k3 <= 0:
        print("SCIENCE OFFICER SPOCK REPORTS  'SENSORS SHOW NO ENEMY SHIPS")
        print("                                IN THIS QUADRANT'")
        return

    if ship.damage_stats[7] < 0:
        print("COMPUTER FAILURE HAMPERS ACCURACY")

    print(f"PHASERS LOCKED ON TARGET;  ENERGY AVAILABLE = {ship.energy} UNITS")
    x = 0
    while True:
        while True:
            units_to_fire = input("NUMBER OF UNITS TO FIRE? ")
            if len(units_to_fire) > 0:
                x = int(units_to_fire)
                break
        if x <= 0:
            return
        if ship.energy >= x:
            break
        print(f"ENERGY AVAILABLE = {ship.energy} UNITS")

    ship.energy -= x
    if ship.damage_stats[7] < 0:  # bug in original, was d[6]
        x *= random.random()  # type: ignore

    h1 = int(x / k3)
    for i in range(3):
        if klingons[i][2] <= 0:
            continue

        h = int((h1 / fnd(i)) * (random.random() + 2))
        if h <= 0.15 * klingons[i][2]:
            print(
                f"SENSORS SHOW NO DAMAGE TO ENEMY AT {klingons[i][0] + 1} , {klingons[i][1] + 1}"
            )
        else:
            klingons[i][2] -= h
            print(
                f" {h} UNIT HIT ON KLINGON AT SECTOR {klingons[i][0] + 1} , {klingons[i][1] + 1}"
            )
            if klingons[i][2] <= 0:
                print("*** KLINGON DESTROYED ***")
                k3 -= 1
                total_klingons -= 1
                insert_marker(klingons[i][0], klingons[i][1], "   ")
                klingons[i][2] = 0
                galaxy_map[q1][q2] -= 100
                charted_galaxy_map[q1][q2] = galaxy_map[q1][q2]
                if total_klingons <= 0:
                    end_game(won=True, quit=False)
                    return
            else:
                print(f"   (SENSORS SHOW {round(klingons[i][2],6)} UNITS REMAINING)")

    klingons_fire()


def photon_torpedoes() -> None:
    """Take photon torpedo input and process firing of torpedoes."""
    global ship, k3, total_klingons, klingons, b3, bases_in_galaxy, galaxy_map, charted_galaxy_map

    if ship.torpedoes <= 0:
        print("ALL PHOTON TORPEDOES EXPENDED")
        return
    if ship.damage_stats[4] < 0:
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
    ship.energy -= 2
    ship.torpedoes -= 1
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
        total_klingons -= 1
        if total_klingons <= 0:
            end_game(won=True, quit=False)
            return
        for i in range(3):
            if x3 == klingons[i][0] and y3 == klingons[i][1]:
                klingons[i][2] = 0
    elif compare_marker(x3, y3, " * "):
        print(f"STAR AT {x3 + 1} , {y3 + 1} ABSORBED TORPEDO ENERGY.")
        klingons_fire()
        return
    elif compare_marker(x3, y3, ">!<"):
        print("*** STARBASE DESTROYED ***")
        b3 -= 1
        bases_in_galaxy -= 1
        if (
            bases_in_galaxy == 0
            and total_klingons <= current_stardate - initial_stardate - mission_duration
        ):
            print("THAT DOES IT, CAPTAIN!! YOU ARE HEREBY RELIEVED OF COMMAND")
            print("AND SENTENCED TO 99 STARDATES AT HARD LABOR ON CYGNUS 12!!")
            end_game(won=False)
            return
        else:
            print("STARFLEET COMMAND REVIEWING YOUR RECORD TO CONSIDER")
            print("COURT MARTIAL!")
            ship.docked = False

    insert_marker(x3, y3, "   ")
    galaxy_map[q1][q2] = k3 * 100 + b3 * 10 + s3
    charted_galaxy_map[q1][q2] = galaxy_map[q1][q2]
    klingons_fire()


def fnd(i: int) -> float:
    """Find distance between Enterprise and i'th Klingon warship."""
    return sqrt((klingons[i][0] - s1) ** 2 + (klingons[i][1] - s2) ** 2)


def klingons_fire() -> None:
    """Process nearby Klingons firing on Enterprise."""
    global ship, klingons

    if k3 <= 0:
        return
    if ship.docked:
        print("STARBASE SHIELDS PROTECT THE ENTERPRISE")
        return

    for i in range(3):
        if klingons[i][2] <= 0:
            continue

        h = int((klingons[i][2] / fnd(i)) * (random.random() + 2))
        ship.shields -= h
        klingons[i][2] /= random.random() + 3
        print(
            f" {h} UNIT HIT ON ENTERPRISE FROM SECTOR {klingons[i][0] + 1} , {klingons[i][1] + 1}"
        )
        if ship.shields <= 0:
            end_game(won=False, quit=False, enterprise_killed=True)
            return
        print(f"      <SHIELDS DOWN TO {ship.shields} UNITS>")
        if h >= 20 and random.random() < 0.60 and h / ship.shields > 0.02:
            r1 = fnr()
            ship.damage_stats[r1] -= h / ship.shields + 0.5 * random.random()
            print(f"DAMAGE CONTROL REPORTS  '{ship.devices[r1]} DAMAGED BY THE HIT'")


def damage_control() -> None:
    """Print a damage control report."""
    global ship, current_stardate

    if ship.damage_stats[5] < 0:
        print("DAMAGE CONTROL REPORT NOT AVAILABLE")
    else:
        print("\nDEVICE             STATE OF REPAIR")
        for r1 in range(8):
            print(
                f"{ship.devices[r1].ljust(26, ' ')}{int(ship.damage_stats[r1] * 100) * 0.01:g}"
            )
        print()

    if not ship.docked:
        return

    d3 = sum(0.1 for i in range(8) if ship.damage_stats[i] < 0)
    if d3 == 0:
        return

    d3 += delay_in_repairs_at_base
    if d3 >= 1:
        d3 = 0.9
    print("\nTECHNICIANS STANDING BY TO EFFECT REPAIRS TO YOUR SHIP;")
    print(f"ESTIMATED TIME TO REPAIR: {round(0.01 * int(100 * d3), 2)} STARDATES")
    if input("WILL YOU AUTHORIZE THE REPAIR ORDER (Y/N)? ").upper().strip() != "Y":
        return

    for i in range(8):
        if ship.damage_stats[i] < 0:
            ship.damage_stats[i] = 0
    current_stardate += d3 + 0.1


def computer() -> None:
    """Perform the various functions of the library computer."""
    global ship, charted_galaxy_map, total_klingons, initial_stardate, mission_duration, current_stardate, bases_in_galaxy, s1, s2, b4, b5

    if ship.damage_stats[7] < 0:
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
                        if charted_galaxy_map[i][j] == 0:
                            line += "***"
                        else:
                            line += str(charted_galaxy_map[i][j] + 1000)[-3:]

                print(line)
                print(sep)

            print()
            return
        elif com == 1:
            print("   STATUS REPORT:")
            print(f"KLINGON{'S' if total_klingons > 1 else ''} LEFT: {total_klingons}")
            print(
                "MISSION MUST BE COMPLETED IN "
                f"{round(0.1 * int((initial_stardate+mission_duration-current_stardate) * 10), 1)} STARDATES"
            )

            if bases_in_galaxy == 0:
                print("YOUR STUPIDITY HAS LEFT YOU ON YOUR OWN IN")
                print("  THE GALAXY -- YOU HAVE NO STARBASES LEFT!")
            else:
                print(
                    f"THE FEDERATION IS MAINTAINING {bases_in_galaxy} "
                    f"STARBASE{'S' if bases_in_galaxy > 1 else ''} IN THE GALAXY"
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
                if klingons[i][2] > 0:
                    print_direction(s1, s2, klingons[i][0], klingons[i][1])
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


def print_direction(from1: float, from2: float, to1: float, to2: float) -> None:
    """Print direction and distance between two locations in the grid."""
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
    """Initialize the game variables and map, and print startup messages."""
    global galaxy_map, charted_galaxy_map, ship, current_stardate
    global initial_stardate, mission_duration
    global initial_energy, initial_torpedoes
    global ship, total_klingons, bases_in_galaxy, klingon_shield_strength, c
    global q1, q2, s1, s2, current_klingons

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
    galaxy_map = [[0] * 8 for _ in range(8)]  # galaxy map
    charted_galaxy_map = [[0] * 8 for _ in range(8)]  # charted galaxy map
    current_stardate = initial_stardate = 100 * random.randint(
        20, 39
    )  # stardate (current, initial)
    mission_duration = random.randint(25, 34)  # mission duration (stardates)
    initial_energy = 3000
    initial_torpedoes = 10
    ship.damage_stats = [0] * 8  # damage stats for devices
    ship.energy = initial_energy
    ship.docked = False
    ship.torpedoes = initial_torpedoes
    ship.shields = 0  # shields
    total_klingons, bases_in_galaxy = 0, 0  # total Klingons, bases in galaxy
    # ^ bug in original, was b9 = 2
    klingon_shield_strength = 200  # avg. Klingon shield strength

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
            total_klingons += k3

            b3 = 0
            if random.random() > 0.96:
                b3 = 1
                bases_in_galaxy += 1
            galaxy_map[i][j] = k3 * 100 + b3 * 10 + fnr() + 1

    if total_klingons > mission_duration:
        mission_duration = total_klingons + 1

    if bases_in_galaxy == 0:  # original has buggy extra code here
        bases_in_galaxy = 1
        galaxy_map[q1][q2] += 10
        q1, q2 = fnr(), fnr()

    current_klingons = total_klingons  # Klingons at start of game

    print(
        "YOUR ORDERS ARE AS FOLLOWS:\n"
        f"     DESTROY THE {total_klingons} KLINGON WARSHIPS WHICH HAVE INVADED\n"
        "   THE GALAXY BEFORE THEY CAN ATTACK FEDERATION HEADQUARTERS\n"
        f"   ON STARDATE {initial_stardate+mission_duration}. THIS GIVES YOU {mission_duration} DAYS. THERE "
        f"{'IS' if bases_in_galaxy == 1 else 'ARE'}\n"
        f"   {bases_in_galaxy} STARBASE{'' if bases_in_galaxy == 1 else 'S'} IN THE GALAXY FOR "
        "RESUPPLYING YOUR SHIP.\n"
    )


def new_quadrant() -> None:
    """Enter a new quadrant: populate map and print a short range scan."""
    global charted_galaxy_map, k3, b3, s3, delay_in_repairs_at_base, klingons, quadrant_string, b4, b5

    k3 = b3 = s3 = 0  # Klingons, bases, stars in quad.
    delay_in_repairs_at_base = 0.5 * random.random()  # extra delay in repairs at base
    charted_galaxy_map[q1][q2] = galaxy_map[q1][q2]

    if 0 <= q1 <= 7 and 0 <= q2 <= 7:
        quad = quadrant_name(q1, q2, False)
        if current_stardate == initial_stardate:
            print("\nYOUR MISSION BEGINS WITH YOUR STARSHIP LOCATED")
            print(f"IN THE GALACTIC QUADRANT, '{quad}'.\n")
        else:
            print(f"\nNOW ENTERING {quad} QUADRANT . . .\n")

        k3 = galaxy_map[q1][q2] // 100
        b3 = galaxy_map[q1][q2] // 10 - 10 * k3
        s3 = galaxy_map[q1][q2] - 100 * k3 - 10 * b3

        if k3 != 0:
            print("COMBAT AREA      CONDITION RED")
            if ship.shields <= 200:
                print("   SHIELDS DANGEROUSLY LOW")

    klingons = [[0, 0, 0], [0, 0, 0], [0, 0, 0]]  # Klingons in current quadrant
    quadrant_string = " " * 192  # quadrant string

    # build quadrant string
    insert_marker(s1, s2, "<*>")
    for i in range(k3):
        r1, r2 = find_empty_place()
        insert_marker(r1, r2, "+K+")
        klingons[i] = [r1, r2, klingon_shield_strength * (0.5 + random.random())]
    if b3 > 0:
        b4, b5 = find_empty_place()  # position of starbase (sector)
        insert_marker(b4, b5, ">!<")
    for _ in range(s3):
        r1, r2 = find_empty_place()
        insert_marker(r1, r2, " * ")

    ship.short_range_scan()


def end_game(
    won: bool = False, quit: bool = True, enterprise_killed: bool = False
) -> None:
    """Handle end-of-game situations."""
    global restart

    if won:
        print("CONGRATULATIONS, CAPTAIN! THE LAST KLINGON BATTLE CRUISER")
        print("MENACING THE FEDERATION HAS BEEN DESTROYED.\n")
        print(
            f"YOUR EFFICIENCY RATING IS {round(1000 * (current_klingons / (current_stardate - initial_stardate))**2, 4)}\n\n"
        )
    else:
        if not quit:
            if enterprise_killed:
                print(
                    "\nTHE ENTERPRISE HAS BEEN DESTROYED. THE FEDERATION "
                    "WILL BE CONQUERED."
                )
            print(f"IT IS STARDATE {round(current_stardate, 1)}")

        print(f"THERE WERE {total_klingons} KLINGON BATTLE CRUISERS LEFT AT")
        print("THE END OF YOUR MISSION.\n\n")

        if bases_in_galaxy == 0:
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
    global restart, ship

    f: Dict[str, Callable[[], None]] = {
        "NAV": navigation,
        "SRS": ship.short_range_scan,
        "LRS": long_range_scan,
        "PHA": phaser_control,
        "TOR": photon_torpedoes,
        "SHE": ship.shield_control,
        "DAM": damage_control,
        "COM": computer,
        "XXX": end_game,
    }

    while True:
        startup()
        new_quadrant()
        restart = False

        while not restart:
            if ship.shields + ship.energy <= 10 or (
                ship.energy <= 10 and ship.damage_stats[6] != 0
            ):
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
