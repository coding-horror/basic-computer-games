"""
Russian Roulette

From Basic Computer Games (1978)

   In this game, you are given by the computer a
  revolver loaded with one bullet and five empty
  chambers. You spin the chamber and pull the trigger
  by inputting a "1", or, if you want to quit, input
  a "2". You win if you play ten times and are still
  alive.
   Tom Adametx wrote this program while a student at
  Curtis Jr. High School in Sudbury, Massachusetts.
"""


from random import random

NUMBER_OF_ROUNDS = 9


def initial_message() -> None:
    print(" " * 28 + "Russian Roulette")
    print(" " * 15 + "Creative Computing  Morristown, New Jersey\n\n\n")
    print("This is a game of >>>>>>>>>>Russian Roulette.\n")
    print("Here is a Revolver.")


def parse_input() -> int:
    while True:
        try:
            i = int(input("? "))
            return i
        except ValueError:
            print("Number expected...")


def main() -> None:
    initial_message()
    while True:
        dead = False
        n = 0
        print("Type '1' to Spin chamber and pull trigger")
        print("Type '2' to Give up")
        print("Go")
        while not dead:
            i = parse_input()

            if i == 2:
                break

            if random() > 0.8333333333333334:
                dead = True
            else:
                print("- CLICK -\n")
                n += 1

            if n > NUMBER_OF_ROUNDS:
                break
        if dead:
            print("BANG!!!!!   You're Dead!")
            print("Condolences will be sent to your relatives.\n\n\n")
            print("...Next victim...")
        else:
            if n > NUMBER_OF_ROUNDS:
                print("You win!!!!!")
                print("Let someone else blow his brain out.\n")
            else:
                print("     Chicken!!!!!\n\n\n")
                print("...Next victim....")


if __name__ == "__main__":
    main()

########################################################
# Porting Notes
#
#    Altough the description says that accepts "1" or "2",
#   the original game accepts any number as input, and
#   if it's different of "2" the program considers
#   as if the user had passed "1". That feature was
#   kept in this port.
#    Also, in the original game you must "pull the trigger"
#   11 times instead of 10 in orden to win,
#   given that N=0 at the beginning and the condition to
#   win is "IF N > 10 THEN  80". That was fixed in this
#   port, asking the user to pull the trigger only ten
#   times, tough the number of round can be set changing
#   the constant NUMBER_OF_ROUNDS.
#
########################################################
