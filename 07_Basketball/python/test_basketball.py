import io

import pytest
from _pytest.monkeypatch import MonkeyPatch
from _pytest.capture import CaptureFixture

from basketball import Basketball


def test_basketball(monkeypatch: MonkeyPatch, capsys: CaptureFixture[str]) -> None:
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO("\n1\n6\n1\n2\n1\n2\n1\n2\n1\n2\n3\n4\n5\n4"),
    )
    with pytest.raises(EOFError):
        Basketball()
