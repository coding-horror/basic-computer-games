// GOMOKO
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

function reset_stats()
{
    for (var j = 1; j <= 4; j++)
        f[j] = 0;
}

var a = [];
var x;
var y;
var n;

// *** PRINT THE BOARD ***
function print_board()
{
    for (i = 1; i <= n; i++) {
        for (j = 1; j <= n; j++) {
            print(" " + a[i][j] + " ");
        }
        print("\n");
    }
    print("\n");
}

// Is valid the movement
function is_valid()
{
    if (x < 1 || x > n || y < 1 || y > n)
        return false;
    return true;
}

// Main program
async function main()
{
    print(tab(33) + "GOMOKO\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    for (i = 0; i <= 19; i++) {
        a[i] = [];
        for (j = 0; j <= 19; j++)
            a[i][j] = 0;
    }
    print("WELCOME TO THE ORIENTAL GAME OF GOMOKO.\n");
    print("\n");
    print("THE GAME IS PLAYED ON AN N BY N GRID OF A SIZE\n");
    print("THAT YOU SPECIFY.  DURING YOUR PLAY, YOU MAY COVER ONE GRID\n");
    print("INTERSECTION WITH A MARKER. THE OBJECT OF THE GAME IS TO GET\n");
    print("5 ADJACENT MARKERS IN A ROW -- HORIZONTALLY, VERTICALLY, OR\n");
    print("DIAGONALLY.  ON THE BOARD DIAGRAM, YOUR MOVES ARE MARKED\n");
    print("WITH A '1' AND THE COMPUTER MOVES WITH A '2'.\n");
    print("\n");
    print("THE COMPUTER DOES NOT KEEP TRACK OF WHO HAS WON.\n");
    print("TO END THE GAME, TYPE -1,-1 FOR YOUR MOVE.\n");
    print("\n");
    while (1) {
        print("WHAT IS YOUR BOARD SIZE (MIN 7/ MAX 19)");
        while (1) {
            n = parseInt(await input());
            if (n >= 7 && n<= 19)
                break;
            print("I SAID, THE MINIMUM IS 7, THE MAXIMUM IS 19.\n");
        }
        for (i = 1; i <= n; i++) {
            for (j = 1; j <= n; j++) {
                a[i][j] = 0;
            }
        }
        print("\n");
        print("WE ALTERNATE MOVES.  YOU GO FIRST...\n");
        print("\n");
        while (1) {
            print("YOUR PLAY (I,J)");
            str = await input();
            i = parseInt(str);
            j = parseInt(str.substr(str.indexOf(",") + 1));
            print("\n");
            if (i == -1)
                break;
            x = i;
            y = j;
            if (!is_valid()) {
                print("ILLEGAL MOVE.  TRY AGAIN...\n");
                continue;
            }
            if (a[i][j] != 0) {
                print("SQUARE OCCUPIED.  TRY AGAIN...\n");
                continue;
            }
            a[i][j] = 1;
            // *** Computer tries an intelligent move ***
            found = false;
            for (e = -1; e <= 1; e++) {
                for (f = -1; f <= 1; f++) {
                    if (e + f - e * f == 0)
                        continue;
                    x = i + f;
                    y = j + f;
                    if (!is_valid())
                        continue;
                    if (a[x][y] == 1) {
                        x = i - e;
                        y = j - f;
                        if (is_valid() || a[x][y] == 0)
                            found = true;
                        break;
                    }
                }
            }
            if (!found) {
                // *** Computer tries a random move ***
                do {
                    x = Math.floor(n * Math.random() + 1);
                    y = Math.floor(n * Math.random() + 1);
                } while (!is_valid() || a[x][y] != 0) ;
            }
            a[x][y] = 2;
            print_board();
        }
        print("\n");
        print("THANKS FOR THE GAME!!\n");
        print("PLAY AGAIN (1 FOR YES, 0 FOR NO)");
        q = parseInt(await input());
        if (q != 1)
            break;
    }
}

main();
