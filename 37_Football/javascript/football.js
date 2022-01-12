// FOOTBALL
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

var player_data = [17,8,4,14,19,3,10,1,7,11,15,9,5,20,13,18,16,2,12,6,
                   20,2,17,5,8,18,12,11,1,4,19,14,10,7,9,15,6,13,16,3];
var aa = [];
var ba = [];
var ca = [];
var ha = [];
var ta = [];
var wa = [];
var xa = [];
var ya = [];
var za = [];
var ms = [];
var da = [];
var ps = [, "PITCHOUT","TRIPLE REVERSE","DRAW","QB SNEAK","END AROUND",
          "DOUBLE REVERSE","LEFT SWEEP","RIGHT SWEEP","OFF TACKLE",
          "WISHBONE OPTION","FLARE PASS","SCREEN PASS",
          "ROLL OUT OPTION","RIGHT CURL","LEFT CURL","WISHBONE OPTION",
          "SIDELINE PASS","HALF-BACK OPTION","RAZZLE-DAZZLE","BOMB!!!!"];
var p;
var t;

function field_headers()
{
    print("TEAM 1 [0   10   20   30   40   50   60   70   80   90");
    print("   100] TEAM 2\n");
    print("\n");
}

function separator()
{
    str = "";
    for (x = 1; x <= 72; x++)
        str += "+";
    print(str + "\n");
}

function show_ball()
{
    print(tab(da[t] + 5 + p / 2) + ms[t] + "\n");
    field_headers();
}

function show_scores()
{
    print("\n");
    print("TEAM 1 SCORE IS " + ha[1] + "\n");
    print("TEAM 2 SCORE IS " + ha[2] + "\n");
    print("\n");
    if (ha[t] >= e) {
        print("TEAM " + t + " WINS*******************");
        return true;
    }
    return false;
}

function loss_posession() {
    print("\n");
    print("** LOSS OF POSSESSION FROM TEAM " + t + " TO TEAM " + ta[t] + "\n");
    print("\n");
    separator();
    print("\n");
    t = ta[t];
}

function touchdown() {
    print("\n");
    print("TOUCHDOWN BY TEAM " + t + " *********************YEA TEAM\n");
    q = 7;
    g = Math.random();
    if (g <= 0.1) {
        q = 6;
        print("EXTRA POINT NO GOOD\n");
    } else {
        print("EXTRA POINT GOOD\n");
    }
    ha[t] = ha[t] + q;
}

// Main program
async function main()
{
    print(tab(32) + "FOOTBALL\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("PRESENTING N.F.U. FOOTBALL (NO FORTRAN USED)\n");
    print("\n");
    print("\n");
    while (1) {
        print("DO YOU WANT INSTRUCTIONS");
        str = await input();
        if (str == "YES" || str == "NO")
            break;
    }
    if (str == "YES") {
        print("THIS IS A FOOTBALL GAME FOR TWO TEAMS IN WHICH PLAYERS MUST\n");
        print("PREPARE A TAPE WITH A DATA STATEMENT (1770 FOR TEAM 1,\n");
        print( "1780 FOR TEAM 2) IN WHICH EACH TEAM SCRAMBLES NOS. 1-20\n");
        print("THESE NUMBERS ARE THEN ASSIGNED TO TWENTY GIVEN PLAYS.\n");
        print("A LIST OF NOS. AND THEIR PLAYS IS PROVIDED WITH\n");
        print("BOTH TEAMS HAVING THE SAME PLAYS. THE MORE SIMILAR THE\n");
        print("PLAYS THE LESS YARDAGE GAINED.  SCORES ARE GIVEN\n");
        print("WHENEVER SCORES ARE MADE. SCORES MAY ALSO BE OBTAINED\n");
        print("BY INPUTTING 99,99 FOR PLAY NOS. TO PUNT OR ATTEMPT A\n");
        print("FIELD GOAL, INPUT 77,77 FOR PLAY NUMBERS. QUESTIONS WILL BE\n");
        print("ASKED THEN. ON 4TH DOWN, YOU WILL ALSO BE ASKED WHETHER\n");
        print("YOU WANT TO PUNT OR ATTEMPT A FIELD GOAL. IF THE ANSWER TO\n");
        print("BOTH QUESTIONS IS NO IT WILL BE ASSUMED YOU WANT TO\n");
        print("TRY AND GAIN YARDAGE. ANSWER ALL QUESTIONS YES OR NO.\n");
        print("THE GAME IS PLAYED UNTIL PLAYERS TERMINATE (CONTROL-C).\n");
        print("PLEASE PREPARE A TAPE AND RUN.\n");
    }
    print("\n");
    print("PLEASE INPUT SCORE LIMIT ON GAME");
    e = parseInt(await input());
    for (i = 1; i <= 40; i++) {
        if (i <= 20) {
            aa[player_data[i - 1]] = i;
        } else {
            ba[player_data[i - 1]] = i - 20;
        }
        ca[i] = player_data[i - 1];
    }
    l = 0;
    t = 1;
    do {
        print("TEAM " + t + " PLAY CHART\n");
        print("NO.      PLAY\n");
        for (i = 1; i <= 20; i++) {
            str = "" + ca[i + l];
            while (str.length < 6)
                str += " ";
            str += ps[i];
            print(str + "\n");
        }
        l += 20;
        t = 2;
        print("\n");
        print("TEAR OFF HERE----------------------------------------------\n");
        for (x = 1; x <= 11; x++)
            print("\n");
    } while (l == 20) ;
    da[1] = 0;
    da[2] = 3;
    ms[1] = "--->";
    ms[2] = "<---";
    ha[1] = 0;
    ha[2] = 0;
    ta[1] = 2;
    ta[2] = 1;
    wa[1] = -1;
    wa[2] = 1;
    xa[1] = 100;
    xa[2] = 0;
    ya[1] = 1;
    ya[2] = -1;
    za[1] = 0;
    za[2] = 100;
    p = 0;
    field_headers();
    print("TEAM 1 DEFEND 0 YD GOAL -- TEAM 2 DEFENDS 100 YD GOAL.\n");
    t = Math.floor(2 * Math.random() + 1);
    print("\n");
    print("THE COIN IS FLIPPED\n");
    routine = 1;
    while (1) {
        if (routine <= 1) {
            p = xa[t] - ya[t] * 40;
            separator();
            print("\n");
            print("TEAM " + t + " RECEIVES KICK-OFF\n");
            k = Math.floor(26 * Math.random() + 40);
        }
        if (routine <= 2) {
            p = p - ya[t] * k;
        }
        if (routine <= 3) {
            if (wa[t] * p >= za[t] + 10) {
                print("\n");
                print("BALL WENT OUT OF ENDZONE --AUTOMATIC TOUCHBACK--\n");
                p = za[t] - wa[t] * 20;
                if (routine <= 4)
                    routine = 5;
            } else {
                print("BALL WENT " + k + " YARDS.  NOW ON " + p + "\n");
                show_ball();
            }
        }
        if (routine <= 4) {
            while (1) {
                print("TEAM " + t + " DO YOU WANT TO RUNBACK");
                str = await input();
                if (str == "YES" || str == "NO")
                    break;
            }
            if (str == "YES") {
                k = Math.floor(9 * Math.random() + 1);
                r = Math.floor(((xa[t] - ya[t] * p + 25) * Math.random() - 15) / k);
                p = p - wa[t] * r;
                print("\n");
                print("RUNBACK TEAM " + t + " " + r + " YARDS\n");
                g = Math.random();
                if (g < 0.25) {
                    loss_posession();
                    routine = 4;
                    continue;
                } else if (ya[t] * p >= xa[t]) {
                    touchdown();
                    if (show_scores())
                        return;
                    t = ta[t];
                    routine = 1;
                    continue;
                } else if (wa[t] * p >= za[t]) {
                    print("\n");
                    print("SAFETY AGAINST TEAM " + t + " **********************OH-OH\n");
                    ha[ta[t]] = ha[ta[t]] + 2;
                    if (show_scores())
                        return;
                    print("TEAM " + t + " DO YOU WANT TO PUNT INSTEAD OF A KICKOFF");
                    str = await input();
                    p = za[t] - wa[t] * 20;
                    if (str == "YES") {
                        print("\n");
                        print("TEAM " + t + " WILL PUNT\n");
                        g = Math.random();
                        if (g < 0.25) {
                            loss_posession();
                            routine = 4;
                            continue;
                        }
                        print("\n");
                        separator();
                        k = Math.floor(25 * Math.random() + 35);
                        t = ta[t];
                        routine = 2;
                        continue;
                    }
                    touchdown();
                    if (show_scores())
                        return;
                    t = ta[t];
                    routine = 1;
                    continue;
                } else {
                    routine = 5;
                    continue;
                }
            } else if (str == "NO") {
                if (wa[t] * p >= za[t])
                    p = za[t] - wa[t] * 20;
            }
        }
        if (routine <= 5) {
            d = 1;
            s = p;
        }
        if (routine <= 6) {
            str = "";
            for (i = 1; i <= 72; i++)
                str += "=";
            print(str + "\n");
            print("TEAM " + t + " DOWN " + d + " ON " + p + "\n");
            if (d == 1) {
                if (ya[t] * (p + ya[t] * 10) >= xa[t])
                    c = 8;
                else
                    c = 4;
            }
            if (c != 8) {
                print(tab(27) + (10 - (ya[t] * p - ya[t] * s)) + " YARDS TO 1ST DOWN\n");
            } else {
                print(tab(27) + (xa[t] - ya[t] * p) + " YARDS\n");
            }
            show_ball();
            if (d == 4)
                routine = 8;
        }
        if (routine <= 7) {
            u = Math.floor(3 * Math.random() - 1);
            while (1) {
                print("INPUT OFFENSIVE PLAY, DEFENSIVE PLAY");
                str = await input();
                if (t == 1) {
                    p1 = parseInt(str);
                    p2 = parseInt(str.substr(str.indexOf(",") + 1));
                } else {
                    p2 = parseInt(str);
                    p1 = parseInt(str.substr(str.indexOf(",") + 1));
                }
                if (p1 == 99) {
                    if (show_scores())
                        return;
                    if (p1 == 99)
                        continue;
                }
                if (p1 < 1 || p1 > 20 || p2 < 1 || p2 > 20) {
                    print("ILLEGAL PLAY NUMBER, CHECK AND\n");
                    continue;
                }
                break;
            }
        }
        if (d == 4 || p1 == 77) {
            while (1) {
                print("DOES TEAM " + t + " WANT TO PUNT");
                str = await input();
                if (str == "YES" || str == "NO")
                    break;
            }
            if (str == "YES") {
                print("\n");
                print("TEAM " + t + " WILL PUNT\n");
                g = Math.random();
                if (g < 0.25) {
                    loss_posession();
                    routine = 4;
                    continue;
                }
                print("\n");
                separator();
                k = Math.floor(25 * Math.random() + 35);
                t = ta[t];
                routine = 2;
                continue;
            }
            while (1) {
                print("DOES TEAM " + t + " WANT TO ATTEMPT A FIELD GOAL");
                str = await input();
                if (str == "YES" || str == "NO")
                    break;
            }
            if (str == "YES") {
                print("\n");
                print("TEAM " + t + " WILL ATTEMPT A FIELD GOAL\n");
                g = Math.random();
                if (g < 0.025) {
                    loss_posession();
                    routine = 4;
                    continue;
                } else {
                    f = Math.floor(35 * Math.random() + 20);
                    print("\n");
                    print("KICK IS " + f + " YARDS LONG\n");
                    p = p - wa[t] * f;
                    g = Math.random();
                    if (g < 0.35) {
                        print("BALL WENT WIDE\n");
                    } else if (ya[t] * p >= xa[t]) {
                        print("FIELD GOLD GOOD FOR TEAM " + t + " *********************YEA");
                        q = 3;
                        ha[t] = ha[t] + q;
                        if (show_scores())
                            return;
                        t = ta[t];
                        routine = 1;
                        continue;
                    }
                    print("FIELD GOAL UNSUCCESFUL TEAM " + t + "-----------------TOO BAD\n");
                    print("\n");
                    separator();
                    if (ya[t] * p < xa[t] + 10) {
                        print("\n");
                        print("BALL NOW ON " + p + "\n");
                        t = ta[t];
                        show_ball();
                        routine = 4;
                        continue;
                    } else {
                        t = ta[t];
                        routine = 3;
                        continue;
                    }
                }
            } else {
                routine = 7;
                continue;
            }
        }
        y = Math.floor(Math.abs(aa[p1] - ba[p2]) / 19 * ((xa[t] - ya[t] * p + 25) * Math.random() - 15));
        print("\n");
        if (t == 1 && aa[p1] < 11 || t == 2 && ba[p2] < 11) {
            print("THE BALL WAS RUN\n");
        } else if (u == 0) {
            print("PASS INCOMPLETE TEAM " + t + "\n");
            y = 0;
        } else {
            g = Math.random();
            if (g <= 0.025 && y > 2) {
                print("PASS COMPLETED\n");
            } else {
                print("QUARTERBACK SCRAMBLED\n");
            }
        }
        p = p - wa[t] * y;
        print("\n");
        print("NET YARDS GAINED ON DOWN " + d + " ARE " + y + "\n");
        
        g = Math.random();
        if (g <= 0.025) {
            loss_posession();
            routine = 4;
            continue;
        } else if (ya[t] * p >= xa[t]) {
            touchdown();
            if (show_scores())
                return;
            t = ta[t];
            routine = 1;
            continue;
        } else if (wa[t] * p >= za[t]) {
            print("\n");
            print("SAFETY AGAINST TEAM " + t + " **********************OH-OH\n");
            ha[ta[t]] = ha[ta[t]] + 2;
            if (show_scores())
                return;
            print("TEAM " + t + " DO YOU WANT TO PUNT INSTEAD OF A KICKOFF");
            str = await input();
            p = za[t] - wa[t] * 20;
            if (str == "YES") {
                print("\n");
                print("TEAM " + t + " WILL PUNT\n");
                g = Math.random();
                if (g < 0.25) {
                    loss_posession();
                    routine = 4;
                    continue;
                }
                print("\n");
                separator();
                k = Math.floor(25 * Math.random() + 35);
                t = ta[t];
                routine = 2;
                continue;
            }
            touchdown();
            if (show_scores())
                return;
            t = ta[t];
            routine = 1;
        } else if (ya[t] * p - ya[t] * s >= 10) {
            routine = 5;
        } else {
            d++;
            if (d != 5) {
                routine = 6;
            } else {
                print("\n");
                print("CONVERSION UNSUCCESSFUL TEAM " + t + "\n");
                t = ta[t];
                print("\n");
                separator();
                routine = 5;
            }
        }
    }
}

main();
