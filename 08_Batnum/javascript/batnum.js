// BATNUM
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

// Main program
async function main()
{
    print(tab(33) + "BATNUM\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS PROGRAM IS A 'BATTLE OF NUMBERS' GAME, WHERE THE\n");
    print("COMPUTER IS YOUR OPPONENT.\n");
    print("\n");
    print("THE GAME STARTS WITH AN ASSUMED PILE OF OBJECTS. YOU\n");
    print("AND YOUR OPPONENT ALTERNATELY REMOVE OBJECTS FROM THE PILE.\n");
    print("WINNING IS DEFINED IN ADVANCE AS TAKING THE LAST OBJECT OR\n");
    print("NOT. YOU CAN ALSO SPECIFY SOME OTHER BEGINNING CONDITIONS.\n");
    print("DON'T USE ZERO, HOWEVER, IN PLAYING THE GAME.\n");
    print("ENTER A NEGATIVE NUMBER FOR NEW PILE SIZE TO STOP PLAYING.\n");
    print("\n");
    first_time = 1;
    while (1) {
        while (1) {
            if (first_time == 1) {
                first_time = 0;
            } else {
                for (i = 1; i <= 10; i++)
                    print("\n");
            }
            print("ENTER PILE SIZE");
            n = parseInt(await input());
            if (n >= 1)
                break;
        }
        while (1) {
            print("ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST: ");
            m = parseInt(await input());
            if (m == 1 || m == 2)
                break;
        }
        while (1) {
            print("ENTER MIN AND MAX ");
            str = await input();
            a = parseInt(str);
            b = parseInt(str.substr(str.indexOf(",") + 1));
            if (a <= b && a >= 1)
                break;
        }
        while (1) {
            print("ENTER START OPTION - 1 COMPUTER FIRST, 2 YOU FIRST ");
            s = parseInt(await input());
            print("\n");
            print("\n");
            if (s == 1 || s == 2)
                break;
        }
        w = 0;
        c = a + b;
        while (1) {
            if (s == 1) {
                // Computer's turn
                q = n;
                if (m != 1)
                    q--;
                if (m != 1 && n <= a) {
                    w = 1;
                    print("COMPUTER TAKES " + n + " AND LOSES.\n");
                } else if (m == 1 && n <= b) {
                    w = 1;
                    print("COMPUTER TAKES " + n + " AND WINS.\n");
                } else {
                    p = q - c * Math.floor(q / c);
                    if (p < a)
                        p = a;
                    if (p > b)
                        p = b;
                    n -= p;
                    print("COMPUTER TAKES " + p + " AND LEAVES " + n + "\n");
                    w = 0;
                }
                s = 2;
            }
            if (w)
                break;
            if (s == 2) {
                while (1) {
                    print("\n");
                    print("YOUR MOVE ");
                    p = parseInt(await input());
                    if (p == 0) {
                        print("I TOLD YOU NOT TO USE ZERO! COMPUTER WINS BY FORFEIT.\n");
                        w = 1;
                        break;
                    } else if (p >= a && p <= b && n - p >= 0) {
                        break;
                    }
                }
                if (p != 0) {
                    n -= p;
                    if (n == 0) {
                        if (m != 1) {
                            print("TOUGH LUCK, YOU LOSE.\n");
                        } else {
                            print("CONGRATULATIONS, YOU WIN.\n");
                        }
                        w = 1;
                    } else {
                        w = 0;
                    }
                }
                s = 1;
            }
            if (w)
                break;
        }
    }
}

main();
