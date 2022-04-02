"""
Original game design: Cram, Goodie, Hibbard Lexington H.S.
Modifications: G. Paul, R. Hess (Ties), 1973
"""
import math
from typing import List


def get_choice(prompt: str, choices: List[str]) -> str:
    while True:
        choice = input(prompt)
        if choice in choices:
            break
    return choice


def main():
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

    historical_data = [
        [],
        ["BULL RUN", 18000, 18500, 1967, 2708, 1],
        ["SHILOH", 40000.0, 44894.0, 10699, 13047, 3],
        ["SEVEN DAYS", 95000.0, 115000.0, 20614, 15849, 3],
        ["SECOND BULL RUN", 54000.0, 63000.0, 10000, 14000, 2],
        ["ANTIETAM", 40000.0, 50000.0, 10000, 12000, 3],
        ["FREDERICKSBURG", 75000.0, 120000.0, 5377, 12653, 1],
        ["MURFREESBORO", 38000.0, 45000.0, 11000, 12000, 1],
        ["CHANCELLORSVILLE", 32000, 90000.0, 13000, 17197, 2],
        ["VICKSBURG", 50000.0, 70000.0, 12000, 19000, 1],
        ["GETTYSBURG", 72500.0, 85000.0, 20000, 23000, 3],
        ["CHICKAMAUGA", 66000.0, 60000.0, 18000, 16000, 2],
        ["CHATTANOOGA", 37000.0, 60000.0, 36700.0, 5800, 2],
        ["SPOTSYLVANIA", 62000.0, 110000.0, 17723, 18000, 2],
        ["ATLANTA", 65000.0, 100000.0, 8500, 3700, 1],
    ]
    sa = {}
    dollars_available = {}
    food_array = {}
    salaries = {}
    ammunition = {}
    oa = {}
    print(" " * 26 + "CIVIL WAR")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")

    # Union info on likely confederate strategy
    sa[1] = 25
    sa[2] = 25
    sa[3] = 25
    sa[4] = 25
    party = -1  # number of players in the game
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
    bs = get_choice("(ANSWER YES OR NO) ", ["YES", "NO"])
    if bs == "YES":
        party = 2
    elif bs == "NO":
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
    line = 0
    w = 0
    r1 = 0
    q1 = 0
    m3 = 0
    m4 = 0
    p1 = 0
    p2 = 0
    t1 = 0
    t2 = 0
    for i in range(1, 3):
        dollars_available[i] = 0
        food_array[i] = 0
        salaries[i] = 0
        ammunition[i] = 0
        oa[i] = 0
    r2 = 0
    q2 = 0
    c6 = 0
    food = 0
    w0 = 0
    strategy_index = 0
    union_strategy_index = 0
    u = 0
    u2 = 0
    random_nb = 0
    while True:
        print()
        print()
        print()
        simulated_battle_index = int(input("WHICH BATTLE DO YOU WISH TO SIMULATE? "))
        if simulated_battle_index < 1 or simulated_battle_index > 14:
            break
        if simulated_battle_index != 0 or random_nb == 0:
            cs = historical_data[simulated_battle_index][0]
            m1 = historical_data[simulated_battle_index][1]
            m2 = historical_data[simulated_battle_index][2]
            c1 = historical_data[simulated_battle_index][3]
            c2 = historical_data[simulated_battle_index][4]
            m = historical_data[simulated_battle_index][5]
            u = 0
            # Inflation calc
            i1 = 10 + (line - w) * 2
            i2 = 10 + (w - line) * 2
            # Money available
            dollars_available[1] = 100 * math.floor(
                (m1 * (100 - i1) / 2000) * (1 + (r1 - q1) / (r1 + 1)) + 0.5
            )
            dollars_available[2] = 100 * math.floor(m2 * (100 - i2) / 2000 + 0.5)
            if bs == "YES":
                dollars_available[2] = 100 * math.floor(
                    (m2 * (100 - i2) / 2000) * (1 + (r2 - q2) / (r2 + 1)) + 0.5
                )
            # Men available
            m5 = math.floor(m1 * (1 + (p1 - t1) / (m3 + 1)))
            m6 = math.floor(m2 * (1 + (p2 - t2) / (m4 + 1)))
            f1 = 5 * m1 / 6
            print()
            print()
            print()
            print()
            print()
            print(f"THIS IS THE BATTLE OF {cs}")
            if xs != "NO":
                print("\n".join(battles[simulated_battle_index - 1]))

        else:
            print(cs + " INSTANT REPLAY")

        print()
        print(" \tCONFEDERACY\t UNION")
        print(f"MEN\t  {m5}\t\t {m6}")
        print(f"MONEY\t ${dollars_available[1]}\t\t${dollars_available[2]}")
        print(f"INFLATION\t {i1 + 15}%\t {i2}%")
        print()
        # ONLY IN PRINTOUT IS CONFED INFLATION = I1 + 15 %
        # IF TWO GENERALS, INPUT CONFED, FIRST
        for i in range(1, party + 1):
            if bs == "YES" and i == 1:
                print("CONFEDERATE GENERAL---", end="")
            print("HOW MUCH DO YOU WISH TO SPEND FOR")
            while True:
                food = int(input(" - FOOD...... ? "))
                if food < 0:
                    if r1 == 0:
                        print("NO PREVIOUS ENTRIES")
                        continue
                    print("ASSUME YOU WANT TO KEEP SAME ALLOCATIONS")
                    print()
                    break
                food_array[i] = food
                while True:
                    salaries[i] = int(input(" - SALARIES.. ? "))
                    if salaries[i] >= 0:
                        break
                    print("NEGATIVE VALUES NOT ALLOWED.")
                while True:
                    ammunition[i] = int(input(" - AMMUNITION ? "))
                    if ammunition[i] >= 0:
                        break
                    print("NEGATIVE VALUES NOT ALLOWED.")
                print()
                if food_array[i] + salaries[i] + ammunition[i] > dollars_available[i]:
                    print("THINK AGAIN! YOU HAVE ONLY $" + dollars_available[i])
                else:
                    break

            if bs != "YES" or i == 2:
                break
            print("UNION GENERAL---", end="")

        for z in range(1, party + 1):
            if bs == "YES":
                if z == 1:
                    print("CONFEDERATE ", end="")
                else:
                    print("      UNION ", end="")
            # Find morale
            o = (2 * math.pow(food_array[z], 2) + math.pow(salaries[z], 2)) / math.pow(
                f1, 2
            ) + 1
            if o >= 10:
                print("MORALE IS HIGH")
            elif o >= 5:
                print("MORALE IS FAIR")
            else:
                print("MORALE IS POOR")
            if bs != "YES":
                break
            oa[z] = o

        o2 = oa[2]
        o = oa[1]
        print("CONFEDERATE GENERAL---")
        # Actual off/def battle situation
        if m == 3:
            print("YOU ARE ON THE OFFENSIVE")
        elif m == 1:
            print("YOU ARE ON THE DEFENSIVE")
        else:
            print("BOTH SIDES ARE ON THE OFFENSIVE")

        print()
        # Choose strategies
        if bs != "YES":
            while True:
                strategy_index = int(input("YOUR STRATEGY "))
                if abs(strategy_index - 3) < 3:
                    break
                print(f"STRATEGY {strategy_index} NOT ALLOWED.")
            if strategy_index == 5:
                print("THE CONFEDERACY HAS SURRENDERED.")
                break
            # Union strategy is computer chosen
            if simulated_battle_index == 0:
                while True:
                    union_strategy_index = int(input("UNION STRATEGY IS "))
                    if union_strategy_index > 0 and union_strategy_index < 5:
                        break
                    print("ENTER 1, 2, 3, OR 4 (USUALLY PREVIOUS UNION STRATEGY)")
            else:
                s0 = 0
                random_nb = math.random() * 100
                for i in range(1, 5):
                    s0 += sa[i]
                    # If actual strategy info is in program data statements
                    # then r-100 is extra weight given to that strategy.
                    if random_nb < s0:
                        break
                union_strategy_index = i
                print(union_strategy_index)
        else:
            for i in range(1, 3):
                if i == 1:
                    print("CONFEDERATE STRATEGY ? ", end="")
                while True:
                    strategy_index = int(input())
                    if abs(strategy_index - 3) < 3:
                        break
                    print(f"STRATEGY {strategy_index} NOT ALLOWED.")
                    print("YOUR STRATEGY ? ", end="")
                if i == 2:
                    union_strategy_index = strategy_index
                    strategy_index = previous_strategy  # noqa: F821
                    if union_strategy_index != 5:
                        break
                else:
                    previous_strategy = strategy_index  # noqa: F841
                print("UNION STRATEGY ? ", end="")
            # Simulated losses - North
            c6 = (2 * c2 / 5) * (
                1 + 1 / (2 * (abs(union_strategy_index - strategy_index) + 1))
            )
            c6 = c6 * (1.28 + (5 * m2 / 6) / (ammunition[2] + 1))
            c6 = math.floor(c6 * (1 + 1 / o2) + 0.5)
            # If loss > men present, rescale losses
            e2 = 100 / o2
            if math.floor(c6 + e2) >= m6:
                c6 = math.floor(13 * m6 / 20)
                e2 = 7 * c6 / 13
                u2 = 1
        # Calculate simulated losses
        print()
        print()
        print()
        print("\t\tCONFEDERACY\tUNION")
        c5 = (2 * c1 / 5) * (
            1 + 1 / (2 * (abs(union_strategy_index - strategy_index) + 1))
        )
        c5 = math.floor(c5 * (1 + 1 / o) * (1.28 + f1 / (ammunition[1] + 1)) + 0.5)
        e = 100 / o
        if c5 + 100 / o >= m1 * (1 + (p1 - t1) / (m3 + 1)):
            c5 = math.floor(13 * m1 / 20 * (1 + (p1 - t1) / (m3 + 1)))
            e = 7 * c5 / 13
            u = 1

        if party == 1:
            c6 = math.floor(17 * c2 * c1 / (c5 * 20))
            e2 = 5 * o

        print("CASUALTIES\t" + str(c5) + "\t\t" + str(c6))
        print("DESERTIONS\t" + str(math.floor(e)) + "\t\t" + str(math.floor(e2)))
        print()
        if bs == "YES":
            print("COMPARED TO THE ACTUAL CASUALTIES AT " + str(cs))
            print(
                "CONFEDERATE: "
                + str(math.floor(100 * (c5 / c1) + 0.5))
                + "% OF THE ORIGINAL"
            )
            print(
                "UNION:       "
                + str(math.floor(100 * (c6 / c2) + 0.5))
                + "% OF THE ORIGINAL"
            )

        print()
        # Find who won
        if u == 1 and u2 == 1 or (u != 1 and u2 != 1 and c5 + e == c6 + e2):
            print("BATTLE OUTCOME UNRESOLVED")
            w0 += 1
        elif u == 1 or (u != 1 and u2 != 1 and c5 + e > c6 + e2):
            print("THE UNION WINS " + str(cs))
            if simulated_battle_index != 0:
                line += 1
        else:
            print("THE CONFEDERACY WINS " + str(cs))
            if simulated_battle_index != 0:
                w += 1

        # Lines 2530 to 2590 from original are unreachable.
        if simulated_battle_index != 0:
            t1 += c5 + e
            t2 += c6 + e2
            p1 += c1
            p2 += c2
            q1 += food_array[1] + salaries[1] + ammunition[1]
            q2 += food_array[2] + salaries[2] + ammunition[2]
            r1 += m1 * (100 - i1) / 20
            r2 += m2 * (100 - i2) / 20
            m3 += m1
            m4 += m2
            # Learn present strategy, start forgetting old ones
            # present startegy of south gains 3*s, others lose s
            # probability points, unless a strategy falls below 5 % .
            s = 3
            s0 = 0
            for i in range(1, 5):
                if sa[i] <= 5:
                    continue
                sa[i] -= 5
                s0 += s
            sa[strategy_index] += s0

        u = 0
        u2 = 0
        print("---------------")
        continue

    print()
    print()
    print()
    print()
    print()
    print()
    print(f"THE CONFEDERACY HAS WON {w} BATTLES AND LOST {line}")
    if strategy_index == 5 or (union_strategy_index != 5 and w <= line):
        print("THE UNION HAS WON THE WAR")
    else:
        print("THE CONFEDERACY HAS WON THE WAR")
    print()
    if r1 > 0:
        print(f"FOR THE {w + line + w0} BATTLES FOUGHT (EXCLUDING RERUNS)")
        print(" \t \t ")
        print("CONFEDERACY\t UNION")
        print(f"HISTORICAL LOSSES\t{math.floor(p1 + 0.5)}\t{math.floor(p2 + 0.5)}")
        print(f"SIMULATED LOSSES\t{math.floor(t1 + 0.5)}\t{math.floor(t2 + 0.5)}")
        print()
        print(
            f"    % OF ORIGINAL\t{math.floor(100 * (t1 / p1) + 0.5)}\t{math.floor(100 * (t2 / p2) + 0.5)}"
        )
        if bs != "YES":
            print()
            print("UNION INTELLIGENCE SUGGEST THAT THE SOUTH USED")
            print("STRATEGIES 1, 2, 3, 4 IN THE FOLLOWING PERCENTAGES")
            print(f"{sa[1]} {sa[2]} {sa[3]} {sa[4]}")


if __name__ == "__main__":
    main()
