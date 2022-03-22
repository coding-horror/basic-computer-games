import io

from _pytest.monkeypatch import MonkeyPatch

from bombardment import play


def test_bombardment(monkeypatch: MonkeyPatch) -> None:
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(
            "\n1 2 3 4\n6\n1\n2\n3\n4\n5\n6\n7\n8\n9\n10"
            "\n11\n12\n13\n14\n15\n16\n17\n18\n19\n20"
            "\n21\n22\n23\n24\n25"
        ),
    )
    play()
