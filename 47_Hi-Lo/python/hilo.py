#!/usr/bin/env python3
import random

MAX_ATTEMPTS = 6
QUESTION_PROMPT = "? "


def play():
    print("HI LO")
    print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")
    print("THIS IS THE GAME OF HI LO.\n")
    print("YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE")
    print("HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU")
    print("GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!")
    print("THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,")
    print("IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.\n\n")

    total_winnings = 0
    while True:
        print()
        secret = random.randint(1, 100)
        guessed_correctly = False

        for _attempt in range(MAX_ATTEMPTS):
            print("YOUR GUESS", end=QUESTION_PROMPT)
            guess = int(input())

            if guess == secret:
                print(f"GOT IT!!!!!!!!!!   YOU WIN {secret} DOLLARS.")
                guessed_correctly = True
                break
            elif guess > secret:
                print("YOUR GUESS IS TOO HIGH.")
            else:
                print("YOUR GUESS IS TOO LOW.")

        if guessed_correctly:
            total_winnings += secret
            print(f"YOUR TOTAL WINNINGS ARE NOW {total_winnings} DOLLARS.")
        else:
            print(f"YOU BLEW IT...TOO BAD...THE NUMBER WAS {secret}")

        print("\n")
        print("PLAY AGAIN (YES OR NO)", end=QUESTION_PROMPT)
        answer = input().upper()
        if answer != "YES":
            break

    print("\nSO LONG.  HOPE YOU ENJOYED YOURSELF!!!")


if __name__ == "__main__":
    play()
