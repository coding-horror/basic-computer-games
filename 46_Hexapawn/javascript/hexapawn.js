// HEXAPAWN
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

var ba = [,
          [,-1,-1,-1,1,0,0,0,1,1],
          [,-1,-1,-1,0,1,0,1,0,1],
          [,-1,0,-1,-1,1,0,0,0,1],
          [,0,-1,-1,1,-1,0,0,0,1],
          [,-1,0,-1,1,1,0,0,1,0],
          [,-1,-1,0,1,0,1,0,0,1],
          [,0,-1,-1,0,-1,1,1,0,0],
          [,0,-1,-1,-1,1,1,1,0,0],
          [,-1,0,-1,-1,0,1,0,1,0],
          [,0,-1,-1,0,1,0,0,0,1],
          [,0,-1,-1,0,1,0,1,0,0],
          [,-1,0,-1,1,0,0,0,0,1],
          [,0,0,-1,-1,-1,1,0,0,0],
          [,-1,0,0,1,1,1,0,0,0],
          [,0,-1,0,-1,1,1,0,0,0],
          [,-1,0,0,-1,-1,1,0,0,0],
          [,0,0,-1,-1,1,0,0,0,0],
          [,0,-1,0,1,-1,0,0,0,0],
          [,-1,0,0,-1,1,0,0,0,0]];
var ma = [,
          [,24,25,36,0],
          [,14,15,36,0],
          [,15,35,36,47],
          [,36,58,59,0],
          [,15,35,36,0],
          [,24,25,26,0],
          [,26,57,58,0],
          [,26,35,0,0],
          [,47,48,0,0],
          [,35,36,0,0],
          [,35,36,0,0],
          [,36,0,0,0],
          [,47,58,0,0],
          [,15,0,0,0],
          [,26,47,0,0],
          [,47,58,0,0],
          [,35,36,47,0],
          [,28,58,0,0],
          [,15,47,0,0]];
var s = [];
var t = [];
var ps = "X.O";

function show_board()
{
    print("\n");
    for (var i = 1; i <= 3; i++) {
        print(tab(10));
        for (var j = 1; j <= 3; j++) {
            print(ps[s[(i - 1) * 3 + j] + 1]);
        }
        print("\n");
    }
}

function mirror(x)
{
    if (x == 1)
        return 3;
    if (x == 3)
        return 1;
    if (x == 6)
        return 4;
    if (x == 4)
        return 6;
    if (x == 9)
        return 7;
    if (x == 7)
        return 9;
    return x;
}

// Main program
async function main()
{
    print(tab(32) + "HEXAPAWN\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    // HEXAPAWN:  INTERPRETATION OF HEXAPAWN GAME AS PRESENTED IN
    // MARTIN GARDNER'S "THE UNEXPECTED HANGING AND OTHER MATHEMATIC-
    // AL DIVERSIONS", CHAPTER EIGHT:  A MATCHBOX GAME-LEARNING MACHINE
    // ORIGINAL VERSION FOR H-P TIMESHARE SYSTEM BY R.A. KAAPKE 5/5/76
    // INSTRUCTIONS BY JEFF DALTON
    // CONVERSION TO MITS BASIC BY STEVE NORTH
    for (i = 0; i <= 9; i++) {
        s[i] = 0;
    }
    w = 0;
    l = 0;
    do {
        print("INSTRUCTIONS (Y-N)");
        str = await input();
        str = str.substr(0, 1);
    } while (str != "Y" && str != "N") ;
    if (str == "Y") {
        print("\n");
        print("THIS PROGRAM PLAYS THE GAME OF HEXAPAWN.\n");
        print("HEXAPAWN IS PLAYED WITH CHESS PAWNS ON A 3 BY 3 BOARD.\n");
        print("THE PAWNS ARE MOVED AS IN CHESS - ONE SPACE FORWARD TO\n");
        print("AN EMPTY SPACE OR ONE SPACE FORWARD AND DIAGONALLY TO\n");
        print("CAPTURE AN OPPOSING MAN.  ON THE BOARD, YOUR PAWNS\n");
        print("ARE 'O', THE COMPUTER'S PAWNS ARE 'X', AND EMPTY \n");
        print("SQUARES ARE '.'.  TO ENTER A MOVE, TYPE THE NUMBER OF\n");
        print("THE SQUARE YOU ARE MOVING FROM, FOLLOWED BY THE NUMBER\n");
        print("OF THE SQUARE YOU WILL MOVE TO.  THE NUMBERS MUST BE\n");
        print("SEPERATED BY A COMMA.\n");
        print("\n");
        print("THE COMPUTER STARTS A SERIES OF GAMES KNOWING ONLY WHEN\n");
        print("THE GAME IS WON (A DRAW IS IMPOSSIBLE) AND HOW TO MOVE.\n");
        print("IT HAS NO STRATEGY AT FIRST AND JUST MOVES RANDOMLY.\n");
        print("HOWEVER, IT LEARNS FROM EACH GAME.  THUS, WINNING BECOMES\n");
        print("MORE AND MORE DIFFICULT.  ALSO, TO HELP OFFSET YOUR\n");
        print("INITIAL ADVANTAGE, YOU WILL NOT BE TOLD HOW TO WIN THE\n");
        print("GAME BUT MUST LEARN THIS BY PLAYING.\n");
        print("\n");
        print("THE NUMBERING OF THE BOARD IS AS FOLLOWS:\n");
        print(tab(10) + "123\n");
        print(tab(10) + "456\n");
        print(tab(10) + "789\n");
        print("\n");
        print("FOR EXAMPLE, TO MOVE YOUR RIGHTMOST PAWN FORWARD,\n");
        print("YOU WOULD TYPE 9,6 IN RESPONSE TO THE QUESTION\n");
        print("'YOUR MOVE ?'.  SINCE I'M A GOOD SPORT, YOU'LL ALWAYS\n");
        print("GO FIRST.\n");
        print("\n");
    }
    while (1) {
        x = 0;
        y = 0;
        s[4] = 0;
        s[5] = 0;
        s[6] = 0;
        s[1] = -1;
        s[2] = -1;
        s[3] = -1;
        s[7] = 1;
        s[8] = 1;
        s[9] = 1;
        show_board();
        while (1) {
            while (1) {
                print("YOUR MOVE");
                str = await input();
                m1 = parseInt(str);
                m2 = parseInt(str.substr(str.indexOf(",") + 1));
                if (m1 > 0 && m1 < 10 && m2 > 0 && m2 < 10) {
                    if (s[m1] != 1 || s[m2] == 1 || (m2 - m1 != -3 && s[m2] != -1) || (m2 > m1) || (m2 - m1 == -3 && s[m2] != 0) || (m2 - m1 < -4) || (m1 == 7 && m2 == 3))
                        print("ILLEGAL MOVE.\n");
                    else
                        break;
                } else {
                    print("ILLEGAL CO-ORDINATES.\n");
                }
            }

            // Move player's pawn
            s[m1] = 0;
            s[m2] = 1;
            show_board();

            // Find computer pawns
            for (i = 1; i <= 9; i++) {
                if (s[i] == -1)
                    break;
            }
            // If none or player reached top then finish
            if (i > 9 || s[1] == 1 || s[2] == 1 || s[3] == 1) {
                computer = false;
                break;
            }
            // Find computer pawns with valid move
            for (i = 1; i <= 9; i++) {
                if (s[i] != -1)
                    continue;
                if (s[i + 3] == 0
                 || (mirror(i) == i && (s[i + 2] == 1 || s[i + 4] == 1))
                 || (i <= 3 && s[5] == 1)
                 || s[8] == 1)
                    break;
            }
            if (i > 9) {  // Finish if none possible
                computer = false;
                break;
            }
            for (i = 1; i <= 19; i++) {
                for (j = 1; j <= 3; j++) {
                    for (k = 3; k >= 1; k--) {
                        t[(j - 1) * 3 + k] = ba[i][(j - 1) * 3 + 4 - k];
                    }
                }
                for (j = 1; j <= 9; j++) {
                    if (s[j] != ba[i][j])
                        break;
                }
                if (j > 9) {
                    r = 0;
                    break;
                }
                for (j = 1; j <= 9; j++) {
                    if (s[j] != t[j])
                        break;
                }
                if (j > 9) {
                    r = 1;
                    break;
                }
            }
            if (i > 19) {
                print("ILLEGAL BOARD PATTERN\n");
                break;
            }
            x = i;
            for (i = 1; i <= 4; i++) {
                if (ma[x][i] != 0)
                    break;
            }
            if (i > 4) {
                print("I RESIGN.\n");
                computer = false;
                break;
            }
            // Select random move from possibilities
            do {
                y = Math.floor(Math.random() * 4 + 1);
            } while (ma[x][y] == 0) ;
            // Announce move
            if (r == 0) {
                print("I MOVE FROM " + Math.floor(ma[x][y] / 10) + " TO " + ma[x][y] % 10 + "\n");
                s[Math.floor(ma[x][y] / 10)] = 0;
                s[ma[x][y] % 10] = -1;
            } else {
                print("I MOVE FROM " + mirror(Math.floor(ma[x][y] / 10)) + " TO " + mirror(ma[x][y]) % 10 + "\n");
                s[mirror(Math.floor(ma[x][y] / 10))] = 0;
                s[mirror(ma[x][y] % 10)] = -1;
            }
            show_board();
            // Finish if computer reaches bottom
            if (s[7] == -1 || s[8] == -1 || s[9] == -1) {
                computer = true;
                break;
            }
            // Finish if no player pawns
            for (i = 1; i <= 9; i++) {
                if (s[i] == 1)
                    break;
            }
            if (i > 9) {
                computer = true;
                break;
            }
            // Finish if player cannot move
            for (i = 1; i <= 9; i++) {
                if (s[i] != 1)
                    continue;
                if (s[i - 3] == 0)
                    break;
                if (mirror(i) != i) {
                    if (i >= 7) {
                        if (s[5] == -1)
                            break;
                    } else {
                        if (s[2] == -1)
                            break;
                    }
                } else {
                    if (s[i - 2] == -1 || s[i - 4] == -1)
                        break;
                }

            }
            if (i > 9) {
                print("YOU CAN'T MOVE, SO ");
                computer = true;
                break;
            }
        }
        if (computer) {
            print("I WIN.\n");
            w++;
        } else {
            print("YOU WIN\n");
            ma[x][y] = 0;
            l++;
        }
        print("I HAVE WON " + w + " AND YOU " + l + " OUT OF " + (l + w) + " GAMES.\n");
        print("\n");
    }
}

main();
