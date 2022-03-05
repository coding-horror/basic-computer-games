// MASTERMIND
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

var p9;
var c9;
var b;
var w;
var f;
var m;

var qa;
var sa;
var ss;
var as;
var gs;
var hs;

function initialize_qa()
{
    for (s = 1; s <= p9; s++)
        qa[s] = 0;
}

function increment_qa()
{
    if (qa[1] <= 0) {
        // If zero, this is our firt increment: make all ones
        for (s = 1; s <= p9; s++)
            qa[s] = 1;
    } else {
        q = 1;
        while (1) {
            qa[q] = qa[q] + 1;
            if (qa[q] <= c9)
                return;
            qa[q] = 1;
            q++;
        }
    }
}

function convert_qa()
{
    for (s = 1; s <= p9; s++) {
        as[s] = ls.substr(qa[s] - 1, 1);
    }
}

function get_number()
{
    b = 0;
    w = 0;
    f = 0;
    for (s = 1; s <= p9; s++) {
        if (gs[s] == as[s]) {
            b++;
            gs[s] = String.fromCharCode(f);
            as[s] = String.fromCharCode(f + 1);
            f += 2;
        } else {
            for (t = 1; t <= p9; t++) {
                if (gs[s] == as[t] && gs[t] != as[t]) {
                    w++;
                    as[t] = String.fromCharCode(f);
                    gs[s] = String.fromCharCode(f + 1);
                    f += 2;
                    break;
                }
            }
        }
    }
}

function convert_qa_hs()
{
    for (s = 1; s <= p9; s++) {
        hs[s] = ls.substr(qa[s] - 1, 1);
    }
}

function copy_hs()
{
    for (s = 1; s <= p9; s++) {
        gs[s] = hs[s];
    }
}

function board_printout()
{
    print("\n");
    print("BOARD\n");
    print("MOVE     GUESS          BLACK     WHITE\n");
    for (z = 1; z <= m - 1; z++) {
        str = " " + z + " ";
        while (str.length < 9)
            str += " ";
        str += ss[z];
        while (str.length < 25)
            str += " ";
        str += sa[z][1];
        while (str.length < 35)
            str += " ";
        str += sa[z][2];
        print(str + "\n");
    }
    print("\n");
}

function quit()
{
    print("QUITTER!  MY COMBINATION WAS: ");
    convert_qa();
    for (x = 1; x <= p9; x++) {
        print(as[x]);
    }
    print("\n");
    print("GOOD BYE\n");
}

function show_score()
{
    print("SCORE:\n");
    show_points();
}

function show_points()
{
    print("     COMPUTER " + c + "\n");
    print("     HUMAN    " + h + "\n");
    print("\n");
}

var color = ["BLACK", "WHITE", "RED", "GREEN",
             "ORANGE", "YELLOW", "PURPLE", "TAN"];

// Main program
async function main()
{
    print(tab(30) + "MASTERMIND\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    //
    //  MASTERMIND II
    //  STEVE NORTH
    //  CREATIVE COMPUTING
    //  PO BOX 789-M MORRISTOWN NEW JERSEY 07960
    //
    //
    while (1) {
        print("NUMBER OF COLORS");
        c9 = parseInt(await input());
        if (c9 <= 8)
            break;
        print("NO MORE THAN 8, PLEASE!\n");
    }
    print("NUMBER OF POSITIONS");
    p9 = parseInt(await input());
    print("NUMBER OF ROUNDS");
    r9 = parseInt(await input());
    p = Math.pow(c9, p9);
    print("TOTAL POSSIBILITIES = " + p + "\n");
    h = 0;
    c = 0;
    qa = [];
    sa = [];
    ss = [];
    as = [];
    gs = [];
    ia = [];
    hs = [];
    ls = "BWRGOYPT";
    print("\n");
    print("\n");
    print("COLOR    LETTER\n");
    print("=====    ======\n");
    for (x = 1; x <= c9; x++) {
        str = color[x - 1];
        while (str.length < 13)
            str += " ";
        str += ls.substr(x - 1, 1);
        print(str + "\n");
    }
    print("\n");
    for (r = 1; r <= r9; r++) {
        print("\n");
        print("ROUND NUMBER " + r + " ----\n");
        print("\n");
        print("GUESS MY COMBINATION.\n");
        print("\n");
        // Get a combination
        a = Math.floor(p * Math.random() + 1);
        initialize_qa();
        for (x = 1; x <= a; x++) {
            increment_qa();
        }
        for (m = 1; m <= 10; m++) {
            while (1) {
                print("MOVE # " + m + " GUESS ");
                str = await input();
                if (str == "BOARD") {
                    board_printout();
                } else if (str == "QUIT") {
                    quit();
                    return;
                } else if (str.length != p9) {
                    print("BAD NUMBER OF POSITIONS.\n");
                } else {
                    // Unpack str into gs(1-p9)
                    for (x = 1; x <= p9; x++) {
                        y = ls.indexOf(str.substr(x - 1, 1));
                        if (y < 0) {
                            print("'" + str.substr(x - 1, 1) + "' IS UNRECOGNIZED.\n");
                            break;
                        }
                        gs[x] = str.substr(x - 1, 1);
                    }
                    if (x > p9)
                        break;
                }
            }
            // Now we convert qa(1-p9) into as(1-p9) [ACTUAL GUESS]
            convert_qa();
            // And get number of blacks and white
            get_number();
            if (b == p9) {
                print("YOU GUESSED IT IN " + m + " MOVES!\n");
                break;
            }
            // Save all this stuff for board printout later
            ss[m] = str;
            sa[m] = [];
            sa[m][1] = b;
            sa[m][2] = w;
        }
        if (m > 10) {
            print("YOU RAN OUT OF MOVES!  THAT'S ALL YOU GET!\n");
        }
        h += m;
        show_score();

        //
        // Now computer guesses
        //
        for (x = 1; x <= p; x++)
            ia[x] = 1;
        print("NOW I GUESS.  THINK OF A COMBINATION.\n");
        print("HIT RETURN WHEN READY:");
        str = await input();
        for (m = 1; m <= 10; m++) {
            initialize_qa();
            // Find a guess
            g = Math.floor(p * Math.random() + 1);
            if (ia[g] != 1) {
                for (x = g; x <= p; x++) {
                    if (ia[x] == 1)
                        break;
                }
                if (x > p) {
                    for (x = 1; x <= g; x++) {
                        if (ia[x] == 1)
                            break;
                    }
                    if (x > g) {
                        print("YOU HAVE GIVEN ME INCONSISTENT INFORMATION.\n");
                        print("TRY AGAIN, AND THIS TIME PLEASE BE MORE CAREFUL.\n");
                        for (x = 1; x <= p; x++)
                            ia[x] = 1;
                        print("NOW I GUESS.  THINK OF A COMBINATION.\n");
                        print("HIT RETURN WHEN READY:");
                        str = await input();
                        m = 0;
                        continue;
                    }
                }
                g = x;
            }
            // Now we convert guess #g into gs
            for (x = 1; x <= g; x++) {
                increment_qa();
            }
            convert_qa_hs();
            print("MY GUESS IS: ");
            for (x = 1; x <= p9; x++) {
                print(hs[x]);
            }
            print("  BLACKS, WHITES ");
            str = await input();
            b1 = parseInt(str);
            w1 = parseInt(str.substr(str.indexOf(",") + 1));
            if (b1 == p9) {
                print("I GOT IT IN " + m + " MOVES!\n");
                break;
            }
            initialize_qa();
            for (x = 1; x <= p; x++) {
                increment_qa();
                if (ia[x] != 0) {
                    copy_hs();
                    convert_qa();
                    get_number();
                    if (b1 != b || w1 != w)
                        ia[x] = 0;
                }
            }
        }
        if (m > 10) {
            print("I USED UP ALL MY MOVES!\n");
            print("I GUESS MY CPU I JUST HAVING AN OFF DAY.\n");
        }
        c += m;
        show_score();
    }
    print("GAME OVER\n");
    print("FINAL SCORE:\n");
    show_points();
}

main();
