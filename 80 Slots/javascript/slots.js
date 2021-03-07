// SLOTS
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

var figures = [, "BAR", "BELL", "ORANGE", "LEMON", "PLUM", "CHERRY"];

// Main program
async function main()
{
    print(tab(30) + "SLOTS\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    // Produced by Fred Mirabelle and Bob Harper on Jan 29, 1973
    // It simulates the slot machine.
    print("YOU ARE IN THE H&M CASINO,IN FRONT ON ONE OF OUR\n");
    print("ONE-ARM BANDITS. BET FROM $1 TO $100.\n");
    print("TO PULL THE ARM, PUNCH THE RETURN KEY AFTER MAKING YOUR BET.\n");
    p = 0;
    while (1) {
        while (1) {
            print("\n");
            print("YOUR BET");
            m = parseInt(await input());
            if (m > 100) {
                print("HOUSE LIMITS ARE $100\n");
            } else if (m < 1) {
                print("MINIMUM BET IS $1\n");
            } else {
                break;
            }
        }
        // Not implemented: GOSUB 1270 ten chimes
        print("\n");
        x = Math.floor(6 * Math.random() + 1);
        y = Math.floor(6 * Math.random() + 1);
        z = Math.floor(6 * Math.random() + 1);
        print("\n");
        // Not implemented: GOSUB 1310 seven chimes after figure x and y
        print(figures[x] + " " + figures[y] + " " + figures[z] + "\n");
        lost = false;
        if (x == y && y == z) {  // Three figure
            print("\n");
            if (z != 1) {
                print("**TOP DOLLAR**\n");
                p += ((10 * m) + m);
            } else {
                print("***JACKPOT***\n");
                p += ((100 * m) + m);
            }
            print("YOU WON!\n");
        } else if (x == y || y == z || x == z) {
            if (x == y)
                c = x;
            else
                c = z;
            if (c == 1) {
                print("\n");
                print("*DOUBLE BAR*\n");
                print("YOU WON\n");
                p += ((5 * m) + m);
            } else if (x != z) {
                print("\n");
                print("DOUBLE!!\n");
                print("YOU WON!\n");
                p += ((2 * m) + m);
            } else {
                lost = true;
            }
        } else {
            lost = true;
        }
        if (lost) {
            print("\n");
            print("YOU LOST.\n");
            p -= m;
        }
        print("YOUR STANDINGS ARE $" + p + "\n");
        print("AGAIN");
        str = await input();
        if (str.substr(0, 1) != "Y")
            break;
    }
    print("\n");
    if (p < 0) {
        print("PAY UP!  PLEASE LEAVE YOUR MONEY ON THE TERMINAL.\n");
    } else if (p == 0) {
        print("HEY, YOU BROKE EVEN.\n");
    } else {
        print("COLLECT YOUR WINNINGS FROM THE H&M CASHIER.\n");
    }
}

main();
