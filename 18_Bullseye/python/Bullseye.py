import random


def print_n_whitespaces(n: int):
    print(" " * n, end="")


def print_n_newlines(n: int):
    for _ in range(n):
        print()


print_n_whitespaces(32)
print("BULLSEYE")
print_n_whitespaces(15)
print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
print_n_newlines(3)
print("IN THIS GAME, UP TO 20 PLAYERS THROW DARTS AT A TARGET")
print("WITH 10, 20, 30, AND 40 POINT ZONES.  THE OBJECTIVE IS")
print("TO GET 200 POINTS.")
print()
print("THROW", end="")
print_n_whitespaces(20)
print("DESCRIPTION", end="")
print_n_whitespaces(45)
print("PROBABLE SCORE")
print(" 1", end="")
print_n_whitespaces(20)
print("FAST OVERARM", end="")
print_n_whitespaces(45)
print("BULLSEYE OR COMPLETE MISS")
print(" 2", end="")
print_n_whitespaces(20)
print("CONTROLLED OVERARM", end="")
print_n_whitespaces(45)
print("10, 20 OR 30 POINTS")
print(" 3", end="")
print_n_whitespaces(20)
print("UNDERARM", end="")
print_n_whitespaces(45)
print("ANYTHING")
print()

nb_winners = 0
round = 0

winners = {}
for i in range(1, 11):
    winners[i] = 0

total_score = {}
for i in range(1, 21):
    total_score[i] = 0

nb_players = int(input("HOW MANY PLAYERS? "))
player_names = {}
for i in range(1, nb_players + 1):
    player_name = input("NAME OF PLAYER #")
    player_names[i] = player_name

while nb_winners == 0:
    round = round + 1
    print()
    print(f"ROUND {round}---------")
    for i in range(1, nb_players + 1):
        print()
        while True:
            throw = int(input(f"{player_names[i]}'S THROW? "))
            if throw not in [1, 2, 3]:
                print("INPUT 1, 2, OR 3!")
            else:
                break
        if throw == 1:
            P1 = 0.65
            P2 = 0.55
            P3 = 0.5
            P4 = 0.5
        elif throw == 2:
            P1 = 0.99
            P2 = 0.77
            P3 = 0.43
            P4 = 0.01
        elif throw == 3:
            P1 = 0.95
            P2 = 0.75
            P3 = 0.45
            P4 = 0.05
        throwing_luck = random.random()
        if throwing_luck >= P1:
            print("BULLSEYE!!  40 POINTS!")
            points = 40
        elif throwing_luck >= P2:
            print("30-POINT ZONE!")
            points = 30
        elif throwing_luck >= P3:
            print("20-POINT ZONE")
            points = 20
        elif throwing_luck >= P4:
            print("WHEW!  10 POINTS.")
            points = 10
        else:
            print("MISSED THE TARGET!  TOO BAD.")
            points = 0
        total_score[i] = total_score[i] + points
        print(f"TOTAL SCORE = {total_score[i]}")
    for player_index in range(1, nb_players + 1):
        if total_score[player_index] > 200:
            nb_winners = nb_winners + 1
            winners[nb_winners] = player_index

print()
print("WE HAVE A WINNER!!")
print()
for i in range(1, nb_winners + 1):
    print(f"{player_names[winners[i]]} SCORED {total_score[winners[i]]} POINTS.")
print()
print("THANKS FOR THE GAME.")
