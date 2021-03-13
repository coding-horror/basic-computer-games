// LEM
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

// Main program
async function main()
{
    print(tab(34) + "LEM\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    // ROCKT2 is an interactive game that simulates a lunar
    // landing is similar to that of the Apollo program.
    // There is absolutely no chance involved
    zs = "GO";
    b1 = 1;
    while (1) {
        m = 17.95;
        f1 = 5.25;
        n = 7.5;
        r0 = 926;
        v0 = 1.29;
        t = 0;
        h0 = 60;
        r = r0 + h0;
        a = -3,425;
        r1 = 0;
        a1 = 8.84361e-4;
        r3 = 0;
        a3 = 0;
        m1 = 7.45;
        m0 = m1;
        b = 750;
        t1 = 0;
        f = 0;
        p = 0;
        n = 1;
        m2 = 0;
        s = 0;
        c = 0;
        if (zs == "YES") {
            print("\n");
            print("OK, DO YOU WANT THE COMPLETE INSTRUCTIONS OR THE INPUT -\n");
            print("OUTPUT STATEMENTS?\n");
            while (1) {
                print("1=COMPLETE INSTRUCTIONS\n");
                print("2=INPUT-OUTPUT STATEMENTS\n");
                print("3=NEITHER\n");
                b1 = parseInt(await input());
                qs = "NO";
                if (b1 == 1)
                    break;
                qs = "YES";
                if (b1 == 2 || b1 == 3)
                    break;
            }
        } else {
            print("\n");
            print("LUNAR LANDING SIMULATION\n");
            print("\n");
            print("HAVE YOU FLOWN AN APOLLO/LEM MISSION BEFORE");
            while (1) {
                print(" (YES OR NO)");
                qs = await input();
                if (qs == "YES" || qs == "NO")
                    break;
                print("JUST ANSWER THE QUESTION, PLEASE, ");
            }
        }
        if (qs == "YES") {
            print("\n");
            print("INPUT MEASUREMENT OPTION NUMBER");
        } else {
            print("\n");
            print("WHICH SYSTEM OF MEASUREMENT DO YOU PREFER?\n");
            print(" 1=METRIC     0=ENGLISH\n");
            print("ENTER THE APPROPIATE NUMBER");
        }
        while (1) {
            k = parseInt(await input());
            if (k == 0 || k == 1)
                break;
            print("ENTER THE APPROPIATE NUMBER");
        }
        if (k == 1) {
            z = 1852.8;
            ms = "METERS";
            g3 = 3.6;
            ns = " KILOMETERS";
            g5 = 1000;
        } else {
            z = 6080;
            ms = "FEET";
            g3 = 0.592;
            ns = "N.MILES";
            g5 = z;
        }
        if (b1 != 3) {
            if (qs != "YES") {
                print("\n");
                print("  YOU ARE ON A LUNAR LANDING MISSION.  AS THE PILOT OF\n");
                print("THE LUNAR EXCURSION MODULE, YOU WILL BE EXPECTED TO\n");
                print("GIVE CERTAIN COMMANDS TO THE MODULE NAVIGATION SYSTEM.\n");
                print("THE ON-BOARD COMPUTER WILL GIVE A RUNNING ACCOUNT\n");
                print("OF INFORMATION NEEDED TO NAVIGATE THE SHIP.\n");
                print("\n");
                print("\n");
                print("THE ATTITUDE ANGLE CALLED FOR IS DESCRIBED AS FOLLOWS.\n");
                print("+ OR -180 DEGREES IS DIRECTLY AWAY FROM THE MOON\n");
                print("-90 DEGREES IS ON A TANGENT IN THE DIRECTION OF ORBIT\n");
                print("+90 DEGREES IS ON A TANGENT FROM THE DIRECTION OF ORBIT\n");
                print("0 (ZERO) DEGREES IS DIRECTLY TOWARD THE MOON\n");
                print("\n");
                print(tab(30) + "-180|+180\n");
                print(tab(34) + "^\n");
                print(tab(27) + "-90 < -+- > +90\n");
                print(tab(34) + "!\n");
                print(tab(34) + "0\n");
                print(tab(21) + "<<<< DIRECTION OF ORBIT <<<<\n");
                print("\n");
                print(tab(20) + "------ SURFACE OF MOON ------\n");
                print("\n");
                print("\n");
                print("ALL ANGLES BETWEEN -180 AND +180 DEGREES ARE ACCEPTED.\n");
                print("\n");
                print("1 FUEL UNIT = 1 SEC. AT MAX THRUST\n");
                print("ANY DISCREPANCIES ARE ACCOUNTED FOR IN THE USE OF FUEL\n");
                print("FOR AN ATTITUDE CHANGE.\n");
                print("AVAILABLE ENGINE POWER: 0 (ZERO) AND ANY VALUE BETWEEN\n");
                print("10 AND 100 PERCENT.\n");
                print("\n");
                print("NEGATIVE THRUST OR TIME IS PROHIBITED.\n");
                print("\n");
            }
            print("\n");
            print("INPUT: TIME INTERVAL IN SECONDS ------ (T)\n");
            print("       PERCENTAGE OF THRUST ---------- (P)\n");
            print("       ATTITUDE ANGLE IN DEGREES ----- (A)\n");
            print("\n");
            if (qs != "YES") {
                print("FOR EXAMPLE:\n");
                print("T,P,A? 10,65,-60\n");
                print("TO ABORT THE MISSION AT ANY TIME, ENTER 0,0,0\n");
                print("\n");
            }
            print("OUTPUT: TOTAL TIME IN ELAPSED SECONDS\n");
            print("        HEIGHT IN " + ms + "\n");
            print("        DISTANCE FROM LANDING SITE IN " + ms + "\n");
            print("        VERTICAL VELOCITY IN " + ms + "/SECOND\n");
            print("        HORIZONTAL VELOCITY IN " + ms + "/SECOND\n");
            print("        FUEL UNITS REMAINING\n");
            print("\n");
        }
        while (1) {
            for (i = 1; i <= n; i++) {
                if (m1 != 0) {
                    m1 -= m2;
                    if (m1 <= 0) {
                        f = f * (1 + m1 / m2);
                        m2 = m1 + m2;
                        print("YOU ARE OUT OF FUEL.\n");
                        m1 = 0;
                    }
                } else {
                    f = 0;
                    m2 = 0;
                }
                m = m - 0.5 * m2;
                r4 = r3;
                r3 = -0.5 * r0 * Math.pow(v0 / r, 2) + r * a1 * a1;
                r2 = (3 * r3 - r4) / 2 + 0.00526 * f1 * f * c / m;
                a4 = a3;
                a3 = -2 * r1 * a1 / r;
                a2 = (3 * a3 - a4) / 2 + 0.0056 * f1 * f * s / (m * r);
                x = r1 * t1 + 0.5 * r2 * t1 * t1;
                r = r + x;
                h0 = h0 + x;
                r1 = r1 + r2 * t1;
                a = a + a1 * t1 + 0.5 * a2 * t1 * t1;
                a1 = a1 + a2 * t1;
                m = m - 0.5 * m2;
                t = t + t1;
                if (h0 < 3.287828e-4)
                    break;
            }
            h = h0 * z;
            h1 = r1 * z;
            d = r0 * a * z;
            d1 = r * a1 * z;
            t2 = m1 * b / m0;
            print(" " + t + "\t" + h + "\t" + d + "\t" + h1 + "\t" + d1 + "\t" + t2 + "\n");
            if (h0 < 3.287828e-4) {
                if (r1 < -8.21957e-4 || Math.abs(r * a1) > 4.93174e-4 || h0 < -3.287828e-4) {
                    print("\n");
                    print("CRASH !!!!!!!!!!!!!!!!\n");
                    print("YOUR IMPACT CREATED A CRATER " + Math.abs(h) + " " + ms + " DEEP.\n");
                    x1 = Math.sqrt(d1 * d1 + h1 * h1) * g3;
                    print("AT CONTACT YOU WERE TRAVELING " + x1 + " " + ns + "/HR\n");
                    break;
                }
                if (Math.abs(d) > 10 * z) {
                    print("YOU ARE DOWN SAFELY - \n");
                    print("\n");
                    print("BUT MISSED THE LANDING SITE BY " + Math.abs(d / g5) + " " + ns + ".\n");
                    break;
                }
                print("\n");
                print("TRANQUILITY BASE HERE -- THE EAGLE HAS LANDED.\n");
                print("CONGRATULATIONS -- THERE WAS NO SPACECRAFT DAMAGE.\n");
                print("YOU MAY NOW PROCEED WITH SURFACE EXPLORATION.\n");
                break;
            }
            if (r0 * a > 164.474) {
                print("\n");
                print("YOU HAVE BEEN LOST IN SPACE WITH NO HOPE OF RECOVERY.\n");
                break;
            }
            if (m1 > 0) {
                while (1) {
                    print("T,P,A");
                    str = await input();
                    t1 = parseFloat(str);
                    f = parseFloat(str.substr(str.indexOf(",") + 1));
                    p = parseFloat(str.substr(str.lastIndexOf(",") + 1));
                    f = f / 100;
                    if (t1 < 0) {
                        print("\n");
                        print("THIS SPACECRAFT IS NOT ABLE TO VIOLATE THE SPACE-");
                        print("TIME CONTINUUM.\n");
                        print("\n");
                    } else if (t1 == 0) {
                        break;
                    } else if (Math.abs(f - 0.05) > 1 || Math.abs(f - 0.05) < 0.05) {
                        print("IMPOSSIBLE THRUST VALUE ");
                        if (f < 0) {
                            print("NEGATIVE\n");
                        } else if (f - 0.05 < 0.05) {
                            print("TOO SMALL\n");
                        } else {
                            print("TOO LARGE\n");
                        }
                        print("\n");
                    } else if (Math.abs(p) > 180) {
                        print("\n");
                        print("IF YOU WANT TO SPIN AROUND, GO OUTSIDE THE MODULE\n");
                        print("FOR AN E.V.A.\n");
                        print("\n");
                    } else {
                        break;
                    }
                }
                if (t1 == 0) {
                    print("\n");
                    print("MISSION ABENDED\n");
                    break;
                }
            } else {
                t1 = 20;
                f = 0;
                p = 0;
            }
            n = 20;
            if (t1 >= 400)
                n = t1 / 20;
            t1 = t1 / n;
            p = p * 3.14159 / 180;
            s = Math.sin(p);
            c = Math.cos(p);
            m2 = m0 * t1 * f / b;
            r3 = -0.5 * r0 * Math.pow(v0 / r, 2) + r * a1 * a1;
            a3 = -2 * r1 * a1 / r;
        }
        print("\n");
        while (1) {
            print("DO YOU WANT TO TRY IT AGAIN (YES/NO)?\n");
            zs = await input();
            if (zs == "YES" || zs == "NO")
                break;
        }
        if (zs != "YES")
            break;
    }
    print("\n");
    print("TOO BAD, THE SPACE PROGRAM HATES TO LOSE EXPERIENCED\n");
    print("ASTRONAUTS.\n");
}

main();
