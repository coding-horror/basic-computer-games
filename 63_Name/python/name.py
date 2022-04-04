"""
NAME

simple string manipulations on the user's name

Ported by Dave LeCompte
"""


def is_yes_ish(answer: str) -> bool:
    cleaned = answer.strip().upper()
    if cleaned in ["Y", "YES"]:
        return True
    return False


def main() -> None:
    print(" " * 34 + "NAME")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")
    print("HELLO.")
    print("MY NAME iS CREATIVE COMPUTER.")
    name = input("WHAT'S YOUR NAME (FIRST AND LAST)?")
    print()
    name_as_list = list(name)
    reversed_name = "".join(name_as_list[::-1])
    print(f"THANK YOU, {reversed_name}.\n")
    print("OOPS!  I GUESS I GOT IT BACKWARDS.  A SMART")
    print("COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!\n\n")
    print("BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER.")

    sorted_name = "".join(sorted(name_as_list))
    print(f"LET'S PUT THEM IN ORDER LIKE THIS: {sorted_name}\n\n")

    print("DON'T YOU LIKE THAT BETTER?")
    like_answer = input()
    print()
    if is_yes_ish(like_answer):
        print("I KNEW YOU'D AGREE!!")
    else:
        print("I'M SORRY YOU DON'T LIKE IT THAT WAY.")
    print()
    print(f"I REALLY ENJOYED MEETING YOU, {name}.")
    print("HAVE A NICE DAY!")


if __name__ == "__main__":
    main()
