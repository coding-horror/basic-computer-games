// FTBALL
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

var os = [];
var sa = [];
var ls = [, "KICK","RECEIVE"," YARD ","RUN BACK FOR ","BALL ON ",
          "YARD LINE"," SIMPLE RUN"," TRICKY RUN"," SHORT PASS",
          " LONG PASS","PUNT"," QUICK KICK "," PLACE KICK"," LOSS ",
          " NO GAIN","GAIN "," TOUCHDOWN "," TOUCHBACK ","SAFETY***",
          "JUNK"];
var p;
var x;
var x1;

function fnf(x)
{
    return 1 - 2 * p;
}

function fng(z)
{
    return p * (x1 - x) + (1 - p) * (x - x1);
}

function show_score()
{
    print("\n");
    print("SCORE:  " + sa[0] + " TO " + sa[1] + "\n");
    print("\n");
    print("\n");
}

function show_position()
{
    if (x <= 50) {
        print(ls[5] + os[0] + " " + x + " " + ls[6] + "\n");
    } else {
        print(ls[5] + os[1] + " " + (100 - x) + " " + ls[6] + "\n");
    }
}

function offensive_td()
{
    print(ls[17] + "***\n");
    if (Math.random() <= 0.8) {
        sa[p] = sa[p] + 7;
        print("KICK IS GOOD.\n");
    } else {
        print("KICK IS OFF TO THE SIDE\n");
        sa[p] = sa[p] + 6;
    }
    show_score();
    print(os[p] + " KICKS OFF\n");
    p = 1 - p;
}

// Main program
async function main()
{
    print(tab(33) + "FTBALL\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("THIS IS DARTMOUTH CHAMPIONSHIP FOOTBALL.\n");
    print("\n");
    print("YOU WILL QUARTERBACK DARTMOUTH. CALL PLAYS AS FOLLOWS:\n");
    print("1= SIMPLE RUN; 2= TRICKY RUN; 3= SHORT PASS;\n");
    print("4= LONG PASS; 5= PUNT; 6= QUICK KICK; 7= PLACE KICK.\n");
    print("\n");
    print("CHOOSE YOUR OPPONENT");
    os[1] = await input();
    os[0] = "DARMOUTH";
    print("\n");
    sa[0] = 0;
    sa[1] = 0;
    p = Math.floor(Math.random() * 2);
    print(os[p] + " WON THE TOSS\n");
    if (p != 0) {
        print(os[1] + " ELECTS TO RECEIVE.\n");
        print("\n");
    } else {
        print("DO YOU ELECT TO KICK OR RECEIVE");
        while (1) {
            str = await input();
            print("\n");
            if (str == ls[1] || str == ls[2])
                break;
            print("INCORRECT ANSWER.  PLEASE TYPE 'KICK' OR 'RECEIVE'");
        }
        e = (str == ls[1]) ? 1 : 2;
        if (e == 1)
            p = 1;
    }
    t = 0;
    start = 1;
    while (1) {
        if (start <= 1) {
            x = 40 + (1 - p) * 20;
        }
        if (start <= 2) {
            y = Math.floor(200 * Math.pow((Math.random() - 0.5), 3) + 55);
            print(" " + y + " " + ls[3] + " KICKOFF\n");
            x = x - fnf(1) * y;
            if (Math.abs(x - 50) >= 50) {
                print("TOUCHBACK FOR " + os[p] + ".\n");
                x = 20 + p * 60;
                start = 4;
            } else {
                start = 3;
            }
        }
        if (start <= 3) {
            y = Math.floor(50 * Math.pow(Math.random(), 2)) + (1 - p) * Math.floor(50 * Math.pow(Math.random(), 4));
            x = x + fnf(1) * y;
            if (Math.abs(x - 50) < 50) {
                print(" " + y + " " + ls[3] + " RUNBACK\n");
            } else {
                print(ls[4]);
                offensive_td();
                start = 1;
                continue;
            }
        }
        if (start <= 4) {
            // First down
            show_position();
        }
        if (start <= 5) {
            x1 = x;
            d = 1;
            print("\n");
            print("FIRST DOWN " + os[p] + "***\n");
            print("\n");
            print("\n");
        }
        // New play
        t++;
        if (t == 30) {
            if (Math.random() <= 1.3) {
                print("GAME DELAYED.  DOG ON FIELD.\n");
                print("\n");
            }
        }
        if (t >= 50 && Math.random() <= 0.2)
            break;
        if (p != 1) {
            // Opponent's play
            if (d <= 1) {
                z = Math.random() > 1 / 3 ? 1 : 3;
            } else if (d != 4) {
                if (10 + x - x1 < 5 || x < 5) {
                    z = Math.random() > 1 / 3 ? 1 : 3;
                } else if (x <= 10) {
                    a = Math.floor(2 * Math.random());
                    z = 2 + a;
                } else if (x <= x1 || d < 3 || x < 45) {
                    a = Math.floor(2 * Math.random());
                    z = 2 + a * 2;
                } else {
                    if (Math.random() > 1 / 4)
                        z = 4;
                    else
                        z = 6;
                }
            } else {
                if (x <= 30) {
                    z = 5;
                } else if (10 + x - x1 < 3 || x < 3) {
                    z = Math.random() > 1 / 3 ? 1 : 3;
                } else {
                    z = 7;
                }
            }
        } else {
            print("NEXT PLAY");
            while (1) {
                z = parseInt(await input());
                if (Math.abs(z - 4) <= 3)
                    break;
                print("ILLEGAL PLAY NUMBER, RETYPE");
            }
        }
        f = 0;
        print(ls[z + 6] + ".  ");
        r = Math.random() * (0.98 + fnf(1) * 0.02);
        r1 = Math.random();
        switch (z) {
            case 1: // Simple run
            case 2: // Tricky run
                if (z == 1) {
                    y = Math.floor(24 * Math.pow(r - 0.5, 3) + 3);
                    if (Math.random() >= 0.05) {
                        routine = 1;
                        break;
                    }
                } else {
                    y = Math.floor(20 * r - 5);
                    if (Math.random() > 0.1) {
                        routine = 1;
                        break;
                    }
                }
                f = -1;
                x3 = x;
                x = x + fnf(1) * y;
                if (Math.abs(x - 50) < 50) {
                    print("***  FUMBLE AFTER ");
                    routine = 2;
                    break;
                } else {
                    print("***  FUMBLE.\n");
                    routine = 4;
                    break;
                }
            case 3: // Short pass
            case 4: // Long pass
                if (z == 3) {
                    y = Math.floor(60 * Math.pow(r1 - 0.5, 3) + 10);
                } else {
                    y = Math.floor(160 * Math.pow((r1 - 0.5), 3) + 30);
                }
                if (z == 3 && r < 0.05 || z == 4 && r < 0.1) {
                    if (d != 4) {
                        print("INTERCEPTED.\n");
                        f = -1;
                        x = x + fnf(1) * y;
                        if (Math.abs(x - 50) >= 50) {
                            routine = 4;
                            break;
                        }
                        routine = 3;
                        break;
                    } else {
                        y = 0;
                        if (Math.random() < 0.3) {
                            print("BATTED DOWN.  ");
                        } else {
                            print("INCOMPLETE.  ");
                        }
                        routine = 1;
                        break;
                    }
                } else if (z == 4 && r < 0.3) {
                    print("PASSER TACKLED.  ");
                    y = -Math.floor(15 * r1 + 3);
                    routine = 1;
                    break;
                } else if (z == 3 && r < 0.15) {
                    print("PASSER TACLKED.  ");
                    y = -Math.floor(10 * r1);
                    routine = 1;
                    break;
                } else if (z == 3 && r < 0.55 || z == 4 && r < 0.75) {
                    y = 0;
                    if (Math.random() < 0.3) {
                        print("BATTED DOWN.  ");
                    } else {
                        print("INCOMPLETE.  ");
                    }
                    routine = 1;
                    break;
                } else {
                    print("COMPLETE.  ");
                    routine = 1;
                    break;
                }
            case 5:  // Punt
            case 6:  // Quick kick
                y = Math.floor(100 * Math.pow((r - 0.5), 3) + 35);
                if (d != 4)
                    y = Math.floor(y * 1.3);
                print(" " + y + " " + ls[3] + " PUNT\n");
                if (Math.abs(x + y * fnf(1) - 50) < 50 && d >= 4) {
                    y1 = Math.floor(Math.pow(r1, 2) * 20);
                    print(" " + y1 + " " + ls[3] + " RUN BACK\n");
                    y = y - y1;
                }
                f = -1;
                x = x + fnf(1) * y;
                if (Math.abs(x - 50) >= 50) {
                    routine = 4;
                    break;
                }
                routine = 3;
                break;
            case 7: // Place kick
                y = Math.floor(100 * Math.pow((r - 0.5), 3) + 35);
                if (r1 <= 0.15) {
                    print("KICK IS BLOCKED  ***\n");
                    x = x - 5 * fnf(1);
                    p = 1 - p;
                    start = 4;
                    continue;
                }
                x = x + fnf(1) * y;
                if (Math.abs(x - 50) >= 60) {
                    if (r1 <= 0.5) {
                        print("KICK IS OFF TO THE SIDE.\n");
                        print(ls[18] + "\n");
                        p = 1 - p;
                        x = 20 + p * 60;
                        start = 4;
                        continue;
                    } else {
                        print("FIELD GOAL ***\n");
                        sa[p] = sa[p] + 3;
                        show_score();
                        print(os[p] + " KICKS OFF\n");
                        p = 1 - p;
                        start = 1;
                        continue;
                    }
                } else {
                    print("KICK IS SHORT.\n");
                    if (Math.abs(x - 50) >= 50) {
                        // Touchback
                        print(ls[18] + "\n");
                        p = 1 - p;
                        x = 20 + p * 60;
                        start = 4;
                        continue;
                    }
                    p = 1 - p;
                    start = 3;
                    continue;
                }
                
        }
        // Gain or loss
        if (routine <= 1) {
            x3 = x;
            x = x + fnf(1) * y;
            if (Math.abs(x - 50) >= 50) {
                routine = 4;
            }
        }
        if (routine <= 2) {
            if (y != 0) {
                print(" " + Math.abs(y) + " " + ls[3]);
                if (y < 0)
                    yt = -1;
                else if (y > 0)
                    yt = 1;
                else
                    yt = 0;
                print(ls[15 + yt]);
                if (Math.abs(x3 - 50) <= 40 && Math.random() < 0.1) {
                    // Penalty
                    p3 = Math.floor(2 * Math.random());
                    print(os[p3] + " OFFSIDES -- PENALTY OF 5 YARDS.\n");
                    print("\n");
                    print("\n");
                    if (p3 != 0) {
                        print("DO YOU ACCEPT THE PENALTY");
                        while (1) {
                            str = await input();
                            if (str == "YES" || str == "NO")
                                break;
                            print("TYPE 'YES' OR 'NO'");
                        }
                        if (str == "YES") {
                            f = 0;
                            d = d - 1;
                            if (p != p3)
                                x = x3 + fnf(1) * 5;
                            else
                                x = x3 - fnf(1) * 5;
                        }
                    } else {
                        // Opponent's strategy on penalty
                        if ((p != 1 && (y <= 0 || f < 0 || fng(1) < 3 * d - 2))
                            || (p == 1 && ((y > 5 && f >= 0) || d < 4 || fng(1) >= 10))) {
                            print("PENALTY REFUSED.\n");
                        } else {
                            print("PENALTY ACCEPTED.\n");
                            f = 0;
                            d = d - 1;
                            if (p != p3)
                                x = x3 + fnf(1) * 5;
                            else
                                x = x3 - fnf(1) * 5;
                        }
                    }
                    routine = 3;
                }
            }
        }
        if (routine <= 3) {
            show_position();
            if (f != 0) {
                p = 1 - p;
                start = 5;
                continue;
            } else if (fng(1) >= 10) {
                start = 5;
                continue;
            } else if (d == 4) {
                p = 1 - p;
                start = 5;
                continue;
            } else {
                d++;
                print("DOWN: " + d + "     ");
                if ((x1 - 50) * fnf(1) >= 40) {
                    print("GOAL TO GO\n");
                } else {
                    print("YARDS TO GO: " + (10 - fng(1)) + "\n");
                }
                print("\n");
                print("\n");
                start = 6;
                continue;
            }
        }
        if (routine <= 4) {
            // Ball in end-zone
            e = (x >= 100) ? 1 : 0;
            switch (1 + e - f * 2 + p * 4) {
                case 1:
                case 5:
                    // Safety
                    sa[1 - p] = sa[1 - p] + 2;
                    print(ls[19] + "\n");
                    show_score();
                    print(os[p] + " KICKS OFF FROM ITS 20 YARD LINE.\n");
                    x = 20 + p * 60;
                    p = 1 - p;
                    start = 2;
                    continue;
                case 3:
                case 6:
                    // Defensive TD
                    print(ls[17] + "FOR " + os[1 - p] + "***\n");
                    p = 1 - p;
                    // Fall-thru
                case 2:
                case 8:
                    // Offensive TD
                    print(ls[17] + "***\n");
                    if (Math.random() <= 0.8) {
                        sa[p] = sa[p] + 7;
                        print("KICK IS GOOD.\n");
                    } else {
                        print("KICK IS OFF TO THE SIDE\n");
                        sa[p] = sa[p] + 6;
                    }
                    show_score();
                    print(os[p] + " KICKS OFF\n");
                    p = 1 - p;
                    start = 1;
                    continue;
                case 4:
                case 7:
                    // Touchback
                    print(ls[18] + "\n");
                    p = 1 - p;
                    x = 20 + p * 60;
                    start = 4;
                    continue;
            }
        }
    }
    print("END OF GAME  ***\n");
    print("FINAL SCORE:  " + os[0] + ": " + sa[0] + "  " + os[1] + ": " + sa[1] + "\n");
}

main();
