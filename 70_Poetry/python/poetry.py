"""
POETRY

A poetry generator

Ported by Dave LeCompte
"""

import random
from dataclasses import dataclass

PAGE_WIDTH = 64


@dataclass
class State:
    u: int = 0
    i: int = 0
    j: int = 0
    k: int = 0
    phrase: int = 1
    line: str = ""


def print_centered(msg: str) -> None:
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def process_phrase_1(state: State) -> str:
    line_1_options = [
        "MIDNIGHT DREARY",
        "FIERY EYES",
        "BIRD OR FIEND",
        "THING OF EVIL",
        "PROPHET",
    ]
    state.line = state.line + line_1_options[state.i]
    return state.line


def process_phrase_2(state: State) -> None:
    line_2_options = [
        ("BEGUILING ME", 2),
        ("THRILLED ME", None),
        ("STILL SITTING....", None),
        ("NEVER FLITTING", 2),
        ("BURNED", None),
    ]
    words, u_modifier = line_2_options[state.i]
    state.line += words
    if not (u_modifier is None):
        state.u = u_modifier


def process_phrase_3(state: State) -> None:
    phrases = [
        (False, "AND MY SOUL"),
        (False, "DARKNESS THERE"),
        (False, "SHALL BE LIFTED"),
        (False, "QUOTH THE RAVEN"),
        (True, "SIGN OF PARTING"),
    ]

    only_if_u, words = phrases[state.i]
    if (not only_if_u) or (state.u > 0):
        state.line = state.line + words


def process_phrase_4(state: State) -> None:
    phrases = [
        ("NOTHING MORE"),
        ("YET AGAIN"),
        ("SLOWLY CREEPING"),
        ("...EVERMORE"),
        ("NEVERMORE"),
    ]

    state.line += phrases[state.i]


def maybe_comma(state: State) -> None:
    if len(state.line) > 0 and state.line[-1] == ".":
        # don't follow a period with a comma, ever
        return

    if state.u != 0 and random.random() <= 0.19:
        state.line += ", "
        state.u = 2
    if random.random() <= 0.65:
        state.line += " "
        state.u += 1
    else:
        print(state.line)
        state.line = ""
        state.u = 0


def pick_phrase(state: State) -> None:
    state.i = random.randint(0, 4)
    state.j += 1
    state.k += 1

    if state.u <= 0 and (state.j % 2) != 0:
        # random indentation is fun!
        state.line += " " * 5
    state.phrase = state.j + 1


def main() -> None:
    print_centered("POETRY")
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")

    state = State()

    phrase_processors = {
        1: process_phrase_1,
        2: process_phrase_2,
        3: process_phrase_3,
        4: process_phrase_4,
    }

    while True:
        if state.phrase >= 1 and state.phrase <= 4:
            phrase_processors[state.phrase](state)
            maybe_comma(state)
        elif state.phrase == 5:
            state.j = 0
            print(state.line)
            state.line = ""
            if state.k > 20:
                print()
                state.u = 0
                state.k = 0
            else:
                state.phrase = 2
                continue
        pick_phrase(state)


if __name__ == "__main__":
    main()
