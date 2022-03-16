// QUEEN
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

var sa = [,81,  71,  61,  51,  41,  31,  21,  11,
           92,  82,  72,  62,  52,  42,  32,  22,
          103,  93,  83,  73,  63,  53,  43,  33,
          114, 104,  94,  84,  74,  64,  54,  44,
          125, 115, 105,  95,  85,  75,  65,  55,
          136, 126, 116, 106,  96,  86,  76,  66,
          147, 137, 127, 117, 107,  97,  87,  77,
          158, 148, 138, 128, 118, 108,  98,  88];

var m;
var m1;
var u;
var t;
var u1;
var t1;

function show_instructions()
{
    print("WE ARE GOING TO PLAY A GAME BASED ON ONE OF THE CHESS\n");
    print("MOVES.  OUR QUEEN WILL BE ABLE TO MOVE ONLY TO THE LEFT,\n");
    print("DOWN, OR DIAGONALLY DOWN AND TO THE LEFT.\n");
    print("\n");
    print("THE OBJECT OF THE GAME IS TO PLACE THE QUEEN IN THE LOWER\n");
    print("LEFT HAND SQUARE BY ALTERNATING MOVES BETWEEN YOU AND THE\n");
    print("COMPUTER.  THE FIRST ONE TO PLACE THE QUEEN THERE WINS.\n");
    print("\n");
    print("YOU GO FIRST AND PLACE THE QUEEN IN ANY ONE OF THE SQUARES\n");
    print("ON THE TOP ROW OR RIGHT HAND COLUMN.\n");
    print("THAT WILL BE YOUR FIRST MOVE.\n");
    print("WE ALTERNATE MOVES.\n");
    print("YOU MAY FORFEIT BY TYPING '0' AS YOUR MOVE.\n");
    print("BE SURE TO PRESS THE RETURN KEY AFTER EACH RESPONSE.\n");
    print("\n");
    print("\n");
}

function show_map()
{
    print("\n");
    for (var a = 0; a <= 7; a++) {
        for (var b = 1; b <= 8; b++) {
            i = 8 * a + b;
            print(" " + sa[i] + " ");
        }
        print("\n");
        print("\n");
        print("\n");
    }
    print("\n");
}

function test_move()
{
    m = 10 * t + u;
    if (m == 158 || m == 127 || m == 126 || m == 75 || m == 73)
        return true;
    return false;
}

function random_move()
{
    // Random move
    z = Math.random();
    if (z > 0.6) {
        u = u1 + 1;
        t = t1 + 1;
    } else if (z > 0.3) {
        u = u1 + 1;
        t = t1 + 2;
    } else {
        u = u1;
        t = t1 + 1;
    }
    m = 10 * t + u;
}

function computer_move()
{
    if (m1 == 41 || m1 == 44 || m1 == 73 || m1 == 75 || m1 == 126 || m1 == 127) {
        random_move();
        return;
    }
    for (k = 7; k >= 1; k--) {
        u = u1;
        t = t1 + k;
        if (test_move())
            return;
        u += k;
        if (test_move())
            return;
        t += k;
        if (test_move())
            return;
    }
    random_move();
}

// Main program
async function main()
{
    print(tab(33) + "QUEEN\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");

    while (1) {
        print("DO YOU WANT INSTRUCTIONS");
        str = await input();
        if (str == "YES" || str == "NO")
            break;
        print("PLEASE ANSWER 'YES' OR 'NO'.\n");
    }
    if (str == "YES")
        show_instructions();
    while (1) {
        show_map();
        while (1) {
            print("WHERE WOULD YOU LIKE TO START");
            m1 = parseInt(await input());
            if (m1 == 0) {
                print("\n");
                print("IT LOOKS LIKE I HAVE WON BY FORFEIT.\n");
                print("\n");
                break;
            }
            t1 = Math.floor(m1 / 10);
            u1 = m1 - 10 * t1;
            if (u1 == 1 || u1 == t1)
                break;
            print("PLEASE READ THE DIRECTIONS AGAIN.\n");
            print("YOU HAVE BEGUN ILLEGALLY.\n");
            print("\n");
        }
        while (m1) {
            if (m1 == 158) {
                print("\n");
                print("C O N G R A T U L A T I O N S . . .\n");
                print("\n");
                print("YOU HAVE WON--VERY WELL PLAYED.\n");
                print("IT LOOKS LIKE I HAVE MET MY MATCH.\n");
                print("THANKS FOR PLAYING--I CAN'T WIN ALL THE TIME.\n");
                print("\n");
                break;
            }
            computer_move();
            print("COMPUTER MOVES TO SQUARE " + m + "\n");
            if (m == 158) {
                print("\n");
                print("NICE TRY, BUT IT LOOKS LIKE I HAVE WON.\n");
                print("THANKS FOR PLAYING.\n");
                print("\n");
                break;
            }
            print("WHAT IS YOUR MOVE");
            while (1) {
                m1 = parseInt(await input());
                if (m1 == 0)
                    break;
                t1 = Math.floor(m1 / 10);
                u1 = m1 - 10 * t1;
                p = u1 - u;
                l = t1 - t;
                if (m1 <= m || p == 0 && l <= 0 || p != 0 && l != p && l != 2 * p) {
                    print("\n");
                    print("Y O U   C H E A T . . .  TRY AGAIN");
                    continue;
                }
                break;
            }
            if (m1 == 0) {
                print("\n");
                print("IT LOOKS LIKE I HAVE WON BY FORFEIT.\n");
                print("\n");
                break;
            }
        }
        while (1) {
            print("ANYONE ELSE CARE TO TRY");
            str = await input();
            print("\n");
            if (str == "YES" || str == "NO")
                break;
            print("PLEASE ANSWER 'YES' OR 'NO'.\n");
        }
        if (str != "YES")
            break;
    }
    print("\n");
    print("OK --- THANKS AGAIN.\n");
}

main();
