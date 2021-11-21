#!/usr/bin/env python3
import random
from functools import partial

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


def positions_list():
    return list(range(1, 26, 1))


def generate_enemy_positions():
    """ Randomly choose 4 'positions' out of a range of 1 to 25 """
    positions = positions_list()
    random.shuffle(positions)
    return set(positions[:4])


def is_valid_position(pos):
    return pos in positions_list()


def prompt_for_player_positions():

    while True:
        raw_positions = input("WHAT ARE YOUR FOUR POSITIONS? ")
        positions = set(int(pos) for pos in raw_positions.split())
        # Verify user inputs (for example, if the player gives a
        # a position for 26, the enemy can never hit it)
        if (len(positions) != 4):
            print("PLEASE ENTER 4 UNIQUE POSITIONS\n")
            continue
        elif (any(not is_valid_position(pos) for pos in positions)):
            print("ALL POSITIONS MUST RANGE (1-25)\n")
            continue
        else:
            return positions


def prompt_player_for_target():

    while True:
        target = int(input("WHERE DO YOU WISH TO FIRE YOUR MISSLE? "))
        if not is_valid_position(target):
            print("POSITIONS MUST RANGE (1-25)\n")
            continue

        return target


def attack(target, positions, hit_message, miss_message, progress_messages):
    """ Performs attack procedure returning True if we are to continue. """

    if target in positions:
        print(hit_message.format(target))
        positions.remove(target)
        print(progress_messages[len(positions)].format(target))
    else:
        print(miss_message.format(target))

    return len(positions) > 0


def init_enemy():
    """ Returns a closure analogous to prompt_player_for_target. Will
        choose from a unique sequence of positions to avoid picking the
        same position twice. """

    position_sequence = positions_list()
    random.shuffle(position_sequence)
    position = iter(position_sequence)

    def choose():
        return next(position)

    return choose


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

    # Build partial functions only requiring the target as input
    player_attacks = partial(attack,
                             positions=enemy_positions,
                             hit_message="YOU GOT ONE OF MY OUTPOSTS!",
                             miss_message="HA, HA YOU MISSED. MY TURN NOW:\n\n",
                             progress_messages=PLAYER_PROGRESS_MESSAGES)

    enemy_attacks = partial(attack,
                            positions=player_positions,
                            hit_message="I GOT YOU. IT WON'T BE LONG NOW. POST {} WAS HIT.",
                            miss_message="I MISSED YOU, YOU DIRTY RAT. I PICKED {}. YOUR TURN:\n\n",
                            progress_messages=ENEMY_PROGRESS_MESSAGES)

    enemy_position_choice = init_enemy()

    # Play as long as both player_attacks and enemy_attacks allow to continue
    while player_attacks(prompt_player_for_target()) and enemy_attacks(enemy_position_choice()):
        pass


if __name__ == "__main__":
    play()
