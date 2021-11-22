// DICE
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
    print(tab(34) + "DICE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    f = [];
    // Danny Freidus
    print("THIS PROGRAM SIMULATES THE ROLLING OF A\n");
    print("PAIR OF DICE.\n");
    print("YOU ENTER THE NUMBER OF TIMES YOU WANT THE COMPUTER TO\n");
    print("'ROLL' THE DICE.  WATCH OUT, VERY LARGE NUMBERS TAKE\n");
    print("A LONG TIME.  IN PARTICULAR, NUMBERS OVER 5000.\n");
    do {
        for (q = 1; q <= 12; q++)
            f[q] = 0;
        print("\n");
        print("HOW MANY ROLLS");
        x = parseInt(await input());
        for (s = 1; s <= x; s++) {
            a = Math.floor(Math.random() * 6 + 1);
            b = Math.floor(Math.random() * 6 + 1);
            r = a + b;
            f[r]++;
        }
        print("\n");
        print("TOTAL SPOTS\tNUMBER OF TIMES\n");
        for (v = 2; v <= 12; v++) {
            print("\t" + v + "\t" + f[v] + "\n");
        }
        print("\n");
        print("\n");
        print("TRY AGAIN");
        str = await input();
    } while (str.substr(0, 1) == "Y") ;
}

main();
