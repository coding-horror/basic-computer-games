#!/usr/bin/env python3
import random
import textwrap


NUMCNT = 9  # How many numbers are we playing with?


def play():
    print('REVERSE'.center(72))
    print('CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY'.center(72))
    print()
    print()
    print('REVERSE -- A GAME OF SKILL')
    print()

    if not input('DO YOU WANT THE RULES? (yes/no) ').lower().startswith('n'):
        rules()

    while True:
        game_loop()

        if not input('TRY AGAIN? (yes/no) ').lower().startswith('y'):
            return


def game_loop():
    """Play the main game."""
    # Make a random list from 1 to NUMCNT
    numbers = list(range(1, NUMCNT + 1))
    random.shuffle(numbers)

    # Print original list and start the game
    print()
    print('HERE WE GO ... THE LIST IS:')
    print_list(numbers)

    turns = 0
    while True:
        try:
            howmany = int(input('HOW MANY SHALL I REVERSE? '))
            assert howmany >= 0
        except (ValueError, AssertionError):
            continue

        if howmany == 0:
            return

        if howmany > NUMCNT:
            print('OOPS! WRONG! I CAN REVERSE AT MOST', NUMCNT)
            continue

        turns += 1

        # Reverse as many items as requested.
        newnums = numbers[0:howmany]
        newnums.reverse()
        newnums.extend(numbers[howmany:])
        numbers = newnums

        print_list(numbers)

        # Check for a win
        if all(numbers[i] == i + 1 for i in range(NUMCNT)):
            print('YOU WON IT IN {} MOVES!'.format(turns))
            print()
            return


def print_list(numbers):
    """Print out the list"""
    print(' '.join(map(str, numbers)))


def rules():
    """Print out the rules"""
    help = textwrap.dedent("""
        THIS IS THE GAME OF "REVERSE".  TO WIN, ALL YOU HAVE
        TO DO IS ARRANGE A LIST OF NUMBERS (1 THROUGH {})
        IN NUMERICAL ORDER FROM LEFT TO RIGHT.  TO MOVE, YOU
        TELL ME HOW MANY NUMBERS (COUNTING FROM THE LEFT) TO
        REVERSE.  FOR EXAMPLE, IF THE CURRENT LIST IS:

        2 3 4 5 1 6 7 8 9

        AND YOU REVERSE 4, THE RESULT WILL BE:

        5 4 3 2 1 6 7 8 9

        NOW IF YOU REVERSE 5, YOU WIN!

        1 2 3 4 5 6 7 8 9

        NO DOUBT YOU WILL LIKE THIS GAME, BUT
        IF YOU WANT TO QUIT, REVERSE 0 (ZERO).
        """.format(NUMCNT))
    print(help)
    print()


if __name__ == '__main__':
    try:
        play()
    except KeyboardInterrupt:
        pass
