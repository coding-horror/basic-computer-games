"""
CHEMIST

A math game posing as a chemistry word problem.

Ported by Dave LeCompte
"""

import random

MAX_LIVES = 9


def print_with_tab(space_count, msg):
    if space_count > 0:
        spaces = " " * space_count
    else:
        spaces = ""

    print(spaces + msg)


def play_scenario():
    acid_amount = random.randint(1, 50)

    water_amount = 7 * acid_amount / 3

    print(f"{acid_amount} LITERS OF KRYPTOCYANIC ACID.  HOW MUCH WATER?")

    response = float(input())

    difference = abs(water_amount - response)

    acceptable_difference = water_amount / 20

    if difference > acceptable_difference:
        show_failure()

        return False
    else:
        show_success()

        return True


def show_failure():
    print(" SIZZLE!  YOU HAVE JUST BEEN DESALINATED INTO A BLOB")
    print(" OF QUIVERING PROTOPLASM!")


def show_success():
    print(" GOOD JOB! YOU MAY BREATHE NOW, BUT DON'T INHALE THE FUMES!")
    print()


def show_ending():
    print(f" YOUR {MAX_LIVES} LIVES ARE USED, BUT YOU WILL BE LONG REMEMBERED FOR")
    print(" YOUR CONTRIBUTIONS TO THE FIELD OF COMIC BOOK CHEMISTRY.")


def main():
    print_with_tab(33, "CHEMIST")
    print_with_tab(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()

    print("THE FICTITIOUS CHEMICAL KRYPTOCYANIC ACID CAN ONLY BE")
    print("DILUTED BY THE RATIO OF 7 PARTS WATER TO 3 PARTS ACID.")
    print("IF ANY OTHER RATIO IS ATTEMPTED, THE ACID BECOMES UNSTABLE")
    print("AND SOON EXPLODES.  GIVEN THE AMOUNT OF ACID, YOU MUST")
    print("DECIDE WHO MUCH WATER TO ADD FOR DILUTION.  IF YOU MISS")
    print("YOU FACE THE CONSEQUENCES.")

    lives_used = 0

    while True:
        success = play_scenario()

        if not success:
            lives_used += 1

            if lives_used == MAX_LIVES:
                show_ending()
                return


if __name__ == "__main__":
    main()
