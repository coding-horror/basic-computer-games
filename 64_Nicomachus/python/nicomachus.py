"""
NICOMACHUS

Math exercise/demonstration

Ported by Dave LeCompte
"""

# PORTING NOTE
#
# The title, as printed ingame, is "NICOMA", hinting at a time when
# filesystems weren't even 8.3, but could only support 6 character
# filenames.

import time


def get_yes_or_no() -> bool:
    while True:
        response = input().upper()
        if response == "YES":
            return True
        elif response == "NO":
            return False
        print(f"EH?  I DON'T UNDERSTAND '{response}'  TRY 'YES' OR 'NO'.")


def play_game() -> None:
    print("PLEASE THINK OF A NUMBER BETWEEN 1 AND 100.")
    print("YOUR NUMBER DIVIDED BY 3 HAS A REMAINDER OF")
    a = int(input())
    print("YOUR NUMBER DIVIDED BY 5 HAS A REMAINDER OF")
    b = int(input())
    print("YOUR NUMBER DIVIDED BY 7 HAS A REMAINDER OF")
    c = int(input())
    print()
    print("LET ME THINK A MOMENT...")
    print()

    time.sleep(2.5)

    d = (70 * a + 21 * b + 15 * c) % 105

    print(f"YOUR NUMBER WAS {d}, RIGHT?")

    response = get_yes_or_no()

    if response:
        print("HOW ABOUT THAT!!")
    else:
        print("I FEEL YOUR ARITHMETIC IS IN ERROR.")
    print()
    print("LET'S TRY ANOTHER")


def main() -> None:
    print(" " * 33 + "NICOMA")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")

    print("BOOMERANG PUZZLE FROM ARITHMETICA OF NICOMACHUS -- A.D. 90!")
    print()
    while True:
        play_game()


if __name__ == "__main__":
    main()
