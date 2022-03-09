from acey_ducey_oo import Card, Deck, Game


def test_card_init() -> None:
    card = Card("\u2665", 2)
    assert card.suit == "\u2665"
    assert card.rank == 2


def test_card_str() -> None:
    card = Card("\u2665", 2)
    assert str(card) == "2\u2665"


def test_deck_init() -> None:
    deck = Deck()
    assert len(deck.cards) == 52
    assert deck.cards[0].suit == "\u2665"
    assert deck.cards[0].rank == 2


def test_deck_shuffle() -> None:
    deck = Deck()
    deck.shuffle()


def test_deck_deal() -> None:
    deck = Deck()
    card = deck.deal()
    assert card.rank == 14
    assert card.suit == "\u2660"


def test_game_init() -> None:
    game = Game()
    assert len(game.deck.cards) == 50  # two are already dealt
