// H-I-Q
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

var b = [];
var t = [];
var m = [,13,14,15,
          22,23,24,
    29,30,31,32,33,34,35,
    38,39,40,41,42,43,44,
    47,48,49,50,51,52,53,
          58,59,60,
          67,68,69];
var z;
var p;

//
// Print board
//
function print_board()
{
    for (x = 1; x <= 9; x++) {
        str = "";
        for (y = 1; y <= 9; y++) {
            if (x == 1 || x == 9 || y == 1 || y == 9)
                continue;
            if (x == 4 || x == 5 || x == 6 || y == 4 || y == 5 || y == 6) {
                while (str.length < y * 2)
                    str += " ";
                if (t[x][y] == 5)
                    str += "!";
                else
                    str += "O";
            }
        }
        print(str + "\n");
    }
}

//
// Update board
//
function update_board()
{
    c = 1;
    for (var x = 1; x <= 9; x++) {
        for (var y = 1; y <= 9; y++, c++) {
            if (c != z)
                continue;
            if (c + 2 == p) {
                if (t[x][y + 1] == 0)
                    return false;
                t[x][y + 2] = 5;
                t[x][y + 1] = 0;
                b[c + 1] = -3;
            } else if (c + 18 == p) {
                if (t[x + 1][y] == 0)
                    return false;
                t[x + 2][y] = 5;
                t[x + 1][y] = 0;
                b[c + 9] = -3;
            } else if (c - 2 == p) {
                if (t[x][y - 1] == 0)
                    return false;
                t[x][y - 2] = 5;
                t[x][y - 1] = 0;
                b[c - 1] = -3;
            } else if (c - 18 == p) {
                if (t[x - 1][y] == 0)
                    return false;
                t[x - 2][y] = 5;
                t[x - 1][y] = 0;
                b[c - 9] = -3;
            } else {
                continue;
            }
            b[z] = -3;
            b[p] = -7;
            t[x][y] = 0;
            return true;
        }
    }
}

//
// Check for game over
//
// Rewritten because original subroutine was buggy
//
function check_game_over()
{
    f = 0;
    for (r = 2; r <= 8; r++) {
        for (c = 2; c <= 8; c++) {
            if (t[r][c] != 5)
                continue;
            f++;
            if (r > 3 && t[r - 1][c] == 5 && t[r - 2][c] == 0)
                return false;
            if (c > 3 && t[r][c - 1] == 5 && t[r][c - 2] == 0)
                return false;
            if (r < 7 && t[r + 1][c] == 5 && t[r + 2][c] == 0)
                return false;
            if (c < 7 && t[r][c + 1] == 5 && t[r][c + 2] == 0)
                return false;
        }
    }
    return true;
}

// Main program
async function main()
{
    print(tab(33) + "H-I-Q\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    for (r = 0; r <= 70; r++)
        b[r] = 0;
    print("HERE IS THE BOARD:\n");
    print("\n");
    print("          !    !    !\n");
    print("         13   14   15\n");
    print("\n");
    print("          !    !    !\n");
    print("         22   23   24\n");
    print("\n");
    print("!    !    !    !    !    !    !\n");
    print("29   30   31   32   33   34   35\n");
    print("\n");
    print("!    !    !    !    !    !    !\n");
    print("38   39   40   41   42   43   44\n");
    print("\n");
    print("!    !    !    !    !    !    !\n");
    print("47   48   49   50   51   52   53\n");
    print("\n");
    print("          !    !    !\n");
    print("         58   59   60\n");
    print("\n");
    print("          !    !    !\n");
    print("         67   68   69\n");
    print("\n");
    print("TO SAVE TYPING TIME, A COMPRESSED VERSION OF THE GAME BOARD\n");
    print("WILL BE USED DURING PLAY.  REFER TO THE ABOVE ONE FOR PEG\n");
    print("NUMBERS.  OK, LET'S BEGIN.\n");
    while (1) {
        // Set up board
        for (r = 1; r <= 9; r++) {
            t[r] = [];
            for (c = 1; c <= 9; c++) {
                if (r == 4 || r == 5 || r == 6 || c == 4 || c == 5 || c == 6 && (r != 1 && c != 1 && r != 9 && c != 9)) {
                    t[r][c] = 5;
                } else {
                    t[r][c] = -5;
                }
            }
        }
        t[5][5] = 0;
        print_board();
        // Init secondary board
        for (w = 1; w <= 33; w++) {
            b[m[w]] = -7;
        }
        b[41] = -3;
        // Input move and check on legality
        do {
            while (1) {
                print("MOVE WHICH PIECE");
                z = parseInt(await input());
                if (b[z] == -7) {
                    print("TO WHERE");
                    p = parseInt(await input());
                    if (p != z
                        && b[p] != 0
                        && b[p] != -7
                        && (z + p) % 2 == 0
                        && (Math.abs(z - p) - 2) * (Math.abs(z - p) - 18) == 0
                        && update_board())
                        break;
                }
                print("ILLEGAL MOVE, TRY AGAIN...\n");
            }
            print_board();
        } while (!check_game_over()) ;
        // Game is over
        print("THE GAME IS OVER.\n");
        print("YOU HAD " + f + " PIECES REMAINING.\n");
        if (f == 1) {
            print("BRAVO!  YOU MADE A PERFECT SCORE!\n");
            print("SAVE THIS PAPER AS A RECORD OF YOUR ACCOMPLISHMENT!\n");
        }
        print("\n");
        print("PLAY AGAIN (YES OR NO)");
        str = await input();
        if (str == "NO")
            break;
    }
    print("\n");
    print("SO LONG FOR NOW.\n");
    print("\n");
}

main();
