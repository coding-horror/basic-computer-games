import io
from unittest import mock
from typing import TypeVar
from _pytest.capture import CaptureFixture
from _pytest.monkeypatch import MonkeyPatch

from acey_ducey import play_game


@mock.patch("random.shuffle")
def test_play_game_lose(mock_random_shuffle, monkeypatch: MonkeyPatch, capsys: CaptureFixture) -> None:
    monkeypatch.setattr("sys.stdin", io.StringIO("100\n100"))
    T = TypeVar("T")

    def identity(x: T) -> T:
        return x

    mock_random_shuffle = identity  # noqa: F841
    play_game()
    captured = capsys.readouterr()
    assert captured.out == (
        "You now have 100 dollars\n\n"
        "Here are you next two cards\n King\n Ace\n\n"
        "What is your bet?  Queen\n"
        "Sorry, you lose\n"
        "Sorry, friend, but you blew your wad\n"
    )
