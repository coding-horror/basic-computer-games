// TOWER
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

var ta = [];

// Print subroutine
function show_towers()
{
    var z;
    
    for (var k = 1; k <= 7; k++) {
        z = 10;
        str = "";
        for (var j = 1; j <= 3; j++) {
            if (ta[k][j] != 0) {
                while (str.length < z - Math.floor(ta[k][j] / 2))
                    str += " ";
                for (v = 1; v <= ta[k][j]; v++)
                    str += "*";
            } else {
                while (str.length < z)
                    str += " ";
                str += "*";
            }
            z += 21;
        }
        print(str + "\n");
    }
}

// Main control section
async function main()
{
    print(tab(33) + "TOWERS\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    while (1) {
        print("\n");
        // Initialize
        e = 0;
        for (d = 1; d <= 7; d++) {
            ta[d] = [];
            for (n = 1; n <= 3; n++)
                ta[d][n] = 0;
        }
        print("TOWERS OF HANOI PUZZLE.\n");
        print("\n");
        print("YOU MUST TRANSFER THE DISKS FROM THE LEFT TO THE RIGHT\n");
        print("TOWER, ONE AT A TIME, NEVER PUTTING A LARGER DISK ON A\n");
        print("SMALLER DISK.\n");
        print("\n");
        while (1) {
            print("HOW MANY DISKS DO YOU WANT TO MOVE (7 IS MAX)");
            s = parseInt(await input());
            print("\n");
            m = 0;
            if (s >= 1 && s <= 7)
                break;
            e++;
            if (e < 2) {
                print("SORRY, BUT I CAN'T DO THAT JOB FOR YOU.\n");
                continue;
            }
            print("ALL RIGHT, WISE GUY, IF YOU CAN'T PLAY THE GAME RIGHT, I'LL\n");
            print("JUST TAKE MY PUZZLE AND GO HOME.  SO LONG.\n");
            return;
        }
        // Store disks from smallest to largest
        print("IN THIS PROGRAM, WE SHALL REFER TO DISKS BY NUMERICAL CODE.\n");
        print("3 WILL REPRESENT THE SMALLEST DISK, 5 THE NEXT SIZE,\n");
        print("7 THE NEXT, AND SO ON, UP TO 15.  IF YOU DO THE PUZZLE WITH\n");
        print("2 DISKS, THEIR CODE NAMES WOULD BE 13 AND 15.  WITH 3 DISKS\n");
        print("THE CODE NAMES WOULD BE 11, 13 AND 15, ETC.  THE NEEDLES\n");
        print("ARE NUMBERED FROM LEFT TO RIGHT, 1 TO 3.  WE WILL\n");
        print("START WITH THE DISKS ON NEEDLE 1, AND ATTEMPT TO MOVE THEM\n");
        print("TO NEEDLE 3.\n");
        print("\n");
        print("GOOD LUCK!\n");
        print("\n");
        y = 7;
        d = 15;
        for (x = s; x >= 1; x--) {
            ta[y][1] = d;
            d -= 2;
            y--;
        }
        show_towers();
        while (1) {
            print("WHICH DISK WOULD YOU LIKE TO MOVE");
            e = 0;
            while (1) {
                d = parseInt(await input());
                if (d % 2 == 0 || d < 3 || d > 15) {
                    print("ILLEGAL ENTRY... YOU MAY ONLY TYPE 3,5,7,9,11,13, OR 15.\n");
                    e++;
                    if (e <= 1)
                        continue;
                    print("STOP WASTING MY TIME.  GO BOTHER SOMEONE ELSE.\n");
                    return;
                } else {
                    break;
                }
            }
            // Check if requested disk is below another
            for (r = 1; r <= 7; r++) {
                for (c = 1; c <= 3; c++) {
                    if (ta[r][c] == d)
                        break;
                }
                if (c <= 3)
                    break;
            }
            for (q = r; q >= 1; q--) {
                if (ta[q][c] != 0 && ta[q][c] < d)
                    break;
            }
            if (q >= 1) {
                print("THAT DISK IS BELOW ANOTHER ONE.  MAKE ANOTHER CHOICE.\n");
                continue;
            }
            e = 0;
            while (1) {
                print("PLACE DISK ON WHICH NEEDLE");
                n = parseInt(await input());
                if (n >= 1 && n <= 3)
                    break;
                e++;
                if (e <= 1) {
                    print("I'LL ASSUME YOU HIT THE WRONG KEY THI TIME.  BUT WATCH IT,\n");
                    print("I ONLY ALLOW ONE MISTAKE.\n");
                    continue;
                } else {
                    print("I TRIED TO WARN YOU, BUT YOU WOULDN'T LISTEN.\n");
                    print("BYE BYE, BIG SHOT.\n");
                    return;
                }
            }
            // Check if requested disk is below another
            for (r = 1; r <= 7; r++) {
                if (ta[r][n] != 0)
                    break;
            }
            if (r <= 7) {
                // Check if disk to be placed on a larger one
                if (d >= ta[r][n]) {
                    print("YOU CAN'T PLACE A LARGER DISK ON TOP OF A SMALLER ONE,\n");
                    print("IT MIGHT CRUSH IT!\n");
                    print("NOW THEN, ");
                    continue;
                }
            }
            // Move relocated disk
            for (v = 1; v <= 7; v++) {
                for (w = 1; w <= 3; w++) {
                    if (ta[v][w] == d)
                        break;
                }
                if (w <= 3)
                    break;
            }
            // Locate empty space on needle n
            for (u = 1; u <= 7; u++) {
                if (ta[u][n] != 0)
                    break;
            }
            ta[--u][n] = ta[v][w];
            ta[v][w] = 0;
            // Print out current status
            show_towers();
            // Check if done
            m++;
            for (r = 1; r <= 7; r++) {
                for (c = 1; c <= 2; c++) {
                    if (ta[r][c] != 0)
                        break;
                }
                if (c <= 2)
                    break;
            }
            if (r > 7)
                break;
            if (m > 128) {
                print("SORRY, BUT I HAVE ORDERS TO STOP IF YOU MAKE MORE THAN\n");
                print("128 MOVES.\n");
                return;
            }
        }
        if (m == Math.pow(2, s) - 1) {
            print("\n");
            print("CONGRATULATIONS!!\n");
            print("\n");
        }
        print("YOU HAVE PERFORMED THE TASK IN " + m + " MOVES.\n");
        print("\n");
        print("TRY AGAIN (YES OR NO)");
        while (1) {
            str = await input();
            if (str == "YES" || str == "NO")
                break;
            print("\n");
            print("'YES' OR 'NO' PLEASE");
        }
        if (str == "NO")
            break;
    }
    print("\n");
    print("THANKS FOR THE GAME!\n");
    print("\n");
}

main();
