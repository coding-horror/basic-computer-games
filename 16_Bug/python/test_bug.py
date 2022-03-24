import io
from typing import Callable

import pytest
from _pytest.monkeypatch import MonkeyPatch

from bug import main
from bug_overengineered import main as overengineered_main


@pytest.mark.parametrize(
    "main",
    [main, overengineered_main],
)
def test_main(monkeypatch: MonkeyPatch, main: Callable[[], None]) -> None:
    monkeypatch.setattr("time.sleep", lambda n: n)
    instructions = "Y"
    pictures = "Y"
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(
            f"{instructions}\n{pictures}\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\nN\n"
        ),
    )
    main()
