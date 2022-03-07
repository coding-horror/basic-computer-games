import random
from datetime import date

global RED_NUMBERS
RED_NUMBERS = [1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36]


def print_instructions():
    print(
        """
THIS IS THE BETTING LAYOUT
  (*=RED)

 1*    2     3*
 4     5*    6
 7*    8     9*
10    11    12*
---------------
13    14*   15
16*   17    18*
19*   20    21*
22    23*   24
---------------
25*   26    27*
28    29    30*
31    32*   33
34*   35    36*
---------------
    00    0

TYPES OF BETS

THE NUMBERS 1 TO 36 SIGNIFY A STRAIGHT BET
ON THAT NUMBER.
THESE PAY OFF 35:1

THE 2:1 BETS ARE:
37) 1-12     40) FIRST COLUMN
38) 13-24    41) SECOND COLUMN
39) 25-36    42) THIRD COLUMN

THE EVEN MONEY BETS ARE:
43) 1-18     46) ODD
44) 19-36    47) RED
45) EVEN     48) BLACK

 49)0 AND 50)00 PAY OFF 35:1
NOTE: 0 AND 00 DO NOT COUNT UNDER ANY
   BETS EXCEPT THEIR OWN.

WHEN I ASK FOR EACH BET, TYPE THE NUMBER
AND THE AMOUNT, SEPARATED BY A COMMA.
FOR EXAMPLE: TO BET $500 ON BLACK, TYPE 48,500
WHEN I ASK FOR A BET.

THE MINIMUM BET IS $5, THE MAXIMUM IS $500.

    """
    )


def query_bets():
    """Queries the user to input their bets"""
    betCount = -1
    while betCount <= 0:
        try:
            betCount = int(input("HOW MANY BETS? "))
        except:
            ...

    bet_IDs = [-1] * betCount
    bet_Values = [0] * betCount

    for i in range(betCount):
        while bet_IDs[i] == -1:
            try:
                inString = input("NUMBER " + str(i + 1) + "? ").split(",")
                id, val = int(inString[0]), int(inString[1])

                # check other bet_IDs
                for j in range(i):
                    if id != -1 and bet_IDs[j] == id:
                        id = -1
                        print("YOU ALREADY MADE THAT BET ONCE, DUM-DUM")
                        break

                if id > 0 and id <= 50 and val >= 5 and val <= 500:
                    bet_IDs[i] = id
                    bet_Values[i] = val
            except:
                ...
    return bet_IDs, bet_Values


def bet_results(bet_IDs, bet_Values, result):
    """Computes the results, prints them, and returns the total net winnings"""
    total_winnings = 0

    def get_modifier(id, num):
        if id == 37 and num <= 12:
            return 2
        elif id == 38 and num > 12 and num <= 24:
            return 2
        elif id == 39 and num > 24 and num < 37:
            return 2
        elif id == 40 and num < 37 and num % 3 == 1:
            return 2
        elif id == 41 and num < 37 and num % 3 == 2:
            return 2
        elif id == 42 and num < 37 and num % 3 == 0:
            return 2
        elif id == 43 and num <= 18:
            return 1
        elif id == 44 and num > 18 and num <= 36:
            return 1
        elif id == 45 and num % 2 == 0:
            return 1
        elif id == 46 and num % 2 == 1:
            return 1
        elif id == 47 and num in RED_NUMBERS:
            return 1
        elif id == 48 and num not in RED_NUMBERS:
            return 1
        elif id < 37 and id == num:
            return 35
        else:
            return -1

    for i in range(len(bet_IDs)):
        winnings = bet_Values[i] * get_modifier(bet_IDs[i], result)
        total_winnings += winnings

        if winnings >= 0:
            print("YOU WIN " + str(winnings) + " DOLLARS ON BET " + str(i + 1))
        else:
            print("YOU LOSE " + str(winnings * -1) + " DOLLARS ON BET " + str(i + 1))

    return winnings


def print_check(amount):
    """Prints a check of a given amount"""
    name = input("TO WHOM SHALL I MAKE THE CHECK? ")

    print("-" * 72)
    print()
    print(" " * 40 + "CHECK NO. " + str(random.randint(0, 100)))
    print(" " * 40 + str(date.today()))
    print()
    print("PAY TO THE ORDER OF -----" + name + "----- $" + str(amount))
    print()
    print(" " * 40 + "THE MEMORY BANK OF NEW YORK")
    print(" " * 40 + "THE COMPUTER")
    print(" " * 40 + "----------X-----")
    print("-" * 72)


def main():
    player_balance = 1000
    host_balance = 100000

    print(" " * 32 + "ROULETTE")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()

    if stringtobool(input("DO YOU WANT INSTRUCTIONS? ")):
        print_instructions()

    while True:
        bet_IDs, bet_Values = query_bets()

        print("SPINNING")
        print()
        print()

        val = random.randint(0, 38)
        if val == 38:
            print("0")
        elif val == 37:
            print("00")
        elif val in RED_NUMBERS:
            print(str(val) + " RED")
        else:
            print(str(val) + " BLACK")

        print()
        total_winnings = bet_results(bet_IDs, bet_Values, val)
        player_balance += total_winnings
        host_balance -= total_winnings

        print()
        print("TOTALS:\tME\t\tYOU")
        print("\t\t" + str(host_balance) + "\t" + str(player_balance))

        if player_balance <= 0:
            print("OOPS! YOU JUST SPENT YOUR LAST DOLLAR!")
            break
        elif host_balance <= 0:
            print("YOU BROKE THE HOUSE!")
            player_balance = 101000
            break
        if not stringtobool(input("PLAY AGAIN? ")):
            break

    if player_balance <= 0:
        print("THANKS FOR YOUR MONEY")
        print("I'LL USE IT TO BUY A SOLID GOLD ROULETTE WHEEL")
    else:
        print_check(player_balance)
    print("COME BACK SOON!")


def stringtobool(string):
    """Converts a string to a bool"""
    return string.lower() in ("yes", "y", "true", "t", "yes")


if __name__ == "__main__":
    main()
