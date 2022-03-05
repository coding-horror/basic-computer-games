// NIM
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

var a = [];
var b = [];
var d = [];

// Main program
async function main()
{
    print(tab(33) + "NIM\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    for (i = 1; i <= 100; i++) {
        a[i] = 0;
        b[i] = [];
        for (j = 0; j <= 10; j++)
            b[i][j] = 0;
    }
    d[0] = 0;
    d[1] = 0;
    d[2] = 0;
    print("DO YOU WANT INSTRUCTIONS");
    while (1) {
        str = await input();
        str = str.toUpperCase();
        if (str == "YES" || str == "NO")
            break;
        print("PLEASE ANSWER YES OR NO\n");
    }
    if (str == "YES") {
        print("THE GAME IS PLAYED WITH A NUMBER OF PILES OF OBJECTS.\n");
        print("ANY NUMBER OF OBJECTS ARE REMOVED FROM ONE PILE BY YOU AND\n");
        print("THE MACHINE ALTERNATELY.  ON YOUR TURN, YOU MAY TAKE\n");
        print("ALL THE OBJECTS THAT REMAIN IN ANY PILE, BUT YOU MUST\n");
        print("TAKE AT LEAST ONE OBJECT, AND YOU MAY TAKE OBJECTS FROM\n");
        print("ONLY ONE PILE ON A SINGLE TURN.  YOU MUST SPECIFY WHETHER\n");
        print("WINNING IS DEFINED AS TAKING OR NOT TAKING THE LAST OBJECT,\n");
        print("THE NUMBER OF PILES IN THE GAME, AND HOW MANY OBJECTS ARE\n");
        print("ORIGINALLY IN EACH PILE.  EACH PILE MAY CONTAIN A\n");
        print("DIFFERENT NUMBER OF OBJECTS.\n");
        print("THE MACHINE WILL SHOW ITS MOVE BY LISTING EACH PILE AND THE\n");
        print("NUMBER OF OBJECTS REMAINING IN THE PILES AFTER  EACH OF ITS\n");
        print("MOVES.\n");
    }
    while (1) {
        print("\n");
        while (1) {
            print("ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST");
            w = parseInt(await input());
            if (w == 1 || w == 2)
                break;
        }
        while (1) {
            print("ENTER NUMBER OF PILES");
            n = parseInt(await input());
            if (n >= 1 && n <= 100)
                break;
        }
        print("ENTER PILE SIZES\n");
        for (i = 1; i <= n; i++) {
            while (1) {
                print(i + " ");
                a[i] = parseInt(await input());
                if (a[i] >= 1 && a[i] <= 2000)
                    break;
            }
        }
        print("DO YOU WANT TO MOVE FIRST");
        while (1) {
            str = await input();
            str = str.toUpperCase();
            if (str == "YES" || str == "NO")
                break;
            print("PLEASE ANSWER YES OR NO.\n");
        }
        if (str == "YES")
            player_first = true;
        else
            player_first = false;
        while (1) {
            if (!player_first) {
                if (w != 1) {
                    c = 0;
                    for (i = 1; i <= n; i++) {
                        if (a[i] == 0)
                            continue;
                        c++;
                        if (c == 3)
                            break;
                        d[c] = i;
                    }
                    if (i > n) {
                        if (c == 2) {
                            if (a[d[1]] == 1 || a[d[2]] == 1) {
                                print("MACHINE WINS\n");
                                break;
                            }
                        } else {
                            if (a[d[1]] > 1)
                                print("MACHINE WINS\n");
                            else
                                print("MACHINE LOSES\n");
                            break;
                        }

                    } else {
                        c = 0;
                        for (i = 1; i <= n; i++) {
                            if (a[i] > 1)
                                break;
                            if (a[i] == 0)
                                continue;
                            c++;
                        }
                        if (i > n && c % 2) {
                            print("MACHINE LOSES\n");
                            break;
                        }
                    }
                }
                for (i = 1; i <= n; i++) {
                    e = a[i];
                    for (j = 0; j <= 10; j++) {
                        f = e / 2;
                        b[i][j] = 2 * (f - Math.floor(f));
                        e = Math.floor(f);
                    }
                }
                for (j = 10; j >= 0; j--) {
                    c = 0;
                    h = 0;
                    for (i = 1; i <= n; i++) {
                        if (b[i][j] == 0)
                            continue;
                        c++;
                        if (a[i] <= h)
                            continue;
                        h = a[i];
                        g = i;
                    }
                    if (c % 2)
                        break;
                }
                if (j < 0) {
                    do {
                        e = Math.floor(n * Math.random() + 1);
                    } while (a[e] == 0) ;
                    f = Math.floor(a[e] * Math.random() + 1);
                    a[e] -= f;
                } else {
                    a[g] = 0;
                    for (j = 0; j <= 10; j++) {
                        b[g][j] = 0;
                        c = 0;
                        for (i = 1; i <= n; i++) {
                            if (b[i][j] == 0)
                                continue;
                            c++;
                        }
                        a[g] = a[g] + (c % 2) * Math.pow(2, j);
                    }
                    if (w != 1) {
                        c = 0;
                        for (i = 1; i <= n; i++) {
                            if (a[i] > 1)
                                break;
                            if (a[i] == 0)
                                continue;
                            c++;
                        }
                        if (i > n && c % 2 == 0)
                            a[g] = 1 - a[g];
                    }
                }
                print("PILE  SIZE\n");
                for (i = 1; i <= n; i++)
                    print(" " + i + "  " + a[i] + "\n");
                if (w != 2) {
                    if (game_completed()) {
                        print("MACHINE WINS");
                        break;
                    }
                }
            } else {
                player_first = false;
            }
            while (1) {
                print("YOUR MOVE - PILE , NUMBER TO BE REMOVED");
                str = await input();
                x = parseInt(str);
                y = parseInt(str.substr(str.indexOf(",") + 1));
                if (x < 1 || x > n)
                    continue;
                if (y < 1 || y > a[x])
                    continue;
                break;
            }
            a[x] -= y;
            if (game_completed()) {
                print("MACHINE LOSES");
                break;
            }
        }
        print("DO YOU WANT TO PLAY ANOTHER GAME");
        while (1) {
            str = await input();
            str = str.toUpperCase();
            if (str == "YES" || str == "NO")
                break;
            print("PLEASE ANSWER YES OR NO.\n");
        }
        if (str == "NO")
            break;
    }
}

function game_completed()
{
    for (var i = 1; i <= n; i++) {
        if (a[i] != 0)
            return false;
    }
    return true;
}

main();
