import io
from typing import List

from _pytest.capture import CaptureFixture
from _pytest.monkeypatch import MonkeyPatch

from bowling import main


def test_bowling_strikes(monkeypatch: MonkeyPatch, capsys: CaptureFixture[str]) -> None:
    def perfect_roll(pins: List[int]) -> None:
        for i in range(20):
            x = i
            if x < len(pins):
                pins[x] = 1

    monkeypatch.setattr("bowling.simulate_roll", perfect_roll)

    instructions1 = "Y"
    players1 = 1
    name1 = "Martin"
    another_game1 = "Y"

    instructions2 = "N"
    players2 = 2
    name21 = "Anna"
    name22 = "Bob"
    another_game2 = "N"
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(
            f"{instructions1}\n{players1}\n{name1}\n{another_game1}\n"
            f"{instructions2}\n{players2}\n{name21}\n{name22}\n{another_game2}"
        ),
    )
    main()
    actual = capsys.readouterr().out
    expected = """                                      Bowl
                   CREATIVE COMPUTING MORRISTOWN, NEW JERSEY

WELCOME TO THE ALLEY.
BRING YOUR FRIENDS.
OKAY LET'S FIRST GET ACQUAINTED.

THE INSTRUCTIONS (Y/N)? THE GAME OF BOWLING TAKES MIND AND SKILL. DURING THE GAME
THE COMPUTER WILL KEEP SCORE. YOU MAY COMPETE WITH
OTHER PLAYERS[UP TO FOUR]. YOU WILL BE PLAYING TEN FRAMES.
ON THE PIN DIAGRAM 'O' MEANS THE PIN IS DOWN...'+' MEANS THE
PIN IS STANDING. AFTER THE GAME THE COMPUTER WILL SHOW YOUR
SCORES.
FIRST OF ALL...HOW MANY ARE PLAYING? 
VERY GOOD...
Enter name for player 1: 
O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Martin
STRIKE!!!
Extra rolls for Martin
Martin: [10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10], total:300
DO YOU WANT ANOTHER GAME? 
THE INSTRUCTIONS (Y/N)? FIRST OF ALL...HOW MANY ARE PLAYING? 
VERY GOOD...
Enter name for player 1: Enter name for player 2: 
O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!

O O O O 
 O O O 
  O O 
   O 
10 for Anna
STRIKE!!!
Extra rolls for Anna

O O O O 
 O O O 
  O O 
   O 
10 for Bob
STRIKE!!!
Extra rolls for Bob
Anna: [10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10], total:300
Bob: [10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10], total:300
DO YOU WANT ANOTHER GAME? """  # noqa: W291
    assert actual.split("\n") == expected.split("\n")
