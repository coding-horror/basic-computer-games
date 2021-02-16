######################################################################
#
# Bagels
#
# From: BASIC Computer Games (1978)
#       Edited by David H. Ahl
#
# "In this game, the computer picks a 3-digit secret number using
#  the digits 0 to 9 and you attempt to guess what it is.  You are
#  allowed up to twenty guesses.  No digit is repeated.  After
#  each guess the computer will give you clues about your guess
#  as follows:
#
#  PICO     One digit is correct, but in the wrong place
#  FERMI    One digit is in the correct place
#  BAGELS   No digit is correct
#
# "You will learn to draw inferences from the clues and, with
#  practice, you'll learn to improve your score.  There are several
#  good strategies for playing Bagels.  After you have found a good
#  strategy, see if you can improve it.  Or try a different strategy
#  altogether and see if it is any better.  While the program allows
#  up to twenty guesses, if you use a good strategy it should not
#  take more than eight guesses to get any number.
#
# "The original authors of this program are D. Resek and P. Rowe of
#  the Lawrence Hall of Science, Berkeley, California."
#
#
# Python port by Jeff Jetton, 2019
#
######################################################################


import random

MAX_GUESSES = 20



def print_rules():
    print("\nI am thinking of a three-digit number.  Try to guess")
    print("my number and I will give you clues as follows:")
    print("   PICO   - One digit correct but in the wrong position")
    print("   FERMI  - One digit correct and in the right position")
    print("   BAGELS - No digits correct")



def pick_number():
    # Note that this returns a list of individual digits
    # as separate strings, not a single integer or string
    numbers = [i for i in range(10)]
    random.shuffle(numbers)
    num = numbers[0:3]
    num = [str(i) for i in num] 
    return num



def get_valid_guess():
    valid = False
    while not valid:
        guess = input(f"Guess # {guesses}     ? ")
        guess = guess.strip()
        # Guess must be three characters
        if len(guess) == 3:
            # And they should be numbers
            if guess.isnumeric():
                # And the numbers must be unique
                if len(set(guess)) == 3:
                    valid = True
                else:
                    print("Oh, I forgot to tell you that " +
                          "the number I have in mind")
                    print("has no two digits the same.")
            else:
                print("What?")
        else:
            print("Try guessing a three-digit number.")

    return guess



def build_result_string(num, guess):

    result = ""

    # Correct digits in wrong place
    for i in range(2):
        if num[i] == guess[i+1]:
            result += "PICO "
        if num[i+1] == guess[i]:
            result += "PICO "
    if num[0] == guess[2]:
        result += "PICO "
    if num[2] == guess[0]:
        result += "PICO "

    # Correct digits in right place
    for i in range(3):
        if num[i] == guess[i]:
            result += "FERMI "

    # Nothing right?
    if result == "":
        result = "BAGELS"

    return result




######################################################################


# Intro text
print("\n                Bagels")
print("Creative Computing  Morristown, New Jersey")
print("\n\n")

# Anything other than N* will show the rules
response = input("Would you like the rules (Yes or No)? ")
if len(response) > 0:
    if response.upper()[0] != 'N':
        print_rules()
else:
    print_rules()

games_won = 0
still_running = True
while still_running:

    # New round
    num = pick_number()
    num_str = ''.join(num)
    guesses = 1

    print("\nO.K.  I have a number in mind.")
    guessing = True
    while guessing:

        guess = get_valid_guess()

        if guess == num_str:
            print("You got it!!!\n")
            games_won += 1
            guessing = False
        else: 
            print(build_result_string(num, guess))
            guesses += 1
            if guesses > MAX_GUESSES:
                print("Oh well")
                print(f"That's {MAX_GUESSES} guesses.  " +
                      "My number was " + num_str)
                guessing = False   


    valid_response = False
    while not valid_response:
        response = input("Play again (Yes or No)? ")
        if len(response) > 0:
            valid_response = True
            if response.upper()[0] != "Y":
                still_running = False


if games_won > 0:
    print(f"\nA {games_won} point Bagels buff!!")

print("Hope you had fun.  Bye.\n")



######################################################################
#
# Porting Notes
#
#   The original program did an unusually good job of validating the
#   player's input (compared to many of the other programs in the
#   book). Those checks and responses have been exactly reproduced.
#   
#
# Ideas for Modifications
#
#   It should probably mention that there's a maximum of MAX_NUM
#   guesses in the instructions somewhere, shouldn't it?
#
#   Could this program be written to use anywhere from, say 2 to 6
#   digits in the number to guess? How would this change the routine
#   that creates the "result" string?
#
######################################################################


            
        
    

    
