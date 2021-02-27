// TRAP
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

// Main control section
async function main()
{
    print(tab(34) + "TRAP\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    g = 6;
    n = 100;
    // Trap
    // Steve Ullman, Aug/01/1972
    print("INSTRUCTIONS");
    str = await input();
    if (str.substr(0, 1) == "Y") {
        print("I AM THINKING OF A NUMBER BETWEEN 1 AND " + n + "\n");
        print("TRY TO GUESS MY NUMBER. ON EACH GUESS,\n");
        print("YOU ARE TO ENTER 2 NUMBERS, TRYING TO TRAP\n");
        print("MY NUMBER BETWEEN THE TWO NUMBERS. I WILL\n");
        print("TELL YOU IF YOU HAVE TRAPPED MY NUMBER, IF MY\n");
        print("NUMBER IS LARGER THAN YOUR TWO NUMBERS, OR IF\n");
        print("MY NUMBER IS SMALLER THAN YOUR TWO NUMBERS.\n");
        print("IF YOU WANT TO GUESS ONE SINGLE NUMBER, TYPE\n");
        print("YOUR GUESS FOR BOTH YOUR TRAP NUMBERS.\n");
        print("YOU GET " + g + " GUESSES TO GET MY NUMBER.\n");
    }
    while (1) {
        x = Math.floor(n * Math.random()) + 1;
        for (q = 1; q <= g; q++) {
            print("\n");
            print("GUESS #" + q + " ");
            str = await input();
            a = parseInt(str);
            b = parseInt(str.substr(str.indexOf(",") + 1));
            if (a == b && x == a) {
                print("YOU GOT IT!!!\n");
                break;
            }
            if (a > b) {
                r = a;
                a = b;
                b = r;
            }
            if (a <= x && x <= b) {
                print("YOU HAVE TRAPPED MY NUMBER.\n");
            } else if (x >= a) {
                print("MY NUMBER IS LARGER THAN YOUR TRAP NUMBERS.\n");
            } else {
                print("MY NUMBER IS SMALLER THAN YOUR TRAP NUMBERS.\n");
            }
        }
        print("\n");
        print("TRY AGAIN.\n");
        print("\n");
    }
}

main();
