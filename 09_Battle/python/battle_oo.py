#!/usr/bin/env python3
from dataclasses import dataclass
from random import randrange


DESTROYER_LENGTH = 2
CRUISER_LENGTH = 3
AIRCRAFT_CARRIER_LENGTH = 4


@dataclass(frozen=True)
class Point:
    x: int
    y: int

    @classmethod
    def random(cls, start: int, stop: int) -> 'Point':
        return Point(randrange(start, stop), randrange(start, stop))

    def __add__(self, vector: 'Vector') -> 'Point':
        return Point(self.x + vector.x, self.y + vector.y)


@dataclass(frozen=True)
class Vector:
    x: int
    y: int

    @staticmethod
    def random() -> 'Vector':
        return Vector(randrange(-1, 2, 2), randrange(-1, 2, 2))

    def __mul__(self, factor: int) -> 'Vector':
        return Vector(self.x * factor, self.y * factor)


class Sea:
    WIDTH = 6

    def __init__(self):
        self._graph = tuple(([0 for _ in range(self.WIDTH)] for _ in range(self.WIDTH)))

    def _validate_item_indices(self, point: Point) -> None:
        if not isinstance(point, Point):
            raise ValueError(f'Sea indices must be Points, not {type(point).__name__}')

        if not((1 <= point.x <= self.WIDTH) and (1 <= point.y <= self.WIDTH)):
            raise IndexError('Sea index out of range')

    # Allows us to get the value using a point as a key, for example, `sea[Point(3,2)]`
    def __getitem__(self, point: Point) -> int:
        self._validate_item_indices(point)

        return self._graph[point.y - 1][point.x -1]

    # Allows us to get the value using a point as a key, for example, `sea[Point(3,2)] = 3`
    def __setitem__(self, point: Point, value: int) -> None:
        self._validate_item_indices(point)
        self._graph[point.y - 1][point.x -1] = value

    # Allows us to check if a point exists in the sea for example, `if Point(3,2) in sea:`
    def __contains__(self, point: Point) -> bool:
        try:
            self._validate_item_indices(point)
        except IndexError:
            return False

        return True

    # Redefines how python will render this object when asked as a str
    def __str__(self):
        # Display it encoded
        return "\n".join([' '.join([str(self._graph[y][x])
                                    for y in range(self.WIDTH - 1, -1, -1)])
                          for x in range(self.WIDTH)])

    def has_ship(self, ship_code: int) -> bool:
        return any(ship_code in row for row in self._graph)

    def count_sunk(self, *ship_codes: int) -> int:
        return sum(not self.has_ship(ship_code) for ship_code in ship_codes)


class Battle:
    def __init__(self) -> None:
        self.sea = Sea()
        self.place_ship(DESTROYER_LENGTH, 1)
        self.place_ship(DESTROYER_LENGTH, 2)
        self.place_ship(CRUISER_LENGTH, 3)
        self.place_ship(CRUISER_LENGTH, 4)
        self.place_ship(AIRCRAFT_CARRIER_LENGTH, 5)
        self.place_ship(AIRCRAFT_CARRIER_LENGTH, 6)
        self.splashes = 0
        self.hits = 0

    def _next_target(self) -> Point:
        while True:
            try:
                guess = input('? ')
                coordinates = guess.split(',')

                if len(coordinates) != 2:
                    raise ValueError()

                point = Point(int(coordinates[0]), int(coordinates[1]))

                if point not in self.sea:
                    raise ValueError()

                return point
            except ValueError:
                print(f'INVALID. SPECIFY TWO NUMBERS FROM 1 TO {Sea.WIDTH}, SEPARATED BY A COMMA.')

    @property
    def splash_hit_ratio(self) -> str:
        return f'{self.splashes}/{self.hits}'

    @property
    def _is_finished(self) -> bool:
        return self.sea.count_sunk(*(i for i in range(1, 7))) == 6

    def place_ship(self, size: int, ship_code: int) -> None:
        while True:
            start = Point.random(1, self.sea.WIDTH + 1)
            vector = Vector.random()
            # Get potential ship points
            points = [start + vector * i for i in range(size)]

            if not (all([point in self.sea for point in points]) and
                    not any([self.sea[point] for point in points])):
                # ship out of bounds or crosses other ship, trying again
                continue

            # We found a valid spot, so actually place it now
            for point in points:
                self.sea[point] = ship_code

            break


    def loop(self):
        while True:
            target = self._next_target()
            target_value = self.sea[target]

            if target_value < 0:
                print(f'YOU ALREADY PUT A HOLE IN SHIP NUMBER {abs(target_value)} AT THAT POINT.')

            if target_value <= 0:
                print('SPLASH! TRY AGAIN.')
                self.splashes += 1
                continue

            print(f'A DIRECT HIT ON SHIP NUMBER {target_value}')
            self.hits += 1
            self.sea[target] = -target_value

            if not self.sea.has_ship(target_value):
                print('AND YOU SUNK IT. HURRAH FOR THE GOOD GUYS.')
                self._display_sunk_report()

            if self._is_finished:
                self._display_game_end()
                break

            print(f'YOUR CURRENT SPLASH/HIT RATIO IS {self.splash_hit_ratio}')

    def _display_sunk_report(self):
        print('SO FAR, THE BAD GUYS HAVE LOST',
              f'{self.sea.count_sunk(1, 2)} DESTROYER(S),',
              f'{self.sea.count_sunk(3, 4)} CRUISER(S),',
              f'AND {self.sea.count_sunk(5, 6)} AIRCRAFT CARRIER(S).')

    def _display_game_end(self):
        print('YOU HAVE TOTALLY WIPED OUT THE BAD GUYS\' FLEET '
              f'WITH A FINAL SPLASH/HIT RATIO OF {self.splash_hit_ratio}')

        if not self.splashes:
            print('CONGRATULATIONS -- A DIRECT HIT EVERY TIME.')

        print("\n****************************")


def main() -> None:
    game = Battle()
    print(f'''
                BATTLE
CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY

THE FOLLOWING CODE OF THE BAD GUYS' FLEET DISPOSITION
HAS BEEN CAPTURED BUT NOT DECODED:

{game.sea}

DE-CODE IT AND USE IT IF YOU CAN
BUT KEEP THE DE-CODING METHOD A SECRET.

START GAME''')
    game.loop()


if __name__ == "__main__":
    main()
