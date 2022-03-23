#!/usr/bin/env python3
import random
from dataclasses import dataclass
from typing import Tuple, Dict, NamedTuple, Literal


class HitStats(NamedTuple):
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
    best: int  # TODO: This is never used!
    is_computer: bool
    weakness: int

    # for each of the 4 punch types, we have a probability of hitting
    hit_stats: Dict[Literal[1, 2, 3, 4], HitStats]

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

# Texts
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
        hit_stats={
            1: HitStats(  # FULL SWING
                choices=30,
                threshold=10,
                hit_damage=15,
                block_damage=0,
                pre_msg="{active.name} SWINGS AND",
                hit_msg="HE CONNECTS!",
                blocked_msg="HE MISSES",
            ),
            2: HitStats(  # HOOK
                choices=2,
                threshold=1,
                hit_damage=7,
                block_damage=0,
                pre_msg="{active.name} GIVES THE HOOK...",
                hit_msg="",
                blocked_msg="",
            ),
            3: HitStats(  # UPPERCUT
                choices=100,
                threshold=50,
                hit_damage=4,
                block_damage=0,
                pre_msg="{player_name} TRIES AN UPPERCUT",
                hit_msg="AND HE CONNECTS!",
                blocked_msg="AND IT'S BLOCKED (LUCKY BLOCK!)",
            ),
            4: HitStats(  # JAB
                choices=8,
                threshold=3,
                hit_damage=3,
                block_damage=0,
                pre_msg="{active.name} JABS AT {passive.name}'S HEAD",
                hit_msg="AND HE CONNECTS!",
                blocked_msg="AND IT'S BLOCKED (LUCKY BLOCK!)",
            ),
        },
    )

    opponent_best, opponent_weakness = get_opponent_stats()
    opponent = Player(
        name=opponent_name,
        best=opponent_best,
        weakness=opponent_weakness,
        is_computer=True,
        hit_stats={
            1: HitStats(  # FULL SWING
                choices=60,
                threshold=29,
                hit_damage=15,
                block_damage=0,
                knockout_possible=True,
                pre_msg="{active.name} TAKES A FULL SWING",
                hit_msg="AND POW!!!! HE HITS HIM RIGHT IN THE FACE!",
                blocked_msg="BUT IT'S BLOCKED!",
            ),
            2: HitStats(  # HOOK
                choices=1,
                threshold=1,
                hit_damage=12,
                block_damage=0,
                knockout_possible=True,
                pre_msg="{active.name} GETS {passive.name} IN THE JAW (OUCH!)....AND AGAIN",
                hit_msg="CONNECTS...",
                blocked_msg="BUT IT'S BLOCKED!!!!!!!!!!!!!",
            ),
            3: HitStats(  # UPPERCUT
                choices=200,
                threshold=125,
                hit_damage=8,
                block_damage=5,
                pre_msg="{passive.name} IS ATTACKED BY AN UPPERCUT (OH,OH)",
                hit_msg="AND {active.name} CONNECTS...",
                blocked_msg="{passive.name} BLOCKS AND HITS {active.name} WITH A HOOK",
            ),
            4: HitStats(  # JAB
                choices=7,
                threshold=3,
                hit_damage=3,
                block_damage=0,
                pre_msg="{active.name} JABS AND",
                hit_msg="BLOOD SPILLS !!!",
                blocked_msg="AND IT'S BLOCKED (LUCKY BLOCK!)",
            ),
        },
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
        hit_stats = active.hit_stats[punch]

        if punch == passive.weakness:
            passive.damage += 2

        print(hit_stats.pre_msg.format(active=active, passive=passive), end=" ")
        if passive.weakness == punch or hit_stats.is_hit():
            print(hit_stats.hit_msg.format(active=active, passive=passive))
            if hit_stats.knockout_possible and passive.damage > KNOCKOUT_THRESHOLD:
                passive.knockedout = True
                break
            passive.damage += hit_stats.hit_damage
        else:
            print(hit_stats.blocked_msg.format(active=active, passive=passive))
            active.damage += hit_stats.block_damage

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
