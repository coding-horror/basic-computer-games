"""
Stars

From: BASIC Computer Games (1978)
      Edited by David H. Ahl

"In this game, the computer selects a random number from 1 to 100
 (or any value you set [for MAX_NUM]).  You try to guess the number
 and the computer gives you clues to tell you how close you're
 getting.  One star (*) means you're far away from the number; seven
 stars (*******) means you're really close.  You get 7  guesses.

"On the surface this game is very similar to GUESS; however, the
 guessing strategy is quite different.  See if you can come up with
 one or more approaches to finding the mystery number.

"Bob Albrecht of People's Computer Company created this game."


Python port by Jeff Jetton, 2019
"""


import random

# Some contants
MAX_NUM = 100
MAX_GUESSES = 7


def print_instructions():
    """Instructions on how to play"""
    print("I am thinking of a whole number from 1 to %d" % MAX_NUM)
    print("Try to guess my number.  After you guess, I")
    print("will type one or more stars (*).  The more")
    print("stars I type, the closer you are to my number.")
    print("one star (*) means far away, seven stars (*******)")
    print("means really close!  You get %d guesses." % MAX_GUESSES)


def print_stars(secret_number, guess):
    diff = abs(guess - secret_number)
    stars = ""
    for i in range(8):
        if diff < 2**i:
            stars += "*"
    print(stars)


def get_guess():
    valid_response = False
    while not valid_response:
        guess = input("Your guess? ")
        if guess.isdigit():
            valid_response = True
            guess = int(guess)
    return guess


def main():
    # Display intro text
    print("\n                   Stars")
    print("Creative Computing  Morristown, New Jersey")
    print("\n\n")
    # "*** Stars - People's Computer Center, MenloPark, CA"

    response = input("Do you want instructions? ")
    if response.upper()[0] == "Y":
        print_instructions()

    still_playing = True
    while still_playing:

        # "*** Computer thinks of a number"
        secret_number = random.randint(1, MAX_NUM)
        print("\n\nOK, I am thinking of a number, start guessing.")

        # Init/start guess loop
        guess_number = 0
        player_has_won = False
        while (guess_number < MAX_GUESSES) and not player_has_won:

            print("")
            guess = get_guess()
            guess_number += 1

            if guess == secret_number:
                # "*** We have a winner"
                player_has_won = True
                print("**************************************************!!!")
                print(f"You got it in {guess_number} guesses!!!")

            else:
                print_stars(secret_number, guess)

            # End of guess loop

        # "*** Did not guess in [MAX_GUESS] guesses"
        if not player_has_won:
            print(f"\nSorry, that's {guess_number} guesses, number was {secret_number}")

        # Keep playing?
        response = input("\nPlay again? ")
        if response.upper()[0] != "Y":
            still_playing = False


if __name__ == "__main__":
    main()

######################################################################
#
# Porting Notes
#
#   The original program never exited--it just kept playing rounds
#   over and over.  This version asks to continue each time.
#
#
# Ideas for Modifications
#
#   Let the player know how many guesses they have remaining after
#   each incorrect guess.
#
#   Ask the player to select a skill level at the start of the game,
#   which will affect the values of MAX_NUM and MAX_GUESSES.
#   For example:
#
#       Easy   = 8 guesses, 1 to 50
#       Medium = 7 guesses, 1 to 100
#       Hard   = 6 guesses, 1 to 200
#
######################################################################
