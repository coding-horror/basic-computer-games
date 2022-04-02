import math
import random
from typing import Dict, List, Literal, Tuple, Union


def print_n_newlines(n: int) -> None:
    for _ in range(n):
        print()


def determine_player_kills(
    bull_quality: int,
    player_type: Literal["TOREAD", "PICADO"],
    plural_form: Literal["ORES", "RES"],
    job_qualities: List[str],
) -> float:
    bull_performance = 3 / bull_quality * random.random()
    if bull_performance < 0.37:
        job_quality_factor = 0.5
    elif bull_performance < 0.5:
        job_quality_factor = 0.4
    elif bull_performance < 0.63:
        job_quality_factor = 0.3
    elif bull_performance < 0.87:
        job_quality_factor = 0.2
    else:
        job_quality_factor = 0.1
    job_quality = math.floor(10 * job_quality_factor + 0.2)  # higher is better
    print(f"THE {player_type}{plural_form} DID A {job_qualities[job_quality]} JOB.")
    if job_quality >= 4:
        if job_quality == 5:
            player_was_killed = random.choice([True, False])
            if player_was_killed:
                print(f"ONE OF THE {player_type}{plural_form} WAS KILLED.")
            elif player_was_killed:
                print(f"NO {player_type}{plural_form} WERE KILLED.")
        else:
            if player_type != "TOREAD":
                killed_horses = random.randint(1, 2)
                print(
                    f"{killed_horses} OF THE HORSES OF THE {player_type}{plural_form} KILLED."
                )
            killed_players = random.randint(1, 2)
            print(f"{killed_players} OF THE {player_type}{plural_form} KILLED.")
    print()
    return job_quality_factor


def calculate_final_score(
    move_risk_sum: float, job_quality_by_round: Dict[int, float], bull_quality: int
) -> float:
    quality = (
        4.5
        + move_risk_sum / 6
        - (job_quality_by_round[1] + job_quality_by_round[2]) * 2.5
        + 4 * job_quality_by_round[4]
        + 2 * job_quality_by_round[5]
        - (job_quality_by_round[3] ** 2) / 120
        - bull_quality
    ) * random.random()
    if quality < 2.4:
        return 0
    elif quality < 4.9:
        return 1
    elif quality < 7.4:
        return 2
    else:
        return 3


def print_header() -> None:
    print(" " * 34 + "BULL")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print_n_newlines(2)


def print_instructions() -> None:
    print("HELLO, ALL YOU BLOODLOVERS AND AFICIONADOS.")
    print("HERE IS YOUR BIG CHANCE TO KILL A BULL.")
    print()
    print("ON EACH PASS OF THE BULL, YOU MAY TRY")
    print("0 - VERONICA (DANGEROUS INSIDE MOVE OF THE CAPE)")
    print("1 - LESS DANGEROUS OUTSIDE MOVE OF THE CAPE")
    print("2 - ORDINARY SWIRL OF THE CAPE.")
    print()
    print("INSTEAD OF THE ABOVE, YOU MAY TRY TO KILL THE BULL")
    print("ON ANY TURN: 4 (OVER THE HORNS), 5 (IN THE CHEST).")
    print("BUT IF I WERE YOU,")
    print("I WOULDN'T TRY IT BEFORE THE SEVENTH PASS.")
    print()
    print("THE CROWD WILL DETERMINE WHAT AWARD YOU DESERVE")
    print("(POSTHUMOUSLY IF NECESSARY).")
    print("THE BRAVER YOU ARE, THE BETTER THE AWARD YOU RECEIVE.")
    print()
    print("THE BETTER THE JOB THE PICADORES AND TOREADORES DO,")
    print("THE BETTER YOUR CHANCES ARE.")


def print_intro() -> None:
    print_header()
    want_instructions = input("DO YOU WANT INSTRUCTIONS? ")
    if want_instructions != "NO":
        print_instructions()
    print_n_newlines(2)


def ask_bool(prompt: str) -> bool:
    while True:
        answer = input(prompt).lower()
        if answer == "yes":
            return True
        elif answer == "no":
            return False
        else:
            print("INCORRECT ANSWER - - PLEASE TYPE 'YES' OR 'NO'.")


def ask_int() -> int:
    while True:
        foo = float(input())
        if foo != float(int(abs(foo))):  # we actually want an integer
            print("DON'T PANIC, YOU IDIOT!  PUT DOWN A CORRECT NUMBER")
        elif foo < 3:
            break
    return int(foo)


def did_bull_hit(
    bull_quality: int,
    cape_move: int,
    job_quality_by_round: Dict[int, float],
    move_risk_sum: float,
) -> Tuple[bool, float]:
    # The bull quality is a grade: The lower the grade, the better the bull
    if cape_move == 0:
        move_risk: Union[int, float] = 3
    elif cape_move == 1:
        move_risk = 2
    else:
        move_risk = 0.5
    move_risk_sum += move_risk
    bull_strength = 6 - bull_quality
    bull_hit_factor = (  # the higher the factor, the more "likely" it hits
        (bull_strength + move_risk / 10)
        * random.random()
        / (
            (
                job_quality_by_round[1]
                + job_quality_by_round[2]
                + job_quality_by_round[3] / 10
            )
            * 5
        )
    )
    bull_hit = bull_hit_factor >= 0.51
    return bull_hit, move_risk_sum


def handle_bullkill_attempt(
    kill_method: int,
    job_quality_by_round: Dict[int, float],
    bull_quality: int,
    gore: int,
) -> int:
    if kill_method not in [4, 5]:
        print("YOU PANICKED.  THE BULL GORED YOU.")
        gore = 2
    else:
        bull_strength = 6 - bull_quality
        kill_probability = (
            bull_strength
            * 10
            * random.random()
            / (
                (job_quality_by_round[1] + job_quality_by_round[2])
                * 5
                * job_quality_by_round[3]
            )
        )
        if kill_method == 4:
            if kill_probability > 0.8:
                gore = 1
        else:
            if kill_probability > 0.2:
                gore = 1
        if gore == 0:
            print("YOU KILLED THE BULL!")
            job_quality_by_round[5] = 2
            return gore
    return gore


def final_message(
    job_quality_by_round: Dict[int, float], bull_quality: int, move_risk_sum: float
) -> None:
    print_n_newlines(3)
    if job_quality_by_round[4] == 0:
        print("THE CROWD BOOS FOR TEN MINUTES.  IF YOU EVER DARE TO SHOW")
        print("YOUR FACE IN A RING AGAIN, THEY SWEAR THEY WILL KILL YOU--")
        print("UNLESS THE BULL DOES FIRST.")
    else:
        if job_quality_by_round[4] == 2:
            print("THE CROWD CHEERS WILDLY!")
        elif job_quality_by_round[5] == 2:
            print("THE CROWD CHEERS!")
            print()
        print("THE CROWD AWARDS YOU")
        score = calculate_final_score(move_risk_sum, job_quality_by_round, bull_quality)
        if score == 0:
            print("NOTHING AT ALL.")
        elif score == 1:
            print("ONE EAR OF THE BULL.")
        elif score == 2:
            print("BOTH EARS OF THE BULL!")
            print("OLE!")
        else:
            print("OLE!  YOU ARE 'MUY HOMBRE'!! OLE!  OLE!")
        print()
        print("ADIOS")
        print_n_newlines(3)


def main() -> None:
    print_intro()
    move_risk_sum: float = 1
    job_quality_by_round: Dict[int, float] = {4: 1, 5: 1}
    job_quality = ["", "SUPERB", "GOOD", "FAIR", "POOR", "AWFUL"]
    bull_quality = random.randint(
        1, 5
    )  # the lower the number, the more powerful the bull
    print(f"YOU HAVE DRAWN A {job_quality[bull_quality]} BULL.")
    if bull_quality > 4:
        print("YOU'RE LUCKY.")
    elif bull_quality < 2:
        print("GOOD LUCK.  YOU'LL NEED IT.")
        print()
    print()

    # Round 1: Run Picadores
    player_type: Literal["TOREAD", "PICADO"] = "PICADO"
    plural_form: Literal["ORES", "RES"] = "RES"
    job_quality_factor = determine_player_kills(
        bull_quality, player_type, plural_form, job_quality
    )
    job_quality_by_round[1] = job_quality_factor

    # Round 2: Run Toreadores
    player_type = "TOREAD"
    plural_form = "ORES"
    determine_player_kills(bull_quality, player_type, plural_form, job_quality)
    job_quality_by_round[2] = job_quality_factor
    print_n_newlines(2)

    # Round 3
    job_quality_by_round[3] = 0
    while True:
        job_quality_by_round[3] = job_quality_by_round[3] + 1
        print(f"PASS NUMBER {job_quality_by_round[3]}")
        if job_quality_by_round[3] >= 3:
            run_from_ring = ask_bool("HERE COMES THE BULL.  TRY FOR A KILL? ")
            if not run_from_ring:
                print("CAPE MOVE? ", end="")
        else:
            print("THE BULL IS CHARGING AT YOU!  YOU ARE THE MATADOR--")
            run_from_ring = ask_bool("DO YOU WANT TO KILL THE BULL? ")
            if not run_from_ring:
                print("WHAT MOVE DO YOU MAKE WITH THE CAPE? ", end="")
        gore = 0
        if not run_from_ring:
            cape_move = ask_int()
            bull_hit, move_risk_sum = did_bull_hit(
                bull_quality, cape_move, job_quality_by_round, move_risk_sum
            )
            if bull_hit:
                gore = 1
            else:
                continue
        else:
            print()
            print("IT IS THE MOMENT OF TRUTH.")
            print()
            kill_method = int(input("HOW DO YOU TRY TO KILL THE BULL? "))
            gore = handle_bullkill_attempt(
                kill_method, job_quality_by_round, bull_quality, gore
            )
            if gore == 0:
                break
        if gore > 0:
            if gore == 1:
                print("THE BULL HAS GORED YOU!")
            death = False
            while True:
                if random.randint(1, 2) == 1:
                    print("YOU ARE DEAD.")
                    job_quality_by_round[4] = 1.5
                    death = True
                    break
                else:
                    print("YOU ARE STILL ALIVE.")
                    print()
                    print("DO YOU RUN FROM THE RING? ", end="")
                    run_from_ring = ask_bool("DO YOU RUN FROM THE RING? ")
                    if not run_from_ring:
                        print("YOU ARE BRAVE.  STUPID, BUT BRAVE.")
                        if random.randint(1, 2) == 1:
                            job_quality_by_round[4] = 2
                            death = True
                            break
                        else:
                            print("YOU ARE GORED AGAIN!")
                    else:
                        print("COWARD")
                        job_quality_by_round[4] = 0
                        death = True
                        break

            if death:
                break

    final_message(job_quality_by_round, bull_quality, move_risk_sum)


if __name__ == "__main__":
    main()
