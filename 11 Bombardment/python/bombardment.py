#!/usr/bin/env python3
import random

def display_intro():
    print("" * 33 + "BOMBARDMENT")
    print("" * 15 + " CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print("\n\n")
    print("YOU ARE ON A BATTLEFIELD WITH 4 PLATOONS AND YOU")
    print("HAVE 25 OUTPOSTS AVAILABLE WHERE THEY MAY BE PLACED.")
    print("YOU CAN ONLY PLACE ONE PLATOON AT ANY ONE OUTPOST.")
    print("THE COMPUTER DOES THE SAME WITH ITS FOUR PLATOONS.")
    print()
    print("THE OBJECT OF THE GAME IS TO FIRE MISSLES AT THE")
    print("OUTPOSTS OF THE COMPUTER.  IT WILL DO THE SAME TO YOU.")
    print("THE ONE WHO DESTROYS ALL FOUR OF THE ENEMY'S PLATOONS")
    print("FIRST IS THE WINNER.")
    print()
    print("GOOD LUCK... AND TELL US WHERE YOU WANT THE BODIES SENT!")
    print()
    print("TEAR OFF MATRIX AND USE IT TO CHECK OFF THE NUMBERS.")
    print("\n" * 4)


def display_field():
    for row in range(5):
        initial = row * 5 + 1
        print('\t'.join([str(initial + column) for column in range(5)]))

    print("\n" * 9)


def generate_enemy_positions():
    """ Randomly choose 4 'positions' out of a range of 1 to 25 """
    positions = list(range(1, 26, 1))
    random.shuffle(positions)
    return set(positions[:4])


def prompt_for_player_positions():

    while True:
        raw_positions = input("WHAT ARE YOUR FOUR POSITIONS? ")
        positions = set(int(pos) for pos in raw_positions.split())

        # Verify user inputs (for example, if the player gives a
        # a position for 26, the enemy can never hit it)
        if (len(positions) != 4):
            print("PLEASE ENTER 4 UNIQUE POSITIONS\n")
            continue
        elif (any(pos not in range(1, 26, 1) for pos in positions)):
            print("ALL POSITIONS MUST RANGE (1-25)\n")
            continue
        else:
            return positions


# Messages correspond to outposts remaining (3, 2, 1, 0)
PLAYER_PROGRESS_MESSAGES = (
    "YOU GOT ME, I'M GOING FAST. BUT I'LL GET YOU WHEN\n"
    "MY TRANSISTO&S RECUP%RA*E!",
    "THREE DOWN, ONE TO GO.\n\n",
    "TWO DOWN, TWO TO GO.\n\n",
    "ONE DOWN, THREE TO GO.\n\n"
    )


ENEMY_PROGRESS_MESSAGES = (
    "YOU'RE DEAD. YOUR LAST OUTPOST WAS AT {}. HA, HA, HA.\n"
    "BETTER LUCK NEXT TIME.",
    "YOU HAVE ONLY ONE OUTPOST LEFT.\n\n",
    "YOU HAVE ONLY TWO OUTPOSTS LEFT.\n\n",
    "YOU HAVE ONLY THREE OUTPOSTS LEFT.\n\n",
    )


def play():
    display_intro()
    display_field()

    enemy_positions = generate_enemy_positions()
    player_positions = prompt_for_player_positions()

    print(enemy_positions)

    while True:
        target = int(input("WHERE DO YOU WISH TO FIRE YOUR MISSLE? "))

        if target in enemy_positions:
            print("YOU GOT ONE OF MY OUTPOSTS!")
            enemy_positions.remove(target)

            outposts_left = len(enemy_positions)
            print(PLAYER_PROGRESS_MESSAGES[outposts_left])
            if outposts_left == 0:
                break
        else:
            print("HA, HA YOU MISSED. MY TURN NOW:\n\n")

        target = random.randint(1, 25)
        if target in player_positions:
            print("I GOT YOU. IT WON'T BE LONG NOW. POST", target, "WAS HIT.")
            player_positions.remove(target)

            outposts_left = len(player_positions)
            print(ENEMY_PROGRESS_MESSAGES[len(player_positions)].format(target))
            if outposts_left == 0:
                break
        else:
            print("I MISSED YOU, YOU DIRTY RAT. I PICKED", target, ". YOUR TURN:\n\n")


if __name__ == "__main__":
    play()
