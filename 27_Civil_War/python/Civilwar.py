"""
Original game design: Cram, Goodie, Hibbard Lexington H.S.
Modifications: G. Paul, R. Hess (Ties), 1973
"""
import enum
import math
import random
from dataclasses import dataclass
from typing import Dict, List, Literal, Tuple


class AttackState(enum.Enum):
    DEFENSIVE = 1
    BOTH_OFFENSIVE = 2
    OFFENSIVE = 3


CONF = 1
UNION = 2


@dataclass
class PlayerStat:
    food: float = 0
    salaries: float = 0
    ammunition: float = 0

    desertions: float = 0
    casualties: float = 0
    morale: float = 0
    strategy: int = 0
    available_men: int = 0
    available_money: int = 0

    army_c: float = 0
    army_m: float = 0  # available_men ????
    inflation: float = 0

    r: float = 0
    t: float = 0  # casualties + desertions
    q: float = 0  # accumulated cost?
    p: float = 0
    m: float = 0

    is_player = False
    excessive_losses = False

    def set_available_money(self):
        if self.is_player:
            factor = 1 + (self.r - self.q) / (self.r + 1)
        else:
            factor = 1
        self.available_money = 100 * math.floor(
            (self.army_m * (100 - self.inflation) / 2000) * factor + 0.5
        )

    def get_cost(self) -> float:
        return self.food + self.salaries + self.ammunition

    def get_army_factor(self) -> float:
        return 1 + (self.p - self.t) / (self.m + 1)

    def get_present_men(self) -> float:
        return self.army_m * self.get_army_factor()


def simulate_losses(player1: PlayerStat, player2: PlayerStat) -> float:
    """Simulate losses of player 1"""
    tmp = (2 * player1.army_c / 5) * (
        1 + 1 / (2 * (abs(player1.strategy - player2.strategy) + 1))
    )
    tmp = tmp * (1.28 + (5 * player1.army_m / 6) / (player1.ammunition + 1))
    tmp = math.floor(tmp * (1 + 1 / player1.morale) + 0.5)
    return tmp


def update_army(player: PlayerStat, enemy: PlayerStat, use_factor=False) -> None:
    player.casualties = simulate_losses(player, enemy)
    player.desertions = 100 / player.morale

    loss = player.casualties + player.desertions
    if not use_factor:
        present_men: float = player.available_men
    else:
        present_men = player.get_present_men()
    if loss >= present_men:
        factor = player.get_army_factor()
        if not use_factor:
            factor = 1
        player.casualties = math.floor(13 * player.army_m / 20 * factor)
        player.desertions = 7 * player.casualties / 13
        player.excessive_losses = True


def get_choice(prompt: str, choices: List[str]) -> str:
    while True:
        choice = input(prompt)
        if choice in choices:
            break
    return choice


def get_morale(stat: PlayerStat, enemy: PlayerStat) -> float:
    """Higher is better"""
    enemy_strength = 5 * enemy.army_m / 6
    return (2 * math.pow(stat.food, 2) + math.pow(stat.salaries, 2)) / math.pow(
        enemy_strength, 2
    ) + 1


def main() -> None:
    battles = [
        [
            "JULY 21, 1861.  GEN. BEAUREGARD, COMMANDING THE SOUTH, MET",
            "UNION FORCES WITH GEN. MCDOWELL IN A PREMATURE BATTLE AT",
            "BULL RUN. GEN. JACKSON HELPED PUSH BACK THE UNION ATTACK.",
        ],
        [
            "APRIL 6-7, 1862.  THE CONFEDERATE SURPRISE ATTACK AT",
            "SHILOH FAILED DUE TO POOR ORGANIZATION.",
        ],
        [
            "JUNE 25-JULY 1, 1862.  GENERAL LEE (CSA) UPHELD THE",
            "OFFENSIVE THROUGHOUT THE BATTLE AND FORCED GEN. MCCLELLAN",
            "AND THE UNION FORCES AWAY FROM RICHMOND.",
        ],
        [
            "AUG 29-30, 1862.  THE COMBINED CONFEDERATE FORCES UNDER LEE",
            "AND JACKSON DROVE THE UNION FORCES BACK INTO WASHINGTON.",
        ],
        [
            "SEPT 17, 1862.  THE SOUTH FAILED TO INCORPORATE MARYLAND",
            "INTO THE CONFEDERACY.",
        ],
        [
            "DEC 13, 1862.  THE CONFEDERACY UNDER LEE SUCCESSFULLY",
            "REPULSED AN ATTACK BY THE UNION UNDER GEN. BURNSIDE.",
        ],
        ["DEC 31, 1862.  THE SOUTH UNDER GEN. BRAGG WON A CLOSE BATTLE."],
        [
            "MAY 1-6, 1863.  THE SOUTH HAD A COSTLY VICTORY AND LOST",
            "ONE OF THEIR OUTSTANDING GENERALS, 'STONEWALL' JACKSON.",
        ],
        [
            "JULY 4, 1863.  VICKSBURG WAS A COSTLY DEFEAT FOR THE SOUTH",
            "BECAUSE IT GAVE THE UNION ACCESS TO THE MISSISSIPPI.",
        ],
        [
            "JULY 1-3, 1863.  A SOUTHERN MISTAKE BY GEN. LEE AT GETTYSBURG",
            "COST THEM ONE OF THE MOST CRUCIAL BATTLES OF THE WAR.",
        ],
        [
            "SEPT. 15, 1863. CONFUSION IN A FOREST NEAR CHICKAMAUGA LED",
            "TO A COSTLY SOUTHERN VICTORY.",
        ],
        [
            "NOV. 25, 1863. AFTER THE SOUTH HAD SIEGED GEN. ROSENCRANS'",
            "ARMY FOR THREE MONTHS, GEN. GRANT BROKE THE SIEGE.",
        ],
        [
            "MAY 5, 1864.  GRANT'S PLAN TO KEEP LEE ISOLATED BEGAN TO",
            "FAIL HERE, AND CONTINUED AT COLD HARBOR AND PETERSBURG.",
        ],
        [
            "AUGUST, 1864.  SHERMAN AND THREE VETERAN ARMIES CONVERGED",
            "ON ATLANTA AND DEALT THE DEATH BLOW TO THE CONFEDERACY.",
        ],
    ]

    historical_data: List[Tuple[str, float, float, float, int, AttackState]] = [
        ("", 0, 0, 0, 0, AttackState.DEFENSIVE),
        ("BULL RUN", 18000, 18500, 1967, 2708, AttackState.DEFENSIVE),
        ("SHILOH", 40000.0, 44894.0, 10699, 13047, AttackState.OFFENSIVE),
        ("SEVEN DAYS", 95000.0, 115000.0, 20614, 15849, AttackState.OFFENSIVE),
        ("SECOND BULL RUN", 54000.0, 63000.0, 10000, 14000, AttackState.BOTH_OFFENSIVE),
        ("ANTIETAM", 40000.0, 50000.0, 10000, 12000, AttackState.OFFENSIVE),
        ("FREDERICKSBURG", 75000.0, 120000.0, 5377, 12653, AttackState.DEFENSIVE),
        ("MURFREESBORO", 38000.0, 45000.0, 11000, 12000, AttackState.DEFENSIVE),
        ("CHANCELLORSVILLE", 32000, 90000.0, 13000, 17197, AttackState.BOTH_OFFENSIVE),
        ("VICKSBURG", 50000.0, 70000.0, 12000, 19000, AttackState.DEFENSIVE),
        ("GETTYSBURG", 72500.0, 85000.0, 20000, 23000, AttackState.OFFENSIVE),
        ("CHICKAMAUGA", 66000.0, 60000.0, 18000, 16000, AttackState.BOTH_OFFENSIVE),
        ("CHATTANOOGA", 37000.0, 60000.0, 36700.0, 5800, AttackState.BOTH_OFFENSIVE),
        ("SPOTSYLVANIA", 62000.0, 110000.0, 17723, 18000, AttackState.BOTH_OFFENSIVE),
        ("ATLANTA", 65000.0, 100000.0, 8500, 3700, AttackState.DEFENSIVE),
    ]
    confederate_strategy_prob_distribution = {}

    # What do you spend money on?
    stats: Dict[int, PlayerStat] = {
        CONF: PlayerStat(),
        UNION: PlayerStat(),
    }

    print(" " * 26 + "CIVIL WAR")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")

    # Union info on likely confederate strategy
    confederate_strategy_prob_distribution[1] = 25
    confederate_strategy_prob_distribution[2] = 25
    confederate_strategy_prob_distribution[3] = 25
    confederate_strategy_prob_distribution[4] = 25
    print()
    show_instructions = get_choice(
        "DO YOU WANT INSTRUCTIONS? YES OR NO -- ", ["YES", "NO"]
    )

    if show_instructions == "YES":
        print()
        print()
        print()
        print()
        print("THIS IS A CIVIL WAR SIMULATION.")
        print("TO PLAY TYPE A RESPONSE WHEN THE COMPUTER ASKS.")
        print("REMEMBER THAT ALL FACTORS ARE INTERRELATED AND THAT YOUR")
        print("RESPONSES COULD CHANGE HISTORY. FACTS AND FIGURES USED ARE")
        print("BASED ON THE ACTUAL OCCURRENCE. MOST BATTLES TEND TO RESULT")
        print("AS THEY DID IN THE CIVIL WAR, BUT IT ALL DEPENDS ON YOU!!")
        print()
        print("THE OBJECT OF THE GAME IS TO WIN AS MANY BATTLES AS ")
        print("POSSIBLE.")
        print()
        print("YOUR CHOICES FOR DEFENSIVE STRATEGY ARE:")
        print("        (1) ARTILLERY ATTACK")
        print("        (2) FORTIFICATION AGAINST FRONTAL ATTACK")
        print("        (3) FORTIFICATION AGAINST FLANKING MANEUVERS")
        print("        (4) FALLING BACK")
        print(" YOUR CHOICES FOR OFFENSIVE STRATEGY ARE:")
        print("        (1) ARTILLERY ATTACK")
        print("        (2) FRONTAL ATTACK")
        print("        (3) FLANKING MANEUVERS")
        print("        (4) ENCIRCLEMENT")
        print("YOU MAY SURRENDER BY TYPING A '5' FOR YOUR STRATEGY.")

    print()
    print()
    print()
    print("ARE THERE TWO GENERALS PRESENT ", end="")
    two_generals = get_choice("(ANSWER YES OR NO) ", ["YES", "NO"]) == "YES"
    stats[CONF].is_player = True
    if two_generals:
        party: Literal[1, 2] = 2  # number of players in the game
        stats[UNION].is_player = True
    else:
        party = 1
        print()
        print("YOU ARE THE CONFEDERACY.   GOOD LUCK!")
        print()

    print("SELECT A BATTLE BY TYPING A NUMBER FROM 1 TO 14 ON")
    print("REQUEST.  TYPE ANY OTHER NUMBER TO END THE SIMULATION.")
    print("BUT '0' BRINGS BACK EXACT PREVIOUS BATTLE SITUATION")
    print("ALLOWING YOU TO REPLAY IT")
    print()
    print("NOTE: A NEGATIVE FOOD$ ENTRY CAUSES THE PROGRAM TO ")
    print("USE THE ENTRIES FROM THE PREVIOUS BATTLE")
    print()
    print("AFTER REQUESTING A BATTLE, DO YOU WISH ", end="")
    print("BATTLE DESCRIPTIONS ", end="")
    xs = get_choice("(ANSWER YES OR NO) ", ["YES", "NO"])
    confederacy_lost = 0
    confederacy_win = 0
    for i in [CONF, UNION]:
        stats[i].p = 0
        stats[i].m = 0
        stats[i].t = 0
        stats[i].available_money = 0
        stats[i].food = 0
        stats[i].salaries = 0
        stats[i].ammunition = 0
        stats[i].strategy = 0
        stats[i].excessive_losses = False
    confederacy_unresolved = 0
    random_nb: float = 0
    while True:
        print()
        print()
        print()
        simulated_battle_index = int(
            get_choice(
                "WHICH BATTLE DO YOU WISH TO SIMULATE? (0-14) ",
                [str(i) for i in range(15)],
            )
        )
        if simulated_battle_index < 1 or simulated_battle_index > 14:
            break
        if simulated_battle_index != 0 or random_nb == 0:
            loaded_battle = historical_data[simulated_battle_index]
            battle_name = loaded_battle[0]
            stats[CONF].army_m = loaded_battle[1]
            stats[UNION].army_m = loaded_battle[2]
            stats[CONF].army_c = loaded_battle[3]
            stats[UNION].army_c = loaded_battle[4]
            stats[CONF].excessive_losses = False

            # Inflation calc
            stats[CONF].inflation = 10 + (confederacy_lost - confederacy_win) * 2
            stats[UNION].inflation = 10 + (confederacy_win - confederacy_lost) * 2

            # Money and Men available
            for i in [CONF, UNION]:
                stats[i].set_available_money()
                stats[i].available_men = math.floor(stats[i].get_army_factor())
            print()
            print()
            print()
            print()
            print()
            print(f"THIS IS THE BATTLE OF {battle_name}")
            if xs != "NO":
                print("\n".join(battles[simulated_battle_index - 1]))

        else:
            print(f"{battle_name} INSTANT REPLAY")

        print()
        print("          CONFEDERACY\t UNION")
        print(f"MEN       {stats[CONF].available_men}\t\t {stats[UNION].available_men}")
        print(
            f"MONEY    ${stats[CONF].available_money}\t${stats[UNION].available_money}"
        )
        print(f"INFLATION {stats[CONF].inflation + 15}%\t\t {stats[UNION].inflation}%")
        print()
        # ONLY IN PRINTOUT IS CONFED INFLATION = I1 + 15 %
        # IF TWO GENERALS, INPUT CONFED, FIRST
        for player_index in range(1, party + 1):
            if two_generals and player_index == 1:
                print("CONFEDERATE GENERAL---", end="")
            print("HOW MUCH DO YOU WISH TO SPEND FOR")
            while True:
                food_input = int(input(" - FOOD...... ? "))
                if food_input < 0:
                    if stats[CONF].r == 0:
                        print("NO PREVIOUS ENTRIES")
                        continue
                    print("ASSUME YOU WANT TO KEEP SAME ALLOCATIONS")
                    print()
                    break
                stats[player_index].food = food_input
                while True:
                    stats[player_index].salaries = int(input(" - SALARIES.. ? "))
                    if stats[player_index].salaries >= 0:
                        break
                    print("NEGATIVE VALUES NOT ALLOWED.")
                while True:
                    stats[player_index].ammunition = int(input(" - AMMUNITION ? "))
                    if stats[player_index].ammunition >= 0:
                        break
                    print("NEGATIVE VALUES NOT ALLOWED.")
                print()
                if stats[player_index].get_cost() > stats[player_index].available_money:
                    print(
                        f"THINK AGAIN! YOU HAVE ONLY ${stats[player_index].available_money}"
                    )
                else:
                    break

            if not two_generals or player_index == 2:
                break
            print("UNION GENERAL---", end="")

        for player_index in range(1, party + 1):
            if two_generals:
                if player_index == 1:
                    print("CONFEDERATE ", end="")
                else:
                    print("      UNION ", end="")
            morale = get_morale(stats[player_index], stats[1 + player_index % 2])

            if morale >= 10:
                print("MORALE IS HIGH")
            elif morale >= 5:
                print("MORALE IS FAIR")
            else:
                print("MORALE IS POOR")
            if not two_generals:
                break
            stats[player_index].morale = morale  # type: ignore

        stats[UNION].morale = get_morale(stats[UNION], stats[CONF])
        stats[CONF].morale = get_morale(stats[CONF], stats[UNION])
        print("CONFEDERATE GENERAL---")
        # Actual off/def battle situation
        if loaded_battle[5] == AttackState.OFFENSIVE:
            print("YOU ARE ON THE OFFENSIVE")
        elif loaded_battle[5] == AttackState.DEFENSIVE:
            print("YOU ARE ON THE DEFENSIVE")
        else:
            print("BOTH SIDES ARE ON THE OFFENSIVE")

        print()
        # Choose strategies
        if not two_generals:
            while True:
                stats[CONF].strategy = int(input("YOUR STRATEGY "))
                if abs(stats[CONF].strategy - 3) < 3:
                    break
                print(f"STRATEGY {stats[CONF].strategy} NOT ALLOWED.")
            if stats[CONF].strategy == 5:
                print("THE CONFEDERACY HAS SURRENDERED.")
                break
            # Union strategy is computer chosen
            if simulated_battle_index == 0:
                while True:
                    stats[UNION].strategy = int(input("UNION STRATEGY IS "))
                    if stats[UNION].strategy > 0 and stats[UNION].strategy < 5:
                        break
                    print("ENTER 1, 2, 3, OR 4 (USUALLY PREVIOUS UNION STRATEGY)")
            else:
                s0 = 0
                random_nb = random.random() * 100
                for player_index in range(1, 5):
                    s0 += confederate_strategy_prob_distribution[player_index]
                    # If actual strategy info is in program data statements
                    # then r-100 is extra weight given to that strategy.
                    if random_nb < s0:
                        break
                stats[UNION].strategy = player_index
                print(stats[UNION].strategy)
        else:
            for player_index in [1, 2]:
                if player_index == 1:
                    print("CONFEDERATE STRATEGY ? ", end="")
                while True:
                    stats[CONF].strategy = int(input())
                    if abs(stats[CONF].strategy - 3) < 3:
                        break
                    print(f"STRATEGY {stats[CONF].strategy} NOT ALLOWED.")
                    print("YOUR STRATEGY ? ", end="")
                if player_index == 2:
                    stats[UNION].strategy = stats[CONF].strategy
                    stats[CONF].strategy = previous_strategy  # type: ignore # noqa: F821
                    if stats[UNION].strategy != 5:
                        break
                else:
                    previous_strategy = stats[CONF].strategy  # noqa: F841
                print("UNION STRATEGY ? ", end="")

            update_army(stats[UNION], stats[CONF], use_factor=False)

        # Calculate simulated losses
        print()
        print()
        print()
        print("\t\tCONFEDERACY\tUNION")
        update_army(stats[CONF], stats[UNION], use_factor=True)

        if party == 1:
            stats[UNION].casualties = math.floor(
                17
                * stats[UNION].army_c
                * stats[CONF].army_c
                / (stats[CONF].casualties * 20)
            )
            stats[CONF].desertions = 5 * morale

        print(
            "CASUALTIES\t"
            + str(stats[CONF].casualties)
            + "\t\t"
            + str(stats[UNION].casualties)
        )
        print(
            "DESERTIONS\t"
            + str(math.floor(stats[CONF].desertions))
            + "\t\t"
            + str(math.floor(stats[UNION].desertions))
        )
        print()
        if two_generals:
            print("COMPARED TO THE ACTUAL CASUALTIES AT " + str(battle_name))
            print(
                "CONFEDERATE: "
                + str(
                    math.floor(
                        100 * (stats[CONF].casualties / stats[CONF].army_c) + 0.5
                    )
                )
                + "% OF THE ORIGINAL"
            )
            print(
                "UNION:       "
                + str(
                    math.floor(
                        100 * (stats[UNION].casualties / stats[UNION].army_c) + 0.5
                    )
                )
                + "% OF THE ORIGINAL"
            )

        print()
        # Find who won
        if (
            stats[CONF].excessive_losses
            and stats[UNION].excessive_losses
            or (
                not stats[CONF].excessive_losses
                and not stats[UNION].excessive_losses
                and stats[CONF].casualties + stats[CONF].desertions
                == stats[UNION].casualties + stats[CONF].desertions
            )
        ):
            print("BATTLE OUTCOME UNRESOLVED")
            confederacy_unresolved += 1
        elif stats[CONF].excessive_losses or (
            not stats[CONF].excessive_losses
            and not stats[UNION].excessive_losses
            and stats[CONF].casualties + stats[CONF].desertions
            > stats[UNION].casualties + stats[CONF].desertions
        ):
            print(f"THE UNION WINS {battle_name}")
            if simulated_battle_index != 0:
                confederacy_lost += 1
        else:
            print(f"THE CONFEDERACY WINS {battle_name}")
            if simulated_battle_index != 0:
                confederacy_win += 1

        # Lines 2530 to 2590 from original are unreachable.
        if simulated_battle_index != 0:
            for i in [CONF, UNION]:
                stats[i].t += stats[i].casualties + stats[i].desertions
                stats[i].p += stats[i].army_c
                stats[i].q += stats[i].get_cost()
                stats[i].r += stats[i].army_m * (100 - stats[i].inflation) / 20
                stats[i].m += stats[i].army_m
            # Learn present strategy, start forgetting old ones
            # present strategy of south gains 3*s, others lose s
            # probability points, unless a strategy falls below 5 % .
            s = 3
            s0 = 0
            for player_index in range(1, 5):
                if confederate_strategy_prob_distribution[player_index] <= 5:
                    continue
                confederate_strategy_prob_distribution[player_index] -= 5
                s0 += s
            confederate_strategy_prob_distribution[stats[CONF].strategy] += s0

        stats[CONF].excessive_losses = False
        stats[UNION].excessive_losses = False
        print("---------------")
        continue

    print()
    print()
    print()
    print()
    print()
    print()
    print(
        f"THE CONFEDERACY HAS WON {confederacy_win} BATTLES AND LOST {confederacy_lost}"
    )
    if stats[CONF].strategy == 5 or (
        stats[UNION].strategy != 5 and confederacy_win <= confederacy_lost
    ):
        print("THE UNION HAS WON THE WAR")
    else:
        print("THE CONFEDERACY HAS WON THE WAR")
    print()
    if stats[CONF].r > 0:
        print(
            f"FOR THE {confederacy_win + confederacy_lost + confederacy_unresolved} BATTLES FOUGHT (EXCLUDING RERUNS)"
        )
        print(" \t \t ")
        print("CONFEDERACY\t UNION")
        print(
            f"HISTORICAL LOSSES\t{math.floor(stats[CONF].p + 0.5)}\t{math.floor(stats[UNION].p + 0.5)}"
        )
        print(
            f"SIMULATED LOSSES\t{math.floor(stats[CONF].t + 0.5)}\t{math.floor(stats[UNION].t + 0.5)}"
        )
        print()
        print(
            f"    % OF ORIGINAL\t{math.floor(100 * (stats[CONF].t / stats[CONF].p) + 0.5)}\t{math.floor(100 * (stats[UNION].t / stats[UNION].p) + 0.5)}"
        )
        if not two_generals:
            print()
            print("UNION INTELLIGENCE SUGGEST THAT THE SOUTH USED")
            print("STRATEGIES 1, 2, 3, 4 IN THE FOLLOWING PERCENTAGES")
            print(
                f"{confederate_strategy_prob_distribution[CONF]} {confederate_strategy_prob_distribution[UNION]} {confederate_strategy_prob_distribution[3]} {confederate_strategy_prob_distribution[4]}"
            )


if __name__ == "__main__":
    main()
