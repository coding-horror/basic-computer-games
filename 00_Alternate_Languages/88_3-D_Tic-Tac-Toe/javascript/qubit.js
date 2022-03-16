// QUBIT
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

var xa = [];
var la = [];
var ma = [[],
          [,1,2,3,4],    // 1
          [,5,6,7,8],    // 2
          [,9,10,11,12], // 3
          [,13,14,15,16],    // 4
          [,17,18,19,20],    // 5
          [,21,22,23,24],    // 6
          [,25,26,27,28],    // 7
          [,29,30,31,32],    // 8
          [,33,34,35,36],    // 9
          [,37,38,39,40],    // 10
          [,41,42,43,44],    // 11
          [,45,46,47,48],    // 12
          [,49,50,51,52],    // 13
          [,53,54,55,56],    // 14
          [,57,58,59,60],    // 15
          [,61,62,63,64],    // 16
          [,1,17,33,49], // 17
          [,5,21,37,53],    // 18
          [,9,25,41,57],   // 19
          [,13,29,45,61], // 20
          [,2,18,34,50], // 21
          [,6,22,38,54],    // 22
          [,10,26,42,58],  // 23
          [,14,30,46,62],   // 24
          [,3,19,35,51], // 25
          [,7,23,39,55],    // 26
          [,11,27,43,59],  // 27
          [,15,31,47,63], // 28
          [,4,20,36,52], // 29
          [,8,24,40,56], // 30
          [,12,28,44,60],    // 31
          [,16,32,48,64],    // 32
          [,1,5,9,13],   // 33
          [,17,21,25,29],    // 34
          [,33,37,41,45],    // 35
          [,49,53,57,61],    // 36
          [,2,6,10,14],  // 37
          [,18,22,26,30],    // 38
          [,34,38,42,46],    // 39
          [,50,54,58,62],    // 40
          [,3,7,11,15],  // 41
          [,19,23,27,31],    // 42
          [,35,39,43,47],    // 43
          [,51,55,59,63],    // 44
          [,4,8,12,16],  // 45
          [,20,24,28,32],    // 46
          [,36,40,44,48],    // 47
          [,52,56,60,64],    // 48
          [,1,6,11,16],  // 49
          [,17,22,27,32],    // 50
          [,33,38,43,48],    // 51
          [,49,54,59,64],    // 52
          [,13,10,7,4],  // 53
          [,29,26,23,20],    // 54
          [,45,42,39,36],    // 55
          [,61,58,55,52],    // 56
          [,1,21,41,61], // 57
          [,2,22,42,62], // 58
          [,3,23,43,63], // 59
          [,4,24,44,64], // 60
          [,49,37,25,13],    // 61
          [,50,38,26,14],    // 62
          [,51,39,27,15],    // 63
          [,52,40,28,16],    // 64
          [,1,18,35,52], // 65
          [,5,22,39,56], // 66
          [,9,26,43,60], // 67
          [,13,30,47,64],    // 68
          [,49,34,19,4], // 69
          [,53,38,23,8], // 70
          [,57,42,27,12],    // 71
          [,61,46,31,16],    // 72
          [,1,22,43,64], // 73
          [,16,27,38,49],    // 74
          [,4,23,42,61], // 75
          [,13,26,39,52] // 76
          ];
var ya = [,1,49,52,4,13,61,64,16,22,39,23,38,26,42,27,43];

function show_board()
{
    for (xx = 1; xx <= 9; xx++)
        print("\n");
    for (i = 1; i <= 4; i++) {
        for (j = 1; j <= 4; j++) {
            str = "";
            for (i1 = 1; i1 <= j; i1++)
                str += "   ";
            for (k = 1; k <= 4; k++) {
                q = 16 * i + 4 * j + k - 20;
                if (xa[q] == 0)
                    str += "( )      ";
                if (xa[q] == 5)
                    str += "(M)      ";
                if (xa[q] == 1)
                    str += "(Y)      ";
                if (xa[q] == 1 / 8)
                    str += "( )      ";
            }
            print(str + "\n");
            print("\n");
        }
        print("\n");
        print("\n");
    }
}

function process_board()
{
    for (i = 1; i <= 64; i++) {
        if (xa[i] == 1 / 8)
            xa[i] = 0;
    }
}

function check_for_lines()
{
    for (s = 1; s <= 76; s++) {
        j1 = ma[s][1];
        j2 = ma[s][2];
        j3 = ma[s][3];
        j4 = ma[s][4];
        la[s] = xa[j1] + xa[j2] + xa[j3] + xa[j4];
    }
}

function show_square(m)
{
    k1 = Math.floor((m - 1) / 16) + 1;
    j2 = m - 16 * (k1 - 1);
    k2 = Math.floor((j2 - 1) / 4) + 1;
    k3 = m - (k1 - 1) * 16 - (k2 - 1) * 4;
    m = k1 * 100 + k2 * 10 + k3;
    print(" " + m + " ");
}

function select_move() {
    if (i % 4 <= 1) {
        a = 1;
    } else {
        a = 2;
    }
    for (j = a; j <= 5 - a; j += 5 - 2 * a) {
        if (xa[ma[i][j]] == s)
            break;
    }
    if (j > 5 - a)
        return false;
    xa[ma[i][j]] = s;
    m = ma[i][j];
    print("MACHINE TAKES");
    show_square(m);
    return true;
}

// Main control section
async function main()
{
    print(tab(33) + "QUBIC\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    while (1) {
        print("DO YOU WANT INSTRUCTIONS");
        str = await input();
        str = str.substr(0, 1);
        if (str == "Y" || str == "N")
            break;
        print("INCORRECT ANSWER.  PLEASE TYPE 'YES' OR 'NO'");
    }
    if (str == "Y") {
        print("\n");
        print("THE GAME IS TIC-TAC-TOE IN A 4 X 4 X 4 CUBE.\n");
        print("EACH MOVE IS INDICATED BY A 3 DIGIT NUMBER, WITH EACH\n");
        print("DIGIT BETWEEN 1 AND 4 INCLUSIVE.  THE DIGITS INDICATE THE\n");
        print("LEVEL, ROW, AND COLUMN, RESPECTIVELY, OF THE OCCUPIED\n");
        print("PLACE.  \n");
        print("\n");
        print("TO PRINT THE PLAYING BOARD, TYPE 0 (ZERO) AS YOUR MOVE.\n");
        print("THE PROGRAM WILL PRINT THE BOARD WITH YOUR MOVES INDI-\n");
        print("CATED WITH A (Y), THE MACHINE'S MOVES WITH AN (M), AND\n");
        print("UNUSED SQUARES WITH A ( ).  OUTPUT IS ON PAPER.\n");
        print("\n");
        print("TO STOP THE PROGRAM RUN, TYPE 1 AS YOUR MOVE.\n");
        print("\n");
        print("\n");
    }
    while (1) {
        for (i = 1; i <= 64; i++)
            xa[i] = 0;
        z = 1;
        print("DO YOU WANT TO MOVE FIRST");
        while (1) {
            str = await input();
            str = str.substr(0, 1);
            if (str == "Y" || str == "N")
                break;
            print("INCORRECT ANSWER.  PLEASE TYPE 'YES' OR 'NO'");
        }
        while (1) {
            while (1) {
                print(" \n");
                print("YOUR MOVE");
                j1 = parseInt(await input());
                if (j1 == 0) {
                    show_board();
                    continue;
                }
                if (j1 == 1)
                    return;
                k1 = Math.floor(j1 / 100);
                j2 = j1 - k1 * 100;
                k2 = Math.floor(j2 / 10);
                k3 = j2 - k2 * 10;
                m = 16 * k1 + 4 * k2 + k3 - 20;
                if (k1 < 1 || k2 < 1 || k3 < 1 || k1 > 4 || k2 > 4 || k3 >> 4) {
                    print("INCORRECT MOVE, RETYPE IT--");
                } else {
                    process_board();
                    if (xa[m] != 0) {
                        print("THAT SQUARE IS USED, TRY AGAIN.\n");
                    } else {
                        break;
                    }
                }
            }
            xa[m] = 1;
            check_for_lines();
            status = 0;
            for (j = 1; j <= 3; j++) {
                for (i = 1; i <= 76; i++) {
                    if (j == 1) {
                        if (la[i] != 4)
                            continue;
                        print("YOU WIN AS FOLLOWS");
                        for (j = 1; j <= 4; j++) {
                            m = ma[i][j];
                            show_square(m);
                        }
                        status = 1;
                        break;
                    }
                    if (j == 2) {
                        if (la[i] != 15)
                            continue;
                        for (j = 1; j <= 4; j++) {
                            m = ma[i][j];
                            if (xa[m] != 0)
                                continue;
                            xa[m] = 5;
                            print("MACHINE MOVES TO ");
                            show_square(m);
                        }
                        print(", AND WINS AS FOLLOWS");
                        for (j = 1; j <= 4; j++) {
                            m = ma[i][j];
                            show_square(m);
                        }
                        status = 1;
                        break;
                    }
                    if (j == 3) {
                        if (la[i] != 3)
                            continue;
                        print("NICE TRY, MACHINE MOVES TO");
                        for (j = 1; j <= 4; j++) {
                            m = ma[i][j];
                            if (xa[m] != 0)
                                continue;
                            xa[m] = 5;
                            show_square(m);
                            status = 2;
                        }
                        break;
                    }
                }
                if (i <= 76)
                    break;
            }
            if (status == 2)
                continue;
            if (status == 1)
                break;
            // x = x; non-useful in original
            i = 1;
            do {
                la[i] = xa[ma[i][1]] + xa[ma[i][2]] + xa[ma[i][3]] + xa[ma[i][4]];
                l = la[i];
                if (l == 10) {
                    for (j = 1; j <= 4; j++) {
                        if (xa[ma[i][j]] == 0)
                            xa[ma[i][j]] = 1 / 8;
                    }
                }
            } while (++i <= 76) ;
            check_for_lines();
            i = 1;
            do {
                if (la[i] == 0.5) {
                    s = 1 / 8;
                    select_move();
                    break;
                }
                if (la[i] == 5 + 3 / 8) {
                    s = 1 / 8;
                    select_move();
                    break;
                }
            } while (++i <= 76) ;
            if (i <= 76)
                continue;

            process_board();

            i = 1;
            do {
                la[i] = xa[ma[i][1]] + xa[ma[i][2]] + xa[ma[i][3]] + xa[ma[i][4]];
                l = la[i];
                if (l == 2) {
                    for (j = 1; j <= 4; j++) {
                        if (xa[ma[i][j]] == 0)
                            xa[ma[i][j]] = 1 / 8;
                    }
                }
            } while (++i <= 76) ;
            check_for_lines();
            i = 1;
            do {
                if (la[i] == 0.5) {
                    s = 1 / 8;
                    select_move();
                    break;
                }
                if (la[i] == 1 + 3 / 8) {
                    s = 1 / 8;
                    select_move();
                    break;
                }
            } while (++i <= 76) ;
            if (i <= 76)
                continue;

            for (k = 1; k <= 18; k++) {
                p = 0;
                for (i = 4 * k - 3; i <= 4 * k; i++) {
                    for (j = 1; j <= 4; j++)
                        p += xa[ma[i][j]];
                }
                if (p == 4 || p == 9) {
                    s = 1 / 8;
                    for (i = 4 * k - 3; i <= 4 * k; i++) {
                        if (select_move())
                            break;
                    }
                    s = 0;
                }
            }
            if (k <= 18)
                continue
            process_board();
            z = 1;
            do {
                if (xa[ya[z]] == 0)
                    break;
            } while (++z < 17) ;
            if (z >= 17) {
                for (i = 1; i <= 64; i++) {
                    if (xa[i] == 0) {
                        xa[i] = 5;
                        m = i;
                        print("MACHINE LIKES");
                        break;
                    }
                }
                if (i > 64) {
                    print("THE GAME IS A DRAW.\n");
                    break;
                }
            } else {
                m = ya[z];
                xa[m] = 5;
                print("MACHINE MOVES TO");
            }
            show_square(m);
        }
        print(" \n");
        print("DO YOU WANT TO TRY ANOTHER GAME");
        while (1) {
            str = await input();
            str = str.substr(0, 1);
            if (str == "Y" || str == "N")
                break;
            print("INCORRECT ANSWER. PLEASE TYPE 'YES' OR 'NO'");
        }
        if (str == "N")
            break;
    }
}

main();
