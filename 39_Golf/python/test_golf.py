import io
import math

import pytest
from golf import (
    CircleGameObj,
    GameObjType,
    Golf,
    Point,
    RectGameObj,
    get_distance,
    is_in_rectangle,
    odds,
    to_degrees_360,
    to_radians,
)


def test_odds() -> None:
    n = 1000
    p = sum(odds(50) for i in range(n)) / n
    assert abs(p - 0.5) < 0.1


@pytest.mark.parametrize(
    ("p1", "p2", "expected"),
    [
        ((0, 0), (0, 0), 0),
        ((0, 0), (1, 0), 1),
        ((0, 0), (0, 1), 1),
        ((0, 1), (0, 0), 1),
        ((1, 0), (0, 0), 1),
        ((0, 0), (2, 0), 2),
        ((0, 0), (0, 2), 2),
        ((0, 2), (0, 0), 2),
        ((2, 0), (0, 0), 2),
        ((0, 0), (1, 1), 2**0.5),
        ((2, 3), (4, 5), (2**2 + 2**2) ** 0.5),
    ],
)
def test_get_distance(p1, p2, expected):
    assert get_distance(Point(*p1), Point(*p2)) == expected


@pytest.mark.parametrize(
    ("pt", "rect", "expected"),
    [
        (
            CircleGameObj(1, 1, 1, GameObjType.BALL),
            RectGameObj(0, 0, 2, 2, GameObjType.GREEN),
            True,
        ),
        (
            CircleGameObj(1, 1, 1, GameObjType.BALL),
            RectGameObj(0, 0, 1, 1, GameObjType.GREEN),
            False,
        ),
    ],
)
def test_is_in_rectangle(pt, rect, expected):
    assert is_in_rectangle(pt, rect) == expected


@pytest.mark.parametrize(
    ("angle", "radians"),
    [
        (0, 0),
        (180, math.pi),
        (360, 2 * math.pi),
    ],
)
def test_to_radians(angle, radians):
    assert to_radians(angle) == radians
    assert to_degrees_360(radians) == angle


def test_golf(monkeypatch, capsys):
    handycap = 10
    difficulty = 4
    club = 10
    swing = 10
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(f"10\na\n{handycap}\n{difficulty}\n{club}\n{swing}\nQUIT"),
    )
    Golf()
    out, err = capsys.readouterr()
    assert err == ""
