"""
The basketball class is a computer game that allows you to play as
Dartmouth College's captain and playmaker
The game uses set probabilites to simulate outcomes of each posession
You are able to choose your shot types as well as defensive formations
"""

import random
from typing import List, Literal, Optional


def print_intro() -> None:
    print("\t\t\t Basketball")
    print("\t Creative Computing  Morristown, New Jersey\n\n\n")
    print("This is Dartmouth College basketball. ")
    print("Υou will be Dartmouth captain and playmaker.")
    print("Call shots as follows:")
    print(
        "1. Long (30ft.) Jump Shot; "
        "2. Short (15 ft.) Jump Shot; "
        "3. Lay up; 4. Set Shot"
    )
    print("Both teams will use the same defense. Call Defense as follows:")
    print("6. Press; 6.5 Man-to-Man; 7. Zone; 7.5 None.")
    print("To change defense, just type 0 as your next shot.")
    print("Your starting defense will be? ", end="")


class Basketball:
    def __init__(self) -> None:
        self.time = 0
        self.score = [0, 0]  # first value is opponents score, second is home
        self.defense_choices: List[float] = [6, 6.5, 7, 7.5]
        self.shot: Optional[int] = None
        self.shot_choices: List[Literal[0, 1, 2, 3, 4]] = [0, 1, 2, 3, 4]
        self.z1: Optional[float] = None

        print_intro()

        self.defense = get_defense_choice(self.defense_choices)

        self.opponent = get_opponents_name()
        self.start_of_period()

    def add_points(self, team: Literal[0, 1], points: Literal[0, 1, 2]) -> None:
        """
        Add points to the score.

        Team can take 0 or 1, for opponent or Dartmouth, respectively
        """
        self.score[team] += points
        self.print_score()

    def ball_passed_back(self) -> None:
        print("Ball passed back to you. ", end="")
        self.dartmouth_ball()

    def change_defense(self) -> None:
        """change defense, called when the user enters 0 for their shot"""
        defense = None

        while defense not in self.defense_choices:
            print("Your new defensive allignment is? ")
            try:
                defense = float(input())
            except ValueError:
                continue
        assert isinstance(defense, float)
        self.defense = defense
        self.dartmouth_ball()

    def foul_shots(self, team: Literal[0, 1]) -> None:
        """Simulate two foul shots for a player and adds the points."""
        print("Shooter fouled.  Two shots.")
        if random.random() > 0.49:
            if random.random() > 0.75:
                print("Both shots missed.")
            else:
                print("Shooter makes one shot and misses one.")
                self.score[team] += 1
        else:
            print("Shooter makes both shots.")
            self.score[team] += 2

        self.print_score()

    def halftime(self) -> None:
        """called when t = 50, starts a new period"""
        print("\n   ***** End of first half *****\n")
        self.print_score()
        self.start_of_period()

    def print_score(self) -> None:
        """Print the current score"""
        print(f"Score:  {self.score[1]} to {self.score[0]}\n")

    def start_of_period(self) -> None:
        """Simulate a center jump for posession at the beginning of a period"""
        print("Center jump")
        if random.random() > 0.6:
            print("Dartmouth controls the tap.\n")
            self.dartmouth_ball()
        else:
            print(self.opponent + " controls the tap.\n")
            self.opponent_ball()

    def two_minute_warning(self) -> None:
        """called when t = 92"""
        print("   *** Two minutes left in the game ***")

    def dartmouth_jump_shot(self) -> None:
        """called when the user enters 1 or 2 for their shot"""
        self.time += 1
        if self.time == 50:
            self.halftime()
        elif self.time == 92:
            self.two_minute_warning()
        print("Jump Shot.")
        # simulates chances of different possible outcomes
        if random.random() > 0.341 * self.defense / 8:
            if random.random() > 0.682 * self.defense / 8:
                if random.random() > 0.782 * self.defense / 8:
                    if random.random() > 0.843 * self.defense / 8:
                        print("Charging foul. Dartmouth loses ball.\n")
                        self.opponent_ball()
                    else:
                        # player is fouled
                        self.foul_shots(1)
                        self.opponent_ball()
                else:
                    if random.random() > 0.5:
                        print(
                            "Shot is blocked. Ball controlled by "
                            + self.opponent
                            + ".\n"
                        )
                        self.opponent_ball()
                    else:
                        print("Shot is blocked. Ball controlled by Dartmouth.")
                        self.dartmouth_ball()
            else:
                print("Shot is off target.")
                if self.defense / 6 * random.random() > 0.45:
                    print("Rebound to " + self.opponent + "\n")
                    self.opponent_ball()
                else:
                    print("Dartmouth controls the rebound.")
                    if random.random() > 0.4:
                        if self.defense == 6 and random.random() > 0.6:
                            print("Pass stolen by " + self.opponent + ", easy lay up")
                            self.add_points(0, 2)
                            self.dartmouth_ball()
                        else:
                            # ball is passed back to you
                            self.ball_passed_back()
                    else:
                        print()
                        self.dartmouth_non_jump_shot()
        else:
            print("Shot is good.")
            self.add_points(1, 2)
            self.opponent_ball()

    def dartmouth_non_jump_shot(self) -> None:
        """
        Lay up, set shot, or defense change

        called when the user enters 0, 3, or 4
        """
        self.time += 1
        if self.time == 50:
            self.halftime()
        elif self.time == 92:
            self.two_minute_warning()

        if self.shot == 4:
            print("Set shot.")
        elif self.shot == 3:
            print("Lay up.")
        elif self.shot == 0:
            self.change_defense()

        # simulates different outcomes after a lay up or set shot
        if 7 / self.defense * random.random() > 0.4:
            if 7 / self.defense * random.random() > 0.7:
                if 7 / self.defense * random.random() > 0.875:
                    if 7 / self.defense * random.random() > 0.925:
                        print("Charging foul. Dartmouth loses the ball.\n")
                        self.opponent_ball()
                    else:
                        print("Shot blocked. " + self.opponent + "'s ball.\n")
                        self.opponent_ball()
                else:
                    self.foul_shots(1)
                    self.opponent_ball()
            else:
                print("Shot is off the rim.")
                if random.random() > 2 / 3:
                    print("Dartmouth controls the rebound.")
                    if random.random() > 0.4:
                        print("Ball passed back to you.\n")
                        self.dartmouth_ball()
                    else:
                        self.dartmouth_non_jump_shot()
                else:
                    print(self.opponent + " controls the rebound.\n")
                    self.opponent_ball()
        else:
            print("Shot is good. Two points.")
            self.add_points(1, 2)
            self.opponent_ball()

    def dartmouth_ball(self) -> None:
        """plays out a Dartmouth posession, starting with your choice of shot"""
        shot = get_dartmouth_ball_choice(self.shot_choices)
        self.shot = shot

        if self.time < 100 or random.random() < 0.5:
            if self.shot == 1 or self.shot == 2:
                self.dartmouth_jump_shot()
            else:
                self.dartmouth_non_jump_shot()
        else:
            if self.score[0] != self.score[1]:
                print("\n   ***** End Of Game *****")
                print(
                    "Final Score: Dartmouth: "
                    + str(self.score[1])
                    + "  "
                    + self.opponent
                    + ": "
                    + str(self.score[0])
                )
            else:
                print("\n   ***** End Of Second Half *****")
                print("Score at end of regulation time:")
                print(
                    "     Dartmouth: "
                    + str(self.score[1])
                    + " "
                    + self.opponent
                    + ": "
                    + str(self.score[0])
                )
                print("Begin two minute overtime period")
                self.time = 93
                self.start_of_period()

    def opponent_jumpshot(self) -> None:
        """Simulate the opponents jumpshot"""
        print("Jump Shot.")
        if 8 / self.defense * random.random() > 0.35:
            if 8 / self.defense * random.random() > 0.75:
                if 8 / self.defense * random.random() > 0.9:
                    print("Offensive foul. Dartmouth's ball.\n")
                    self.dartmouth_ball()
                else:
                    self.foul_shots(0)
                    self.dartmouth_ball()
            else:
                print("Shot is off the rim.")
                if self.defense / 6 * random.random() > 0.5:
                    print(self.opponent + " controls the rebound.")
                    if self.defense == 6:
                        if random.random() > 0.75:
                            print("Ball stolen. Easy lay up for Dartmouth.")
                            self.add_points(1, 2)
                            self.opponent_ball()
                        else:
                            if random.random() > 0.5:
                                print()
                                self.opponent_non_jumpshot()
                            else:
                                print("Pass back to " + self.opponent + " guard.\n")
                                self.opponent_ball()
                    else:
                        if random.random() > 0.5:
                            self.opponent_non_jumpshot()
                        else:
                            print("Pass back to " + self.opponent + " guard.\n")
                            self.opponent_ball()
                else:
                    print("Dartmouth controls the rebound.\n")
                    self.dartmouth_ball()
        else:
            print("Shot is good.")
            self.add_points(0, 2)
            self.dartmouth_ball()

    def opponent_non_jumpshot(self) -> None:
        """Simulate opponents lay up or set shot."""
        if self.z1 > 3:  # type: ignore
            print("Set shot.")
        else:
            print("Lay up")
        if 7 / self.defense * random.random() > 0.413:
            print("Shot is missed.")
            if self.defense / 6 * random.random() > 0.5:
                print(self.opponent + " controls the rebound.")
                if self.defense == 6:
                    if random.random() > 0.75:
                        print("Ball stolen. Easy lay up for Dartmouth.")
                        self.add_points(1, 2)
                        self.opponent_ball()
                    else:
                        if random.random() > 0.5:
                            print()
                            self.opponent_non_jumpshot()
                        else:
                            print("Pass back to " + self.opponent + " guard.\n")
                            self.opponent_ball()
                else:
                    if random.random() > 0.5:
                        print()
                        self.opponent_non_jumpshot()
                    else:
                        print("Pass back to " + self.opponent + " guard\n")
                        self.opponent_ball()
            else:
                print("Dartmouth controls the rebound.\n")
                self.dartmouth_ball()
        else:
            print("Shot is good.")
            self.add_points(0, 2)
            self.dartmouth_ball()

    def opponent_ball(self) -> None:
        """
        Simulate an opponents possesion

        Randomly picks jump shot or lay up / set shot.
        """
        self.time += 1
        if self.time == 50:
            self.halftime()
        self.z1 = 10 / 4 * random.random() + 1
        if self.z1 > 2:
            self.opponent_non_jumpshot()
        else:
            self.opponent_jumpshot()


def get_defense_choice(defense_choices: List[float]) -> float:
    """Takes input for a defense"""
    try:
        defense = float(input())
    except ValueError:
        defense = None

    # if the input wasn't a valid defense, takes input again
    while defense not in defense_choices:
        print("Your new defensive allignment is? ", end="")
        try:
            defense = float(input())
        except ValueError:
            continue
    assert isinstance(defense, float)
    return defense


def get_dartmouth_ball_choice(shot_choices: List[Literal[0, 1, 2, 3, 4]]) -> int:
    print("Your shot? ", end="")
    shot = None
    try:
        shot = int(input())
    except ValueError:
        shot = None

    while shot not in shot_choices:
        print("Incorrect answer. Retype it. Your shot? ", end="")
        try:
            shot = int(input())
        except Exception:
            continue
    assert isinstance(shot, int)
    return shot


def get_opponents_name() -> str:
    """Take input for opponent's name"""
    print("\nChoose your opponent? ", end="")
    return input()


if __name__ == "__main__":
    Basketball()
