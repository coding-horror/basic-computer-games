import io

import pytest
from superstartrek import main


def test_main(monkeypatch, capsys):
    monkeypatch.setattr("sys.stdin", io.StringIO("NAV\n1\n1\nSRS\nXXX\nXXX\n"))
    with pytest.raises(SystemExit):
        main()
    # captured = capsys.readouterr()
