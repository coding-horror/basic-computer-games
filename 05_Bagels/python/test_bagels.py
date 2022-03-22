import io
from _pytest.capture import CaptureFixture
from _pytest.monkeypatch import MonkeyPatch

from bagels import build_result_string, main, pick_number


def test_build_result_string() -> None:
    build_result_string(["a", "b", "c"], "abc")


def test_pick_number() -> None:
    picked = pick_number()
    assert len(picked) == 3
    for el in picked:
        assert el in "0123456789"


def test_main(monkeypatch: MonkeyPatch, capsys: CaptureFixture) -> None:
    # Succeed
    round_1 = "Y\n4444\nabc\n444\n456\n145\n321\n123"

    # Fail after 20 guesses
    round_2 = (
        "666\n132\n321\n312\n132\n213\n678\n678\n678\n678\n678\n"
        "678\n678\n678\n678\n678\n678\n678\n678\n678\n678\nNo"
    )
    monkeypatch.setattr("sys.stdin", io.StringIO(f"{round_1}\nYES\n{round_2}"))
    monkeypatch.setattr("bagels.pick_number", lambda: ["1", "2", "3"])
    main()
    captured = capsys.readouterr()
    assert "Would you like the rules" in captured.out
    assert "I have a number in mind" in captured.out
    assert "My number was" in captured.out
    assert "Hope you had fun." in captured.out
