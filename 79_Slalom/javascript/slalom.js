// SLALOM
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

var speed = [,14,18,26,29,18,
             25,28,32,29,20,
             29,29,25,21,26,
             29,20,21,20,18,
             26,25,33,31,22];

function show_instructions()
{
    print("\n");
    print("*** SLALOM: THIS IS THE 1976 WINTER OLYMPIC GIANT SLALOM.  YOU ARE\n");
    print("            THE AMERICAN TEAM'S ONLY HOPE OF A GOLD MEDAL.\n");
    print("\n");
    print("     0 -- TYPE THIS IS YOU WANT TO SEE HOW LONG YOU'VE TAKEN.\n");
    print("     1 -- TYPE THIS IF YOU WANT TO SPEED UP A LOT.\n");
    print("     2 -- TYPE THIS IF YOU WANT TO SPEED UP A LITTLE.\n");
    print("     3 -- TYPE THIS IF YOU WANT TO SPEED UP A TEENSY.\n");
    print("     4 -- TYPE THIS IF YOU WANT TO KEEP GOING THE SAME SPEED.\n");
    print("     5 -- TYPE THIS IF YOU WANT TO CHECK A TEENSY.\n");
    print("     6 -- TYPE THIS IF YOU WANT TO CHECK A LITTLE.\n");
    print("     7 -- TYPE THIS IF YOU WANT TO CHECK A LOT.\n");
    print("     8 -- TYPE THIS IF YOU WANT TO CHEAT AND TRY TO SKIP A GATE.\n");
    print("\n");
    print(" THE PLACE TO USE THESE OPTIONS IS WHEN THE COMPUTER ASKS:\n");
    print("\n");
    print("OPTION?\n");
    print("\n");
    print("                GOOD LUCK!\n");
    print("\n");
}

function show_speeds()
{
    print("GATE MAX\n");
    print(" #  M.P.H.\n");
    print("----------\n");
    for (var b = 1; b <= v; b++) {
        print(" " + b + "  " + speed[b] + "\n");
    }
}

// Main program
async function main()
{
    var gold = 0;
    var silver = 0;
    var bronze = 0;

    print(tab(33) + "SLALOM\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    while (1) {
        print("HOW MANY GATES DOES THIS COURSE HAVE (1 TO 25)");
        v = parseInt(await input());
        if (v >= 25) {
            print("25 IS THE LIMIT\n");
            v = 25;
        } else if (v < 1) {
            print("TRY AGAIN.\n");
        } else {
            break;
        }
    }
    print("\n");
    print("TYPE \"INS\" FOR INSTRUCTIONS\n");
    print("TYPE \"MAX\" FOR APPROXIMATE MAXIMUM SPEEDS\n");
    print("TYPE \"RUN\" FOR THE BEGINNING OF THE RACE\n");
    while (1) {
        print("COMMAND--");
        str = await input();
        if (str == "INS") {
            show_instructions();
        } else if (str == "MAX") {
            show_speeds();
        } else if (str == "RUN") {
            break;
        } else {
            print("\"" + str + "\" IS AN ILLEGAL COMMAND--RETRY");
        }
    }
    while (1) {
        print("RATE YOURSELF AS A SKIER, (1=WORST, 3=BEST)");
        a = parseInt(await input());
        if (a < 1 || a > 3)
            print("THE BOUNDS ARE 1-3\n");
        else
            break;
    }
    while (1) {
        print("THE STARTER COUNTS DOWN...5...4...3...2...1...GO!");
        t = 0;
        s = Math.floor(Math.random(1) * (18 - 9) + 9);
        print("\n");
        print("YOU'RE OFF!\n");
        for (o = 1; o <= v; o++) {
            q = speed[o];
            print("\n");
            print("HERE COMES GATE #" + o + " :\n");
            print(s + " M.P.H.\n");
            s1 = s;
            while (1) {
                print("OPTION");
                o1 = parseInt(await input());
                if (o1 < 0 || o1 > 8)
                    print("WHAT?\n");
                else if (o1 == 0)
                    print("YOU'VE TAKEN " + (t + Math.random()) + " SECONDS.\n");
                else
                    break;
            }
            finish = false;
            switch (o1) {
                case 1:
                    s += Math.floor(Math.random() * (10 - 5) + 5);
                    break;
                case 2:
                    s += Math.floor(Math.random() * (5 - 3) + 3);
                    break;
                case 3:
                    s += Math.floor(Math.random() * (4 - 1) + 1);
                    break;
                case 4:
                    break;
                case 5:
                    s -= Math.floor(Math.random() * (4 - 1) + 1);
                    break;
                case 6:
                    s -= Math.floor(Math.random() * (5 - 3) + 3);
                    break;
                case 7:
                    s -= Math.floor(Math.random() * (10 - 5) + 5);
                    break;
                case 8:
                    print("***CHEAT\n");
                    if (Math.random() >= 0.7) {
                        print("YOU MADE IT!\n");
                        t += 1.5;
                    } else {
                        print("AN OFFICIAL CAUGHT YOU!\n");
                        print("YOU TOOK " + (t + Math.random()) + " SECONDS.\n");
                        finish = true;
                    }
                    break;
            }
            if (!finish) {
                if (o1 != 4)
                    print(s + " M.P.H.\n");
                if (s > q) {
                    if (Math.random() < ((s - q) * 0.1) + 0.2) {
                        print("YOU WENT OVER THE MAXIMUM SPEED AND ");
                        if (Math.random() < 0.5) {
                            print("SNAGGED A FLAG!\n");
                        } else {
                            print("WIPED OUT!\n");
                        }
                        print("YOU TOOK " + (t + Math.random()) + " SECONDS.\n");
                        finish = true;
                    } else {
                        print("YOU WENT OVER THE MAXIMUM SPEED AND MADE IT!\n");
                    }
                } else if (s > q - 1) {
                    print("CLOSE ONE!\n");
                }
            }
            if (finish)
                break;
            if (s < 7) {
                print("LET'S BE REALISTIC, OK?  LET'S GO BACK AND TRY AGAIN...\n");
                s = s1;
                o--;
                continue;
            }
            t += q - s + 1;
            if (s > q) {
                t += 0.5;
            }
        }
        if (!finish) {
            print("\n");
            print("YOU TOOK " + (t + Math.random()) + " SECONDS.\n");
            m = t;
            m /= v;
            if (m < 1.5 - (a * 0.1)) {
                print("YOU WON A GOLD MEDAL!\n");
                gold++;
            } else if (m < 2.9 - (a * 0.1)) {
                print("YOU WON A SILVER MEDAL\n");
                silver++;
            } else if (m < 4.4 - (a * 0.1)) {
                print("YOU WON A BRONZE MEDAL\n");
                bronze++;
            }
        }
        while (1) {
            print("\n");
            print("DO YOU WANT TO RACE AGAIN");
            str = await input();
            if (str != "YES" && str != "NO")
                print("PLEASE TYPE 'YES' OR 'NO'\n");
            else
                break;
        }
        if (str != "YES")
            break;
    }
    print("THANKS FOR THE RACE\n");
    if (gold >= 1)
        print("GOLD MEDALS: " + gold + "\n");
    if (silver >= 1)
        print("SILVER MEDALS: " + silver + "\n");
    if (bronze >= 1)
        print("BRONZE MEDALS: " + bronze + "\n");
}

main();
