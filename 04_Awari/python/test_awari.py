import io
from _pytest.monkeypatch import MonkeyPatch
import pytest

from awari import print_with_tab, main


def test_print_with_tab() -> None:
    print_with_tab(3, "Hello")


def test_main(monkeypatch: MonkeyPatch) -> None:
    monkeypatch.setattr("sys.stdin", io.StringIO("1\n2\n3\n4\n5\n6"))
    with pytest.raises(EOFError):
        main()
