import io
import pytest
from _pytest.capture import CaptureFixture
from _pytest.monkeypatch import MonkeyPatch

from amazing import build_maze, welcome_header, main


def test_welcome_header(capsys: CaptureFixture[str]) -> None:
    capsys.readouterr()
    welcome_header()
    out, err = capsys.readouterr()
    assert out == (
        "                            AMAZING PROGRAM\n"
        "               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n\n"
    )
    assert err == ""


@pytest.mark.parametrize(
    ("width", "length"),
    [
        (1, 1),
        (1, 0),
        (1, -1),
        (1, 2),
        (2, 1),
    ],
)
def test_build_maze(width: int, length: int) -> None:
    with pytest.raises(AssertionError):
        build_maze(width, length)


@pytest.mark.parametrize(
    ("width", "length"),
    [
        (3, 3),
        (10, 10),
    ],
)
def test_main(monkeypatch: MonkeyPatch, width: int, length: int) -> None:
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(f"{width},{length}"),
    )
    main()


def test_main_error(monkeypatch: MonkeyPatch) -> None:
    width = 1
    length = 2

    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(f"{width},{length}\n3,3"),
    )
    main()
