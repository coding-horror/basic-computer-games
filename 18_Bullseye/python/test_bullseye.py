import io

from _pytest.monkeypatch import MonkeyPatch
from bullseye import main


def test_main(monkeypatch: MonkeyPatch) -> None:
    nb_players = 1
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(f"{nb_players}\nMartin\n3\n2\n1" + ("\n2" * 21)),
    )
    main()
