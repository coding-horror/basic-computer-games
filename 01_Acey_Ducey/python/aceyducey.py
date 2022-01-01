"""aceyducey.py contains game code"""
########################################################
#
# Acey Ducey
#
# From: BASIC Computer Games (1978)
#       Edited by David Ahl
#
# "This is a simulation of the Acey Ducey card game.
#  In the game, the dealer (the computer) deals two
#  cards face up.  You have an option to bet or not to
#  bet depending on whether or not you feel the next
#  card dealt will have a value between the first two.
#
# "Your initial money is set to $100. The game keeps
#  going on until you lose all your money or interrupt
#  the program.
#
# "The original BASIC program author was Bill Palmby
#  of Prairie View, Illinois."
#
# Python port by Jeff Jetton, 2019
#
########################################################


import random

# "You may alter [the following statement] if you want
#  to start with more or less than $100."
DEFAULT_BANKROLL = 100

# functions
def deal_card_num():
    """Get card number"""
    return random.randint(0, 12)


def get_card_name(number):
    """Get card name"""
    card_names = (
        " 2",
        " 3",
        " 4",
        " 5",
        " 6",
        " 7",
        " 8",
        " 9",
        " 10",
        "Jack",
        "Queen",
        "King",
        "Ace",
    )
    return card_names[number]


def display_bankroll(bank_roll):
    """Print current bankroll"""
    if BANK_ROLL > 0:
        print("You now have %s dollars\n" % bank_roll)


# Display initial title and instructions
print("\n           Acey Ducey Card Game")
print("Creative Computing  Morristown, New Jersey")
print("\n\n")
print("Acey-Ducey is played in the following manner")
print("The dealer (computer) deals two cards face up")
print("You have an option to bet or not bet depending")
print("on whether or not you feel the card will have")
print("a value between the first two.")
print("If you do not want to bet, input a 0")


# Loop for series of multiple games
KEEP_PLAYING = True
while KEEP_PLAYING:

    # Initialize bankroll at start of each game
    BANK_ROLL = DEFAULT_BANKROLL
    display_bankroll(BANK_ROLL)

    # Loop for a single round.  Repeat until out of money.
    while BANK_ROLL > 0:

        # Deal out dealer cards
        print("Here are your next two cards")
        dealer1 = deal_card_num()
        # If the cards match, we redeal 2nd card until they don't
        dealer2 = dealer1
        while dealer1 == dealer2:
            dealer2 = deal_card_num()
        # Organize the cards in order if they're not already
        if dealer1 >= dealer2:
            (dealer1, dealer2) = (dealer2, dealer1)  # Ya gotta love Python!
        # Show dealer cards to the player
        # (use card name rather than internal number)
        print(get_card_name(dealer1))
        print(get_card_name(dealer2) + "\n")

        # Get and handle player bet choice
        BET_IS_VALID = False
        while not BET_IS_VALID:
            curr_bet = input("What is your bet? ")
            try:
                curr_bet = int(curr_bet)
            except ValueError:
                # Bad input? Just loop back up and ask again...
                pass
            else:
                if curr_bet == 0:
                    BET_IS_VALID = True
                    print("Chicken!!\n")
                elif curr_bet > BANK_ROLL:
                    print("Sorry, my friend but you bet too much")
                    print("You have only %s dollars to bet\n" % BANK_ROLL)
                else:
                    # Deal player card
                    BET_IS_VALID = True
                    player = deal_card_num()
                    print(get_card_name(player))

                    # Did we win?
                    if dealer1 < player < dealer2:
                        print("You win!!!")
                        BANK_ROLL += curr_bet
                    else:
                        print("Sorry, you lose")
                        BANK_ROLL -= curr_bet

                    # Update player on new bankroll level
                    display_bankroll(BANK_ROLL)

    # End of loop for a single round

    print("\n\nSorry, friend but you blew your wad")
    player_response = input("Try again (yes or no) ")
    if player_response.lower() == "yes":
        print()
    else:
        KEEP_PLAYING = False

# End of multiple game loop

print("OK Hope you had fun\n")


########################################################
#
# Porting notes:
#
#   The original BASIC version had a variable named N
#   that was initialized to 100 and then never used.
#   Maybe it did something in feature that was edited
#   out of the final version used in the book?
#
#   The original program simply generated random numbers
#   for each card.  It did not simulate a true card deck,
#   where the dealing of a card eliminates it from the
#   deck and reduces the chances of the same value
#   being drawn.  This "infinite deck" logic (or "deal,
#   with replacement after") has NOT been changed.
#
#   In the interests of historical fidelity, the bug
#   in the original BASIC listing that let you input a
#   negative bet value has been faithfully reproduced.
#   This lets the player lose money when they win and
#   earn money when they lose! :-)
#
#
# Ideas for Modifications
#
#   Give the user the ability to quit the game, perhaps
#   by typing "quit" instead of making a bet.  Provide a
#   final assessment based on how much of the original
#   bankroll they have left.
#
#   Or have the game run for a set number of rounds or
#   until a certain bankroll goal is attained.
#
#   Implement an "ante"--a set amount the player has to
#   bet each time rather than having the option to lay
#   out entirely.
#
#   See "porting notes" above about negative bet values.
#   How would you fix this?
#
#   When the player "chickens out", show them what the
#   next card would've been and point out whether they
#   made a good or bad decision.
#
#   In what situations are the odds of winning high
#   enough to justify making a bet? Create a cheat mode
#   where the program identifies these situations and
#   lets the player know.
#
#   Change the card dealing to simulate deals from a
#   single deck (or a user-selectable number of decks).
#
#   Implement a two-player mode where players take turns
#   betting (or both bet on the same dealer cards and
#   get their own player card dealt).
#
########################################################
