// RUSSIAN ROULETTE
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
    print(tab(28) + "RUSSIAN ROULETTE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS IS A GAME OF >>>>>>>>>>RUSSIAN ROULETTE.\n");
    restart = true;
    while (1) {
        if (restart) {
            restart = false;
            print("\n");
            print("HERE IS A REVOLVER.\n");
        }
        print("TYPE '1' TO SPIN CHAMBER AND PULL TRIGGER.\n");
        print("TYPE '2' TO GIVE UP.\n");
        print("GO");
        n = 0;
        while (1) {
            i = parseInt(await input());
            if (i == 2) {
                print("     CHICKEN!!!!!\n");
                break;
            }
            n++;
            if (Math.random() > 0.833333) {
                print("     BANG!!!!!   YOU'RE DEAD!\n");
                print("CONDOLENCES WILL BE SENT TO YOUR RELATIVES.\n");
                break;
            }
            if (n > 10) {
                print("YOU WIN!!!!!\n");
                print("LET SOMEONE ELSE BLOW HIS BRAINS OUT.\n");
                restart = true;
                break;
            }
            print("- CLICK -\n");
            print("\n");
        }
        print("\n");
        print("\n");
        print("\n");
        print("...NEXT VICTIM...\n");
    }
}

main();
