#!/usr/bin/env python3
from random import randrange
from typing import List, Tuple

PointType = Tuple[int, int]
VectorType = PointType
SeaType = Tuple[List[int], ...]

SEA_WIDTH = 6
DESTROYER_LENGTH = 2
CRUISER_LENGTH = 3
AIRCRAFT_CARRIER_LENGTH = 4


def random_vector() -> Tuple[int, int]:
    while True:
        vector = (randrange(-1, 2), randrange(-1, 2))

        if vector == (0, 0):
            # We can't have a zero vector, so try again
            continue

        return vector


def add_vector(point: PointType, vector: VectorType) -> PointType:
    return (point[0] + vector[0], point[1] + vector[1])


def place_ship(sea: SeaType, size: int, code: int) -> None:
    while True:
        start = (randrange(1, SEA_WIDTH + 1), randrange(1, SEA_WIDTH + 1))
        vector = random_vector()

        # Get potential ship points
        point = start
        points = []

        for _ in range(size):
            point = add_vector(point, vector)
            points.append(point)

        if not (all([is_within_sea(point, sea) for point in points]) and
                all([value_at(point, sea) == 0  for point in points])):
            # ship out of bounds or crosses other ship, trying again
            continue

        # We found a valid spot, so actually place it now
        for point in points:
            set_value_at(code, point, sea)

        break


def print_encoded_sea(sea: SeaType) -> None:
    for x in range(len(sea)):
        print(' '.join([str(sea[y][x]) for y in range(len(sea) - 1, -1, -1)]))


def is_within_sea(point: PointType, sea: SeaType) -> bool:
    return (1 <= point[0] <= len(sea)) and (1 <= point[1] <= len(sea))


def has_ship(sea: SeaType, code: int) -> bool:
    return any(code in row for row in sea)


def count_sunk(sea: SeaType, codes: Tuple[int, ...]) -> int:
    return sum(not has_ship(sea, code) for code in codes)


def value_at(point: PointType, sea: SeaType) -> int:
    return sea[point[1] - 1][point[0] -1]


def set_value_at(value: int, point: PointType, sea: SeaType) -> None:
    sea[point[1] - 1][point[0] -1] = value


def get_next_target(sea: SeaType) -> PointType:
    while True:
        try:
            guess = input('? ')
            point = guess.split(',')

            if len(point) != 2:
                raise ValueError()

            point = (int(point[0]), int(point[1]))

            if not is_within_sea(point, sea):
                raise ValueError()

            return point
        except ValueError:
            print(f'INVALID. SPECIFY TWO NUMBERS FROM 1 TO {len(sea)}, SEPARATED BY A COMMA.')


def setup_ships(sea: SeaType):
    place_ship(sea, DESTROYER_LENGTH, 1)
    place_ship(sea, DESTROYER_LENGTH, 2)
    place_ship(sea, CRUISER_LENGTH, 3)
    place_ship(sea, CRUISER_LENGTH, 4)
    place_ship(sea, AIRCRAFT_CARRIER_LENGTH, 5)
    place_ship(sea, AIRCRAFT_CARRIER_LENGTH, 6)


def main() -> None:
    print('                BATTLE')
    print('CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY')
    print()
    sea = tuple(([0 for _ in range(SEA_WIDTH)] for _ in range(SEA_WIDTH)))
    setup_ships(sea)
    print('THE FOLLOWING CODE OF THE BAD GUYS\' FLEET DISPOSITION')
    print('HAS BEEN CAPTURED BUT NOT DECODED:')
    print()
    print_encoded_sea(sea)
    print()
    print('DE-CODE IT AND USE IT IF YOU CAN')
    print('BUT KEEP THE DE-CODING METHOD A SECRET.')
    print()
    print('START GAME')
    splashes = 0
    hits = 0

    while True:
        target = get_next_target(sea)
        target_value = value_at(target, sea)

        if target_value < 0:
            print(f'YOU ALREADY PUT A HOLE IN SHIP NUMBER {abs(target_value)} AT THAT POINT.')

        if target_value <= 0:
            print('SPLASH! TRY AGAIN.')
            splashes += 1
            continue

        print(f'A DIRECT HIT ON SHIP NUMBER {target_value}')
        hits += 1
        set_value_at(-target_value, target, sea)

        if not has_ship(sea, target_value):
            print('AND YOU SUNK IT. HURRAH FOR THE GOOD GUYS.')
            print('SO FAR, THE BAD GUYS HAVE LOST')
            print(f'{count_sunk(sea, (1, 2))} DESTROYER(S),',
                  f'{count_sunk(sea, (3, 4))} CRUISER(S),',
                  f'AND {count_sunk(sea, (5, 6))} AIRCRAFT CARRIER(S).')

        if any(has_ship(sea, code) for code in range(1, SEA_WIDTH + 1)):
            print(f'YOUR CURRENT SPLASH/HIT RATIO IS {splashes}/{hits}')
            continue

        print('YOU HAVE TOTALLY WIPED OUT THE BAD GUYS\' FLEET '
              f'WITH A FINAL SPLASH/HIT RATIO OF {splashes}/{hits}')

        if not splashes:
            print('CONGRATULATIONS -- A DIRECT HIT EVERY TIME.')

        print()
        print('****************************')
        break


if __name__ == "__main__":
    main()
