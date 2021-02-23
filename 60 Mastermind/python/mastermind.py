import random

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
    while (num_colors > 8):
        num_colors = int(input("Number of colors (max 8): ")) # C9 in BASIC
    # TODO no more than 8 colors
    num_positions = int(input("Number of positions: ")) # P9 in BASIC
    num_rounds = int(input("Number of rounds: ")) # R9 in BASIC
    possibilities = num_colors**num_positions
    all_possibilities = [1] * possibilities
    
    print("Number of possibilities {}".format(possibilities))
    #TODO tab fixed formatting
    print('Color     Letter')
    print('=====     ======')
    for element in range(0, num_colors):
        print("{}     {}".format(colors[element], colors[element][0]))
    
    num_moves = 1
    guesses = []
    current_round = 1
    
    while current_round <= num_rounds:
        print('Round number {}'.format(current_round))
        num_moves = 1
        turn_over = False
        print('Guess my combination ...')
        answer = int(possibilities * random.random()) # A in BASIC
        numeric_answer = init_possibility()
        for i in range(0, answer):
            numeric_answer = get_possibility(numeric_answer)
        #debug
        print("{} - {}".format(numeric_answer, make_human_readable(numeric_answer)))
        #human_readable_answer = make_human_readable(numeric_answer)
        while (num_moves < 10 and not(turn_over)):
            print('Move # {} Guess : '.format(num_moves))
            user_command = input('Guess ')
            if user_command == "BOARD":
                print_board(guesses) #2000
            elif user_command == "QUIT":
                quit_game(numeric_answer) #2500
                quit()
            elif len(user_command) != num_positions: #410
                print("BAD NUMBER OF POSITIONS")
            else:
                invalid_letters = get_invalid_letters(user_command)
                if (invalid_letters > ""):
                    print("INVALID GUESS: {}".format(invalid_letters))
                else:
                    guess_results = compare_two_positions(user_command, make_human_readable(numeric_answer))
                    print("Results: {}".format(guess_results))
                    if (guess_results[1] == num_positions): # correct guess
                        turn_over = True
                        print("You guessed it in {} moves!".format(num_moves))
                        human_score = human_score + num_moves
                        print_score()
                    else:
                        print("You have {} blacks and {} whites".format(guess_results[1], guess_results[2]))
                        num_moves = num_moves + 1
                        guesses.append(guess_results)
        if (not(turn_over)): # RAN OUT OF MOVES
            print ("YOU RAN OUT OF MOVES! THAT'S ALL YOU GET!")
            print("THE ACTUAL COMBINATION WAS: {}".format(make_human_readable(numeric_answer)))
            human_score = human_score + num_moves
            print_score()
       
        # COMPUTER TURN
        turn_over = False
        all_possibilities = [1] * possibilities
        num_moves = 1
        print ("NOW I GUESS. THINK OF A COMBINATION.")
        player_ready = input("HIT RETURN WHEN READY: ")
        while (num_moves < 10 and not(turn_over)):
            foundGuess = False
            computer_guess = int(possibilities * random.random())
            if (all_possibilities[computer_guess] == 1): # random guess is possible, use it
                foundGuess = True
                guess = computer_guess
            else:
                for i in range (computer_guess, possibilities):
                    if (all_possibilities[i] == 1):
                        foundGuess = True
                        guess = i
                        break
                if (not(foundGuess)):
                    for i in range (0, computer_guess):
                        if (all_possibilities[i] == 1):
                            foundGuess = True
                            guess = i
                            break
            if (not(foundGuess)): # inconsistent info from user
                print('YOU HAVE GIVEN ME INCONSISTENT INFORMATION.')
                print('TRY AGAIN, AND THIS TIME PLEASE BE MORE CAREFUL.')
                quit()
                # TODO start computer turn over
            else:
                numeric_guess = init_possibility()
                for i in range(0, guess):
                    numeric_guess = get_possibility(numeric_guess)
                human_readable_guess = make_human_readable(numeric_guess)
                print('My guess is: {}'.format(human_readable_guess))
                blacks, whites = input("Enter blacks, whites (e.g. 1,2): ").split(",")
                blacks = int(blacks)
                whites = int(whites)
                if (blacks == num_positions): #Correct guess
                    print('I GOT IT IN {} moves'.format(num_moves))
                    turn_over = True
                    computer_score = computer_score + num_moves
                    print_score()
                else:
                    num_moves += 1
                    for i in range (0, possibilities):   
                        if(all_possibilities[i] == 0): #already ruled out
                            continue
                        numeric_possibility = init_possibility()
                        for j in range (0, i):
                            numeric_possibility = get_possibility(numeric_possibility)
                        human_readable_possibility = make_human_readable(numeric_possibility) #4000
                        comparison = compare_two_positions(human_readable_possibility, human_readable_guess)
                        print("{} {} {}".format(human_readable_guess, human_readable_possibility, comparison))
                        if ((blacks != comparison[1]) or (whites != comparison[2])):
                            all_possibilities[i] = 0
        if (not(turn_over)): # COMPUTER DID NOT GUESS
            print("I USED UP ALL MY MOVES!")
            print("I GUESS MY CPU IS JUST HAVING AN OFF DAY.")
            computer_score = computer_score + num_moves
            print_score()
        current_round += 1

def get_invalid_letters(user_command):
    #TODO
    return ""

def validate_human_guess(user_input):
   
    guess_results = compare_two_positions(user_input, answer)
    return guess_results

#2000
def print_board(guesses):
    print("Board")
    print("Move Guess Black White")
    for guess in guesses:
        print('{} {} {} {}'.format(guess[0], guess[0], guess[1], guess[2]))

#2500
def quit_game(numeric_answer):
    human_readable_answer = make_human_readable(numeric_answer)
    print('QUITTER! MY COMBINATION WAS: {}'.format(human_readable_answer))
    print('GOOD BYE')

#3000
def init_possibility():
    possibility = [0] * num_positions
    return possibility

#3500
def get_possibility(possibility):
    if possibility[0] > 0: #3530
            current_position = 0 # Python arrays are zero-indexed
            while (True):
                if possibility[current_position] < num_colors:
                    possibility[current_position] += 1
                    return possibility
                else:
                    possibility[current_position] = 1
                    current_position += 1
    else: #3524
        possibility = [1] * num_positions
    return possibility

#4000
def convert_q_to_a():
    return map_num_to_vals(q, color_letters)

#4500
def compare_two_positions(guess, answer):
    f = 0
    b = 0
    w = 0
    initial_guess = guess
    for pos in range(0, num_positions):
        if (guess[pos] != answer[pos]):
            for pos2 in range(0, num_positions):
                if not(guess[pos] != answer[pos2] or guess[pos2] == answer[pos2]): # correct color but not correct place
                    w = w + 1
                    answer = answer[:pos2] + chr(f) + answer[pos2+1:]
                    guess = guess[:pos] + chr(f+1) + guess[pos+1:]
                    f = f + 2
        else: #correct color and placement
            b = b + 1
            # THIS IS DEVIOUSLY CLEVER
            guess = guess[:pos] + chr(f+1) + guess[pos+1:]
            answer = answer[:pos] + chr(f) + answer[pos+1:]
            f = f + 2
    return [initial_guess, b, w]
    
    



#5000
def print_score(is_final_score=False):
    if (is_final_score):
        print("GAME OVER")
        print("FINAL SCORE:")
    else:
        print("SCORE:")
    print("     COMPUTER {}".format(computer_score))
    print("     HUMAN    {}".format(human_score))

#5500
def convert_q_to_g():
    return map_num_to_vals(q, g)

#6000
def convert_q_to_h():
    return map_num_to_vals(q, h)

#6500
def copy_g_to_h():
        g = h

def make_human_readable(num):
    retval = ''
    for z in range(0, len(num)):
        retval = retval + color_letters[int(num[z])]
    return retval


def map_num_to_vals(num, v):
    retval = ''
    print(len(num))
    for z in range(0, len(num)):
        print(num[z])
        print(v[int(num[z])])
        retval = retval + v[int(num[z])]
    return retval

if __name__ == "__main__":
    main()