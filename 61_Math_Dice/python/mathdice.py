from random import randint
print("Math Dice")
print("https://github.com/coding-horror/basic-computer-games")
print()
print("""This program generates images of two dice.
When two dice and an equals sign followed by a question
mark have been printed, type your answer, and hit the ENTER
key.
To conclude the program, type 0.
""")

def print_dice(n):
    
    def print_0():
        print("|     |")
    def print_2():
        print("| * * |")

    print(" ----- ")

    if n in [4,5,6]:
        print_2()
    elif n in [2,3]:
        print("| *   |")
    else:
        print_0()

    if n in [1,3,5]:
        print("|  *  |")
    elif n in [2,4]:
        print_0()
    else:
        print_2()

    if n in [4,5,6]:
        print_2()
    elif n in [2,3]:
        print("|   * |")
    else:
        print_0()

    print(" ----- ")

def main():

    while True:
        d1 = randint(1,6)
        d2 = randint(1,6)
        guess = 13

        print_dice(d1)
        print("   +")
        print_dice(d2)
        print("   =")

        tries = 0
        while guess != (d1 + d2) and tries < 2:
            if tries == 1:
                print("No, count the spots and give another answer.")
            try:
                guess = int(input())
            except ValueError:
                print("That's not a number!")
            if guess == 0:
                exit()
            tries += 1

        if guess != (d1 + d2):
            print(f"No, the answer is {d1 + d2}!")
        else:
            print("Correct!")


        print("The dice roll again....")



if __name__ == "__main__":
    main()