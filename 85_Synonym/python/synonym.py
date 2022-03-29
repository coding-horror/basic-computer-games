"""
SYNONYM

Vocabulary quiz

Ported by Dave LeCompte
"""

import random

PAGE_WIDTH = 64


def print_centered(msg: str) -> None:
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title: str) -> None:
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def print_instructions() -> None:
    print("A SYNONYM OF A WORD MEANS ANOTHER WORD IN THE ENGLISH")
    print("LANGUAGE WHICH HAS THE SAME OR VERY NEARLY THE SAME MEANING.")
    print("I CHOOSE A WORD -- YOU TYPE A SYNONYM.")
    print("IF YOU CAN'T THINK OF A SYNONYM, TYPE THE WORD 'HELP'")
    print("AND I WILL TELL YOU A SYNONYM.")
    print()


right_words = ["RIGHT", "CORRECT", "FINE", "GOOD!", "CHECK"]

synonym_words = [
    ["FIRST", "START", "BEGINNING", "ONSET", "INITIAL"],
    ["SIMILAR", "ALIKE", "SAME", "LIKE", "RESEMBLING"],
    ["MODEL", "PATTERN", "PROTOTYPE", "STANDARD", "CRITERION"],
    ["SMALL", "INSIGNIFICANT", "LITTLE", "TINY", "MINUTE"],
    ["STOP", "HALT", "STAY", "ARREST", "CHECK", "STANDSTILL"],
    ["HOUSE", "DWELLING", "RESIDENCE", "DOMICILE", "LODGING", "HABITATION"],
    ["PIT", "HOLE", "HOLLOW", "WELL", "GULF", "CHASM", "ABYSS"],
    ["PUSH", "SHOVE", "THRUST", "PROD", "POKE", "BUTT", "PRESS"],
    ["RED", "ROUGE", "SCARLET", "CRIMSON", "FLAME", "RUBY"],
    ["PAIN", "SUFFERING", "HURT", "MISERY", "DISTRESS", "ACHE", "DISCOMFORT"],
]


def print_right() -> None:
    print(random.choice(right_words))


def ask_question(question_number):
    words = synonym_words[question_number]
    clues = words[:]
    base_word = clues.pop(0)

    while True:
        question = f"     WHAT IS A SYNONYM OF {base_word}? "
        response = input(question).upper()

        if response == "HELP":
            clue = random.choice(clues)
            print(f"**** A SYNONYM OF {base_word} IS {clue}.")
            print()

            # remove the clue from available clues
            clues.remove(clue)
            continue

        if (response != base_word) and (response in words):
            print_right()
            return


def finish() -> None:
    print()
    print("SYNONYM DRILL COMPLETED.")


def main() -> None:
    print_header("SYNONYM")
    print_instructions()

    num_questions = len(synonym_words)
    word_indices = list(range(num_questions))
    random.shuffle(word_indices)

    for word_number in word_indices:
        ask_question(word_number)

    finish()


if __name__ == "__main__":
    main()
