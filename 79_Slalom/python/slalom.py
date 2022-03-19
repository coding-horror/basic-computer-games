from random import random

medals = {
    "gold": 0,
    "silver": 0,
    "bronze": 0,
}


def ask(question):
    print(question, end="? ")
    return input().upper()


def ask_int(question):
    reply = ask(question)
    return int(reply) if reply.isnumeric() else -1


def pre_run(gates, max_speeds):
    print('\nType "INS" for instructions')
    print('Type "MAX" for approximate maximum speeds')
    print('Type "RUN" for the beginning of the race')
    cmd = ask("Command--")
    while cmd != "RUN":
        if cmd == "INS":
            print("\n*** Slalom: This is the 1976 Winter Olypic Giant Slalom.  You are")
            print("            the American team's only hope for a gold medal.\n")
            print("     0 -- Type this if you want to see how long you've taken.")
            print("     1 -- Type this if you want to speed up a lot.")
            print("     2 -- Type this if you want to speed up a little.")
            print("     3 -- Type this if you want to speed up a teensy.")
            print("     4 -- Type this if you want to keep going the same speed.")
            print("     5 -- Type this if you want to check a teensy.")
            print("     6 -- Type this if you want to check a little.")
            print("     7 -- Type this if you want to check a lot.")
            print("     8 -- Type this if you want to cheat and try to skip a gate.\n")
            print(" The place to use these options is when the Computer asks:\n")
            print("Option?\n")
            print("                Good Luck!\n")
            cmd = ask("Command--")
        elif cmd == "MAX":
            print("Gate Max")
            print(" # M.P.H.")
            print("----------")
            for i in range(0, gates):
                print(f" {i + 1}  {max_speeds[i]}")
            cmd = ask("Command--")
        else:
            cmd = ask(f'"{cmd}" is an illegal command--Retry')


def run(gates, lvl, max_speeds):
    global medals
    print("The starter counts down...5...4...3...2...1...Go!")
    time = 0
    speed = int(random() * (18 - 9) + 9)
    print("You're off")
    for i in range(0, gates):
        while True:
            print(f"\nHere comes gate #{i + 1}:")
            print(f" {int(speed)} M.P.H.")
            old_speed = speed
            opt = ask_int("Option")
            while opt < 1 or opt > 8:
                if opt == 0:
                    print(f"You've taken {int(time)} seconds.")
                else:
                    print("What?")
                opt = ask_int("Option")

            if opt == 8:
                print("***Cheat")
                if random() < 0.7:
                    print("An official caught you!")
                    print(f"You took {int(time + random())} seconds.")
                    return
                else:
                    print("You made it!")
                    time += 1.5
            else:
                match opt:
                    case 1:
                        speed += int(random() * (10 - 5) + 5)

                    case 2:
                        speed += int(random() * (5 - 3) + 3)

                    case 3:
                        speed += int(random() * (4 - 1) + 1)

                    case 5:
                        speed -= int(random() * (4 - 1) + 1)

                    case 6:
                        speed -= int(random() * (5 - 3) + 3)

                    case 7:
                        speed -= int(random() * (10 - 5) + 5)
                print(f" {int(speed)} M.P.H.")
                if speed > max_speeds[i]:
                    if random() < ((speed - max_speeds[i]) * 0.1) + 0.2:
                        print(
                            f"You went over the maximum speed and {'snagged a flag' if random() < .5 else 'wiped out'}!"
                        )
                        print(f"You took {int(time + random())} seconds")
                        return
                    else:
                        print("You went over the maximum speed and made it!")
                if speed > max_speeds[i] - 1:
                    print("Close one!")
            if speed < 7:
                print("Let's be realistic, ok? Let's go back and try again...")
                speed = old_speed
            else:
                time += max_speeds[i] - speed + 1
                if speed > max_speeds[i]:
                    time += 0.5
                break
    print(f"\nYou took {int(time + random())} seconds.")
    avg = time / gates
    if avg < 1.5 - (lvl * 0.1):
        print("Yout won a gold medal!")
        medals["gold"] += 1
    elif avg < 2.9 - (lvl * 0.1):
        print("You won a silver medal!")
        medals["silver"] += 1
    elif avg < 4.4 - (lvl * 0.01):
        print("You won a bronze medal!")
        medals["bronze"] += 1


def main():
    print("Slalom".rjust(39))
    print("Creative Computing Morristown, New Jersey\n\n\n".rjust(57))

    max_speeds = [
        14,
        18,
        26,
        29,
        18,
        25,
        28,
        32,
        29,
        20,
        29,
        29,
        25,
        21,
        26,
        29,
        20,
        21,
        20,
        18,
        26,
        25,
        33,
        31,
        22,
    ]

    while True:
        gates = ask_int("How many gates does this course have (1 to 25)")
        if gates < 1:
            print("Try again,")
        else:
            if gates > 25:
                print("25 is the limit.")
            break

    pre_run(gates, max_speeds)

    while True:
        lvl = ask_int("Rate yourself as a skier, (1=Worst, 3=Best)")
        if lvl < 1 or lvl > 3:
            print("The bounds are 1-3.")
        else:
            break

    while True:
        run(gates, lvl, max_speeds)
        while True:
            answer = ask("Do you want to play again?")
            if answer == "YES" or answer == "NO":
                break
            else:
                print('Please type "YES" or "NO"')
        if answer == "NO":
            break

    print("Thanks for the race")
    if medals["gold"] > 0:
        print(f"Gold medals: {medals['gold']}")
    if medals["silver"] > 0:
        print(f"Silver medals: {medals['silver']}")
    if medals["bronze"] > 0:
        print(f"Bronze medals: {medals['bronze']}")


if __name__ == "__main__":
    main()
