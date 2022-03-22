import io
from _pytest.monkeypatch import MonkeyPatch
from _pytest.capture import CaptureFixture
from banner import print_banner


def test_print_banner(monkeypatch: MonkeyPatch) -> None:
    horizontal = "1"
    vertical = "1"
    centered = "1"
    char = "*"
    statement = "O"  # only capital letters
    set_page = "2"
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(
            f"{horizontal}\n{vertical}\n{centered}\n{char}\n{statement}\n{set_page}"
        ),
    )
    print_banner()


def test_print_banner_horizontal_0(
    monkeypatch: MonkeyPatch, capsys: CaptureFixture
) -> None:
    horizontal = "1"
    vertical = "1"
    centered = "1"
    char = "*"
    statement = "O"  # only capital letters
    set_page = "2"
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(
            f"0\n{horizontal}\n{vertical}\n{centered}\n{char}\n{statement}\n{set_page}"
        ),
    )
    print_banner()
    captured = capsys.readouterr()
    assert "Please enter a number greater than zero" in captured.out


def test_print_banner_vertical_0(
    monkeypatch: MonkeyPatch, capsys: CaptureFixture
) -> None:
    horizontal = "1"
    vertical = "1"
    centered = "1"
    char = "*"
    statement = "O"  # only capital letters
    set_page = "2"
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(
            f"{horizontal}\n0\n{vertical}\n{centered}\n{char}\n{statement}\n{set_page}"
        ),
    )
    print_banner()
    captured = capsys.readouterr()
    assert "Please enter a number greater than zero" in captured.out


def test_print_banner_centered(
    monkeypatch: MonkeyPatch, capsys: CaptureFixture
) -> None:
    horizontal = "1"
    vertical = "1"
    centered = "Y"
    char = "*"
    statement = "O"  # only capital letters
    set_page = "2"
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(
            f"{horizontal}\n{vertical}\n{centered}\n{char}\n{statement}\n{set_page}"
        ),
    )
    print_banner()
    captured = capsys.readouterr()
    expected = (
        "Horizontal Vertical Centered Character "
        "(type 'ALL' if you want character being printed) Statement Set page "
        "                                                             *****\n"
        "                                                            *     *\n"
        "                                                           *       *\n"
        "                                                           *       *\n"
        "                                                           *       *\n"
        "                                                            *     *\n"
        "                                                             *****\n\n\n"
    )
    assert captured.out.split("\n") == expected.split("\n")


def test_print_banner_all_statement(
    monkeypatch: MonkeyPatch, capsys: CaptureFixture
) -> None:
    horizontal = "1"
    vertical = "1"
    centered = "1"
    char = "UNIT TESTING"
    statement = "ALL"  # only capital letters
    set_page = "2"
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(
            f"{horizontal}\n{vertical}\n{centered}\n{char}\n{statement}\n{set_page}"
        ),
    )
    print_banner()
