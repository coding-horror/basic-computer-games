"""
AceyDuchy

From: BASIC Computer Games (1978)
      Edited by David Ahl

"The original BASIC program author was Bill Palmby
 of Prairie View, Illinois."

Python port by Aviyam Fischer, 2022
"""

from typing import List, Literal, TypeAlias, get_args

Suit: TypeAlias = Literal["\u2665", "\u2666", "\u2663", "\u2660"]
Rank: TypeAlias = Literal[2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]


class Card:
    def __init__(self, suit: Suit, rank: Rank) -> None:
        self.suit = suit
        self.rank = rank

    def __str__(self) -> str:
        r = str(self.rank)
        r = {"11": "J", "12": "Q", "13": "K", "14": "A"}.get(r, r)
        return f"{r}{self.suit}"


class Deck:
    def __init__(self) -> None:
        self.cards: List[Card] = []
        self.build()

    def build(self) -> None:
        for suit in get_args(Suit):
            for rank in get_args(Rank):
                self.cards.append(Card(suit, rank))

    def shuffle(self) -> None:
        import random

        random.shuffle(self.cards)

    def deal(self) -> Card:
        return self.cards.pop()


class Game:
    def __init__(self) -> None:
        self.deck = Deck()
        self.deck.shuffle()
        self.card_a = self.deck.deal()
        self.card_b = self.deck.deal()
        self.money = 100
        self.not_done = True

    def play(self) -> None:
        while self.not_done:
            while self.money > 0:
                card_a = self.card_a
                card_b = self.card_b

                if card_a.rank > card_b.rank:
                    card_a, card_b = card_b, card_a

                if card_a.rank == card_b.rank:
                    self.card_b = self.deck.deal()
                    card_b = self.card_b

                print(f"You have:\t ${self.money} ")
                print(f"Your cards:\t {card_a} {card_b}")

                bet = int(input("What is your bet? "))
                player_card = self.deck.deal()
                if 0 < bet <= self.money:

                    print(f"Your deal:\t {player_card}")
                    if card_a.rank < player_card.rank < card_b.rank:
                        print("You Win!")
                        self.money += bet
                    else:
                        print("You Lose!")
                        self.money -= bet
                        self.not_done = False
                else:
                    print("Chicken!")
                    print(f"Your deal should have been: {player_card}")
                    if card_a.rank < player_card.rank < card_b.rank:
                        print("You could have won!")
                    else:
                        print("You would lose, so it was wise of you to chicken out!")

                    self.not_done = False
                    break

                if len(self.deck.cards) <= 3:
                    print("You ran out of cards. Game over.")
                    self.not_done = False
                    break

                self.card_a = self.deck.deal()
                self.card_b = self.deck.deal()

        if self.money == 0:
            self.not_done = False


def game_loop() -> None:
    game_over = False

    while not game_over:
        game = Game()
        game.play()
        print(f"You have ${game.money} left")
        print("Would you like to play again? (y/n)")
        if input() == "n":
            game_over = True


def main():
    print(
        """
    Acey Ducey is a card game where you play against the computer.
    The Dealer(computer) will deal two cards facing up.
    You have an option to bet or not bet depending on whether or not you
    feel the card will have a value between the first two.
    If you do not want to bet input a 0
    """
    )
    game_loop()
    print("\nThanks for playing!")


if __name__ == "__main__":
    main()
