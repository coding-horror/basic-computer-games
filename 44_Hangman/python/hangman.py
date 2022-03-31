#!/usr/bin/env python3

"""
HANGMAN

Converted from BASIC to Python by Trevor Hobson and Daniel Piron
"""

import random
from typing import List


class Canvas:
    """For drawing text-based figures"""

    def __init__(self, width: int = 12, height: int = 12, fill: str = " ") -> None:
        self._buffer = []
        for _ in range(height):
            line = []
            for _ in range(width):
                line.append("")
            self._buffer.append(line)

        self.clear()

    def clear(self, fill: str = " ") -> None:
        for row in self._buffer:
            for x in range(len(row)):
                row[x] = fill

    def render(self) -> str:
        lines = []
        for line in self._buffer:
            # Joining by the empty string ("") smooshes all of the
            # individual characters together as one line.
            lines.append("".join(line))
        return "\n".join(lines)

    def put(self, s: str, x: int, y: int) -> None:
        # In an effort to avoid distorting the drawn image, only write the
        # first character of the given string to the buffer.
        self._buffer[y][x] = s[0]


def init_gallows(canvas: Canvas) -> None:
    for i in range(12):
        canvas.put("X", 0, i)
    for i in range(7):
        canvas.put("X", i, 0)
    canvas.put("X", 6, 1)


def draw_head(canvas: Canvas) -> None:
    canvas.put("-", 5, 2)
    canvas.put("-", 6, 2)
    canvas.put("-", 7, 2)
    canvas.put("(", 4, 3)
    canvas.put(".", 5, 3)
    canvas.put(".", 7, 3)
    canvas.put(")", 8, 3)
    canvas.put("-", 5, 4)
    canvas.put("-", 6, 4)
    canvas.put("-", 7, 4)


def draw_body(canvas: Canvas) -> None:
    for i in range(5, 9, 1):
        canvas.put("X", 6, i)


def draw_right_arm(canvas: Canvas) -> None:
    for i in range(3, 7):
        canvas.put("\\", i - 1, i)


def draw_left_arm(canvas: Canvas) -> None:
    canvas.put("/", 10, 3)
    canvas.put("/", 9, 4)
    canvas.put("/", 8, 5)
    canvas.put("/", 7, 6)


def draw_right_leg(canvas: Canvas) -> None:
    canvas.put("/", 5, 9)
    canvas.put("/", 4, 10)


def draw_left_leg(canvas: Canvas) -> None:
    canvas.put("\\", 7, 9)
    canvas.put("\\", 8, 10)


def draw_left_hand(canvas: Canvas) -> None:
    canvas.put("\\", 10, 2)


def draw_right_hand(canvas: Canvas) -> None:
    canvas.put("/", 2, 2)


def draw_left_foot(canvas: Canvas) -> None:
    canvas.put("\\", 9, 11)
    canvas.put("-", 10, 11)


def draw_right_foot(canvas: Canvas) -> None:
    canvas.put("-", 2, 11)
    canvas.put("/", 3, 11)


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
    ("Here's the other foot -- you're hung!!", draw_right_foot),
)


words = [
    "GUM",
    "SIN",
    "FOR",
    "CRY",
    "LUG",
    "BYE",
    "FLY",
    "UGLY",
    "EACH",
    "FROM",
    "WORK",
    "TALK",
    "WITH",
    "SELF",
    "PIZZA",
    "THING",
    "FEIGN",
    "FIEND",
    "ELBOW",
    "FAULT",
    "DIRTY",
    "BUDGET",
    "SPIRIT",
    "QUAINT",
    "MAIDEN",
    "ESCORT",
    "PICKAX",
    "EXAMPLE",
    "TENSION",
    "QUININE",
    "KIDNEY",
    "REPLICA",
    "SLEEPER",
    "TRIANGLE",
    "KANGAROO",
    "MAHOGANY",
    "SERGEANT",
    "SEQUENCE",
    "MOUSTACHE",
    "DANGEROUS",
    "SCIENTIST",
    "DIFFERENT",
    "QUIESCENT",
    "MAGISTRATE",
    "ERRONEOUSLY",
    "LOUDSPEAKER",
    "PHYTOTOXIC",
    "MATRIMONIAL",
    "PARASYMPATHOMIMETIC",
    "THIGMOTROPISM",
]


def play_game(guess_target: str) -> None:
    """Play one round of the game"""
    wrong_guesses = 0
    guess_progress = ["-"] * len(guess_target)
    guess_list: List[str] = []

    gallows = Canvas()
    init_gallows(gallows)

    guess_count = 0
    while True:
        print("Here are the letters you used:")
        print(",".join(guess_list) + "\n")
        print("".join(guess_progress) + "\n")
        guess_letter = ""
        guess_word = ""
        while guess_letter == "":

            guess_letter = input("What is your guess? ").upper()[0]
            if not guess_letter.isalpha():
                guess_letter = ""
                print("Only letters are allowed!")
            elif guess_letter in guess_list:
                guess_letter = ""
                print("You guessed that letter before!")

        guess_list.append(guess_letter)
        guess_count += 1
        if guess_letter in guess_target:
            indices = [
                i for i, letter in enumerate(guess_target) if letter == guess_letter
            ]
            for i in indices:
                guess_progress[i] = guess_letter
            if "".join(guess_progress) == guess_target:
                print("You found the word!")
                break
            else:
                print("\n" + "".join(guess_progress) + "\n")
                while guess_word == "":
                    guess_word = input("What is your guess for the word? ").upper()
                    if not guess_word.isalpha():
                        guess_word = ""
                        print("Only words are allowed!")
                if guess_word == guess_target:
                    print("Right!! It took you", guess_count, "guesses!")
                    break
        else:
            comment, draw_bodypart = PHASES[wrong_guesses]

            print(comment)
            draw_bodypart(gallows)
            print(gallows.render())

            wrong_guesses += 1
            print("Sorry, that letter isn't in the word.")

            if wrong_guesses == 10:
                print("Sorry, you lose. The word was " + guess_target)
                break


def main() -> None:
    print(" " * 32 + "HANGMAN")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")

    random.shuffle(words)
    current_word = 0
    word_count = len(words)

    keep_playing = True
    while keep_playing:

        play_game(words[current_word])
        current_word += 1

        if current_word == word_count:
            print("You did all the words!!")
            keep_playing = False
        else:
            keep_playing = (
                input("Want another word? (yes or no) ").lower().startswith("y")
            )

    print("It's been fun! Bye for now.")


if __name__ == "__main__":
    main()
