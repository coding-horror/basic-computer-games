'''
        8""""8 8"""88 8     8""""
        8    " 8    8 8     8
        8e     8    8 8e    8eeee
        88  ee 8    8 88    88
        88   8 8    8 88    88
        88eee8 8eeee8 88eee 88

GOLF


Despite being a text based game, the code uses simple geometry to simulate a course.
Fairways are 40 yard wide rectangles, surrounded by 5 yards of rough around the perimeter.
The green is a circle of 10 yards radius around the cup.
The cup is always at point (0,0).

Using basic trigonometry we can plot the ball's location using the distance of the stroke and
and the angle of deviation (hook/slice).

The stroke distances are based on real world averages of different club types.
Lots of randomization, "business rules", and luck influence the game play.
Probabilities are commented in the code.

note: 'courseInfo', 'clubs', & 'scoreCard' arrays each include an empty object so indexing
can begin at 1. Like all good programmers we count from zero, but in this context,
it's more natural when hole number one is at index one


    |-----------------------------|
    |            rough            |
    |   ----------------------    |
    |   |                     |   |
    | r |        =  =         | r |
    | o |     =        =      | o |
    | u |    =    .     =     | u |
    | g |    =   green  =     | g |
    | h |     =        =      | h |
    |   |        =  =         |   |
    |   |                     |   |
    |   |                     |   |
    |   |      Fairway        |   |
    |   |                     |   |
    |   |               ------    |
    |   |            --        -- |
    |   |           --  hazard  --|
    |   |            --        -- |
    |   |               ------    |
    |   |                     |   |
    |   |                     |   |   out
    |   |                     |   |   of
    |   |                     |   |   bounds
    |   |                     |   |
    |   |                     |   |
    |            tee              |


Typical green size: 20-30 yards
Typical golf course fairways are 35 to 45 yards wide
Our fairway extends 5 yards past green
Our rough is a 5 yard perimeter around fairway

We calculate the new position of the ball given the ball's point, the distance
of the stroke, and degrees off line (hook or slice).

Degrees off (for a right handed golfer):
Slice: positive degrees = ball goes right
Hook: negative degrees = left goes left

The cup is always at point: 0,0.
We use atan2 to compute the angle between the cup and the ball.
Setting the cup's vector to 0,-1 on a 360 circle is equivalent to:
0 deg = 12 o'clock;  90 deg = 3 o'clock;  180 deg = 6 o'clock;  270 = 9 o'clock
The reverse angle between the cup and the ball is a difference of PI (using radians).

Given the angle and stroke distance (hypotenuse), we use cosine to compute
the opposite and adjacent sides of the triangle, which, is the ball's new position.

        0
        |
270 - cup - 90
        |
        180


        cup
        |
        |
        | opp
        |-----* new position
        |    /
        |   /
    adj  |  /
        | /  hyp
        |/
        tee

<- hook    slice ->


Given the large number of combinations needed to describe a particular stroke / ball location,
we use the technique of "bitwise masking" to describe stroke results.
With bit masking, multiple flags (bits) are combined into a single binary number that can be
tested by applying a mask. A mask is another binary number that isolates a particular bit that
you are interested in. You can then apply your language's bitwise opeartors to test or
set a flag.

Game design by Jason Bonthron, 2021
www.bonthron.com
for my father, Raymond Bonthron, an avid golfer

Inspired by the 1978 "Golf" from "Basic Computer Games"
by Steve North, who modified an existing golf game by an unknown author

Ported in 2022 to Python by Martin Thoma
'''


import enum
import math
import random
import time
from dataclasses import dataclass
from functools import partial
from typing import Any, Callable, List, NamedTuple, Tuple


def clear_console() -> None:
    print("\033[H\033[J", end="")


class Point(NamedTuple):
    X: int
    Y: int


class GameObjType(enum.Enum):
    BALL = enum.auto()
    CUP = enum.auto()
    GREEN = enum.auto()
    FAIRWAY = enum.auto()
    ROUGH = enum.auto()
    TREES = enum.auto()
    WATER = enum.auto()
    SAND = enum.auto()


class CircleGameObj(NamedTuple):
    # center point
    X: int
    Y: int
    Radius: int
    Type: GameObjType


class RectGameObj(NamedTuple):
    # Upper left corner
    X: int
    Y: int
    Width: int
    Length: int
    Type: GameObjType


Ball = CircleGameObj
Hazard = CircleGameObj


class HoleInfo(NamedTuple):
    hole: int
    yards: int
    par: int
    hazards: List[Hazard]
    description: str


class HoleGeometry(NamedTuple):
    cup: CircleGameObj
    green: CircleGameObj
    fairway: RectGameObj
    rough: RectGameObj
    hazards: List[Hazard]


@dataclass
class Plot:
    X: int
    Y: int
    Offline: int


def get_distance(pt1: Point, pt2: Point) -> float:
    """distance between 2 points"""
    return math.sqrt(math.pow((pt2.X - pt1.X), 2) + math.pow((pt2.Y - pt1.Y), 2))


def is_in_rectangle(pt: CircleGameObj, rect: RectGameObj) -> bool:
    # only true if its completely inside
    return (
        (pt.X > rect.X)
        and (pt.X < rect.X + rect.Width)
        and (pt.Y > rect.Y)
        and (pt.Y < rect.Y + rect.Length)
    )


def to_radians(angle: float) -> float:
    return angle * (math.pi / 180.0)


def to_degrees_360(angle: float) -> float:
    """radians to 360 degrees"""
    deg = angle * (180.0 / math.pi)
    if deg < 0.0:
        deg += 360.0
    return deg


def odds(x: int) -> bool:
    # chance an integer is <= the given argument
    # between 1-100
    return random.randint(1, 101) <= x


# THE COURSE
CourseInfo = [
    HoleInfo(0, 0, 0, [], ""),  # include a blank so index 1 == hole 1
    # -------------------------------------------------------- front 9
    HoleInfo(
        1,
        361,
        4,
        [
            Hazard(20, 100, 10, GameObjType.TREES),
            Hazard(-20, 80, 10, GameObjType.TREES),
            Hazard(-20, 100, 10, GameObjType.TREES),
        ],
        "There are a couple of trees on the left and right.",
    ),
    HoleInfo(
        2,
        389,
        4,
        [Hazard(0, 160, 20, GameObjType.WATER)],
        "There is a large water hazard across the fairway about 150 yards.",
    ),
    HoleInfo(
        3,
        206,
        3,
        [
            Hazard(20, 20, 5, GameObjType.WATER),
            Hazard(-20, 160, 10, GameObjType.WATER),
            Hazard(10, 12, 5, GameObjType.SAND),
        ],
        "There is some sand and water near the green.",
    ),
    HoleInfo(
        4,
        500,
        5,
        [Hazard(-14, 12, 12, GameObjType.SAND)],
        "There's a bunker to the left of the green.",
    ),
    HoleInfo(
        5,
        408,
        4,
        [
            Hazard(20, 120, 20, GameObjType.TREES),
            Hazard(20, 160, 20, GameObjType.TREES),
            Hazard(10, 20, 5, GameObjType.SAND),
        ],
        "There are some trees to your right.",
    ),
    HoleInfo(
        6,
        359,
        4,
        [Hazard(14, 0, 4, GameObjType.SAND), Hazard(-14, 0, 4, GameObjType.SAND)],
        "",
    ),
    HoleInfo(
        7,
        424,
        5,
        [
            Hazard(20, 200, 10, GameObjType.SAND),
            Hazard(10, 180, 10, GameObjType.SAND),
            Hazard(20, 160, 10, GameObjType.SAND),
        ],
        "There are several sand traps along your right.",
    ),
    HoleInfo(8, 388, 4, [Hazard(-20, 340, 10, GameObjType.TREES)], ""),
    HoleInfo(
        9,
        196,
        3,
        [Hazard(-30, 180, 20, GameObjType.TREES), Hazard(14, -8, 5, GameObjType.SAND)],
        "",
    ),
    # -------------------------------------------------------- back 9
    HoleInfo(
        hole=10,
        yards=400,
        par=4,
        hazards=[
            Hazard(-14, -8, 5, GameObjType.SAND),
            Hazard(14, -8, 5, GameObjType.SAND),
        ],
        description="",
    ),
    HoleInfo(
        11,
        560,
        5,
        [
            Hazard(-20, 400, 10, GameObjType.TREES),
            Hazard(-10, 380, 10, GameObjType.TREES),
            Hazard(-20, 260, 10, GameObjType.TREES),
            Hazard(-20, 200, 10, GameObjType.TREES),
            Hazard(-10, 180, 10, GameObjType.TREES),
            Hazard(-20, 160, 10, GameObjType.TREES),
        ],
        "Lots of trees along the left of the fairway.",
    ),
    HoleInfo(
        12,
        132,
        3,
        [
            Hazard(-10, 120, 10, GameObjType.WATER),
            Hazard(-5, 100, 10, GameObjType.SAND),
        ],
        "There is water and sand directly in front of you. A good drive should clear both.",
    ),
    HoleInfo(
        13,
        357,
        4,
        [
            Hazard(-20, 200, 10, GameObjType.TREES),
            Hazard(-10, 180, 10, GameObjType.TREES),
            Hazard(-20, 160, 10, GameObjType.TREES),
            Hazard(14, 12, 8, GameObjType.SAND),
        ],
        "",
    ),
    HoleInfo(14, 294, 4, [Hazard(0, 20, 10, GameObjType.SAND)], ""),
    HoleInfo(
        15,
        475,
        5,
        [Hazard(-20, 20, 10, GameObjType.WATER), Hazard(10, 20, 10, GameObjType.SAND)],
        "Some sand and water near the green.",
    ),
    HoleInfo(16, 375, 4, [Hazard(-14, -8, 5, GameObjType.SAND)], ""),
    HoleInfo(
        17,
        180,
        3,
        [
            Hazard(20, 100, 10, GameObjType.TREES),
            Hazard(-20, 80, 10, GameObjType.TREES),
        ],
        "",
    ),
    HoleInfo(
        18,
        550,
        5,
        [Hazard(20, 30, 15, GameObjType.WATER)],
        "There is a water hazard near the green.",
    ),
]


# -------------------------------------------------------- bitwise Flags
dub = 0b00000000000001
hook = 0b00000000000010
slice_ = 0b00000000000100
passed_cup = 0b00000000001000
in_cup = 0b00000000010000
on_fairway = 0b00000000100000
on_green = 0b00000001000000
in_rough = 0b00000010000000
in_sand = 0b00000100000000
in_trees = 0b00001000000000
in_water = 0b00010000000000
out_of_bounds = 0b00100000000000
luck = 0b01000000000000
ace = 0b10000000000000


class Golf:
    BALL: Ball
    HOLE_NUM: int = 0
    STROKE_NUM: int = 0
    handicap: int = 0
    player_difficulty: int = 0
    hole_geometry: HoleGeometry

    # all fairways are 40 yards wide, extend 5 yards beyond the cup, and
    # have 5 yards of rough around the perimeter
    fairway_width: int = 40
    fairway_extension: int = 5
    rough_amt: int = 5

    # ScoreCard records the ball position after each stroke
    # a new list for each hole
    # include a blank list so index 1 == hole 1
    score_card: List[List[Ball]] = [[]]

    # YOUR BAG
    clubs: List[Tuple[str, int]] = [
        ("", 0),
        # name, average yardage
        ("Driver", 250),
        ("3 Wood", 225),
        ("5 Wood", 200),
        ("Hybrid", 190),
        ("4 Iron", 170),
        ("7 Iron", 150),
        ("9 Iron", 125),
        ("Pitching wedge", 110),
        ("Sand wedge", 75),
        ("Putter", 10),
    ]

    def __init__(self) -> None:
        print(" ")
        print('          8""""8 8"""88 8     8"""" ')
        print('          8    " 8    8 8     8     ')
        print("          8e     8    8 8e    8eeee ")
        print("          88  ee 8    8 88    88    ")
        print("          88   8 8    8 88    88    ")
        print("          88eee8 8eeee8 88eee 88    ")
        print(" ")
        print("Welcome to the Creative Computing Country Club,")
        print("an eighteen hole championship layout located a short")
        print("distance from scenic downtown Lambertville, New Jersey.")
        print("The game will be explained as you play.")
        print("Enjoy your game! See you at the 19th hole...")
        print(" ")
        print("Type QUIT at any time to leave the game.")
        print("Type BAG at any time to review the clubs in your bag.")
        print(" ")

        input("Press any key to continue.")
        clear_console()
        self.start_game()

    def start_game(self) -> None:
        print(" ")
        print("              YOUR BAG")
        self.review_bag()
        print("Type BAG at any time to review the clubs in your bag.")
        print(" ")

        input("Press any key to continue.")
        clear_console()
        self.ask_handicap()

    def ask_handicap(self) -> None:
        print(" ")

        self.ask(
            "PGA handicaps range from 0 to 30.\nWhat is your handicap?",
            0,
            30,
            self.set_handicap_ask_difficulty,
        )

    def set_handicap_ask_difficulty(self, i: int) -> None:
        self.handicap = i
        print(" ")

        self.ask(
            (
                "Common difficulties at golf include:\n"
                "1=Hook, 2=Slice, 3=Poor Distance, 4=Trap Shots, 5=Putting\n"
                "Which one is your worst?"
            ),
            1,
            5,
            self.set_difficulty_and_hole,
        )

    def set_difficulty_and_hole(self, j: int) -> None:
        self.player_difficulty = j
        clear_console()
        self.new_hole()

    def new_hole(self) -> None:
        self.HOLE_NUM += 1
        self.STROKE_NUM = 0

        info: HoleInfo = CourseInfo[self.HOLE_NUM]

        yards: int = info.yards
        # from tee to cup
        cup = CircleGameObj(0, 0, 0, GameObjType.CUP)
        green = CircleGameObj(0, 0, 10, GameObjType.GREEN)

        fairway = RectGameObj(
            0 - int(self.fairway_width / 2),
            0 - (green.Radius + self.fairway_extension),
            self.fairway_width,
            yards + (green.Radius + self.fairway_extension) + 1,
            GameObjType.FAIRWAY,
        )

        rough = RectGameObj(
            fairway.X - self.rough_amt,
            fairway.Y - self.rough_amt,
            fairway.Width + (2 * self.rough_amt),
            fairway.Length + (2 * self.rough_amt),
            GameObjType.ROUGH,
        )

        self.BALL = Ball(0, yards, 0, GameObjType.BALL)

        self.score_card_start_new_hole()

        self.hole_geometry = HoleGeometry(cup, green, fairway, rough, info.hazards)

        print(f"                |> {self.HOLE_NUM}")
        print("                |        ")
        print("                |        ")
        print("          ^^^^^^^^^^^^^^^")

        print(
            f"Hole #{self.HOLE_NUM}. You are at the tee. Distance {info.yards} yards, par {info.par}."
        )
        print(info.description)

        self.tee_up()

    def set_putter_and_stroke(self, strength: float) -> None:
        putter = self.clubs[self.putt]
        self.Stroke((putter[1] * (strength / 10.0)), self.putt)

    def ask_gauge(self, c: int) -> None:
        self.club = self.clubs[c]

        print(" ")
        print(f"[{self.club[0].upper()}: average {self.club[1]} yards]")

        foo = partial(self.make_stroke, c=c)

        self.ask(
            "Now gauge your distance by a percentage of a full swing. (1-10)",
            1,
            10,
            foo,
        )

    def make_stroke(self, strength: float, c: int) -> None:
        self.Stroke((self.club[1] * (strength / 10.0)), c)

    def tee_up(self) -> None:
        # on the green? automatically select putter
        # otherwise Ask club and swing strength
        if self.is_on_green(self.BALL) and not self.is_in_hazard(
            self.BALL, GameObjType.SAND
        ):
            self.putt = 10
            print("[PUTTER: average 10 yards]")
            if odds(20):
                msg = "Keep your head down.\n"
            else:
                msg = ""

            self.ask(
                msg + "Choose your putt potency. (1-10)",
                1,
                10,
                self.set_putter_and_stroke,
            )
        else:
            self.ask("What club do you choose? (1-10)", 1, 10, self.ask_gauge)

    def Stroke(self, clubAmt: float, clubIndex: int) -> None:
        self.STROKE_NUM += 1

        flags = 0b000000000000

        # fore! only when driving
        if (self.STROKE_NUM == 1) and (clubAmt > 210) and odds(30):
            print('"...Fore !"')

        # dub
        if odds(5):
            # there's always a 5% chance of dubbing it
            flags |= dub

        # if you're in the rough, or sand, you really should be using a wedge
        if (
            self.is_in_rough(self.BALL)
            or self.is_in_hazard(self.BALL, GameObjType.SAND)
        ) and not (clubIndex == 8 or clubIndex == 9):
            if odds(40):
                flags |= dub

        # trap difficulty
        if (
            self.is_in_hazard(self.BALL, GameObjType.SAND)
            and self.player_difficulty == 4
        ):
            if odds(20):
                flags |= dub

        # hook/slice
        # There's 10% chance of a hook or slice
        # if it's a known playerDifficulty then increase chance to 30%
        # if it's a putt & putting is a playerDifficulty increase to 30%

        randHookSlice: bool
        if (
            self.player_difficulty == 1
            or self.player_difficulty == 2
            or (self.player_difficulty == 5 and self.is_on_green(self.BALL))
        ):
            randHookSlice = odds(30)
        else:
            randHookSlice = odds(10)

        if randHookSlice:
            if self.player_difficulty == 1:
                if odds(80):
                    flags |= hook
                else:
                    flags |= slice_
            elif self.player_difficulty == 2:
                if odds(80):
                    flags |= slice_
                else:
                    flags |= hook
            else:
                if odds(50):
                    flags |= hook
                else:
                    flags |= slice_

        # beginner's luck !
        # If handicap is greater than 15, there's a 10% chance of avoiding all errors
        if (self.handicap > 15) and (odds(10)):
            flags |= luck

        # ace
        # there's a 10% chance of an Ace on a par 3
        if CourseInfo[self.HOLE_NUM].par == 3 and odds(10) and self.STROKE_NUM == 1:
            flags |= ace

        # distance:
        # If handicap is < 15, there a 50% chance of reaching club average,
        # a 25% of exceeding it, and a 25% of falling short
        # If handicap is > 15, there's a 25% chance of reaching club average,
        # and 75% chance of falling short
        # The greater the handicap, the more the ball falls short
        # If poor distance is a known playerDifficulty, then reduce distance by 10%

        distance: float
        rnd = random.randint(1, 101)

        if self.handicap < 15:
            if rnd <= 25:
                distance = clubAmt - (clubAmt * (self.handicap / 100.0))
            elif rnd > 25 and rnd <= 75:
                distance = clubAmt
            else:
                distance = clubAmt + (clubAmt * 0.10)
        else:
            if rnd <= 75:
                distance = clubAmt - (clubAmt * (self.handicap / 100.0))
            else:
                distance = clubAmt

        if self.player_difficulty == 3:  # poor distance
            if odds(80):
                distance = distance * 0.80

        if (flags & luck) == luck:
            distance = clubAmt

        # angle
        # For all strokes, there's a possible "drift" of 4 degrees
        # a hooks or slice increases the angle between 5-10 degrees,
        # hook uses negative degrees
        angle = random.randint(0, 5)
        if (flags & slice_) == slice_:
            angle = random.randint(5, 11)
        if (flags & hook) == hook:
            angle = 0 - random.randint(5, 11)
        if (flags & luck) == luck:
            angle = 0

        plot = self.plot_ball(self.BALL, distance, angle)
        # calculate a new location
        if (flags & luck) == luck:
            if plot.Y > 0:
                plot.Y = 2

        flags = self.find_ball(
            Ball(plot.X, plot.Y, plot.Offline, GameObjType.BALL), flags
        )

        self.interpret_results(plot, flags)

    def plot_ball(self, ball: Ball, strokeDistance: float, degreesOff: float) -> Plot:
        cupVector = Point(0, -1)
        radFromCup = math.atan2(ball.Y, ball.X) - math.atan2(cupVector.Y, cupVector.X)
        radFromBall = radFromCup - math.pi

        hypotenuse = strokeDistance
        adjacent = math.cos(radFromBall + to_radians(degreesOff)) * hypotenuse
        opposite = math.sqrt(math.pow(hypotenuse, 2) - math.pow(adjacent, 2))

        newPos: Point
        if to_degrees_360(radFromBall + to_radians(degreesOff)) > 180:
            newPos = Point(int(ball.X - opposite), int(ball.Y - adjacent))
        else:
            newPos = Point(int(ball.X + opposite), int(ball.Y - adjacent))

        return Plot(newPos.X, newPos.Y, int(opposite))

    def interpret_results(self, plot: Plot, flags: int) -> None:
        cupDistance: int = int(
            get_distance(
                Point(plot.X, plot.Y),
                Point(self.hole_geometry.cup.X, self.hole_geometry.cup.Y),
            )
        )
        travelDistance: int = int(
            get_distance(Point(plot.X, plot.Y), Point(self.BALL.X, self.BALL.Y))
        )

        print(" ")

        if (flags & ace) == ace:
            print("Hole in One! You aced it.")
            self.score_card_record_stroke(Ball(0, 0, 0, GameObjType.BALL))
            self.ReportCurrentScore()
            return

        if (flags & in_trees) == in_trees:
            print("Your ball is lost in the trees. Take a penalty stroke.")
            self.score_card_record_stroke(self.BALL)
            self.tee_up()
            return

        if (flags & in_water) == in_water:
            if odds(50):
                msg = "Your ball has gone to a watery grave."
            else:
                msg = "Your ball is lost in the water."
            print(msg + " Take a penalty stroke.")
            self.score_card_record_stroke(self.BALL)
            self.tee_up()
            return

        if (flags & out_of_bounds) == out_of_bounds:
            print("Out of bounds. Take a penalty stroke.")
            self.score_card_record_stroke(self.BALL)
            self.tee_up()
            return

        if (flags & dub) == dub:
            print("You dubbed it.")
            self.score_card_record_stroke(self.BALL)
            self.tee_up()
            return

        if (flags & in_cup) == in_cup:
            if odds(50):
                msg = "You holed it."
            else:
                msg = "It's in!"
            print(msg)
            self.score_card_record_stroke(Ball(plot.X, plot.Y, 0, GameObjType.BALL))
            self.ReportCurrentScore()
            return

        if ((flags & slice_) == slice_) and not ((flags & on_green) == on_green):
            if (flags & out_of_bounds) == out_of_bounds:
                bad = "badly"
            else:
                bad = ""
            print(f"You sliced{bad}: {plot.Offline} yards offline.")

        if ((flags & hook) == hook) and not ((flags & on_green) == on_green):
            if (flags & out_of_bounds) == out_of_bounds:
                bad = "badly"
            else:
                bad = ""
            print(f"You hooked{bad}: {plot.Offline} yards offline.")

        if self.STROKE_NUM > 1:
            prevBall = self.score_card_get_previous_stroke()
            d1 = get_distance(
                Point(prevBall.X, prevBall.Y),
                Point(self.hole_geometry.cup.X, self.hole_geometry.cup.Y),
            )
            d2 = cupDistance
            if d2 > d1:
                print("Too much club.")

        if (flags & in_rough) == in_rough:
            print("You're in the rough.")

        if (flags & in_sand) == in_sand:
            print("You're in a sand trap.")

        if (flags & on_green) == on_green:
            if cupDistance < 4:
                pd = str(cupDistance * 3) + " feet"
            else:
                pd = f"{cupDistance} yards"
            print(f"You're on the green. It's {pd} from the pin.")

        if ((flags & on_fairway) == on_fairway) or ((flags & in_rough) == in_rough):
            print(
                f"Shot went {travelDistance} yards. "
                f"It's {cupDistance} yards from the cup."
            )

        self.score_card_record_stroke(Ball(plot.X, plot.Y, 0, GameObjType.BALL))

        self.BALL = Ball(plot.X, plot.Y, 0, GameObjType.BALL)

        self.tee_up()

    def ReportCurrentScore(self) -> None:
        par = CourseInfo[self.HOLE_NUM].par
        if len(self.score_card[self.HOLE_NUM]) == par + 1:
            print("A bogey. One above par.")
        if len(self.score_card[self.HOLE_NUM]) == par:
            print("Par. Nice.")
        if len(self.score_card[self.HOLE_NUM]) == (par - 1):
            print("A birdie! One below par.")
        if len(self.score_card[self.HOLE_NUM]) == (par - 2):
            print("An Eagle! Two below par.")
        if len(self.score_card[self.HOLE_NUM]) == (par - 3):
            print("Double Eagle! Unbelievable.")

        totalPar: int = 0
        for i in range(1, self.HOLE_NUM + 1):
            totalPar += CourseInfo[i].par

        print(" ")
        print("-----------------------------------------------------")
        if self.HOLE_NUM > 1:
            hole_str = "holes"
        else:
            hole_str = "hole"
        print(
            f" Total par for {self.HOLE_NUM} {hole_str} is: {totalPar}. "
            f"Your total is: {self.score_card_get_total()}."
        )
        print("-----------------------------------------------------")
        print(" ")

        if self.HOLE_NUM == 18:
            self.game_over()
        else:
            time.sleep(2)
            self.new_hole()

    def find_ball(self, ball: Ball, flags: int) -> int:
        if self.is_on_fairway(ball) and not self.is_on_green(ball):
            flags |= on_fairway
        if self.is_on_green(ball):
            flags |= on_green
        if self.is_in_rough(ball):
            flags |= in_rough
        if self.is_out_of_bounds(ball):
            flags |= out_of_bounds
        if self.is_in_hazard(ball, GameObjType.WATER):
            flags |= in_water
        if self.is_in_hazard(ball, GameObjType.TREES):
            flags |= in_trees
        if self.is_in_hazard(ball, GameObjType.SAND):
            flags |= in_sand

        if ball.Y < 0:
            flags |= passed_cup

        # less than 2, it's in the cup
        d = get_distance(
            Point(ball.X, ball.Y),
            Point(self.hole_geometry.cup.X, self.hole_geometry.cup.Y),
        )
        if d < 2:
            flags |= in_cup

        return flags

    def is_on_fairway(self, ball: Ball) -> bool:
        return is_in_rectangle(ball, self.hole_geometry.fairway)

    def is_on_green(self, ball: Ball) -> bool:
        d = get_distance(
            Point(ball.X, ball.Y),
            Point(self.hole_geometry.cup.X, self.hole_geometry.cup.Y),
        )
        return d < self.hole_geometry.green.Radius

    def hazard_hit(self, h: Hazard, ball: Ball, hazard: GameObjType) -> bool:
        d = get_distance(Point(ball.X, ball.Y), Point(h.X, h.Y))
        result = False
        if (d < h.Radius) and h.Type == hazard:
            result = True
        return result

    def is_in_hazard(self, ball: Ball, hazard: GameObjType) -> bool:
        result: bool = False
        for h in self.hole_geometry.hazards:
            result = result and self.hazard_hit(h, ball, hazard)
        return result

    def is_in_rough(self, ball: Ball) -> bool:
        return is_in_rectangle(ball, self.hole_geometry.rough) and (
            not is_in_rectangle(ball, self.hole_geometry.fairway)
        )

    def is_out_of_bounds(self, ball: Ball) -> bool:
        return (not self.is_on_fairway(ball)) and (not self.is_in_rough(ball))

    def score_card_start_new_hole(self) -> None:
        self.score_card.append([])

    def score_card_record_stroke(self, ball: Ball) -> None:
        clone = Ball(ball.X, ball.Y, 0, GameObjType.BALL)
        self.score_card[self.HOLE_NUM].append(clone)

    def score_card_get_previous_stroke(self) -> Ball:
        return self.score_card[self.HOLE_NUM][len(self.score_card[self.HOLE_NUM]) - 1]

    def score_card_get_total(self) -> int:
        total: int = 0
        for h in self.score_card:
            total += len(h)
        return total

    def ask(
        self, question: str, min_: int, max_: int, callback: Callable[[int], Any]
    ) -> None:
        # input from console is always an integer passed to a callback
        # or "quit" to end game
        print(question)
        i = input().strip().lower()
        if i == "quit":
            self.quit_game()
            return
        if i == "bag":
            self.review_bag()

        try:
            n = int(i)
            success = True
        except Exception:
            success = False
            n = 0

        if success:
            if n >= min_ and n <= max_:
                callback(n)
            else:
                self.ask(question, min_, max_, callback)
        else:
            self.ask(question, min_, max_, callback)

    def review_bag(self) -> None:
        print(" ")
        print("  #     Club      Average Yardage")
        print("-----------------------------------")
        print("  1    Driver           250")
        print("  2    3 Wood           225")
        print("  3    5 Wood           200")
        print("  4    Hybrid           190")
        print("  5    4 Iron           170")
        print("  6    7 Iron           150")
        print("  7    9 Iron           125")
        print("  8    Pitching wedge   110")
        print("  9    Sand wedge        75")
        print(" 10    Putter            10")
        print(" ")

    def quit_game(self) -> None:
        print("")
        print("Looks like rain. Goodbye!")
        print("")
        return

    def game_over(self) -> None:
        net = self.score_card_get_total() - self.handicap
        print("Good game!")
        print(f"Your net score is: {net}")
        print("Let's visit the pro shop...")
        print(" ")
        return


if __name__ == "__main__":
    Golf()
