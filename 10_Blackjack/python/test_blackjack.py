import io

from _pytest.monkeypatch import MonkeyPatch
from _pytest.capture import CaptureFixture

from blackjack import main


def test_blackjack(monkeypatch: MonkeyPatch, capsys: CaptureFixture[str]) -> None:
    nb_players = 1
    instructions = "y"
    bet = 100
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(f"{nb_players}\n{instructions}\n\n{bet}\ns\nn\n"),
    )
    main()
