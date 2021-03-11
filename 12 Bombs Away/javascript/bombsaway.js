// BOMBS AWAY
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
    while (1) {
        print("YOU ARE A PILOT IN A WORLD WAR II BOMBER.\n");
        while (1) {
            print("WHAT SIDE -- ITALY(1), ALLIES(2), JAPAN(3), GERMANY(4)");
            a = parseInt(await input());
            if (a < 1 || a > 4)
                print("TRY AGAIN...\n");
            else
                break;
        }
        if (a == 1) {
            while (1) {
                print("YOUR TARGET -- ALBANIA(1), GREECE(2), NORTH AFRICA(3)");
                b = parseInt(await input());
                if (b < 1 || b > 3)
                    print("TRY AGAIN...\n");
                else
                    break;
            }
            print("\n");
            if (b == 1) {
                print("SHOULD BE EASY -- YOU'RE FLYING A NAZI-MADE PLANE.\n");
            } else if (b == 2) {
                print("BE CAREFUL!!!\n");
            } else {
                print("YOU'RE GOING FOR THE OIL, EH?\n");
            }
        } else if (a == 2) {
            while (1) {
                print("AIRCRAFT -- LIBERATOR(1), B-29(2), B-17(3), LANCASTER(4)");
                g = parseInt(await input());
                if (g < 1 || g > 4)
                    print("TRY AGAIN...\n");
                else
                    break;
            }
            print("\n");
            if (g == 1) {
                print("YOU'VE GOT 2 TONS OF BOMBS FLYING FOR PLOESTI.\n");
            } else if (g == 2) {
                print("YOU'RE DUMPING THE A-BOMB ON HIROSHIMA.\n");
            } else if (g == 3) {
                print("YOU'RE CHASING THE BISMARK IN THE NORTH SEA.\n");
            } else {
                print("YOU'RE BUSTING A GERMAN HEAVY WATER PLANT IN THE RUHR.\n");
            }
        } else if (a == 3) {
            print("YOU'RE FLYING A KAMIKAZE MISSION OVER THE USS LEXINGTON.\n");
            print("YOUR FIRST KAMIKAZE MISSION(Y OR N)");
            str = await input();
            if (str == "N") {
                s = 0;
            } else {
                s = 1;
                print("\n");
            }
        } else {
            while (1) {
                print("A NAZI, EH?  OH WELL.  ARE YOU GOING FOR RUSSIA(1),\n");
                print("ENGLAND(2), OR FRANCE(3)");
                m = parseInt(await input());
                if (m < 1 || m > 3)
                    print("TRY AGAIN...\n");
                else
                    break;
            }
            print("\n");
            if (m == 1) {
                print("YOU'RE NEARING STALINGRAD.\n");
            } else if (m == 2) {
                print("NEARING LONDON.  BE CAREFUL, THEY'VE GOT RADAR.\n");
            } else if (m == 3) {
                print("NEARING VERSAILLES.  DUCK SOUP.  THEY'RE NEARLY DEFENSELESS.\n");
            }
        }
        if (a != 3) {
            print("\n");
            while (1) {
                print("HOW MANY MISSIONS HAVE YOU FLOWN");
                d = parseInt(await input());
                if (d < 160)
                    break;
                print("MISSIONS, NOT MILES...\n");
                print("150 MISSIONS IS HIGH EVEN FOR OLD-TIMERS.\n");
                print("NOW THEN, ");
            }
            print("\n");
            if (d >= 100) {
                print("THAT'S PUSHING THE ODDS!\n");
            } else if (d < 25) {
                print("FRESH OUT OF TRAINING, EH?\n");
            }
            print("\n");
            if (d >= 160 * Math.random())
                hit = true;
            else
                hit = false;
        } else {
            if (s == 0) {
                hit = false;
            } else if (Math.random() > 0.65) {
                hit = true;
            } else {
                hit = false;
                s = 100;
            }
        }
        if (hit) {
            print("DIRECT HIT!!!! " + Math.floor(100 * Math.random()) + " KILLED.\n");
            print("MISSION SUCCESSFUL.\n");
        } else {
            t = 0;
            if (a != 3) {
                print("MISSED TARGET BY " + Math.floor(2 + 30 * Math.random()) + " MILES!\n");
                print("NOW YOU'RE REALLY IN FOR IT !!\n");
                print("\n");
                while (1) {
                    print("DOES THE ENEMY HAVE GUNS(1), MISSILE(2), OR BOTH(3)");
                    r = parseInt(await input());
                    if (r < 1 || r > 3)
                        print("TRY AGAIN...\n");
                    else
                        break;
                }
                print("\n");
                if (r != 2) {
                    print("WHAT'S THE PERCENT HIT RATE OF ENEMY GUNNERS (10 TO 50)");
                    s = parseInt(await input());
                    if (s < 10)
                        print("YOU LIE, BUT YOU'LL PAY...\n");
                    print("\n");
                }
                print("\n");
                if (r > 1)
                    t = 35;
            }
            if (s + t <= 100 * Math.random()) {
                print("YOU MADE IT THROUGH TREMENDOUS FLAK!!\n");
            } else {
                print("* * * * BOOM * * * *\n");
                print("YOU HAVE BEEN SHOT DOWN.....\n");
                print("DEARLY BELOVED, WE ARE GATHERED HERE TODAY TO PAY OUR\n");
                print("LAST TRIBUTE...\n");
            }
        }
        print("\n");
        print("\n");
        print("\n");
        print("ANOTHER MISSION (Y OR N)");
        str = await input();
        if (str != "Y")
            break;
    }
    print("CHICKEN !!!\n");
    print("\n");
}

main();
