// TIC TAC TOE 2
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

var s = [];

function who_win(piece)
{
    if (piece == -1) {
        print("I WIN, TURKEY!!!\n");
    } else if (piece == 1) {
        print("YOU BEAT ME!! GOOD GAME.\n");
    }
}

function show_board()
{
    print("\n");
    for (i = 1; i <= 9; i++) {
        print(" ");
        if (s[i] == -1) {
            print(qs + " ");
        } else if (s[i] == 0) {
            print("  ");
        } else {
            print(ps + " ");
        }
        if (i == 3 || i == 6) {
            print("\n");
            print("---+---+---\n");
        } else if (i != 9) {
            print("!");
        }
    }
    print("\n");
    print("\n");
    print("\n");
    for (i = 1; i <= 7; i += 3) {
        if (s[i] && s[i] == s[i + 1] && s[i] == s[i + 2]) {
            who_win(s[i]);
            return true;
        }
    }
    for (i = 1; i <= 3; i++) {
        if (s[i] && s[i] == s[i + 3] && s[i] == s[i + 6]) {
            who_win(s[i]);
            return true;
        }
    }
    if (s[1] && s[1] == s[5] && s[1] == s[9]) {
        who_win(s[1]);
        return true;
    }
    if (s[3] && s[3] == s[5] && s[3] == s[7]) {
        who_win(s[3]);
        return true;
    }
    for (i = 1; i <= 9; i++) {
        if (s[i] == 0)
            break;
    }
    if (i > 9) {
        print("IT'S A DRAW. THANK YOU.\n");
        return true;
    }
    return false;
}

// Main control section
async function main()
{
    print(tab(30) + "TIC-TAC-TOE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    for (i = 1; i <= 9; i++)
        s[i] = 0;
    print("THE BOARD IS NUMBERED:\n");
    print(" 1  2  3\n");
    print(" 4  5  6\n");
    print(" 7  8  9\n");
    print("\n");
    print("\n");
    print("\n");
    print("DO YOU WANT 'X' OR 'O'");
    str = await input();
    if (str == "X") {
        ps = "X";
        qs = "O";
        first_time = true;
    } else {
        ps = "O";
        qs = "X";
        first_time = false;
    }
    while (1) {
        if (!first_time) {
            g = -1;
            h = 1;
            if (s[5] == 0) {
                s[5] = -1;
            } else if (s[5] == 1 && s[1] == 0) {
                s[1] = -1;
            } else if (s[5] != 1 && s[2] == 1 && s[1] == 0 || s[5] != 1 && s[4] == 1 && s[1] == 0) {
                s[1] = -1;
            } else if (s[5] != 1 && s[6] == 1 && s[9] == 0 || s[5] != 1 && s[8] == 1 && s[9] == 0) {
                s[9] = -1;
            } else {
                while (1) {
                    played = false;
                    if (g == 1) {
                        j = 3 * Math.floor((m - 1) / 3) + 1;
                        if (3 * Math.floor((m - 1) / 3) + 1 == m)
                            k = 1;
                        if (3 * Math.floor((m - 1) / 3) + 2 == m)
                            k = 2;
                        if (3 * Math.floor((m - 1) / 3) + 3 == m)
                            k = 3;
                    } else {
                        j = 1;
                        k = 1;
                    }
                    while (1) {
                        if (s[j] == g) {
                            if (s[j + 2] == g) {
                                if (s[j + 1] == 0) {
                                    s[j + 1] = -1;
                                    played = true;
                                    break;
                                }
                            } else {
                                if (s[j + 2] == 0 && s[j + 1] == g) {
                                    s[j + 2] = -1;
                                    played = true;
                                    break;
                                }
                            }
                        } else {
                            if (s[j] != h && s[j + 2] == g && s[j + 1] == g) {
                                s[j] = -1;
                                played = true;
                                break;
                            }
                        }
                        if (s[k] == g) {
                            if (s[k + 6] == g) {
                                if (s[k + 3] == 0) {
                                    s[k + 3] = -1;
                                    played = true;
                                    break;
                                }
                            } else {
                                if (s[k + 6] == 0 && s[k + 3] == g) {
                                    s[k + 6] = -1;
                                    played = true;
                                    break;
                                }
                            }
                        } else {
                            if (s[k] != h && s[k + 6] == g && s[k + 3] == g) {
                                s[k] = -1;
                                played = true;
                                break;
                            }
                        }
                        if (g == 1)
                            break;
                        if (j == 7 && k == 3)
                            break;
                        k++;
                        if (k > 3) {
                            k = 1;
                            j += 3;
                            if (j > 7)
                                break;
                        }
                    }
                    if (!played) {
                        if (s[5] == g) {
                            if (s[3] == g && s[7] == 0) {
                                s[7] = -1;
                                played = true;
                            } else if (s[9] == g && s[1] == 0) {
                                s[1] = -1;
                                played = true;
                            } else if (s[7] == g && s[3] == 0) {
                                s[3] = -1;
                                played = true;
                            } else if (s[9] == 0 && s[1] == g) {
                                s[9] = -1;
                                played = true;
                            }
                        }
                        if (!played) {
                            if (g == -1) {
                                g = 1;
                                h = -1;
                            }
                        }
                    }
                    if (played)
                        break;
                }
                if (!played) {
                    if (s[9] == 1 && s[3] == 0 && s[1] != 1) {
                        s[3] = -1;
                    } else {
                        for (i = 2; i <= 9; i++) {
                            if (s[i] == 0) {
                                s[i] = -1;
                                break;
                            }
                        }
                        if (i > 9) {
                            s[1] = -1;
                        }
                    }
                }
            }
            print("\n");
            print("THE COMPUTER MOVES TO...");
            if (show_board())
                break;
        }
        first_time = false;
        while (1) {
            print("\n");
            print("WHERE DO YOU MOVE");
            m = parseInt(await input());
            if (m == 0) {
                print("THANKS FOR THE GAME.\n");
                break;
            }
            if (m >= 1 && m <= 9 && s[m] == 0)
                break;
            print("THAT SQUARE IS OCCUPIED.\n");
            print("\n");
            print("\n");
        }
        g = 1;
        s[m] = 1;
        if (show_board())
            break;
    }
}

main();
