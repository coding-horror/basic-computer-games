########################################################
#
# Slots
#
# From Basic Computer Games (1978)
#
#    "The slot machine or one-arm bandit is a mechanical
#   device that will absorb coins just about as fast as
#   you can feed it. After inserting a coin, you pull a
#   handle that sets three indepent reels spining. If the
#   reels stop with certain symbols appearing in the pay
#   line, you get a certain payoff. The original slot
#   machine, called the Liberty bell, was invented in 1895
#   by Charles Fey in San Francisco. Fey refused to sell
#   or lease the manufacturing rights, so H.S. Mills in
#   Chicago built a similar, but much improved, machine
#   called the Operators Bell. This has survived nearly
#   unchanged to today.
#     On the operators Bell and other standard slot
#   machines, there are 20 symbols on each wheel but they
#   are not distributed evenly among the objects(cherries,
#   bar, apples, etc). Of the 8000 possible combinations,
#   the expected payoff(to the player) is 7049 or $89.11
#   for every $100.00 put in, one of the lowest expected
#   payoffs of all casino games.
#     In the program here, the payoff is considerably more
#   liberal; indeed it appears to favor the player by 11%
#   -- i.e., an expected payoff of $111 for each $100 bet."
#     The program was originally written by Fred Mirabelle
#   and Bob Harper
#
########################################################

from random import choices
from collections import Counter
import sys


def initial_message():
    print(" "*30 + "Slots")
    print(" "*15 + "Creative Computing Morrison, New Jersey")
    print("\n"*3)
    print("You are in the H&M Casino, in front of one of our")
    print("one-arm Bandits. Bet from $1 to $100.")
    print("To pull the arm, punch the return key after making your bet.")


def input_betting():
    print("\n")
    b = -1
    while b < 1 or b > 100:
        try:
            b = int(input("Your bet:"))
        except ValueError:
            b = -1
        if b > 100:
            print("House limits are $100")
        elif b < 1:
            print("Minium bet is $1")
    beeping()
    return int(b)


def beeping():
    # Function to produce a beep sound.
    # In the original program is the subroutine at line 1270
    for _ in range(5):
        sys.stdout.write('\a')
        sys.stdout.flush()


def spin_wheels():
    possible_fruits = ["Bar", "Bell", "Orange", "Lemon", "Plum", "Cherry"]
    wheel = choices(possible_fruits, k=3)

    print(*wheel)
    beeping()

    return wheel


def adjust_profits(wheel, m, profits):
    # we remove the duplicates
    s = set(wheel)

    if len(s) == 1:
        # the three fruits are the same
        fruit = s.pop()

        if fruit == "Bar":
            print("\n***Jackpot***")
            profits = (((100*m)+m)+profits)
        else:
            print("\n**Top Dollar**")
            profits = (((10*m)+m)+profits)

        print("You Won!")
    elif len(s) == 2:
        # two fruits are equal
        c = Counter(wheel)
        # we get the fruit that appears two times
        fruit = sorted(c.items(), key=lambda x: x[1], reverse=True)[0][0]

        if fruit == "Bar":
            print("\n*Double Bar*")
            profits = (((5*m)+m)+profits)
        else:
            print("\nDouble!!")
            profits = (((2*m)+m)+profits)

        print("You Won!")
    else:
        # three different fruits
        print("\nYou Lost.")
        profits = profits - m

    return profits


def final_message(profits):
    if profits < 0:
        print("Pay up!  Please leave your money on the terminal")
    elif profits == 0:
        print("Hey, You broke even.")
    else:
        print("Collect your winings from the H&M cashier.")


profits = 0
keep_betting = True

initial_message()
while keep_betting:
    m = input_betting()
    w = spin_wheels()
    profits = adjust_profits(w, m, profits)

    print("Your standings are ${}".format(profits))
    answer = input("Again?")

    try:
        if not answer[0].lower() == "y":
            keep_betting = False
    except IndexError:
        keep_betting = False

final_message(profits)


######################################################################
#
# Porting notes
#
#   The selections of the fruits(Bar, apples, lemon, etc.) are made
#   with equal probability, accordingly to random.choices documentation.
#   It could be added a weights list to the function and therefore
#   adjust the expected payoff
#
######################################################################
