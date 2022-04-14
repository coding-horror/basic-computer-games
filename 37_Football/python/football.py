"""
FOOTBALL

A game.

Ported to Python by Martin Thoma in 2022.
The JavaScript version by Oscar Toledo G. (nanochess) was used
"""
# NOTE: The newlines might be wrong

import json
from math import floor
from pathlib import Path
from random import randint, random
from typing import List, Tuple

with open(Path(__file__).parent / "data.json") as f:
    data = json.load(f)

player_data = [num - 1 for num in data["players"]]
actions = data["actions"]


aa: List[int] = [-100 for _ in range(20)]
ba: List[int] = [-100 for _ in range(20)]
ca: List[int] = [-100 for _ in range(40)]
score: List[int] = [0, 0]
ta: Tuple[int, int] = (1, 0)
wa: Tuple[int, int] = (-1, 1)
xa: Tuple[int, int] = (100, 0)
ya: Tuple[int, int] = (1, -1)
za: Tuple[int, int] = (0, 100)
marker: Tuple[str, str] = ("--->", "<---")
t: int = 0
p: int = 0
winning_score: int


def ask_bool(prompt: str) -> bool:
    while True:
        answer = input(prompt).lower()
        if answer in ["yes", "y"]:
            return True
        elif answer in ["no", "n"]:
            return False


def ask_int(prompt: str) -> int:
    while True:
        answer = input(prompt)
        try:
            int_answer = int(answer)
            return int_answer
        except Exception:
            pass


def get_offense_defense() -> Tuple[int, int]:
    while True:
        input_str = input("INPUT OFFENSIVE PLAY, DEFENSIVE PLAY: ")
        try:
            p1, p2 = (int(n) for n in input_str.split(","))
            return p1, p2
        except Exception:
            pass


def field_headers() -> None:
    print("TEAM 1 [0   10   20   30   40   50   60   70   80   90   100] TEAM 2")
    print("\n\n")


def separator() -> None:
    print("+" * 72 + "\n")


def show_ball() -> None:
    da: Tuple[int, int] = (0, 3)
    print(" " * (da[t] + 5 + int(p / 2)) + marker[t] + "\n")
    field_headers()


def show_scores() -> bool:
    print()
    print(f"TEAM 1 SCORE IS {score[0]}")
    print(f"TEAM 2 SCORE IS {score[1]}")
    print()
    if score[t] >= winning_score:
        print(f"TEAM {t+1} WINS*******************")
        return True
    return False


def loss_posession() -> None:
    global t
    print()
    print(f"** LOSS OF POSSESSION FROM TEAM {t+1} TO TEAM {ta[t]+1}")
    print()
    separator()
    print()
    t = ta[t]


def touchdown() -> None:
    print()
    print(f"TOUCHDOWN BY TEAM {t+1} *********************YEA TEAM")
    q = 7
    g = random()
    if g <= 0.1:
        q = 6
        print("EXTRA POINT NO GOOD")
    else:
        print("EXTRA POINT GOOD")
    score[t] = score[t] + q


def print_header() -> None:
    print(" " * 32 + "FOOTBALL")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")
    print("PRESENTING N.F.U. FOOTBALL (NO FORTRAN USED)\n\n")


def print_instructions() -> None:
    print(
        """THIS IS A FOOTBALL GAME FOR TWO TEAMS IN WHICH PLAYERS MUST
PREPARE A TAPE WITH A DATA STATEMENT (1770 FOR TEAM 1,
1780 FOR TEAM 2) IN WHICH EACH TEAM SCRAMBLES NOS. 1-20
THESE NUMBERS ARE THEN ASSIGNED TO TWENTY GIVEN PLAYS.
A LIST OF NOS. AND THEIR PLAYS IS PROVIDED WITH
BOTH TEAMS HAVING THE SAME PLAYS. THE MORE SIMILAR THE
PLAYS THE LESS YARDAGE GAINED.  SCORES ARE GIVEN
WHENEVER SCORES ARE MADE. SCORES MAY ALSO BE OBTAINED
BY INPUTTING 99,99 FOR PLAY NOS. TO PUNT OR ATTEMPT A
FIELD GOAL, INPUT 77,77 FOR PLAY NUMBERS. QUESTIONS WILL BE
ASKED THEN. ON 4TH DOWN, YOU WILL ALSO BE ASKED WHETHER
YOU WANT TO PUNT OR ATTEMPT A FIELD GOAL. IF THE ANSWER TO
BOTH QUESTIONS IS NO IT WILL BE ASSUMED YOU WANT TO
TRY AND GAIN YARDAGE. ANSWER ALL QUESTIONS YES OR NO.
THE GAME IS PLAYED UNTIL PLAYERS TERMINATE (CONTROL-C).
PLEASE PREPARE A TAPE AND RUN.
"""
    )


def main() -> None:
    global winning_score
    print_header()
    want_instructions = ask_bool("DO YOU WANT INSTRUCTIONS? ")
    if want_instructions:
        print_instructions()
    print()
    winning_score = ask_int("PLEASE INPUT SCORE LIMIT ON GAME: ")
    for i in range(40):
        index = player_data[i - 1]
        if i < 20:
            aa[index] = i
        else:
            ba[index] = i - 20
        ca[i] = index
    offset = 0
    for t in [0, 1]:
        print(f"TEAM {t+1} PLAY CHART")
        print("NO.      PLAY")
        for i in range(20):
            input_str = f"{ca[i + offset]}"
            while len(input_str) < 6:
                input_str += " "
            input_str += actions[i]
            print(input_str)
        offset += 20
        t = 1
        print()
        print("TEAR OFF HERE----------------------------------------------")
        print("\n" * 10)

    field_headers()
    print("TEAM 1 DEFEND 0 YD GOAL -- TEAM 2 DEFENDS 100 YD GOAL.")
    t = randint(0, 1)
    print()
    print("THE COIN IS FLIPPED")
    routine = 1
    while True:
        if routine <= 1:
            p = xa[t] - ya[t] * 40
            separator()
            print(f"TEAM {t+1} RECEIVES KICK-OFF")
            k = floor(26 * random() + 40)
        if routine <= 2:
            p = p - ya[t] * k
        if routine <= 3:
            if wa[t] * p >= za[t] + 10:
                print("BALL WENT OUT OF ENDZONE --AUTOMATIC TOUCHBACK--")
                p = za[t] - wa[t] * 20
                if routine <= 4:
                    routine = 5
            else:
                print(f"BALL WENT {k} YARDS.  NOW ON {p}")
                show_ball()

        if routine <= 4:
            want_runback = ask_bool(f"TEAM {t+1} DO YOU WANT TO RUNBACK? ")

            if want_runback:
                k = floor(9 * random() + 1)
                r = floor(((xa[t] - ya[t] * p + 25) * random() - 15) / k)
                p = p - wa[t] * r
                print(f"RUNBACK TEAM {t+1} {r} YARDS")
                g = random()
                if g < 0.25:
                    loss_posession()
                    routine = 4
                    continue
                elif ya[t] * p >= xa[t]:
                    touchdown()
                    if show_scores():
                        return
                    t = ta[t]
                    routine = 1
                    continue
                elif wa[t] * p >= za[t]:
                    print(f"SAFETY AGAINST TEAM {t+1} **********************OH-OH")
                    score[ta[t]] = score[ta[t]] + 2
                    if show_scores():
                        return

                    p = za[t] - wa[t] * 20
                    want_punt = ask_bool(
                        f"TEAM {t+1} DO YOU WANT TO PUNT INSTEAD OF A KICKOFF? "
                    )
                    if want_punt:
                        print(f"TEAM {t+1} WILL PUNT")
                        g = random()
                        if g < 0.25:
                            loss_posession()
                            routine = 4
                            continue

                        print()
                        separator()
                        k = floor(25 * random() + 35)
                        t = ta[t]
                        routine = 2
                        continue

                    touchdown()
                    if show_scores():
                        return
                    t = ta[t]
                    routine = 1
                    continue
                else:
                    routine = 5
                    continue

            else:
                if wa[t] * p >= za[t]:
                    p = za[t] - wa[t] * 20

        if routine <= 5:
            d = 1
            s = p

        if routine <= 6:
            print("=" * 72 + "\n")
            print(f"TEAM {t+1} DOWN {d} ON {p}")
            if d == 1:
                if ya[t] * (p + ya[t] * 10) >= xa[t]:
                    c = 8
                else:
                    c = 4

            if c != 8:
                yards = 10 - (ya[t] * p - ya[t] * s)
                print(" " * 27 + f"{yards} YARDS TO 1ST DOWN")
            else:
                yards = xa[t] - ya[t] * p
                print(" " * 27 + f"{yards} YARDS")

            show_ball()
            if d == 4:
                routine = 8

        if routine <= 7:
            u = floor(3 * random() - 1)
            while True:
                p1, p2 = get_offense_defense()
                if t != 1:
                    p2, p1 = p1, p2

                if p1 == 99:
                    if show_scores():
                        return
                    if p1 == 99:
                        continue

                if p1 < 1 or p1 > 20 or p2 < 1 or p2 > 20:
                    print("ILLEGAL PLAY NUMBER, CHECK AND ", end="")
                    continue

                break
            p1 -= 1
            p2 -= 1

        if d == 4 or p1 == 77:
            want_punt = ask_bool(f"DOES TEAM {t+1} WANT TO PUNT? ")

            if want_punt:
                print()
                print(f"TEAM {t+1} WILL PUNT")
                g = random()
                if g < 0.25:
                    loss_posession()
                    routine = 4
                    continue

                print()
                separator()
                k = floor(25 * random() + 35)
                t = ta[t]
                routine = 2
                continue

            attempt_field_goal = ask_bool(
                f"DOES TEAM {t+1} WANT TO ATTEMPT A FIELD GOAL? "
            )

            if attempt_field_goal:
                print()
                print(f"TEAM {t+1} WILL ATTEMPT A FIELD GOAL")
                g = random()
                if g < 0.025:
                    loss_posession()
                    routine = 4
                    continue
                else:
                    f = floor(35 * random() + 20)
                    print()
                    print(f"KICK IS {f} YARDS LONG")
                    p = p - wa[t] * f
                    g = random()
                    if g < 0.35:
                        print("BALL WENT WIDE")
                    elif ya[t] * p >= xa[t]:
                        print(
                            f"FIELD GOLD GOOD FOR TEAM {t+1} *********************YEA"
                        )
                        q = 3
                        score[t] = score[t] + q
                        if show_scores():
                            return
                        t = ta[t]
                        routine = 1
                        continue

                    print(f"FIELD GOAL UNSUCCESFUL TEAM {t+1}-----------------TOO BAD")
                    print()
                    separator()
                    if ya[t] * p < xa[t] + 10:
                        print()
                        print(f"BALL NOW ON {p}")
                        t = ta[t]
                        show_ball()
                        routine = 4
                        continue
                    else:
                        t = ta[t]
                        routine = 3
                        continue

            else:
                routine = 7
                continue

        y = floor(
            abs(aa[p1] - ba[p2]) / 19 * ((xa[t] - ya[t] * p + 25) * random() - 15)
        )
        print()
        if t == 1 and aa[p1] < 11 or t == 2 and ba[p2] < 11:
            print("THE BALL WAS RUN")
        elif u == 0:
            print(f"PASS INCOMPLETE TEAM {t+1}")
            y = 0
        else:
            g = random()
            if g <= 0.025 and y > 2:
                print("PASS COMPLETED")
            else:
                print("QUARTERBACK SCRAMBLED")

        p = p - wa[t] * y
        print()
        print(f"NET YARDS GAINED ON DOWN {d} ARE {y}")

        g = random()
        if g <= 0.025:
            loss_posession()
            routine = 4
            continue
        elif ya[t] * p >= xa[t]:
            touchdown()
            if show_scores():
                return
            t = ta[t]
            routine = 1
            continue
        elif wa[t] * p >= za[t]:
            print()
            print(f"SAFETY AGAINST TEAM {t+1} **********************OH-OH")
            score[ta[t]] = score[ta[t]] + 2
            if show_scores():
                return
            p = za[t] - wa[t] * 20
            want_punt = ask_bool(
                f"TEAM {t+1} DO YOU WANT TO PUNT INSTEAD OF A KICKOFF? "
            )
            if want_punt:
                print()
                print(f"TEAM {t+1} WILL PUNT")
                g = random()
                if g < 0.25:
                    loss_posession()
                    routine = 4
                    continue

                print()
                separator()
                k = floor(25 * random() + 35)
                t = ta[t]
                routine = 2
                continue

            touchdown()
            if show_scores():
                return
            t = ta[t]
            routine = 1
        elif ya[t] * p - ya[t] * s >= 10:
            routine = 5
        else:
            d += 1
            if d != 5:
                routine = 6
            else:
                print()
                print(f"CONVERSION UNSUCCESSFUL TEAM {t+1}")
                t = ta[t]
                print()
                separator()
                routine = 5


if __name__ == "__main__":
    main()
