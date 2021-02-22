#!/usr/bin/env python3
# CUBE
#
# Converted from BASIC to Python by Trevor Hobson

import random


def mine_position():
    mine = []
    for _ in range(3):
        mine.append(random.randint(1, 3))
    return mine


def play_game():
    """Play one round of the game"""

    money = 500
    print("\nYou have", money, "dollars.")
    while True:
        mines = []
        for _ in range(5):
            mine = []
            while True:
                mine = mine_position()
                if not(mine in mines or mine == [1, 1, 1] or mine == [3, 3, 3]):
                    break
            mines.append(mine)
        wager = -1
        while wager == -1:
            try:
                wager = int(input("\nHow much do you want to wager? "))
                if not 0 <= wager <= money:
                    wager = -1
                    print("Tried to fool me; bet again")
            except ValueError:
                print("Please enter a number.")
        prompt = "\nIt's your move: "
        position = [1, 1, 1]
        while True:
            move = [-1, -1, -1]
            while move == [-1, -1, -1]:
                try:
                    coordinates = [int(item)
                                   for item in input(prompt).split(",")]
                    if len(coordinates) == 3:
                        move = coordinates
                    else:
                        raise ValueError
                except (ValueError, IndexError):
                    print("Please enter valid coordinates.")
            if (abs(move[0]-position[0]) + abs(move[1]-position[1]) + abs(move[2]-position[2])) > 1:
                print("\nIllegal move. You lose")
                money = money - wager
                break
            elif not move[0] in [1, 2, 3] or not move[1] in [1, 2, 3] or not move[2] in [1, 2, 3]:
                print("\nIllegal move. You lose")
                money = money - wager
                break
            elif move == [3, 3, 3]:
                print("\nCongratulations!")
                money = money + wager
                break
            elif move in mines:
                print("\n******BANG******")
                print("You lose!")
                money = money - wager
                break
            else:
                position = move
                prompt = "\nNext move: "
        if money > 0:
            print("\nYou now have", money, "dollars.")
            if not input("Do you want to try again ").lower().startswith("y"):
                break
        else:
            print("\nYou bust.")
    print("\nTough luck")
    print("\nGoodbye.")


def main():
    print(" " * 34 + "CUBE")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
    if input("Do you want to see the instructions ").lower().startswith("y"):
        print("\nThis is a game in which you will be playing against the")
        print("random decisions of the computer. The field of play is a")
        print("cube of side 3. Any of the 27 locations can be designated")
        print("by inputing three numbers such as 2,3,1. At the start,")
        print("you are automatically at location 1,1,1. The object of")
        print("the game is to get to location 3,3,3. One minor detail:")
        print("the computer will pick, at random, 5 locations at which")
        print("it will plant land mines. If you hit one of these locations")
        print("you lose. One other detail: You may move only one space")
        print("in one direction each move. For example: From 1,1,2 you")
        print("may move to 2,1,2 or 1,1,3. You may not change")
        print("two of the numbers on the same move. If you make an illegal")
        print("move, you lose and the computer takes the money you may")
        print("have bet on that round.\n")
        print("When stating the amount of a wager, print only the number")
        print("of dollars (example: 250) you are automatically started with")
        print("500 dollars in your account.\n")
        print("Good luck!")

    keep_playing = True
    while keep_playing:
        play_game()
        keep_playing = input(
            "\nPlay again? (yes or no) ").lower().startswith("y")


if __name__ == "__main__":
    main()
