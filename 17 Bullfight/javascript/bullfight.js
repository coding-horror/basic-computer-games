// BULLFIGHT
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

var a;
var b;
var c;
var l;
var t;
var as;
var bs;
var d = [];
var ls = [, "SUPERB", "GOOD", "FAIR", "POOR", "AWFUL"];

function af(k)
{
    return Math.floor(Math.random() * 2 + 1);
}

function cf(q)
{
    return df(q) * Math.random();
}

function df(q)
{
    return (4.5 + l / 6 - (d[1] + d[2]) * 2.5 + 4 * d[4] + 2 * d[5] - Math.pow(d[3], 2) / 120 - a);
}

function setup_helpers()
{
    b = 3 / a * Math.random();
    if (b < 0.37)
        c = 0.5;
    else if (b < 0.5)
        c = 0.4;
    else if (b < 0.63)
        c = 0.3;
    else if (b < 0.87)
        c = 0.2;
    else
        c = 0.1;
    t = Math.floor(10 * c + 0.2);
    print("THE " + as + bs + " DID A " + ls[t] + " JOB.\n");
    if (4 <= t) {
        if (5 != t) {
            // Lines 1800 and 1810 of original program are unreachable
            switch (af(0)) {
                case 1:
                    print("ONE OF THE " + as + bs + " WAS KILLED.\n");
                    break;
                case 2:
                    print("NO " + as + b + " WERE KILLED.\n");
                    break;
            }
        } else {
            if (as != "TOREAD")
                print(af(0) + " OF THE HORSES OF THE " + as + bs + " KILLED.\n");
            print(af(0) + " OF THE " + as + bs + " KILLED.\n");
        }
    }
    print("\n");
}

// Main program
async function main()
{
    print(tab(34) + "BULL\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    l = 1;
    print("DO YOU WANT INSTRUCTIONS");
    str = await input();
    if (str != "NO") {
        print("HELLO, ALL YOU BLOODLOVERS AND AFICIONADOS.\n");
        print("HERE IS YOUR BIG CHANCE TO KILL A BULL.\n");
        print("\n");
        print("ON EACH PASS OF THE BULL, YOU MAY TRY\n");
        print("0 - VERONICA (DANGEROUS INSIDE MOVE OF THE CAPE)\n");
        print("1 - LESS DANGEROUS OUTSIDE MOVE OF THE CAPE\n");
        print("2 - ORDINARY SWIRL OF THE CAPE.\n");
        print("\n");
        print("INSTEAD OF THE ABOVE, YOU MAY TRY TO KILL THE BULL\n");
        print("ON ANY TURN: 4 (OVER THE HORNS), 5 (IN THE CHEST).\n");
        print("BUT IF I WERE YOU,\n");
        print("I WOULDN'T TRY IT BEFORE THE SEVENTH PASS.\n");
        print("\n");
        print("THE CROWD WILL DETERMINE WHAT AWARD YOU DESERVE\n");
        print("(POSTHUMOUSLY IF NECESSARY).\n");
        print("THE BRAVER YOU ARE, THE BETTER THE AWARD YOU RECEIVE.\n");
        print("\n");
        print("THE BETTER THE JOB THE PICADORES AND TOREADORES DO,\n");
        print("THE BETTER YOUR CHANCES ARE.\n");
    }
    print("\n");
    print("\n");
    d[5] = 1;
    d[4] = 1;
    d[3] = 0;
    a = Math.floor(Math.random() * 5 + 1);
    print("YOU HAVE DRAWN A " + ls[a] + " BULL.\n");
    if (a > 4) {
        print("YOU'RE LUCKY.\n");
    } else if (a < 2) {
        print("GOOD LUCK.  YOU'LL NEED IT.\n");
        print("\n");
    }
    print("\n");
    as = "PICADO";
    bs = "RES";
    setup_helpers();
    d[1] = c;
    as = "TOREAD";
    bs = "ORES";
    setup_helpers();
    d[2] = c;
    print("\n");
    print("\n");
    z = 0;
    while (z == 0) {
        d[3]++;
        print("PASS NUMBER " + d[3] + "\n");
        if (d[3] >= 3) {
            print("HERE COMES THE BULL.  TRY FOR A KILL");
            while (1) {
                str = await input();
                if (str != "YES" && str != "NO")
                    print("INCORRECT ANSWER - - PLEASE TYPE 'YES' OR 'NO'.\n");
                else
                    break;
            }
            z1 = (str == "YES") ? 1 : 2;
            if (z1 != 1) {
                print("CAPE MOVE");
            }
        } else {
            print("THE BULL IS CHARGING AT YOU!  YOU ARE THE MATADOR--\n");
            print("DO YOU WANT TO KILL THE BULL");
            while (1) {
                str = await input();
                if (str != "YES" && str != "NO")
                    print("INCORRECT ANSWER - - PLEASE TYPE 'YES' OR 'NO'.\n");
                else
                    break;
            }
            z1 = (str == "YES") ? 1 : 2;
            if (z1 != 1) {
                print("WHAT MOVE DO YOU MAKE WITH THE CAPE");
            }
        }
        gore = 0;
        if (z1 != 1) {
            while (1) {
                e = parseInt(await input());
                if (e >= 3) {
                    print("DON'T PANIC, YOU IDIOT!  PUT DOWN A CORRECT NUMBER\n");
                } else {
                    break;
                }
            }
            if (e == 0)
                m = 3;
            else if (e == 1)
                m = 2;
            else
                m = 0.5;
            l += m;
            f = (6 - a + m / 10) * Math.random() / ((d[1] + d[2] + d[3] / 10) * 5);
            if (f < 0.51)
                continue;
            gore = 1;
        } else {
            z = 1;
            print("\n");
            print("IT IS THE MOMENT OF THE TRUTH.\n");
            print("\n");
            print("HOW DO YOU TRY TO KILL THE BULL");
            h = parseInt(await input());
            if (h != 4 && h != 5) {
                print("YOU PANICKED.  THE BULL GORED YOU.\n");
                gore = 2;
            } else {
                k = (6 - a) * 10 * Math.random() / ((d[1] + d[2]) * 5 * d[3]);
                if (h != 4) {   // Bug in original game, it says J instead of H
                    if (k > 0.2)
                        gore = 1;
                } else {
                    if (k > 0.8)
                        gore = 1;
                }
                if (gore == 0) {
                    print("YOU KILLED THE BULL!\n");
                    d[5] = 2;
                    break;
                }
            }
        }
        if (gore) {
            if (gore == 1)
                print("THE BULL HAS GORED YOU!\n");
            kill = false;
            while (1) {
                if (af(0) == 1) {
                    print("YOU ARE DEAD.\n");
                    d[4] = 1.5;
                    kill = true;
                    break;
                }
                print("YOU ARE STILL ALIVE.\n");
                print("\n");
                print("DO YOU RUN FROM THE RING");
                while (1) {
                    str = await input();
                    if (str != "YES" && str != "NO")
                        print("INCORRECT ANSWER - - PLEASE TYPE 'YES' OR 'NO'.\n");
                    else
                        break;
                }
                z1 = (str == "YES") ? 1 : 2;
                if (z1 != 2) {
                    print("COWARD\n");
                    d[4] = 0;
                    kill = true;
                    break;
                }
                print("YOU ARE BRAVE.  STUPID, BUT BRAVE.\n");
                if (af(0) == 1) {
                    d[4] = 2;
                    kill = false;
                    break;
                }
                print("YOU ARE GORED AGAIN!\n");
            }
            if (kill)
                break;
            continue;
        }
    }
    print("\n");
    print("\n");
    print("\n");
    if (d[4] == 0) {
        print("THE CROWD BOOS FOR TEN MINUTES.  IF YOU EVER DARE TO SHOW\n");
        print("YOUR FACE IN A RING AGAIN, THEY SWEAR THEY WILL KILL YOU--\n");
        print("UNLESS THE BULL DOES FIRST.\n");
    } else {
        if (d[4] == 2) {
            print("THE CROWD CHEERS WILDLY!\n");
        } else if (d[5] == 2) {
            print("THE CROWD CHEERS!\n");
            print("\n");
        }
        print("THE CROWD AWARDS YOU\n");
        if (cf(0) < 2.4) {
            print("NOTHING AT ALL.\n");
        } else if (cf(0) < 4.9) {
            print("ONE EAR OF THE BULL.\n");
        } else if (cf(0) < 7.4) {
            print("BOTH EARS OF THE BULL!\n");
            print("OLE!\n");
        } else {
            print("OLE!  YOU ARE 'MUY HOMBRE'!! OLE!  OLE!\n");
        }
        print("\n");
        print("ADIOS\n");
        print("\n");
        print("\n");
        print("\n");
    }
}


main();
