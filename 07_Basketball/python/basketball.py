import random

# The basketball class is a computer game that allows you to play as
# Dartmouth College's captain and playmaker
# The game uses set probabilites to simulate outcomes of each posession
# You are able to choose your shot types as well as defensive formations


class Basketball():
    def __init__(self):
        self.time = 0
        self.score = [0, 0]  # first value is opponents score, second is home
        self.defense = None
        self.defense_choices = [6, 6.5, 7, 7.5]
        self.shot = None
        self.shot_choices = [0, 1, 2, 3, 4]
        self.z1 = None

        # Explains the keyboard inputs
        print("\t\t\t Basketball")
        print("\t Creative Computing  Morristown, New Jersey\n\n\n")
        print("This is Dartmouth College basketball. ")
        print("Υou will be Dartmouth captain and playmaker.")
        print("Call shots as follows:")
        print("1. Long (30ft.) Jump Shot; 2. Short (15 ft.) Jump Shot; "
              + "3. Lay up; 4. Set Shot")
        print("Both teams will use the same defense. Call Defense as follows:")
        print("6. Press; 6.5 Man-to-Man; 7. Zone; 7.5 None.")
        print("To change defense, just type 0 as your next shot.")
        print("Your starting defense will be? ", end='')

        # takes input for a defense
        try:
            self.defense = float(input())

        except ValueError:
            self.defense = None

        # if the input wasn't a valid defense, takes input again
        while self.defense not in self.defense_choices:
            print("Your new defensive allignment is? ", end='')
            try:
                self.defense = float(input())

            except ValueError:
                continue

        # takes input for opponent's name
        print("\nChoose your opponent? ", end='')

        self.opponent = input()
        self.start_of_period()

    # adds points to the score
    # team can take 0 or 1, for opponent or Dartmouth, respectively
    def add_points(self, team, points):
        self.score[team] += points
        self.print_score()

    def ball_passed_back(self):
        print("Ball passed back to you. ", end='')
        self.dartmouth_ball()

    # change defense, called when the user enters 0 for their shot
    def change_defense(self):
        self.defense = None

        while self.defense not in self.defense_choices:
            print("Your new defensive allignment is? ")
            try:
                self.defense = float(input())

            except ValueError:
                continue
        self.dartmouth_ball()

    # simulates two foul shots for a player and adds the points
    def foul_shots(self, team):
        print("Shooter fouled.  Two shots.")
        if random.random() > .49:
            if random.random() > .75:
                print("Both shots missed.")
            else:
                print("Shooter makes one shot and misses one.")
                self.score[team] += 1
        else:
            print("Shooter makes both shots.")
            self.score[team] += 2

        self.print_score()

    # called when t = 50, starts a new period
    def halftime(self):
        print("\n   ***** End of first half *****\n")
        self.print_score()
        self.start_of_period()

    # prints the current score
    def print_score(self):
        print("Score:  " + str(self.score[1])
              + " to " + str(self.score[0]) + "\n")

    # simulates a center jump for posession at the beginning of a period
    def start_of_period(self):
        print("Center jump")
        if random.random() > .6:
            print("Dartmouth controls the tap.\n")
            self.dartmouth_ball()
        else:
            print(self.opponent + " controls the tap.\n")
            self.opponent_ball()

    # called when t = 92
    def two_minute_warning(self):
        print("   *** Two minutes left in the game ***")

    # called when the user enters 1 or 2 for their shot
    def dartmouth_jump_shot(self):
        self.time += 1
        if self.time == 50:
            self.halftime()
        elif self.time == 92:
            self.two_minute_warning()
        print("Jump Shot.")
        # simulates chances of different possible outcomes
        if random.random() > .341 * self.defense / 8:
            if random.random() > .682 * self.defense / 8:
                if random.random() > .782 * self.defense / 8:
                    if random.random() > .843 * self.defense / 8:
                        print("Charging foul. Dartmouth loses ball.\n")
                        self.opponent_ball()
                    else:
                        # player is fouled
                        self.foul_shots(1)
                        self.opponent_ball()
                else:
                    if random.random() > .5:
                        print("Shot is blocked. Ball controlled by " +
                              self.opponent + ".\n")
                        self.opponent_ball()
                    else:
                        print("Shot is blocked. Ball controlled by Dartmouth.")
                        self.dartmouth_ball()
            else:
                print("Shot is off target.")
                if self.defense / 6 * random.random() > .45:
                    print("Rebound to " + self.opponent + "\n")
                    self.opponent_ball()
                else:
                    print("Dartmouth controls the rebound.")
                    if random.random() > .4:
                        if self.defense == 6 and random.random() > .6:
                            print("Pass stolen by " + self.opponent
                                  + ", easy lay up")
                            self.add_points(0, 2)
                            self.dartmouth_ball()
                        else:
                            # ball is passed back to you
                            self.ball_passed_back()
                    else:
                        print("")
                        self.dartmouth_non_jump_shot()
        else:
            print("Shot is good.")
            self.add_points(1, 2)
            self.opponent_ball()

    # called when the user enters 0, 3, or 4
    # lay up, set shot, or defense change
    def dartmouth_non_jump_shot(self):
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
        if 7/self.defense*random.random() > .4:
            if 7/self.defense*random.random() > .7:
                if 7/self.defense*random.random() > .875:
                    if 7/self.defense*random.random() > .925:
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
                if random.random() > 2/3:
                    print("Dartmouth controls the rebound.")
                    if random.random() > .4:
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

    # plays out a Dartmouth posession, starting with your choice of shot
    def dartmouth_ball(self):
        print("Your shot? ", end='')
        self.shot = None
        try:
            self.shot = int(input())
        except ValueError:
            self.shot = None

        while self.shot not in self.shot_choices:
            print("Incorrect answer. Retype it. Your shot? ", end='')
            try:
                self.shot = int(input())
            except:
                continue

        if self.time < 100 or random.random() < .5:
            if self.shot == 1 or self.shot == 2:
                self.dartmouth_jump_shot()
            else:
                self.dartmouth_non_jump_shot()
        else:
            if self.score[0] != self.score[1]:
                print("\n   ***** End Of Game *****")
                print("Final Score: Dartmouth: " + str(self.score[1]) + "  "
                      + self.opponent + ": " + str(self.score[0]))
            else:
                print("\n   ***** End Of Second Half *****")
                print("Score at end of regulation time:")
                print("     Dartmouth: " + str(self.score[1]) + " " +
                      self.opponent + ": " + str(self.score[0]))
                print("Begin two minute overtime period")
                self.time = 93
                self.start_of_period()

    # simulates the opponents jumpshot
    def opponent_jumpshot(self):
        print("Jump Shot.")
        if 8/self.defense*random.random() > .35:
            if 8/self.defense*random.random() > .75:
                if 8/self.defense*random.random() > .9:
                    print("Offensive foul. Dartmouth's ball.\n")
                    self.dartmouth_ball()
                else:
                    self.foul_shots(0)
                    self.dartmouth_ball()
            else:
                print("Shot is off the rim.")
                if self.defense/6*random.random() > .5:
                    print(self.opponent + " controls the rebound.")
                    if self.defense == 6:
                        if random.random() > .75:
                            print("Ball stolen. Easy lay up for Dartmouth.")
                            self.add_points(1, 2)
                            self.opponent_ball()
                        else:
                            if random.random() > .5:
                                print("")
                                self.opponent_non_jumpshot()
                            else:
                                print("Pass back to " + self.opponent +
                                      " guard.\n")
                                self.opponent_ball()
                    else:
                        if random.random() > .5:
                            self.opponent_non_jumpshot()
                        else:
                            print("Pass back to " + self.opponent +
                                  " guard.\n")
                            self.opponent_ball()
                else:
                    print("Dartmouth controls the rebound.\n")
                    self.dartmouth_ball()
        else:
            print("Shot is good.")
            self.add_points(0, 2)
            self.dartmouth_ball()

    # simulates opponents lay up or set shot
    def opponent_non_jumpshot(self):
        if self.z1 > 3:
            print("Set shot.")
        else:
            print("Lay up")
        if 7/self.defense*random.random() > .413:
            print("Shot is missed.")
            if self.defense/6*random.random() > .5:
                print(self.opponent + " controls the rebound.")
                if self.defense == 6:
                    if random.random() > .75:
                        print("Ball stolen. Easy lay up for Dartmouth.")
                        self.add_points(1, 2)
                        self.opponent_ball()
                    else:
                        if random.random() > .5:
                            print("")
                            self.opponent_non_jumpshot()
                        else:
                            print("Pass back to " + self.opponent +
                                  " guard.\n")
                            self.opponent_ball()
                else:
                    if random.random() > .5:
                        print("")
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

    # simulates an opponents possesion
    # #randomly picks jump shot or lay up / set shot.
    def opponent_ball(self):
        self.time += 1
        if self.time == 50:
            self.halftime()
        self.z1 = 10/4*random.random()+1
        if self.z1 > 2:
            self.opponent_non_jumpshot()
        else:
            self.opponent_jumpshot()


new_game = Basketball()
