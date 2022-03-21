import random
import enum
from typing import List, Tuple
from dataclasses import dataclass

# Python translation by Frank Palazzolo - 2/2021


class Maze:
    def __init__(
        self,
        width: int,
        length: int,
    ):
        assert width >= 2 and length >= 2
        used: List[List[int]] = []
        walls: List[List[int]] = []
        for _ in range(length):
            used.append([0] * width)
            walls.append([0] * width)

        # Pick a random entrance, mark as used
        enter_col = random.randint(0, width - 1)
        used[0][enter_col] = 1

        self.used = used
        self.walls = walls
        self.enter_col = enter_col
        self.width = width
        self.length = length

    def add_exit(self) -> None:
        """Modifies 'walls' to add an exit to the maze."""
        col = random.randint(0, self.width - 1)
        row = self.length - 1
        self.walls[row][col] = self.walls[row][col] + 1

    def display(self) -> None:
        for col in range(self.width):
            if col == self.enter_col:
                print(".  ", end="")
            else:
                print(".--", end="")
        print(".")
        for row in range(self.length):
            print("I", end="")
            for col in range(self.width):
                if self.walls[row][col] < 2:
                    print("  I", end="")
                else:
                    print("   ", end="")
            print()
            for col in range(self.width):
                if self.walls[row][col] == 0 or self.walls[row][col] == 2:
                    print(":--", end="")
                else:
                    print(":  ", end="")
            print(".")


class Direction(enum.Enum):
    LEFT = 0
    UP = 1
    RIGHT = 2
    DOWN = 3


@dataclass
class Position:
    row: int
    col: int


# Give Exit directions nice names
EXIT_DOWN = 1
EXIT_RIGHT = 2


def main() -> None:
    welcome_header()
    width, length = get_maze_dimensions()
    maze = build_maze(width, length)
    maze.display()


def welcome_header() -> None:
    print(" " * 28 + "AMAZING PROGRAM")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def build_maze(width: int, length: int) -> Maze:
    """Build two 2D arrays."""
    #
    # used:
    #   Initially set to zero, unprocessed cells
    #   Filled in with consecutive non-zero numbers as cells are processed
    #
    # walls:
    #   Initially set to zero, (all paths blocked)
    #   Remains 0 if there is no exit down or right
    #   Set to 1 if there is an exit down
    #   Set to 2 if there is an exit right
    #   Set to 3 if there are exits down and right
    assert width >= 2 and length >= 2

    maze = Maze(width, length)
    position = Position(row=0, col=maze.enter_col)
    count = 2

    while count != width * length + 1:
        possible_dirs = get_possible_directions(maze, position)

        # If we can move in a direction, move and make opening
        if len(possible_dirs) != 0:
            position, count = make_opening(maze, possible_dirs, position, count)
        # otherwise, move to the next used cell, and try again
        else:
            while True:
                if position.col != width - 1:
                    position.col = position.col + 1
                elif position.row != length - 1:
                    position.row, position.col = position.row + 1, 0
                else:
                    position.row, position.col = 0, 0
                if maze.used[position.row][position.col] != 0:
                    break

    maze.add_exit()
    return maze


def make_opening(
    maze: Maze,
    possible_dirs: List[Direction],
    pos: Position,
    count: int,
) -> Tuple[Position, int]:
    """
    Attention! This modifies 'used' and 'walls'
    """
    direction = random.choice(possible_dirs)
    if direction == Direction.LEFT:
        pos.col = pos.col - 1
        maze.walls[pos.row][pos.col] = EXIT_RIGHT
    elif direction == Direction.UP:
        pos.row = pos.row - 1
        maze.walls[pos.row][pos.col] = EXIT_DOWN
    elif direction == Direction.RIGHT:
        maze.walls[pos.row][pos.col] = maze.walls[pos.row][pos.col] + EXIT_RIGHT
        pos.col = pos.col + 1
    elif direction == Direction.DOWN:
        maze.walls[pos.row][pos.col] = maze.walls[pos.row][pos.col] + EXIT_DOWN
        pos.row = pos.row + 1
    maze.used[pos.row][pos.col] = count
    count = count + 1
    return pos, count


def get_possible_directions(maze: Maze, pos: Position) -> List[Direction]:
    """
    Get a list of all directions that are not blocked.

    Also ignore hit cells that we have already processed
    """
    possible_dirs = list(Direction)
    if pos.col == 0 or maze.used[pos.row][pos.col - 1] != 0:
        possible_dirs.remove(Direction.LEFT)
    if pos.row == 0 or maze.used[pos.row - 1][pos.col] != 0:
        possible_dirs.remove(Direction.UP)
    if pos.col == maze.width - 1 or maze.used[pos.row][pos.col + 1] != 0:
        possible_dirs.remove(Direction.RIGHT)
    if pos.row == maze.length - 1 or maze.used[pos.row + 1][pos.col] != 0:
        possible_dirs.remove(Direction.DOWN)
    return possible_dirs


def get_maze_dimensions() -> Tuple[int, int]:
    while True:
        input_str = input("What are your width and length?")
        if input_str.count(",") == 1:
            width_str, length_str = input_str.split(",")
            width = int(width_str)
            length = int(length_str)
            if width > 1 and length > 1:
                break
        print("Meaningless dimensions. Try again.")
    return width, length


if __name__ == "__main__":
    main()
