import io
from unittest import mock

from acey_ducey import play_game


@mock.patch("random.shuffle")
def test_play_game_lose(mock_random_shuffle, monkeypatch, capsys) -> None:
    monkeypatch.setattr("sys.stdin", io.StringIO("100\n100"))
    mock_random_shuffle = lambda n: n
    play_game()
    captured = capsys.readouterr()
    assert captured.out == (
        "You now have 100 dollars\n\n"
        "Here are you next two cards\n King\n Ace\n\n"
        "What is your bet?  Queen\n"
        "Sorry, you lose\n"
        "Sorry, friend, but you blew your wad\n"
    )
