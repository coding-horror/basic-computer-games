"""
HOCKEY

A simulation of an ice hockey game.

The original author is Robert Puopolo;
modifications by Steve North of Creative Computing.

Ported to Python by Martin Thoma in 2022
"""

from dataclasses import dataclass, field
from random import randint
from typing import List, Tuple

NB_PLAYERS = 6


@dataclass
class Team:
    # TODO: It would be better to use a Player-class (name, goals, assits)
    #       and have the attributes directly at each player. This would avoid
    #       dealing with indices that much
    #
    #       I'm also rather certain that I messed up somewhere with the indices
    #       - instead of using those, one could use actual player positions:
    #       LEFT WING,    CENTER,        RIGHT WING
    #       LEFT DEFENSE, RIGHT DEFENSE, GOALKEEPER
    name: str
    players: List[str]  # 6 players
    shots_on_net: int = 0
    goals: List[int] = field(default_factory=lambda: [0 for _ in range(NB_PLAYERS)])
    assists: List[int] = field(default_factory=lambda: [0 for _ in range(NB_PLAYERS)])
    score: int = 0

    def show_lineup(self) -> None:
        print(" " * 10 + f"{self.name} STARTING LINEUP")
        for player in self.players:
            print(player)


def ask_binary(prompt: str, error_msg: str) -> bool:
    while True:
        answer = input(prompt).lower()
        if answer in ["y", "yes"]:
            return True
        if answer in ["n", "no"]:
            return False
        print(error_msg)


def get_team_names() -> Tuple[str, str]:
    while True:
        answer = input("ENTER THE TWO TEAMS: ")
        if answer.count(",") == 1:
            return answer.split(",")  # type: ignore
        print("separated by a single comma")


def get_pass() -> int:
    while True:
        answer = input("PASS? ")
        try:
            passes = int(answer)
            if passes >= 0 and passes <= 3:
                return passes
        except ValueError:
            print("ENTER A NUMBER BETWEEN 0 AND 3")


def get_minutes_per_game() -> int:
    while True:
        answer = input("ENTER THE NUMBER OF MINUTES IN A GAME ")
        try:
            minutes = int(answer)
            if minutes >= 1:
                return minutes
        except ValueError:
            print("ENTER A NUMBER")


def get_player_names(prompt: str) -> List[str]:
    players = []
    print(prompt)
    for i in range(1, 7):
        player = input(f"PLAYER {i}: ")
        players.append(player)
    return players


def make_shot(
    controlling_team: int, team_a: Team, team_b: Team, player_index: List[int], j: int
) -> Tuple[int, int, int, int]:
    while True:
        try:
            s = int(input("SHOT? "))
        except ValueError:
            continue
        if s >= 1 and s <= 4:
            break
    if controlling_team == 1:
        print(team_a.players[player_index[j - 1]])
        g = player_index[j - 1]
        g1 = 0
        g2 = 0
    else:
        print(team_b.players[player_index[j - 1]])
        g2 = 0
        g2 = 0
        g = player_index[j - 1]
    if s == 1:
        print(" LET'S A BOOMER GO FROM THE RED LINE!!\n")  # line 400
        z = 10
    elif s == 2:
        print(" FLIPS A WRISTSHOT DOWN THE ICE\n")  # line 420
        # Probable missing line 430 in original
    elif s == 3:
        print(" BACKHANDS ONE IN ON THE GOALTENDER\n")
        z = 25
    elif s == 4:
        print(" SNAPS A LONG FLIP SHOT\n")
        # line 460
        z = 17
    return z, g, g1, g2


def print_header() -> None:
    print(" " * 33 + "HOCKEY")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")


def instructions() -> None:
    wants_it = ask_binary("WOULD YOU LIKE THE INSTRUCTIONS? ", "ANSWER YES OR NO!!")
    if wants_it:
        print()
        print("THIS IS A SIMULATED HOCKEY GAME.")
        print("QUESTION     RESPONSE")
        print("PASS         TYPE IN THE NUMBER OF PASSES YOU WOULD")
        print("             LIKE TO MAKE, FROM 0 TO 3.")
        print("SHOT         TYPE THE NUMBER CORRESPONDING TO THE SHOT")
        print("             YOU WANT TO MAKE.  ENTER:")
        print("             1 FOR A SLAPSHOT")
        print("             2 FOR A WRISTSHOT")
        print("             3 FOR A BACKHAND")
        print("             4 FOR A SNAP SHOT")
        print("AREA         TYPE IN THE NUMBER CORRESPONDING TO")
        print("             THE AREA YOU ARE AIMING AT.  ENTER:")
        print("             1 FOR UPPER LEFT HAND CORNER")
        print("             2 FOR UPPER RIGHT HAND CORNER")
        print("             3 FOR LOWER LEFT HAND CORNER")
        print("             4 FOR LOWER RIGHT HAND CORNER")
        print("AT THE START OF THE GAME, YOU WILL BE ASKED FOR THE NAMES")
        print("OF YOUR PLAYERS.  THEY ARE ENTERED IN THE ORDER: ")
        print("LEFT WING, CENTER, RIGHT WING, LEFT DEFENSE,")
        print("RIGHT DEFENSE, GOALKEEPER.  ANY OTHER INPUT REQUIRED WILL")
        print("HAVE EXPLANATORY INSTRUCTIONS.")


def team1_action(
    pass_value: int, player_index: List[int], team_a: Team, team_b: Team, j: int
) -> Tuple[int, int, int, int]:
    if pass_value == 1:
        print(
            team_a.players[player_index[j - 2]]
            + " LEADS "
            + team_a.players[player_index[j - 1]]
            + " WITH A PERFECT PASS.\n"
        )
        print(team_a.players[player_index[j - 1]] + " CUTTING IN!!!\n")
        scoring_player = player_index[j - 1]
        goal_assistant1 = player_index[j - 2]
        goal_assistant2 = 0
        z1 = 3
    elif pass_value == 2:
        print(
            team_a.players[player_index[j - 2]]
            + " GIVES TO A STREAKING "
            + team_a.players[player_index[j - 1]]
        )
        print(
            team_a.players[player_index[j - 3]]
            + " COMES DOWN ON "
            + team_b.players[4]
            + " AND "
            + team_b.players[3]
        )
        scoring_player = player_index[j - 3]
        goal_assistant1 = player_index[j - 1]
        goal_assistant2 = player_index[j - 2]
        z1 = 2
    elif pass_value == 3:
        print("OH MY GOD!! A ' 4 ON 2 ' SITUATION\n")
        print(
            team_a.players[player_index[j - 3]]
            + " LEADS "
            + team_a.players[player_index[j - 2]]
            + "\n"
        )
        print(team_a.players[player_index[j - 2]] + " IS WHEELING THROUGH CENTER.\n")
        print(
            team_a.players[player_index[j - 2]]
            + " GIVES AND GOEST WITH "
            + team_a.players[player_index[j - 1]]
            + "\n"
        )
        print("PRETTY PASSING!\n")
        print(
            team_a.players[player_index[j - 1]]
            + " DROPS IT TO "
            + team_a.players[player_index[j - 4]]
            + "\n"
        )
        scoring_player = player_index[j - 4]
        goal_assistant1 = player_index[j - 1]
        goal_assistant2 = player_index[j - 2]
        z1 = 1
    return scoring_player, goal_assistant1, goal_assistant2, z1


def team2_action(
    pass_value: int, player_index: List[int], team_a: Team, team_b: Team, j: int
) -> Tuple[int, int, int, int]:
    if pass_value == 1:
        print(
            team_b.players[player_index[j - 1]]
            + " HITS "
            + team_b.players[player_index[j - 2]]
            + " FLYING DOWN THE LEFT SIDE\n"
        )
        scoring_player = player_index[j - 2]
        goal_assistant1 = player_index[j - 1]
        goal_assistant2 = 0
        z1 = 3
    elif pass_value == 2:
        print("IT'S A ' 3 ON 2 '!\n")
        print(
            "ONLY " + team_a.players[3] + " AND " + team_a.players[4] + " ARE BACK.\n"
        )
        print(
            team_b.players[player_index[j - 2]]
            + " GIVES OFF TO "
            + team_b.players[player_index[j - 1]]
            + "\n"
        )
        print(
            team_b.players[player_index[j - 1]]
            + " DROPS TO "
            + team_b.players[player_index[j - 3]]
            + "\n"
        )
        scoring_player = player_index[j - 3]
        goal_assistant1 = player_index[j - 1]
        goal_assistant2 = player_index[j - 2]
        z1 = 2
    elif pass_value == 3:
        print(" A '3 ON 2 ' WITH A ' TRAILER '!\n")
        print(
            team_b.players[player_index[j - 4]]
            + " GIVES TO "
            + team_b.players[player_index[j - 2]]
            + " WHO SHUFFLES IT OFF TO\n"
        )
        print(
            team_b.players[player_index[j - 1]] + " WHO FIRES A WING TO WING PASS TO \n"
        )
        print(team_b.players[player_index[j - 3]] + " AS HE CUTS IN ALONE!!\n")
        scoring_player = player_index[j - 3]
        goal_assistant1 = player_index[j - 1]
        goal_assistant2 = player_index[j - 2]
        z1 = 1
    return scoring_player, goal_assistant1, goal_assistant2, z1


def final_message(team_a: Team, team_b: Team, player_index: List[int]) -> None:
    # Bells chime
    print("THAT'S THE SIREN\n")
    print("\n")
    print(" " * 15 + "FINAL SCORE:\n")
    if team_b.score <= team_a.score:
        print(f"{team_a.name}: {team_a.score}\t{team_b.name}: {team_b.score}\n")
    else:
        print(f"{team_b.name}: {team_b.score}\t{team_a.name}\t:{team_a.score}\n")
    print("\n")
    print(" " * 10 + "SCORING SUMMARY\n")
    print("\n")
    print(" " * 25 + team_a.name + "\n")
    print("\tNAME\tGOALS\tASSISTS\n")
    print("\t----\t-----\t-------\n")
    for i in range(1, 6):
        print(f"\t{team_a.players[i]}\t{team_a.goals[i]}\t{team_a.assists[i]}\n")
    print("\n")
    print(" " * 25 + team_b.name + "\n")
    print("\tNAME\tGOALS\tASSISTS\n")
    print("\t----\t-----\t-------\n")
    for t in range(1, 6):
        print(f"\t{team_b.players[t]}\t{team_b.goals[t]}\t{team_b.assists[t]}\n")
    print("\n")
    print("SHOTS ON NET\n")
    print(f"{team_a.name}: {team_a.shots_on_net}\n")
    print(team_b.name + f": {team_b.shots_on_net}\n")


def main() -> None:
    # Intro
    print_header()
    player_index: List[int] = [0 for _ in range(21)]
    print("\n" * 3)
    instructions()

    # Gather input
    team_name_a, team_name_b = get_team_names()
    print()
    minutes_per_game = get_minutes_per_game()
    print()
    players_a = get_player_names(f"WOULD THE {team_name_a} COACH ENTER HIS TEAM")
    print()
    players_b = get_player_names(f"WOULD THE {team_name_b} COACH DO THE SAME")
    team_a = Team(team_name_a, players_a)
    team_b = Team(team_name_b, players_b)
    print()
    referee = input("INPUT THE REFEREE FOR THIS GAME: ")
    print()
    team_a.show_lineup()
    print()
    team_b.show_lineup()
    print("WE'RE READY FOR TONIGHTS OPENING FACE-OFF.")
    print(
        f"{referee} WILL DROP THE PUCK BETWEEN "
        f"{team_a.players[0]} AND {team_b.players[0]}"
    )
    remaining_time = minutes_per_game

    # Play the game
    while remaining_time > 0:
        cont, remaining_time = simulate_game_round(
            team_a, team_b, player_index, remaining_time
        )
        remaining_time -= 1
        if cont == "break":
            break

    # Outro
    final_message(team_a, team_b, player_index)


def handle_hit(
    controlling_team: int,
    team_a: Team,
    team_b: Team,
    player_index: List[int],
    goal_player: int,
    goal_assistant1: int,
    goal_assistant2: int,
    hit_area: int,
    z: int,
) -> int:
    while True:
        player_index[20] = randint(1, 100)
        if player_index[20] % z != 0:
            break
        a2 = randint(1, 100)
        if a2 % 4 == 0:
            if controlling_team == 1:
                print(f"SAVE {team_b.players[5]} --  REBOUND\n")
            else:
                print(f"SAVE {team_a.players[5]} --  FOLLOW up\n")
            continue
        else:
            hit_area += 1
    if player_index[20] % z != 0:
        if controlling_team == 1:
            print(f"GOAL {team_a.name}\n")
            team_a.score += 1
        else:
            print(f"SCORE {team_b.name}\n")
            team_b.score += 1
        # Bells in origninal
        print("\n")
        print("SCORE: ")
        if team_b.score <= team_a.score:
            print(f"{team_a.name}: {team_a.score}\t{team_b.name}: {team_b.score}\n")
        else:
            print(f"{team_b.name}: {team_b.score}\t{team_a.name}: {team_a.score}\n")
        if controlling_team == 1:
            print("GOAL SCORED BY: " + team_a.players[goal_player] + "\n")
            if goal_assistant1 != 0:
                if goal_assistant2 != 0:
                    print(
                        " ASSISTED BY: "
                        + team_a.players[goal_assistant1]
                        + " AND "
                        + team_a.players[goal_assistant2]
                        + "\n"
                    )
                else:
                    print(" ASSISTED BY: " + team_a.players[goal_assistant1] + "\n")
            else:
                print(" UNASSISTED.\n")
            team_a.goals[goal_player] += 1
            team_a.assists[goal_assistant1] += 1
            team_a.assists[goal_assistant2] += 1
            # 1540
        else:
            print("GOAL SCORED BY: " + team_b.players[goal_player] + "\n")
            if goal_assistant1 != 0:
                if goal_assistant2 != 0:
                    print(
                        " ASSISTED BY: "
                        + team_b.players[goal_assistant1]
                        + " AND "
                        + team_b.players[goal_assistant2]
                        + "\n"
                    )
                else:
                    print(" ASSISTED BY: " + team_b.players[goal_assistant1] + "\n")
            else:
                print(" UNASSISTED.\n")
            team_b.goals[goal_player] += 1
            team_b.assists[goal_assistant1] += 1
            team_b.assists[goal_assistant2] += 1
            # 1540
    return hit_area


def handle_miss(
    controlling_team: int,
    team_a: Team,
    team_b: Team,
    remaining_time: int,
    goal_player: int,
) -> Tuple[str, int]:
    saving_player = randint(1, 7)
    if controlling_team == 1:
        if saving_player == 1:
            print("KICK SAVE AND A BEAUTY BY " + team_b.players[5] + "\n")
            print("CLEARED OUT BY " + team_b.players[3] + "\n")
            remaining_time -= 1
            return ("continue", remaining_time)
        if saving_player == 2:
            print("WHAT A SPECTACULAR GLOVE SAVE BY " + team_b.players[5] + "\n")
            print("AND " + team_b.players[5] + " GOLFS IT INTO THE CROWD\n")
            return ("break", remaining_time)
        if saving_player == 3:
            print("SKATE SAVE ON A LOW STEAMER BY " + team_b.players[5] + "\n")
            remaining_time -= 1
            return ("continue", remaining_time)
        if saving_player == 4:
            print(f"PAD SAVE BY {team_b.players[5]} OFF THE STICK\n")
            print(
                f"OF {team_a.players[goal_player]} AND "
                f"{team_b.players[5]} COVERS UP\n"
            )
            return ("break", remaining_time)
        if saving_player == 5:
            print(f"WHISTLES ONE OVER THE HEAD OF {team_b.players[5]}\n")
            remaining_time -= 1
            return ("continue", remaining_time)
        if saving_player == 6:
            print(f"{team_b.players[5]} MAKES A FACE SAVE!! AND HE IS HURT\n")
            print(f"THE DEFENSEMAN {team_b.players[5]} COVERS UP FOR HIM\n")
            return ("break", remaining_time)
    else:
        if saving_player == 1:
            print(f"STICK SAVE BY {team_a.players[5]}\n")
            print(f"AND CLEARED OUT BY {team_a.players[3]}\n")
            remaining_time -= 1
            return ("continue", remaining_time)
        if saving_player == 2:
            print(
                "OH MY GOD!! "
                f"{team_b.players[goal_player]} RATTLES ONE OFF THE POST\n"
            )
            print(
                f"TO THE RIGHT OF {team_a.players[5]} AND "
                f"{team_a.players[5]} COVERS "
            )
            print("ON THE LOOSE PUCK!\n")
            return ("break", remaining_time)
        if saving_player == 3:
            print("SKATE SAVE BY " + team_a.players[5] + "\n")
            print(team_a.players[5] + " WHACKS THE LOOSE PUCK INTO THE STANDS\n")
            return ("break", remaining_time)
        if saving_player == 4:
            print(
                "STICK SAVE BY " + team_a.players[5] + " AND HE CLEARS IT OUT HIMSELF\n"
            )
            remaining_time -= 1
            return ("continue", remaining_time)
        if saving_player == 5:
            print("KICKED OUT BY " + team_a.players[5] + "\n")
            print("AND IT REBOUNDS ALL THE WAY TO CENTER ICE\n")
            remaining_time -= 1
            return ("continue", remaining_time)
        if saving_player == 6:
            print("GLOVE SAVE " + team_a.players[5] + " AND HE HANGS ON\n")
            return ("break", remaining_time)
    return ("continue", remaining_time)


def simulate_game_round(
    team_a: Team, team_b: Team, player_index: List[int], remaining_time: int
) -> Tuple[str, int]:
    controlling_team = randint(1, 2)
    if controlling_team == 1:
        print(f"{team_a.name} HAS CONTROL OF THE PUCK.")
    else:
        print(f"{team_b.name} HAS CONTROL.")
    pass_value = get_pass()
    for i in range(1, 4):
        player_index[i] = 0

    # Line 310:
    while True:
        j = 0
        for j in range(1, (pass_value + 2) + 1):
            player_index[j] = randint(1, 5)
        if player_index[j - 1] == player_index[j - 2] or (
            pass_value + 2 >= 3
            and (
                player_index[j - 1] == player_index[j - 3]
                or player_index[j - 2] == player_index[j - 3]
            )
        ):
            break
    if pass_value == 0:  # line 350
        z, goal_player, goal_assistant1, goal_assistant2 = make_shot(
            controlling_team, team_a, team_b, player_index, j
        )
    else:
        if controlling_team == 1:
            goal_player, goal_assistant1, goal_assistant2, z1 = team1_action(
                pass_value, player_index, team_a, team_b, j
            )
        else:
            goal_player, goal_assistant1, goal_assistant2, z1 = team2_action(
                pass_value, player_index, team_a, team_b, j
            )
        while True:
            shot_type = int(input("SHOT? "))
            if not (shot_type < 1 or shot_type > 4):
                break
        if controlling_team == 1:
            print(team_a.players[goal_player], end="")
        else:
            print(team_b.players[goal_player], end="")
        if shot_type == 1:
            print(" LET'S A BIG SLAP SHOT GO!!\n")
            z = 4
            z += z1
        if shot_type == 2:
            print(" RIPS A WRIST SHOT OFF\n")
            z = 2
            z += z1
        if shot_type == 3:
            print(" GETS A BACKHAND OFF\n")
            z = 3
            z += z1
        if shot_type == 4:
            print(" SNAPS OFF A SNAP SHOT\n")
            z = 2
            z += z1
    while True:
        goal_area = int(input("AREA? "))
        if not (goal_area < 1 or goal_area > 4):
            break
    if controlling_team == 1:
        team_a.shots_on_net += 1
    else:
        team_b.shots_on_net += 1
    hit_area = randint(1, 5)
    if goal_area == hit_area:
        hit_area = handle_hit(
            controlling_team,
            team_a,
            team_b,
            player_index,
            goal_player,
            goal_assistant1,
            goal_assistant2,
            hit_area,
            z,
        )
    if goal_area != hit_area:
        return handle_miss(
            controlling_team, team_a, team_b, remaining_time, goal_player
        )
    print("AND WE'RE READY FOR THE FACE-OFF\n")
    return ("continue", remaining_time)


if __name__ == "__main__":
    main()
