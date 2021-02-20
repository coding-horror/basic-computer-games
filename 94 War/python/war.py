#!/usr/bin/env python3
# WAR
#
# Converted from BASIC to Python by Trevor Hobson

import random


def card_value(input):
    return ["2", "3", "4", "5", "6", "7", "8",
            "9", "10", "J", "Q", "K", "A"].index(input.split("-")[1])


cards = ["S-2", "H-2", "C-2", "D-2", "S-3", "H-3", "C-3", "D-3",
         "S-4", "H-4", "C-4", "D-4", "S-5", "H-5", "C-5", "D-5",
         "S-6", "H-6", "C-6", "D-6", "S-7", "H-7", "C-7", "D-7",
         "S-8", "H-8", "C-8", "D-8", "S-9", "H-9", "C-9", "D-9",
         "S-10", "H-10", "C-10", "D-10", "S-J", "H-J", "C-J", "D-J",
         "S-Q", "H-Q", "C-Q", "D-Q", "S-K", "H-K", "C-K", "D-K",
         "S-A", "H-A", "C-A", "D-A"]


def play_game():
    """Play one round of the game"""

    random.shuffle(cards)
    score_you = 0
    score_computer = 0
    cards_left = 52
    for round in range(26):
        print()
        card_you = cards[round]
        card_computer = cards[round * 2]
        print("You:", card_you, " " * (8 - len(card_you)) +
              "Computer:", card_computer)
        value_you = card_value(card_you)
        value_computer = card_value(card_computer)
        if value_you > value_computer:
            score_you += 1
            print("You win. You have", score_you,
                  "and the computer has", score_computer)
        elif value_computer > value_you:
            score_computer += 1
            print("The computer wins!!! You have", score_you,
                  "and the computer has", score_computer)
        else:
            print("Tie. No score change.")
        cards_left -= 2
        if cards_left > 2:
            if input("Do you want to continue ").lower().startswith("n"):
                break
    if cards_left == 0:
        print("\nWe have run out of cards. Final score: You:",
              score_you, "the computer:", score_computer)
    print("\nThanks for playing. It was fun.")


def main():
    print(" " * 33 + "WAR")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
    print("This is the card game of war. Each card is given by suit-#")
    print("as S-7 for Spade 7.")
    if input("Do you want directions ").lower().startswith("y"):
        print("The computer gives you and it a 'card'. The higher card")
        print("(numerically) wins. The game ends when you choose not to")
        print("contine or when you have finished the pack.")

    keep_playing = True
    while keep_playing:
        play_game()
        keep_playing = input(
            "\nPlay again? (yes or no) ").lower().startswith("y")


if __name__ == "__main__":
    main()
