"""
TARGET

Weapon targeting simulation / 3d trigonometry practice

Ported by Dave LeCompte
"""

import math
import random
from typing import List

PAGE_WIDTH = 64


def print_centered(msg: str) -> None:
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title: str) -> None:
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def print_instructions() -> None:
    print("YOU ARE THE WEAPONS OFFICER ON THE STARSHIP ENTERPRISE")
    print("AND THIS IS A TEST TO SEE HOW ACCURATE A SHOT YOU")
    print("ARE IN A THREE-DIMENSIONAL RANGE.  YOU WILL BE TOLD")
    print("THE RADIAN OFFSET FOR THE X AND Z AXES, THE LOCATION")
    print("OF THE TARGET IN THREE DIMENSIONAL RECTANGULAR COORDINATES,")
    print("THE APPROXIMATE NUMBER OF DEGREES FROM THE X AND Z")
    print("AXES, AND THE APPROXIMATE DISTANCE TO THE TARGET.")
    print("YOU WILL THEN PROCEEED TO SHOOT AT THE TARGET UNTIL IT IS")
    print("DESTROYED!")
    print()
    print("GOOD LUCK!!")
    print()
    print()


def prompt() -> List[float]:
    while True:
        response = input("INPUT ANGLE DEVIATION FROM X, DEVIATION FROM Z, DISTANCE? ")
        if "," not in response:
            continue

        terms = response.split(",")
        if len(terms) != 3:
            continue

        return [float(t) for t in terms]


def next_target() -> None:
    for _ in range(5):
        print()
    print("NEXT TARGET...")
    print()


def describe_miss(x, y, z, x1, y1, z1, d) -> None:
    x2 = x1 - x
    y2 = y1 - y
    z2 = z1 - z

    if x2 < 0:
        print(f"SHOT BEHIND TARGET {-x2:.2f} KILOMETERS.")
    else:
        print(f"SHOT IN FRONT OF TARGET {x2:.2f} KILOMETERS.")

    if y2 < 0:
        print(f"SHOT TO RIGHT OF TARGET {-y2:.2f} KILOMETERS.")
    else:
        print(f"SHOT TO LEFT OF TARGET {y2:.2f} KILOMETERS.")

    if z2 < 0:
        print(f"SHOT BELOW TARGET {-z2:.2f} KILOMETERS.")
    else:
        print(f"SHOT ABOVE TARGET {z2:.2f} KILOMETERS.")

    print(f"APPROX POSITION OF EXPLOSION:  X={x1:.4f}   Y={y1:.4f}   Z={z1:.4f}")
    print(f"     DISTANCE FROM TARGET = {d:.2f}")
    print()
    print()
    print()


def do_shot_loop(p1, x, y, z) -> None:
    shot_count = 0
    while True:
        shot_count += 1
        if shot_count == 1:
            p3 = int(p1 * 0.05) * 20
        elif shot_count == 2:
            p3 = int(p1 * 0.1) * 10
        elif shot_count == 3:
            p3 = int(p1 * 0.5) * 2
        elif shot_count == 4:
            p3 = int(p1)
        else:
            p3 = p1

        if p3 == int(p3):
            print(f"     ESTIMATED DISTANCE: {p3}")
        else:
            print(f"     ESTIMATED DISTANCE: {p3:.2f}")
        print()
        a1, b1, p2 = prompt()

        if p2 < 20:
            print("YOU BLEW YOURSELF UP!!")
            return

        a1 = math.radians(a1)
        b1 = math.radians(b1)
        show_radians(a1, b1)

        x1 = p2 * math.sin(b1) * math.cos(a1)
        y1 = p2 * math.sin(b1) * math.sin(a1)
        z1 = p2 * math.cos(b1)

        distance = math.sqrt((x1 - x) ** 2 + (y1 - y) ** 2 + (z1 - z) ** 2)

        if distance <= 20:
            print()
            print(" * * * HIT * * *   TARGET IS NON FUNCTIONAL")
            print()
            print(f"DISTANCE OF EXPLOSION FROM TARGET WAS {distance:.4f} KILOMETERS")
            print()
            print(f"MISSION ACCOMPLISHED IN {shot_count} SHOTS.")

            return
        else:
            describe_miss(x, y, z, x1, y1, z1, distance)


def show_radians(a, b) -> None:
    print(f"RADIANS FROM X AXIS = {a:.4f}   FROM Z AXIS = {b:.4f}")


def play_game() -> None:
    while True:
        a = random.uniform(0, 2 * math.pi)  # random angle
        b = random.uniform(0, 2 * math.pi)  # random angle

        show_radians(a, b)

        p1 = random.uniform(0, 100000) + random.uniform(0, 1)
        x = math.sin(b) * math.cos(a) * p1
        y = math.sin(b) * math.sin(a) * p1
        z = math.cos(b) * p1
        print(
            f"TARGET SIGHTED: APPROXIMATE COORDINATES:  X={x:.1f}  Y={y:.1f}  Z={z:.1f}"
        )

        do_shot_loop(p1, x, y, z)
        next_target()


def main() -> None:
    print_header("TARGET")
    print_instructions()

    play_game()


if __name__ == "__main__":
    main()
