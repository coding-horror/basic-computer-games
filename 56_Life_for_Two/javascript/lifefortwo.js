// LIFE FOR TWO
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

var na = [];
var ka = [, 3,102,103,120,130,121,112,111,12,
          21,30,1020,1030,1011,1021,1003,1002,1012];
var aa = [,-1,0,1,0,0,-1,0,1,-1,-1,1,-1,-1,1,1,1];
var xa = [];
var ya = [];
var j;
var k;
var m2;
var m3;

function show_data()
{
    k = 0;
    m2 = 0;
    m3 = 0;
    for (j = 0; j <= 6; j++) {
        print("\n");
        for (k = 0; k <= 6; k++) {
            if (j == 0 || j == 6) {
                if (k == 6)
                    print(" 0 ");
                else
                    print(" " + k + " ");
            } else if (k == 0 || k == 6) {
                if (j == 6)
                    print(" 0\n");
                else
                    print(" " + j + " ");
            } else {
                if (na[j][k] >= 3) {
                    for (o1 = 1; o1 <= 18; o1++) {
                        if (na[j][k] == ka[o1])
                            break;
                    }
                    if (o1 <= 18) {
                        if (o1 <= 9) {
                            na[j][k] = 100;
                            m2++;
                            print(" * ");
                        } else {
                            na[j][k] = 1000;
                            m3++;
                            print(" # ");
                        }
                    } else {
                        na[j][k] = 0;
                        print("   ");
                    }
                } else {
                    na[j][k] = 0;
                    print("   ");
                }
            }
        }
    }
}

function process_board()
{
    for (j = 1; j <= 5; j++) {
        for (k = 1; k <= 5; k++) {
            if (na[j][k] > 99) {
                b = 1;
                if (na[j][k] > 999)
                    b = 10;
                for (o1 = 1; o1 <= 15; o1 += 2) {
                    na[j + aa[o1]][k + aa[o1 + 1]] = na[j + aa[o1]][k + aa[o1 + 1]] + b;
                }
            }
        }
    }
    show_data();
}

// Main program
async function main()
{
    print(tab(33) + "LIFE2\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print(tab(10) + "U.B. LIFE GAME\n");
    m2 = 0;
    m3 = 0;
    for (j = 0; j <= 6; j++) {
        na[j] = [];
        for (k = 0; k <= 6; k++)
            na[j][k] = 0;
    }
    for (b = 1; b <= 2; b++) {
        p1 = (b == 2) ? 30 : 3;
        print("\n");
        print("PLAYER " + b + " - 3 LIVE PIECES.\n");
        for (k1 = 1; k1 <= 3; k1++) {
            while (1) {
                print("X,Y\n");
                str = await input();
                ya[b] = parseInt(str);
                xa[b] = parseInt(str.substr(str.indexOf(",") + 1));
                if (xa[b] > 0 && xa[b] < 6 && ya[b] > 0 && ya[b] < 5 && na[xa[b]][ya[b]] == 0)
                    break;
                print("ILLEGAL COORDS. RETYPE\n");
            }
            if (b != 1) {
                if (xa[1] == xa[2] && ya[1] == ya[2]) {
                    print("SAME COORD.  SET TO 0\n");
                    na[xa[b] + 1][ya[b] + 1] = 0;
                    b = 99;
                }
            }
            na[xa[b]][ya[b]] = p1;
        }
    }
    show_data();
    while (1) {
        print("\n");
        process_board();
        if (m2 == 0 && m3 == 0) {
            print("\n");
            print("A DRAW\n");
            break;
        }
        if (m3 == 0) {
            print("\n");
            print("PLAYER 1 IS THE WINNER\n");
            break;
        }
        if (m2 == 0) {
            print("\n");
            print("PLAYER 2 IS THE WINNER\n");
            break;
        }
        for (b = 1; b <= 2; b++) {
            print("\n");
            print("\n");
            print("PLAYER " + b + " ");
            while (1) {
                print("X,Y\n");
                str = await input();
                ya[b] = parseInt(str);
                xa[b] = parseInt(str.substr(str.indexOf(",") + 1));
                if (xa[b] > 0 && xa[b] < 6 && ya[b] > 0 && ya[b] < 5 && na[xa[b]][ya[b]] == 0)
                    break;
                print("ILLEGAL COORDS. RETYPE\n");
            }
            if (b != 1) {
                if (xa[1] == xa[2] && ya[1] == ya[2]) {
                    print("SAME COORD.  SET TO 0\n");
                    na[xa[b] + 1][ya[b] + 1] = 0;
                    b = 99;
                }
            }
            if (b == 99)
                break;
        }
        if (b <= 2) {
            na[x[1]][y[1]] = 100;
            na[x[2]][y[2]] = 1000;
        }
    }
}

main();
