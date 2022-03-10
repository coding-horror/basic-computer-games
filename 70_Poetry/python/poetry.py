"""
POETRY

A poetry generator

Ported by Dave LeCompte
"""

# PORTING EDITORIAL NOTE:
#
# The original code is a pretty convoluted mesh of GOTOs and global
# state. This adaptation pulls things apart into phrases, but I have
# left the variables as globals, which makes goes against decades of
# wisdom that global state is bad.

PAGE_WIDTH = 64

import random

# globals
u = 0
i = 0
j = 0
k = 0
phrase = 1
line = ""


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def process_phrase_1():
    global line

    line_1_options = [
        "MIDNIGHT DREARY",
        "FIERY EYES",
        "BIRD OR FIEND",
        "THING OF EVIL",
        "PROPHET",
    ]

    line = line + line_1_options[i]
    return line


def process_phrase_2():
    global line
    global u

    line_2_options = [
        ("BEGUILING ME", 2),
        ("THRILLED ME", None),
        ("STILL SITTING....", None),
        ("NEVER FLITTING", 2),
        ("BURNED", None),
    ]
    words, u_modifier = line_2_options[i]
    line += words
    if not (u_modifier is None):
        u = u_modifier


def process_phrase_3():
    global line

    phrases = [
        (False, "AND MY SOUL"),
        (False, "DARKNESS THERE"),
        (False, "SHALL BE LIFTED"),
        (False, "QUOTH THE RAVEN"),
        (True, "SIGN OF PARTING"),
    ]

    only_if_u, words = phrases[i]
    if (not only_if_u) or (u > 0):
        line = line + words


def process_phrase_4():
    global line

    phrases = [
        ("NOTHING MORE"),
        ("YET AGAIN"),
        ("SLOWLY CREEPING"),
        ("...EVERMORE"),
        ("NEVERMORE"),
    ]

    line += phrases[i]


def maybe_comma():
    # line 210
    global u
    global line

    if len(line) > 0 and line[-1] == ".":
        # don't follow a period with a comma, ever
        return

    if u != 0 and random.random() <= 0.19:
        line += ", "
        u = 2
    # line 212
    if random.random() <= 0.65:
        line += " "
        u += 1
    else:
        # line 214
        print(line)
        line = ""
        u = 0


def pick_phrase():
    global phrase
    global line
    global i, j, k

    i = random.randint(0, 4)
    j += 1
    k += 1

    if u <= 0 and (j % 2) != 0:
        # random indentation is fun!
        line += " " * 5
    phrase = j + 1


def main():
    print_centered("POETRY")
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()

    global line, phrase, j, k, u

    phrase_processors = {
        1: process_phrase_1,
        2: process_phrase_2,
        3: process_phrase_3,
        4: process_phrase_4,
    }

    while True:
        if phrase >= 1 and phrase <= 4:
            phrase_processors[phrase]()
            maybe_comma()
        elif phrase == 5:
            j = 0
            print(line)
            line = ""
            if k > 20:
                print()
                u = 0
                k = 0
            else:
                phrase = 2
                continue
        pick_phrase()


if __name__ == "__main__":
    main()
