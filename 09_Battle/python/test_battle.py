import io
from typing import Callable

import pytest
from _pytest.monkeypatch import MonkeyPatch

from battle import main as main_one
from battle_oo import main as main_oo


@pytest.mark.parametrize(
    "main",
    [main_one, main_oo],
)
def test_main(monkeypatch: MonkeyPatch, main: Callable[[], None]) -> None:
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(
            "1,1\n1,2\n1,3\n1,4\n1,5\n1,6\n"
            "2,1\n2,2\n2,3\n2,4\n2,5\n2,6\n"
            "3,1\n3,2\n3,3\n3,4\n3,5\n3,6\n"
            "4,1\n4,2\n4,3\n4,4\n4,5\n4,6\n"
            "5,1\n5,2\n5,3\n5,4\n5,5\n5,6\n"
            "6,1\n6,2\n6,3\n6,4\n6,5\n6,6\n"
        ),
    )
    main()
