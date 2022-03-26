import random
from dataclasses import dataclass
from typing import List


@dataclass
class Player:
    name: str
    score: int = 0


def print_intro() -> None:
    print(" " * 32 + "BULLSEYE")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print("\n" * 3, end="")
    print("IN THIS GAME, UP TO 20 PLAYERS THROW DARTS AT A TARGET")
    print("WITH 10, 20, 30, AND 40 POINT ZONES.  THE OBJECTIVE IS")
    print("TO GET 200 POINTS.")
    print()
    print("THROW", end="")
    print(" " * 20 + "DESCRIPTION", end="")
    print(" " * 45 + "PROBABLE SCORE")
    print(" 1", end="")
    print(" " * 20 + "FAST OVERARM", end="")
    print(" " * 45 + "BULLSEYE OR COMPLETE MISS")
    print(" 2", end="")
    print(" " * 20 + "CONTROLLED OVERARM", end="")
    print(" " * 45 + "10, 20 OR 30 POINTS")
    print(" 3", end="")
    print(" " * 20 + "UNDERARM", end="")
    print(" " * 45 + "ANYTHING")
    print()


def print_outro(players: List[Player], winners: List[int]) -> None:
    print()
    print("WE HAVE A WINNER!!")
    print()
    for winner in winners:
        print(f"{players[winner].name} SCORED {players[winner].score} POINTS.")
    print()
    print("THANKS FOR THE GAME.")


def main() -> None:
    print_intro()
    players: List[Player] = []

    winners: List[int] = []  # will point to indices of player_names

    nb_players = int(input("HOW MANY PLAYERS? "))
    for _ in range(nb_players):
        player_name = input("NAME OF PLAYER #")
        players.append(Player(player_name))

    round_number = 0
    while len(winners) == 0:
        round_number += 1
        print()
        print(f"ROUND {round_number}---------")
        for player in players:
            print()
            while True:
                throw = int(input(f"{player.name}'S THROW? "))
                if throw not in [1, 2, 3]:
                    print("INPUT 1, 2, OR 3!")
                else:
                    break
            if throw == 1:
                probability_1 = 0.65
                probability_2 = 0.55
                probability_3 = 0.5
                probability_4 = 0.5
            elif throw == 2:
                probability_1 = 0.99
                probability_2 = 0.77
                probability_3 = 0.43
                probability_4 = 0.01
            elif throw == 3:
                probability_1 = 0.95
                probability_2 = 0.75
                probability_3 = 0.45
                probability_4 = 0.05
            throwing_luck = random.random()
            if throwing_luck >= probability_1:
                print("BULLSEYE!!  40 POINTS!")
                points = 40
            elif throwing_luck >= probability_2:
                print("30-POINT ZONE!")
                points = 30
            elif throwing_luck >= probability_3:
                print("20-POINT ZONE")
                points = 20
            elif throwing_luck >= probability_4:
                print("WHEW!  10 POINTS.")
                points = 10
            else:
                print("MISSED THE TARGET!  TOO BAD.")
                points = 0
            player.score += points
            print(f"TOTAL SCORE = {player.score}")
        for player_index, player in enumerate(players):
            if player.score > 200:
                winners.append(player_index)

    print_outro(players, winners)


if __name__ == "__main__":
    main()
