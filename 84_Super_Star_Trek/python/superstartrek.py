"""
****        **** STAR TREK ****        ****
**** SIMULATION OF A MISSION OF THE STARSHIP ENTERPRISE,
**** AS SEEN ON THE STAR TREK TV SHOW.
**** ORIGINAL PROGRAM BY MIKE MAYFIELD, MODIFIED VERSION
**** PUBLISHED IN DEC'S "101 BASIC GAMES", BY DAVE AHL.
**** MODIFICATIONS TO THE LATTER (PLUS DEBUGGING) BY BOB
**** LEEDOM - APRIL & DECEMBER 1974,
**** WITH A LITTLE HELP FROM HIS FRIENDS . . .

  Output is identical to BASIC version except for a few
  fixes (as noted, search `bug`) and minor cleanup.
"""


import random
import sys
from dataclasses import dataclass
from enum import Enum
from math import sqrt
from typing import Callable, Dict, Final, List, Optional, Tuple


def get_user_float(prompt: str) -> float:
    """Get input from user and return it."""
    while True:
        answer = input(prompt)
        try:
            answer_float = float(answer)
            return answer_float
        except ValueError:
            pass


class Entity(Enum):
    klingon = "+K+"
    ship = "<*>"
    empty = "***"
    starbase = ">!<"
    star = " * "
    void = "   "


@dataclass
class Point:
    x: int
    y: int

    def __str__(self) -> str:
        return f"{self.x + 1} , {self.y + 1}"


@dataclass
class Position:
    """
    Every quadrant has 8 sectors

    Hence the position could also be represented as:
    x = quadrant.x * 8 + sector.x
    y = quadrant.y * 8 + sector.y
    """

    quadrant: Point
    sector: Point


@dataclass
class QuadrantData:
    klingons: int
    bases: int
    stars: int

    def num(self) -> int:
        return 100 * self.klingons + 10 * self.bases + self.stars


@dataclass
class KlingonShip:
    sector: Point
    shield: float


class Ship:
    energy_capacity: int = 3000
    torpedo_capacity: int = 10

    def __init__(self) -> None:
        self.position = Position(Point(fnr(), fnr()), Point(fnr(), fnr()))
        self.energy: int = Ship.energy_capacity
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
        self.damage_stats: List[float] = [0] * len(self.devices)
        self.shields = 0
        self.torpedoes = Ship.torpedo_capacity
        self.docked: bool = False  # true when docked at starbase

    def refill(self) -> None:
        self.energy = Ship.energy_capacity
        self.torpedoes = Ship.torpedo_capacity

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
        print(f"  'SHIELDS NOW AT {self.shields} UNITS PER YOUR COMMAND.'")


class Quadrant:
    def __init__(
        self,
        point: Point,  # position of the quadrant
        population: QuadrantData,
        ship_position: Position,
    ) -> None:
        """Populate quadrant map"""
        assert 0 <= point.x <= 7 and 0 <= point.y <= 7
        self.name = Quadrant.quadrant_name(point.x, point.y, False)

        self.nb_klingons = population.klingons
        self.nb_bases = population.bases
        self.nb_stars = population.stars

        # extra delay in repairs at base
        self.delay_in_repairs_at_base: float = 0.5 * random.random()

        # Klingons in current quadrant
        self.klingon_ships: List[KlingonShip] = []

        # Initialize empty: save what is at which position
        self.data = [[Entity.void for _ in range(8)] for _ in range(8)]

        self.populate_quadrant(ship_position)

    @classmethod
    def quadrant_name(cls, row: int, col: int, region_only: bool = False) -> str:
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

    def set_value(self, x: float, y: float, entity: Entity) -> None:
        self.data[round(x)][round(y)] = entity

    def get_value(self, x: float, y: float) -> Entity:
        return self.data[round(x)][round(y)]

    def find_empty_place(self) -> Tuple[int, int]:
        """Find an empty location in the current quadrant."""
        while True:
            row, col = fnr(), fnr()
            if self.get_value(row, col) == Entity.void:
                return row, col

    def populate_quadrant(self, ship_position: Position) -> None:
        self.set_value(ship_position.sector.x, ship_position.sector.y, Entity.ship)
        for _ in range(self.nb_klingons):
            x, y = self.find_empty_place()
            self.set_value(x, y, Entity.klingon)
            self.klingon_ships.append(
                KlingonShip(
                    Point(x, y), klingon_shield_strength * (0.5 + random.random())
                )
            )
        if self.nb_bases > 0:
            # Position of starbase in current sector
            starbase_x, starbase_y = self.find_empty_place()
            self.starbase = Point(starbase_x, starbase_y)
            self.set_value(starbase_x, starbase_y, Entity.starbase)
        for _ in range(self.nb_stars):
            x, y = self.find_empty_place()
            self.set_value(x, y, Entity.star)

    def __str__(self) -> str:
        quadrant_string = ""
        for row in self.data:
            for entity in row:
                quadrant_string += entity.value
        return quadrant_string


class World:
    def __init__(
        self,
        total_klingons: int = 0,  # Klingons at start of game
        bases_in_galaxy: int = 0,
    ) -> None:
        self.ship = Ship()
        self.initial_stardate = 100 * random.randint(20, 39)
        self.stardate: float = self.initial_stardate
        self.mission_duration = random.randint(25, 34)

        # Enemy
        self.remaining_klingons = total_klingons

        # Player starbases
        self.bases_in_galaxy = bases_in_galaxy

        self.galaxy_map: List[List[QuadrantData]] = [
            [QuadrantData(0, 0, 0) for _ in range(8)] for _ in range(8)
        ]
        self.charted_galaxy_map: List[List[QuadrantData]] = [
            [QuadrantData(0, 0, 0) for _ in range(8)] for _ in range(8)
        ]

        # initialize contents of galaxy
        for x in range(8):
            for y in range(8):
                r1 = random.random()

                if r1 > 0.98:
                    quadrant_klingons = 3
                elif r1 > 0.95:
                    quadrant_klingons = 2
                elif r1 > 0.80:
                    quadrant_klingons = 1
                else:
                    quadrant_klingons = 0
                self.remaining_klingons += quadrant_klingons

                quadrant_bases = 0
                if random.random() > 0.96:
                    quadrant_bases = 1
                    self.bases_in_galaxy += 1
                self.galaxy_map[x][y] = QuadrantData(
                    quadrant_klingons, quadrant_bases, 1 + fnr()
                )

        if self.remaining_klingons > self.mission_duration:
            self.mission_duration = self.remaining_klingons + 1

        if self.bases_in_galaxy == 0:  # original has buggy extra code here
            self.bases_in_galaxy = 1
            self.galaxy_map[self.ship.position.quadrant.x][
                self.ship.position.quadrant.y
            ].bases += 1

        curr = self.ship.position.quadrant
        self.quadrant = Quadrant(
            self.ship.position.quadrant,
            self.galaxy_map[curr.x][curr.y],
            self.ship.position,
        )

    def remaining_time(self) -> float:
        return self.initial_stardate + self.mission_duration - self.stardate

    def has_mission_ended(self) -> bool:
        return self.remaining_time() < 0


class Game:
    """Handle user actions"""

    def __init__(self) -> None:
        self.restart = False
        self.world = World()

    def startup(self) -> None:
        """Initialize the game variables and map, and print startup messages."""
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
        self.world = World()
        world = self.world
        print(
            "YOUR ORDERS ARE AS FOLLOWS:\n"
            f"     DESTROY THE {world.remaining_klingons} KLINGON WARSHIPS WHICH HAVE INVADED\n"
            "   THE GALAXY BEFORE THEY CAN ATTACK FEDERATION HEADQUARTERS\n"
            f"   ON STARDATE {world.initial_stardate+world.mission_duration}. "
            f" THIS GIVES YOU {world.mission_duration} DAYS. THERE "
            f"{'IS' if world.bases_in_galaxy == 1 else 'ARE'}\n"
            f"   {world.bases_in_galaxy} "
            f"STARBASE{'' if world.bases_in_galaxy == 1 else 'S'} IN THE GALAXY FOR "
            "RESUPPLYING YOUR SHIP.\n"
        )

    def new_quadrant(self) -> None:
        """Enter a new quadrant: populate map and print a short range scan."""
        world = self.world
        ship = world.ship
        q = ship.position.quadrant

        world.quadrant = Quadrant(
            q,
            world.galaxy_map[q.x][q.y],
            ship.position,
        )

        world.charted_galaxy_map[q.x][q.y] = world.galaxy_map[q.x][q.y]

        if world.stardate == world.initial_stardate:
            print("\nYOUR MISSION BEGINS WITH YOUR STARSHIP LOCATED")
            print(f"IN THE GALACTIC QUADRANT, '{world.quadrant.name}'.\n")
        else:
            print(f"\nNOW ENTERING {world.quadrant.name} QUADRANT . . .\n")

        if world.quadrant.nb_klingons != 0:
            print("COMBAT AREA      CONDITION RED")
            if ship.shields <= 200:
                print("   SHIELDS DANGEROUSLY LOW")
        self.short_range_scan()

    def fnd(self, i: int) -> float:
        """Find distance between Enterprise and i'th Klingon warship."""
        ship = self.world.ship.position.sector
        klingons = self.world.quadrant.klingon_ships[i].sector
        return sqrt((klingons.x - ship.x) ** 2 + (klingons.y - ship.y) ** 2)

    def klingons_fire(self) -> None:
        """Process nearby Klingons firing on Enterprise."""
        ship = self.world.ship

        if self.world.quadrant.nb_klingons <= 0:
            return
        if ship.docked:
            print("STARBASE SHIELDS PROTECT THE ENTERPRISE")
            return

        for i, klingon_ship in enumerate(self.world.quadrant.klingon_ships):
            if klingon_ship.shield <= 0:
                continue

            h = int((klingon_ship.shield / self.fnd(i)) * (random.random() + 2))
            ship.shields -= h
            klingon_ship.shield /= random.random() + 3
            print(f" {h} UNIT HIT ON ENTERPRISE FROM SECTOR {klingon_ship.sector} ")
            if ship.shields <= 0:
                self.end_game(won=False, quit=False, enterprise_killed=True)
                return
            print(f"      <SHIELDS DOWN TO {ship.shields} UNITS>")
            if h >= 20 and random.random() < 0.60 and h / ship.shields > 0.02:
                device = fnr()
                ship.damage_stats[device] -= h / ship.shields + 0.5 * random.random()
                print(
                    f"DAMAGE CONTROL REPORTS  '{ship.devices[device]} DAMAGED BY THE HIT'"
                )

    def phaser_control(self) -> None:
        """Take phaser control input and fire phasers."""
        world = self.world
        klingon_ships = world.quadrant.klingon_ships
        ship = world.ship

        if ship.damage_stats[3] < 0:
            print("PHASERS INOPERATIVE")
            return

        if self.world.quadrant.nb_klingons <= 0:
            print("SCIENCE OFFICER SPOCK REPORTS  'SENSORS SHOW NO ENEMY SHIPS")
            print("                                IN THIS QUADRANT'")
            return

        if ship.damage_stats[7] < 0:
            print("COMPUTER FAILURE HAMPERS ACCURACY")

        print(f"PHASERS LOCKED ON TARGET;  ENERGY AVAILABLE = {ship.energy} UNITS")
        phaser_firepower: float = 0
        while True:
            while True:
                units_to_fire = input("NUMBER OF UNITS TO FIRE? ")
                if len(units_to_fire) > 0:
                    phaser_firepower = int(units_to_fire)
                    break
            if phaser_firepower <= 0:
                return
            if ship.energy >= phaser_firepower:
                break
            print(f"ENERGY AVAILABLE = {ship.energy} UNITS")

        ship.energy -= phaser_firepower
        if ship.damage_stats[7] < 0:  # bug in original, was d[6]
            phaser_firepower *= random.random()

        phaser_per_klingon = int(phaser_firepower / self.world.quadrant.nb_klingons)
        for i, klingon_ship in enumerate(klingon_ships):
            if klingon_ship.shield <= 0:
                continue

            h = int((phaser_per_klingon / self.fnd(i)) * (random.random() + 2))
            if h <= 0.15 * klingon_ship.shield:
                print(f"SENSORS SHOW NO DAMAGE TO ENEMY AT {klingon_ship.sector}")
            else:
                klingon_ship.shield -= h
                print(f" {h} UNIT HIT ON KLINGON AT SECTOR {klingon_ship.sector}")
                if klingon_ship.shield <= 0:
                    print("*** KLINGON DESTROYED ***")
                    self.world.quadrant.nb_klingons -= 1
                    world.remaining_klingons -= 1
                    world.quadrant.set_value(
                        klingon_ship.sector.x, klingon_ship.sector.y, Entity.void
                    )
                    klingon_ship.shield = 0
                    world.galaxy_map[ship.position.quadrant.x][
                        ship.position.quadrant.y
                    ].klingons -= 1
                    world.charted_galaxy_map[ship.position.quadrant.x][
                        ship.position.quadrant.y
                    ] = world.galaxy_map[ship.position.quadrant.x][
                        ship.position.quadrant.y
                    ]
                    if world.remaining_klingons <= 0:
                        self.end_game(won=True, quit=False)
                        return
                else:
                    print(
                        f"   (SENSORS SHOW {round(klingon_ship.shield,6)} UNITS REMAINING)"
                    )

        self.klingons_fire()

    def photon_torpedoes(self) -> None:
        """Take photon torpedo input and process firing of torpedoes."""
        world = self.world
        klingon_ships = world.quadrant.klingon_ships
        ship = world.ship

        if ship.torpedoes <= 0:
            print("ALL PHOTON TORPEDOES EXPENDED")
            return
        if ship.damage_stats[4] < 0:
            print("PHOTON TUBES ARE NOT OPERATIONAL")
            return

        cd = get_user_float("PHOTON TORPEDO COURSE (1-9)? ")
        if cd == 9:
            cd = 1
        if cd < 1 or cd >= 9:
            print("ENSIGN CHEKOV REPORTS, 'INCORRECT COURSE DATA, SIR!'")
            return

        cdi = int(cd)

        # Interpolate direction:
        dx = dirs[cdi - 1][0] + (dirs[cdi][0] - dirs[cdi - 1][0]) * (cd - cdi)
        dy = dirs[cdi - 1][1] + (dirs[cdi][1] - dirs[cdi - 1][1]) * (cd - cdi)

        ship.energy -= 2
        ship.torpedoes -= 1

        # Exact position
        x: float = ship.position.sector.x
        y: float = ship.position.sector.y

        # Rounded position (to coordinates)
        torpedo_x, torpedo_y = x, y
        print("TORPEDO TRACK:")
        while True:
            x += dx
            y += dy
            torpedo_x, torpedo_y = round(x), round(y)
            if torpedo_x < 0 or torpedo_x > 7 or torpedo_y < 0 or torpedo_y > 7:
                print("TORPEDO MISSED")
                self.klingons_fire()
                return
            print(f"                {torpedo_x + 1} , {torpedo_y + 1}")
            if world.quadrant.get_value(torpedo_x, torpedo_y) != Entity.void:
                break

        if world.quadrant.get_value(torpedo_x, torpedo_y) == Entity.klingon:
            print("*** KLINGON DESTROYED ***")
            self.world.quadrant.nb_klingons -= 1
            world.remaining_klingons -= 1
            if world.remaining_klingons <= 0:
                self.end_game(won=True, quit=False)
                return
            for klingon_ship in klingon_ships:
                if (
                    torpedo_x == klingon_ship.sector.x
                    and torpedo_y == klingon_ship.sector.y
                ):
                    klingon_ship.shield = 0
        elif world.quadrant.get_value(torpedo_x, torpedo_y) == Entity.star:
            print(f"STAR AT {torpedo_x + 1} , {torpedo_y + 1} ABSORBED TORPEDO ENERGY.")
            self.klingons_fire()
            return
        elif world.quadrant.get_value(torpedo_x, torpedo_y) == Entity.starbase:
            print("*** STARBASE DESTROYED ***")
            self.world.quadrant.nb_bases -= 1
            world.bases_in_galaxy -= 1
            if (
                world.bases_in_galaxy == 0
                and world.remaining_klingons
                <= world.stardate - world.initial_stardate - world.mission_duration
            ):
                print("THAT DOES IT, CAPTAIN!! YOU ARE HEREBY RELIEVED OF COMMAND")
                print("AND SENTENCED TO 99 STARDATES AT HARD LABOR ON CYGNUS 12!!")
                self.end_game(won=False)
                return
            print("STARFLEET COMMAND REVIEWING YOUR RECORD TO CONSIDER")
            print("COURT MARTIAL!")
            ship.docked = False

        world.quadrant.set_value(torpedo_x, torpedo_y, Entity.void)
        world.galaxy_map[ship.position.quadrant.x][
            ship.position.quadrant.y
        ] = QuadrantData(
            self.world.quadrant.nb_klingons,
            self.world.quadrant.nb_bases,
            self.world.quadrant.nb_stars,
        )
        world.charted_galaxy_map[ship.position.quadrant.x][
            ship.position.quadrant.y
        ] = world.galaxy_map[ship.position.quadrant.x][ship.position.quadrant.y]
        self.klingons_fire()

    def short_range_scan(self) -> None:
        """Print a short range scan."""
        self.world.ship.docked = False
        ship = self.world.ship
        for x in (
            ship.position.sector.x - 1,
            ship.position.sector.x,
            ship.position.sector.x + 1,
        ):
            for y in (
                ship.position.sector.y - 1,
                ship.position.sector.y,
                ship.position.sector.y + 1,
            ):
                if (
                    0 <= x <= 7
                    and 0 <= y <= 7
                    and self.world.quadrant.get_value(x, y) == Entity.starbase
                ):
                    ship.docked = True
                    cs = "DOCKED"
                    ship.refill()
                    print("SHIELDS DROPPED FOR DOCKING PURPOSES")
                    ship.shields = 0
                    break
            else:
                continue
            break
        else:
            if self.world.quadrant.nb_klingons > 0:
                cs = "*RED*"
            elif ship.energy < Ship.energy_capacity * 0.1:
                cs = "YELLOW"
            else:
                cs = "GREEN"

        if ship.damage_stats[1] < 0:
            print("\n*** SHORT RANGE SENSORS ARE OUT ***\n")
            return

        sep = "---------------------------------"
        print(sep)
        for x in range(8):
            line = ""
            for y in range(8):
                line = line + " " + self.world.quadrant.data[x][y].value

            if x == 0:
                line += f"        STARDATE           {round(int(self.world.stardate * 10) * 0.1, 1)}"
            elif x == 1:
                line += f"        CONDITION          {cs}"
            elif x == 2:
                line += f"        QUADRANT           {ship.position.quadrant}"
            elif x == 3:
                line += f"        SECTOR             {ship.position.sector}"
            elif x == 4:
                line += f"        PHOTON TORPEDOES   {int(ship.torpedoes)}"
            elif x == 5:
                line += f"        TOTAL ENERGY       {int(ship.energy + ship.shields)}"
            elif x == 6:
                line += f"        SHIELDS            {int(ship.shields)}"
            else:
                line += f"        KLINGONS REMAINING {self.world.remaining_klingons}"

            print(line)
        print(sep)

    def long_range_scan(self) -> None:
        """Print a long range scan."""
        if self.world.ship.damage_stats[2] < 0:
            print("LONG RANGE SENSORS ARE INOPERABLE")
            return

        print(f"LONG RANGE SCAN FOR QUADRANT {self.world.ship.position.quadrant}")
        print_scan_results(
            self.world.ship.position.quadrant,
            self.world.galaxy_map,
            self.world.charted_galaxy_map,
        )

    def navigation(self) -> None:
        """
        Take navigation input and move the Enterprise.

        1/8 warp goes 1 sector in the direction dirs[course]
        """
        world = self.world
        ship = world.ship

        cd = get_user_float("COURSE (1-9)? ") - 1  # Convert to 0-8
        if cd == len(dirs) - 1:
            cd = 0
        if cd < 0 or cd >= len(dirs):
            print("   LT. SULU REPORTS, 'INCORRECT COURSE DATA, SIR!'")
            return

        warp = get_user_float(
            f"WARP FACTOR (0-{'0.2' if ship.damage_stats[0] < 0 else '8'})? "
        )
        if ship.damage_stats[0] < 0 and warp > 0.2:
            print("WARP ENGINES ARE DAMAGED. MAXIMUM SPEED = WARP 0.2")
            return
        if warp == 0:
            return
        if warp < 0 or warp > 8:
            print(
                f"   CHIEF ENGINEER SCOTT REPORTS 'THE ENGINES WON'T TAKE WARP {warp}!'"
            )
            return

        warp_rounds = round(warp * 8)
        if ship.energy < warp_rounds:
            print("ENGINEERING REPORTS   'INSUFFICIENT ENERGY AVAILABLE")
            print(f"                       FOR MANEUVERING AT WARP {warp}!'")
            if ship.shields >= warp_rounds - ship.energy and ship.damage_stats[6] >= 0:
                print(
                    f"DEFLECTOR CONTROL ROOM ACKNOWLEDGES {ship.shields} UNITS OF ENERGY"
                )
                print("                         PRESENTLY DEPLOYED TO SHIELDS.")
            return

        # klingons move and fire
        for klingon_ship in self.world.quadrant.klingon_ships:
            if klingon_ship.shield != 0:
                world.quadrant.set_value(
                    klingon_ship.sector.x, klingon_ship.sector.y, Entity.void
                )
                (
                    klingon_ship.sector.x,
                    klingon_ship.sector.y,
                ) = world.quadrant.find_empty_place()
                world.quadrant.set_value(
                    klingon_ship.sector.x, klingon_ship.sector.y, Entity.klingon
                )

        self.klingons_fire()

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
                    line += f"   {ship.devices[i]} REPAIR COMPLETED\n"
        if len(line) > 0:
            print(line)
        if random.random() <= 0.2:
            device = fnr()
            if random.random() < 0.6:
                ship.damage_stats[device] -= random.random() * 5 + 1
                print(f"DAMAGE CONTROL REPORT:   {ship.devices[device]} DAMAGED\n")
            else:
                ship.damage_stats[device] += random.random() * 3 + 1
                print(
                    f"DAMAGE CONTROL REPORT:   {ship.devices[device]} STATE OF REPAIR IMPROVED\n"
                )

        self.move_ship(warp_rounds, cd)
        world.stardate += 0.1 * int(10 * warp) if warp < 1 else 1
        if world.has_mission_ended():
            self.end_game(won=False, quit=False)
            return

        self.short_range_scan()

    def move_ship(self, warp_rounds: int, cd: float) -> None:
        assert cd >= 0
        assert cd < len(dirs) - 1
        # cd is the course data which points to 'dirs'
        world = self.world
        ship = self.world.ship
        world.quadrant.set_value(
            int(ship.position.sector.x), int(ship.position.sector.y), Entity.void
        )
        cdi = int(cd)

        # Interpolate direction:
        dx = dirs[cdi][0] + (dirs[cdi + 1][0] - dirs[cdi][0]) * (cd - cdi)
        dy = dirs[cdi][1] + (dirs[cdi + 1][1] - dirs[cdi][1]) * (cd - cdi)

        start_quadrant = Point(ship.position.quadrant.x, ship.position.quadrant.y)
        sector_start_x: float = ship.position.sector.x
        sector_start_y: float = ship.position.sector.y

        for _ in range(warp_rounds):
            ship.position.sector.x += dx  # type: ignore
            ship.position.sector.y += dy  # type: ignore

            if (
                ship.position.sector.x < 0
                or ship.position.sector.x > 7
                or ship.position.sector.y < 0
                or ship.position.sector.y > 7
            ):
                # exceeded quadrant limits; calculate final position
                sector_start_x += ship.position.quadrant.x * 8 + warp_rounds * dx
                sector_start_y += ship.position.quadrant.y * 8 + warp_rounds * dy
                ship.position.quadrant.x = int(sector_start_x / 8)
                ship.position.quadrant.y = int(sector_start_y / 8)
                ship.position.sector.x = int(
                    sector_start_x - ship.position.quadrant.x * 8
                )
                ship.position.sector.y = int(
                    sector_start_y - ship.position.quadrant.y * 8
                )
                if ship.position.sector.x < 0:
                    ship.position.quadrant.x -= 1
                    ship.position.sector.x = 7
                if ship.position.sector.y < 0:
                    ship.position.quadrant.y -= 1
                    ship.position.sector.y = 7

                hit_edge = False
                if ship.position.quadrant.x < 0:
                    hit_edge = True
                    ship.position.quadrant.x = ship.position.sector.x = 0
                if ship.position.quadrant.x > 7:
                    hit_edge = True
                    ship.position.quadrant.x = ship.position.sector.x = 7
                if ship.position.quadrant.y < 0:
                    hit_edge = True
                    ship.position.quadrant.y = ship.position.sector.y = 0
                if ship.position.quadrant.y > 7:
                    hit_edge = True
                    ship.position.quadrant.y = ship.position.sector.y = 7
                if hit_edge:
                    print("LT. UHURA REPORTS MESSAGE FROM STARFLEET COMMAND:")
                    print("  'PERMISSION TO ATTEMPT CROSSING OF GALACTIC PERIMETER")
                    print("  IS HEREBY *DENIED*. SHUT DOWN YOUR ENGINES.'")
                    print("CHIEF ENGINEER SCOTT REPORTS  'WARP ENGINES SHUT DOWN")
                    print(
                        f"  AT SECTOR {ship.position.sector} OF "
                        f"QUADRANT {ship.position.quadrant}.'"
                    )
                    if world.has_mission_ended():
                        self.end_game(won=False, quit=False)
                        return

                stayed_in_quadrant = (
                    ship.position.quadrant.x == start_quadrant.x
                    and ship.position.quadrant.y == start_quadrant.y
                )
                if stayed_in_quadrant:
                    break
                world.stardate += 1
                ship.maneuver_energy(warp_rounds)
                self.new_quadrant()
                return
            ship_sector = self.world.ship.position.sector
            ship_x = int(ship_sector.x)
            ship_y = int(ship_sector.y)
            if self.world.quadrant.data[ship_x][ship_y] != Entity.void:
                ship_sector.x = int(ship_sector.x - dx)
                ship_sector.y = int(ship_sector.y - dy)
                print(
                    "WARP ENGINES SHUT DOWN AT SECTOR "
                    f"{ship_sector} DUE TO BAD NAVIGATION"
                )
                break
        else:
            ship.position.sector.x, ship.position.sector.y = int(
                ship.position.sector.x
            ), int(ship.position.sector.y)

        world.quadrant.set_value(
            int(ship.position.sector.x), int(ship.position.sector.y), Entity.ship
        )
        ship.maneuver_energy(warp_rounds)

    def damage_control(self) -> None:
        """Print a damage control report."""
        ship = self.world.ship

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

        damage_sum = sum(0.1 for i in range(8) if ship.damage_stats[i] < 0)
        if damage_sum == 0:
            return

        damage_sum += self.world.quadrant.delay_in_repairs_at_base
        if damage_sum >= 1:
            damage_sum = 0.9
        print("\nTECHNICIANS STANDING BY TO EFFECT REPAIRS TO YOUR SHIP;")
        print(
            f"ESTIMATED TIME TO REPAIR: {round(0.01 * int(100 * damage_sum), 2)} STARDATES"
        )
        if input("WILL YOU AUTHORIZE THE REPAIR ORDER (Y/N)? ").upper().strip() != "Y":
            return

        for i in range(8):
            if ship.damage_stats[i] < 0:
                ship.damage_stats[i] = 0
        self.world.stardate += damage_sum + 0.1

    def computer(self) -> None:
        """Perform the various functions of the library computer."""
        world = self.world
        ship = world.ship

        if ship.damage_stats[7] < 0:
            print("COMPUTER DISABLED")
            return

        while True:
            command = input("COMPUTER ACTIVE AND AWAITING COMMAND? ")
            if len(command) == 0:
                com = 6
            else:
                try:
                    com = int(command)
                except ValueError:
                    com = 6
            if com < 0:
                return

            print()

            if com in [0, 5]:
                if com == 5:
                    print("                        THE GALAXY")
                else:
                    print(
                        "\n        COMPUTER RECORD OF GALAXY FOR "
                        f"QUADRANT {ship.position.quadrant}\n"
                    )

                print("       1     2     3     4     5     6     7     8")
                sep = "     ----- ----- ----- ----- ----- ----- ----- -----"
                print(sep)

                for i in range(8):
                    line = " " + str(i + 1) + " "

                    if com == 5:
                        g2s = Quadrant.quadrant_name(i, 0, True)
                        line += (" " * int(12 - 0.5 * len(g2s))) + g2s
                        g2s = Quadrant.quadrant_name(i, 4, True)
                        line += (" " * int(39 - 0.5 * len(g2s) - len(line))) + g2s
                    else:
                        for j in range(8):
                            line += "   "
                            if world.charted_galaxy_map[i][j].num() == 0:
                                line += "***"
                            else:
                                line += str(
                                    world.charted_galaxy_map[i][j].num() + 1000
                                )[-3:]

                    print(line)
                    print(sep)

                print()
            elif com == 1:
                print("   STATUS REPORT:")
                print(
                    f"KLINGON{'S' if world.remaining_klingons > 1 else ''} LEFT: {world.remaining_klingons}"
                )
                print(
                    "MISSION MUST BE COMPLETED IN "
                    f"{round(0.1 * int(world.remaining_time() * 10), 1)} STARDATES"
                )

                if world.bases_in_galaxy == 0:
                    print("YOUR STUPIDITY HAS LEFT YOU ON YOUR OWN IN")
                    print("  THE GALAXY -- YOU HAVE NO STARBASES LEFT!")
                else:
                    print(
                        f"THE FEDERATION IS MAINTAINING {world.bases_in_galaxy} "
                        f"STARBASE{'S' if world.bases_in_galaxy > 1 else ''} IN THE GALAXY"
                    )

                self.damage_control()
            elif com == 2:
                if self.world.quadrant.nb_klingons <= 0:
                    print(
                        "SCIENCE OFFICER SPOCK REPORTS  'SENSORS SHOW NO ENEMY "
                        "SHIPS\n"
                        "                                IN THIS QUADRANT'"
                    )
                    return

                print(
                    f"FROM ENTERPRISE TO KLINGON BATTLE CRUISER{'S' if self.world.quadrant.nb_klingons > 1 else ''}"
                )

                for klingon_ship in self.world.quadrant.klingon_ships:
                    if klingon_ship.shield > 0:
                        print_direction(
                            Point(ship.position.sector.x, ship.position.sector.y),
                            Point(
                                int(klingon_ship.sector.x),
                                int(klingon_ship.sector.y),
                            ),
                        )
            elif com == 3:
                if self.world.quadrant.nb_bases == 0:
                    print(
                        "MR. SPOCK REPORTS,  'SENSORS SHOW NO STARBASES IN THIS "
                        "QUADRANT.'"
                    )
                    return

                print("FROM ENTERPRISE TO STARBASE:")
                print_direction(
                    Point(ship.position.sector.x, ship.position.sector.y),
                    self.world.quadrant.starbase,
                )
            elif com == 4:
                print("DIRECTION/DISTANCE CALCULATOR:")
                print(
                    f"YOU ARE AT QUADRANT {ship.position.quadrant} "
                    f"SECTOR {ship.position.sector}"
                )
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
                print_direction(Point(from1, from2), Point(to1, to2))
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

    def end_game(
        self, won: bool = False, quit: bool = True, enterprise_killed: bool = False
    ) -> None:
        """Handle end-of-game situations."""
        if won:
            print("CONGRATULATIONS, CAPTAIN! THE LAST KLINGON BATTLE CRUISER")
            print("MENACING THE FEDERATION HAS BEEN DESTROYED.\n")
            print(
                f"YOUR EFFICIENCY RATING IS {round(1000 * (self.world.remaining_klingons / (self.world.stardate - self.world.initial_stardate))**2, 4)}\n\n"
            )
        else:
            if not quit:
                if enterprise_killed:
                    print(
                        "\nTHE ENTERPRISE HAS BEEN DESTROYED. THE FEDERATION "
                        "WILL BE CONQUERED."
                    )
                print(f"IT IS STARDATE {round(self.world.stardate, 1)}")

            print(
                f"THERE WERE {self.world.remaining_klingons} KLINGON BATTLE CRUISERS LEFT AT"
            )
            print("THE END OF YOUR MISSION.\n\n")

            if self.world.bases_in_galaxy == 0:
                sys.exit()

        print("THE FEDERATION IS IN NEED OF A NEW STARSHIP COMMANDER")
        print("FOR A SIMILAR MISSION -- IF THERE IS A VOLUNTEER,")
        if input("LET HIM STEP FORWARD AND ENTER 'AYE'? ").upper().strip() != "AYE":
            sys.exit()
        self.restart = True


klingon_shield_strength: Final = 200
# 8 sectors = 1 quadrant
dirs: Final = [  # (down-up, left,right)
    [0, 1],  # 1: go right (same as #9)
    [-1, 1],  # 2: go up-right
    [-1, 0],  # 3: go up  (lower x-coordines; north)
    [-1, -1],  # 4: go up-left (north-west)
    [0, -1],  # 5: go left (west)
    [1, -1],  # 6: go down-left (south-west)
    [1, 0],  # 7: go down (higher x-coordines; south)
    [1, 1],  # 8: go down-right
    [0, 1],  # 9: go right (east)
]  # vectors in cardinal directions


def fnr() -> int:
    """Generate a random integer from 0 to 7 inclusive."""
    return random.randint(0, 7)


def print_scan_results(
    quadrant: Point,
    galaxy_map: List[List[QuadrantData]],
    charted_galaxy_map: List[List[QuadrantData]],
) -> None:
    sep = "-------------------"
    print(sep)
    for x in (quadrant.x - 1, quadrant.x, quadrant.x + 1):
        n: List[Optional[int]] = [None, None, None]

        # Reveal parts of the current map
        for y in (quadrant.y - 1, quadrant.y, quadrant.y + 1):
            if 0 <= x <= 7 and 0 <= y <= 7:
                n[y - quadrant.y + 1] = galaxy_map[x][y].num()
                charted_galaxy_map[x][y] = galaxy_map[x][y]

        line = ": "
        for line_col in n:
            if line_col is None:
                line += "*** : "
            else:
                line += str(line_col + 1000).rjust(4, " ")[-3:] + " : "
        print(line)
        print(sep)


def print_direction(source: Point, to: Point) -> None:
    """Print direction and distance between two locations in the grid."""
    delta1 = -(to.x - source.x)  # flip so positive is up (heading = 3)
    delta2 = to.y - source.y

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


def main() -> None:
    game = Game()
    world = game.world
    ship = world.ship

    f: Dict[str, Callable[[], None]] = {
        "NAV": game.navigation,
        "SRS": game.short_range_scan,
        "LRS": game.long_range_scan,
        "PHA": game.phaser_control,
        "TOR": game.photon_torpedoes,
        "SHE": ship.shield_control,
        "DAM": game.damage_control,
        "COM": game.computer,
        "XXX": game.end_game,
    }

    while True:
        game.startup()
        game.new_quadrant()
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
