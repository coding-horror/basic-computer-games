import io

from _pytest.monkeypatch import MonkeyPatch

from bombs_away import play_game


def test_bombs_away(monkeypatch: MonkeyPatch) -> None:
    side = 1
    target = 1
    missions = 1
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(f"{side}\n{target}\n{missions}\n3\n50"),
    )
    play_game()
