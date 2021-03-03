from random import random

NUMBER_OF_ROUNDS = 9


def initial_message():
    print(" " * 28 + "Russian Roulette")
    print(" " * 15 + "Creative Computing  Morristown, New Jersey\n\n\n")
    print("This is a game of >>>>>>>>>>Russian Roulette.\n")
    print("Here is a Revolver.")


def parse_input():
    correct_input = False
    while not correct_input:
        try:
            i = int(input('?'))
            correct_input = True
        except ValueError:
            print('Number expected...')
    return i


initial_message()
while True:
    dead = False
    n = 0
    print("Type \'1\' to Spin chamber and pull trigger")
    print("Type \'2\' to Give up")
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
            print("Let someone else blow his brain out.")
        else:
            print("     Chicken!!!!!\n\n\n")
            print("...Next victim....")
