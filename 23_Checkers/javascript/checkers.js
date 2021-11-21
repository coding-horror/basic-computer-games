// CHECKERS
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

// x,y = origin square
// a,b = movement direction
function try_computer()
{
    u = x + a;
    v = y + b;
    if (u < 0 || u > 7 || v < 0 || v > 7)
        return;
    if (s[u][v] == 0) {
        eval_move();
        return;
    }
    if (s[u][v] < 0)	// Cannot jump over own pieces
        return;
    u += a;
    u += b;
    if (u < 0 || u > 7 || v < 0 || v > 7)
        return;
    if (s[u][v] == 0)
        eval_move();
}

// x,y = origin square
// u,v = target square
function eval_move()
{
    if (v == 0 && s[x][y] == -1)
        q += 2;
    if (Math.abs(y - v) == 2)
        q += 5;
    if (y == 7)
        q -= 2;
    if (u == 0 || u == 7)
        q++;
    for (c = -1; c <= 1; c += 2) {
        if (u + c < 0 || u + c > 7 || v + g < 0)
            continue;
        if (s[u + c][v + g] < 0) {	// Computer piece
            q++;
            continue;
        }
        if (u - c < 0 || u - c > 7 || v - g > 7)
            continue;
        if (s[u + c][v + g] > 0 && (s[u - c][v - g] == 0 || (u - c == x && v - g == y)))
            q -= 2;
    }
    if (q > r[0]) {	// Best movement so far?
        r[0] = q;	// Take note of score
        r[1] = x;	// Origin square
        r[2] = y;
        r[3] = u;	// Target square
        r[4] = v;
    }
    q = 0;
}

function more_captures() {
    u = x + a;
    v = y + b;
    if (u < 0 || u > 7 || v < 0 || v > 7)
        return;
    if (s[u][v] == 0 && s[x + a / 2][y + b / 2] > 0)
        eval_move();
}

var r = [-99, 0, 0, 0, 0];
var s = [];

for (x = 0; x <= 7; x++)
    s[x] = [];
    
var g = -1;
var data = [1, 0, 1, 0, 0, 0, -1, 0, 0, 1, 0, 0, 0, -1, 0, -1, 15];
var p = 0;
var q = 0;

// Main program
async function main()
{
    print(tab(32) + "CHECKERS\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS IS THE GAME OF CHECKERS.  THE COMPUTER IS X,\n");
    print("AND YOU ARE O.  THE COMPUTER WILL MOVE FIRST.\n");
    print("SQUARES ARE REFERRED TO BY A COORDINATE SYSTEM.\n");
    print("(0,0) IS THE LOWER LEFT CORNER\n");
    print("(0,7) IS THE UPPER LEFT CORNER\n");
    print("(7,0) IS THE LOWER RIGHT CORNER\n");
    print("(7,7) IS THE UPPER RIGHT CORNER\n");
    print("THE COMPUTER WILL TYPE '+TO' WHEN YOU HAVE ANOTHER\n");
    print("JUMP.  TYPE TWO NEGATIVE NUMBERS IF YOU CANNOT JUMP.\n");
    print("\n");
    print("\n");
    print("\n");
    for (x = 0; x <= 7; x++) {
        for (y = 0; y <= 7; y++) {
            if (data[p] == 15)
                p = 0;
            s[x][y] = data[p];
            p++;
        }
    }
    while (1) {

        // Search the board for the best movement
        for (x = 0; x <= 7; x++) {
            for (y = 0; y <= 7; y++) {
                if (s[x][y] > -1)
                    continue;
                if (s[x][y] == -1) {	// Piece
                    for (a = -1; a <= 1; a += 2) {
                        b = g;	// Only advances
                        try_computer();
                    }
                } else if (s[x][y] == -2) {	// King
                    for (a = -1; a <= 1; a += 2) {
                        for (b = -1; b <= 1; b += 2) {
                            try_computer();
                        }
                    }
                }
            }
        }
        if (r[0] == -99) {
            print("\n");
            print("YOU WIN.\n");
            break;
        }
        print("FROM " + r[1] + "," + r[2] + " TO " + r[3] + "," + r[4]);
        r[0] = -99;
        while (1) {
            if (r[4] == 0) {	// Computer reaches the bottom
                s[r[3]][r[4]] = -2;	// King
                break;
            }
            s[r[3]][r[4]] = s[r[1]][r[2]];	// Move
            s[r[1]][r[2]] = 0;
            if (Math.abs(r[1] - r[3]) == 2) {
                s[(r[1] + r[3]) / 2][(r[2] + r[4]) / 2] = 0;	// Capture
                x = r[3];
                y = r[4];
                if (s[x][y] == -1) {
                    b = -2;
                    for (a = -2; a <= 2; a += 4) {
                        more_captures();
                    }
                } else if (s[x][y] == -2) {
                    for (a = -2; a <= 2; a += 4) {
                        for (b = -2; b <= 2; b += 4) {
                            more_captures();
                        }
                    }
                }
                if (r[0] != -99) {
                    print(" TO " + r[3] + "," + r[4]);
                    r[0] = -99;
                    continue;
                }
            }
            break;
        }
        print("\n");
        print("\n");
        print("\n");
        for (y = 7; y >= 0; y--) {
            str = "";
            for (x = 0; x <= 7; x++) {
                if (s[x][y] == 0)
                    str += ".";
                if (s[x][y] == 1)
                    str += "O";
                if (s[x][y] == -1)
                    str += "X";
                if (s[x][y] == -2)
                    str += "X*";
                if (s[x][y] == 2)
                    str += "O*";
                while (str.length % 5)
                    str += " ";
            }
            print(str + "\n");
            print("\n");
        }
        print("\n");
        z = 0;
        t = 0;
        for (l = 0; l <= 7; l++) {
            for (m = 0; m <= 7; m++) {
                if (s[l][m] == 1 || s[l][m] == 2)
                    z = 1;
                if (s[l][m] == -1 || s[l][m] == -2)
                    t = 1;
            }
        }
        if (z != 1) {
            print("\n");
            print("I WIN.\n");
            break;
        }
        if (t != 1) {
            print("\n");
            print("YOU WIN.\n");
            break;
        }
        do {
            print("FROM");
            e = await input();
            h = parseInt(e.substr(e.indexOf(",") + 1));
            e = parseInt(e);
            x = e;
            y = h;
        } while (s[x][y] <= 0) ;
        do {
            print("TO");
            a = await input();
            b = parseInt(a.substr(a.indexOf(",") + 1));
            a = parseInt(a);
            x = a;
            y = b;
            if (s[x][y] == 0 && Math.abs(a - e) <= 2 && Math.abs(a - e) == Math.abs(b - h))
                break;
            print("WHAT?\n");
        } while (1) ;
        i = 46;
        do {
            s[a][b] = s[e][h]
            s[e][h] = 0;
            if (Math.abs(e - a) != 2)
                break;
            s[(e + a) / 2][(h + b) / 2] = 0;
            while (1) {
                print("+TO");
                a1 = await input();
                b1 = parseInt(a1.substr(a1.indexOf(",") + 1));
                a1 = parseInt(a1);
                if (a1 < 0)
                    break;
                if (s[a1][b1] == 0 && Math.abs(a1 - a) == 2 && Math.abs(b1 - b) == 2)
                    break;
            }
            if (a1 < 0)
                break;
            e = a;
            h = b;
            a = a1;
            b = b1;
            i += 15;
        } while (1);
        if (b == 7)	// Player reaches top
            s[a][b] = 2;	// Convert to king
    }
}

main();
