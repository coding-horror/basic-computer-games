// HORSERACE
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

var sa = [];
var ws = [];
var da = [];
var qa = [];
var pa = [];
var ma = [];
var ya = [];
var vs = [];

// Main program
async function main()
{
    print(tab(31) + "HORSERACE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("WELCOME TO SOUTH PORTLAND HIGH RACETRACK\n");
    print("                      ...OWNED BY LAURIE CHEVALIER\n");
    print("DO YOU WANT DIRECTIONS");
    str = await input();
    if (str == "YES") {
        print("UP TO 10 MAY PLAY.  A TABLE OF ODDS WILL BE PRINTED.  YOU\n");
        print("MAY BET ANY + AMOUNT UNDER 100000 ON ONE HORSE.\n");
        print("DURING THE RACE, A HORSE WILL BE SHOWN BY ITS\n");
        print("NUMBER.  THE HORSES RACE DOWN THE PAPER!\n");
        print("\n");
    }
    print("HOW MANY WANT TO BET");
    c = parseInt(await input());
    print("WHEN ? APPEARS,TYPE NAME\n");
    for (a = 1; a <= c; a++) {
        ws[a] = await input();
    }
    do {
        print("\n");
        print("HORSE\t\tNUMBERS\tODDS\n");
        print("\n");
        for (i = 1; i <= 8; i++) {
            sa[i] = 0;
        }
        r = 0;
        for (a = 1; a <= 8; a++) {
            da[a] = Math.floor(10 * Math.random() + 1);
        }
        for (a = 1; a <= 8; a++) {
            r = r + da[a];
        }
        vs[1] = "JOE MAN";
        vs[2] = "L.B.J.";
        vs[3] = "MR.WASHBURN";
        vs[4] = "MISS KAREN";
        vs[5] = "JOLLY";
        vs[6] = "HORSE";
        vs[7] = "JELLY DO NOT";
        vs[8] = "MIDNIGHT";
        for (n = 1; n <= 8; n++) {
            print(vs[n] + "\t\t" + n + "\t" + (r / da[n]) + ":1\n");
        }
        print("--------------------------------------------------\n");
        print("PLACE YOUR BETS...HORSE # THEN AMOUNT\n");
        for (j = 1; j <= c; j++) {
            while (1) {
                print(ws[j]);
                str = await input();
                qa[j] = parseInt(str);
                pa[j] = parseInt(str.substr(str.indexOf(",") + 1));
                if (pa[j] < 1 || pa[j] >= 100000) {
                    print("  YOU CAN'T DO THAT!\N");
                } else {
                    break;
                }
            }
        }
        print("\n");
        print("1 2 3 4 5 6 7 8\n");
        t = 0;
        do {
            print("XXXXSTARTXXXX\n");
            for (i = 1; i <= 8; i++) {
                m = i;
                ma[i] = m;
                ya[ma[i]] = Math.floor(100 * Math.random() + 1);
                if (ya[ma[i]] < 10) {
                    ya[ma[i]] = 1;
                    continue;
                }
                s = Math.floor(r / da[i] + 0.5);
                if (ya[ma[i]] < s + 17) {
                    ya[ma[i]] = 2;
                    continue;
                }
                if (ya[ma[i]] < s + 37) {
                    ya[ma[i]] = 3;
                    continue;
                }
                if (ya[ma[i]] < s + 57) {
                    ya[ma[i]] = 4;
                    continue;
                }
                if (ya[ma[i]] < s + 77) {
                    ya[ma[i]] = 5;
                    continue;
                }
                if (ya[ma[i]] < s + 92) {
                    ya[ma[i]] = 6;
                    continue;
                }
                ya[ma[i]] = 7;
            }
            m = i;
            for (i = 1; i <= 8; i++) {
                sa[ma[i]] = sa[ma[i]] + ya[ma[i]];
            }
            i = 1;
            for (l = 1; l <= 8; l++) {
                for (i = 1; i <= 8 - l; i++) {
                    if (sa[ma[i]] < sa[ma[i + 1]])
                        continue;
                    h = ma[i];
                    ma[i] = ma[i + 1];
                    ma[i + 1] = h;
                }
            }
            t = sa[ma[8]];
            for (i = 1; i <= 8; i++) {
                b = sa[ma[i]] - sa[ma[i - 1]];
                if (b != 0) {
                    for (a = 1; a <= b; a++) {
                        print("\n");
                        if (sa[ma[i]] > 27)
                            break;
                    }
                    if (a <= b)
                        break;
                }
                print(" " + ma[i] + " ");
            }
            for (a = 1; a < 28 - t; a++) {
                print("\n");
            }
            print("XXXXFINISHXXXX\n");
            print("\n");
            print("\n");
            print("---------------------------------------------\n");
            print("\n");
        } while (t < 28) ;
        print("THE RACE RESULTS ARE:\n");
        z9 = 1;
        for (i = 8; i >= 1; i--) {
            f = ma[i];
            print("\n");
            print("" + z9 + " PLACE HORSE NO. " + f + " AT " + (r / da[f]) + ":1\n");
            z9++;
        }
        for (j = 1; j <= c; j++) {
            if (qa[j] != ma[8])
                continue;
            n = qa[j];
            print("\n");
            print(ws[j] + " WINS $" + (r / da[n]) * pa[j] + "\n");
        }
        print("DO YOU WANT TO BET ON THE NEXT RACE ?\n");
        print("YES OR NO");
        str = await input();
    } while (str == "YES") ;
}

main();
