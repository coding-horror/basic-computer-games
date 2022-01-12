#!/usr/bin/env python3
"""
Play the Acey-Ducey game
https://www.atariarchives.org/basicgames/showpage.php?page=2
"""

import random

cards = {
    1: "1",
    2: "2",
    3: "3",
    4: "4",
    5: "5",
    6: "6",
    7: "7",
    8: "8",
    9: "9",
    10: "Jack",
    11: "Queen",
    12: "King",
    13: "Ace",
}


def play_game():
    """Play the game"""
    cash = 100
    while cash > 0:
        print(f"You now have {cash} dollars\n")
        print("Here are you next two cards")
        round_cards = list(cards.keys())
        random.shuffle(round_cards)
        card_a, card_b, card_c = round_cards.pop(), round_cards.pop(), round_cards.pop()
        if card_a > card_b:
            card_a, card_b = card_b, card_a
        print(f" {cards[card_a]}")
        print(f" {cards[card_b]}\n")
        while True:
            try:
                bet = int(input("What is your bet? "))
                if bet < 0:
                    raise ValueError("Bet must be more than zero")
                if bet == 0:
                    print("CHICKEN!!\n")
                if bet > cash:
                    print("Sorry, my friend but you bet too much")
                    print(f"You only have {cash} dollars to bet")
                    continue
                cash -= bet
                break

            except ValueError:
                print("Please enter a positive number")
        print(f" {cards[card_c]}")
        if bet > 0:
            if card_a < card_c < card_b:
                print("You win!!!")
                cash += bet * 2
            else:
                print("Sorry, you lose")

    print("Sorry, friend, but you blew your wad")


def main():
    """Main"""
    keep_playing = True

    while keep_playing:
        play_game()
        keep_playing = input("Try again? (yes or no) ").lower().startswith("y")
    print("Ok hope you had fun")


if __name__ == "__main__":
    print(
        """
Acey-Ducey is played in the following manner
The dealer (computer) deals two cards face up
You have an option to bet or not bet depending
on whether or not you feel the card will have
a value between the first two.
If you do not want to bet, input a 0
  """
    )
    main()
