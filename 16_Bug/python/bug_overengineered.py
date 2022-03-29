"""
BUG (overengineered)

Overengineered version of bug game
Demonstrates function-based Model View Controller pattern

Ported by Peter Sharp
"""

from collections import namedtuple
from random import randint
from typing import Any, Callable, Dict, List, Literal, Optional, Tuple, TypedDict, Union

PAGE_WIDTH = 64

OneParamFunc = Callable[[Any], Any]
TwoParamFunc = Callable[[Any, Any], Any]
StateFunctions = Tuple[OneParamFunc, OneParamFunc, TwoParamFunc]
Action = Literal["instructions", "game", "pictures", "won", "start", "exit"]

Bodypart = namedtuple("Bodypart", ["name", "count", "depends"])

# body part types used by the game to work out whether a player's body part can be added
part_types = (
    Bodypart(name="BODY", count=1, depends=None),
    Bodypart(name="NECK", count=1, depends=0),
    Bodypart(name="HEAD", count=1, depends=1),
    Bodypart(name="FEELERS", count=2, depends=2),
    Bodypart(name="TAIL", count=1, depends=0),
    Bodypart(name="LEGS", count=6, depends=0),
)


class DataDict(TypedDict):
    state: Action
    partNo: Optional[Any]
    players: Dict[str, List[int]]
    partTypes: Tuple[Bodypart, ...]
    finished: List[Any]
    logs: List[Any]


def game_loop(states: Dict[Action, StateFunctions], data: DataDict) -> None:
    """
    Starts the game loop using given states and data

    Uses a modified version of the MVC (Model View Controller) pattern that uses functions instead of objects

    each state in the game has one of each of the following:
    View, displays data
    Control, converts raw command from user into something the model understands
    Model, updates game data based on action received from controller
    """

    while True:
        if data["state"] == "exit":
            break
        view, control, model = states[data["state"]]
        cmd = view(data)
        action = control(cmd)
        data = model(data, action)


def print_start(_: Any) -> str:
    """
    Prints start message
    """
    print_centered("BUG")
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()
    print("THE GAME BUG")
    print("I HOPE YOU ENJOY THIS GAME.")
    print()
    return input("DO YOU WANT INSTRUCTIONS? ")


def control_start(cmd: str) -> str:
    """
    Controls the start state
    """
    if cmd.lower() in ("y", "yes"):
        action = "instructions"
    else:
        action = "game"
    return action


def print_instructions(data: DataDict) -> str:
    """
    Prints game instructions
    """
    print("THE OBJECT OF BUG IS TO FINISH YOUR BUG BEFORE I FINISH")
    print("MINE. EACH NUMBER STANDS FOR A PART OF THE BUG BODY.")
    print("I WILL ROLL THE DIE FOR YOU, TELL YOU WHAT I ROLLED FOR YOU")
    print("WHAT THE NUMBER STANDS FOR, AND IF YOU CAN GET THE PART.")
    print("IF YOU CAN GET THE PART I WILL GIVE IT TO YOU.")
    print("THE SAME WILL HAPPEN ON MY TURN.")
    print("IF THERE IS A CHANGE IN EITHER BUG I WILL GIVE YOU THE")
    print("OPTION OF SEEING THE PICTURES OF THE BUGS.")
    print("THE NUMBERS STAND FOR PARTS AS FOLLOWS:")

    print_table(
        [
            ("NUMBER", "PART", "NUMBER OF PART NEEDED"),
            *[
                (i + 1, part.name, part.count)
                for i, part in enumerate(data["partTypes"])
            ],
        ]
    )
    print()
    print()
    return ""


def goto_game(_: Any) -> Literal["game"]:
    return "game"


def update_state(data: DataDict, action: Action) -> DataDict:
    """
    sets game state to given player value
    """
    return {**data, "state": action}  # type: ignore


def update_game(data: DataDict, action: Action) -> DataDict:
    """
    Updates game data for player turns until one player successfully gets a body part
    """
    # stores logs of what happened during a particular round
    Log1 = Tuple[str, int, Any]
    Log2 = Tuple[str, int, Any, Any]
    logs: List[Union[Log1, Log2]] = []

    if action == "pictures":
        data["state"] = "pictures"
    else:
        part_added = False
        while not part_added:
            for player, parts in data["players"].items():
                # rolls the dice for a part
                new_part_idx = randint(1, 6) - 1

                # gets information about the picked part
                part_type = data["partTypes"][new_part_idx]

                # gets the number of existing parts of that type the player has
                part_count = parts[new_part_idx]

                logs.append(("rolled", new_part_idx, player))

                # a new part can only be added if the player has the parts
                # the new part depends on and doesn't have enough of the part already
                overMaxParts = part_type.count < part_count + 1
                missing_part_dep = (
                    part_type.depends is not None and parts[part_type.depends] == 0
                )

                if not overMaxParts and not missing_part_dep:
                    # adds a new part
                    part_count += 1
                    logs.append(("added", new_part_idx, player))
                    part_added = True
                elif missing_part_dep:
                    logs.append(("missingDep", new_part_idx, player, part_type.depends))
                if overMaxParts:
                    logs.append(("overMax", new_part_idx, player, part_count))

                data["players"][player][new_part_idx] = part_count
    data["logs"] = logs

    # checks if any players have finished their bug
    finished = get_finished(data)
    if len(finished) > 0:
        # and sets the state to 'won' if that's the case
        data["finished"] = finished
        data["state"] = "won"
    return data


def get_finished(data: DataDict) -> List[str]:
    """
    Gets players who have finished their bugs
    """
    total_parts = sum(part_type.count for part_type in data["partTypes"])
    finished = []
    for player, parts in data["players"].items():
        if sum(parts) == total_parts:
            finished.append(player)
    return finished


def print_game(data: DataDict) -> str:
    """
    Displays the results of the game turn
    """
    for log in data["logs"]:
        code, part_idx, player, *logdata = log
        part_type = data["partTypes"][part_idx]

        if code == "rolled":
            print()
            print(f"{player} ROLLED A {part_idx + 1}")
            print(f"{part_idx + 1}={part_type.name}")

        elif code == "added":
            if player == "YOU":
                if part_type.name in ["FEELERS", "LEGS", "TAIL"]:
                    print(f"I NOW GIVE YOU A {part_type.name.replace('s', '')}.")
                else:
                    print(f"YOU NOW HAVE A {part_type.name}.")
            elif player == "I":
                if part_type.name in ["BODY", "NECK", "TAIL"]:
                    print(f"I NOW HAVE A {part_type.name}.")
                elif part_type.name == "FEELERS":
                    print("I GET A FEELER.")

            if part_type.count > 2:
                print(
                    f"{player} NOW HAVE {data['players'][player][part_idx]} {part_type.name}"
                )

        elif code == "missingDep":
            (dep_idx,) = logdata
            dep = data["partTypes"][dep_idx]
            print(
                f"YOU DO NOT HAVE A {dep.name}"
                if player == "YOU"
                else f"I NEEDED A {dep.name}"
            )

        elif code == "overMax":
            (part_count,) = logdata
            if part_count > 1:
                num = "TWO" if part_count == 2 else part_count
                maxMsg = f"HAVE {num} {part_type.name}S ALREADY"
            else:
                maxMsg = f"ALREADY HAVE A {part_type.name}"
            print(f"{player} {maxMsg}")

    return input("DO YOU WANT THE PICTURES? ") if len(data["logs"]) else "n"


def print_pictures(data: DataDict) -> None:
    """
    Displays what the bugs look like for each player
    """
    typeIxs = {part_type.name: idx for idx, part_type in enumerate(data["partTypes"])}
    PIC_WIDTH = 22
    for player, parts in data["players"].items():
        print(f"*****{'YOUR' if player == 'YOU' else 'MY'} BUG*****")
        print()
        print()
        if parts[typeIxs["BODY"]] > 0:
            if parts[typeIxs["FEELERS"]] > 0:
                F = " ".join(["F"] * parts[typeIxs["FEELERS"]])
                for _ in range(4):
                    print(" " * 9 + F)
            if parts[typeIxs["HEAD"]] > 0:
                print_centered("HHHHHHH", PIC_WIDTH)
                print_centered("H     H", PIC_WIDTH)
                print_centered("H O O H", PIC_WIDTH)
                print_centered("H     H", PIC_WIDTH)
                print_centered("H  V  H", PIC_WIDTH)
                print_centered("HHHHHHH", PIC_WIDTH)
            if parts[typeIxs["NECK"]] > 0:
                for _ in range(2):
                    print_centered("N N", PIC_WIDTH)
            print_centered("BBBBBBBBBBBB", PIC_WIDTH)
            for _ in range(2):
                print_centered("B          B", PIC_WIDTH)

            if parts[typeIxs["TAIL"]] > 0:
                print("TTTTTB          B")
            print_centered("BBBBBBBBBBBB", PIC_WIDTH)
            if parts[typeIxs["LEGS"]] > 0:
                L = "L" * parts[typeIxs["LEGS"]]
                for _ in range(2):
                    print(" " * 5 + L)
        print()


def control_game(cmd: str) -> Literal["pictures", "game"]:
    """
    returns state based on command
    """
    if cmd.lower() in ("y", "yes"):
        action = "pictures"
    else:
        action = "game"
    return action  # type: ignore


def print_winner(data: DataDict) -> None:
    """
    Displays the winning message
    """
    for player in data["finished"]:
        print(f"{'YOUR' if player == 'YOU' else 'MY'} BUG IS FINISHED.")
    print("I HOPE YOU ENJOYED THE GAME, PLAY IT AGAIN SOON!!")


def exit_game(_: Any) -> Literal["exit"]:
    """Exist the game regardless of input"""
    return "exit"


def print_centered(msg: str, width: int = PAGE_WIDTH) -> None:
    """Print given message centered to given width."""
    spaces = " " * ((width - len(msg)) // 2)
    print(spaces + msg)


def print_table(rows: List[Any]) -> None:
    for row in rows:
        print(*row, sep="\t")


def main() -> None:
    # The main states in the game
    states: Dict[Action, StateFunctions] = {
        # Initial state of the game
        "start": (print_start, control_start, update_state),
        # displays game instructions
        "instructions": (print_instructions, goto_game, update_state),
        # the main game state
        "game": (print_game, control_game, update_game),
        # displays pictures before returning to game
        "pictures": (print_pictures, goto_game, update_state),
        # Displays the winning players and message
        "won": (print_winner, exit_game, update_state),
    }

    # all the data used by the game
    data = DataDict(
        state="start",
        partNo=None,
        players={"YOU": [0] * len(part_types), "I": [0] * len(part_types)},
        partTypes=part_types,
        finished=[],
        logs=[],
    )
    game_loop(states, data)


if __name__ == "__main__":
    main()
