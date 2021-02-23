// WAR
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

var a = [, "S-2","H-2","C-2","D-2","S-3","H-3","C-3","D-3",
         "S-4","H-4","C-4","D-4","S-5","H-5","C-5","D-5",
         "S-6","H-6","C-6","D-6","S-7","H-7","C-7","D-7",
         "S-8","H-8","C-8","D-8","S-9","H-9","C-9","D-9",
         "S-10","H-10","C-10","D-10","S-J","H-J","C-J","D-J",
         "S-Q","H-Q","C-Q","D-Q","S-K","H-K","C-K","D-K",
         "S-A","H-A","C-A","D-A"];

var l = [];

// Main control section
async function main()
{
    print(tab(33) + "WAR\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS IS THE CARD GAME OF WAR.  EACH CARD IS GIVEN BY SUIT-#\n");
    print("AS S-7 FOR SPADE 7.  ");
    while (1) {
        print("DO YOU WANT DIRECTIONS");
        str = await input();
        if (str == "YES") {
            print("THE COMPUTER GIVES YOU AND IT A 'CARD'.  THE HIGHER CARD\n");
            print("(NUMERICALLY) WINS.  THE GAME ENDS WHEN YOU CHOOSE NOT TO\n");
            print("CONTINUE OR WHEN YOU HAVE FINISHED THE PACK.\n");
            break;
        }
        if (str == "NO")
            break;
        print("YES OR NO, PLEASE.  ");
    }
    print("\n");
    print("\n");
    
    a1 = 0;
    b1 = 0;
    p = 0;
    
    // Generate a random deck
    for (j = 1; j <= 52; j++) {
        do {
            l[j] = Math.floor(52 * Math.random()) + 1;
            for (k = 1; k < j; k++) {
                if (l[k] == l[j])   // Already in deck?
                    break;
            }
        } while (j != 1 && k < j) ;
    }
    l[j] = 0;   // Mark the end of the deck
    
    while (1) {
        m1 = l[++p];    // Take a card
        m2 = l[++p];    // Take a card
        print("\n");
        print("YOU: " + a[m1] + "\tCOMPUTER: " + a[m2] + "\n");
        n1 = Math.floor((m1 - 0.5) / 4);
        n2 = Math.floor((m2 - 0.5) / 4);
        if (n1 < n2) {
            a1++;
            print("THE COMPUTER WINS!!! YOU HAVE " + b1 + " AND THE COMPUTER HAS " + a1 + "\n");
        } else if (n1 > n2) {
            b1++;
            print("YOU WIN. YOU HAVE " + b1 + " AND THE COMPUTER HAS " + a1 + "\n");
        } else {
            print("TIE.  NO SCORE CHANGE.\n");
        }
        if (l[p + 1] == 0) {
            print("\n");
            print("\n");
            print("WE HAVE RUN OUT OF CARDS.  FINAL SCORE:  YOU: " + b1 + "  THE COMPUTER: " + a1 + "\n");
            print("\n");
            break;
        }
        while (1) {
            print("DO YOU WANT TO CONTINUE");
            str = await input();
            if (str == "YES")
                break;
            if (str == "NO")
                break;
            print("YES OR NO, PLEASE.  ");
        }
        if (str == "NO")
            break;
    }
    print("THANKS FOR PLAYING.  IT WAS FUN.\n");
    print("\n");
}

main();
