"""
KINEMA

A kinematics physics quiz.

Ported by Dave LeCompte
"""

import random

# We approximate gravity from 9.8 meters/second squared to 10, which
# is only off by about 2%. 10 is also a lot easier for people to use
# for mental math.

g = 10

# We only expect the student to get within this percentage of the
# correct answer. This isn't rocket science.

EXPECTED_ACCURACY_PERCENT = 15


def print_with_tab(spaces_count, msg):
    if spaces_count > 0:
        spaces = " " * spaces_count
    else:
        spaces = ""
    print(spaces + msg)


def do_quiz():
    print()
    print()
    num_questions_correct = 0

    # pick random initial velocity
    v0 = random.randint(5, 40)
    print(f"A BALL IS THROWN UPWARDS AT {v0} METERS PER SECOND.")
    print()

    answer = v0 ** 2 / (2 * g)
    num_questions_correct += ask_player("HOW HIGH WILL IT GO (IN METERS)?", answer)

    answer = 2 * v0 / g
    num_questions_correct += ask_player(
        "HOW LONG UNTIL IT RETURNS (IN SECONDS)?", answer
    )

    t = 1 + random.randint(0, 2 * v0) // g
    answer = v0 - g * t
    num_questions_correct += ask_player(
        f"WHAT WILL ITS VELOCITY BE AFTER {t} SECONDS?", answer
    )

    print()
    print(f"{num_questions_correct} right out of 3.")
    if num_questions_correct >= 2:
        print("  NOT BAD.")


def ask_player(question, answer):
    print(question)
    player_answer = float(input())

    accuracy_frac = EXPECTED_ACCURACY_PERCENT / 100.0
    if abs((player_answer - answer) / answer) < accuracy_frac:
        print("CLOSE ENOUGH.")
        score = 1
    else:
        print("NOT EVEN CLOSE....")
        score = 0
    print(f"CORRECT ANSWER IS {answer}")
    print()
    return score


def main():
    print_with_tab(33, "KINEMA")
    print_with_tab(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()

    while True:
        do_quiz()


if __name__ == "__main__":
    main()
