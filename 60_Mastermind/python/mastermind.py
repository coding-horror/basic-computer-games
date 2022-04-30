import random
import sys
from typing import List, Union, Tuple


#  define some parameters for the game which should not be modified.
def setup_game() -> Tuple[int, int, int, int]:
    print("""
                                  MASTERMIND
                   CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



    """)
    # get user inputs for game conditions
    num_colors: int = len(COLOR_LETTERS) + 1
    while num_colors > len(COLOR_LETTERS):
        num_colors = int(input("Number of colors (max 8): "))  # C9 in BASIC
    num_positions = int(input("Number of positions: "))  # P9 in BASIC
    num_rounds = int(input("Number of rounds: "))  # R9 in BASIC
    possibilities = num_colors**num_positions

    print(f"Number of possibilities {possibilities}")
    print("Color\tLetter")
    print("=====\t======")
    for element in range(0, num_colors):
        print(f"{COLORS[element]}\t{COLORS[element][0]}")
    return num_colors, num_positions, num_rounds, possibilities


# Global variables
COLORS = ["BLACK", "WHITE", "RED", "GREEN", "ORANGE", "YELLOW", "PURPLE", "TAN"]
COLOR_LETTERS = "BWRGOYPT"
NUM_COLORS, NUM_POSITIONS, NUM_ROUNDS, POSSIBILITIES = setup_game()
human_score = 0
computer_score = 0


def main() -> None:
    current_round = 1
    while current_round <= NUM_ROUNDS:
        print(f"Round number {current_round}")
        human_turn()
        computer_turn()
        current_round += 1
    print_score(is_final_score=True)
    sys.exit()


def human_turn() -> None:
    global human_score
    num_moves = 1
    guesses: List[List[Union[str, int]]] = []
    print("Guess my combination ...")
    secret_combination = int(POSSIBILITIES * random.random())
    answer = possibility_to_color_code(secret_combination)
    while True:
        print(f"Move # {num_moves} Guess : ")
        user_command = input("Guess ")
        if user_command == "BOARD":
            print_board(guesses)  # 2000
        elif user_command == "QUIT":  # 2500
            print(f"QUITTER! MY COMBINATION WAS: {answer}")
            print("GOOD BYE")
            quit()
        elif len(user_command) != NUM_POSITIONS:  # 410
            print("BAD NUMBER OF POSITIONS")
        else:
            invalid_letters = get_invalid_letters(user_command)
            if invalid_letters > "":
                print(f"INVALID GUESS: {invalid_letters}")
            else:
                guess_results = compare_two_positions(user_command, answer)
                if guess_results[1] == NUM_POSITIONS:  # correct guess
                    print(f"You guessed it in {num_moves} moves!")
                    human_score = human_score + num_moves
                    print_score()
                    return  # from human turn, triumphant
                else:
                    print(
                        "You have {} blacks and {} whites".format(
                            guess_results[1], guess_results[2]
                        )
                    )
                    guesses.append(guess_results)
                    num_moves += 1

        if num_moves > 10:  # RAN OUT OF MOVES
            print("YOU RAN OUT OF MOVES! THAT'S ALL YOU GET!")
            print(f"THE ACTUAL COMBINATION WAS: {answer}")
            human_score = human_score + num_moves
            print_score()
            return  # from human turn, defeated


def computer_turn() -> None:
    global computer_score
    while True:
        all_possibilities = [1] * POSSIBILITIES
        num_moves = 1
        print("NOW I GUESS. THINK OF A COMBINATION.")
        input("HIT RETURN WHEN READY: ")
        while True:
            possible_guess = find_first_solution_of(all_possibilities)
            if possible_guess < 0:  # no solutions left :(
                print("YOU HAVE GIVEN ME INCONSISTENT INFORMATION.")
                print("TRY AGAIN, AND THIS TIME PLEASE BE MORE CAREFUL.")
                break  # out of inner while loop, restart computer turn

            computer_guess = possibility_to_color_code(possible_guess)
            print(f"My guess is: {computer_guess}")
            blacks_str, whites_str = input(
                "ENTER BLACKS, WHITES (e.g. 1,2): "
            ).split(",")
            blacks = int(blacks_str)
            whites = int(whites_str)
            if blacks == NUM_POSITIONS:  # Correct guess
                print(f"I GOT IT IN {num_moves} MOVES")
                computer_score = computer_score + num_moves
                print_score()
                return  # from computer turn

            # computer guessed wrong, deduce which solutions to eliminate.
            for i in range(0, POSSIBILITIES):
                if all_possibilities[i] == 0:  # already ruled out
                    continue
                possible_answer = possibility_to_color_code(i)
                comparison = compare_two_positions(
                    possible_answer, computer_guess
                )
                if (blacks != comparison[1]) or (whites != comparison[2]):
                    all_possibilities[i] = 0

            if num_moves == 10:
                print("I USED UP ALL MY MOVES!")
                print("I GUESS MY CPU IS JUST HAVING AN OFF DAY.")
                computer_score = computer_score + num_moves
                print_score()
                return  # from computer turn, defeated.
            num_moves += 1


def find_first_solution_of(all_possibilities: List[int]) -> int:
    """Scan through all_possibilities for first remaining non-zero marker,
    starting from some random position and wrapping around if needed.
    If not found return -1."""
    start = int(POSSIBILITIES * random.random())
    for i in range(0, POSSIBILITIES):
        solution = (i + start) % POSSIBILITIES
        if all_possibilities[solution]:
            return solution
    return -1


# 470
def get_invalid_letters(user_command) -> str:
    """Makes sure player input consists of valid colors for selected game configuration."""
    valid_colors = COLOR_LETTERS[:NUM_COLORS]
    invalid_letters = ""
    for letter in user_command:
        if letter not in valid_colors:
            invalid_letters = invalid_letters + letter
    return invalid_letters


# 2000
def print_board(guesses) -> None:
    """Print previous guesses within the round."""
    print("Board")
    print("Move\tGuess\tBlack White")
    for idx, guess in enumerate(guesses):
        print(f"{idx + 1}\t{guess[0]}\t{guess[1]}     {guess[2]}")


def possibility_to_color_code(possibility: int) -> str:
    """Accepts a (decimal) number representing one permutation in the realm of
    possible secret codes and returns the color code mapped to that permutation.
    This algorithm is essentially converting a decimal  number to a number with
    a base of #num_colors, where each color code letter represents a digit in
    that #num_colors base."""
    color_code: str = ""
    pos: int = NUM_COLORS ** NUM_POSITIONS  # start with total possibilities
    remainder = possibility
    for _ in range(NUM_POSITIONS - 1, 0, -1):  # process all but the last digit
        pos = pos // NUM_COLORS
        color_code += COLOR_LETTERS[remainder // pos]
        remainder = remainder % pos
    color_code += COLOR_LETTERS[remainder]  # last digit is what remains
    return color_code


# 4500
def compare_two_positions(guess: str, answer: str) -> List[Union[str, int]]:
    """Returns blacks (correct color and position) and whites (correct color
    only) for candidate position (guess) versus reference position (answer)."""
    increment = 0
    blacks = 0
    whites = 0
    initial_guess = guess
    for pos in range(0, NUM_POSITIONS):
        if guess[pos] != answer[pos]:
            for pos2 in range(0, NUM_POSITIONS):
                if not (
                    guess[pos] != answer[pos2] or guess[pos2] == answer[pos2]
                ):  # correct color but not correct place
                    whites = whites + 1
                    answer = answer[:pos2] + chr(increment) + answer[pos2 + 1:]
                    guess = guess[:pos] + chr(increment + 1) + guess[pos + 1:]
                    increment = increment + 2
        else:  # correct color and placement
            blacks = blacks + 1
            # THIS IS DEVIOUSLY CLEVER
            guess = guess[:pos] + chr(increment + 1) + guess[pos + 1:]
            answer = answer[:pos] + chr(increment) + answer[pos + 1:]
            increment = increment + 2
    return [initial_guess, blacks, whites]


# 5000 + logic from 1160
def print_score(is_final_score: bool = False) -> None:
    """Print score after each turn ends, including final score at end of game."""
    if is_final_score:
        print("GAME OVER")
        print("FINAL SCORE:")
    else:
        print("SCORE:")
    print(f"     COMPUTER {computer_score}")
    print(f"     HUMAN    {human_score}")


if __name__ == "__main__":
    main()
