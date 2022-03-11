import io

import hamurabi


def test_main(monkeypatch, capsys):
    monkeypatch.setattr("sys.stdin", io.StringIO("100\n100\n100"))
    hamurabi.main()
    captured = capsys.readouterr()
    actual_lines = captured.out.splitlines()
    expected_lines = [
        "HAMURABI",  # 0
        "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY",  # 1
        "",  # 2
        "",  # 3
        "",  # 4
        "",  # 5
        "TRY YOUR HAND AT GOVERNING ANCIENT SUMERIA",  # 6
        "FOR A TEN-YEAR TERM OF OFFICE.",  # 7
        "",  # 8
        "",  # 9
        "",  # 10
        "",  # 11
        "HAMURABI:  I BEG TO REPORT TO YOU\n",  # 12
        "IN YEAR 1 , 0 PEOPLE STARVED, 5 CAME TO THE CITY,\n",  # 13
        "POPULATION IS NOW 100\n",  # 14
        "THE CITY NOW OWNS 1000.0 ACRES.",  # 15
    ]
    for i, (actual, expected) in enumerate(zip(actual_lines, expected_lines)):
        assert actual.strip() == expected.strip(), f"Line {i} is wrong"
