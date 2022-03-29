import io

from _pytest.monkeypatch import MonkeyPatch
from _pytest.capture import CaptureFixture

from bounce import main


def test_bounce(monkeypatch: MonkeyPatch, capsys: CaptureFixture[str]) -> None:
    time_increment = 0.1
    velocity = 30
    coefficient = 1
    monkeypatch.setattr(
        "sys.stdin",
        io.StringIO(f"{time_increment:0.1f}\n{velocity}\n{coefficient}\n"),
    )
    main()
    actual = capsys.readouterr().out
    expected = """                             BOUNCE
           CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



THIS SIMULATION LETS YOU SPECIFY THE INITIAL VELOCITY
OF A BALL THROWN STRAIGHT UP, AND THE COEFFICIENT OF
ELASTICITY OF THE BALL.  PLEASE USE A DECIMAL FRACTION
COEFFICIENCY (LESS THAN 1).

YOU ALSO SPECIFY THE TIME INCREMENT TO BE USED IN
'STROBING' THE BALL'S FLIGHT (TRY .1 INITIALLY).

TIME INCREMENT (SEC)? 
VELOCITY (FPS)? 
COEFFICIENT? 
FEET

14      000                 000               000
           0                   0                 0
13     0     0             0    0            0    0

12    0       0           0      0           0     0

11   0                   0                  0
               0                  0                 0
10
     0                  0                  0
9               0                  0                 0

8
    0                  0                  0
7                0                  0                 0

6
   0                  0                  0
5                 0                  0                 0

4

3 0                  0                  0

2                  0                  0                 0

1

00                  0                  0
...................................................................
 0        1         2         3         4         5         6

                               SECONDS

"""  # noqa: W291
    assert actual.split("\n") == expected.split("\n")
