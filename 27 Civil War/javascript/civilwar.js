// CIVIL WAR
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

// Historical data...can add more (strat., etc) by inserting
// data statements after appro. info, and adjusting read
//                      0 - C$     1-M1  2-M2  3-C1 4-C2 5-D
var historical_data = [,
                       ["BULL RUN",18000,18500,1967,2708,1],
                       ["SHILOH",40000.,44894.,10699,13047,3],
                       ["SEVEN DAYS",95000.,115000.,20614,15849,3],
                       ["SECOND BULL RUN",54000.,63000.,10000,14000,2],
                       ["ANTIETAM",40000.,50000.,10000,12000,3],
                       ["FREDERICKSBURG",75000.,120000.,5377,12653,1],
                       ["MURFREESBORO",38000.,45000.,11000,12000,1],
                       ["CHANCELLORSVILLE",32000,90000.,13000,17197,2],
                       ["VICKSBURG",50000.,70000.,12000,19000,1],
                       ["GETTYSBURG",72500.,85000.,20000,23000,3],
                       ["CHICKAMAUGA",66000.,60000.,18000,16000,2],
                       ["CHATTANOOGA",37000.,60000.,36700.,5800,2],
                       ["SPOTSYLVANIA",62000.,110000.,17723,18000,2],
                       ["ATLANTA",65000.,100000.,8500,3700,1]];
var sa = [];
var da = [];
var fa = [];
var ha = [];
var ba = [];
var oa = [];

// Main program
async function main()
{
    print(tab(26) + "CIVIL WAR\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    // Original game design: Cram, Goodie, Hibbard Lexington H.S.
    // Modifications: G. Paul, R. Hess (Ties), 1973
    // Union info on likely confederate strategy
    sa[1] = 25;
    sa[2] = 25;
    sa[3] = 25;
    sa[4] = 25;
    d = Math.random();
    print("\n");
    print("DO YOU WANT INSTRUCTIONS");
    while (1) {
        str = await input();
        if (str == "YES" || str == "NO")
            break;
        print("YES OR NO -- \n");
    }
    if (str == "YES") {
        print("\n");
        print("\n");
        print("\n");
        print("\n");
        print("THIS IS A CIVIL WAR SIMULATION.\n");
        print("TO PLAY TYPE A RESPONSE WHEN THE COMPUTER ASKS.\n");
        print("REMEMBER THAT ALL FACTORS ARE INTERRELATED AND THAT YOUR\n");
        print("RESPONSES COULD CHANGE HISTORY. FACTS AND FIGURES USED ARE\n");
        print("BASED ON THE ACTUAL OCCURRENCE. MOST BATTLES TEND TO RESULT\n");
        print("AS THEY DID IN THE CIVIL WAR, BUT IT ALL DEPENDS ON YOU!!\n");
        print("\n");
        print("THE OBJECT OF THE GAME IS TO WIN AS MANY BATTLES AS ");
        print("POSSIBLE.\n");
        print("\n");
        print("YOUR CHOICES FOR DEFENSIVE STRATEGY ARE:\n");
        print("        (1) ARTILLERY ATTACK\n");
        print("        (2) FORTIFICATION AGAINST FRONTAL ATTACK\n");
        print("        (3) FORTIFICATION AGAINST FLANKING MANEUVERS\n");
        print("        (4) FALLING BACK\n");
        print(" YOUR CHOICES FOR OFFENSIVE STRATEGY ARE:\n");
        print("        (1) ARTILLERY ATTACK\n");
        print("        (2) FRONTAL ATTACK\n");
        print("        (3) FLANKING MANEUVERS\n");
        print("        (4) ENCIRCLEMENT\n");
        print("YOU MAY SURRENDER BY TYPING A '5' FOR YOUR STRATEGY.\n");
    }
    print("\n");
    print("\n");
    print("\n");
    print("ARE THERE TWO GENERALS PRESENT ");
    while (1) {
        print("(ANSWER YES OR NO)");
        bs = await input();
        if (bs == "YES") {
            d = 2;
            break;
        } else if (bs == "NO") {
            print("\n");
            print("YOU ARE THE CONFEDERACY.   GOOD LUCK!\n");
            print("\n");
            d = 1;
            break;
        }
    }
    print("SELECT A BATTLE BY TYPING A NUMBER FROM 1 TO 14 ON\n");
    print("REQUEST.  TYPE ANY OTHER NUMBER TO END THE SIMULATION.\n");
    print("BUT '0' BRINGS BACK EXACT PREVIOUS BATTLE SITUATION\n");
    print("ALLOWING YOU TO REPLAY IT\n");
    print("\n");
    print("NOTE: A NEGATIVE FOOD$ ENTRY CAUSES THE PROGRAM TO \n");
    print("USE THE ENTRIES FROM THE PREVIOUS BATTLE\n");
    print("\n");
    print("AFTER REQUESTING A BATTLE, DO YOU WISH ");
    print("BATTLE DESCRIPTIONS ");
    while (1) {
        print("(ANSWER YES OR NO)");
        xs = await input();
        if (xs == "YES" || xs == "NO")
            break;
    }
    l = 0;
    w = 0;
    r1 = 0;
    q1 = 0;
    m3 = 0;
    m4 = 0;
    p1 = 0;
    p2 = 0;
    t1 = 0;
    t2 = 0;
    for (i = 1; i <= 2; i++) {
        da[i] = 0;
        fa[i] = 0;
        ha[i] = 0;
        ba[i] = 0;
        oa[i] = 0;
    }
    r2 = 0;
    q2 = 0;
    c6 = 0;
    f = 0;
    w0 = 0;
    y = 0;
    y2 = 0;
    u = 0;
    u2 = 0;
    while (1) {
        print("\n");
        print("\n");
        print("\n");
        print("WHICH BATTLE DO YOU WISH TO SIMULATE");
        a = parseInt(await input());
        if (a < 1 || a > 14)
            break;
        if (a != 0 || r == 0) {
            cs = historical_data[a][0];
            m1 = historical_data[a][1];
            m2 = historical_data[a][2];
            c1 = historical_data[a][3];
            c2 = historical_data[a][4];
            m = historical_data[a][5];
            u = 0;
            // Inflation calc
            i1 = 10 + (l - w) * 2;
            i2 = 10 + (w - l) * 2;
            // Money available
            da[1] = 100 * Math.floor((m1 * (100 - i1) / 2000) * (1 + (r1 - q1) / (r1 + 1)) + 0.5);
            da[2] = 100 * Math.floor(m2 * (100 - i2) / 2000 + 0.5);
            if (bs == "YES") {
                da[2] = 100 * Math.floor((m2 * (100 - i2) / 2000) * (1 + (r2 - q2) / (r2 + 1)) + 0.5);
            }
            // Men available
            m5 = Math.floor(m1 * (1 + (p1 - t1) / (m3 + 1)));
            m6 = Math.floor(m2 * (1 + (p2 - t2) / (m4 + 1)));
            f1 = 5 * m1 / 6;
            print("\n");
            print("\n");
            print("\n");
            print("\n");
            print("\n");
            print("THIS IS THE BATTLE OF " + cs + "\n");
            if (xs != "NO") {
                switch (a) {
                    case 1:
                        print("JULY 21, 1861.  GEN. BEAUREGARD, COMMANDING THE SOUTH, MET\n");
                        print("UNION FORCES WITH GEN. MCDOWELL IN A PREMATURE BATTLE AT\n");
                        print("BULL RUN. GEN. JACKSON HELPED PUSH BACK THE UNION ATTACK.\n");
                        break;
                    case 2:
                        print("APRIL 6-7, 1862.  THE CONFEDERATE SURPRISE ATTACK AT\n");
                        print("SHILOH FAILED DUE TO POOR ORGANIZATION.\n");
                        break;
                    case 3:
                        print("JUNE 25-JULY 1, 1862.  GENERAL LEE (CSA) UPHELD THE\n");
                        print("OFFENSIVE THROUGHOUT THE BATTLE AND FORCED GEN. MCCLELLAN\n");
                        print("AND THE UNION FORCES AWAY FROM RICHMOND.\n");
                        break;
                    case 4:
                        print("AUG 29-30, 1862.  THE COMBINED CONFEDERATE FORCES UNDER LEE\n");
                        print("AND JACKSON DROVE THE UNION FORCES BACK INTO WASHINGTON.\n");
                        break;
                    case 5:
                        print("SEPT 17, 1862.  THE SOUTH FAILED TO INCORPORATE MARYLAND\n");
                        print("INTO THE CONFEDERACY.\n");
                        break;
                    case 6:
                        print("DEC 13, 1862.  THE CONFEDERACY UNDER LEE SUCCESSFULLY\n");
                        print("REPULSED AN ATTACK BY THE UNION UNDER GEN. BURNSIDE.\n");
                        break;
                    case 7:
                        print("DEC 31, 1862.  THE SOUTH UNDER GEN. BRAGG WON A CLOSE BATTLE.\n");
                        break;
                    case 8:
                        print("MAY 1-6, 1863.  THE SOUTH HAD A COSTLY VICTORY AND LOST\n");
                        print("ONE OF THEIR OUTSTANDING GENERALS, 'STONEWALL' JACKSON.\n");
                        break;
                    case 9:
                        print("JULY 4, 1863.  VICKSBURG WAS A COSTLY DEFEAT FOR THE SOUTH\n");
                        print("BECAUSE IT GAVE THE UNION ACCESS TO THE MISSISSIPPI.\n");
                        break;
                    case 10:
                        print("JULY 1-3, 1863.  A SOUTHERN MISTAKE BY GEN. LEE AT GETTYSBURG\n");
                        print("COST THEM ONE OF THE MOST CRUCIAL BATTLES OF THE WAR.\n");
                        break;
                    case 11:
                        print("SEPT. 15, 1863. CONFUSION IN A FOREST NEAR CHICKAMAUGA LED\n");
                        print("TO A COSTLY SOUTHERN VICTORY.\n");
                        break;
                    case 12:
                        print("NOV. 25, 1863. AFTER THE SOUTH HAD SIEGED GEN. ROSENCRANS'\n");
                        print("ARMY FOR THREE MONTHS, GEN. GRANT BROKE THE SIEGE.\n");
                        break;
                    case 13:
                        print("MAY 5, 1864.  GRANT'S PLAN TO KEEP LEE ISOLATED BEGAN TO\n");
                        print("FAIL HERE, AND CONTINUED AT COLD HARBOR AND PETERSBURG.\n");
                        break;
                    case 14:
                        print("AUGUST, 1864.  SHERMAN AND THREE VETERAN ARMIES CONVERGED\n");
                        print("ON ATLANTA AND DEALT THE DEATH BLOW TO THE CONFEDERACY.\n");
                        break;
                }
            }
        } else {
            print(cs + " INSTANT REPLAY\n");
        }
        print("\n");
        print(" \tCONFEDERACY\t UNION\n"),
        print("MEN\t  " + m5 + "\t\t " + m6 + "\n");
        print("MONEY\t $" + da[1] + "\t\t$" + da[2] + "\n");
        print("INFLATION\t " + (i1 + 15) + "%\t " + i2 + "%\n");
        print("\n");
        // ONLY IN PRINTOUT IS CONFED INFLATION = I1 + 15%
        // IF TWO GENERALS, INPUT CONFED, FIRST
        for (i = 1; i <= d; i++) {
            if (bs == "YES" && i == 1)
                print("CONFEDERATE GENERAL---");
            print("HOW MUCH DO YOU WISH TO SPEND FOR\n");
            while (1) {
                print(" - FOOD......");
                f = parseInt(await input());
                if (f < 0) {
                    if (r1 == 0) {
                        print("NO PREVIOUS ENTRIES\n");
                        continue;
                    }
                    print("ASSUME YOU WANT TO KEEP SAME ALLOCATIONS\n");
                    print("\n");
                    break;
                }
                fa[i] = f;
                while (1) {
                    print(" - SALARIES..");
                    ha[i] = parseInt(await input());
                    if (ha[i] >= 0)
                        break;
                    print("NEGATIVE VALUES NOT ALLOWED.\n");
                }
                while (1) {
                    print(" - AMMUNITION");
                    ba[i] = parseInt(await input());
                    if (ba[i] >= 0)
                        break;
                    print("NEGATIVE VALUES NOT ALLOWED.\n");
                }
                print("\n");
                if (fa[i] + ha[i] + ba[i] > da[i]) {
                    print("THINK AGAIN! YOU HAVE ONLY $" + da[i] + "\n");
                } else {
                    break;
                }
            }
            if (bs != "YES" || i == 2)
                break;
            print("UNION GENERAL---");
        }
        for (z = 1; z <= d; z++) {
            if (bs == "YES") {
                if (z == 1)
                    print("CONFEDERATE ");
                else
                    print("      UNION ");
            }
            // Find morale
            o = ((2 * Math.pow(fa[z], 2) + Math.pow(ha[z], 2)) / Math.pow(f1, 2) + 1);
            if (o >= 10) {
                print("MORALE IS HIGH\n");
            } else if (o >= 5) {
                print("MORALE IS FAIR\n");
            } else {
                print("MORALE IS POOR\n");
            }
            if (bs != "YES")
                break;
            oa[z] = o;
        }
        o2 = oa[2];
        o = oa[1];
        print("CONFEDERATE GENERAL---");
        // Actual off/def battle situation
        if (m == 3) {
            print("YOU ARE ON THE OFFENSIVE\n");
        } else if (m == 1) {
            print("YOU ARE ON THE DEFENSIVE\n");
        } else {
            print("BOTH SIDES ARE ON THE OFFENSIVE \n");
        }
        print("\n");
        // Choose strategies
        if (bs != "YES") {
            print("YOUR STRATEGY ");
            while (1) {
                y = parseInt(await input());
                if (Math.abs(y - 3) < 3)
                    break;
                print("STRATEGY " + y + " NOT ALLOWED.\n");
            }
            if (y == 5) {
                print("THE CONFEDERACY HAS SURRENDERED.\n");
                break;
            }
            // Union strategy is computer choesn
            print("UNION STRATEGY IS ");
            if (a == 0) {
                while (1) {
                    y2 = parseInt(await input());
                    if (y2 > 0 && y2 < 5)
                        break;
                    print("ENTER 1, 2, 3, OR 4 (USUALLY PREVIOUS UNION STRATEGY)\n");
                }
            } else {
                s0 = 0;
                r = Math.random() * 100;
                for (i = 1; i <= 4; i++) {
                    s0 += sa[i];
                    // If actual strategy info is in program data statements
                    // then r-100 is extra weight given to that strategy.
                    if (r < s0)
                        break;
                }
                y2 = i;
                print(y2 + "\n");
            }
        } else {
            for (i = 1; i <= 2; i++) {
                if (i == 1)
                    print("CONFEDERATE STRATEGY ");
                while (1) {
                    y = parseInt(await input());
                    if (Math.abs(y - 3) < 3)
                        break;
                    print("STRATEGY " + y + " NOT ALLOWED.\n");
                }
                if (i == 2) {
                    y2 = y;
                    y = y1;
                    if (y2 != 5)
                        break;
                } else {
                    y1 = y;
                }
                print("UNION STRATEGY ");
            }
            // Simulated losses - North
            c6 = (2 * c2 / 5) * (1 + 1 / (2 * (Math.abs(y2 - y) + 1)));
            c6 = c6 * (1.28 + (5 * m2 / 6) / (b[2] + 1));
            c6 = Math.floor(c6 * (1 + 1 / o2) + 0.5);
            // If loss > men present, rescale losses
            e2 = 100 / o2;
            if (Math.floor(c6 + e2) >= m6) {
                c6 = Math.floor(13 * m6 / 20);
                e2 = 7 * c6 / 13;
                u2 = 1;
            }
        }
        // Calculate simulated losses
        print("\n");
        print("\n");
        print("\n");
        print("\t\tCONFEDERACY\tUNION\n");
        c5 = (2 * c1 / 5) * (1 + 1 / (2 * (Math.abs(y2 - y) + 1)));
        c5 = Math.floor(c5 * (1 + 1 / o) * (1.28 + f1 / (ba[1] + 1)) + 0.5);
        e = 100 / o;
        if (c5 + 100 / o >= m1 * (1 + (p1 - t1) / (m3 + 1))) {
            c5 = Math.floor(13 * m1 / 20 * (1 + (p1 - t1) / (m3 + 1)));
            e = 7 * c5 / 13;
            u = 1;
        }
        if (d == 1) {
            c6 = Math.floor(17 * c2 * c1 / (c5 * 20));
            e2 = 5 * o;
        }
        print("CASUALTIES\t" + c5 + "\t\t" + c6 + "\n");
        print("DESERTIONS\t" + Math.floor(e) + "\t\t" + Math.floor(e2) + "\n");
        print("\n");
        if (bs == "YES") {
            print("COMPARED TO THE ACTUAL CASUALTIES AT " + cs + "\n");
            print("CONFEDERATE: " + Math.floor(100 * (c5 / c1) + 0.5) + "% OF THE ORIGINAL\n");
            print("UNION:       " + Math.floor(100 * (c6 / c2) + 0.5) + "% OF THE ORIGINAL\n");
        }
        print("\n");
        // 1 Who one
        if (u == 1 && u2 == 1 || (u != 1 && u2 != 1 && c5 + e == c6 + e2)) {
            print("BATTLE OUTCOME UNRESOLVED\n");
            w0++;
        } else if (u == 1 || (u != 1 && u2 != 1 && c5 + e > c6 + e2)) {
            print("THE UNION WINS " + cs + "\n");
            if (a != 0)
                l++;
        } else  {
            print("THE CONFEDERACY WINS " + cs + "\n");
            if (a != 0)
                w++;
        }
        // Lines 2530 to 2590 from original are unreachable.
        if (a != 0) {
            t1 += c5 + e;
            t2 += c6 + e2;
            p1 += c1;
            p2 += c2;
            q1 += fa[1] + ha[1] + ba[1];
            q2 += fa[2] + ha[2] + ba[2];
            r1 += m1 * (100 - i1) / 20;
            r2 += m2 * (100 - i2) / 20;
            m3 += m1;
            m4 += m2;
            // Learn present strategy, start forgetting old ones
            // present startegy of south gains 3*s, others lose s
            // probability points, unless a strategy falls below 5%.
            s = 3;
            s0 = 0;
            for (i = 1; i <= 4; i++) {
                if (s[i] <= 5)
                    continue;
                s[i] -= 5;
                s0 += s;
            }
            s[y] += s0;
        }
        u = 0;
        u2 = 0;
        print("---------------");
        continue;
    }
    print("\n");
    print("\n");
    print("\n");
    print("\n");
    print("\n");
    print("\n");
    print("THE CONFEDERACY HAS WON " + w + " BATTLES AND LOST " + l + "\n");
    if (y == 5 || (y2 != 5 && w <= l)) {
        print("THE UNION HAS WON THE WAR\n");
    } else {
        print("THE CONFEDERACY HAS WON THE WAR\n");
    }
    print("\n");
    if (r1) {
        print("FOR THE " + (w + l + w0) + " BATTLES FOUGHT (EXCLUDING RERUNS)\n");
        print(" \t \t ");
        print("CONFEDERACY\t UNION\n");
        print("HISTORICAL LOSSES\t" + Math.floor(p1 + 0.5) + "\t" + Math.floor(p2 + 0.5) + "\n");
        print("SIMULATED LOSSES\t" + Math.floor(t1 + 0.5) + "\t" + Math.floor(t2 + 0.5) + "\n");
        print("\n");
        print("    % OF ORIGINAL\t" + Math.floor(100 * (t1 / p1) + 0.5) + "\t" + Math.floor(100 * (t2 / p2) + 0.5) + "\n");
        if (bs != "YES") {
            print("\n");
            print("UNION INTELLIGENCE SUGGEST THAT THE SOUTH USED \n");
            print("STRATEGIES 1, 2, 3, 4 IN THE FOLLOWING PERCENTAGES\n");
            print(sa[1] + " " + sa[2] + " " + sa[3] + " " + sa[4] + "\n");
        }
    }
}

main();
