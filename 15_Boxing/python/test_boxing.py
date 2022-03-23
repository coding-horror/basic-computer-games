import io

from _pytest.capture import CaptureFixture
from _pytest.monkeypatch import MonkeyPatch

from boxing import play


def test_boxing_bad_opponent(
    monkeypatch: MonkeyPatch, capsys: CaptureFixture[str]
) -> None:
    monkeypatch.setattr("boxing.Player.get_punch_choice", lambda self: 1)
    monkeypatch.setattr("boxing.get_opponent_stats", lambda: (2, 1))
    monkeypatch.setattr("boxing.is_opponents_turn", lambda: False)

    opponent = "Anna"
    my_man = "Bob"
    strength = "1"
    weakness = "2"

    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(f"{opponent}\n{my_man}\n{strength}\n{weakness}\n1\n1\n1"),
    )
    play()
    actual = capsys.readouterr().out
    expected = """BOXING
CREATIVE COMPUTING   MORRISTOWN, NEW JERSEY



BOXING OLYMPIC STYLE (3 ROUNDS -- 2 OUT OF 3 WINS)
WHAT IS YOUR OPPONENT'S NAME? WHAT IS YOUR MAN'S NAME? DIFFERENT PUNCHES ARE 1 FULL SWING 2 HOOK 3 UPPERCUT 4 JAB
WHAT IS YOUR MAN'S BEST? WHAT IS HIS VULNERABILITY? Anna'S ADVANTAGE is 1 AND VULNERABILITY IS SECRET.
ROUND 1 BEGINS...

Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob WINS ROUND 1
ROUND 2 BEGINS...

Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob SWINGS AND HE CONNECTS!
Bob WINS ROUND 2
ROUND 3 BEGINS...

Bob AMAZINGLY WINS


AND NOW GOODBYE FROM THE OLYMPIC ARENA.
"""
    assert actual.split("\n") == expected.split("\n")
