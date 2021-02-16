// ACEY DUCEY
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

print(tab(26) + "ACEY DUCEY CARD GAME\n");
print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
print("\n");
print("\n");
print("ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER\n");
print("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP\n");
print("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING\n");
print("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE\n");
print("A VALUE BETWEEN THE FIRST TWO.\n");
print("IF YOU DO NOT WANT TO BET, INPUT A 0\n");

function show_card(card)
{
    if (card < 11)
        print(card + "\n");
    else if (card == 11)
        print("JACK\n");
    else if (card == 12)
        print("QUEEN\n");
    else if (card == 13)
        print("KING\n");
    else
        print("ACE\n");
}

// Main program
async function main()
{
    q = 100;
    while (1) {
        print("YOU NOW HAVE " + q + " DOLLARS.\n");
        print("\n");
        
        do {
            print("HERE ARE YOUR NEXT TWO CARDS: \n");
            do {
                a = Math.floor(Math.random() * 13 + 2);
                b = Math.floor(Math.random() * 13 + 2);
            } while (a >= b) ;
            show_card(a);
            show_card(b);
            print("\n");
            while (1) {
                print("\n");
                print("WHAT IS YOUR BET");
                m = parseInt(await input());
                if (m > 0) {
                    if (m > q) {
                        print("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.\n");
                        print("YOU HAVE ONLY " + q + "DOLLARS TO BET.\n");
                        continue;
                    }
                    break;
                }
                m = 0;
                print("CHICKEN!!\n");
                print("\n");
                break;
            }
        } while (m == 0) ;
        c = Math.floor(Math.random() * 13 + 2);
        show_card(c);
        if (c > a && c < b) {
            print("YOU WIN!!!\n");
            q = q + m;
        } else {
            print("SORRY, YOU LOSE\n");
            if (m >= q) {
                print("\n");
                print("\n");
                print("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.\n");
                print("\n");
                print("\n");
                print("TRY AGAIN (YES OR NO)");
                a = await input();
                print("\n");
                print("\n");
                if (a == "YES") {
                    q = 100;
                } else {
                    print("O.K., HOPE YOU HAD FUN!");
                    break;
                }
            } else {
                q = q - m;
            }
        }
    }
}

main();
