import random
from typing import Tuple


class NIM:
    def __init__(self) -> None:
        self.piles = {1: 7, 2: 5, 3: 3, 4: 1}

    def remove_pegs(self, command) -> None:
        try:

            pile, num = command.split(",")
            num = int(num)
            pile = int(pile)

        except Exception as e:

            if "not enough values" in str(e):
                print(
                    '\nNot a valid command. Your command should be in the form of "1,3", Try Again\n'
                )

            else:
                print("\nError, Try again\n")
            return None

        if self._command_integrity(num, pile):
            self.piles[pile] -= num
        else:
            print("\nInvalid value of either Peg or Pile\n")

    def get_ai_move(self) -> Tuple[int, int]:
        possible_pile = []
        for k, v in self.piles.items():
            if v != 0:
                possible_pile.append(k)

        pile = random.choice(possible_pile)

        num = random.randint(1, self.piles[pile])

        return pile, num

    def _command_integrity(self, num, pile) -> bool:
        return pile <= 4 and pile >= 1 and num <= self.piles[pile]

    def print_pegs(self) -> None:
        for pile, peg in self.piles.items():
            print("Pile {} : {}".format(pile, "O " * peg))

    def help(self) -> None:
        print("-" * 10)
        print('\nThe Game is player with a number of Piles of Objects("O" == one peg)')
        print("\nThe Piles are arranged as given below(Tradional NIM)\n")
        self.print_pegs()
        print(
            '\nAny Number of of Objects are removed one pile by "YOU" and the machine alternatively'
        )
        print("\nOn your turn, you may take all the objects that remain in any pile")
        print("but you must take ATLEAST one object")
        print("\nAnd you may take objects from only one pile on a single turn.")
        print("\nThe winner is defined as the one that picks the last remaning object")
        print("-" * 10)

    def check_for_win(self) -> bool:
        sum = 0
        for v in self.piles.values():
            sum += v

        return sum == 0


def main() -> None:
    # Game initialization
    game = NIM()

    print("Hello, This is a game of NIM")
    help = input("Do You Need Instruction (YES or NO): ")

    if help.lower() == "yes":
        game.help()

    # Start game loop
    input("\nPress Enter to start the Game:\n")
    end = False
    while True:
        game.print_pegs()

        # Players Move
        command = input("\nYOUR MOVE - Number of PILE, Number of Object? ")
        game.remove_pegs(command)
        end = game.check_for_win()
        if end:
            print("\nPlayer Wins the Game, Congratulations!!")
            input("\nPress any key to exit")
            break

        # Computers Move
        ai_command = game.get_ai_move()
        print(
            "\nA.I MOVE - A.I Removed {} pegs from Pile {}".format(
                ai_command[1], ai_command[0]
            )
        )
        game.remove_pegs(str(ai_command[0]) + "," + str(ai_command[1]))
        end = game.check_for_win()
        if end:
            print("\nComputer Wins the Game, Better Luck Next Time\n")
            input("Press any key to exit")
            break


if __name__ == "__main__":
    main()
