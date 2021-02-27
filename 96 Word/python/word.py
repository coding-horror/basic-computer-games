#!/usr/bin/env python3
# WORD
#
# Converted from BASIC to Python by Trevor Hobson

import random

words = ["DINKY", "SMOKE", "WATER", "GRASS", "TRAIN", "MIGHT", "FIRST",
         "CANDY", "CHAMP", "WOULD", "CLUMP", "DOPEY"]


def play_game():
    """Play one round of the game"""

    random.shuffle(words)
    target_word = words[0]
    guess_count = 0
    guess_progress = ["-"] * 5

    print("You are starting a new game...")
    while True:
        guess_word = ""
        while guess_word == "":
            guess_word = input("\nGuess a five letter word. ").upper()
            if guess_word == "?":
                break
            elif not guess_word.isalpha() or len(guess_word) != 5:
                guess_word = ""
                print("You must guess a five letter word. Start again.")
        guess_count += 1
        if guess_word == "?":
            print("The secret word is", target_word)
            break
        else:
            common_letters = ""
            matches = 0
            for i in range(5):
                for j in range(5):
                    if guess_word[i] == target_word[j]:
                        matches += 1
                        common_letters = common_letters + guess_word[i]
                        if i == j:
                            guess_progress[j] = guess_word[i]
            print("There were", matches,
                  "matches and the common letters were... " + common_letters)
            print(
                "From the exact letter matches, you know............ " + "".join(guess_progress))
            if "".join(guess_progress) == guess_word:
                print("\nYou have guessed the word. It took",
                      guess_count, "guesses!")
                break
            elif matches == 0:
                print("\nIf you give up, type '?' for you next guess.")


def main():
    print(" " * 33 + "WORD")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")

    print("I am thinking of a word -- you guess it. I will give you")
    print("clues to help you get it. Good luck!!\n")

    keep_playing = True
    while keep_playing:
        play_game()
        keep_playing = input(
            "\nWant to play again? ").lower().startswith("y")


if __name__ == "__main__":
    main()
