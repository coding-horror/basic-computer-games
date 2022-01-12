# Original BASIC version as published in Basic Computer Games (1978)
# https://www.atariarchives.org/basicgames/showpage.php?page=55
#
# Converted to Python by Anson VanDoren in 2021

import math
import random


def show_welcome():
    # Clear screen. chr(27) is `Esc`, and the control sequence is
    # initiated by Ctrl+[
    # `J` is "Erase in Display" and `2J` means clear the entire screen
    print(chr(27) + "[2J")

    # Show the intro text, centered
    print("DEPTH CHARGE".center(45))
    print("Creative Computing  Morristown, New Jersey\n\n".center(45))


def get_num_charges():
    print("Depth Charge game\n")
    while True:
        search_area = input("Dimensions of search area? ")

        # Make sure the input is an integer
        try:
            search_area = int(search_area)
            break
        except ValueError:
            print("Must enter an integer number. Please try again...")

    num_charges = int(math.log2(search_area)) + 1
    return search_area, num_charges


def ask_for_new_game():
    answer = input("Another game (Y or N): ")
    if answer.lower().strip()[0] == 'y':
        start_new_game()
    else:
        print("OK. Hope you enjoyed yourself")
        exit()


def show_shot_result(shot, location):
    result = "Sonar reports shot was "
    if shot[1] > location[1]:  # y-direction
        result += "north"
    elif shot[1] < location[1]:  # y-direction
        result += "south"
    if shot[0] > location[0]:  # x-direction
        result += "east"
    elif shot[0] < location[0]:  # x-direction
        result += "west"
    if shot[1] != location[1] or shot[0] != location[0]:
        result += " and "

    if shot[2] > location[2]:
        result += "too low."
    elif shot[2] < location [2]:
        result += "too high."
    else:
        result += "depth OK."
    print(result)
    return


def get_shot_input():
    while True:
        raw_guess = input("Enter coordinates: ")
        try:
            x, y, z = raw_guess.split()
        except ValueError:
            print("Please enter coordinates separated by spaces")
            print(f"Example: 3 2 1")
            continue
        try:
            x, y, z = [int(num) for num in [x, y, z]]
            return x, y, z
        except ValueError:
            print("Please enter whole numbers only")


def play_game(search_area, num_charges):
    print("\nYou are the captain of the destroyer USS Computer.")
    print("An enemy sub has been causing you trouble. Your")
    print(f"mission is to destroy it. You have {num_charges} shots.")
    print("Specify depth charge explosion point with a")
    print("trio of numbers -- the first two are the")
    print("surface coordinates; the third is the depth.")
    print("\nGood luck!\n")

    # Generate position for submarine
    a, b, c = [random.randint(0, search_area) for _ in range(3)]

    # Get inputs until win or lose
    for i in range(num_charges):
        print(f"\nTrial #{i+1}")
        x, y, z = get_shot_input()

        if (x, y, z) == (a, b, c):
            print(f"\nB O O M ! ! You found it in {i+1} tries!\n")
            ask_for_new_game()
        else:
            show_shot_result((x, y, z), (a, b, c))

    # out of shots
    print("\nYou have been torpedoed! Abandon ship!")
    print(f"The submarine was at {a} {b} {c}")
    ask_for_new_game()


def start_new_game():
    search_area, num_charges = get_num_charges()
    play_game(search_area, num_charges)


if __name__ == '__main__':
    start_new_game()
