import math
import random
import time


def basic_print(*zones, **kwargs) -> None:
    """Simulates the PRINT command from BASIC to some degree.
    Supports `printing zones` if given multiple arguments."""

    line = ""
    if len(zones) == 1:
        line = str(zones[0])
    else:
        line = "".join([f"{str(zone):<14}" for zone in zones])
    identation = kwargs.get("indent", 0)
    end = kwargs.get("end", "\n")
    print(" " * identation + line, end=end)


def basic_input(prompt, type_conversion=None):
    """BASIC INPUT command with optional type conversion"""

    while True:
        try:
            inp = input(f"{prompt}? ")
            if type_conversion is not None:
                inp = type_conversion(inp)
            break
        except ValueError:
            basic_print("INVALID INPUT!")
    return inp


# horse names do not change over the program, therefore making it a global.
# throught the game, the ordering of the horses is used to indentify them
HORSE_NAMES = [
    "JOE MAW",
    "L.B.J.",
    "MR.WASHBURN",
    "MISS KAREN",
    "JOLLY",
    "HORSE",
    "JELLY DO NOT",
    "MIDNIGHT",
]


def introduction():
    """Print the introduction, and optional the instructions"""

    basic_print("HORSERACE", indent=31)
    basic_print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", indent=15)
    basic_print("\n\n")
    basic_print("WELCOME TO SOUTH PORTLAND HIGH RACETRACK")
    basic_print("                      ...OWNED BY LAURIE CHEVALIER")
    y_n = basic_input("DO YOU WANT DIRECTIONS")

    # if no instructions needed, return
    if y_n.upper() == "NO":
        return

    basic_print("UP TO 10 MAY PLAY.  A TABLE OF ODDS WILL BE PRINTED.  YOU")
    basic_print("MAY BET ANY + AMOUNT UNDER 100000 ON ONE HORSE.")
    basic_print("DURING THE RACE, A HORSE WILL BE SHOWN BY ITS")
    basic_print("NUMBER.  THE HORSES RACE DOWN THE PAPER!")
    basic_print("")


def setup_players():
    """Gather the number of players and their names"""

    # ensure we get an integer value from the user
    number_of_players = basic_input("HOW MANY WANT TO BET", int)

    # for each user query their name and return the list of names
    player_names = []
    basic_print("WHEN ? APPEARS,TYPE NAME")
    for _ in range(number_of_players):
        player_names.append(basic_input(""))
    return player_names


def setup_horses():
    """Generates random odds for each horse. Returns a list of
    odds, indexed by the order of the global HORSE_NAMES."""

    odds = [random.randrange(1, 10) for _ in HORSE_NAMES]
    total = sum(odds)

    # rounding odds to two decimals for nicer output,
    # this is not in the origin implementation
    return [round(total / odd, 2) for odd in odds]


def print_horse_odds(odds) -> None:
    """Print the odds for each horse"""

    basic_print("")
    for i in range(len(HORSE_NAMES)):
        basic_print(HORSE_NAMES[i], i, f"{odds[i]}:1")
    basic_print("")


def get_bets(player_names):
    """For each player, get the number of the horse to bet on,
    as well as the amount of money to bet"""

    basic_print("--------------------------------------------------")
    basic_print("PLACE YOUR BETS...HORSE # THEN AMOUNT")

    bets = []
    for name in player_names:
        horse = basic_input(name, int)
        amount = None
        while amount is None:
            amount = basic_input("", float)
            if amount < 1 or amount >= 100000:
                basic_print("  YOU CAN'T DO THAT!")
                amount = None
        bets.append((horse, amount))

    basic_print("")

    return bets


def get_distance(odd):
    """Advances a horse during one step of the racing simulation.
    The amount travelled is random, but scaled by the odds of the horse"""

    d = random.randrange(1, 100)
    s = math.ceil(odd)
    if d < 10:
        return 1
    elif d < s + 17:
        return 2
    elif d < s + 37:
        return 3
    elif d < s + 57:
        return 4
    elif d < s + 77:
        return 5
    elif d < s + 92:
        return 6
    else:
        return 7


def print_race_state(total_distance, race_pos) -> None:
    """Outputs the current state/stop of the race.
    Each horse is placed according to the distance they have travelled. In
    case some horses travelled the same distance, their numbers are printed
    on the same name"""

    # we dont want to modify the `race_pos` list, since we need
    # it later. Therefore we generating an interator from the list
    race_pos_iter = iter(race_pos)

    # race_pos is stored by last to first horse in the race.
    # we get the next horse we need to print out
    next_pos = next(race_pos_iter)

    # start line
    basic_print("XXXXSTARTXXXX")

    # print all 28 lines/unit of the race course
    for line in range(28):

        # ensure we still have a horse to print and if so, check if the
        # next horse to print is not the current line
        # needs iteration, since multiple horses can share the same line
        while next_pos is not None and line == total_distance[next_pos]:
            basic_print(f"{next_pos} ", end="")
            next_pos = next(race_pos_iter, None)
        else:
            # if no horses are left to print for this line, print a new line
            basic_print("")

    # finish line
    basic_print("XXXXFINISHXXXX")


def simulate_race(odds):
    num_horses = len(HORSE_NAMES)

    # in spirit of the original implementation, using two arrays to
    # track the total distance travelled, and create an index from
    # race position -> horse index
    total_distance = [0] * num_horses

    # race_pos maps from the position in the race, to the index of the horse
    # it will later be sorted from last to first horse, based on the
    # distance travelled by each horse.
    # e.g. race_pos[0] => last horse
    #      race_pos[-1] => winning horse
    race_pos = list(range(num_horses))

    basic_print("\n1 2 3 4 5 6 7 8")

    while True:

        # advance each horse by a random amount
        for i in range(num_horses):
            total_distance[i] += get_distance(odds[i])

        # bubble sort race_pos based on total distance travelled
        # in the original implementation, race_pos is reset for each
        # simulation step, so we keep this behaviour here
        race_pos = list(range(num_horses))
        for line in range(num_horses):
            for i in range(num_horses - 1 - line):
                if total_distance[race_pos[i]] < total_distance[race_pos[i + 1]]:
                    continue
                race_pos[i], race_pos[i + 1] = race_pos[i + 1], race_pos[i]

        # print current state of the race
        print_race_state(total_distance, race_pos)

        # goal line is defined as 28 units from start
        # check if the winning horse is already over the finish line
        if total_distance[race_pos[-1]] >= 28:
            return race_pos

        # this was not in the original BASIC implementation, but it makes the
        # race visualization a nice animation (if the terminal size is set to 31 rows)
        time.sleep(1)


def print_race_results(race_positions, odds, bets, player_names) -> None:
    """Print the race results, as well as the winnings of each player"""

    # print the race positions first
    basic_print("THE RACE RESULTS ARE:")
    for position, horse_idx in enumerate(reversed(race_positions), start=1):
        line = f"{position} PLACE HORSE NO. {horse_idx} AT {odds[horse_idx]}:1"
        basic_print("")
        basic_print(line)

    # followed by the amount the players won
    winning_horse_idx = race_positions[-1]
    for idx, name in enumerate(player_names):
        (horse, amount) = bets[idx]
        if horse == winning_horse_idx:
            basic_print("")
            basic_print(f"{name} WINS ${amount * odds[winning_horse_idx]}")


def main_loop(player_names, horse_odds) -> None:
    """Main game loop"""

    while True:
        print_horse_odds(horse_odds)
        bets = get_bets(player_names)
        final_race_positions = simulate_race(horse_odds)
        print_race_results(final_race_positions, horse_odds, bets, player_names)

        basic_print("DO YOU WANT TO BET ON THE NEXT RACE ?")
        one_more = basic_input("YES OR NO")
        if one_more.upper() != "YES":
            break


def main() -> None:
    # introduction, player names and horse odds are only generated once
    introduction()
    player_names = setup_players()
    horse_odds = setup_horses()

    # main loop of the game, the player can play multiple races, with the
    # same odds
    main_loop(player_names, horse_odds)


if __name__ == "__main__":
    main()
