// CRAPS
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
    print(tab(33) + "CRAPS\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    r = 0;
    print("2,3,12 ARE LOSERS: 4,5,6,8,9,10 ARE POINTS: 7,11 ARE NATURAL WINNERS.\n");
    t = 1;
    print("PICK A NUMBER AND INPUT TO ROLL DICE");
    z = parseInt(await input());
    do {
        x = Math.random();
        t++;
    } while (t <= z) ;
    while (1) {
        print("INPUT THE AMOUNT OF YOUR WAGER.");
        f = parseInt(await input());
        print("I WILL NOW THROW THE DICE\n");
        do {
            e = Math.floor(7 * Math.random());
            s = Math.floor(7 * Math.random());
            x = e + s;
        } while (x == 0 || x == 1) ;
        if (x == 7 || x == 11) {
            print(x + " - NATURAL....A WINNER!!!!\n");
            print(x + " PAYS EVEN MONEY, YOU WIN " + f + " DOLLARS\n");
            r += f;
        } else if (x == 2) {
            print(x + " - SNAKE EYES....YOU LOSE.\n");
            print("YOU LOSE " + f + " DOLLARS.\n");
            r -= f;
        } else if (x == 3 || x == 12) { // Original duplicates comparison in line 70
            print(x + " - CRAPS....YOU LOSE.\n");
            print("YOU LOSE " + f + " DOLLARS.\n");
            r -= f;
        } else {
            print(x + " IS THE POINT. I WILL ROLL AGAIN\n");
            while (1) {
                do {
                    h = Math.floor(7 * Math.random());
                    q = Math.floor(7 * Math.random());
                    o = h + q;
                } while (o == 0 || o == 1) ;
                if (o == 7) {
                    print(o + " - CRAPS, YOU LOSE.\n");
                    print("YOU LOSE $" + f + "\n");
                    r -= f;
                    break;
                }
                if (o == x) {
                    print(x + " - A WINNER.........CONGRATS!!!!!!!!\n");
                    print(x + " AT 2 TO 1 ODDS PAYS YOU...LET ME SEE..." + 2 * f + " DOLLARS\n");
                    r += f * 2;
                    break;
                }
                print(o + " - NO POINT. I WILL ROLL AGAIN\n");
            }
        }
        print(" IF YOU WANT TO PLAY AGAIN PRINT 5 IF NOT PRINT 2");
        m = parseInt(await input());
        if (r < 0) {
            print("YOU ARE NOW UNDER $" + -r + "\n");
        } else if (r > 0) {
            print("YOU ARE NOW AHEAD $" + r + "\n");
        } else {
            print("YOU ARE NOW EVEN AT 0\n");
        }
        if (m != 5)
            break;
    }
    if (r < 0) {
        print("TOO BAD, YOU ARE IN THE HOLE. COME AGAIN.\n");
    } else if (r > 0) {
        print("CONGRATULATIONS---YOU CAME OUT A WINNER. COME AGAIN!\n");
    } else {
        print("CONGRATULATIONS---YOU CAME OUT EVEN, NOT BAD FOR AN AMATEUR\n");
    }
    
}

main();
