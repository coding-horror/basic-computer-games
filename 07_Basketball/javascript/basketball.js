// BASKETBALL
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

var s = [0, 0];
var z;
var d;
var p;
var your_turn;
var game_restart;

function two_minutes()
{
    print("\n");
    print("   *** TWO MINUTES LEFT IN THE GAME ***\n");
    print("\n");
}

function show_scores()
{
    print("SCORE: " + s[1] + " TO " + s[0] + "\n");
}

function score_computer()
{
    s[0] = s[0] + 2;
    show_scores();
}

function score_player()
{
    s[1] = s[1] + 2;
    show_scores();
}

function half_time()
{
    print("\n");
    print("   ***** END OF FIRST HALF *****\n");
    print("SCORE: DARMOUTH: " + s[1] + "  " + os + ": " + s[0] + "\n");
    print("\n");
    print("\n");
}

function foul()
{
    if (Math.random() <= 0.49) {
        print("SHOOTER MAKES BOTH SHOTS.\n");
        s[1 - p] = s[1 - p] + 2;
        show_scores();
    } else if (Math.random() <= 0.75) {
        print("SHOOTER MAKES ONE SHOT AND MISSES ONE.\n");
        s[1 - p] = s[1 - p] + 1;
        show_scores();
    } else {
        print("BOTH SHOTS MISSED.\n");
        show_scores();
    }
}

function player_play()
{
    if (z == 1 || z == 2) {
        t++;
        if (t == 50) {
            half_time();
            game_restart = 1;
            return;
        }
        if (t == 92)
            two_minutes();
        print("JUMP SHOT\n");
        if (Math.random() <= 0.341 * d / 8) {
            print("SHOT IS GOOD.\n");
            score_player();
            return;
        }
        if (Math.random() <= 0.682 * d / 8) {
            print("SHOT IS OFF TARGET.\n");
            if (d / 6 * Math.random() >= 0.45) {
                print("REBOUND TO " + os + "\n");
                return;
            }
            print("DARTMOUTH CONTROLS THE REBOUND.\n");
            if (Math.random() > 0.4) {
                if (d == 6) {
                    if (Math.random() > 0.6) {
                        print("PASS STOLEN BY " + os + " EASY LAYUP.\n");
                        score_computer();
                        return;
                    }
                }
                print("BALL PASSED BACK TO YOU. ");
                your_turn = 1;
                return;
            }
        } else if (Math.random() <= 0.782 * d / 8) {
            print("SHOT IS BLOCKED.  BALL CONTROLLED BY ");
            if (Math.random() <= 0.5) {
                print("DARTMOUTH.\n");
                your_turn = 1;
                return;
            }
            print(os + ".\n");
            return;
        } else if (Math.random() <= 0.843 * d / 8) {
            print("SHOOTER IS FOULED.  TWO SHOTS.\n");
            foul();
            return;
            // In original code but lines 1180-1195 aren't used (maybe replicate from computer's play)
            //        } else if (Math.random() <= 0.9 * d / 8) {
            //            print("PLAYER FOULED, TWO SHOTS.\n");
            //            foul();
            //            return;
        } else {
            print("CHARGING FOUL.  DARTMOUTH LOSES BALL.\n");
            return;
        }
    }
    while (1) {
        if (++t == 50) {
            half_time();
            game_restart = 1;
            return;
        }
        if (t == 92)
            two_minutes();
        if (z == 0) {
            your_turn = 2;
            return;
        }
        if (z <= 3)
            print("LAY UP.\n");
        else
            print("SET SHOT.\n");
        if (7 / d * Math.random() <= 0.4) {
            print("SHOT IS GOOD.  TWO POINTS.\n");
            score_player();
            return;
        }
        if (7 / d * Math.random() <= 0.7) {
            print("SHOT IS OFF THE RIM.\n");
            if (Math.random() <= 2.0 / 3.0) {
                print(os + " CONTROLS THE REBOUND.\n");
                return;
            }
            print("DARMOUTH CONTROLS THE REBOUND.\n");
            if (Math.random() <= 0.4)
                continue;
            print("BALL PASSED BACK TO YOU.\n");
            your_turn = 1;
            return;
        }
        if (7 /d * Math.random() <= 0.875) {
            print("SHOOTER FOULED.  TWO SHOTS.\n");
            foul();
            return;
        }
        if (7 /d * Math.random() <= 0.925) {
            print("SHOT BLOCKED. " + os + "'S BALL.\n");
            return;
        }
        print("CHARGING FOUL.  DARTHMOUTH LOSES THE BALL.\n");
        return;
    }
}

function computer_play()
{
    rebound = 0;
    while (1) {
        p = 1;
        if (++t == 50) {
            half_time();
            game_restart = 1;
            return;
        }
        print("\n");
        z1 = 10 / 4 * Math.random() + 1;
        if (z1 <= 2) {
            print("JUMP SHOT.\n");
            if (8 / d * Math.random() <= 0.35) {
                print("SHOT IS GOOD.\n");
                score_computer();
                return;
            }
            if (8 / d * Math.random() <= 0.75) {
                print("SHOT IS OFF RIM.\n");
                if (d / 6 * Math.random() <= 0.5) {
                    print("DARMOUTH CONTROLS THE REBOUND.\n");
                    return;
                }
                print(os + " CONTROLS THE REBOUND.\n");
                if (d == 6) {
                    if (Math.random() <= 0.75) {
                        print("BALL STOLEN.  EASY LAP UP FOR DARTMOUTH.\n");
                        score_player();
                        continue;
                    }
                    if (Math.random() > 0.6) {
                        print("PASS STOLEN BY " + os + " EASY LAYUP.\n");
                        score_computer();
                        return;
                    }
                    print("BALL PASSED BACK TO YOU. ");
                    return;
                }
                if (Math.random() <= 0.5) {
                    print("PASS BACK TO " + os + " GUARD.\n");
                    continue;
                }
            } else if (8 / d * Math.random() <= 0.90) {
                print("PLAYER FOULED.  TWO SHOTS.\n");
                foul();
                return;
            } else {
                print("OFFENSIVE FOUL.  DARTMOUTH'S BALL.\n");
                return;
            }
        }
        while (1) {
            if (z1 > 3) {
                print("SET SHOT.\n");
            } else {
                print("LAY UP.\n");
            }
            if (7 / d * Math.random() <= 0.413) {
                print("SHOT IS GOOD.\n");
                score_computer();
                return;
            }
            print("SHOT IS MISSED.\n");
            // Spaguetti jump, better to replicate code
            if (d / 6 * Math.random() <= 0.5) {
                print("DARMOUTH CONTROLS THE REBOUND.\n");
                return;
            }
            print(os + " CONTROLS THE REBOUND.\n");
            if (d == 6) {
                if (Math.random() <= 0.75) {
                    print("BALL STOLEN.  EASY LAP UP FOR DARTMOUTH.\n");
                    score_player();
                    break;
                }
                if (Math.random() > 0.6) {
                    print("PASS STOLEN BY " + os + " EASY LAYUP.\n");
                    score_computer();
                    return;
                }
                print("BALL PASSED BACK TO YOU. ");
                return;
            }
            if (Math.random() <= 0.5) {
                print("PASS BACK TO " + os + " GUARD.\n");
                break;
            }
        }
    }
}

// Main program
async function main()
{
    print(tab(31) + "BASKETBALL\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS IS DARTMOUTH COLLEGE BASKETBALL.  YOU WILL BE DARTMOUTH\n");
    print(" CAPTAIN AND PLAYMAKER.  CALL SHOTS AS FOLLOWS:  1. LONG\n");
    print(" (30 FT.) JUMP SHOT; 2. SHORT (15 FT.) JUMP SHOT; 3. LAY\n");
    print(" UP; 4. SET SHOT.\n");
    print("BOTH TEAMS WILL USE THE SAME DEFENSE.  CALL DEFENSE AS\n");
    print("FOLLOWS:  6. PRESS; 6.5 MAN-TO MAN; 7. ZONE; 7.5 NONE.\n");
    print("TO CHANGE DEFENSE, JUST TYPE 0 AS YOUR NEXT SHOT.\n");
    print("YOUR STARTING DEFENSE WILL BE");
    t = 0;
    p = 0;
    d = parseFloat(await input());
    if (d < 6) {
        your_turn = 2;
    } else {
        print("\n");
        print("CHOOSE YOUR OPPONENT");
        os = await input();
        game_restart = 1;
    }
    while (1) {
        if (game_restart) {
            game_restart = 0;
            print("CENTER JUMP\n");
            if (Math.random() > 3.0 / 5.0) {
                print("DARMOUTH CONTROLS THE TAP.\n");
            } else {
                print(os + " CONTROLS THE TAP.\n");
                computer_play();
            }
        }
        if (your_turn == 2) {
            print("YOUR NEW DEFENSIVE ALLIGNMENT IS");
            d = parseFloat(await input());
        }
        print("\n");
        while (1) {
            print("YOUR SHOT");
            z = parseInt(await input());
            p = 0;
            if (z != Math.floor(z) || z < 0 || z > 4)
                print("INCORRECT ANSWER.  RETYPE IT. ");
            else
                break;
        }
        if (Math.random() < 0.5 || t < 100) {
            game_restart = 0;
            your_turn = 0;
            player_play();
            if (game_restart == 0 && your_turn == 0)
                computer_play();
        } else {
            print("\n");
            if (s[1] == s[0]) {
                print("\n");
                print("   ***** END OF SECOND HALF *****\n");
                print("\n");
                print("SCORE AT END OF REGULATION TIME:\n");
                print("        DARTMOUTH: " + s[1] + "  " + os + ": " + s[0] + "\n");
                print("\n");
                print("BEGIN TWO MINUTE OVERTIME PERIOD\n");
                t = 93;
                print("CENTER JUMP\n");
                if (Math.random() > 3.0 / 5.0)
                    print("DARMOUTH CONTROLS THE TAP.\n");
                else
                    print(os + " CONTROLS THE TAP.\n");
            } else {
                print("   ***** END OF GAME *****\n");
                print("FINAL SCORE: DARMOUTH: " + s[1] + "  " + os + ": " + s[0] + "\n");
                break;
            }
        }
    }
}

main();
