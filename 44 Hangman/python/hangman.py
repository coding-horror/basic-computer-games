#!/usr/bin/env python3
# HANGMAN
#
# Converted from BASIC to Python by Trevor Hobson and Daniel Piron

import random


class Canvas:
    ''' For drawing text-based figures '''

    def __init__(self, width=12, height=12, fill=' '):
        self._buffer = []
        for _ in range(height):
            line = []
            for _ in range(width):
                line.append('')
            self._buffer.append(line)

        self.clear()

    def clear(self, fill=' '):
        for row in self._buffer:
            for x in range(len(row)):
                row[x] = fill

    def render(self):
        lines = []
        for line  in self._buffer:
            # Joining by the empty string ('') smooshes all of the
            # individual characters together as one line.
            lines.append(''.join(line))
        return '\n'.join(lines)

    def put(self, s, x, y):
        # In an effort to avoid distorting the drawn image, only write the
        # first character of the given string to the buffer.
        self._buffer[y][x] = s[0]


def draw_gallows(canvas):
    for i in range(12):
        canvas.put('X', 0, i)
    for i in range(7):
        canvas.put('X', i, 0)
    canvas.put('X', 6, 1)


def draw_head(canvas):
    canvas.put('-', 5, 2)
    canvas.put('-', 6, 2)
    canvas.put('-', 7, 2)
    canvas.put('(', 4, 3)
    canvas.put('.', 5, 3)
    canvas.put('.', 7, 3)
    canvas.put(')', 8, 3)
    canvas.put('-', 5, 4)
    canvas.put('-', 6, 4)
    canvas.put('-', 7, 4)


def draw_body(canvas):
    for i in range(5, 9, 1):
        canvas.put('X', 6, i)


def draw_right_arm(canvas):
    for i in range(3, 7):
        canvas.put('\\', i - 1, i)


def draw_left_arm(canvas):
    canvas.put('/', 10, 3)
    canvas.put('/', 9, 4)
    canvas.put('/', 8, 5)
    canvas.put('/', 7, 6)


def draw_right_leg(canvas):
    canvas.put('/', 5, 9)
    canvas.put('/', 4, 10)


def draw_left_leg(canvas):
    canvas.put('\\', 7, 9)
    canvas.put('\\', 8, 10)


def draw_left_hand(canvas):
    canvas.put('\\', 10, 2)


def draw_right_hand(canvas):
    canvas.put('/', 2, 2)


def draw_left_foot(canvas):
    canvas.put('\\', 9, 11)
    canvas.put('-', 10, 11)


def draw_right_foot(canvas):
    canvas.put('-', 2, 11)
    canvas.put('/', 3, 11)


PHASES = (
    ("First, we draw a head", draw_head),
    ("Now we draw a body.", draw_body),
    ("Next we draw an arm.", draw_right_arm),
    ("this time it's the other arm.", draw_left_arm),
    ("Now, let's draw the right leg.", draw_right_leg),
    ("This time we draw the left leg.", draw_left_leg),
    ("Now we put up a hand.", draw_left_hand),
    ("Next the other hand.", draw_right_hand),
    ("Now we draw one foot", draw_left_foot),
    ("Here's the other foot -- you're hung!!", draw_right_foot)
)


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

    gallows = Canvas()
    draw_gallows(gallows)

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
            comment, drawingBodyPart = PHASES[guessWrong]

            print(comment)
            drawingBodyPart(gallows)
            print(gallows.render())

            guessWrong = guessWrong + 1
            print("Sorry, that letter isn't in the word.")

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
