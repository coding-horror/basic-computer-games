#!/usr/bin/env python3
import json
import random
from dataclasses import dataclass
from pathlib import Path
from typing import Dict, Literal, NamedTuple, Tuple


class PunchProfile(NamedTuple):
    choices: int
    threshold: int
    hit_damage: int
    block_damage: int

    pre_msg: str
    hit_msg: str
    blocked_msg: str

    knockout_possible: bool = False

    def is_hit(self) -> bool:
        return random.randint(1, self.choices) <= self.threshold


@dataclass
class Player:
    name: str
    best: int  # this hit guarantees 2 damage on opponent
    weakness: int  # you're always hit when your opponent uses this punch
    is_computer: bool

    # for each of the 4 punch types, we have a probability of hitting
    punch_profiles: Dict[Literal[1, 2, 3, 4], PunchProfile]

    damage: int = 0
    score: int = 0
    knockedout: bool = False

    def get_punch_choice(self) -> Literal[1, 2, 3, 4]:
        if self.is_computer:
            return random.randint(1, 4)  # type: ignore
        else:
            punch = -1
            while punch not in [1, 2, 3, 4]:
                print(f"{self.name}'S PUNCH", end="? ")
                punch = int(input())
            return punch  # type: ignore


KNOCKOUT_THRESHOLD = 35

QUESTION_PROMPT = "? "
KNOCKED_COLD = "{loser} IS KNOCKED COLD AND {winner} IS THE WINNER AND CHAMP"


def get_vulnerability() -> int:
    print("WHAT IS HIS VULNERABILITY", end=QUESTION_PROMPT)
    vulnerability = int(input())
    return vulnerability


def get_opponent_stats() -> Tuple[int, int]:
    opponent_best = 0
    opponent_weakness = 0
    while opponent_best == opponent_weakness:
        opponent_best = random.randint(1, 4)
        opponent_weakness = random.randint(1, 4)
    return opponent_best, opponent_weakness


def read_punch_profiles(filepath: Path) -> Dict[Literal[1, 2, 3, 4], PunchProfile]:
    with open(filepath) as f:
        punch_profile_dict = json.load(f)
    result = {}
    for key, value in punch_profile_dict.items():
        result[int(key)] = PunchProfile(**value)
    return result  # type: ignore


def play() -> None:
    print("BOXING")
    print("CREATIVE COMPUTING   MORRISTOWN, NEW JERSEY")
    print("\n\n")
    print("BOXING OLYMPIC STYLE (3 ROUNDS -- 2 OUT OF 3 WINS)")

    print("WHAT IS YOUR OPPONENT'S NAME", end=QUESTION_PROMPT)
    opponent_name = input()
    print("WHAT IS YOUR MAN'S NAME", end=QUESTION_PROMPT)
    player_name = input()

    print("DIFFERENT PUNCHES ARE 1 FULL SWING 2 HOOK 3 UPPERCUT 4 JAB")
    print("WHAT IS YOUR MAN'S BEST", end=QUESTION_PROMPT)
    player_best = int(input())  # noqa: TODO - this likely is a bug!
    player_weakness = get_vulnerability()
    player = Player(
        name=player_name,
        best=player_best,
        weakness=player_weakness,
        is_computer=False,
        punch_profiles=read_punch_profiles(
            Path(__file__).parent / "player-profile.json"
        ),
    )

    opponent_best, opponent_weakness = get_opponent_stats()
    opponent = Player(
        name=opponent_name,
        best=opponent_best,
        weakness=opponent_weakness,
        is_computer=True,
        punch_profiles=read_punch_profiles(
            Path(__file__).parent / "opponent-profile.json"
        ),
    )

    print(
        f"{opponent.name}'S ADVANTAGE is {opponent.weakness} AND VULNERABILITY IS SECRET."
    )

    for round_number in (1, 2, 3):
        play_round(round_number, player, opponent)

    if player.knockedout:
        print(KNOCKED_COLD.format(loser=player.name, winner=opponent.name))
    elif opponent.knockedout:
        print(KNOCKED_COLD.format(loser=opponent.name, winner=player.name))
    elif opponent.score > player.score:
        print(f"{opponent.name} WINS (NICE GOING), {player.name}")
    else:
        print(f"{player.name} AMAZINGLY WINS")

    print("\n\nAND NOW GOODBYE FROM THE OLYMPIC ARENA.")


def is_opponents_turn() -> bool:
    return random.randint(1, 10) > 5


def play_round(round_number: int, player: Player, opponent: Player) -> None:
    print(f"ROUND {round_number} BEGINS...\n")
    if opponent.score >= 2 or player.score >= 2:
        return

    for _action in range(7):
        if is_opponents_turn():
            punch = opponent.get_punch_choice()
            active = opponent
            passive = player
        else:
            punch = player.get_punch_choice()
            active = player
            passive = opponent

        # Load the hit characteristics of the current player's punch
        punch_profile = active.punch_profiles[punch]

        if punch == active.best:
            passive.damage += 2

        print(punch_profile.pre_msg.format(active=active, passive=passive), end=" ")
        if passive.weakness == punch or punch_profile.is_hit():
            print(punch_profile.hit_msg.format(active=active, passive=passive))
            if punch_profile.knockout_possible and passive.damage > KNOCKOUT_THRESHOLD:
                passive.knockedout = True
                break
            passive.damage += punch_profile.hit_damage
        else:
            print(punch_profile.blocked_msg.format(active=active, passive=passive))
            active.damage += punch_profile.block_damage

    if player.knockedout or opponent.knockedout:
        return
    elif player.damage > opponent.damage:
        print(f"{opponent.name} WINS ROUND {round_number}")
        opponent.score += 1
    else:
        print(f"{player.name} WINS ROUND {round_number}")
        player.score += 1


if __name__ == "__main__":
    play()
