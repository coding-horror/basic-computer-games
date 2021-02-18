#!/usr/bin/env python3

# HANGMAN
#
# Converted from BASIC to Python by Trevor Hobson

import random

print(" " * 32 + "HANGMAN")
print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")

words = ["GUM", "SIN", "FOR", "CRY", "LUG", "BYE", "FLY",
         "UGLY", "EACH", "FROM", "WORK", "TALK", "WITH", "SELF",
         "PIZZA", "THING", "FEIGN", "FIEND", "ELBOW", "FAULT", "DIRTY",
         "BUDGET", "SPIRIT", "QUAINT", "MAIDEN", "ESCORT", "PICKAX",
         "EXAMPLE", "TENSION", "QUININE", "KIDNEY", "REPLICA", "SLEEPER",
         "TRIANGLE", "KANGAROO", "MAHOGANY", "SERGEANT", "SEQUENCE",
         "MOUSTACHE", "DANGEROUS", "SCIENTIST", "DIFFERENT", "QUIESCENT",
         "MAGISTRATE", "ERRONEOUSLY", "LOUDSPEAKER", "PHYTOTOXIC",
         "MATRIMONIAL", "PARASYMPATHOMIMETIC", "THIGMOTROPISM"]


def play_game(guessTarget):
    """Play the game"""
    guessWrong = 0
    guessProgress = ["-"] * len(guessTarget)
    guessList = []
    gallows = [([" "] * 12) for i in range(12)]
    for i in range(12):
        gallows[i][0] = "X"
    for i in range(7):
        gallows[0][i] = "X"
    gallows[1][6] = "X"
    guessCount = 0
    while True:
        print("Here are the letters you used:")
        print(",".join(guessList) + "\n")
        print("".join(guessProgress) + "\n")
        guessLetter = ""
        guessWord = ""
        while guessLetter == "":
            guessLetter = input("What is your guess? ").upper()[0]
            if not guessLetter.isalpha():
                guessLetter = ""
                print("Only letters are allowed!")
            elif guessLetter in guessList:
                guessLetter = ""
                print("You guessed that letter before!")
        guessList.append(guessLetter)
        guessCount = guessCount + 1
        if guessLetter in guessTarget:
            indices = [i for i, letter in enumerate(guessTarget) if letter == guessLetter]
            for i in indices:
                guessProgress[i] = guessLetter
            if guessProgress == guessTarget:
                print("You found the word!")
                break
            else:
                print("\n" + "".join(guessProgress) + "\n")
                while guessWord == "":
                    guessWord = input("What is your guess for the word? ").upper()
                    if not guessWord.isalpha():
                        guessWord = ""
                        print("Only words are allowed!")
                if guessWord == guessTarget:
                    print("Right!! It took you", guessCount, "guesses!")
                    break
        else:
            guessWrong = guessWrong + 1
            print("Sorry, that letter isn't in the word.")
            if guessWrong == 1:
                print("First, we draw the head.")
                for i in range(5, 8):
                    gallows[2][i] = "-"
                    gallows[4][i] = "-"
                gallows[3][4] = "("
                gallows[3][5] = "."
                gallows[3][7] = "."
                gallows[3][8] = ")"
            elif guessWrong == 2:
                print("Now we draw a body.")
                for i in range(5, 9):
                    gallows[i][6] = "X"
            elif guessWrong == 3:
                print("Next we draw an arm.")
                for i in range(3, 7):
                    gallows[i][i-1] = "\\"
            elif guessWrong == 4:
                print("This time it's the other arm.")
                for i in range(3, 7):
                    gallows[i][13-i] = "/"
            elif guessWrong == 5:
                print("Now, let's draw the right leg.")
                gallows[9][5] = "/"
                gallows[10][4] = "/"
            elif guessWrong == 6:
                print("This time we draw the left leg.")
                gallows[9][7] = "\\"
                gallows[10][8] = "\\"
            elif guessWrong == 7:
                print("Now we put up a hand.")
                gallows[2][10] = "\\"
            elif guessWrong == 8:
                print("Next the other hand.")
                gallows[2][2] = "/"
            elif guessWrong == 9:
                print("Now we draw one foot.")
                gallows[11][9] = "\\"
                gallows[11][10] = "-"
            elif guessWrong == 10:
                print("Here's the other foot -- You're hung!!.")
                gallows[11][2] = "-"
                gallows[11][3] = "/"
            for i in range(12):
                print("".join(gallows[i]))
            print("\n")
            if guessWrong == 10:
                print("Sorry, you lose. The word was " + guessTarget)
                break


def main():
    """Main"""

    random.shuffle(words)
    wordCurrent = 0
    wordCount = 49

    keep_playing = True
    while keep_playing:
        play_game(words[wordCurrent])
        wordCurrent = wordCurrent + 1
        if wordCurrent >= wordCount:
            print("You did all the words!!")
            keep_playing = False
        else:
            keep_playing = input("Want another word? (yes or no) ").lower().startswith("y")
    print("It's been fun! Bye for now.")


if __name__ == "__main__":
    main()
    