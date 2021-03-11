#!/usr/bin/env python3
# 23 Matches
#
# Converted from BASIC to Python by Trevor Hobson

import random


def play_game():
    """Play one round of the game"""

    matches = 23
    humans_turn = random.randint(0, 1) == 1
    if humans_turn:
        print("Tails! You go first.\n")
        prompt_human = "How many do you wish to remove "
    else:
        print("Heads! I win! Ha! Ha!")
        print("Prepare to lose, meatball-nose!!")

    choice_human = 2
    while matches > 0:
        if humans_turn:
            choice_human = 0
            if matches == 1:
                choice_human = 1
            while choice_human == 0:
                try:
                    choice_human = int(input(prompt_human))
                    if choice_human not in [1, 2, 3] or choice_human > matches:
                        choice_human = 0
                        print("Very funny! Dummy!")
                        print("Do you want to play or goof around?")
                        prompt_human = "Now, how many matches do you want "
                except ValueError:
                    print("Please enter a number.")
                    prompt_human = "How many do you wish to remove "
            matches = matches - choice_human
            if matches == 0:
                print("You poor boob! You took the last match! I gotcha!!")
                print("Ha ! Ha ! I beat you !!\n")
                print("Good bye loser!")
            else:
                print("There are now", matches, "matches remaining.\n")
        else:
            choice_computer = 4 - choice_human
            if matches == 1:
                choice_computer = 1
            elif 1 < matches < 4:
                choice_computer = matches - 1
            matches = matches - choice_computer
            if matches == 0:
                print("You won, floppy ears !")
                print("Think you're pretty smart !")
                print("Let's play again and I'll blow your shoes off !!")
            else:
                print("My turn ! I remove", choice_computer, "matches")
                print("The number of matches is now", matches, "\n")
        humans_turn = not humans_turn
        prompt_human = "Your turn -- you may take 1, 2 or 3 matches.\nHow many do you wish to remove "


def main():
    print(" " * 31 + "23 MATCHHES")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
    print("This is a game called '23 Matches'.\n")
    print("When it is your turn, you may take one, two, or three")
    print("matches. The object of the game is not to have to take")
    print("the last match.\n")
    print("Let's flip a coin to see who goes first.")
    print("If it comes up heads, I will win the toss.\n")

    keep_playing = True
    while keep_playing:
        play_game()
        keep_playing = input(
            "\nPlay again? (yes or no) ").lower().startswith("y")


if __name__ == "__main__":
    main()
