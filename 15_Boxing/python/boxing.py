#!/usr/bin/env python3
import random

QUESTION_PROMPT = "? "


def play():
    print("BOXING")
    print("CREATIVE COMPUTING   MORRISTOWN, NEW JERSEY")
    print("\n\n")
    print("BOXING OLYMPIC STYLE (3 ROUNDS -- 2 OUT OF 3 WINS)")

    opponent_score = 0
    player_score = 0

    opponent_damage = 0
    player_damage = 0

    opponent_knockedout = False
    player_knockedout = False

    print("WHAT IS YOUR OPPONENT'S NAME", end=QUESTION_PROMPT)
    opponent_name = input()
    print("WHAT IS YOUR MAN'S NAME", end=QUESTION_PROMPT)
    player_name = input()

    print("DIFFERENT PUNCHES ARE 1 FULL SWING 2 HOOK 3 UPPERCUT 4 JAB")
    print("WHAT IS YOUR MAN'S BEST", end=QUESTION_PROMPT)
    player_best = int(input())  # noqa: TODO - this likely is a bug!

    print("WHAT IS HIS VULNERABILITY", end=QUESTION_PROMPT)
    player_weakness = int(input())

    opponent_best = 0
    opponent_weakness = 0
    while opponent_best == opponent_weakness:
        opponent_best = random.randint(1, 4)
        opponent_weakness = random.randint(1, 4)

    print(
        "{}'S ADVANTAGE is {} AND VULNERABILITY IS SECRET.".format(
            opponent_name, opponent_weakness
        )
    )

    for round in (1, 2, 3):
        print(f"ROUND {round} BEGINS...\n")
        if opponent_score >= 2 or player_score >= 2:
            break

        for _action in range(7):
            if random.randint(1, 10) > 5:
                # opponent swings
                punch = random.randint(1, 4)

                if punch == player_weakness:
                    player_damage += 2

                if punch == 1:
                    print(f"{opponent_name} TAKES A FULL SWING AND", end=" ")
                    if player_weakness == 1 or random.randint(1, 60) < 30:
                        print("POW!!!! HE HITS HIM RIGHT IN THE FACE!")
                        if player_damage > 35:
                            player_knockedout = True
                            break
                        player_damage += 15
                    else:
                        print("BUT IT'S BLOCKED!")
                elif punch == 2:
                    print(
                        "{} GETS {} IN THE JAW (OUCH!)".format(
                            opponent_name, player_name
                        ),
                        end=" ",
                    )
                    player_damage += 7
                    print("....AND AGAIN")
                    if player_damage > 35:
                        player_knockedout = True
                        break
                    player_damage += 5
                elif punch == 3:
                    print(f"{player_name} IS ATTACKED BY AN UPPERCUT (OH,OH)")
                    if player_weakness == 3 or random.randint(1, 200) > 75:
                        print(
                            "{} BLOCKS AND HITS {} WITH A HOOK".format(
                                player_name, opponent_name
                            )
                        )
                        opponent_damage += 5
                    else:
                        print(f"AND {opponent_name} CONNECTS...")
                        player_damage += 8
                else:
                    print(f"{opponent_name} JABS AND", end=" ")
                    if player_weakness == 4 or random.randint(1, 7) > 4:
                        print("BLOOD SPILLS !!!")
                        player_damage += 3
                    else:
                        print("AND IT'S BLOCKED (LUCKY BLOCK!)")
            else:
                print(f"{player_name}'S PUNCH", end="? ")
                punch = int(input())

                if punch == opponent_weakness:
                    opponent_damage += 2

                if punch == 1:
                    print(f"{player_name} SWINGS AND", end=" ")
                    if opponent_weakness == 1 or random.randint(1, 30) < 10:
                        print("HE CONNECTS!")
                        if opponent_damage > 35:
                            opponent_knockedout = True
                            break
                        opponent_damage += 15
                    else:
                        print("HE MISSES")
                elif punch == 2:
                    print(f"{player_name} GIVES THE HOOK...", end=" ")
                    if opponent_weakness == 2 or random.randint(1, 2) == 1:
                        print("CONNECTS...")
                        opponent_damage += 7
                    else:
                        print("BUT IT'S BLOCKED!!!!!!!!!!!!!")
                elif punch == 3:
                    print(f"{player_name} TRIES AN UPPERCUT", end=" ")
                    if opponent_weakness == 3 or random.randint(1, 100) < 51:
                        print("AND HE CONNECTS!")
                        opponent_damage += 4
                    else:
                        print("AND IT'S BLOCKED (LUCKY BLOCK!)")
                else:
                    print(
                        f"{player_name} JABS AT {opponent_name}'S HEAD",
                        end=" ",
                    )
                    if opponent_weakness == 4 or random.randint(1, 8) < 4:
                        print("AND HE CONNECTS!")
                        opponent_damage += 3
                    else:
                        print("AND IT'S BLOCKED (LUCKY BLOCK!)")

        if player_knockedout or opponent_knockedout:
            break
        elif player_damage > opponent_damage:
            print(f"{opponent_name} WINS ROUND {round}")
            opponent_score += 1
        else:
            print(f"{player_name} WINS ROUND {round}")
            player_score += 1

    if player_knockedout:
        print(
            "{} IS KNOCKED COLD AND {} IS THE WINNER AND CHAMP".format(
                player_name, opponent_name
            )
        )
    elif opponent_knockedout:
        print(
            "{} IS KNOCKED COLD AND {} IS THE WINNER AND CHAMP".format(
                opponent_name, player_name
            )
        )
    elif opponent_score > player_score:
        print(f"{opponent_name} WINS (NICE GOING), {player_name}")
    else:
        print(f"{player_name} AMAZINGLY WINS")

    print("\n\nAND NOW GOODBYE FROM THE OLYMPIC ARENA.")


if __name__ == "__main__":
    play()
