// GOLF
//
// Converted from BASIC to Javascript by Oscar Toledo G. (nanochess)
//

function print(str)
{
    document.getElementById("output").appendChild(document.createTextNode(str));
}

function input()
{
    var input_element;
    var input_str;

    return new Promise(function (resolve) {
                       input_element = document.createElement("INPUT");

                       print("? ");
                       input_element.setAttribute("type", "text");
                       input_element.setAttribute("length", "50");
                       document.getElementById("output").appendChild(input_element);
                       input_element.focus();
                       input_str = undefined;
                       input_element.addEventListener("keydown", function (event) {
                                                      if (event.keyCode == 13) {
                                                      input_str = input_element.value;
                                                      document.getElementById("output").removeChild(input_element);
                                                      print(input_str);
                                                      print("\n");
                                                      resolve(input_str);
                                                      }
                                                      });
                       });
}

function tab(space)
{
    var str = "";
    while (space-- > 0)
        str += " ";
    return str;
}

var la = [];
var f;
var s1;
var g2;
var g3;
var x;

var hole_data = [
    361,4,4,2,389,4,3,3,206,3,4,2,500,5,7,2,
    408,4,2,4,359,4,6,4,424,4,4,2,388,4,4,4,
    196,3,7,2,400,4,7,2,560,5,7,2,132,3,2,2,
    357,4,4,4,294,4,2,4,475,5,2,3,375,4,4,2,
    180,3,6,2,550,5,6,6,
];

function show_obstacle()
{
    switch (la[x]) {
        case 1:
            print("FAIRWAY.\n");
            break;
        case 2:
            print("ROUGH.\n");
            break;
        case 3:
            print("TREES.\n");
            break;
        case 4:
            print("ADJACENT FAIRWAY.\n");
            break;
        case 5:
            print("TRAP.\n");
            break;
        case 6:
            print("WATER.\n");
            break;
    }
}

function show_score()
{
    g2 += s1;
    print("TOTAL PAR FOR " + (f - 1) + " HOLES IS " + g3 + "  YOUR TOTAL IS " + g2 + "\n");
}

// Main program
async function main()
{
    print(tab(34) + "GOLF\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("WELCOME TO THE CREATIVE COMPUTING COUNTRY CLUB,\n");
    print("AN EIGHTEEN HOLE CHAMPIONSHIP LAYOUT LOCATED A SHORT\n");
    print("DISTANCE FROM SCENIC DOWNTOWN MORRISTOWN.  THE\n");
    print("COMMENTATOR WILL EXPLAIN THE GAME AS YOU PLAY.\n");
    print("ENJOY YOUR GAME; SEE YOU AT THE 19TH HOLE...\n");
    print("\n");
    print("\n");
    next_hole = 0;
    g1 = 18;
    g2 = 0;
    g3 = 0;
    a = 0;
    n = 0.8;
    s2 = 0;
    f = 1;
    while (1) {
        print("WHAT IS YOUR HANDICAP");
        h = parseInt(await input());
        print("\n");
        if (h < 0 || h > 30) {
            print("PGA HANDICAPS RANGE FROM 0 TO 30.\n");
        } else {
            break;
        }
    }
    do {
        print("DIFFICULTIES AT GOLF INCLUDE:\n");
        print("0=HOOK, 1=SLICE, 2=POOR DISTANCE, 4=TRAP SHOTS, 5=PUTTING\n");
        print("WHICH ONE (ONLY ONE) IS YOUR WORST");
        t = parseInt(await input());
        print("\n");
    } while (t > 5) ;
    s1 = 0;
    first_routine = true;
    while (1) {
        if (first_routine) {
            la[0] = 0;
            j = 0;
            q = 0;
            s2++;
            k = 0;
            if (f != 1) {
                print("YOUR SCORE ON HOLE " + (f - 1) + " WAS " + s1 + "\n");
                show_score();
                if (g1 == f - 1)    // Completed all holes?
                    return;         // Exit game
                if (s1 > p + 2) {
                    print("KEEP YOUR HEAD DOWN.\n");
                } else if (s1 == p) {
                    print("A PAR.  NICE GOING.\n");
                } else if (s1 == p - 1) {
                    print("A BIRDIE.\n");
                } else if (s1 == p - 2) {
                    if (p != 3)
                        print("A GREAT BIG EAGLE.\n");
                    else
                        print("A HOLE IN ONE.\n");
                }
            }
            if (f == 19) {
                print("\n");
                show_score();
                if (g1 == f - 1)
                    return;
            }
            s1 = 0;
            print("\n");
            if (s1 != 0 && la[0] < 1)
                la[0] = 1;
        }
        if (s1 == 0) {
            d = hole_data[next_hole++];
            p = hole_data[next_hole++];
            la[1] = hole_data[next_hole++];
            la[2] = hole_data[next_hole++];
            print("\n");
            print("YOU ARE AT THE TEE OFF HOLE " + f + " DISTANCE " + d + " YARDS, PAR " + p + "\n");
            g3 += p;
            print("ON YOUR RIGHT IS ");
            x = 1;
            show_obstacle();
            print("ON YOUR LEFT IS ");
            x = 2
            show_obstacle();
        } else {
            x = 0;
            if (la[0] > 5) {
                if (la[0] > 6) {
                    print("YOUR SHOT WENT OUT OF BOUNDS.\n");
                } else {
                    print("YOUR SHOT WENT INTO THE WATER.\n");
                }
                s1++;
                print("PENALTY STROKE ASSESSED.  HIT FROM PREVIOUS LOCATION.\n");
                j++;
                la[0] = 1;
                d = b;
            } else {
                print("SHOT WENT " + d1 + " YARDS.  IT'S " + d2 + " YARDS FROM THE CUP.\n");
                print("BALL IS " + Math.floor(o) + " YARDS OFF LINE... IN ");
                show_obstacle();
            }
        }

        while (1) {
            if (a != 1) {
                print("SELECTION OF CLUBS\n");
                print("YARDAGE DESIRED                       SUGGESTED CLUBS\n");
                print("200 TO 280 YARDS                           1 TO 4\n");
                print("100 TO 200 YARDS                          19 TO 13\n");
                print("  0 TO 100 YARDS                          29 TO 23\n");
                a = 1;
            }
            print("WHAT CLUB DO YOU CHOOSE");
            c = parseInt(await input());
            print("\n");
            if (c >= 1 && c <= 29 && (c < 5 || c >= 12)) {
                if (c > 4)
                    c -= 6;
                if (la[0] <= 5 || c == 14 || c == 23) {
                    s1++;
                    w = 1;
                    if (c <= 13) {
                        if (f % 3 == 0 && s2 + q + (10 * (f - 1) / 18) < (f - 1) * (72 + ((h + 1) / 0.85)) / 18) {
                            q++;
                            if (s1 % 2 != 0 && d >= 95) {
                                print("BALL HIT TREE - BOUNCED INTO ROUGH " + (d - 75) + " YARDS FROM HOLE.\n");
                                d -= 75;
                                continue;
                            }
                            print("YOU DUBBED IT.\n");
                            d1 = 35;
                            second_routine = 1;
                            break;
                        } else if (c < 4 && la[0] == 2) {
                            print("YOU DUBBED IT.\n");
                            d1 = 35;
                            second_routine = 1;
                            break;
                        } else {
                            second_routine = 0;
                            break;
                        }
                    } else {
                        print("NOW GAUGE YOUR DISTANCE BY A PERCENTAGE (1 TO 100)\n");
                        print("OF A FULL SWING");
                        w = parseInt(await input());
                        w /= 100;
                        print("\n");
                        if (w <= 1) {
                            if (la[0] == 5) {
                                if (t == 3) {
                                    if (Math.random() <= n) {
                                        n *= 0.2;
                                        print("SHOT DUBBED, STILL IN TRAP.\n");
                                        continue;
                                    }
                                    n = 0.8;
                                }
                                d2 = 1 + (3 * Math.floor((80 / (40 - h)) * Math.random()));
                                second_routine = 2;
                                break;
                            }
                            if (c != 14)
                                c -= 10;
                            second_routine = 0;
                            break;
                        }
                        s1--;
                        // Fall through to THAT CLUB IS NOT IN THE BAG.
                    }
                }
            }
            print("THAT CLUB IS NOT IN THE BAG.\n");
            print("\n");
        }
        if (second_routine == 0) {
            if (s1 > 7 && d < 200) {
                d2 = 1 + (3 * Math.floor((80 / (40 - h)) * Math.random()));
                second_routine = 2;
            } else {
                d1 = Math.floor(((30 - h) * 2.5 + 187 - ((30 - h) * 0.25 + 15) * c / 2) + 25 * Math.random());
                d1 = Math.floor(d1 * w);
                if (t == 2)
                    d1 = Math.floor(d1 * 0.85);
            }
        }
        if (second_routine <= 1) {
            o = (Math.random() / 0.8) * (2 * h + 16) * Math.abs(Math.tan(d1 * 0.0035));
            d2 = Math.floor(Math.sqrt(Math.pow(o, 2) + Math.pow(Math.abs(d - d1), 2)));
            if (d - d1 < 0) {
                if (d2 >= 20)
                    print("TOO MUCH CLUB, YOU'RE PAST THE HOLE.\n");
            }
            b = d;
            d = d2;
            if (d2 > 27) {
                if (o < 30 || j > 0) {
                    la[0] = 1;
                } else {
                    if (t <= 0) {
                        s9 = (s2 + 1) / 15;
                        if (Math.floor(s9) == s9) {
                            print("YOU SLICED- ");
                            la[0] = la[1];
                        } else {
                            print("YOU HOOKED- ");
                            la[0] = la[2];
                        }
                    } else {
                        s9 = (s2 + 1) / 15;
                        if (Math.floor(s9) == s9) {
                            print("YOU HOOKED- ");
                            la[0] = la[2];
                        } else {
                            print("YOU SLICED- ");
                            la[0] = la[1];
                        }
                    }
                    if (o > 45)
                        print("BADLY.\n");
                }
                first_routine = false;
            } else if (d2 > 20) {
                la[0] = 5;
                first_routine = false;
            } else if (d2 > 0.5) {
                la[0] = 8;
                d2 = Math.floor(d2 * 3);
                second_routine = 2;
            } else {
                la[0] = 9;
                print("YOU HOLED IT.\n");
                print("\n");
                f++;
                first_routine = true;
            }
        }
        if (second_routine == 2) {
            while (1) {
                print("ON GREEN, " + d2 + " FEET FROM THE PIN.\n");
                print("CHOOSE YOUR PUTT POTENCY (1 TO 13):");
                i = parseInt(await input());
                s1++;
                if (s1 + 1 - p <= (h * 0.072) + 2 && k <= 2) {
                    k++;
                    if (t == 4)
                        d2 -= i * (4 + 1 * Math.random()) + 1;
                    else
                        d2 -= i * (4 + 2 * Math.random()) + 1.5;
                    if (d2 < -2) {
                        print("PASSED BY CUP.\n");
                        d2 = Math.floor(-d2);
                        continue;
                    }
                    if (d2 > 2) {
                        print("PUTT SHORT.\n");
                        d2 = Math.floor(d2);
                        continue;
                    }
                }
                print("YOU HOLED IT.\n");
                print("\n");
                f++;
                break;
            }
            first_routine = true;
        }
    }
}

main();
