"""
LETTER

A letter guessing game.

Ported by Dave LeCompte
"""

import random

# The original code printed character 7, the "BELL" character 15 times
# when the player won. Many modern systems do not support this, and in
# any case, it can quickly become annoying, so it is disabled here.

BELLS_ON_SUCCESS = False

def print_with_tab(space_count, msg):
    if space_count > 0:
        spaces = " " * space_count
    else:
        spaces = ""

    print(spaces + msg)

def print_instructions():
    print("LETTER GUESSING GAME")
    print()
    print("I'LL THINK OF A LETTER OF THE ALPHABET, A TO Z.")
    print("TRY TO GUESS MY LETTER AND I'LL GIVE YOU CLUES")
    print("AS TO HOW CLOSE YOU'RE GETTING TO MY LETTER.")

def play_game():
    target_value = random.randint(ord('A'), ord('Z'))
    num_guesses = 0
    print()
    print("O.K., I HAVE A LETTER.  START GUESSING.")
    print()
    while True:
      print("WHAT IS YOUR GUESS?")
      num_guesses += 1
      guess = ord(input())
      print()
      if guess == target_value:
          print()
          print(f"YOU GOT IT IN {num_guesses} GUESSES!!")
          if num_guesses > 5:
              print("BUT IT SHOULDN'T TAKE MORE THAN 5 GUESSES!")
              # goto 515
          print("GOOD JOB !!!!!")
          
          if BELLS_ON_SUCCESS:
            bellStr = chr(7) * 15
            print(bellStr)
            
          print()
          print("LET'S PLAY AGAIN.....")
          return
      elif guess > target_value:
          print("TOO HIGH. TRY A LOWER LETTER.")
          continue
      else:
          print("TOO LOW. TRY A HIGHER LETTER.")
          continue

def main():
    print_with_tab(33, "LETTER")
    print_with_tab(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()

    print_instructions()

    while True:
        play_game()

if __name__ == "__main__":
    main()
