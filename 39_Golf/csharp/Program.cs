//
//          8""""8 8"""88 8     8"""" 
//          8    " 8    8 8     8     
//          8e     8    8 8e    8eeee 
//          88  ee 8    8 88    88    
//          88   8 8    8 88    88    
//          88eee8 8eeee8 88eee 88    
//
// GOLF
//
// C#
// .NET Core
// TargetFramework: netcoreapp 3.1
//
// Run source:
// dotnet run
//
// Linux compile:
// dotnet publish --self-contained -c Release -r linux-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true
//
// Windows compile:
// dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
// 
//
// INDEX
// ----------------- methods
// constructor
// NewHole
// TeeUp
// Stroke
// PlotBall
// InterpretResults
// ReportCurrentScore
// FindBall
// IsOnFairway
// IsOnGreen
// IsInHazard
// IsInRough
// IsOutOfBounds
// ScoreCardNewHole
// ScoreCardRecordStroke
// ScoreCardGetPreviousStroke
// ScoreCardGetTotal
// Ask
// Wait
// ReviewBag
// Quit
// GameOver
// ----------------- DATA
// Clubs
// CourseInfo
// ----------------- classes
// HoleInfo
// CircleGameObj
// RectGameObj
// HoleGeometry
// Plot
// ----------------- helper methods
// GetDistance
// IsInRectangle
// ToRadians
// ToDegrees360
// Odds
//
//  Despite being a text based game, the code uses simple geometry to simulate a course. 
//  Fairways are 40 yard wide rectangles, surrounded by 5 yards of rough around the perimeter.
//  The green is a circle of 10 yards radius around the cup. 
//  The cup is always at point (0,0). 
//
//  Using basic trigonometry we can plot the ball's location using the distance of the stroke and
//  and the angle of deviation (hook/slice). 
//
//  The stroke distances are based on real world averages of different club types. 
//  Lots of randomization, "business rules", and luck influence the game play.
//  Probabilities are commented in the code. 
//
//  note: 'courseInfo', 'clubs', & 'scoreCard' arrays each include an empty object so indexing
//  can begin at 1. Like all good programmers we count from zero, but in this context,
//  it's more natural when hole number one is at index one
//
//            
//     |-----------------------------|   
//     |            rough            |
//     |   ----------------------    | 
//     |   |                     |   | 
//     | r |        =  =         | r | 
//     | o |     =        =      | o | 
//     | u |    =    .     =     | u | 
//     | g |    =   green  =     | g |  
//     | h |     =        =      | h | 
//     |   |        =  =         |   | 
//     |   |                     |   | 
//     |   |                     |   | 
//     |   |      Fairway        |   | 
//     |   |                     |   | 
//     |   |               ------    |
//     |   |            --        -- |
//     |   |           --  hazard  --|
//     |   |            --        -- |
//     |   |               ------    |
//     |   |                     |   | 
//     |   |                     |   |   out
//     |   |                     |   |   of
//     |   |                     |   |   bounds
//     |   |                     |   |
//     |   |                     |   |     
//     |            tee              |  
//                  
//
//  Typical green size: 20-30 yards
//  Typical golf course fairways are 35 to 45 yards wide          
//  Our fairway extends 5 yards past green
//  Our rough is a 5 yard perimeter around fairway
//  
//  We calculate the new position of the ball given the ball's point, the distance
//  of the stroke, and degrees off line (hook or slice).
//
//  Degrees off (for a right handed golfer):
//  Slice: positive degrees = ball goes right 
//  Hook: negative degrees = left goes left
//  
//  The cup is always at point: 0,0.
//  We use atan2 to compute the angle between the cup and the ball.
//  Setting the cup's vector to 0,-1 on a 360 circle is equivalent to: 
//  0 deg = 12 o'clock;  90 deg = 3 o'clock;  180 deg = 6 o'clock;  270 = 9 o'clock
//  The reverse angle between the cup and the ball is a difference of PI (using radians).
//
//  Given the angle and stroke distance (hypotenuse), we use cosine to compute
//  the opposite and adjacent sides of the triangle, which, is the ball's new position.
//    
//           0
//           |
//    270 - cup - 90
//           |
//          180
//    
//    
//          cup
//           |
//           |
//           | opp
//           |-----* new position
//           |    /
//           |   /
//      adj  |  /
//           | /  hyp
//           |/
//          tee
//
//    <- hook    slice ->  
//
//
//  Given the large number of combinations needed to describe a particular stroke / ball location,
//  we use the technique of "bitwise masking" to describe stroke results. 
//  With bit masking, multiple flags (bits) are combined into a single binary number that can be
//  tested by applying a mask. A mask is another binary number that isolates a particular bit that
//  you are interested in. You can then apply your language's bitwise opeartors to test or
//  set a flag. 
//
//  Game design by Jason Bonthron, 2021
//  www.bonthron.com
//  for my father, Raymond Bonthron, an avid golfer 
//
//  Inspired by the 1978 "Golf" from "Basic Computer Games"
//  by Steve North, who modified an existing golf game by an unknown author
//
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;


namespace Golf
{
    using Ball = Golf.CircleGameObj;
    using Hazard = Golf.CircleGameObj;
    
    // --------------------------------------------------------------------------- Program
    class Program
    {
        static void Main(string[] args)
        {
            Golf g = new Golf();
        }
    }

    
    // --------------------------------------------------------------------------- Golf
    public class Golf
    {
        Ball BALL;
        int HOLE_NUM = 0;
        int STROKE_NUM = 0;
        int Handicap = 0;
        int PlayerDifficulty = 0;
        HoleGeometry holeGeometry;

        // all fairways are 40 yards wide, extend 5 yards beyond the cup, and
        // have 5 yards of rough around the perimeter
        const int FairwayWidth = 40;
        const int FairwayExtension = 5;
        const int RoughAmt = 5;

        // ScoreCard records the ball position after each stroke
        // a new list for each hole
        // include a blank list so index 1 == hole 1

        List<List<Ball>> ScoreCard = new List<List<Ball>> { new List<Ball>() };

        static void w(string s) { Console.WriteLine(s); } // WRITE        
        Random RANDOM = new Random();


        // --------------------------------------------------------------- constructor
        public Golf()
        {
            Console.Clear();
            w(" ");
            w("          8\"\"\"\"8 8\"\"\"88 8     8\"\"\"\" ");
            w("          8    \" 8    8 8     8     ");
            w("          8e     8    8 8e    8eeee ");
            w("          88  ee 8    8 88    88    ");
            w("          88   8 8    8 88    88    ");
            w("          88eee8 8eeee8 88eee 88    ");
            w(" ");
            w("Welcome to the Creative Computing Country Club,");
            w("an eighteen hole championship layout located a short");
            w("distance from scenic downtown Lambertville, New Jersey.");
            w("The game will be explained as you play.");
            w("Enjoy your game! See you at the 19th hole...");
            w(" ");
            w("Type QUIT at any time to leave the game.");
            w("Type BAG at any time to review the clubs in your bag.");
            w(" ");

            Wait((z) =>
            {
                w(" ");
                w("              YOUR BAG");                
                ReviewBag();
                w("Type BAG at any time to review the clubs in your bag.");
                w(" ");

                Wait((zz) =>
                {
                    w(" ");
                    
                    Ask("PGA handicaps range from 0 to 30.\nWhat is your handicap?", 0, 30, (i) =>
                    {
                        Handicap = i;
                        w(" ");

                        Ask("Common difficulties at golf include:\n1=Hook, 2=Slice, 3=Poor Distance, 4=Trap Shots, 5=Putting\nWhich one is your worst?", 1, 5, (j) =>
                        {
                            PlayerDifficulty = j;
                            Console.Clear();
                            NewHole();
                        });
                    });
                });
            });
        }


        // --------------------------------------------------------------- NewHole
        void NewHole()
        {
            HOLE_NUM++;
            STROKE_NUM = 0;

            HoleInfo info = CourseInfo[HOLE_NUM];

            int yards = info.Yards;  // from tee to cup
            int par = info.Par;
            var cup = new CircleGameObj(0, 0, 0, GameObjType.CUP);
            var green = new CircleGameObj(0, 0, 10, GameObjType.GREEN);

            var fairway = new RectGameObj(0 - (FairwayWidth / 2),
                                          0 - (green.Radius + FairwayExtension),
                                          FairwayWidth,
                                          yards + (green.Radius + FairwayExtension) + 1,
                                          GameObjType.FAIRWAY);

            var rough = new RectGameObj(fairway.X - RoughAmt,
                                        fairway.Y - RoughAmt,
                                        fairway.Width + (2 * RoughAmt),
                                        fairway.Length + (2 * RoughAmt),
                                        GameObjType.ROUGH);

            BALL = new Ball(0, yards, 0, GameObjType.BALL);

            ScoreCardStartNewHole();

            holeGeometry = new HoleGeometry(cup, green, fairway, rough, info.Hazard);

            w("                |> " + HOLE_NUM);
            w("                |        ");
            w("                |        ");
            w("          ^^^^^^^^^^^^^^^");

            Console.WriteLine("Hole #{0}. You are at the tee. Distance {1} yards, par {2}.", HOLE_NUM, info.Yards, info.Par);
            w(info.Description);

            TeeUp();
        }


        // --------------------------------------------------------------- TeeUp
        // on the green? automatically select putter
        // otherwise Ask club and swing strength

        void TeeUp()
        {
            if (IsOnGreen(BALL) && !IsInHazard(BALL, GameObjType.SAND))
            {
                var putt = 10;
                w("[PUTTER: average 10 yards]");
                var msg = Odds(20) ? "Keep your head down.\n" : "";

                Ask(msg + "Choose your putt potency. (1-10)", 1, 10, (strength) =>
                {
                    var putter = Clubs[putt];
                    Stroke(Convert.ToDouble((double)putter.Item2 * ((double)strength / 10.0)), putt);
                });
            }
            else
            {
                Ask("What club do you choose? (1-10)", 1, 10, (c) =>
                {
                    var club = Clubs[c];

                    w(" ");
                    Console.WriteLine("[{0}: average {1} yards]", club.Item1.ToUpper(), club.Item2);

                    Ask("Now gauge your distance by a percentage of a full swing. (1-10)", 1, 10, (strength) =>
                    {
                        Stroke(Convert.ToDouble((double)club.Item2 * ((double)strength / 10.0)), c);
                    });
                });
            };
        }

        
        // -------------------------------------------------------- bitwise Flags
        int dub         = 0b00000000000001;
        int hook        = 0b00000000000010;
        int slice       = 0b00000000000100;
        int passedCup   = 0b00000000001000;
        int inCup       = 0b00000000010000;
        int onFairway   = 0b00000000100000;
        int onGreen     = 0b00000001000000;
        int inRough     = 0b00000010000000;
        int inSand      = 0b00000100000000;
        int inTrees     = 0b00001000000000;
        int inWater     = 0b00010000000000;
        int outOfBounds = 0b00100000000000;
        int luck        = 0b01000000000000;
        int ace         = 0b10000000000000;


        // --------------------------------------------------------------- Stroke
        void Stroke(double clubAmt, int clubIndex)
        {
            STROKE_NUM++;

            var flags = 0b000000000000;

            // fore! only when driving
            if ((STROKE_NUM == 1) && (clubAmt > 210) && Odds(30)) { w("\"...Fore !\""); };

            // dub
            if (Odds(5)) { flags |= dub; }; // there's always a 5% chance of dubbing it

            // if you're in the rough, or sand, you really should be using a wedge
            if ((IsInRough(BALL) || IsInHazard(BALL, GameObjType.SAND)) &&
                !(clubIndex == 8 || clubIndex == 9))
            {
                if (Odds(40)) { flags |= dub; };
            };

            // trap difficulty
            if (IsInHazard(BALL, GameObjType.SAND) && PlayerDifficulty == 4)
            {
                if (Odds(20)) { flags |= dub; };
            }

            // hook/slice
            // There's 10% chance of a hook or slice
            // if it's a known playerDifficulty then increase chance to 30%
            // if it's a putt & putting is a playerDifficulty increase to 30%

            bool randHookSlice = (PlayerDifficulty == 1 ||
                                  PlayerDifficulty == 2 ||
                                  (PlayerDifficulty == 5 && IsOnGreen(BALL))) ? Odds(30) : Odds(10);

            if (randHookSlice)
            {
                if (PlayerDifficulty == 1)
                {
                    if (Odds(80)) { flags |= hook; } else { flags |= slice; };
                }
                else if (PlayerDifficulty == 2)
                {
                    if (Odds(80)) { flags |= slice; } else { flags |= hook; };
                }
                else
                {
                    if (Odds(50)) { flags |= hook; } else { flags |= slice; };
                };
            };

            // beginner's luck !
            // If handicap is greater than 15, there's a 10% chance of avoiding all errors 
            if ((Handicap > 15) && (Odds(10))) { flags |= luck; };

            // ace
            // there's a 10% chance of an Ace on a par 3           
            if (CourseInfo[HOLE_NUM].Par == 3 && Odds(10) && STROKE_NUM == 1) { flags |= ace; };

            // distance:
            // If handicap is < 15, there a 50% chance of reaching club average,
            // a 25% of exceeding it, and a 25% of falling short 
            // If handicap is > 15, there's a 25% chance of reaching club average,
            // and 75% chance of falling short
            // The greater the handicap, the more the ball falls short
            // If poor distance is a known playerDifficulty, then reduce distance by 10% 

            double distance;
            int rnd = RANDOM.Next(1, 101);
             
            if (Handicap < 15)
            {
                if (rnd <= 25)
                {
                    distance = clubAmt - (clubAmt * ((double)Handicap / 100.0));
                }
                else if (rnd > 25 && rnd <= 75)
                {
                    distance = clubAmt;
                }
                else
                {
                    distance = clubAmt + (clubAmt * 0.10);
                };
            }
            else
            {
                if (rnd <= 75)
                {
                    distance = clubAmt - (clubAmt * ((double)Handicap / 100.0));
                }
                else
                {
                    distance = clubAmt;
                };
            };

            if (PlayerDifficulty == 3)  // poor distance
            {
                if (Odds(80)) { distance = (distance * 0.80); };
            };

            if ((flags & luck) == luck) { distance = clubAmt; }

            // angle
            // For all strokes, there's a possible "drift" of 4 degrees 
            // a hooks or slice increases the angle between 5-10 degrees, hook uses negative degrees
            int angle = RANDOM.Next(0, 5);
            if ((flags & slice) == slice) { angle = RANDOM.Next(5, 11); };
            if ((flags & hook) == hook) { angle = 0 - RANDOM.Next(5, 11); };
            if ((flags & luck) == luck) { angle = 0; };

            var plot = PlotBall(BALL, distance, Convert.ToDouble(angle));  // calculate a new location
            if ((flags & luck) == luck) { if(plot.Y > 0){ plot.Y = 2; }; };

            flags = FindBall(new Ball(plot.X, plot.Y, plot.Offline, GameObjType.BALL), flags);

            InterpretResults(plot, flags);
        }


        // --------------------------------------------------------------- plotBall
        Plot PlotBall(Ball ball, double strokeDistance, double degreesOff)
        {
            var cupVector = new Point(0, -1);
            double radFromCup = Math.Atan2((double)ball.Y, (double)ball.X) - Math.Atan2((double)cupVector.Y, (double)cupVector.X);
            double radFromBall = radFromCup - Math.PI;

            var hypotenuse = strokeDistance;
            var adjacent = Math.Cos(radFromBall + ToRadians(degreesOff)) * hypotenuse;
            var opposite = Math.Sqrt(Math.Pow(hypotenuse, 2) - Math.Pow(adjacent, 2));

            Point newPos;
            if (ToDegrees360(radFromBall + ToRadians(degreesOff)) > 180)
            {
                newPos = new Point(Convert.ToInt32(ball.X - opposite),
                                   Convert.ToInt32(ball.Y - adjacent));
            }
            else
            {
                newPos = new Point(Convert.ToInt32(ball.X + opposite),
                                   Convert.ToInt32(ball.Y - adjacent));
            }

            return new Plot(newPos.X, newPos.Y, Convert.ToInt32(opposite));
        }


        // --------------------------------------------------------------- InterpretResults
        void InterpretResults(Plot plot, int flags)
        {
            int cupDistance = Convert.ToInt32(GetDistance(new Point(plot.X, plot.Y),
                                                          new Point(holeGeometry.Cup.X, holeGeometry.Cup.Y)));
            int travelDistance = Convert.ToInt32(GetDistance(new Point(plot.X, plot.Y),
                                                             new Point(BALL.X, BALL.Y)));

            w(" ");

            if ((flags & ace) == ace)
            {
                w("Hole in One! You aced it.");
                ScoreCardRecordStroke(new Ball(0, 0, 0, GameObjType.BALL));
                ReportCurrentScore();
                return;
            };

            if ((flags & inTrees) == inTrees)
            {
                w("Your ball is lost in the trees. Take a penalty stroke.");
                ScoreCardRecordStroke(BALL);
                TeeUp();
                return;
            };

            if ((flags & inWater) == inWater)
            {
                var msg = Odds(50) ? "Your ball has gone to a watery grave." : "Your ball is lost in the water.";
                w(msg + " Take a penalty stroke.");
                ScoreCardRecordStroke(BALL);
                TeeUp();
                return;
            };

            if ((flags & outOfBounds) == outOfBounds)
            {
                w("Out of bounds. Take a penalty stroke.");
                ScoreCardRecordStroke(BALL);
                TeeUp();
                return;
            };

            if ((flags & dub) == dub)
            {
                w("You dubbed it.");
                ScoreCardRecordStroke(BALL);
                TeeUp();
                return;
            };

            if ((flags & inCup) == inCup)
            {
                var msg = Odds(50) ? "You holed it." : "It's in!";
                w(msg);
                ScoreCardRecordStroke(new Ball(plot.X, plot.Y, 0, GameObjType.BALL));
                ReportCurrentScore();
                return;
            };

            if (((flags & slice) == slice) &&
                !((flags & onGreen) == onGreen))
            {
                var bad = ((flags & outOfBounds) == outOfBounds) ? " badly" : "";
                Console.WriteLine("You sliced{0}: {1} yards offline.", bad, plot.Offline);
            };

            if (((flags & hook) == hook) &&
                !((flags & onGreen) == onGreen))
            {
                var bad = ((flags & outOfBounds) == outOfBounds) ? " badly" : "";
                Console.WriteLine("You hooked{0}: {1} yards offline.", bad, plot.Offline);
            };

            if (STROKE_NUM > 1)
            {
                var prevBall = ScoreCardGetPreviousStroke();
                var d1 = GetDistance(new Point(prevBall.X, prevBall.Y),
                                     new Point(holeGeometry.Cup.X, holeGeometry.Cup.Y));
                var d2 = cupDistance;
                if (d2 > d1) { w("Too much club."); };
            };

            if ((flags & inRough) == inRough) { w("You're in the rough."); };

            if ((flags & inSand) == inSand) { w("You're in a sand trap."); };

            if ((flags & onGreen) == onGreen)
            {
                var pd = (cupDistance < 4) ? ((cupDistance * 3) + " feet") : (cupDistance + " yards");
                Console.WriteLine("You're on the green. It's {0} from the pin.", pd);
            };

            if (((flags & onFairway) == onFairway) ||
                ((flags & inRough) == inRough))
            {
                Console.WriteLine("Shot went {0} yards. It's {1} yards from the cup.", travelDistance, cupDistance);
            };

            ScoreCardRecordStroke(new Ball(plot.X, plot.Y, 0, GameObjType.BALL));

            BALL = new Ball(plot.X, plot.Y, 0, GameObjType.BALL);

            TeeUp();
        }

        
        // --------------------------------------------------------------- ReportCurrentScore
        void ReportCurrentScore()
        {
            var par = CourseInfo[HOLE_NUM].Par;
            if (ScoreCard[HOLE_NUM].Count == par + 1) { w("A bogey. One above par."); };
            if (ScoreCard[HOLE_NUM].Count == par) { w("Par. Nice."); };
            if (ScoreCard[HOLE_NUM].Count == (par - 1)) { w("A birdie! One below par."); };
            if (ScoreCard[HOLE_NUM].Count == (par - 2)) { w("An Eagle! Two below par."); };
            if (ScoreCard[HOLE_NUM].Count == (par - 3)) { w("Double Eagle! Unbelievable."); };
            
            int totalPar = 0;
            for (var i = 1; i <= HOLE_NUM; i++) { totalPar += CourseInfo[i].Par; };

            w(" ");
            w("-----------------------------------------------------");            
            Console.WriteLine(" Total par for {0} hole{1} is: {2}. Your total is: {3}.",
                              HOLE_NUM,
                              ((HOLE_NUM > 1) ? "s" : ""), //plural    
                              totalPar,
                              ScoreCardGetTotal());
            w("-----------------------------------------------------");            
            w(" ");

            if (HOLE_NUM == 18)
            {
                GameOver();
            }
            else
            {
                Thread.Sleep(2000);
                NewHole();
            };
        }

        
        // --------------------------------------------------------------- FindBall
        int FindBall(Ball ball, int flags)
        {
            if (IsOnFairway(ball) && !IsOnGreen(ball)) { flags |= onFairway; }
            if (IsOnGreen(ball)) { flags |= onGreen; }
            if (IsInRough(ball)) { flags |= inRough; }
            if (IsOutOfBounds(ball)) { flags |= outOfBounds; }
            if (IsInHazard(ball, GameObjType.WATER)) { flags |= inWater; }
            if (IsInHazard(ball, GameObjType.TREES)) { flags |= inTrees; }
            if (IsInHazard(ball, GameObjType.SAND))  { flags |= inSand;  }
            
            if (ball.Y < 0) { flags |= passedCup; }

            // less than 2, it's in the cup
            var d = GetDistance(new Point(ball.X, ball.Y),
                                new Point(holeGeometry.Cup.X, holeGeometry.Cup.Y));
            if (d < 2) { flags |= inCup; };

            return flags;
        }

        
        // --------------------------------------------------------------- IsOnFairway
        bool IsOnFairway(Ball ball)
        {
            return IsInRectangle(ball, holeGeometry.Fairway);
        }

        
        // --------------------------------------------------------------- IsOngreen
        bool IsOnGreen(Ball ball)
        {
            var d = GetDistance(new Point(ball.X, ball.Y),
                                new Point(holeGeometry.Cup.X, holeGeometry.Cup.Y));
            return d < holeGeometry.Green.Radius;
        }

        
        // --------------------------------------------------------------- IsInHazard
        bool IsInHazard(Ball ball, GameObjType hazard)
        {
            bool result = false;
            Array.ForEach(holeGeometry.Hazards, (Hazard h) =>
            {
                var d = GetDistance(new Point(ball.X, ball.Y), new Point(h.X, h.Y));
                if ((d < h.Radius) && h.Type == hazard) { result = true; };
            });
            return result;
        }

        
        // --------------------------------------------------------------- IsInRough
        bool IsInRough(Ball ball)
        {
            return IsInRectangle(ball, holeGeometry.Rough) &&
                (IsInRectangle(ball, holeGeometry.Fairway) == false);
        }

        
        // --------------------------------------------------------------- IsOutOfBounds
        bool IsOutOfBounds(Ball ball)
        {
            return (IsOnFairway(ball) == false) && (IsInRough(ball) == false);
        }


        // --------------------------------------------------------------- ScoreCardNewHole
        void ScoreCardStartNewHole()
        {
            ScoreCard.Add(new List<Ball>());
        }


        // --------------------------------------------------------------- ScoreCardRecordStroke 
        void ScoreCardRecordStroke(Ball ball)
        {
            var clone = new Ball(ball.X, ball.Y, 0, GameObjType.BALL);
            ScoreCard[HOLE_NUM].Add(clone);
        }


        // ------------------------------------------------------------ ScoreCardGetPreviousStroke
        Ball ScoreCardGetPreviousStroke()
        {
            return ScoreCard[HOLE_NUM][ScoreCard[HOLE_NUM].Count - 1];
        }


        // --------------------------------------------------------------- ScoreCardGetTotal
        int ScoreCardGetTotal()
        {
            int total = 0;
            ScoreCard.ForEach((h) => { total += h.Count; });
            return total;
        }


        // --------------------------------------------------------------- Ask
        // input from console is always an integer passed to a callback
        // or "quit" to end game

        void Ask(string question, int min, int max, Action<int> callback)
        {
            w(question);
            string i = Console.ReadLine().Trim().ToLower();
            if (i == "quit") { Quit(); return; };
            if (i == "bag") { ReviewBag(); };

            int n;
            bool success = Int32.TryParse(i, out n);

            if (success)
            {
                if (n >= min && n <= max)
                {
                    callback(n);
                }
                else
                {
                    Ask(question, min, max, callback);
                }
            }
            else
            {
                Ask(question, min, max, callback);
            };
        }


        // --------------------------------------------------------------- Wait
        void Wait(Action<int> callback)
        {
            w("Press any key to continue.");
            
            ConsoleKeyInfo keyinfo;
            do { keyinfo = Console.ReadKey(true); }
            while (keyinfo.KeyChar < 0);
            Console.Clear();
            callback(0);
        }


        // --------------------------------------------------------------- ReviewBag
        void ReviewBag()
        {
            w(" ");
            w("  #     Club      Average Yardage");
            w("-----------------------------------");
            w("  1    Driver           250");
            w("  2    3 Wood           225");
            w("  3    5 Wood           200");
            w("  4    Hybrid           190");
            w("  5    4 Iron           170");
            w("  6    7 Iron           150");
            w("  7    9 Iron           125");
            w("  8    Pitching wedge   110");
            w("  9    Sand wedge        75");
            w(" 10    Putter            10");
            w(" ");
        }


        // --------------------------------------------------------------- Quit
        void Quit()
        {
            w("");
            w("Looks like rain. Goodbye!");
            w("");
            Wait((z) => { });
            return;
        }


        // --------------------------------------------------------------- GameOver
        void GameOver()
        {
            var net = ScoreCardGetTotal() - Handicap;
            w("Good game!");
            w("Your net score is: " + net);            
            w("Let's visit the pro shop...");
            w(" ");
            Wait((z) => { });            
            return;
        }


        // YOUR BAG
        // ======================================================== Clubs
        (string, int)[] Clubs = new (string, int)[] {
            ("",0),
                
                // name, average yardage                
                ("Driver", 250),
                ("3 Wood", 225),
                ("5 Wood", 200),
                ("Hybrid", 190),
                ("4 Iron", 170),
                ("7 Iron", 150),
                ("9 Iron", 125),
                ("Pitching wedge", 110),
                ("Sand wedge", 75),
                ("Putter", 10)
                };


        // THE COURSE
        // ======================================================== CourseInfo

        HoleInfo[] CourseInfo = new HoleInfo[]{
            new HoleInfo(0, 0, 0, new Hazard[]{}, ""), // include a blank so index 1 == hole 1

            
            // -------------------------------------------------------- front 9
            // hole, yards, par, hazards, (description)

            new HoleInfo(1, 361, 4,
                         new Hazard[]{
                             new Hazard( 20, 100, 10, GameObjType.TREES),
                             new Hazard(-20,  80, 10, GameObjType.TREES),
                             new Hazard(-20, 100, 10, GameObjType.TREES)
                         },
                         "There are a couple of trees on the left and right."),

            new HoleInfo(2, 389, 4,
                         new Hazard[]{
                             new Hazard(0, 160, 20, GameObjType.WATER)
                         },
                         "There is a large water hazard across the fairway about 150 yards."),

            new HoleInfo(3, 206, 3,
                         new Hazard[]{
                             new Hazard( 20,  20,  5, GameObjType.WATER),
                             new Hazard(-20, 160, 10, GameObjType.WATER),
                             new Hazard( 10,  12,  5, GameObjType.SAND)
                         },
                         "There is some sand and water near the green."),

            new HoleInfo(4, 500, 5,
                         new Hazard[]{
                             new Hazard(-14, 12, 12, GameObjType.SAND)
                         },
                         "There's a bunker to the left of the green."),

            new HoleInfo(5, 408, 4,
                         new Hazard[]{
                             new Hazard(20, 120, 20, GameObjType.TREES),
                             new Hazard(20, 160, 20, GameObjType.TREES),
                             new Hazard(10,  20,  5, GameObjType.SAND)
                         },
                         "There are some trees to your right."),

            new HoleInfo(6, 359, 4,
                         new Hazard[]{
                             new Hazard( 14, 0, 4, GameObjType.SAND),
                             new Hazard(-14, 0, 4, GameObjType.SAND)
                         },
                         ""),

            new HoleInfo(7, 424, 5,
                         new Hazard[]{
                             new Hazard(20, 200, 10, GameObjType.SAND),
                             new Hazard(10, 180, 10, GameObjType.SAND),
                             new Hazard(20, 160, 10, GameObjType.SAND)
                         },
                         "There are several sand traps along your right."),

            new HoleInfo(8, 388, 4,
                         new Hazard[]{
                             new Hazard(-20, 340, 10, GameObjType.TREES)
                         },
                         ""),

            new HoleInfo(9, 196, 3,
                         new Hazard[]{
                             new Hazard(-30, 180, 20, GameObjType.TREES),
                             new Hazard( 14,  -8,  5, GameObjType.SAND)
                         },
                         ""), 
            
            // -------------------------------------------------------- back 9
            // hole, yards, par, hazards, (description)

            new HoleInfo(10, 400, 4,
                         new Hazard[]{
                             new Hazard(-14, -8, 5, GameObjType.SAND),
                             new Hazard( 14, -8, 5, GameObjType.SAND)
                         },
                         ""),

            new HoleInfo(11, 560, 5,
                         new Hazard[]{
                             new Hazard(-20, 400, 10, GameObjType.TREES),
                             new Hazard(-10, 380, 10, GameObjType.TREES),
                             new Hazard(-20, 260, 10, GameObjType.TREES),
                             new Hazard(-20, 200, 10, GameObjType.TREES),
                             new Hazard(-10, 180, 10, GameObjType.TREES),
                             new Hazard(-20, 160, 10, GameObjType.TREES)
                         },
                         "Lots of trees along the left of the fairway."),

            new HoleInfo(12, 132, 3,
                         new Hazard[]{
                             new Hazard(-10, 120, 10, GameObjType.WATER),
                             new Hazard( -5, 100, 10, GameObjType.SAND)
                         },
                         "There is water and sand directly in front of you. A good drive should clear both."),

            new HoleInfo(13, 357, 4,
                         new Hazard[]{
                             new Hazard(-20, 200, 10, GameObjType.TREES),
                             new Hazard(-10, 180, 10, GameObjType.TREES),
                             new Hazard(-20, 160, 10, GameObjType.TREES),
                             new Hazard( 14,  12,  8, GameObjType.SAND)
                         },
                         ""),

            new HoleInfo(14, 294, 4,
                         new Hazard[]{
                             new Hazard(0, 20, 10, GameObjType.SAND)
                         },
                         ""),

            new HoleInfo(15, 475, 5,
                         new Hazard[]{
                             new Hazard(-20, 20, 10, GameObjType.WATER),
                             new Hazard( 10, 20, 10, GameObjType.SAND)
                         },
                         "Some sand and water near the green."),

            new HoleInfo(16, 375, 4,
                         new Hazard[]{
                             new Hazard(-14, -8, 5, GameObjType.SAND)
                         },
                         ""),

            new HoleInfo(17, 180, 3,
                         new Hazard[]{
                             new Hazard( 20, 100, 10, GameObjType.TREES),
                             new Hazard(-20,  80, 10, GameObjType.TREES)
                         },
                         ""),

            new HoleInfo(18, 550, 5,
                         new Hazard[]{
                             new Hazard(20, 30, 15, GameObjType.WATER)
                         },
                         "There is a water hazard near the green.")
        };

        
        // -------------------------------------------------------- HoleInfo
        class HoleInfo
        {
            public int Hole { get; }
            public int Yards { get; }
            public int Par { get; }
            public Hazard[] Hazard { get; }
            public string Description { get; }

            public HoleInfo(int hole, int yards, int par, Hazard[] hazard, string description)
            {
                Hole = hole;
                Yards = yards;
                Par = par;
                Hazard = hazard;
                Description = description;
            }
        }


        public enum GameObjType { BALL, CUP, GREEN, FAIRWAY, ROUGH, TREES, WATER, SAND }


        // -------------------------------------------------------- CircleGameObj
        public class CircleGameObj
        {
            public GameObjType Type { get; }
            public int X { get; }
            public int Y { get; }
            public int Radius { get; }

            public CircleGameObj(int x, int y, int r, GameObjType type)
            {
                Type = type;
                X = x;
                Y = y;
                Radius = r;
            }
        }


        // -------------------------------------------------------- RectGameObj
        public class RectGameObj
        {
            public GameObjType Type { get; }
            public int X { get; }
            public int Y { get; }
            public int Width { get; }
            public int Length { get; }

            public RectGameObj(int x, int y, int w, int l, GameObjType type)
            {
                Type = type;
                X = x;
                Y = y;
                Width = w;
                Length = l;
            }
        }


        // -------------------------------------------------------- HoleGeometry
        public class HoleGeometry
        {
            public CircleGameObj Cup { get; }
            public CircleGameObj Green { get; }
            public RectGameObj Fairway { get; }
            public RectGameObj Rough { get; }
            public Hazard[] Hazards { get; }

            public HoleGeometry(CircleGameObj cup, CircleGameObj green, RectGameObj fairway, RectGameObj rough, Hazard[] haz)
            {
                Cup = cup;
                Green = green;
                Fairway = fairway;
                Rough = rough;
                Hazards = haz;
            }
        }


        // -------------------------------------------------------- Plot
        public class Plot
        {
            public int X { get; }
            public int Y { get; set; }
            public int Offline { get; }

            public Plot(int x, int y, int offline)
            {
                X = x;
                Y = y;
                Offline = offline;
            }
        }


        // -------------------------------------------------------- GetDistance
        // distance between 2 points
        double GetDistance(Point pt1, Point pt2)
        {
            return Math.Sqrt(Math.Pow((pt2.X - pt1.X), 2) + Math.Pow((pt2.Y - pt1.Y), 2));
        }


        // -------------------------------------------------------- IsInRectangle
        bool IsInRectangle(CircleGameObj pt, RectGameObj rect)
        {
            return ((pt.X > rect.X) &&
                    (pt.X < rect.X + rect.Width) &&
                    (pt.Y > rect.Y) &&
                    (pt.Y < rect.Y + rect.Length));
        }

        
        // -------------------------------------------------------- ToRadians
        double ToRadians(double angle) { return angle * (Math.PI / 180.0); }


        // -------------------------------------------------------- ToDegrees360
        // radians to 360 degrees
        double ToDegrees360(double angle)
        {
            double deg = angle * (180.0 / Math.PI);
            if (deg < 0.0) { deg += 360.0; }
            return deg;
        }


        // -------------------------------------------------------- Odds
        // chance an integer is <= the given argument
        // between 1-100
        Random RND = new Random();

        bool Odds(int x)
        {
            return RND.Next(1, 101) <= x;
        }
    }
}
