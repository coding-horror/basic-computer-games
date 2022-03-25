import io

from _pytest.monkeypatch import MonkeyPatch
from bullfight import main


def test_main(monkeypatch: MonkeyPatch) -> None:
    instructions = "Y"
    kill_bull = "YES"
    kill_method = "0"
    run_from_ring = "YES"
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(f"{instructions}\n{kill_bull}\n{kill_method}\n{run_from_ring}\n"),
    )
    main()
