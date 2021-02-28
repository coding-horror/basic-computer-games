"""
NAME

simple string manipulations on the user's name

Ported by Dave LeCompte
"""


def print_with_tab(space_count, msg):
    if space_count > 0:
        spaces = " " * space_count
    else:
        spaces = ""
    print(spaces + msg)


def is_yes_ish(answer):
    cleaned = answer.strip().upper()
    if cleaned == "Y" or cleaned == "YES":
        return True
    return False


def main():
    print_with_tab(34, "NAME")
    print_with_tab(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()
    print("HELLO.")
    print("MY NAME iS CREATIVE COMPUTER.")
    name = input("WHAT'S YOUR NAME (FIRST AND LAST)?")
    print()
    name_as_list = list(name)
    reversed_name = "".join(name_as_list[::-1])
    print(f"THANK YOU, {reversed_name}.")
    print()
    print("OOPS!  I GUESS I GOT IT BACKWARDS.  A SMART")
    print("COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!")
    print()
    print()
    print("BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER.")

    sorted_name = "".join(sorted(name_as_list))
    print(f"LET'S PUT THEM IN ORDER LIKE THIS: {sorted_name}")
    print()
    print()

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


main()
