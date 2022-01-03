#!/usr/bin/env python3
"""This game simulates the games of craps played according to standard Nevada craps table rules.

That is:

1. A 7 or 11 on the first roll wins
2. A 2, 3, or 12 on the first roll loses
3. Any other number rolled becomes your "point." You continue to roll; if you get your point you win. If you
   roll a 7, you lose and the dice change hands when this happens.

This version of craps was modified by Steve North of Creative Computing. It is based on an original which
appeared one day one a computer at DEC.
"""
from random import randint


def throw_dice():
    return randint(1, 6) + randint(1, 6)


print(" " * 33 + "Craps")
print(" " * 15 + "Creative Computing  Morristown, New Jersey")
print()
print()
print()

winnings = 0
print("2,3,12 are losers; 4,5,6,8,9,10 are points; 7,11 are natural winners.")

play_again = True
while play_again:
    wager = int(input("Input the amount of your wager: "))

    print("I will now throw the dice")
    roll_1 = throw_dice()

    if roll_1 in [7, 11]:
        print(f"{roll_1} - natural.... a winner!!!!")
        print(f"{roll_1} pays even money, you win {wager} dollars")
        winnings += wager
    elif roll_1 == 2:
        print(f"{roll_1} - snake eyes.... you lose.")
        print(f"You lose {wager} dollars")
        winnings -= wager
    elif roll_1 in [3, 12]:
        print(f"{roll_1} - craps.... you lose.")
        print(f"You lose {wager} dollars")
        winnings -= wager
    else:
        print(f"{roll_1} is the point. I will roll again")
        roll_2 = 0
        while roll_2 not in [roll_1, 7]:
            roll_2 = throw_dice()
            if roll_2 == 7:
                print(f"{roll_2} - craps. You lose.")
                print(f"You lose $ {wager}")
                winnings -= wager
            elif roll_2 == roll_1:
                print(f"{roll_1} - a winner.........congrats!!!!!!!!")
                print(f"{roll_1} at 2 to 1 odds pays you...let me see... {2 * wager} dollars")
                winnings += 2 * wager
            else:
                print(f"{roll_2} - no point. I will roll again")

    m = input("  If you want to play again print 5 if not print 2: ")
    if winnings < 0:
        print(f"You are now under ${-winnings}")
    elif winnings > 0:
        print(f"You are now ahead ${winnings}")
    else:
        print("You are now even at 0")
    play_again = (m == "5")

if winnings < 0:
    print(f"Too bad, you are in the hole. Come again.")
elif winnings > 0:
    print(f"Congratulations---you came out a winner. Come again.")
else:
    print(f"Congratulations---you came out even, not bad for an amateur")
