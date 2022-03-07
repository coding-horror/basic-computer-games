// HOCKEY
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

var as = [];
var bs = [];
var ha = [];
var ta = [];
var t1 = [];
var t2 = [];
var t3 = [];

// Main program
async function main()
{
    print(tab(33) + "HOCKEY\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    // Robert Puopolo Alg. 1 140 McCowan 6/7/73 Hockey
    for (c = 0; c <= 20; c++)
        ha[c] = 0;
    for (c = 1; c <= 5; c++) {
        ta[c] = 0;
        t1[c] = 0;
        t2[c] = 0;
        t3[c] = 0;
    }
    x = 1;
    print("\n");
    print("\n");
    print("\n");
    while (1) {
        print("WOULD YOU LIKE THE INSTRUCTIONS");
        str = await input();
        print("\n");
        if (str == "YES" || str == "NO")
            break;
        print("ANSWER YES OR NO!!\n");
    }
    if (str == "YES") {
        print("\n");
        print("THIS IS A SIMULATED HOCKEY GAME.\n");
        print("QUESTION     RESPONSE\n");
        print("PASS         TYPE IN THE NUMBER OF PASSES YOU WOULD\n");
        print("             LIKE TO MAKE, FROM 0 TO 3.\n");
        print("SHOT         TYPE THE NUMBER CORRESPONDING TO THE SHOT\n");
        print("             YOU WANT TO MAKE.  ENTER:\n");
        print("             1 FOR A SLAPSHOT\n");
        print("             2 FOR A WRISTSHOT\n");
        print("             3 FOR A BACKHAND\n");
        print("             4 FOR A SNAP SHOT\n");
        print("AREA         TYPE IN THE NUMBER CORRESPONDING TO\n");
        print("             THE AREA YOU ARE AIMING AT.  ENTER:\n");
        print("             1 FOR UPPER LEFT HAND CORNER\n");
        print("             2 FOR UPPER RIGHT HAND CORNER\n");
        print("             3 FOR LOWER LEFT HAND CORNER\n");
        print("             4 FOR LOWER RIGHT HAND CORNER\n");
        print("\n");
        print("AT THE START OF THE GAME, YOU WILL BE ASKED FOR THE NAMES\n");
        print("OF YOUR PLAYERS.  THEY ARE ENTERED IN THE ORDER: \n");
        print("LEFT WING, CENTER, RIGHT WING, LEFT DEFENSE,\n");
        print("RIGHT DEFENSE, GOALKEEPER.  ANY OTHER INPUT REQUIRED WILL\n");
        print("HAVE EXPLANATORY INSTRUCTIONS.\n");
    }
    print("ENTER THE TWO TEAMS");
    str = await input();
    c = str.indexOf(",");
    as[7] = str.substr(0, c);
    bs[7] = str.substr(c + 1);
    print("\n");
    do {
        print("ENTER THE NUMBER OF MINUTES IN A GAME");
        t6 = parseInt(await input());
        print("\n");
    } while (t6 < 1) ;
    print("\n");
    print("WOULD THE " + as[7] + " COACH ENTER HIS TEAM\n");
    print("\n");
    for (i = 1; i <= 6; i++) {
        print("PLAYER " + i + " ");
        as[i] = await input();
    }
    print("\n");
    print("WOULD THE " + bs[7] + " COACH DO THE SAME\n");
    print("\n");
    for (t = 1; t <= 6; t++) {
        print("PLAYER " + t + " ");
        bs[t] = await input();
    }
    print("\n");
    print("INPUT THE REFEREE FOR THIS GAME");
    rs = await input();
    print("\n");
    print(tab(10) + as[7] + " STARTING LINEUP\n");
    for (t = 1; t <= 6; t++) {
        print(as[t] + "\n");
    }
    print("\n");
    print(tab(10) + bs[7] + " STARTING LINEUP\n");
    for (t = 1; t <= 6; t++) {
        print(bs[t] + "\n");
    }
    print("\n");
    print("WE'RE READY FOR TONIGHTS OPENING FACE-OFF.\n");
    print(rs + " WILL DROP THE PUCK BETWEEN " + as[2] + " AND " + bs[2] + "\n");
    s2 = 0;
    s3 = 0;
    for (l = 1; l <= t6; l++) {
        c = Math.floor(2 * Math.random()) + 1;
        if (c == 1)
            print(as[7] + " HAS CONTROL OF THE PUCK\n");
        else
            print(bs[7] + " HAS CONTROL.\n");
        do {

            print("PASS");
            p = parseInt(await input());
            for (n = 1; n <= 3; n++)
                ha[n] = 0;
        } while (p < 0 || p > 3) ;
        do {
            for (j = 1; j <= p + 2; j++)
                ha[j] = Math.floor(5 * Math.random()) + 1;
        } while (ha[j - 1] == ha[j - 2] || (p + 2 >= 3 && (ha[j - 1] == ha[j - 3] || ha[j - 2] == ha[j - 3]))) ;
        if (p == 0) {
            while (1) {
                print("SHOT");
                s = parseInt(await input());
                if (s >= 1 && s <= 4)
                    break;
            }
            if (c == 1) {
                print(as[ha[j - 1]]);
                g = ha[j - 1];
                g1 = 0;
                g2 = 0;
            } else {
                print(bs[ha[j - 1]]);
                g2 = 0;
                g2 = 0;
                g = ha[j - 1];
            }
            switch (s) {
                case 1:
                    print(" LET'S A BOOMER GO FROM THE RED LINE!!\n");
                    z = 10;
                    break;
                case 2:
                    print(" FLIPS A WRISTSHOT DOWN THE ICE\n");
                    // Probable missing line 430 in original
                case 3:
                    print(" BACKHANDS ONE IN ON THE GOALTENDER\n");
                    z = 25;
                    break;
                case 4:
                    print(" SNAPS A LONG FLIP SHOT\n");
                    z = 17;
                    break;
            }
        } else {
            if (c == 1) {
                switch (p) {
                    case 1:
                        print(as[ha[j - 2]] + " LEADS " + as[ha[j - 1]] + " WITH A PERFECT PASS.\n");
                        print(as[ha[j - 1]] + " CUTTING IN!!!\n");
                        g = ha[j - 1];
                        g1 = ha[j - 2];
                        g2 = 0;
                        z1 = 3;
                        break;
                    case 2:
                        print(as[ha[j - 2]] + " GIVES TO A STREAKING " + as[ha[j - 1]] + "\n");
                        print(as[ha[j - 3]] + " COMES DOWN ON " + bs[5] + " AND " + bs[4] + "\n");
                        g = ha[j - 3];
                        g1 = ha[j - 1];
                        g2 = ha[j - 2];
                        z1 = 2;
                        break;
                    case 3:
                        print("OH MY GOD!! A ' 4 ON 2 ' SITUATION\n");
                        print(as[ha[j - 3]] + " LEADS " + as[ha[j - 2]] + "\n");
                        print(as[ha[j - 2]] + " IS WHEELING THROUGH CENTER.\n");
                        print(as[ha[j - 2]] + " GIVES AND GOEST WITH " + as[ha[j - 1]] + "\n");
                        print("PRETTY PASSING!\n");
                        print(as[ha[j - 1]] + " DROPS IT TO " + as[ha[j - 4]] + "\n");
                        g = ha[j - 4];
                        g1 = ha[j - 1];
                        g2 = ha[j - 2];
                        z1 = 1;
                        break;
                }
            } else {
                switch (p) {
                    case 1:
                        print(bs[ha[j - 1]] + " HITS " + bs[ha[j - 2]] + " FLYING DOWN THE LEFT SIDE\n");
                        g = ha[j - 2];
                        g1 = ha[j - 1];
                        g2 = 0;
                        z1 = 3;
                        break;
                    case 2:
                        print("IT'S A ' 3 ON 2 '!\n");
                        print("ONLY " + as[4] + " AND " + as[5] + " ARE BACK.\n");
                        print(bs[ha[j - 2]] + " GIVES OFF TO " + bs[ha[j - 1]] + "\n");
                        print(bs[ha[j - 1]] + " DROPS TO " + bs[ha[j - 3]] + "\n");
                        g = ha[j - 3];
                        g1 = ha[j - 1];
                        g2 = ha[j - 2];
                        z1 = 2;
                        break;
                    case 3:
                        print(" A '3 ON 2 ' WITH A ' TRAILER '!\n");
                        print(bs[ha[j - 4]] + " GIVES TO " + bs[ha[j - 2]] + " WHO SHUFFLES IT OFF TO\n");
                        print(bs[ha[j - 1]] + " WHO FIRES A WING TO WING PASS TO \n");
                        print(bs[ha[j - 3]] + " AS HE CUTS IN ALONE!!\n");
                        g = ha[j - 3];
                        g1 = ha[j - 1];
                        g2 = ha[j - 2];
                        z1 = 1;
                        break;
                }
            }
            do {
                print("SHOT");
                s = parseInt(await input());
            } while (s < 1 || s > 4) ;
            if (c == 1)
                print(as[g]);
            else
                print(bs[g]);
            switch (s) {
                case 1:
                    print(" LET'S A BIG SLAP SHOT GO!!\n");
                    z = 4;
                    z += z1;
                    break;
                case 2:
                    print(" RIPS A WRIST SHOT OFF\n");
                    z = 2;
                    z += z1;
                    break;
                case 3:
                    print(" GETS A BACKHAND OFF\n");
                    z = 3;
                    z += z1;
                    break;
                case 4:
                    print(" SNAPS OFF A SNAP SHOT\n");
                    z = 2;
                    z += z1;
                    break;
            }
        }
        do {
            print("AREA");
            a = parseInt(await input());
        } while (a < 1 || a > 4) ;
        if (c == 1)
            s2++;
        else
            s3++;
        a1 = Math.floor(4 * Math.random()) + 1;
        if (a == a1) {
            while (1) {
                ha[20] = Math.floor(100 * Math.random()) + 1;
                if (ha[20] % z != 0)
                    break;
                a2 = Math.floor(100 * Math.random()) + 1;
                if (a2 % 4 == 0) {
                    if (c == 1)
                        print("SAVE " + bs[6] + " --  REBOUND\n");
                    else
                        print("SAVE " + as[6] + " --  FOLLOW up\n");
                    continue;
                } else {
                    a1 = a + 1;  // So a != a1
                }
            }
            if (ha[20] % z != 0) {
                if (c == 1) {
                    print("GOAL " + as[7] + "\n");
                    ha[9]++;
                } else {
                    print("SCORE " + bs[7] + "\n");
                    ha[8]++;
                }
                // Bells in origninal
                print("\n");
                print("SCORE: ");
                if (ha[8] <= ha[9]) {
                    print(as[7] + ": " + ha[9] + "\t" + bs[7] + ": " + ha[8] + "\n");
                } else {
                    print(bs[7] + ": " + ha[8] + "\t" + as[7] + ": " + ha[9] + "\n");
                }
                if (c == 1) {
                    print("GOAL SCORED BY: " + as[g] + "\n");
                    if (g1 != 0) {
                        if (g2 != 0) {
                            print(" ASSISTED BY: " + as[g1] + " AND " + as[g2] + "\n");
                        } else {
                            print(" ASSISTED BY: " + as[g1] + "\n");
                        }
                    } else {
                        print(" UNASSISTED.\n");
                    }
                    ta[g]++;
                    t1[g1]++;
                    t1[g2]++;
                    // 1540
                } else {
                    print("GOAL SCORED BY: " + bs[g] + "\n");
                    if (g1 != 0) {
                        if (g2 != 0) {
                            print(" ASSISTED BY: " + bs[g1] + " AND " + bs[g2] + "\n");
                        } else {
                            print(" ASSISTED BY: " + bs[g1] + "\n");
                        }
                    } else {
                        print(" UNASSISTED.\n");
                    }
                    t2[g]++;
                    t3[g1]++;
                    t3[g2]++;
                    // 1540
                }
            }
        }
        if (a != a1) {
            s1 = Math.floor(6 * Math.random()) + 1;
            if (c == 1) {
                switch (s1) {
                    case 1:
                        print("KICK SAVE AND A BEAUTY BY " + bs[6] + "\n");
                        print("CLEARED OUT BY " + bs[3] + "\n");
                        l--;
                        continue;
                    case 2:
                        print("WHAT A SPECTACULAR GLOVE SAVE BY " + bs[6] + "\n");
                        print("AND " + bs[6] + " GOLFS IT INTO THE CROWD\n");
                        break;
                    case 3:
                        print("SKATE SAVE ON A LOW STEAMER BY " + bs[6] + "\n");
                        l--;
                        continue;
                    case 4:
                        print("PAD SAVE BY " + bs[6] + " OFF THE STICK\n");
                        print("OF " + as[g] + " AND " + bs[6] + " COVERS UP\n");
                        break;
                    case 5:
                        print("WHISTLES ONE OVER THE HEAD OF " + bs[6] + "\n");
                        l--;
                        continue;
                    case 6:
                        print(bs[6] + " MAKES A FACE SAVE!! AND HE IS HURT\n");
                        print("THE DEFENSEMAN " + bs[5] + " COVERS UP FOR HIM\n");
                        break;
                }
            } else {
                switch (s1) {
                    case 1:
                        print("STICK SAVE BY " + as[6] +"\n");
                        print("AND CLEARED OUT BY " + as[4] + "\n");
                        l--;
                        continue;
                    case 2:
                        print("OH MY GOD!! " + bs[g] + " RATTLES ONE OFF THE POST\n");
                        print("TO THE RIGHT OF " + as[6] + " AND " + as[6] + " COVERS ");
                        print("ON THE LOOSE PUCK!\n");
                        break;
                    case 3:
                        print("SKATE SAVE BY " + as[6] + "\n");
                        print(as[6] + " WHACKS THE LOOSE PUCK INTO THE STANDS\n");
                        break;
                    case 4:
                        print("STICK SAVE BY " + as[6] + " AND HE CLEARS IT OUT HIMSELF\n");
                        l--;
                        continue;
                    case 5:
                        print("KICKED OUT BY " + as[6] + "\n");
                        print("AND IT REBOUNDS ALL THE WAY TO CENTER ICE\n");
                        l--;
                        continue;
                    case 6:
                        print("GLOVE SAVE " + as[6] + " AND HE HANGS ON\n");
                        break;
                }
            }
        }
        print("AND WE'RE READY FOR THE FACE-OFF\n");
    }
    // Bells chime
    print("THAT'S THE SIREN\n");
    print("\n");
    print(tab(15) + "FINAL SCORE:\n");
    if (ha[8] <= ha[9]) {
        print(as[7] + ": " + ha[9] + "\t" + bs[7] + ": " + ha[8] + "\n");
    } else {
        print(bs[7] + ": " + ha[8] + "\t" + as[7] + ": " + ha[9] + "\n");
    }
    print("\n");
    print(tab(10) + "SCORING SUMMARY\n");
    print("\n");
    print(tab(25) + as[7] + "\n");
    print("\tNAME\tGOALS\tASSISTS\n");
    print("\t----\t-----\t-------\n");
    for (i = 1; i <= 5; i++) {
        print("\t" + as[i] + "\t" + ta[i] + "\t" + t1[i] + "\n");
    }
    print("\n");
    print(tab(25) + bs[7] + "\n");
    print("\tNAME\tGOALS\tASSISTS\n");
    print("\t----\t-----\t-------\n");
    for (t = 1; t <= 5; t++) {
        print("\t" + bs[t] + "\t" + t2[t] + "\t" + t3[t] + "\n");
    }
    print("\n");
    print("SHOTS ON NET\n");
    print(as[7] + ": " + s2 + "\n");
    print(bs[7] + ": " + s3 + "\n");
}

main();
