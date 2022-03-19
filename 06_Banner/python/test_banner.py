import io

from banner import print_banner


def test_print_banner(monkeypatch) -> None:
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
