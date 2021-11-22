// REVERSE
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

var a = [];
var n;

// Subroutine to print the rules
function print_rules()
{
    print("\n");
    print("THIS IS THE GAME OF 'REVERSE'.  TO WIN, ALL YOU HAVE\n");
    print("TO DO IS ARRANGE A LIST OF NUMBERS (1 THROUGH " + n + ")\n");
    print("IN NUMERICAL ORDER FROM LEFT TO RIGHT.  TO MOVE, YOU\n");
    print("TELL ME HOW MANY NUMBERS (COUNTING FROM THE LEFT) TO\n");
    print("REVERSE.  FOR EXAMPLE, IF THE CURRENT LIST IS:\n");
    print("\n");
    print("2 3 4 5 1 6 7 8 9\n");
    print("\n");
    print("AND YOU REVERSE 4, THE RESULT WILL BE:\n");
    print("\n");
    print("5 4 3 2 1 6 7 8 9\n");
    print("\n");
    print("NOW IF YOU REVERSE 5, YOU WIN!\n");
    print("\n");
    print("1 2 3 4 5 6 7 8 9\n");
    print("\n");
    print("NO DOUBT YOU WILL LIKE THIS GAME, BUT\n");
    print("IF YOU WANT TO QUIT, REVERSE 0 (ZERO).\n");
    print("\n");
}

// Subroutine to print list
function print_list()
{
    print("\n");
    for (k = 1; k <= n; k++)
        print(" " + a[k] + " ");
    print("\n");
    print("\n");
}

// Main program
async function main()
{
    print(tab(32) + "REVERSE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("REVERSE -- A GAME OF SKILL\n");
    print("\n");
    for (i = 0; i <= 20; i++)
        a[i] = 0;
    // *** N=NUMBER OF NUMBER
    n = 9;
    print("DO YOU WANT THE RULES");
    str = await input();
    if (str != "NO")
        print_rules();
    while (1) {
        // *** Make a random list a(1) to a(n)
        a[1] = Math.floor((n - 1) * Math.random() + 2);
        for (k = 2; k <= n; k++) {
            do {
                a[k] = Math.floor(n * Math.random() + 1);
                for (j = 1; j <= k - 1; j++) {
                    if (a[k] == a[j])
                        break;
                }
            } while (j <= k - 1) ;
        }
        // *** Print original list and start game
        print("\n");
        print("HERE WE GO ... THE LIST IS:\n");
        t = 0;
        print_list();
        while (1) {
            while (1) {
                print("HOW MANY SHALL I REVERSE");
                r = parseInt(await input());
                if (r == 0)
                    break;
                if (r <= n)
                    break;
                print("OOPS! TOO MANY! I CAN REVERSE AT MOST " + n + "\n");
            }
            if (r == 0)
                break;
            t++;
            // *** Reverse r numbers and print new list
            for (k = 1; k <= Math.floor(r / 2); k++) {
                z = a[k];
                a[k] = a[r - k + 1];
                a[r - k + 1] = z;
            }
            print_list();
            // *** Check for a win
            for (k = 1; k <= n; k++) {
                if (a[k] != k)
                    break;
            }
            if (k > n) {
                print("YOU WON IT IN " + t + " MOVES!!!\n");
                print("\n");
                break;
            }
        }
        print("\n");
        print("TRY AGAIN (YES OR NO)");
        str = await input();
        if (str != "YES")
            break;
    }
    print("\n");
    print("O.K. HOPE YOU HAD FUN!!\n");
}

main();
