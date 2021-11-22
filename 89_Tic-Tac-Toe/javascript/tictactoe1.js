// TIC TAC TOE 1
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

function mf(x)
{
    return x - 8 * Math.floor((x - 1) / 8);
}

function computer_moves()
{
    print("COMPUTER MOVES " + m + "\n");
}

var m;

// Main control section
async function main()
{
    print(tab(30) + "TIC TAC TOE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    //
    // This program plays Tic Tac Toe
    // The machine goes first
    print("THE GAME BOARD IS NUMBERED:\n");
    print("\n");
    print("1  2  3\n");
    print("8  9  4\n");
    print("7  6  5\n");
    print("\n");
    //
    // Main program
    while (1) {
        print("\n");
        print("\n");
        a = 9;
        m = a;
        
        computer_moves();
        print("YOUR MOVE");
        m = parseInt(await input());
        
        p = m;
        b = mf(p + 1);
        m = b;
        
        computer_moves();
        print("YOUR MOVE");
        m = parseInt(await input());
        
        q = m;
        if (q != mf(b + 4)) {
            c = mf(b + 4);
            m = c;
            computer_moves();
            print("AND WINS ********\n");
            continue;
        }
        
        c = mf(b + 2);
        m = c;

        computer_moves();
        print("YOUR MOVE");
        m = parseInt(await input());

        r = m;
        if (r != mf(c + 4)) {
            d = mf(c + 4);
            m = d;
            computer_moves();
            print("AND WINS ********\n");
            continue;
        }
        
        if (p % 2 == 0) {
            d = mf(c + 7);
            m = d;
            computer_moves();
            print("AND WINS ********\n");
            continue;
        }
        
        d = mf(c + 3);
        m = d;
        
        computer_moves();
        print("YOUR MOVE");
        m = parseInt(await input());
        
        s = m;
        if (s != mf(d + 4)) {
            e = mf(d + 4);
            m = e;
            computer_moves();
        }
        e = mf(d + 6);
        m = e;
        computer_moves();
        print("THE GAME IS A DRAW.\n");
    }
}

main();
