import random


def simulateRoll(pins):
    for _ in range(20):
        x = random.randint(0, 14)
        if x < len(pins):
            pins[x] = 1


def calculate_score(rolls):
    score = 0
    frame = 1
    b = 1
    for index, pins in enumerate(rolls):
        score += pins
        if b == 1:
            if pins == 10:  # strike
                score += sum(rolls[index + 1:index + 3])
                frame += 1
            else:
                b = 2
        else:
            if sum(rolls[index - 1:index + 1]) == 10:  # spare
                score += rolls[index + 1]
            b = 1
            frame += 1
        if frame > 10:
            break

    return score


class Player:
    def __init__(self, name):
        self.name = name
        self.rolls = []

    def play_frame(self, frame):
        extra = 0
        prev_score = 0
        pins = [0] * 10  # reset the pins
        for ball in range(2):
            simulateRoll(pins)
            score = sum(pins)
            self.show(pins)
            pin_count = score - prev_score
            self.rolls.append(pin_count)  # log the number of pins toppled this roll
            print(f'{pin_count} for {self.name}')
            if score - prev_score == 0:
                print("GUTTER!!!")
            if ball == 0:
                if score == 10:
                    print("STRIKE!!!")
                    extra = 2
                    break  # cannot roll more than once in a frame
                else:
                    print(f"next roll {self.name}")
            else:
                if score == 10:
                    print("SPARE!")
                    extra = 1

            prev_score = score  # remember previous pins to distinguish ...
        if frame == 9 and extra > 0:
            print(f'Extra rolls for {self.name}')
            pins = [0] * 10  # reset the pins
            score = 0
            for ball in range(extra):
                if score == 10:
                    pins = [0] * 10
                simulateRoll(pins)
                score = sum(pins)
                self.rolls.append(score)

    def __str__(self):
        return f'{self.name}: {self.rolls}, total:{calculate_score(self.rolls)}'

    def show(self, pins):
        pins_iter = iter(pins)
        print()
        for row in range(4):
            print(' ' * row, end='')
            for _ in range(4 - row):
                p = next(pins_iter)
                print('O ' if p else '+ ', end='')
            print()


def centreText(text, width):
    t = len(text)
    return (' ' * ((width - t) // 2)) + text


def main():
    print(centreText('Bowl', 80))
    print(centreText('CREATIVE COMPUTING MORRISTOWN, NEW JERSEY', 80))
    print()
    print('WELCOME TO THE ALLEY.')
    print('BRING YOUR FRIENDS.')
    print("OKAY LET'S FIRST GET ACQUAINTED.")

    while True:
        print()
        if input('THE INSTRUCTIONS (Y/N)? ') in 'yY':
            print('THE GAME OF BOWLING TAKES MIND AND SKILL. DURING THE GAME')
            print('THE COMPUTER WILL KEEP SCORE. YOU MAY COMPETE WITH')
            print('OTHER PLAYERS[UP TO FOUR]. YOU WILL BE PLAYING TEN FRAMES.')
            print("ON THE PIN DIAGRAM 'O' MEANS THE PIN IS DOWN...'+' MEANS THE")
            print('PIN IS STANDING. AFTER THE GAME THE COMPUTER WILL SHOW YOUR')
            print('SCORES.')

        total_players = int(input('FIRST OF ALL...HOW MANY ARE PLAYING? '))
        player_names = []
        print()
        print('VERY GOOD...')
        for index in range(total_players):
            player_names.append(Player(input(f'Enter name for player {index + 1}: ')))

        for frame in range(10):
            for player in player_names:
                player.play_frame(frame)

        for player in player_names:
            print(player)

        if input('DO YOU WANT ANOTHER GAME? ') not in 'yY':
            break


if __name__ == '__main__':
    main()


############################################################################################
#
# This is a fairly straight conversion to python with some exceptions.
# I have kept most of the upper case text that the program prints.
# I have added the feature of giving names to players.
# I have added a Player class to store player data in.
# This last change works around the problems in the original storing data in a matrix.
# The original had bugs in calculating indexes which meant that the program
# would overwrite data in the matrix, so the results printed out contained errors.
# The last change is to do with the strict rules which allow extra rolls if the player
# scores a spare or strike in the last frame.
# This program allows these extra rolls and also calculates the proper score.
#
############################################################################################
