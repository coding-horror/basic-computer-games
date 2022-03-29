import io
from _pytest.capture import CaptureFixture
from _pytest.monkeypatch import MonkeyPatch

from batnum import main


def test_main_win(monkeypatch: MonkeyPatch, capsys: CaptureFixture[str]) -> None:
    pile_size = 1
    monkeypatch.setattr("sys.stdin", io.StringIO(f"{pile_size}\n1\n1 2\n2\n1\n-1\n"))
    main()
    captured = capsys.readouterr()
    assert "CONGRATULATIONS, YOU WIN" in captured.out


def test_main_lose(monkeypatch: MonkeyPatch, capsys: CaptureFixture[str]) -> None:
    pile_size = 3
    monkeypatch.setattr("sys.stdin", io.StringIO(f"{pile_size}\n2\n1 2\n2\n1\n1\n-1\n"))
    main()
    captured = capsys.readouterr()
    assert "TOUGH LUCK, YOU LOSE" in captured.out
