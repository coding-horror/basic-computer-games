import random, sys



def main():

    global colors, color_letters, num_positions, num_colors, human_score, computer_score
    colors = ["BLACK", "WHITE", "RED", "GREEN", "ORANGE", "YELLOW", "PURPLE", "TAN"]
    color_letters = "BWRGOYPT"

    num_colors = 100
    human_score = 0
    computer_score = 0

    # get user inputs for game conditions
    print("Mastermind")
    print('Creative Computing Morristown, New Jersey')
    while num_colors > 8:
        num_colors = int(input("Number of colors (max 8): ")) # C9 in BASIC
    num_positions = int(input("Number of positions: ")) # P9 in BASIC
    num_rounds = int(input("Number of rounds: ")) # R9 in BASIC
    possibilities = num_colors**num_positions
    all_possibilities = [1] * possibilities

    print("Number of possibilities {}".format(possibilities))
    print('Color\tLetter')
    print('=====\t======')
    for element in range(0, num_colors):
        print("{}\t{}".format(colors[element], colors[element][0]))

    current_round = 1

    while current_round <= num_rounds:
        print('Round number {}'.format(current_round))
        num_moves = 1
        guesses = []
        turn_over = False
        print('Guess my combination ...')
        answer = int(possibilities * random.random()) 
        numeric_answer = [-1] * num_positions
        for i in range(0, answer):
            numeric_answer = get_possibility(numeric_answer)
        #human_readable_answer = make_human_readable(numeric_answer)
        while (num_moves < 10 and not turn_over ):
            print('Move # {} Guess : '.format(num_moves))
            user_command = input('Guess ')
            if user_command == "BOARD":
                print_board(guesses) #2000
            elif user_command == "QUIT": #2500
                human_readable_answer = make_human_readable(numeric_answer)
                print('QUITTER! MY COMBINATION WAS: {}'.format(human_readable_answer))
                print('GOOD BYE')
                quit()
            elif len(user_command) != num_positions: #410
                print("BAD NUMBER OF POSITIONS")
            else:
                invalid_letters = get_invalid_letters(user_command)
                if invalid_letters > "":
                    print("INVALID GUESS: {}".format(invalid_letters))
                else:
                    guess_results = compare_two_positions(user_command, make_human_readable(numeric_answer))
                    print("Results: {}".format(guess_results))
                    if guess_results[1] == num_positions: # correct guess
                        turn_over = True
                        print("You guessed it in {} moves!".format(num_moves))
                        human_score = human_score + num_moves
                        print_score()
                    else:
                        print("You have {} blacks and {} whites".format(guess_results[1], guess_results[2]))
                        num_moves = num_moves + 1
                        guesses.append(guess_results)
        if not turn_over: # RAN OUT OF MOVES
            print ("YOU RAN OUT OF MOVES! THAT'S ALL YOU GET!")
            print("THE ACTUAL COMBINATION WAS: {}".format(make_human_readable(numeric_answer)))
            human_score = human_score + num_moves
            print_score()

        # COMPUTER TURN
        guesses = []
        turn_over = False
        inconsistent_information = False
        while(not turn_over  and not inconsistent_information ):
            all_possibilities = [1] * possibilities
            num_moves = 1
            inconsistent_information = False
            print ("NOW I GUESS. THINK OF A COMBINATION.")
            player_ready = input("HIT RETURN WHEN READY: ")
            while (num_moves < 10 and not turn_over  and not inconsistent_information):
                found_guess = False
                computer_guess = int(possibilities * random.random())
                if all_possibilities[computer_guess] == 1: # random guess is possible, use it
                    found_guess = True
                    guess = computer_guess
                else:
                    for i in range (computer_guess, possibilities):
                        if all_possibilities[i] == 1:
                            found_guess = True
                            guess = i
                            break
                    if not found_guess:
                        for i in range (0, computer_guess):
                            if all_possibilities[i] == 1:
                                found_guess = True
                                guess = i
                                break
                if not found_guess: # inconsistent info from user
                    print('YOU HAVE GIVEN ME INCONSISTENT INFORMATION.')
                    print('TRY AGAIN, AND THIS TIME PLEASE BE MORE CAREFUL.')
                    turn_over = True
                    inconsistent_information = True
                else:
                    numeric_guess = [-1] * num_positions
                    for i in range(0, guess):
                        numeric_guess = get_possibility(numeric_guess)
                    human_readable_guess = make_human_readable(numeric_guess)
                    print('My guess is: {}'.format(human_readable_guess))
                    blacks, whites = input("ENTER BLACKS, WHITES (e.g. 1,2): ").split(",")
                    blacks = int(blacks)
                    whites = int(whites)
                    if blacks == num_positions: #Correct guess
                        print('I GOT IT IN {} MOVES'.format(num_moves))
                        turn_over = True
                        computer_score = computer_score + num_moves
                        print_score()
                    else:
                        num_moves += 1
                        for i in range (0, possibilities):   
                            if all_possibilities[i] == 0: #already ruled out
                                continue
                            numeric_possibility = [-1] * num_positions
                            for j in range (0, i):
                                numeric_possibility = get_possibility(numeric_possibility)
                            human_readable_possibility = make_human_readable(numeric_possibility) #4000
                            comparison = compare_two_positions(human_readable_possibility, human_readable_guess)
                            print(comparison)
                            if ((blacks != comparison[1]) or (whites != comparison[2])):
                                all_possibilities[i] = 0
            if not turn_over: # COMPUTER DID NOT GUESS
                print("I USED UP ALL MY MOVES!")
                print("I GUESS MY CPU IS JUST HAVING AN OFF DAY.")
                computer_score = computer_score + num_moves
                print_score()
        current_round += 1
    print_score(is_final_score=True)
    sys.exit()

#470
def get_invalid_letters(user_command):
    """Makes sure player input consists of valid colors for selected game configuration."""
    valid_colors = color_letters[:num_colors]
    invalid_letters = ""
    for letter in user_command:
        if letter not in valid_colors:
            invalid_letters = invalid_letters + letter
    return invalid_letters

#2000
def print_board(guesses):
    """Prints previous guesses within the round."""
    print("Board")
    print("Move\tGuess\tBlack White")
    for idx, guess in enumerate(guesses):
        print('{}\t{}\t{}     {}'.format(idx+1, guess[0], guess[1], guess[2]))

#3500
# Easily the place for most optimization, since they generate every possibility 
# every time when checking for potential solutions
# From the original article: 
#    "We did try a version that kept an actual list of all possible combinations
#    (as a string array), which was significantly faster than this versionn but
#    which ate tremendous amounts of memory."
def get_possibility(possibility):
    #print(possibility)
    if possibility[0] > -1: #3530
        current_position = 0 # Python arrays are zero-indexed
        while True:
            if possibility[current_position] < num_colors-1: # zero-index again
                possibility[current_position] += 1
                return possibility
            else:
                possibility[current_position] = 0
                current_position += 1
    else: #3524
        possibility = [0] * num_positions
    return possibility

#4500
def compare_two_positions(guess, answer):
    """Returns blacks (correct color and position) and whites (correct color only) for candidate position (guess) versus reference position (answer)."""
    increment = 0
    blacks = 0
    whites = 0
    initial_guess = guess
    for pos in range(0, num_positions):
        if guess[pos] != answer[pos]:
            for pos2 in range(0, num_positions):
                if not(guess[pos] != answer[pos2] or guess[pos2] == answer[pos2]): # correct color but not correct place
                    whites = whites + 1
                    answer = answer[:pos2] + chr(increment) + answer[pos2+1:]
                    guess = guess[:pos] + chr(increment+1) + guess[pos+1:]
                    increment = increment + 2
        else: #correct color and placement
            blacks = blacks + 1
            # THIS IS DEVIOUSLY CLEVER
            guess = guess[:pos] + chr(increment+1) + guess[pos+1:]
            answer = answer[:pos] + chr(increment) + answer[pos+1:]
            increment = increment + 2
    return [initial_guess, blacks, whites]
 
#5000 + logic from 1160
def print_score(is_final_score=False):
    """Prints score after each turn ends, including final score at end of game."""
    if is_final_score:
        print("GAME OVER")
        print("FINAL SCORE:")
    else:
        print("SCORE:")
    print("     COMPUTER {}".format(computer_score))
    print("     HUMAN    {}".format(human_score))

#4000, 5500, 6000 subroutines are all identical
def make_human_readable(num):
    """Make the numeric representation of a position human readable."""
    retval = ''
    for i in range(0, len(num)):
        retval = retval + color_letters[int(num[i])]
    return retval

if __name__ == "__main__":
    main()
