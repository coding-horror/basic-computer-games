#!/usr/bin/env python3
# TRAP
#
# STEVE ULLMAN, 8-1-72
# Converted from BASIC to Python by Trevor Hobson

import random

number_max = 100
guess_max = 6


def play_game():
    """Play one round of the game"""

    number_computer = random.randint(1, number_max)
    turn = 0
    while True:
        turn += 1
        user_guess = [-1, -1]
        while user_guess == [-1, -1]:
            try:
                user_input = [int(item) for item in input(
                    "\nGuess # " + str(turn) + " ? ").split(",")]
                if len(user_input) == 2:
                    if sum(1 < x < number_max for x in user_input) == 2:
                        user_guess = user_input
                    else:
                        raise ValueError
                else:
                    raise ValueError
            except (ValueError, IndexError):
                print("Please enter a valid guess.")
        if user_guess[0] > user_guess[1]:
            user_guess[0], user_guess[1] = user_guess[1], user_guess[0]
        if user_guess[0] == user_guess[1] == number_computer:
            print("You got it!!!")
            break
        elif user_guess[0] <= number_computer <= user_guess[1]:
            print("You have trapped my number.")
        elif number_computer < user_guess[0]:
            print("My number is smaller than your trap numbers.")
        else:
            print("My number is larger than your trap numbers.")
        if turn == guess_max:
            print("That's", turn, "guesses. The number was", number_computer)
            break


def main():
    print(" " * 34 + "TRAP")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
    if input("Instructions ").lower().startswith("y"):
        print("\nI am thinking of a number between 1 and", number_max)
        print("try to guess my number. On each guess,")
        print("you are to enter 2 numbers, trying to trap")
        print("my number between the two numbers. I will")
        print("tell you if you have trapped my number, if my")
        print("number is larger than your two numbers, or if")
        print("my number is smaller than your two numbers.")
        print("If you want to guess one single number, type")
        print("your guess for both your trap numbers.")
        print("You get", guess_max, "guesses to get my number.")

    keep_playing = True
    while keep_playing:
        play_game()
        keep_playing = input(
            "\nTry again. ").lower().startswith("y")


if __name__ == "__main__":
    main()
