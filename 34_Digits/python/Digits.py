import random

def printIntro():
    print("                                DIGITS")
    print("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print("\n\n")
    print("THIS IS A GAME OF GUESSING.")


def readInstructionChoice():
    print("FOR INSTRUCTIONS, TYPE '1', ELSE TYPE '0' ? ")
    try:
        choice = int(input())
        return choice == 1
    except (ValueError, TypeError) as m:
        return False


def printInstructions():
    print("\n")
    print("PLEASE TAKE A PIECE OF PAPER AND WRITE DOWN")
    print("THE DIGITS '0', '1', OR '2' THIRTY TIMES AT RANDOM.")
    print("ARRANGE THEM IN THREE LINES OF TEN DIGITS EACH.")
    print("I WILL ASK FOR THEN TEN AT A TIME.")
    print("I WILL ALWAYS GUESS THEM FIRST AND THEN LOOK AT YOUR")
    print("NEXT NUMBER TO SEE IF I WAS RIGHT. BY PURE LUCK,")
    print("I OUGHT TO BE RIGHT TEN TIMES. BUT I HOPE TO DO BETTER")
    print("THAN THAT *****")
    print()


def read10Numbers():
    print("TEN NUMBERS, PLEASE ? ")
    numbers = []

    for i in range(10):
        validInput = False
        while not validInput:
            try:
                n = int(input())
                validInput = True
                numbers.append(n)
            except (TypeError, ValueError) as e:
                print("!NUMBER EXPECTED - RETRY INPUT LINE")

    return numbers

def readContinueChoice():
    print("\nDO YOU WANT TO TRY AGAIN (1 FOR YES, 0 FOR NO) ? ")
    try:
        choice = int(input())
        return choice == 1
    except (ValueError, TypeError) as m:
        return False

if __name__ == '__main__':
    printIntro()
    if readInstructionChoice():
        printInstructions()

    a = 0
    b = 1
    c = 3

    m = [[1] * 3 for i in range(27)]
    k = [[9] * 3 for i in range(3)]
    l = [[3] * 3 for i in range(9)]

    continueGame = True
    while continueGame:
        l[0][0] = 2
        l[4][1] = 2
        l[8][2] = 2
        z = 26
        z1 = 8
        z2 = 2
        runningCorrect = 0

        for t in range(1, 4):
            validNumbers = False
            numbers = []
            while not validNumbers:
                print()
                numbers = read10Numbers()
                validNumbers = True
                for number in numbers:
                    if number < 0 or number > 2:
                        print("ONLY USE THE DIGITS '0', '1', OR '2'.")
                        print("LET'S TRY AGAIN.")
                        validNumbers = False
                        break

            print("\n%-14s%-14s%-14s%-14s" % ("MY GUESS", "YOUR NO.", "RESULT", "NO. RIGHT"))

            for number in numbers:
                s = 0
                myGuess = 0
                for j in range(0, 3):
                    # What did the original author have in mind ?
                    # The first expression always results in 0 because a is always 0
                    s1 = a * k[z2][j] + b * l[int(z1)][j] + c * m[int(z)][j]
                    if s < s1:
                        s = s1
                        myGuess = j
                    elif s1 == s:
                        if random.random() >= 0.5:
                            myGuess = j

                result = ""

                if myGuess != number:
                    result = "WRONG"
                else:
                    runningCorrect += 1
                    result = "RIGHT"
                    m[int(z)][number] = m[int(z)][number] + 1
                    l[int(z1)][number] = l[int(z1)][number] + 1
                    k[int(z2)][number] = k[int(z2)][number] + 1
                    z = z - (z / 9) * 9
                    z = 3 * z + number
                print("\n%-14d%-14d%-14s%-14d" % (myGuess, number, result, runningCorrect))

                z1 = z - (z / 9) * 9
                z2 = number

        # print summary report
        print()
        if runningCorrect > 10:
            print()
            print("I GUESSED MORE THAN 1/3 OF YOUR NUMBERS.")
            print("I WIN." + u"\u0007")
        elif runningCorrect < 10:
            print("I GUESSED LESS THAN 1/3 OF YOUR NUMBERS.")
            print("YOU BEAT ME.  CONGRATULATIONS *****")
        else:
            print("I GUESSED EXACTLY 1/3 OF YOUR NUMBERS.")
            print("IT'S A TIE GAME.")

        continueGame = readContinueChoice()

    print("\nTHANKS FOR THE GAME.")